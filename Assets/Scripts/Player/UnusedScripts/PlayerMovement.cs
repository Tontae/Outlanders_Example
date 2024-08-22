//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.UI;
//using Mirror;

//namespace Outlander.Player
//{
//    public class PlayerMovement : PlayerElements
//    {
//        public float staminaBarLength;
//        [SerializeField] public GameObject spawnPoint;
//        //[SerializeField] public Image staminaBar, staminaBarFrame;
//        [SerializeField] public ParticleSystem TrailFX;
//        public PlayerCamera.CameraStyle cameraStyle;

//        [Header("Stamina System")]
//        [SerializeField] private float stamina = 100;
//        public float Stamina { get => stamina; set => stamina = value; }
//        private bool isUsing;
//        public bool useConsumable { get => isUsing; set => isUsing = value; }
//        [SerializeField] private float maxStamina = 100;
//        [SerializeField] private bool reachMaxStamina;

//        [SerializeField] private bool rechargeStamina;

//        //Tired after stamina = 0
//        [SerializeField] private float minStamina = 20;
//        [SerializeField] private bool reachMinStamina;

//        [Header("Movement")]
//        [SerializeField] private float playerMovementSpeed = 8;
//        [SerializeField] public float walkSpeed = 4;
//        [SerializeField] public float sprintSpeed;
//        [SerializeField] public float climbSpeed = 3;
//        [SerializeField] public float dashSpeed = 20;

//        public float Speed { get => walkSpeed; set => walkSpeed = value; }


//        [SerializeField] private GameObject[] overweightIcon;
//        [SerializeField] private float groundDrag = 5;
//        [SerializeField] private float jumpForce = 10;
//        [SerializeField] private float jumpCooldown = 0.25f;

//        [SerializeField] private float weightDebuffSpeed;
//        [SerializeField] private float jumpCooldownTimer;

//        [SerializeField] private float airMultiplier = 0.4f;

//        [SerializeField] private TMP_Text borderText;
//        private float percentage;
//        public float borderTimer;

//        private Coroutine stuckCoroutine;
//        private bool restricting;
//        public bool stunned;
//        public bool exitBordering;
//        public bool skilling;
//        public bool onCrouch;
//        public bool stucked;
//        public bool isMoving;
//        public bool readyToJump;
//        public bool climbing;
//        public bool freeze;
//        public bool unlimited;
//        public bool restricted;
//        public bool dashing;
//        public bool inWater;
//        public bool isSprint;
//        public bool wallHolding;
//        public bool sitting = false;
//        Vector3 moveDirection;

//        [Header("Swimming")]
//        public bool isSwim;
//        public float inWaterSpeed = 3;
//        public LayerMask whatIsWater;
//        public bool swimming;
//        public bool drowned;

//        [Header("Climbimg")]
//        public float jumpOffForce = 10f;
//        private float climbJumpTimer;
//        private float climbSpeedTmp;
//        public float climbJumpCD = 1.5f;
//        public bool onClimbJump;
//        public bool climbMove;
//        public LayerMask whatIsWall;
//        private Vector3 climbDirection;
//        private bool jumpOff;
//        public bool jumpFromWall;
//        private bool lerpCoroutine;
//        private Coroutine wallLerp;
//        public GameObject unstuckpoint;

//        [Header("Stair Handling")]
//        private RaycastHit lowStairHit, highStairHit;
//        private bool onStair;

//        [Header("WallDetection")]
//        public float detectionLength = 2;
//        public float climbOffset = 1.5f;
//        public float sphereCastRadius = 0.25f;
//        public float maxWallLookAngle = 30;
//        public bool topCheck;
//        private Transform topWall;
//        private float wallLookAngle;
//        private float wallVerticalAngle;
//        private Coroutine climbjumpCoroutine;
//        private float angleClimb;
//        private bool climbable;
//        private RaycastHit topwallHit, climbableWallHit, closewallHit, topreachableHit;
//        private bool wallClose, topreachable;
//        private Vector3 startPos, offsetDetecting, checkDirection;
//        private Vector3 targetPos;

//        private bool wallTopReach;
//        [HideInInspector] public bool wallLerping;

//        public bool forceReaching;

//        [Header("Dashing")]
//        public float dashForce = 20;
//        public float dashUpwardForce;
//        public float dashDuration = 0.25f;
//        private Vector3 delayedForceToApple;

//        [Header("Cooldown")]
//        public float dashCd = 1.5f;
//        private float dashCdTimer;
//        private float rechargeCooldown;
//        public void ResetRechargeCooldown() => rechargeCooldown = 2f;

//        [Header("GroundCheck")]
//        [SerializeField] private float playerHeight = 1.8f;
//        public LayerMask whatIsGround;
//        public bool grounded, highground;
//        private float fallDamageMultiply = 1.5f;
//        Vector3 lastFallPosition;
//        private bool startFalling;

//        [Header("Slope Handling")]
//        public float maxSlopeAngle = 70f;
//        public RaycastHit slopeHit;
//        private bool exitingSlope = false;

//        [Header("Camera")]
//        [SerializeField] private Transform orientation;

//        [SerializeField] private Transform wall;

//        [Header("input system")]
//        public bool ENABLE_INPUT_SYSTEM = true;
//        public bool onJump = false;
//        [HideInInspector] public Vector2 movementInput = Vector2.zero;
//        [HideInInspector] public Vector2 lookDir;

//        [Header("Helper")]
//        string environment;

//        private void OnDrawGizmos()
//        {
//            if (grounded)
//            {
//                Vector3 originRay = transform.position;
//                originRay.y += 0.2f;

