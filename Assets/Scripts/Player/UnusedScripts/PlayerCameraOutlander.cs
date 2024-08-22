// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Mirror;
// using Cinemachine;
// using UnityEngine.InputSystem;

// namespace Outlander.Player
// {
//     [RequireComponent(typeof(PlayerOutlander))]
//     public class PlayerCameraOutlander : NetworkBehaviour
//     {
//         // [SerializeField] private float rotationSpeed = 1f;
//         // [SerializeField] private float turnSpeed = 10f;
//         [Header("Player Transform")]
//         [SerializeField] private Transform player;


//         [Header("Rotate Camera")]
//         [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
//         private float rotationVelocity;
//         private float RotationSmoothTime = 0.12f;
//         private Vector2 mouseDelta;
//         private PlayerInputAction inputActions;
//         // private const float _threshold = 0.01f;
//         private float _cinemachineTargetYaw;
//         private float _cinemachineTargetPitch;
//         [SerializeField] private GameObject CinemachineCameraTarget;
//         public float TopClamp = 70.0f;
//         public float BottomClamp = -30.0f;

//         [Header("MinimapCamera")]
//         [SerializeField] private Camera minimapCamera;

//         private bool IsCurrentDeviceMouse
//         {
//             get
//             {
// #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
//                         return _playerInput.currentControlScheme == "KeyboardMouse";
// #else
//                 return false;
// #endif
//             }
//         }
//         [SerializeField] private float sesitivityX;
//         [SerializeField] private float sesitivityY;
//         // [SerializeField] private bool isReady;
//         private float readyTimer = 1f;

//         // [Header("Zoom Module")]
//         // [SerializeField] private float zoomSpeed = 2;
//         // [SerializeField] private float minFOV = 50;
//         // [SerializeField] private float maxFOV = 70;
//         // private CinemachineInputProvider _cInput;

//         private float mouseX, mouseY;
//         private PlayerOutlander playerController;
//         private GameObject mainCamera;
//         // private CinemachineTransposer transposer;

//         public static bool enableMouse = false;

//         private void Awake()
//         {
//             playerController = player.GetComponentInParent<PlayerOutlander>();
//             // _cInput = GetComponent<CinemachineInputProvider>();
//             // virtualCamera = GetComponent<CinemachineVirtualCamera>();

//             inputActions = new PlayerInputAction();
//             if (mainCamera == null)
//             {
//                 mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
//             }
//         }

//         private void Start()
//         {
//             if (!hasAuthority) { return; }

//             _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

//             // transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
//             if (isLocalPlayer)
//             {
//                 virtualCamera.gameObject.SetActive(true); //check player camera
//                 minimapCamera.gameObject.SetActive(true);
//             }

//         }

//         private void Update()
//         {
//             if (!PlayerManager.instance.isPlayerDead)
//                 ToggleMouse();
//         }

//         private void ToggleMouse()
//         {
//             Cursor.visible = enableMouse;
//             Cursor.lockState = (Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;
//         }

//         private void LateUpdate()
//         {
//             if (!hasAuthority) { return; }

//             // if (isLocalPlayer)
//             CameraRotation();
//         }

//         private bool IsReady()
//         {
//             if (readyTimer > 0) readyTimer -= Time.deltaTime;
//             return (readyTimer <= 0) ? true : false;
//         }

//         // [Client]
//         // private void CameraControl()
//         // {
//         //     // mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
//         //     // mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
//         //     mouseX += mouseDelta.x * rotationSpeed;
//         //     mouseY -= mouseDelta.y * rotationSpeed;
//         //     mouseY = Mathf.Clamp(mouseY, -35, 60);


//         //     Quaternion playerTurnAngle = Quaternion.Euler(0, mouseX, 0);
//         //     player.rotation = Quaternion.Lerp(player.rotation, playerTurnAngle, Time.deltaTime * turnSpeed);
//         // }

//         [Client]
//         private void CameraRotation()
//         {
//             if (IsReady() && !Cursor.visible)
//             {
//                 // if there is an input and camera position is not fixed
//                 float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

//                 _cinemachineTargetYaw += mouseDelta.x * deltaTimeMultiplier * sesitivityX;
//                 _cinemachineTargetPitch -= mouseDelta.y * deltaTimeMultiplier * sesitivityY;

//                 // clamp our rotations so our values are limited 360 degrees
//                 _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
//                 _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

//                 // Cinemachine will follow this target
//                 CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);

//                 Vector3 movementInput = playerController.movementInput;
//                 Vector3 lookDir = playerController.lookDir;

//                 if (movementInput != Vector3.zero)
//                 {
//                     float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, mainCamera.transform.eulerAngles.y, ref rotationVelocity, RotationSmoothTime);

//                     // // rotate to face input direction relative to camera position
//                     if (movementInput != Vector3.zero)
//                     {
//                         transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
//                     }
//                 }
//             }
//         }

//         [Client]
//         private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
//         {
//             if (lfAngle < -360f) lfAngle += 360f;
//             if (lfAngle > 360f) lfAngle -= 360f;
//             return Mathf.Clamp(lfAngle, lfMin, lfMax);
//         }

//         [Client]
//         private void OnLook(InputValue value)
//         {
//             mouseDelta = value.Get<Vector2>();
//             // Debug.Log(mouseDelta);
//         }

//         [Client]
//         public void OnEnableMouse(InputValue value)
//         {
//             enableMouse = !enableMouse;
//         }
//     }
// }

