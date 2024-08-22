using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

namespace Outlander.Player
{
    //[RequireComponent(typeof(Outlander.Manager.PlayerInputManager))]
    public class BotPSM : PlayerElements
    {
        [Header("Handler")]
        [SerializeField, ReadOnlyInspector] private string playerState;
        [SerializeField, Space(10)] private PlayerMovementSO playerMovementSO;


        #region Movement Controller
        [Header("Controller")]
        private bool isGround;
        private bool isIdle = true;
        private bool isMoving;
        private bool isClimb;
        private bool isSwim;
        private bool isFall = false;
        private bool isClimbToTop;

        private CharacterController characterController;


        [Header("Movement")]
        [SerializeField, ReadOnlyInspector] private float gravity;
        [SerializeField, ReadOnlyInspector] private float currentSpeed;
        private Vector3 playerVelocity;
        private float walkSpeed;
        private float sprintSpeed;
        private float dodgeSpeed;
        private float climbSpeed;
        private float swimSpeed;
        private float jumpForce;
        #endregion


        [Header("SurfaceLayer")]
        // [SerializeField] private LayerMask groundLayer;
        [SerializeField, ReadOnlyInspector] private LayerMask wallLayer;
        [SerializeField, ReadOnlyInspector] private Transform wallHelper;
        private RaycastHit wallHit;


        #region Input
        [Header("InputSystem")]
        private bool onJump = false;
        private bool onSprint = false;
        private bool onDodge = false;
        private bool onCrouch = false;
        private Vector2 movementInput;
        private Vector2 lookDir;

        // timeout
        // private float currentJumpCooldown;
        #endregion


        // StateMachine
        private BotBaseState currentState;
        private BotMovementStateData state;
        // private PlayerMovementState movementState;


        #region Properties
        // State
        public BotBaseState CurrentState { get => currentState; set => currentState = value; }
        public BotMovementStateData State { get => state; set => state = value; }
        // public PlayerMovementState MovementState { get => movementState; set => movementState = value; }
        public string PlayerState { get => playerState; set => playerState = value; }

        // Camera
        public Camera m_Camera { get => Camera.main; }

        // Wall Layer
        public LayerMask WallLayer { get => wallLayer; set => wallLayer = value; }
        public RaycastHit WallHit { get => wallHit; set => wallHit = value; }
        public Transform WallHelper
        {
            get
            {
                if (wallHelper == null)
                {
                    wallHelper = new GameObject().transform;
                    wallHelper.hideFlags = HideFlags.HideInHierarchy;
                    return wallHelper;
                }
                return wallHelper;
            }
            set => wallHelper = value;
        }

        // Vector
        public Vector2 MovementInput { get => movementInput; set => movementInput = value; }
        public Vector2 LookDir { get => lookDir; set => lookDir = value; }
        public Vector3 PlayerVelocity { get => playerVelocity; set => playerVelocity = value; }

        // Bool
        public bool IsSprint { get => onSprint; set => onSprint = value; }
        public bool IsJump { get => onJump; set => onJump = value; }
        public bool IsDodge { get => onDodge; set => onDodge = value; }
        public bool IsClimb { get => isClimb; set => isClimb = (Player.PlayerStamina.IsMin) ? false : value; }
        public bool IsCrouch
        {
            get => onCrouch;
            set
            {
                onCrouch = value;
                // if (Player.WeaponManager.currentWeaponType == WeaponManager.WeaponType.None) return;
                // Player.PlayerAppearance.rightWeaponGO.SetActive(!onCrouch);
                // Player.PlayerAppearance.leftWeaponGO.SetActive(!onCrouch);
                // CmdSetCrouch(onCrouch);
            }
        }
        public bool IsSwim { get => isSwim; set => isSwim = value; }
        public bool IsIdle { get => isIdle; set => isIdle = value; }
        public bool IsMoving { get => isMoving; set => isMoving = value; }
        public bool IsGround { get => isGround; set => isGround = value; }
        public bool IsFall { get => isFall; set => isFall = value; }

        // Controller
        public CharacterController CharacterController { get => characterController; set => characterController = value; }

        // Float
        public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
        public float Gravity { get => gravity; set => gravity = value; }
        public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
        public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }
        public float DodgeSpeed { get => dodgeSpeed; set => dodgeSpeed = value; }
        public float ClimbSpeed { get => climbSpeed; set => climbSpeed = value; }
        public float SwimSpeed { get => swimSpeed; set => swimSpeed = value; }
        public float JumpForce { get => jumpForce; set => jumpForce = value; }
        public bool IsClimbToTop { get => isClimbToTop; set => isClimbToTop = value; }

        // Cooldown
        // public float CurrentJumpCooldown { get => currentJumpCooldown; set { currentJumpCooldown = value; ResetJump(); } }
        #endregion


        #region Unity Event
        private void Awake()
        {
            if (!NetworkServer.active) return;
            CharacterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            if (!NetworkServer.active) return;
            // if (!isLocalPlayer) return;

            Gravity = Physics.gravity.y;

            InitMovementSpeed();

            //State = new BotMovementStateData(this);
            //CurrentState = State.Grounded();
            //CurrentState.EnterState();
        }