//                Gizmos.color = Color.red;
//                Gizmos.DrawRay(originRay, Vector3.down * 2f);
//                Gizmos.DrawWireSphere(originRay + Vector3.down * 2f, sphereCastRadius);
//            }
//            else
//            {
//                Vector3 originRay = transform.position;
//                originRay.y += 0.2f;

//                Gizmos.color = Color.green;
//                Gizmos.DrawRay(originRay, Vector3.down * 2f);

//            }
//        }

//        // Start is called before the first frame update
//        private void OnEnable()
//        {
//            if (!isLocalPlayer) return;
//            Player.PlayerUIActionHandler.OnUIUnstuck += DisplayUnstuckPanel;
//        }

//        private void OnDisable()
//        {
//            if (!isLocalPlayer) return;
//            Player.PlayerUIActionHandler.OnUIUnstuck -= DisplayUnstuckPanel;
//        }
//        /*private IEnumerator GetStuned(float duration)
//        {
//            Debug.Log("2");

//            stunned = true;
//            yield return new WaitForSeconds(duration);
//            stunned = false;
//            Debug.Log("3");


//        }

//        public void SetPlayerStun(float duration)
//        {
//            Debug.Log("1");
//            StartCoroutine(GetStuned(duration));
//        }*/

//        void Start()
//        {
//            if (!isLocalPlayer) return;

//            wall = new GameObject().transform;
//            spawnPoint = new GameObject();
//            spawnPoint.transform.position = transform.position;
//            spawnPoint.name = "SpawnPoint Handler";
//            Player.Rigidbody.freezeRotation = true;
//            staminaBarLength = UIManagers.Instance.playerCanvas.staminaBar.rectTransform.sizeDelta.x;
//            PlayerInit();
//            EnableInput();
//        }
//        public bool IsAction()
//        {
//            skilling = Player.PlayerOutlander.skilling;
//            return (swimming || climbing || skilling || onJump || dashing || isSprint || stunned || Player.PlayerOutlander.GetWeaponAction());
//        }
//        public bool IsActionSwap()
//        {
//            skilling = Player.PlayerOutlander.skilling;
//            return (swimming || climbing || skilling || dashing || stunned || Player.PlayerOutlander.GetWeaponAction());
//        }

//        // Update is called once per frame
//        private void Update()
//        {
//            if (!isLocalPlayer) return;
//            Vector3 originRay = transform.position;
//            originRay.y += 0.2f;
//            if (wallClose)
//                wall.transform.position = closewallHit.point;
//            grounded = Physics.Raycast(originRay, Vector3.down, playerHeight * 0.4f, whatIsGround);
//            highground = Physics.Raycast(originRay, Vector3.down, playerHeight * 0.7f, whatIsGround);
//            if (Player.PlayerOutlander.skilling)
//            {
//                if (onCrouch)
//                    SetCrouchState(false);
//            }
//            if (!highground && !swimming && !climbing)
//            {
//                if (!startFalling && (Player.Rigidbody.velocity.y <= 0))
//                {
//                    lastFallPosition = Vector3.zero + transform.position;
//                    Player.AnimationScript.SetRigWeight(0);
//                    startFalling = true;
//                }
//            }
//            else if (grounded)
//            {
//                if (startFalling)
//                {
//                    float fallinDamage = lastFallPosition.y - transform.position.y;

//                    if (fallinDamage >= 6)
//                    {
//                        if (PlayerManagers.Instance.matchManager.canInteract)
//                            Player.PlayerOutlander.CmdDamageIgnoreDefense("FALL", fallinDamage * fallDamageMultiply);
//                    }
//                    startFalling = false;
//                }

//            }

//            if (Stamina >= maxStamina)
//            {
//                UIManagers.Instance.playerCanvas.staminaBar.gameObject.SetActive(false);
//                UIManagers.Instance.playerCanvas.staminaBarFrame.gameObject.SetActive(false);
//            }
//            else
//            {
//                UIManagers.Instance.playerCanvas.staminaBar.gameObject.SetActive(true);
//                UIManagers.Instance.playerCanvas.staminaBarFrame.gameObject.SetActive(true);
//            }

//            //Press G to Sit
//            //if (Input.GetKeyDown(KeyCode.G))
//            //{
//            //    //if (sitting)
//            //    //{
//            //    //    sitting = false;
//            //    //}
//            //    //else
//            //    //{
//            //    //    sitting = true;
//            //    //    restricted = true;
//            //    //}
//            //}

//            //Press J to Stun
//            //if (Input.GetKeyDown(KeyCode.J))
//            //{
//            //    if (stunned)
//            //    {
//            //        stunned = false;
//            //    }
//            //    else
//            //    {
//            //        stunned = true;
//            //    }
//            //}

//            //handle drag
//            if (grounded)
//                Player.Rigidbody.drag = groundDrag;
//            else
//                Player.Rigidbody.drag = 0;

//            if (dashCdTimer > 0)
//            {
//                dashCdTimer -= Time.deltaTime;
//            }

//            if (climbJumpTimer > 0)
//            {
//                climbJumpTimer -= Time.deltaTime;
//            }
//            if (jumpCooldownTimer > 0)
//            {
//                jumpCooldownTimer -= Time.deltaTime;
//            }

//            PlayerSpeedControl();
//            WallCheck();
//            StateClimbing();
//            StaminaController();

//            if (climbing)
//            {
//                ClimbingMovement();
//            }

//            if (wallLerping)
//            {
//                Player.Rigidbody.useGravity = false;
//                wallLerp = StartCoroutine(LerpingWallTop(topWall));
//            }

//        }

//        private void FixedUpdate()
//        {
//            if (!isLocalPlayer) return;

