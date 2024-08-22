using System.Collections;
using System.Collections.Generic;
using Outlander.Player;
using UnityEngine;
using Mirror;
using Cinemachine;

namespace Outlander.Player
{
    public class PlayerCamera : PlayerElements
    {
        [Header("References")]
        private new GameObject camera;
        public Transform orientation;

        // private PlayerInputAction playerInputAction;

        [SerializeField, Range(1, 100)] private float rotationSpeed;
        [SerializeField, Range(1, 100)] private float xSensitivity = 5;
        public float XSensitivity
        {
            get => xSensitivity;
            set
            {
                xSensitivity = value;
                SetCameraSensitivity(currentCamera);
            }
        }

        [SerializeField, Range(1, 100)] private float ySensitivity = 5;
        public float YSensitivity
        {
            get => ySensitivity;
            set
            {
                ySensitivity = value;
                SetCameraSensitivity(currentCamera);
            }
        }

        //[SerializeField, Range(1, 100)] private float xAimingSensitivity = 5;
        //[SerializeField, Range(1, 100)] private float yAimingSensitivity = 5;


        private float currectXSensitivity;
        public float CurrectXSensitivity
        {
            get => currectXSensitivity;
            set => currectXSensitivity = value;
        }

        private float currectYSensitivity;
        public float CurrectYSensitivity
        {
            get => currectYSensitivity;
            set => currectYSensitivity = value;
        }

        public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
        public GameObject Camera { get => camera; set => camera = value; }

        public enum SensitivityState
        {
            Increase,
            Decrease
        }
        // private SensitivityState sensState;

        public Transform combatLookAt;

        public CinemachineFreeLook thirdPersonCam;
        public CinemachineFreeLook combatCam;
        private CinemachineFreeLook currentCamera;

        public CameraStyle currentStyle;
        public enum CameraStyle
        {
            Basic,
            Combat
        }

        private void Awake()
        {
            // playerInputAction = new PlayerInputAction();

            Camera = GameObject.FindGameObjectWithTag("MainCamera");
            Camera.GetComponent<Camera>();

            switch (currentStyle)
            {
                case CameraStyle.Basic:
                    currentCamera = thirdPersonCam;
                    CurrectXSensitivity = currentCamera.m_XAxis.m_MaxSpeed;
                    CurrectYSensitivity = currentCamera.m_YAxis.m_MaxSpeed;
                    break;
                case CameraStyle.Combat:
                    currentCamera = combatCam;
                    CurrectXSensitivity = currentCamera.m_XAxis.m_MaxSpeed;
                    CurrectYSensitivity = currentCamera.m_YAxis.m_MaxSpeed;
                    break;
            }

            // currectXSensitivity = thirdPersonCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed;
            // currectYSensitivity = thirdPersonCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed;

            for (int i = 0; i < 3; i++)
            {
                thirdPersonCam.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XDamping = 0;
                thirdPersonCam.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_YDamping = 0;
                thirdPersonCam.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_ZDamping = 0;

                combatCam.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XDamping = 0;
                combatCam.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_YDamping = 0;
                combatCam.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>().m_ZDamping = 0;
            }
        }

        private void Start()
        {
            // playeroutlander = GetComponent<PlayerOutlander>();
            // playerStateMachine = GetComponent<PlayerStatMachine>();

            if (!isLocalPlayer)
            {
                thirdPersonCam.gameObject.SetActive(false);
                combatCam.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if (isLocalPlayer)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // playerInputAction.Enable();
            }

        }
        private void OnDisable()
        {
            if (isLocalPlayer)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // playerInputAction.Disable();
            }
        }

        // private void Update()
        // {
        // if (!isLocalPlayer) return;
        // Debug.Log(playerStateMachine.MovementInput);
        // CalculateSensitivity();
        // }

        void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            // switch Camera styles
            OnAimingSwitchCamera();

            // rotate orientation
            Vector3 viewDir = transform.position - new Vector3(Camera.transform.position.x, transform.position.y, Camera.transform.position.z);
            orientation.forward = viewDir.normalized;

