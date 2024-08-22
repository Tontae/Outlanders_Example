using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSO", menuName = "ScriptableObjects/PlayerMovementSO", order = 0)]
public class PlayerMovementSO : ScriptableObject
{
    public float walkSpeed;
    public float sprintSpeed;
    public float dodgeSpeed;
    public float climbSpeed;
    public float swimSpeed;
    public float jumpForce;

    [Space(10)]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
}