//            //if (GetComponentInChildren<IPlayer>().Die)
//            //{
//            //    ENABLE_INPUT_SYSTEM = false;
//            //    PlayerInput playerInput = GetComponent<PlayerInput>();
//            //    playerInput.enabled = ENABLE_INPUT_SYSTEM;
//            //}
//            MovePlayer();
//        }

//        //IEnumerator RestrictedStuck()
//        //{
//        //    yield return new WaitForSeconds(5);
//        //    stucked = true;
//        //}

//        private void StairHandler()
//        {
//            if (!Physics.Raycast(transform.position - transform.up * 0.05f, transform.forward, out RaycastHit hitted, 0.6f) && onStair)
//            {
//                onStair = false;
//            }
//            else
//            {
//                if (Physics.Raycast(transform.position + transform.up * 0.1f, transform.forward, out RaycastHit hitLower, 0.6f))
//                {
//                    if (!Physics.Raycast(transform.position + transform.up * 0.7f, transform.forward, out RaycastHit hitUpper, 2f))
//                    {
//                        onStair = true;
//                    }
//                }

//                if (Physics.Raycast(transform.position + transform.up * 0.1f, transform.TransformDirection(1.5f, 0, 1), out RaycastHit hitLower45, 0.6f))
//                {
//                    if (!Physics.Raycast(transform.position + transform.up * 0.7f, transform.TransformDirection(1.5f, 0, 1), out RaycastHit hitUpper45, 2f))
//                    {
//                        onStair = true;
//                    }
//                }

//                if (Physics.Raycast(transform.position + transform.up * 0.1f, transform.TransformDirection(-1.5f, 0, 1), out RaycastHit hitLowerMinus45, 0.6f))
//                {
//                    if (!Physics.Raycast(transform.position + transform.up * 0.7f, transform.TransformDirection(-1.5f, 0, 1), out RaycastHit hitMinusUpper45, 2f))
//                    {
//                        onStair = true;
//                    }
//                }
//            }

//            /* if (onStair && (moveDirection.x > 0 || moveDirection.y > 0) && !dashing && !swimming)
//             {
//                 if (Vector3.Angle(Vector3.up, slopeHit.normal) < maxSlopeAngle)
//                     Player.Rigidbody.position -= new Vector3(0f, -0.1f, 0f);
//             }*/

//        }

//        private void WallCheck()
//        {

//            topreachable = Physics.Raycast(transform.position + (transform.forward * 0.5f) + (transform.up * 4f)
//                , -transform.up, out closewallHit, whatIsGround);

//            //Debug.DrawRay(transform.position + (transform.forward * 0.5f) + (transform.up * 2f), -transform.up * 2f, Color.yellow, 0f);

//            wallClose = Physics.Raycast(transform.position + offsetDetecting, transform.forward, out closewallHit, detectionLength, whatIsWall);

//            if (!restricted || !onClimbJump)
//            {
//                offsetDetecting = transform.TransformDirection(Vector2.one * 0.5f);
//                offsetDetecting += transform.forward * -2f;
//                checkDirection = Vector3.zero;
//                int k = 0;

//                for (int i = 0; i < 4; i++)
//                {
//                    if (wallClose)
//                    {
//                        checkDirection += closewallHit.normal;
//                        k++;
//                    }
//                    offsetDetecting = Quaternion.AngleAxis(90f, transform.forward) * offsetDetecting;
//                }
//                checkDirection /= k;
//            }

//            climbable = Physics.Raycast((transform.position + offsetDetecting) + (transform.up * Player.CapsuleCollider.height * 0.8f), transform.forward, out climbableWallHit, detectionLength, whatIsWall);

//            wallLookAngle = Vector3.Angle(transform.forward, -closewallHit.normal);

//            wallVerticalAngle = Vector3.Angle(transform.up, -closewallHit.normal);

//            if (climbable && climbing)
//            {
//                topCheck = false;
//            }
//            if (grounded && !sitting)
//            {
//                restricted = false;
//                forceReaching = false;
//            }
//        }

//        private void StartClimbing()
//        {
//            if (!unstuckpoint)
//                unstuckpoint = Instantiate(orientation.gameObject, transform.position, Quaternion.identity);
//            else
//                unstuckpoint.transform.position = transform.position;

//            if (startFalling)
//                startFalling = false;

//            if (lerpCoroutine)
//            {
//                Player.CapsuleCollider.height = 1.8f;
//                StopCoroutine(wallLerp);
//                lerpCoroutine = false;
//            }
//            if (onCrouch)
//                SetCrouchState(false);
//            climbSpeed = 3;
//            Player.Rigidbody.useGravity = false;
//            climbing = true;
//            readyToJump = false;
//            wallHolding = true;

//        }
//        private void ClimbingMovement()
//        {
//            if (PlayerManagers.Instance.matchManager.canInteract)
//                Stamina -= Time.deltaTime;

//            if (!restricted)
//            {
//                climbDirection = (transform.up * movementInput.y) + (transform.right * movementInput.x);
//            }

//            if ((movementInput.x != 0 || movementInput.y != 0))
//            {
//                climbMove = true;
//                RaycastHit hit;

//                if (Physics.Raycast(transform.position + transform.forward * -1f, -checkDirection, out hit, detectionLength, whatIsWall) || onClimbJump)
//                {
//                    if (!onClimbJump)
//                    {
//                        Player.Rigidbody.position = Vector3.Lerp(Player.Rigidbody.position, hit.point + hit.normal * 0.05f, 5f * Time.fixedDeltaTime);
//                    }
//                    transform.forward = Vector3.Lerp(transform.forward, -hit.normal, 10f * Time.fixedDeltaTime);

//                    Player.Rigidbody.velocity = climbDirection * climbSpeed;

//                }
//                else if (climbing)
//                    StopClimbing();
//            }
//            else
//            {
//                climbMove = false;

