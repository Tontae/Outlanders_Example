using Cinemachine;
using UnityEngine;

public class PlayerCameraPOV : CinemachineExtension
{
    public Outlander.Player.MainPlayerController playerCam;
    private Vector3 startRotation;

    [SerializeField] private GameObject player;

    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAngle = 80f;

    protected override void Awake()
    {
        playerCam.GetComponent<Outlander.Player.MainPlayerController>();
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // throw new System.NotImplementedException();
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startRotation == null)
                    startRotation = transform.localRotation.eulerAngles;

                Vector2 deltaInput = playerCam.LookDir;
                startRotation.x += deltaInput.x * Time.deltaTime * verticalSpeed;
                startRotation.y += deltaInput.y * Time.deltaTime * horizontalSpeed;

                startRotation.y = Mathf.Clamp(startRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startRotation.y, startRotation.x, 0);
            }
        }
    }
}