        //private void FixedUpdate()
        //{
        //    // if (!isLocalPlayer) return;
        //    IdleCheck();

        //    CurrentState.UpdateState();
        //}
        #endregion


        private void InitMovementSpeed()
        {
            if (!NetworkServer.active) return;
            WalkSpeed = playerMovementSO.walkSpeed;
            SprintSpeed = playerMovementSO.sprintSpeed;
            DodgeSpeed = playerMovementSO.dodgeSpeed;
            ClimbSpeed = playerMovementSO.climbSpeed;
            SwimSpeed = playerMovementSO.swimSpeed;
            JumpForce = playerMovementSO.jumpForce;

            WallLayer = playerMovementSO.wallLayer;
        }

        public void SetMovementSpeed(float _speed)
        {
            // WalkSpeed = _speed;
            // SprintSpeed = _speed * 2;
            // DodgeSpeed = _speed * 2;
            // ClimbSpeed = _speed;
            // SwimSpeed = _speed;
        }

        public void ResetMovementBoolData()
        {
            if (!NetworkServer.active) return;
            IsGround = true;

            IsClimb = false;
            IsCrouch = false;
            IsDodge = false;
            IsJump = false;
            IsMoving = false;
            IsSwim = false;
        }


        #region Bool Helper
        public void GroundCheck()
        {
            if (!NetworkServer.active) return;
            IsGround = Player.CharacterController.isGrounded;

            if ((IsGround || IsClimb) && PlayerVelocity.y <= 0)
                PlayerVelocity = new Vector3(PlayerVelocity.x, 0, PlayerVelocity.z);

            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100);
            if (hit.distance >= 0.5)
            {
                IsFall = (IsJump || IsClimb || IsGround) ? false : true;
            }
            else
            {
                IsFall = false;
            }
        }

        private bool IdleCheck()
        {
            return IsIdle = (IsFall || isMoving) ? false : true;
        }

        // private void WallDectection()
        // {
        //     if (Physics.SphereCast(transform.position + (Vector3.up * Player.CharacterController.height), 0.2f, transform.forward, out RaycastHit tophitted, 1.5f, wallLayer))
        //     {
        //         if (Physics.SphereCast(transform.position + (Vector3.up * Player.CharacterController.height * 0.5f), 0.2f, transform.forward, out wallHit, 0.5f, wallLayer))
        //         {
        //             if (MovementInput.y <= 0) return;

        //             IsClimb = true;

        //             WallHelper = (WallHelper == null) ? new GameObject().transform : WallHelper;
        //             WallHelper.hideFlags = HideFlags.HideInHierarchy;
        //         }
        //     }
        //     else if (IsClimb && IsJump)
        //     {
        //         IsClimb = false;
        //         IsGround = true;
        //         return;
        //     }
        // }

        // public void ResetJump()
        // {
        //     if (CurrentJumpCooldown > 0)
        //     {
        //         CurrentJumpCooldown -= Time.deltaTime;
        //         IsJump = false;
        //     }
        // }
        #endregion


        // #region Input Event
        // public void MoveInput(Vector2 value)
        // {
        //     MovementInput = value;

        //     IsMoving = (MovementInput != Vector2.zero) ? true : false;
        // }

        // public void LookInput(Vector2 value)
        // {
        //     LookDir = value;
        // }

        // public void JumpInput(bool value)
        // {
        //     if (Player.PlayerStamina.IsMin) return;
        //     if (IsDodge) return;
        //     if (IsFall) return;

        //     // if (CurrentJumpCooldown <= 0)
        //     IsJump = value;
        // }

        // public void SprintInput(bool value)
        // {
        //     if (Player.PlayerStamina.IsMin)
        //     {
        //         IsSprint = false;
        //         return;
        //     }

        //     if (!IsGround)
        //     {
        //         IsSprint = false;
        //         return;
        //     }

        //     IsSprint = value;
        // }

        // public void CrouchInput()
        // {
        //     // if (isServer) return;
        //     if (IsDodge) return;
        //     if (IsMoving) return;
        //     IsCrouch = !IsCrouch;
        // }

        // public void DodgeInput(bool value)
        // {
        //     if (Player.PlayerStamina.IsMin) return;
        //     if (!IsGround) return;
        //     if (IsDodge) return;
        //     if (!IsMoving) return;

        //     IsDodge = value;
        // }

        // [Command]
        // private void CmdSetCrouch(bool value)
        // {
        //     RpcSetCrouch(value);
        // }

        // [ClientRpc]
        // public void RpcSetCrouch(bool value)
        // {
        //     //IsCrouch = value;
        //     // Player.PlayerAppearance.rightWeaponGO.SetActive(!value);
        //     // Player.PlayerAppearance.leftWeaponGO.SetActive(!value);
        // }
        // #endregion

        #region Gizmos
        public float radiusToDebug = 0.25f;
        /*private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3.up * Player.CharacterController.height) + transform.forward, radiusToDebug);
            Gizmos.DrawRay(transform.position + (Vector3.up * Player.CharacterController.height), (transform.forward - transform.up).normalized);
        }*/
        #endregion
    }

}