//            }

//            /*if (onClimbJump && !restricted && reachMinStamina && climbMove)
//            {
//                ClimbJumping();
//            }*/

//            if (!isMoving && wallHolding)
//            {
//                if (onClimbJump)
//                {
//                    StopClimbing();
//                    Player.Animator.Play("floating");
//                }
//                else
//                    freeze = true;
//            }
//        }

//        private void JumpOffFromWall()
//        {
//            StopClimbing();
//            freeze = false;
//            restricted = true;
//            Player.Rigidbody.useGravity = true;
//            Player.Rigidbody.AddForce(transform.forward * -jumpOffForce, ForceMode.Impulse);
//            onClimbJump = false;
//        }

//        private void RotatePlayer180()
//        {
//            transform.forward = Vector3.Slerp(transform.forward, -transform.forward.normalized, Time.deltaTime * 30);

//        }

//        private void ClimbJumping()
//        {
//            if (climbJumpTimer > 0)
//            {
//                return;
//            }

//            else
//            {
//                climbJumpTimer = climbJumpCD;
//            }

//            restricted = true;
//            climbjumpCoroutine = StartCoroutine(DelayedClimbJumpForce());
//        }


//        private void StopClimbing()
//        {
//            Vector3 inputDir = orientation.forward * movementInput.y + orientation.right * movementInput.x;
//            //transform.forward = Vector3.Slerp(transform.forward, inputDir.normalized, Time.deltaTime * 7f);
//            Player.Rigidbody.useGravity = true;
//            onClimbJump = false;
//            climbing = false;
//            wallHolding = false;
//            freeze = false;
//            readyToJump = true;
//        }

//        IEnumerator DelayedClimbJumpForce()
//        {
//            yield return new WaitForSeconds(0.2f);
//            climbSpeedTmp = 0 + climbSpeed;
//            climbSpeed += 10;

//            if (PlayerManagers.Instance.matchManager.canInteract)
//                Stamina -= 20f;
//            yield return new WaitForSeconds(0.2f);
//            ResetClimbJump();

//        }
//        private void ResetClimbJump()
//        {
//            climbSpeed = climbSpeedTmp;
//            onClimbJump = false;
//            restricted = false;
//        }

//        private void StateClimbing()
//        {
//            if (wallClose && wallLookAngle < maxWallLookAngle && wallVerticalAngle <= 100)
//            {
//                if (movementInput.y > 0 && !climbing && !restricted && climbable)
//                {
//                    StartClimbing();
//                }
//                wallTopReach = Physics.Raycast(transform.position + offsetDetecting + transform.up * 0.6f, transform.forward, out topwallHit, detectionLength, whatIsWall);

//                //Debug.DrawRay(transform.position + transform.forward * -1f + transform.up * 1.5f, transform.forward * detectionLength, Color.magenta, 0f);

//                if (wallTopReach && !wallLerping)
//                {
//                    forceReaching = true;

//                    if (!topWall)
//                    {
//                        topWall = new GameObject().transform;
//                        topWall.name = "Topwall Handler";
//                    }

//                    topWall.position = topwallHit.point + transform.forward * 2f + transform.up * 0.1f;
//                }
//            }

//            else
//            {
//                if (climbing && !jumpOff && !wallClose)
//                {
//                    StopClimbing();
//                }
//            }
//            //Debug.Log(wallTopReach.ToString() + forceReaching.ToString() + wallClose.ToString());
//            if (!wallTopReach && forceReaching && wallClose && topreachable)
//            {
//                Player.Rigidbody.AddForce(Vector3.up * 45f, ForceMode.Force);
//                Player.CapsuleCollider.height = 0.5f;
//                //playerCollider.center = new Vector3(0, 1f, 0);
//                climbDirection = Vector3.zero;
//                restricted = true;
//                topCheck = true;

//            }

//            if (topCheck && !wallClose)
//            {
//                ResetVelocity();
//                Invoke(nameof(StopClimbing), 0.25f);
//                forceReaching = false;
//                wallLerping = true;

//                topCheck = false;
//            }

//            if (Stamina <= 0 && climbing)
//            {
//                StopClimbing();
//                ResetVelocity();
//                Player.Rigidbody.useGravity = true;
//                restricted = true;

//            }
//            if (grounded && movementInput.y < 0 && climbing)
//            {
//                StopClimbing();
//                Vector3 originAngle = transform.rotation.eulerAngles;
//                transform.rotation = Quaternion.Euler(0, originAngle.y, originAngle.z);
//            }

//        }
//        private IEnumerator LerpingWallTop(Transform topwallPo)
//        {
//            transform.position = Vector3.Lerp(transform.position, topwallPo.position, 20f * Time.fixedDeltaTime);
//            wallLerping = false;
//            lerpCoroutine = true;
//            yield return new WaitForSeconds(1f);
//            Player.CapsuleCollider.height = 1.8f;
//            StopClimbing();
//        }
//        private void PlayerInit()
//        {
//            readyToJump = true;
//        }

//        #region Player Movement
//        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//        public void SetSpeedForUsingConsumable()
//        {
//            //playerMovementSpeed = 4;
//            walkSpeed = 2;
//            Player.Animator.speed = 0.1f;
//        }
//        public void SetSpeedForUsingConsumableBack()
//        {
//            //playerMovementSpeed = Player.PlayerOutlander.MoveSpeed;
//            walkSpeed = Player.PlayerOutlander.MoveSpeed;
//            Player.Animator.speed = 1f;
//        }
//        public void SetSpeedPlayer(float speed)
//        {
//            walkSpeed = speed;
//            sprintSpeed = speed * 2;
//        }
//        private void AdjustSpeedPlayer()
//        {
//            if (!isSprint)
//            {
//                playerMovementSpeed = walkSpeed;
//                if (!reachMaxStamina) rechargeStamina = true;

