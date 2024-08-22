using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockCameraBehaviour : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    private CinemachineFreeLook cinemachineFreeLook;
    // private string m_XAxis;
    // private string m_YAxis;

    private CinemachineInputProvider cinemachineInputProvider;

    // [SerializeField] private GameObject playerObjRef;
    // private Outlander.Player.PlayerOutlanderMovement playerOutlanderMovement;

    // private Vector2 lookDir;

    private void OnEnable()
    {
        playerInputAction.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Disable();
    }

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        cinemachineInputProvider = GetComponent<CinemachineInputProvider>();
        // m_XAxis = cinemachineFreeLook.m_XAxis.m_InputAxisName;
        // m_YAxis = cinemachineFreeLook.m_YAxis.m_InputAxisName;

        // Application.focusChanged += CheckFocus;

        // playerOutlanderMovement = playerObjRef.GetComponent<Outlander.Player.PlayerOutlanderMovement>();
    }

    private void Update()
    {
        // Debug.Log($"{cinemachineFreeLook.m_XAxis.m_MaxSpeed} : {cinemachineFreeLook.m_YAxis.m_MaxSpeed}");
        LockCursor();
        // Debug.Log(Application.isFocused);
        // if (Application.isFocused && Keyboard.current.anyKey.wasPressedThisFrame)
        // {
        //     cinemachineInputProvider.enabled = true;
        //     return;
        // }
    }

    private void LockCursor()
    {
        // if (Cursor.visible)
        // {
        // cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        // cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
        // cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0;
        // cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0;
        // cinemachineInputProvider.enabled = false;
        // return;
        // }

        if (Cursor.visible) cinemachineInputProvider.XYAxis.action.Disable();
        else cinemachineInputProvider.XYAxis.action.Enable();


        // cinemachineInputProvider.enabled = true;

        // cinemachineFreeLook.m_XAxis.m_InputAxisName = m_XAxis;
        // cinemachineFreeLook.m_YAxis.m_InputAxisName = m_YAxis;
        // cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0;
        // cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0;
    }

    // private void CheckFocus(bool focus)
    // {

    // }

    // private void OnApplicationFocus(bool focusStatus)
    // {
    //     // Debug.Log($"Focus : {focusStatus}");
    //     Cursor.visible = false;

    //     cinemachineInputProvider.enabled = true;

    //     // cinemachineFreeLook.m_XAxis.m_InputAxisName = m_XAxis;
    //     // cinemachineFreeLook.m_YAxis.m_InputAxisName = m_YAxis;
    // }
}
