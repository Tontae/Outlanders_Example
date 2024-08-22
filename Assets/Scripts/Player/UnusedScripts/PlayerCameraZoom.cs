// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Cinemachine;

// public class PlayerCameraZoom : MonoBehaviour
// {
//     private CinemachineInputProvider _cInput;
//     private CinemachineVirtualCamera _vcam;

//     [SerializeField] private float zoomSpeed = 2;
//     [SerializeField] private float minFOV;
//     [SerializeField] private float maxFOV;

//     void Awake()
//     {
//         _cInput = GetComponent<CinemachineInputProvider>();
//         _vcam = GetComponent<CinemachineVirtualCamera>();
//         // _vcam.m_Lens.FieldOfView = maxFOV;

//         _vcam.m_Lens.FieldOfView = 50;
//     }

//     void Update()
//     {
//         float x = _cInput.GetAxisValue(0);
//         float y = _cInput.GetAxisValue(1);
//         float z = _cInput.GetAxisValue(2);

//         var view = Mouse.current.position.ReadValue();
//         var isInsideScreen = view.x < 0 || view.x > Screen.width - 1 || view.y < 0 || view.y > Screen.height - 1;

//         if (z != 0 && !isInsideScreen)
//         {
//             ZoomScreen(z);
//         }
//     }

//     void ZoomScreen(float increment)
//     {
//         float fov = _vcam.m_Lens.FieldOfView;
//         float target = Mathf.Clamp(fov + increment, maxFOV + 5, minFOV - 5);

//         _vcam.m_Lens.FieldOfView = Mathf.Lerp(fov, target, zoomSpeed);

//         if (_vcam.m_Lens.FieldOfView < minFOV) _vcam.m_Lens.FieldOfView = minFOV;
//         if (_vcam.m_Lens.FieldOfView > maxFOV) _vcam.m_Lens.FieldOfView = maxFOV;
//     }
// }