//                if (onCrouch)
//                {
//                    playerMovementSpeed -= 1;
//                }
//            }

//            else if (isSprint && reachMinStamina)
//            {
//                if (highground || swimming)
//                {
//                    if (swimming) playerMovementSpeed = sprintSpeed - 3;

//                    else
//                    {
//                        playerMovementSpeed = sprintSpeed;
//                    }

//                    if (movementInput.x != 0 || movementInput.y != 0)
//                    {
//                        if (PlayerManagers.Instance.matchManager.canInteract)
//                            Stamina -= 4 * Time.deltaTime;
//                        rechargeStamina = false;
//                    }

//                    else
//                    {
//                        if (!reachMaxStamina)
//                        {
//                            if (!rechargeStamina)
//                                rechargeCooldown = 2f;
//                            rechargeStamina = true;
//                        }
//                    }
//                }
//                if (onCrouch)
//                    SetCrouchState(false);
//            }
//        }
//        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//        private void MovePlayer()
//        {
//            if (restricted || dashing || stunned) return;

//            if (IsAction())
//                ResetRechargeCooldown();

//            if (Player.PlayerOutlander.GetWeaponAction() && !Player.PlayerOutlander.IsAiming)
//            {
//                if (onJump) onJump = false;
//                if (!Player.Rigidbody.useGravity) Player.Rigidbody.useGravity = true;
//                if (onCrouch) SetCrouchState(false);
//                return;
//            }

//            AdjustSpeedPlayer();

//            JumpHandler();

//            if (isSwim)
//            {
//                if (onCrouch)
//                    SetCrouchState(false);
//                Player.Rigidbody.useGravity = false;
//                Player.Rigidbody.AddForce(Vector3.up * 50f, ForceMode.Force);
//            }
//            //cal move direction
//            moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;
//            if (OnSlope() && !exitingSlope && !climbing && !onJump && grounded && !swimming)
//            {
//                if (!dashing)
//                {
//                    float debuff = weightDebuffSpeed;
//                    float speedLimit = playerMovementSpeed - debuff < 0 ? 1 : playerMovementSpeed - debuff;
//                    Player.Rigidbody.AddForce(GetslopeMove() * speedLimit * 25f, ForceMode.Force);
//                }
//                if (Player.Rigidbody.velocity.y > 0 || dashing)
//                {
//                    Player.Rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
//                }
//            }

//            //onground
//            if (!climbing)
//            {
//                if (grounded)
//                {
//                    if (onCrouch)
//                    {
//                        Player.CapsuleCollider.center = new Vector3(0, 0.5f, 0);
//                        Player.CapsuleCollider.height = 1;
//                    }
//                    else
//                    {
//                        Player.CapsuleCollider.height = 1.8f;
//                        Player.CapsuleCollider.center = new Vector3(0, 0.9f, 0);
//                    }

//                    if (!swimming)
//                    {
//                        Player.Rigidbody.useGravity = (!OnSlope() && isMoving);
//                        if (OnSlope())
//                        {
//                            if (isMoving)
//                            {
//                                Player.Rigidbody.useGravity = true;
//                            }
//                            else
//                            {
//                                Player.Rigidbody.useGravity = false;
//                            }
//                        }
//                        else
//                        {
//                            Player.Rigidbody.useGravity = true;
//                        }

//                        if (jumpCooldownTimer <= 0)
//                        {
//                            readyToJump = true;
//                        }
//                    }

//                    else
//                    {
//                        if (isMoving)
//                        {
//                            if (PlayerManagers.Instance.matchManager.canInteract)
//                                Stamina -= 2 * Time.deltaTime;
//                        }

//                        rechargeStamina = false;
//                        readyToJump = false;
//                    }

//                    if (topWall && wallLerping)
//                    {
//                        ResetVelocity();
//                        Player.Rigidbody.useGravity = true;
//                        Destroy(unstuckpoint);
//                        StopCoroutine(wallLerp);
//                        wallLerping = false;
//                    }
//                    if (OnSlope() && !exitingSlope)
//                    {
//                        Player.Rigidbody.AddForce(moveDirection.normalized * playerMovementSpeed, ForceMode.Force);
//                    }
//                    else
//                    {
//                        Player.Rigidbody.AddForce(moveDirection.normalized * playerMovementSpeed * 10f, ForceMode.Force);
//                    }
//                    jumpFromWall = false;

//                    if (jumpOff)
//                    {
//                        transform.forward = -transform.forward;
//                        jumpOff = false;
//                        restricted = false;
//                    }
//                    if (rechargeStamina)
//                    {
//                        rechargeCooldown -= Time.deltaTime;
//                        if (isMoving)
//                            if (rechargeCooldown <= 0f)
//                                Stamina += 15 * Time.deltaTime;
//                            else
//                                Stamina += 10 * Time.deltaTime;
//                        else
//                            if (rechargeCooldown <= 0f)
//                            Stamina += 20 * Time.deltaTime;
//                        else
//                            Stamina += 10 * Time.deltaTime;
//                    }
//                }
//                else if (!grounded)
//                {
//                    Player.Rigidbody.AddForce(moveDirection.normalized * playerMovementSpeed * 10f * airMultiplier, ForceMode.Force);
//                    if (swimming)
//                    {
//                        rechargeStamina = false;
//                        if (onCrouch)
//                            SetCrouchState(false);
//                        if (isMoving)
//                        {
//                            if (PlayerManagers.Instance.matchManager.canInteract)
//                                Stamina -= 2 * Time.deltaTime;
//                        }

