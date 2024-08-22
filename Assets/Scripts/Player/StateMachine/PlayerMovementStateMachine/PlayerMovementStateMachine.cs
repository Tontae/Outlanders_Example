using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Outlander.Player
{
    [RequireComponent(typeof(Outlander.Manager.PlayerInputManager))]
    public class PlayerMovementStateMachine : PlayerElements
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

        private bool isStartSumDistance = false;
        private float distanceTravel = 0f;
        private Vector3 previousPosition = Vector3.zero;
        #endregion


        [Header("SurfaceLayer")]
        // [SerializeField] private LayerMask groundLayer;
        [SerializeField, ReadOnlyInspector] private LayerMask groundLayer;
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
        private PlayerBaseState currentState;
        private PlayerMovementStateData state;
        private PlayerMovementState movementStateIndex = PlayerMovementState.GROUNDED;
        private PlayerMovementState movementSubStateIndex = PlayerMovementState.IDLE;


        #region Properties
        // State
        public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }
        public PlayerMovementStateData State { get => state; set => state = value; }
        public PlayerMovementState MovementStateIndex { get => movementStateIndex; set => movementStateIndex = value; }
        public PlayerMovementState MovementSubStateIndex { get => movementSubStateIndex; set => movementSubStateIndex = value; }
        public string PlayerState { get => playerState; set => playerState = value; }

        // Camera
        public Camera m_Camera { get => Camera.main; }

        // Wall Layer
        public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }
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
        public bool IsDodge
        {
            get => onDodge;
            set
            {
                onDodge = value;
            }
        }
        public bool IsClimb
        {
            get => isClimb;
            set
            {
                isClimb = (Player.PlayerStamina.IsMin) ? false : value;
            }
        }
        public bool IsCrouch
        {
            get => onCrouch;
            set
            {
                onCrouch = value;
            }
        }
        public bool IsSwim
        {
            get
            {
                IsSwim = Physics.CheckSphere(transform.position + (Vector3.up * 1.364f), 0.17f, 1 << 4, QueryTriggerInteraction.Collide);
                /*if (isSwim)
                {
                    PlayerVelocity += new Vector3(0, 9.81f * Time.deltaTime, 0);
                    Player.CharacterController.Move(PlayerVelocity);
                }*/
                return isSwim;
            }
            set
            {
                if (isSwim == value) return;
                isSwim = value;
            }
        }
        public bool IsIdle { get => isIdle; set => isIdle = value; }
        public bool IsMoving { get => isMoving; set => isMoving = value; }
        public bool IsGround
        {
            get => isGround = GroundCheck();
            set => isGround = value;
        }
        public bool IsFall 
        { 
            get => isFall; 
            set => isFall = value;
        }
        public bool IsClimbToTop { get => isClimbToTop; set => isClimbToTop = value; }
        public bool IsStartSumDistance { get => isStartSumDistance; set => isStartSumDistance = value; }

        // Float
        public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
        public float Gravity { get => gravity; set => gravity = value; }
        public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
        public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }
        public float DodgeSpeed { get => dodgeSpeed; set => dodgeSpeed = value; }
        public float ClimbSpeed { get => climbSpeed; set => climbSpeed = value; }
        public float SwimSpeed { get => swimSpeed; set => swimSpeed = value; }
        public float JumpForce { get => jumpForce; set => jumpForce = value; }
        public float DistanceTravel { get => distanceTravel; set => distanceTravel = value; }

        // Cooldown
        // public float CurrentJumpCooldown { get => currentJumpCooldown; set { currentJumpCooldown = value; ResetJump(); } }
        #endregion


        #region Unity Event
        private void Start()
        {
            if (!isLocalPlayer) return;

            Gravity = Physics.gravity.y;

            InitMovementSpeed();

            State = new PlayerMovementStateData(this);
            CurrentState = State.Grounded();
            CurrentState.EnterState();
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            IdleCheck();
            CurrentState.UpdateState();
            StartSumDistance();
        }
        #endregion


        private void InitMovementSpeed()
        {
            WalkSpeed = playerMovementSO.walkSpeed;
            SprintSpeed = playerMovementSO.sprintSpeed;
            DodgeSpeed = playerMovementSO.dodgeSpeed;
            ClimbSpeed = playerMovementSO.climbSpeed;
            SwimSpeed = playerMovementSO.swimSpeed;
            JumpForce = playerMovementSO.jumpForce;

            GroundLayer = playerMovementSO.groundLayer;
            WallLayer = playerMovementSO.wallLayer;
        }

        public void UpdateMovementSpeed()
        {
            float _speed = Player.PlayerStatisticManager.GetFinalStat(StatusType.MOVSPD);
            if (Player.InventoryManager.TotalWeight >= Player.InventoryManager.MaxWeight * 1.2f)
                _speed *= 0.5f;
            else if (Player.InventoryManager.TotalWeight >= Player.InventoryManager.MaxWeight * 1.1f)
                _speed *= 0.7f;
            else if (Player.InventoryManager.TotalWeight >= Player.InventoryManager.MaxWeight)
                _speed *= 0.9f;

            CurrentSpeed = IsSprint ? _speed * 2f : _speed;

            WalkSpeed = _speed;
            SprintSpeed = _speed * 2f;
            DodgeSpeed = _speed * 2f;
            ClimbSpeed = _speed;
            SwimSpeed = _speed;
            JumpForce = -_speed;

            UIManagers.Instance.playerCanvas.movspdTxt.text = WalkSpeed.ToString("0.#");
        }

        public void ResetMovementBoolData()
        {
            IsGround = true;

            IsClimb = false;
            IsCrouch = false;
            IsDodge = false;
            IsJump = false;
            IsMoving = false;
            IsSwim = false;
        }

        private void StartSumDistance()
        {
            if (IsStartSumDistance)
            {
                Vector3 currentPosition = transform.position;
                currentPosition.y = 0f;
                float curDistance = Vector3.Distance(previousPosition, currentPosition);
                if (curDistance >= 10f)
                    return;
                DistanceTravel += curDistance;
                previousPosition = currentPosition;
                Debug.Log($"[Server] Player travel : {DistanceTravel}m.(+{curDistance})");
            }
        }

        #region Bool Helper
        public bool GroundCheck()
        {

            //Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            /*IsGround = Physics.CheckSphere(
                transform.position,
                Player.CharacterController.radius,
                GroundLayer,
                QueryTriggerInteraction.Ignore
            );*/

            return Physics.CheckSphere(
                transform.position,
                Player.CharacterController.radius * 0.5f,
                GroundLayer,
                QueryTriggerInteraction.Ignore
            );
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


        #region Input Event
        public void MoveInput(Vector2 value)
        {
            MovementInput = value;

            IsMoving = (MovementInput != Vector2.zero) ? true : false;
        }

        public void LookInput(Vector2 value)
        {
            LookDir = value;
        }

        public void JumpInput(bool value)
        {
            if (Player.PlayerStamina.Stamina < 20f) return;
            if (MovementStateIndex == PlayerMovementState.DODGE) return;
            if (MovementStateIndex == PlayerMovementState.FALL) return;
            if (MovementStateIndex == PlayerMovementState.SWIM) return;
            if (Player.OutlanderStateMachine.OnWeaponAction) return;
            if (Player.OutlanderStateMachine.OnSkill) return;
            // if (CurrentJumpCooldown <= 0)
            IsJump = value;
        }

        public void SprintInput(bool value)
        {
            if (Player.PlayerStamina.IsMin
                || MovementStateIndex == PlayerMovementState.FALL
                || MovementStateIndex == PlayerMovementState.JUMP
                || Player.OutlanderStateMachine.OnSkill)
            {
                IsSprint = false;
                return;
            }

            IsSprint = value;
        }

        public void CrouchInput()
        {
            if (isServer) return;
            if (MovementStateIndex == PlayerMovementState.DODGE) return;
            if (IsMoving) return;
            if (Player.OutlanderStateMachine.OnWeaponAction) return;
            if (Player.OutlanderStateMachine.OnSkill) return;
            IsCrouch = !IsCrouch;
        }

        public void DodgeInput(bool value)
        {
            if (Player.PlayerStamina.Stamina < 20f) return;
            if (!IsGround) return;
            if (MovementStateIndex == PlayerMovementState.DODGE) return;
            if (!IsMoving) return;
            if (Player.OutlanderStateMachine.OnWeaponAction) return;
            if (Player.OutlanderStateMachine.OnSkill) return;

            IsDodge = value;
        }
        private bool isWeaponVisible = false;

        [Command]
        private void CmdSetWeaponVisible(bool value)
        {
            RpcSetWeaponVisible(value);
        }

        [ClientRpc]
        private void RpcSetWeaponVisible(bool value)
        {
            //IsCrouch = value;
            Player.PlayerCustume.mainWeapon.SetActive(!value);
        }

        public void SetWeaponVisible()
        {
            bool value = MovementStateIndex == PlayerMovementState.CLIMB
                || MovementStateIndex == PlayerMovementState.CROUCH
                || MovementStateIndex == PlayerMovementState.DODGE
                || MovementStateIndex == PlayerMovementState.SWIM;
            //if (isWeaponVisible == value) return;
            isWeaponVisible = value;
            if (Player.WeaponManager.currentWeaponType == WeaponManager.WeaponType.None) return;
            if (!Player.PlayerCustume.mainWeapon) return;
            if (Player.PlayerCustume.mainWeapon.activeInHierarchy != value) return;
            Player.PlayerCustume.mainWeapon.SetActive(!value);
            CmdSetWeaponVisible(value);
        }
        #endregion

        #region Gizmos
        public float radiusToDebug = 0.25f;
        /*private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3.up * Player.CharacterController.height) + transform.forward, radiusToDebug);
            Gizmos.DrawRay(transform.position + (Vector3.up * Player.CharacterController.height), (transform.forward - transform.up).normalized);
        }*/

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(spherePosition, Player.CharacterController.radius);
        }
#endif
        #endregion
    }

}

