using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Outlander.Player
{
    public class MainPlayerController : NetworkBehaviour
    {
        [Header("Handler")]
        private GameObject playerObjectRef;
        private Rigidbody playerRigidbody;
        private PlayerCamera playerCamera;


        [Header("Movement")]
        private CharacterController controller;
        private bool isGround;
        private Vector3 playerVelocity;
        [SerializeField] private float gravity;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float dodgeSpeed;
        [SerializeField] private float climbSpeed;
        [SerializeField] private float swimSpeed;
        [SerializeField] private float jumpForce;


        [Header("Stamina")]
        [SerializeField] private float stamina;
        private float minStamina;
        private float maxStamina;
        private bool reachMinStamina;
        private bool reachMaxStamina;
        private bool rechargeStamina;

        [Header("Climbing")]
        [SerializeField] private LayerMask wallLayer;

        [Header("InputSystem")]
        private bool onJump;
        private bool onSprint;
        private bool onDodge;
        private bool onClimb;
        private bool isSwim;
        private Vector2 movementInput;
        private Vector2 lookDir;

        private PlayerState playerState;
        public enum PlayerState
        {
            Idle,
            Walk,
            Sprint,
            Jump,
            Dodge,
            Climb,
            Swim
        }

        public Vector2 MovementInput { get => movementInput; set => movementInput = value; }
        private float CurrentSpeed
        {
            get => currentSpeed;
            set => currentSpeed = value;
            // {
            //     currentSpeed = value;
            //     OnPlayerSpeedChange();
            // }
        }

        public float Gravity { get => gravity; set => gravity = value; }
        public bool IsSwim { get => isSwim; set => isSwim = value; }
        public Vector2 LookDir { get => lookDir; set => lookDir = value; }
        public PlayerState MovementState
        {
            get => playerState;
            set
            {
                playerState = value;
                OnPlayerSpeedChange();
            }
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void OnDrawGizmos()
        {
            /*            Gizmos.DrawWireSphere(transform.position + (Vector3.up * controller.height * 0.5f), 0.2f);
                        Gizmos.DrawWireSphere(transform.position + (Vector3.up * controller.height), 0.2f);
            */
            // RaycastHit hit;
            if (Physics.SphereCast(transform.position + (Vector3.up * controller.height * 0.5f), 0.2f, transform.forward, out RaycastHit hit, 0.5f, wallLayer))
            {
                Gizmos.color = Color.green;
                Vector3 sphereCastMidpoint = transform.position + (Vector3.up * controller.height * 0.5f) + (transform.forward * hit.distance);
                Gizmos.DrawWireSphere(sphereCastMidpoint, 0.2f);
                Gizmos.DrawSphere(hit.point, 0.1f);
                Debug.DrawLine(transform.position + (Vector3.up * controller.height * 0.5f), sphereCastMidpoint, Color.green);
            }
            else
            {
                Gizmos.color = Color.red;
                Vector3 sphereCastMidpoint = transform.position + (Vector3.up * controller.height * 0.5f) + (transform.forward * (0.5f - 0.2f));
                Gizmos.DrawWireSphere(sphereCastMidpoint, 0.2f);
                Debug.DrawLine(transform.position + (Vector3.up * controller.height * 0.5f), sphereCastMidpoint, Color.red);
            }

            if (Physics.SphereCast(transform.position + (Vector3.up * controller.height), 0.2f, transform.forward, out RaycastHit targetHit, 1.5f, wallLayer))
            {
                Gizmos.color = Color.green;
                Vector3 sphereCastMidpoint = transform.position + (Vector3.up * controller.height) + (transform.forward * targetHit.distance);
                Gizmos.DrawWireSphere(sphereCastMidpoint, 0.2f);
                Gizmos.DrawSphere(targetHit.point, 0.1f);
                Debug.DrawLine(transform.position + (Vector3.up * controller.height), sphereCastMidpoint, Color.green);
            }
            else
            {
                Gizmos.color = Color.red;
                Vector3 sphereCastMidpoint = transform.position + (Vector3.up * controller.height) + (transform.forward * (1.5f - 0.2f));
                Gizmos.DrawWireSphere(sphereCastMidpoint, 0.2f);
                Debug.DrawLine(transform.position + (Vector3.up * controller.height), sphereCastMidpoint, Color.red);
            }
        }

        private void Awake()
        {
            Gravity = Physics.gravity.y;

            controller = GetComponent<CharacterController>();
            playerRigidbody = GetComponent<Rigidbody>();
            playerCamera = GetComponent<PlayerCamera>();
        }

        private void Update()
        {
            // if (!isLocalPlayer) return;

        }

        private void FixedUpdate()
        {
            // if (!isLocalPlayer) return;

            WallDectection();
            Movement();
        }

        private void Movement()
        {
            isGround = controller.isGrounded;

            if (isGround && playerVelocity.y < 0)
                playerVelocity.y = 0f;

            Vector3 movement = new Vector3(MovementInput.x, 0, MovementInput.y);
            if (movement == Vector3.zero)
                MovementState = PlayerState.Idle;

            movement = Camera.main.transform.forward * movement.z + Camera.main.transform.right * movement.x;
            movement.y = 0;
            controller.Move(movement * Time.deltaTime * CurrentSpeed);

            // if (movement != Vector3.zero)
            //     gameObject.transform.forward = movement;

            if (onJump && isGround)
                JumpGravity();

            if (!isSwim)
                JumpHandler();

            // switch (playerState)
            // {
            //     case PlayerState.Walk:
            //         controller.Move(movement * Time.deltaTime * currentSpeed);
            //         playerVelocity.y += Gravity * Time.deltaTime;
            //         controller.Move(playerVelocity * Time.deltaTime);
            //         break;
            //     case PlayerState.Climb:
            //         playerVelocity.y = MovementInput.y;
            //         controller.Move(playerVelocity * Time.deltaTime * 2f);
            //         break;

            //     case PlayerState.Idle:
            //         playerVelocity.y += Gravity * Time.deltaTime;
            //         controller.Move(playerVelocity * Time.deltaTime);
            //         break;
            //     case PlayerState.Swim:
            //         playerVelocity.y += Gravity * Time.deltaTime;
            //         controller.Move(playerVelocity * Time.deltaTime);
            //         break;
            // }
        }

        private void JumpGravity()
        {
            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * Gravity);
        }

        private void JumpHandler()
        {
            playerVelocity.y += Gravity * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }

        private void OnPlayerSpeedChange()
        {
            // Debug.Log("OnPlayerSpeedChange");
            switch (MovementState)
            {
                case PlayerState.Idle:
                    Debug.Log("PlayerState.Idle");
                    CurrentSpeed = 0;
                    break;
                case PlayerState.Walk:
                    Debug.Log("PlayerState.Walk");
                    CurrentSpeed = walkSpeed;
                    break;
                case PlayerState.Sprint:
                    Debug.Log("PlayerState.Sprint");
                    CurrentSpeed = sprintSpeed;
                    break;
                case PlayerState.Dodge:
                    Debug.Log("PlayerState.Dodge");
                    CurrentSpeed = dodgeSpeed;
                    break;
                case PlayerState.Climb:
                    Debug.Log("PlayerState.Climb");
                    CurrentSpeed = climbSpeed;
                    break;
                case PlayerState.Swim:
                    Debug.Log("PlayerState.Swim");
                    CurrentSpeed = swimSpeed;
                    break;
            }
        }

        private bool WallDectection()
        {
            if (Physics.SphereCast(transform.position + (Vector3.up * controller.height), 0.2f, transform.forward, out RaycastHit tophitted, 1.5f, wallLayer))
            {
                if (Physics.SphereCast(transform.position + (Vector3.up * controller.height * 0.5f), 0.2f, transform.forward, out RaycastHit hitted, 0.5f, wallLayer))
                {
                    MovementState = PlayerState.Climb;
                    return true;
                }
            }
            return false;
        }

        private void OnClimbing()
        {

        }

        public void OnMovement(InputValue value)
        {
            MovementInput = value.Get<Vector2>();
            MovementState = PlayerState.Walk;
            // Debug.Log(MovementInput);
        }

        private void OnLook(InputValue value)
        {
            LookDir = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            onJump = value.isPressed;
            MovementState = PlayerState.Jump;
            // Debug.Log(onJump);
        }

        private void OnSprint(InputValue value)
        {
            onSprint = value.isPressed;
            MovementState = PlayerState.Sprint;
            // Debug.Log(onSprint);
        }

        public void OnDodge(InputValue value)
        {
            onDodge = value.isPressed;
            MovementState = PlayerState.Dodge;
            // Debug.Log(onDodge);
        }

        public void OnEnableMouse(InputValue value)
        {
            CursorManager.Instance.alt_interact = !CursorManager.Instance.alt_interact;
        }

        public void OnUnstuck(InputValue value)
        {
            // if (CursorManager.instance.unstuck)
            // {

            // }
            // else
            // {

            // }
        }
    }
}