//                    }
//                    else
//                        Player.Rigidbody.useGravity = true;
//                    if (isSwim)
//                    {
//                        Player.Rigidbody.useGravity = false;
//                        swimming = true;
//                    }
//                }
//            }

//            if (swimming && Stamina <= 0 && !restricted)
//            {
//                drowned = true;
//                restricted = true;
//                //transform.GetComponent<IPlayer>().Die = true;
//                //transform.GetComponent<IPlayer>().Health = 0;
//                Player.PlayerOutlander.CmdDamageIgnoreDefense("DROWN", float.MaxValue);
//            }

//            if (movementInput.x != 0 || movementInput.y != 0)
//            {
//                isMoving = true;
//            }

//            else
//            {
//                isMoving = false;
//            }
//        }

//        private void StaminaController()
//        {
//            float updatestaminaBar = staminaBarLength * Stamina / maxStamina;
//            UIManagers.Instance.playerCanvas.staminaBar.rectTransform.sizeDelta = new Vector2(updatestaminaBar, UIManagers.Instance.playerCanvas.staminaBar.rectTransform.sizeDelta.y);

//            if (Stamina >= maxStamina)
//            {
//                Stamina = maxStamina;
//                reachMaxStamina = true;
//                rechargeStamina = false;
//            }
//            else if (Stamina < maxStamina) reachMaxStamina = false;

//            if (Stamina <= 0 && reachMinStamina)
//            {
//                Stamina = 0;
//                reachMinStamina = false;
//            }

//            if (!reachMinStamina)
//            {
//                onJump = false;
//                onClimbJump = false;
//                isSprint = false;
//                if (Stamina >= minStamina)
//                    reachMinStamina = true;
//            }
//        }

//        public void ResetVelocity()
//        {
//            Player.Rigidbody.velocity = Vector3.zero;
//            Player.Rigidbody.angularVelocity = Vector3.zero;
//        }

//        private void OnCollisionEnter(Collision collision)
//        {
//            if (collision == null)
//            {
//                return;
//            }

//            if (swimming && collision.gameObject.layer == LayerMask.NameToLayer("Ground") && grounded)
//            {
//                Player.AnimationScript.SetRigWeight(1);
//                swimming = false;
//            }


//        }
//        private void OnTriggerEnter(Collider other)
//        {
//            if (other.gameObject.CompareTag("Border") && !exitBordering)
//            {
//                borderTimer = 10f;
//                exitBordering = true;
//            }
//        }

//        private void OnTriggerExit(Collider other)
//        {
//            if (other.gameObject.CompareTag("Border") && exitBordering)
//            {
//                borderText.alpha = 0;
//                exitBordering = false;
//            }
//        }

//        private void ResetSkillState()
//        {
//            Player.PlayerOutlander.skilling = false;
//        }
//        private void PlayerSpeedControl()
//        {
//            //Weight
//            if (Player.InventoryManager.TotalWeight > Player.InventoryManager.MaxWeight)
//            {
//                float total = Player.InventoryManager.TotalWeight;
//                float max = Player.InventoryManager.MaxWeight;

//                //if (total > max * 1.2f)
//                if (total >= max * 1.2f)
//                {
//                    if (!climbing) Player.Animator.speed = 0.5f;
//                    weightDebuffSpeed = 3;
//                }
//                //else if (total > max * 1.1f)
//                else if (total >= max * 1.1f)
//                {
//                    if (!climbing) Player.Animator.speed = 0.7f;
//                    weightDebuffSpeed = 2;
//                }
//                //else if (total > max)
//                else if (total > max)
//                {
//                    if (!climbing) Player.Animator.speed = 0.9f;
//                    weightDebuffSpeed = 1;
//                }

//            }

//            else if (weightDebuffSpeed != 0 && Player.InventoryManager.TotalWeight <= Player.InventoryManager.MaxWeight)
//            {
//                Player.Animator.speed = 1;
//                weightDebuffSpeed = 0;
//            }

//            //State not walking

//            if (swimming)
//            {
//                float debuff = 2 + weightDebuffSpeed;
//                float speedLimit = isSprint ? sprintSpeed - debuff : walkSpeed - debuff;
//                if (Player.Rigidbody.velocity.magnitude > speedLimit)
//                {
//                    if (speedLimit < 0 && highground)
//                    {
//                        Player.Rigidbody.velocity = Vector3.zero;
//                        return;
//                    }
//                    Player.Rigidbody.velocity = Player.Rigidbody.velocity.normalized * speedLimit;
//                }

//                if (startFalling)
//                {
//                    startFalling = false;
//                }
//            }

//            else if (dashing)
//            {
//                if (dashSpeed - weightDebuffSpeed < 0 && highground)
//                {
//                    Player.Rigidbody.velocity = Vector3.zero;
//                    return;
//                }
//                Player.Rigidbody.velocity = Player.Rigidbody.velocity.normalized * (dashSpeed - weightDebuffSpeed);
//                if (Player.Rigidbody.velocity.y > 0)
//                    Player.Rigidbody.velocity = new Vector3(Player.Rigidbody.velocity.x, 0, Player.Rigidbody.velocity.z);
//                if (!highground) ResetDodge();
//                //Player.Rigidbody.velocity = new Vector3(Player.Rigidbody.velocity.x, 0, Player.Rigidbody.velocity.z);
//            }

//            else if (freeze)
//            {
//                Player.Rigidbody.velocity = Vector3.zero;
//            }

//            else if (unlimited)
//            {

//                Player.Rigidbody.velocity = (Player.Rigidbody.velocity * 999f).normalized;