            if (Player.OutlanderStateMachine.OnWeaponAction) return;
            // // roate player object
            if (currentStyle == CameraStyle.Basic)
            {
                if (!Player.MovementStateMachine.IsClimb)
                {
                    Vector2 movement = Player.MovementStateMachine.MovementInput;

                    if (movement != Vector2.zero)
                    {
                        float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + UnityEngine.Camera.main.transform.eulerAngles.y;
                        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
                        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                    }
                }
                else
                {
                    Vector2 movement = Player.MovementStateMachine.MovementInput;

                    if (movement != Vector2.zero)
                    {
                        // Debug.Log("TEST");
                        return;
                    }
                    // float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_camera.transform.eulerAngles.y, ref rotationSpeed, 0.12f);

                    // // // rotate to face input direction relative to camera position
                    // if (playerStateMachine.MovementInput != Vector2.zero)
                    // {
                    //     transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    // }
                }

            }

            else if (currentStyle == CameraStyle.Combat)
            {
                // Vector3 dirToCombatLookAt = combatLookAt.position - m_camera.transform.position;
                // orientation.forward = dirToCombatLookAt.normalized;
                // playerObj.forward = dirToCombatLookAt.normalized;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, Camera.transform.eulerAngles.y, ref rotationSpeed, 0.12f);

                // // rotate to face input direction relative to camera position
                if (Player.MovementStateMachine.MovementInput != Vector2.zero)
                {
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }
        }

        private void OnAimingSwitchCamera()
        {
            if (Player.OutlanderStateMachine.IsAiming)
            {
                if (currentStyle != CameraStyle.Combat)
                {
                    currentCamera = combatCam.GetComponent<CinemachineFreeLook>();
                    // Debug.Log(currentCamera.name);

                    CinemachineFreeLook tmpCamera = thirdPersonCam.GetComponent<CinemachineFreeLook>();

                    currentCamera.m_XAxis.Value = tmpCamera.m_XAxis.Value;
                    currentCamera.m_YAxis.Value = tmpCamera.m_YAxis.Value/* + 0.4f*/;

                    SwitchCameraStyle(CameraStyle.Combat);
                }
            }
            else
            {
                if (currentStyle != CameraStyle.Basic)
                {
                    currentCamera = thirdPersonCam.GetComponent<CinemachineFreeLook>();
                    // Debug.Log(currentCamera.name);

                    CinemachineFreeLook tmpCamera = combatCam.GetComponent<CinemachineFreeLook>();

                    currentCamera.m_XAxis.Value = tmpCamera.m_XAxis.Value;
                    currentCamera.m_YAxis.Value = tmpCamera.m_YAxis.Value/* - 0.4f*/;

                    SwitchCameraStyle(CameraStyle.Basic);
                }
            }
        }

        private void SwitchCameraStyle(CameraStyle newStyle)
        {
            switch (newStyle)
            {
                case CameraStyle.Basic:
                    thirdPersonCam.GetComponent<CinemachineFreeLook>().m_Priority = 1;
                    combatCam.GetComponent<CinemachineFreeLook>().m_Priority = 0;
                    break;
                case CameraStyle.Combat:
                    thirdPersonCam.GetComponent<CinemachineFreeLook>().m_Priority = 0;
                    combatCam.GetComponent<CinemachineFreeLook>().m_Priority = 1;
                    break;
            }

            currentStyle = newStyle;
        }

        private void SetCameraSensitivity(CinemachineFreeLook _camera)
        {
            _camera.m_XAxis.m_MaxSpeed = CurrectXSensitivity * xSensitivity;
            _camera.m_YAxis.m_MaxSpeed = CurrectYSensitivity * ySensitivity;
        }

        // public void CalculateSensitivity(/*SensitivityState _state*/)
        // {
        //     if (GetBoolIncreaseSensitivity())
        //     {
        //         XSensitivity += 0.1f;
        //         YSensitivity += 0.1f;
        //     }

        //     if (GetBoolDecreaseSensitivity())
        //     {
        //         XSensitivity -= 0.1f;
        //         YSensitivity -= 0.1f;
        //     }

        //     // Debug.Log($"{XSensitivity} : {YSensitivity}");
        // }

        // private bool GetBoolIncreaseSensitivity() => Player.PlayerInputManager.PlayerInput.Camera.IncreaseSensitivity.triggered;

        // private bool GetBoolDecreaseSensitivity() => Player.PlayerInputManager.PlayerInput.Camera.DecreaseSensitivity.triggered;

        public void GetIncreaseSensitivity()
        {
            XSensitivity += 0.1f;
            YSensitivity += 0.1f;
        }

        public void GetDecreaseSensitivity()
        {
            XSensitivity -= 0.1f;
            YSensitivity -= 0.1f;
        }

        //Player Camera code (8/7) https://pastebin.com/Uy8WZs7N
    }
}
