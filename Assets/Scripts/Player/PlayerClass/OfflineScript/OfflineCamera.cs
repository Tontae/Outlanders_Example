using System.Collections;
using System.Collections.Generic;
using Outlander.Player;
using UnityEngine;

public class OfflineCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    protected GameObject m_camera;
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public OfflineMovement playermovement;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;
    //public GameObject topDownCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private void Awake()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        m_camera.GetComponent<Camera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // switch Camera styles
        // if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        // if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);



        // rotate orientation

        Vector3 viewDir = player.position - new Vector3(m_camera.transform.position.x, player.position.y, m_camera.transform.position.z);
        orientation.forward = viewDir.normalized;

        // roate player object
        if (currentStyle == CameraStyle.Basic)
        {
            playermovement.cameraStyle = PlayerCamera.CameraStyle.Basic.ToString();
            float horizontalInput = playermovement.movementInput.x;

            float verticalInput = playermovement.movementInput.y;
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            // Debug.Log(inputDir);
            if (!playermovement.climbing && !playermovement.restricted)
            {
                if (inputDir != Vector3.zero)
                    playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }

        }
        else if (currentStyle == CameraStyle.Combat)
        {
            playermovement.cameraStyle = PlayerCamera.CameraStyle.Combat.ToString();
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(m_camera.transform.position.x, combatLookAt.position.y, m_camera.transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        //topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        //if (newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);

        currentStyle = newStyle;
    }

    //Player Camera code (8/7) https://pastebin.com/Uy8WZs7N
}