//                return;
//            }
//            else if (climbing)
//            {
//                if (Player.Rigidbody.velocity.magnitude > climbSpeed - weightDebuffSpeed)
//                {
//                    if (climbSpeed - weightDebuffSpeed < 0 && highground)
//                    {
//                        Player.Rigidbody.velocity = Vector3.zero;
//                        return;
//                    }
//                    Player.Rigidbody.velocity = Player.Rigidbody.velocity.normalized * (climbSpeed - weightDebuffSpeed);
//                }
//            }

//            else //Walk
//            {
//                float debuff = weightDebuffSpeed;
//                float speedLimit;

//                speedLimit = playerMovementSpeed - debuff < 0 ? 1 : playerMovementSpeed - debuff;

//                if (OnSlope() && !exitingSlope && grounded)
//                {
//                    if (Player.Rigidbody.velocity.magnitude > speedLimit)
//                    {
//                        Player.Rigidbody.velocity = Player.Rigidbody.velocity.normalized * speedLimit;
//                    }
//                }
//                else
//                {
//                    Vector3 flatVel = new Vector3(Player.Rigidbody.velocity.x, 0f, Player.Rigidbody.velocity.z);

//                    if (flatVel.magnitude > speedLimit)
//                    {
//                        if (speedLimit < 0 && highground)
//                        {
//                            Player.Rigidbody.velocity = Vector3.zero;
//                            return;
//                        }
//                        Vector3 limitedVel = flatVel.normalized * speedLimit;
//                        Player.Rigidbody.velocity = new Vector3(limitedVel.x, Player.Rigidbody.velocity.y, limitedVel.z);
//                    }
//                }
//                if (!grounded)
//                {
//                    if (Player.Rigidbody.velocity.y > 7)
//                    {

//                        Player.Rigidbody.velocity = new Vector3(Player.Rigidbody.velocity.x, 7f, Player.Rigidbody.velocity.z);
//                    }
//                }

//            }

//        }

//        private void Jump()
//        {
//            if (jumpCooldownTimer > 0)
//            {
//                return;
//            }
//            else
//            {
//                jumpCooldownTimer = jumpCooldown;
//                readyToJump = false;
//            }
//            exitingSlope = true;
//            if (PlayerManagers.Instance.matchManager.canInteract)
//                Stamina -= 20f;
//            Player.Rigidbody.velocity = new Vector3(Player.Rigidbody.velocity.x, 0f, Player.Rigidbody.velocity.z);
//            jumpForce = OnSlope() ? 8 : 10;
//            Player.Rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
//        }
//        public void ResetStage()
//        {
//            swimming = false;
//            wallLerping = false;
//            inWater = false;
//            StopClimbing();
//            Stamina = maxStamina;
//            stunned = false;
//            dashing = false;
//            isSprint = false;
//            restricted = false;
//            onClimbJump = false;
//            onJump = false;
//            drowned = false;
//            startFalling = false;
//            Player.Animator.Play("Idle");
//            SetSpeedForUsingConsumableBack();
//        }

//        public void ResetStageUnstuck()
//        {
//            StopClimbing();
//            wallLerping = false;
//            isSwim = false;
//            inWater = false;
//            swimming = false;
//            stunned = false;
//            dashing = false;
//            isSprint = false;
//            restricted = false;
//            onClimbJump = false;
//            onJump = false;
//            drowned = false;
//            startFalling = false;
//            Player.Animator.Play("Idle");
//            SetSpeedForUsingConsumableBack();
//            ResetSkillState();
//        }

//        private void Dodge()
//        {
//            if (dashCdTimer > 0)
//            {
//                return;
//            }

//            else
//            {
//                dashCdTimer = dashCd;
//            }

//            dashing = true;

//            Vector3 forceToApply;

//            forceToApply = transform.forward * dashForce + orientation.up * dashUpwardForce;

//            /*            if (isSprint) isSprint = false;
//            */
//            delayedForceToApple = forceToApply;

//            Invoke(nameof(DelayedDashForce), 0.1f);

//            //Invoke(nameof(ResetDodge), 0.75f);
//        }

//        private void DelayedDashForce()
//        {
//            ResetVelocity();
//            Player.Rigidbody.AddForce(delayedForceToApple, ForceMode.Impulse);
//            if (PlayerManagers.Instance.matchManager.canInteract)
//                Stamina -= 20f;
//        }

//        private void ResetDodge()
//        {
//            if (!isLocalPlayer) return;
//            dashing = false;
//            if (onCrouch)
//                SetCrouchState(false);
//            Player.AnimationScript.SetRigWeight(1);
//        }
//        private void ResetJump()
//        {
//            exitingSlope = false;
//            onJump = false;
//        }

//        private bool OnSlope()
//        {
//            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, whatIsGround))
//            {
//                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
//                if (angle < maxSlopeAngle && angle != 0 && angle > 20)
//                    return angle < maxSlopeAngle && angle != 0 && angle > 20;
//            }
//            return false;
//        }
//        private Vector3 GetslopeMove()
//        {
//            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
//        }
//        #endregion


//        #region Input System
//        private void EnableInput()
//        {
//            Player.PlayerInput.enabled = ENABLE_INPUT_SYSTEM;
//        }

//        private void JumpHandler()
//        {
//            if (onJump && !climbing & grounded && !(jumpCooldownTimer > 0))
//            {
//                Invoke(nameof(Jump), 0.025f);

//                Invoke(nameof(ResetJump), jumpCooldown);

//            }
//        }

