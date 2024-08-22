using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Outlander.Player
{
    public class OfflineMovement : MonoBehaviour
    {

        Rigidbody playerRigibody;
        CapsuleCollider playerCollider;

        //public float staminaBarLength;
        [SerializeField] public GameObject spawnPoint;
        //[SerializeField] public Image staminaBar;
        //[SerializeField] public ParticleSystem TrailFX;
        public string cameraStyle;

        [Header("Stamina System")]
        [SerializeField] private float stamina = 100;
        public float DecreaseStamina { get => stamina; set => stamina -= value; }

        [SerializeField] private float maxStamina = 100;
        [SerializeField] private bool reachMaxStamina;

        [SerializeField] private bool rechargeStamina;

        //Tired after stamina = 0
        [SerializeField] private float minStamina = 20;
        [SerializeField] private bool reachMinStamina;


        [Header("Movement")]
        [SerializeField] private float playerMovementSpeed = 8;
        [SerializeField] public float walkSpeed = 4;
        [SerializeField] public float sprintSpeed = 8;
        [SerializeField] public float climbSpeed = 3;
        [SerializeField] public float dashSpeed = 20;

        [SerializeField] private float groundDrag = 5;
        [SerializeField] private float jumpForce = 10;
        [SerializeField] private float jumpCooldown = 0.25f;

        [SerializeField] private float jumpCooldownTimer;

        [SerializeField] private float airMultiplier = 0.4f;

        private Coroutine stuckCoroutine;
        private bool restricting;
        public bool skilling;
        public bool stucked;
        public bool isMoving;
        public bool readyToJump;
        public bool climbing;
        public bool freeze;
        public bool unlimited;
        public bool restricted;
        public bool dashing;
        public bool inWater;
        public bool isSprint;
        public bool wallHolding;
        public bool sitting = false;
        Vector3 moveDirection;

        [Header("Swimming")]
        public bool isSwim;
        public float inWaterSpeed = 3;
        public LayerMask whatIsWater;
        public bool swimming;
        public bool drowned;

        [Header("Climbimg")]
        public float jumpOffForce = 10f;
        private float climbJumpTimer;
        public float climbJumpCD = 1.5f;
        public bool onClimbJump;
        public bool climbMove;
        public LayerMask whatIsWall;
        private Vector3 climbDirection;
        private bool jumpOff;
        public bool jumpFromWall;
        public GameObject unstuckpoint;

        [Header("Stair Handling")]
        private RaycastHit lowStairHit, highStairHit;
        private bool onStair;

        [Header("WallDetection")]
        public float detectionLength = 2;
        public float climbOffset = 1.5f;
        public float sphereCastRadius = 0.25f;
        public float maxWallLookAngle = 30;
        public bool topCheck;
        private Transform topWall;
        private float wallLookAngle;
        private float wallVerticalAngle;

        private float angleClimb;
        private bool climbable;
        private RaycastHit frontwallHit, topwallHit, leftwallHit, rightwallHit,
            climbableWallHit, closewallHit;
        private bool wallFront, wallLeft, wallRight, wallClose, topreachable;
        private Vector3 startPos, checkDirection, offsetDetecting;
        private Vector3 targetPos;

        private bool wallTopReach;
        [HideInInspector] public bool wallLerping;

        public bool forceReaching;

        [Header("Dashing")]
        public float dashForce = 20;
        public float dashUpwardForce;
        public float dashDuration = 0.25f;
        private Vector3 delayedForceToApple;

        [Header("Cooldown")]
        public float dashCd = 1.5f;
        private float dashCdTimer;

        [Header("GroundCheck")]
        [SerializeField] private float playerHeight = 1.8f;
        public LayerMask whatIsGround;
        public bool grounded, highground;
        //private float fallDamageMultiply = 1.5f;
        Vector3 lastFallPosition;
        private bool startFalling;

        [Header("Slope Handling")]
        public float maxSlopeAngle = 50f;
        public RaycastHit slopeHit;
        private bool exitingSlope = false;

        [Header("Camera")]
        [SerializeField] private Transform orientation;

        [SerializeField] private Transform wall;

        [Header("PlayerObj")]
        [SerializeField] private Transform playerObj;

        [Header("input system")]
        public bool ENABLE_INPUT_SYSTEM = true;
        public bool onJump = false;
        [HideInInspector] public Vector2 movementInput = Vector2.zero;
        [HideInInspector] public Vector2 lookDir;

        // Start is called before the first frame update
        void Start()
        {
            wall = new GameObject().transform;
            spawnPoint = new GameObject();
            spawnPoint.transform.position = transform.position;
            spawnPoint.name = "SpawnPoint Handler";
            playerRigibody = GetComponent<Rigidbody>();
            playerRigibody.freezeRotation = true;
            playerCollider = GetComponent<CapsuleCollider>();
            //staminaBarLength = staminaBar.rectTransform.sizeDelta.x;
            PlayerInit();
            EnableInput();
        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 originRay = transform.position;
            originRay.y += 0.2f;
            if (wallFront)
                wall.transform.position = frontwallHit.point;
            grounded = Physics.Raycast(originRay, Vector3.down, playerHeight * 0.4f, whatIsGround);
            highground = Physics.Raycast(originRay, Vector3.down, playerHeight * 0.7f, whatIsGround);
            //Debug.DrawRay(originRay, Vector3.down * playerHeight * 0.2f,Color.black,0f);

            //Stuck Handler
            if (restricted)
            {
                if (!stucked && !restricting)
                {
                    restricting = true;

                    stuckCoroutine = StartCoroutine(RestrictedStuck());
                }
            }
            else if (restricting)
            {
                StopCoroutine(stuckCoroutine);
                restricting = false;
            }

            if (!highground && !swimming && !climbing)
            {
                if (!startFalling)
                {
                    lastFallPosition = Vector3.zero + transform.position;
                    startFalling = true;
                }
            }
            else if (grounded)
            {
                if (startFalling)
                {
                    float fallinDamage = lastFallPosition.y - transform.position.y;

                    if (fallinDamage >= 3)
                    {
                        //transform.GetComponent<IPlayer>().Damage(fallinDamage * fallDamageMultiply);
                    }
                    startFalling = false;
                }

            }

            //if (stamina >= maxStamina)
            //{
            //    staminaBar.gameObject.SetActive(false);
            //}
            //else
            //{
            //    staminaBar.gameObject.SetActive(true);
            //}

            //Press G to Sit
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (sitting)
                {
                    sitting = false;
            }
            else
            {
                    sitting = true;
                    restricted = true;
            }
            }

            // handle drag
            if (grounded)
                playerRigibody.drag = groundDrag;
            else
                playerRigibody.drag = 0;

            if (dashCdTimer > 0)
            {
                dashCdTimer -= Time.deltaTime;
            }

            if (climbJumpTimer > 0)
            {
                climbJumpTimer -= Time.deltaTime;
            }
            if (jumpCooldownTimer > 0)
            {
                jumpCooldownTimer -= Time.deltaTime;
            }

            PlayerSpeedControl();
            WallCheck();
            StateClimbing();
            staminaController();

            if (climbing)
            {
                ClimbingMovement();
            }

            if (wallLerping)
            {
                playerRigibody.useGravity = false;
                LerpingWallTop(topWall);
            }

        }

        private void FixedUpdate()
        {

            //if (GetComponentInChildren<IPlayer>().Die)
            //{
            //    ENABLE_INPUT_SYSTEM = false;
            //    PlayerInput playerInput = GetComponent<PlayerInput>();
            //    playerInput.enabled = ENABLE_INPUT_SYSTEM;
            //}
            MovePlayer();
        }

        IEnumerator RestrictedStuck()
        {
            yield return new WaitForSeconds(5);
            stucked = true;
        }

        private void StairHandler()
        {
            //Debug.DrawRay(transform.position + transform.up * 0.1f, playerObj.forward * 2f,Color.blue,0f) ;

            //Debug.DrawRay(transform.position + transform.up * 0.5f, playerObj.forward * 2f, Color.green, 0f);
            if (!Physics.Raycast(transform.position - transform.up * 0.05f, playerObj.forward, out RaycastHit hitted, 0.3f) && onStair)
            {
                onStair = false;
            }
            else
            {
                if (Physics.Raycast(transform.position + transform.up * 0.1f, playerObj.forward, out RaycastHit hitLower, 0.3f))
                {

                    if (!Physics.Raycast(transform.position + transform.up * 0.7f, playerObj.forward, out RaycastHit hitUpper, 0.5f))
                    {
                        onStair = true;
                    }
                }

                if (Physics.Raycast(transform.position + transform.up * 0.1f, playerObj.TransformDirection(1.5f, 0, 1), out RaycastHit hitLower45, 0.3f))
                {


                    if (!Physics.Raycast(transform.position + transform.up * 0.7f, playerObj.TransformDirection(1.5f, 0, 1), out RaycastHit hitUpper45, 0.5f))
                    {
                        onStair = true;
                    }
                }

                if (Physics.Raycast(transform.position + transform.up * 0.1f, playerObj.TransformDirection(-1.5f, 0, 1), out RaycastHit hitLowerMinus45, 0.3f))
                {


                    if (!Physics.Raycast(transform.position + transform.up * 0.7f, playerObj.TransformDirection(-1.5f, 0, 1), out RaycastHit hitMinusUpper45, 0.5f))
                    {
                        onStair = true;
                    }
                }
            }

            if (onStair && (moveDirection.x > 0 || moveDirection.y > 0) && !dashing)
            {
                if (Vector3.Angle(Vector3.up, slopeHit.normal) < maxSlopeAngle)
                    playerRigibody.position -= new Vector3(0f, -0.1f, 0f);
            }

        }

        private void WallCheck()
        {
            //Debug.DrawRay(transform.position + playerObj.forward * -1f + transform.up * 1f, playerObj.forward * climbOffset,Color.blue,0f);

            //Debug.DrawRay(transform.position + playerObj.forward * -1f + transform.up * 2f, playerObj.forward * detectionLength,Color.gray,0f);

            //Debug.DrawRay(transform.position + playerObj.forward * -1f + transform.up * 1f, playerObj.forward * detectionLength, Color.green, 0f);
            //Debug.DrawRay(transform.position + playerObj.forward * -1f + playerObj.right * 0.5f + transform.up * 1f, playerObj.forward * detectionLength, Color.red, 0f);
            //Debug.DrawRay(transform.position + playerObj.forward * -1f + playerObj.right * -0.5f + transform.up * 1f, playerObj.forward * detectionLength, Color.yellow, 0f);


            topreachable = Physics.Raycast(transform.position + (playerObj.forward * 0.5f) + (playerObj.up * 4f)
                , -playerObj.up, out closewallHit, whatIsGround);

            //Debug.DrawRay(transform.position + (playerObj.forward * 0.5f) + (playerObj.up * 2f), -playerObj.up * 2f, Color.yellow, 0f);

            wallClose = Physics.Raycast(transform.position + offsetDetecting, playerObj.forward, out closewallHit, detectionLength, whatIsWall);

            if (!restricted || !onClimbJump)
            {
                offsetDetecting = transform.TransformDirection(Vector2.one * 0.5f);
                offsetDetecting += playerObj.forward * -2f;
                checkDirection = Vector3.zero;
                int k = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (wallClose)
                    {
                        checkDirection += closewallHit.normal;
                        k++;
                    }
                    offsetDetecting = Quaternion.AngleAxis(90f, playerObj.forward) * offsetDetecting;
                }
                checkDirection /= k;
            }

            climbable = Physics.Raycast(transform.position + offsetDetecting + transform.up * 2f, playerObj.forward, out climbableWallHit, detectionLength, whatIsWall);

            //wallFront = Physics.SphereCast(transform.position + playerObj.forward * -1f, sphereCastRadius, playerObj.forward, out frontwallHit, detectionLength, whatIsWall);

            //wallRight = Physics.SphereCast(transform.position + playerObj.forward * -1f + playerObj.right * 0.5f, sphereCastRadius, playerObj.forward, out leftwallHit, climbOffset, whatIsWall);

            //wallLeft = Physics.SphereCast(transform.position + playerObj.forward * -1f + playerObj.right * -0.5f, sphereCastRadius, playerObj.forward, out rightwallHit, climbOffset, whatIsWall);

            wallLookAngle = Vector3.Angle(playerObj.forward, -closewallHit.normal);

            wallVerticalAngle = Vector3.Angle(playerObj.up, -closewallHit.normal);

            if (climbable && climbing)
            {
                topCheck = false;
            }
            if (grounded && !sitting)
            {
                restricted = false;
                forceReaching = false;
            }
        }

        private void StartClimbing()
        {
            if (!unstuckpoint)
                unstuckpoint = Instantiate(orientation.gameObject, transform.position, Quaternion.identity);
            else
                unstuckpoint.transform.position = transform.position;
            playerRigibody.useGravity = false;
            climbing = true;
            readyToJump = false;
            wallHolding = true;

        }
        private void ClimbingMovement()
        {

            if (!restricted)
            {
                climbDirection = (playerObj.up * movementInput.y) + (playerObj.right * movementInput.x);
            }

            if ((movementInput.x != 0 || movementInput.y != 0))
            {
                climbMove = true;
                ////if (wallFront)
                ////    transform.position = Vector3.Slerp(transform.position, frontwallHit.point, Time.deltaTime * 5);
                //playerRigibody.velocity = climbDirection * climbSpeed;

                RaycastHit hit;

                if (Physics.Raycast(transform.position + playerObj.forward * -1f, -checkDirection, out hit, detectionLength, whatIsWall) || onClimbJump)
                {
                    if (!onClimbJump)
                    {
                        playerRigibody.position = Vector3.Lerp(playerRigibody.position, hit.point + hit.normal * 0.05f, 5f * Time.fixedDeltaTime);
                    }
                    playerObj.forward = Vector3.Lerp(playerObj.forward, -hit.normal, 10f * Time.fixedDeltaTime);

                    playerRigibody.velocity = climbDirection * climbSpeed;

                }
                else if (climbing)
                    StopClimbing();
            }
            else
            {
                climbMove = false;

            }

            if (onClimbJump && !restricted && reachMinStamina && climbMove)
            {
                ClimbJumping();
            }

            if (movementInput.y == 0 && movementInput.x == 0 && wallHolding)
            {
                if (onClimbJump)
                {

                    jumpFromWall = true;
                    if (!jumpOff)
                    {

                        Invoke(nameof(JumpOffFromWall), 0.9f);

                        jumpOff = true;
                    }
                }
                else
                    freeze = true;
            }
        }

        private void JumpOffFromWall()
        {
            StopClimbing();
            freeze = false;
            restricted = true;
            playerRigibody.useGravity = true;
            playerRigibody.AddForce(playerObj.forward * -jumpOffForce, ForceMode.Impulse);
        }

        private void RotatePlayer180()
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, -playerObj.forward.normalized, Time.deltaTime * 30);

        }

        private void ForceRespawn()
        {
            swimming = false;
            playerRigibody.useGravity = true;
            drowned = false;
            restricted = false;
            GetComponent<Animator>().CrossFade("Idle", 0.1f);
            GetComponent<Animator>().SetBool("swimMove", false);

            transform.position = spawnPoint.transform.position;

        }

        private void ClimbJumping()
        {
            if (climbJumpTimer > 0)
            {
                return;
            }

            else
            {
                climbJumpTimer = climbJumpCD;
            }

            onClimbJump = true;
            restricted = true;
            Invoke(nameof(DelayedClimbJumpForce), 0.2f);

            Invoke(nameof(ResetClimbJump), 0.5f);
        }


        private void StopClimbing()
        {
            Vector3 inputDir = orientation.forward * movementInput.y + orientation.right * movementInput.x;
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * 7f);
            playerRigibody.useGravity = true;
            climbing = false;
            wallHolding = false;
            freeze = false;
            readyToJump = true;
        }

        private void DelayedClimbJumpForce()
        {
            playerRigibody.AddForce(climbDirection * 30f, ForceMode.VelocityChange);
            stamina -= 20f;
        }

        private void ResetClimbJump()
        {
            onClimbJump = false;
            restricted = false;
        }

        private void StateClimbing()
        {
            if (wallClose && wallLookAngle < maxWallLookAngle && wallVerticalAngle <= 100)
            {
                if (movementInput.y > 0 && !climbing && !restricted && climbable)
                {
                    StartClimbing();
                }
                //wallTopReach = Physics.SphereCast(transform.position + playerObj.forward * -1f + transform.up * 1.5f, sphereCastRadius, playerObj.forward, out topwallHit, climbOffset, whatIsWall);

                wallTopReach = Physics.Raycast(transform.position + offsetDetecting + transform.up * 0.6f, playerObj.forward, out topwallHit, detectionLength, whatIsWall);

                //Debug.DrawRay(transform.position + playerObj.forward * -1f + transform.up * 1.5f, playerObj.forward * detectionLength, Color.magenta, 0f);

                if (wallTopReach)
                {
                    forceReaching = true;

                    if (!topWall)
                    {
                        topWall = new GameObject().transform;
                        topWall.name = "Topwall Handler";
                    }

                    topWall.position = topwallHit.point + playerObj.forward * 0.5f + playerObj.up * 0.1f;
                }

            }

            else
            {
                if (climbing && !jumpOff && !wallClose)
                {
                    StopClimbing();
                }
            }
            //Debug.Log(wallTopReach.ToString() + forceReaching.ToString() + wallClose.ToString());
            if (!wallTopReach && forceReaching && wallClose && topreachable)
            {
                playerRigibody.AddForce(Vector3.up * 45f, ForceMode.Force);
                //playerCollider.center = new Vector3(0, 1f, 0);
                climbDirection = Vector3.zero;
                restricted = true;
                topCheck = true;

            }

            if (topCheck && !wallClose)
            {
                ResetVelocity();
                Invoke(nameof(StopClimbing), 0.25f);
                forceReaching = false;
                wallLerping = true;
                topCheck = false;
            }

            if (stamina <= 0 && climbing)
            {
                StopClimbing();
                ResetVelocity();
                playerRigibody.useGravity = true;
                restricted = true;

            }
            if (grounded && movementInput.y < 0 && climbing)
            {
                StopClimbing();
                Vector3 originAngle = playerObj.rotation.eulerAngles;
                playerObj.rotation = Quaternion.Euler(0, originAngle.y, originAngle.z);
            }

        }
        private void LerpingWallTop(Transform topwallPo)
        {
            transform.position = Vector3.Lerp(transform.position, topwallPo.position, 0.3f);
        }
        private void PlayerInit()
        {
            readyToJump = true;
        }

        #region Player Movement
        private void MovePlayer()
        {
            if (restricted || skilling) return;
            StairHandler();

            JumpHandler();

            if (!isSprint)
            {
                playerMovementSpeed = walkSpeed;
                if (!reachMaxStamina) rechargeStamina = true;
            }

            else if (isSprint && reachMinStamina)
            {
                if (swimming) playerMovementSpeed = sprintSpeed - 3;
                else playerMovementSpeed = sprintSpeed;
                stamina -= 4 * Time.deltaTime;
                    rechargeStamina = false;
                }

            if (isSwim)
            {
                playerRigibody.useGravity = false;
                playerRigibody.AddForce(Vector3.up * 50f, ForceMode.Force);
            }
            else if (!inWater && !climbing && grounded)
            {
                playerRigibody.useGravity = !OnSlope();
            }
            //cal move direction
            moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

            if (OnSlope() && !exitingSlope && !climbing && !onJump && grounded)
            {
                if (!dashing)
                {
                    playerRigibody.AddForce(GetslopeMove() * playerMovementSpeed * 20f, ForceMode.Force);
                }
                if (playerRigibody.velocity.y > 0 || dashing)
                {
                    playerRigibody.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }

            //onground
            if (!climbing)
            {
                if (grounded)
                {
                    if (!swimming)
                        playerRigibody.useGravity = true;
                    if (topWall && wallLerping)
                    {
                        wallLerping = false;
                        playerRigibody.useGravity = true;
                    }
                    playerRigibody.AddForce(moveDirection.normalized * playerMovementSpeed * 10f, ForceMode.Force);
                    jumpFromWall = false;
                    if (jumpCooldownTimer <= 0)
                    {
                        readyToJump = true;
                    }
                    if (jumpOff)
                    {
                        playerObj.forward = -playerObj.forward;
                        jumpOff = false;
                        restricted = false;
                    }
                    if (rechargeStamina) stamina += 10 * Time.deltaTime;
                }
                else if (!grounded)
                {
                    playerRigibody.AddForce(moveDirection.normalized * playerMovementSpeed * 10f * airMultiplier, ForceMode.Force);
                    if (swimming && isMoving)
                        stamina -= 2 * Time.deltaTime;
                    if (isSwim)
                    {
                        swimming = true;
                    }
                    if (swimming && stamina <= 0 && !restricted)
            {
                drowned = true;
                restricted = true;
            }
                }
            }

            if (movementInput.x != 0 || movementInput.y != 0)
            {
                isMoving = true;
            }

            else
            {
                isMoving = false;
            }
        }

        private void staminaController()
        {

            if (stamina >= maxStamina && !reachMaxStamina)
            {
                stamina = maxStamina;
                reachMaxStamina = true;
                rechargeStamina = false;
            }
            else if (stamina < maxStamina) reachMaxStamina = false;

            if (stamina <= 0 && reachMinStamina)
            {
                stamina = 0;
                reachMinStamina = false;
            }

            if (!reachMinStamina)
            {
                onJump = false;
                onClimbJump = false;
                isSprint = false;
                if (stamina >= minStamina)
                    reachMinStamina = true;
            }
        }

        public void ResetVelocity()
        {
            playerRigibody.velocity = Vector3.zero;
            playerRigibody.angularVelocity = Vector3.zero;
        }
        private void OnCollisionEnter(Collision collision)
        {
            //Unstuck
            if (!readyToJump && !climbing)
            {
                restricted = true;
            }
            if (swimming && collision.gameObject.layer == LayerMask.NameToLayer("Ground") && grounded)
            {
                swimming = false;
            }
        }
        private void ResetSkillState()
        {
            skilling = false;
        }
        private void PlayerSpeedControl()
        {
            if (inWater)
            {
                if (isSprint == true)
                {
                    if (playerRigibody.velocity.magnitude > sprintSpeed - 2)
                    {
                        playerRigibody.velocity = playerRigibody.velocity.normalized * (sprintSpeed - 2);
                    }
                }
                else
                {
                    if (playerRigibody.velocity.magnitude > walkSpeed - 2)
                    {
                        playerRigibody.velocity = playerRigibody.velocity.normalized * (walkSpeed - 2);
                    }
                }


            }
            else if (dashing)
            {
                playerRigibody.velocity = playerRigibody.velocity.normalized * dashSpeed;
            }

            else if (OnSlope() && !exitingSlope)
            {
                if (playerRigibody.velocity.magnitude > playerMovementSpeed)
                {
                    playerRigibody.velocity = playerRigibody.velocity.normalized * playerMovementSpeed;
                }
            }
            else if (freeze)
            {
                playerRigibody.velocity = Vector3.zero;
            }
            else if (unlimited)
            {

                playerRigibody.velocity = (playerRigibody.velocity * 999f).normalized;

                return;
            }

            else if (climbing)
            {
                if (playerRigibody.velocity.magnitude > climbSpeed)
                {
                    playerRigibody.velocity = playerRigibody.velocity.normalized * climbSpeed;
                }
            }

            else
            {
                Vector3 flatVel = new Vector3(playerRigibody.velocity.x, 0f, playerRigibody.velocity.z);
                //limit velocity
                if (flatVel.magnitude > playerMovementSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * playerMovementSpeed;
                    playerRigibody.velocity = new Vector3(limitedVel.x, playerRigibody.velocity.y, limitedVel.z);
                }
            }

        }

        private void Jump()
        {
            if (jumpCooldownTimer > 0)
                return;
            else
                jumpCooldownTimer = jumpCooldown;

            exitingSlope = true;
            stamina -= 20f;
            playerRigibody.velocity = new Vector3(playerRigibody.velocity.x, 0f, playerRigibody.velocity.z);
            playerRigibody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void Dodge()
        {
            if (dashCdTimer > 0)
            {
                return;
            }

            else
            {
                dashCdTimer = dashCd;
            }

            dashing = true;

            Vector3 forceToApply = playerObj.forward * dashForce + orientation.up * dashUpwardForce;

            delayedForceToApple = forceToApply;

            Invoke(nameof(DelayedDashForce), 0.1f);

            Invoke(nameof(ResetDodge), dashDuration);
        }

        private void DelayedDashForce()
        {
            playerRigibody.AddForce(delayedForceToApple, ForceMode.Impulse);
        }

        private void ResetDodge()
        {
            dashing = false;
        }

        private void ResetJump()
        {
            exitingSlope = false;

            onJump = false;
        }

        private bool OnSlope()
        {
            Vector3 originRay = transform.position;
            originRay.y += 0.2f;
            if (Physics.Raycast(originRay, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, whatIsGround))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }
            else return false;
        }

        private Vector3 GetslopeMove()
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }
        #endregion


        #region Input System
        private void EnableInput()
        {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = ENABLE_INPUT_SYSTEM;
        }

        private void JumpHandler()
        {
            if (onJump && readyToJump && grounded && !climbing && reachMinStamina)
            {
                Invoke(nameof(Jump), 0.025f);

                Invoke(nameof(ResetJump), jumpCooldown);

                readyToJump = false;
            }
        }

        //[Client]
        public void OnMovement(InputValue value)
        {
            movementInput = value.Get<Vector2>();
        }
        //[Client]
        public void OnJump(InputValue value)
        {
            if (readyToJump && grounded && !restricted && !skilling)
            {
                onJump = value.isPressed;
            }
            if (climbing && !restricted && !skilling)
            {
                onClimbJump = value.isPressed;
            }
        }
        public void OnSprint(InputValue value)
        {
            if (movementInput != Vector2.zero)
                isSprint = value.isPressed;
        }

        public void OnDodge(InputValue value)
        {
            if (grounded && !climbing && !restricted && !skilling)
            {
                Dodge();
            }
        }

        public void OnLook(InputValue value)
        {
            lookDir = value.Get<Vector2>();
        }

        public void OnEnableMouse(InputValue value)
        {
            Debug.Log($"OnEnableMouse {value.isPressed}");

            // if (!Cursor.visible) return;

            Cursor.visible = value.isPressed;
            Cursor.lockState = (Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;
        }

        #endregion
    }
}