//        //[Client]
//        public void OnMovement(InputValue value)
//        {
//            if (enabled)
//                movementInput = value.Get<Vector2>();
//        }
//        //[Client]
//        public void OnJump(InputValue value)
//        {
//            if (readyToJump && grounded && !restricted && !Player.PlayerOutlander.skilling && percentage < 150 && !dashing && reachMinStamina && !swimming && !isSwim && !Player.PlayerOutlander.GetWeaponAction())
//            //if (readyToJump && grounded && !restricted && !skilling && percentage < 150 && !dashing && reachMinStamina && !swimming && !isSwim)
//            {
//                if (onCrouch)
//                    SetCrouchState(false);
//                onJump = value.isPressed;
//            }
//            if (climbing && !restricted && !Player.PlayerOutlander.skilling && !isMoving)
//            {
//                onClimbJump = value.isPressed;
//            }
//        }
//        public void OnSprint(InputValue value)
//        {
//            //if (movementInput != Vector2.zero)
//            if (!climbing || swimming)
//            {
//                if(onCrouch)
//                    SetCrouchState(false);
//                isSprint = value.isPressed;
//            }
//        }

//        public void SetCrouchState(bool state)
//        {
//            if (isServer) return;
//            onCrouch = state;
//            Player.PlayerAppearance.rightWeaponGO.SetActive(!state);
//            Player.PlayerAppearance.leftWeaponGO.SetActive(!state);
//            CmdSetCrouchState(state);
//        }
//        [Command] private void CmdSetCrouchState(bool state)
//        {
//            Player.Animator.SetBool("onCrouch", state);
//            RpcSetCrouchState(state);
//        }
//        [ClientRpc]
//        public void RpcSetCrouchState(bool state)
//        {
//            onCrouch = state;
//            Player.Animator.SetBool("onCrouch", state);
//            Player.PlayerAppearance.rightWeaponGO.SetActive(!state);
//            Player.PlayerAppearance.leftWeaponGO.SetActive(!state);
//        }
//        public void OnDodge(InputValue value)
//        {
//            if (climbing || restricted || Player.PlayerOutlander.skilling || swimming || onJump || Player.PlayerOutlander.GetWeaponAction()) return;

//            if (isMoving)
//            {
//                if (grounded && reachMinStamina)
//                {
//                    Dodge();
//                    SetCrouchState(false);
//                }
//            }
//            else
//            {
//                if(onCrouch)
//                {
//                    SetCrouchState(false);
//                    Player.AnimationScript.SetRigWeight(1);
//                }
//                else
//                {
//                    SetCrouchState(true);
//                    Player.AnimationScript.SetRigWeight(0);
//                }
//            }
//        }

//        public void OnLook(InputValue value)
//        {
//            lookDir = value.Get<Vector2>();
//        }

//        public void OnEnableMouse(InputValue value)
//        {
//            //Debug.Log($"OnEnableMouse {value.isPressed}");

//            // if (!Cursor.visible) return;

//            CursorManager.Instance.alt_interact = !CursorManager.Instance.alt_interact;
//        }

//        [Client]
//        protected virtual void OnUnstuck(InputValue value)
//        {
//            if (CursorManager.Instance.unstuck)
//            {
//                UnDisplayStuckPanel();
//            }
//            else
//            {
//                DisplayUnstuckPanel();
//            }
//        }

//        public void DisplayUnstuckPanel()
//        {
//            if (CursorManager.Instance.selectmap || CursorManager.Instance.lobby || CursorManager.Instance.option || CursorManager.Instance.login || CursorManager.Instance.summary || CursorManager.Instance.loading || GetComponent<PlayerUIManager>().unStuckDelay > 0) return;
            
//            UIManagers.Instance.uiWarning.ShowDoubleWarningButton("Would you like to unstuck\nyour Character ?", EnableUnStuckPanel, UnDisplayStuckPanel);
//            CursorManager.Instance.unstuck = true;

//            UIManagers.Instance.uiTutorial.Hide();
//            Player.PlayerUIManager.OnPlayerCloseMap();
//            Player.InventoryManager.CloseInventory();
//            UIManagers.Instance.playerCanvas.unstuckButton.interactable = false;
//        }

//        public void UnDisplayStuckPanel()
//        {
//            CursorManager.Instance.unstuck = false;
//            UIManagers.Instance.playerCanvas.unstuckButton.interactable = true;
//            UIManagers.Instance.uiWarning.CancelWarning();
//        }

//        public void EnableUnStuckPanel()
//        {
//            GetComponent<PlayerUIManager>().unStuckDelay = 2f;
//            //transform.position = GameObject.FindGameObjectWithTag("Bonfire").transform.position;
//            //PlayerSpawnManager spawnPoint = GetComponent<PlayerSpawnManager>();
//            //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(spawnPoint.mapName))
//            //transform.position = spawnPoint.spawnpoint;
//            //else
//            //LoadSceneManager.singleton.OnClientSpawnMoveScene(spawnPoint.mapName, spawnPoint.spawnpoint, gameObject);

//            //GameBattleRoyalManager.singleton.isStart = false;
//            //GameBattleRoyalManager.singleton.SendPlayersMoveScene();
//            //RaycastHit hit;

//            //if (Physics.Raycast(new Vector3(transform.position.x, 100, transform.position.z), Vector3.down, out hit, 3000))
//            //{
//            transform.position += new Vector3(0, 0.1f, 0);
//            //}

//            ResetVelocity();
//            Player.Animator.Play("Idle");

//            ResetStageUnstuck();

//            UnDisplayStuckPanel();
//        }

//        #endregion

//        #region skill movement
//        public void movementforward()
//        {
//            Vector3 forceToApply;
//            forceToApply = transform.forward * dashForce * 2 + orientation.up * dashUpwardForce;
//            delayedForceToApple = forceToApply;
//            Player.Rigidbody.AddForce(delayedForceToApple, ForceMode.Impulse);
//        }

//        public void movementup()
//        {
//            //Player.Rigidbody.AddForce(transform.up * (jumpForce/3), ForceMode.Impulse);
//        }
//        #endregion
//    }
//}
