//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using Mirror;
//using Outlander.UI;
//using UnityEngine.UI;
//using Outlander.Network;
//// using System;

//namespace Outlander.Player
//{
//    public class PlayerOutlander : PlayerElements, IPlayer
//    {
//        #region Handler
//        [Header("Handler")]
//        [SerializeField] private GameObject playerObjectRef;
//        [SerializeField] private GameObject deadBox;
//        private GameObject arrowObject;

//        public delegate void SkillInputHandler(int index);
//        public SkillInputHandler SkillAction;

//        public delegate void SkillHandler();
//        public SkillHandler SkillHandler_1;
//        public SkillHandler SkillHandler_2;
//        public SkillHandler SkillHandler_3;
//        public SkillHandler SkillHandler_4;
//        // public SkillHandler AttackHandler;

//        public delegate void ResetSkillHandler();
//        public ResetSkillHandler ResetSkillActionHandler;

//        #endregion

//        #region Var
//        [Header("InputActions")]
//        //private PlayerMovement playerMovement;
//        private PlayerInputAction playerInputAction;

//        // [HideInInspector] public Vector2 movementInput = Vector2.zero;
//        private bool onFire = false;
//        // private bool onDodge = false;
//        // private bool onJump = false;
//        private bool onPickUp = false;
//        private bool onSkill = false;
//        // private bool onSprint = false;
//        private bool onUlt = false;
//        [SyncVar, SerializeField, ReadOnly] private bool onWeaponAction = false;
//        [SyncVar, SerializeField, ReadOnly] private bool onGodmode = false;
//        private bool onChat = false;
//        [SerializeField, ReadOnly] private bool onInteract = false;

//        // private bool onEscape = false;
//        private bool onLeftItem = false;
//        private bool onRightItem = false;

//        // [HideInInspector] public Vector2 lookDir;
//        // public static bool ENABLE_INPUT_SYSTEM = true;


//        [Header("Animator")]// change animator //
//        [SerializeField] private RuntimeAnimatorController runtimeAnimator;
//        [SerializeField, ReadOnly] public bool skilling = false;
//        [HideInInspector] public bool pickingup = false;
//        [HideInInspector] public bool attacking = false;


//        [Header("Attributes")]
//        public LayerMask enemyLayer;
//        public Collider attackPoint;
//        //public Collider attackCollider;
//        private Transform shootingPoint;
//        public float attackRange = 1f;
//        private float currentAtkDmg;
//        [SyncVar, SerializeField] private float atkDamage;
//        [SyncVar, SerializeField] private float def;
//        [SyncVar, SerializeField] private float health;
//        [SyncVar, SerializeField] private float mana;
//        [SyncVar, SerializeField] private float maxHealth;
//        [SyncVar, SerializeField] private float maxMana;
//        [SyncVar, SerializeField] private float crit;
//        [SyncVar, SerializeField] private float critDamage;
//        [SyncVar, SerializeField] private float attackSpeed;
//        [SyncVar, SerializeField] private float moveSpeed;
//        public GameObject hitFX;
//        [SerializeField] public Queue<float> damageBufferLog = new Queue<float>();
//        [SerializeField] public Queue<GameObject> enemyBufferLog = new Queue<GameObject>();
//        [SyncVar] private CharacterScriptable character;
//        private int skillIndex = 0;

//        [Header("PlayerInfo")]
//        //[SerializeField] private UIPlayerInfo uiPlayerInfo;
//        //[SerializeField] private PlayerUIManager playerUI;
//        [SerializeField] private DamagePopUp damagePopUp;

//        //private int killCount = 0;
//        //public static event Action<int> onKillCountChange;

//        [HideInInspector] public Transform spawnpoint;
//        private GameObject crosshair;


//        [Header("Combat")]
//        // private Rigidbody playerRigidbody;
//        // private bool comboPossible;
//        // private int comboStep;
//        private bool isHit;

//        private bool isAiming = false;
//        private bool hitting = false;
//        private bool isCounter = false;
//        private LayerMask playerLayer;
//        private Vector3 targetDirection;
//        [SyncVar] private bool die = false;
//        private Vector3 targetObject;

//        // public float cooldownTime = 2f;
//        // private float nextFireTime = 0f;
//        // public static int noOfClicks = 0;
//        // float lastClickedTime = 0;
//        // float maxComboDelay = 1;

//        private bool canReceiveFireInput = true;

//        // [Header("Chat")]
//        // [SerializeField] private TMP_InputField chatInputField = null;
//        // private bool chatMode;
//        #endregion


//        public override void OnStartAuthority()
//        {
//            base.OnStartAuthority();

//            PlayerInput playerInput = GetComponent<PlayerInput>();
//            playerInput.enabled = true;
//        }

//        private void OnEnable()
//        {
//            if (!isLocalPlayer) return;
//            playerInputAction.Enable();

//            Player.PlayerAnimationEvent.OnAttackHit += AttackHit;
//            // playerAnimationEvent.OnCombo += Combo;
//            // playerAnimationEvent.OnComboPossible += ComboPossible;
//            // playerAnimationEvent.OnComboReset += ComboReset;
//            Player.PlayerAnimationEvent.OnEndSkill += EndSkill;
//            Player.PlayerAnimationEvent.OnResetStage += ResetStage;
//            Player.PlayerAnimationEvent.OnIdle += OnIdle;
//            Player.PlayerAnimationEvent.OnShooting += ShootingObjectAction;

//            Player.PlayerUIActionHandler.OnUILeftItem += LeftItem;
//            Player.PlayerUIActionHandler.OnUIRightItem += RightItem;
//        }

//        private void OnDisable()
//        {
//            if (!isLocalPlayer) return;
//            playerInputAction.Disable();

//            Player.PlayerAnimationEvent.OnAttackHit -= AttackHit;
//            // playerAnimationEvent.OnCombo -= Combo;
//            // playerAnimationEvent.OnComboPossible -= ComboPossible;
//            // playerAnimationEvent.OnComboReset -= ComboReset;
//            Player.PlayerAnimationEvent.OnEndSkill -= EndSkill;
//            Player.PlayerAnimationEvent.OnResetStage -= ResetStage;
//            Player.PlayerAnimationEvent.OnIdle -= OnIdle;
//            Player.PlayerAnimationEvent.OnShooting -= ShootingObjectAction;

//            Player.PlayerUIActionHandler.OnUILeftItem -= LeftItem;
//            Player.PlayerUIActionHandler.OnUIRightItem -= RightItem;
//        }

//        /*public void OnDestroyPlayer(string scene)
//        {
//            character.location.mapName = scene;

//            CharacterDestroyMsg characterDestroyMsg = new CharacterDestroyMsg
//            {
//                player = gameObject,
//                mapName = scene
//            };
//            NetworkClient.Send(characterDestroyMsg);

//            //NetworkClient.DestroyAllClientObjects();
//        }*/

//        #region Encapsulate Field Properties    
//        public float Health { get => health; set => health = value; }
//        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
//        public float Mana { get => mana; set => mana = value; }
//        public float MaxMana { get => maxMana; set => maxMana = value; }
//        public float AttackDamage { get => atkDamage; set => atkDamage = value; }
//        public float CurAttackDamage { get => currentAtkDmg; set => currentAtkDmg = value; }
//        public float Defense { get => def; set => def = value; }
//        public float Crit { get => crit; set => crit = value; }
//        public float CritDamage { get => critDamage; set => critDamage = value; }
//        public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
//        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
//        // public Transform Spawnpoint { get => spawnpoint; set => spawnpoint = value; }
//        public bool Die { get => die; set => die = value; }
//        public RuntimeAnimatorController RuntimeAnimator { get => runtimeAnimator; set => runtimeAnimator = value; }
//        public bool OnInteract { get => onInteract; set => onInteract = value; }
//        public CharacterScriptable Character { get => character; set => character = value; }
//        public int SkillIndex { get => skillIndex; set => skillIndex = value; }
//        public bool Skilling { get => skilling; set => skilling = value; }
//        public GameObject PlayerObjectRef { get => playerObjectRef; set => playerObjectRef = value; }
//        public GameObject ArrowObject { get => arrowObject; set => arrowObject = value; }
//        public Transform ShootingPoint { get => shootingPoint; set => shootingPoint = value; }
//        public bool CanReceiveFireInput { get => canReceiveFireInput; set => canReceiveFireInput = value; }
//        public bool OnFireInput { get => onFire; set => onFire = value; }
//        public bool IsHit { get => isHit; }
//        public bool IsCounter { get => isCounter; set => isCounter = value; }
//        public GameObject Crosshair { get => crosshair; set => crosshair = value; }
//        public bool IsAiming { get => isAiming; set => isAiming = value; }
//        public Vector3 TargetDirection { get => targetDirection; private set => targetDirection = value; }
//        public Quaternion TargetRotation { get; set; }
//        public Vector3 TargetObject { get => targetObject; private set => targetObject = value; }

//        public bool GetCursorCheck() => Cursor.visible;

//        #endregion

//        #region PlayerHP

//        [Server, TargetRpc]
//        private void RpcSetPlayerHealth(float newHealth)
//        {
//            health = newHealth;
//        }

//        [Command]
//        private void CmdSetPlayerHealth(float newHealth)
//        {
//            health = newHealth;
//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerMana(float newMana)
//        {
//            mana = newMana;
//        }

//        [Command]
//        private void CmdSetPlayerMana(float newMana)
//        {
//            mana = newMana;
//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerMaxHealth(float newMaxHealth)
//        {
//            MaxHealth = newMaxHealth;
//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerMaxMana(float newMaxMana)
//        {
//            MaxMana = newMaxMana;
//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerAttackDamage(float newAttackDamage)
//        {
//            atkDamage = newAttackDamage;
//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerDefense(float newDefense)
//        {
//            def = newDefense;
//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerCrit(float newCrit)
//        {
//            crit = newCrit;
//        }

//        [ClientRpc]
//        public void RpcSetDamageBuffer(float damageAmount)
//        {
//            damageBufferLog.Enqueue(damageAmount);
//            //Debug.Log($"Rpc Queue Damage : {damageAmount} Player: {this.gameObject.name}");
//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerAttackSpeed(float newAttackSpeed)
//        {
//            attackSpeed = newAttackSpeed;
//            //anim.SetFloat("speedModify", attackSpeed);

//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerMoveSpeed(float newMoveSpeed)
//        {
//            moveSpeed = newMoveSpeed;
//            Player.PlayerMovement.SetSpeedPlayer(moveSpeed);
//        }

//        [Server, TargetRpc]
//        private void RpcSetPlayerFloatAttribute(float _value, float _newValue)
//        {
//            _value = _newValue;
//        }

//        [Command]
//        public void CmdSetDamageBuffer(float damageAmount)
//        {
//            RpcSetDamageBuffer(damageAmount);
//            // damageBufferLog.Enqueue(damageAmount);
//            //Debug.Log($"Cmd Queue Damage : {damageAmount} Player: {this.gameObject.name}");
//        }

//        [Command]
//        public void CmdSetWeaponAction(bool newWeaponaction)
//        {
//            onWeaponAction = newWeaponaction;
//        }

//        // [Command]
//        // private void CmdSetGodmode(bool newGodmode)
//        // {
//        //     onGodmode = newGodmode;
//        // }

//        public void ShootingObjectAction(float _speed)
//        {
//            //Debug.Log($"StarPlatinumZaWarudo");
//            if (Player.PlayerMovement.Stamina <= 20) return;
//            Player.PlayerMovement.Stamina -= 20;

//            CmdShootingAction(TargetObject, TargetDirection, TargetRotation, _speed);
//        }

//        [Command(requiresAuthority = false)]
//        private void CmdShootingAction(Vector3 _target, Vector3 _dir, Quaternion _rot, float _speed)
//        {
//            GameObject shootingObject = Instantiate(ArrowObject, ShootingPoint.position, _rot);

//            shootingObject.GetComponent<NetworkMatch>().matchId = GetComponent<IPlayerMatch>().MatchID.ToGuid();

//            switch (true)
//            {
//                case true when shootingObject.TryGetComponent(out Outlander.Weapon.BaseArrow baseArrow):
//                    baseArrow.PlayerOwner = playerObjectRef;
//                    baseArrow.ArrowDamage = AttackDamage;
//                    baseArrow.ArrowDirection = _dir;
//                    baseArrow.ArrowSpeed = _speed;
//                    baseArrow.ArrowCrit = Crit;
//                    baseArrow.ArrowCritDamage = CritDamage;
//                    // baseAoESpell.TargetObject = _target;
//                    baseArrow.Skilling = skilling;

//                    // baseArrow.transform.position = _target;
//                    break;
//                case true when shootingObject.TryGetComponent(out Outlander.Weapon.BaseAoEMagic baseAoESpell):
//                    baseAoESpell.PlayerOwner = playerObjectRef;
//                    baseAoESpell.SpellDamage = AttackDamage;
//                    baseAoESpell.SpellDirection = _dir;
//                    baseAoESpell.SpellSpeed = _speed;
//                    baseAoESpell.SpellCrit = Crit;
//                    baseAoESpell.SpellCritDamage = CritDamage;
//                    // baseAoESpell.TargetObject = _target;
//                    baseAoESpell.Skilling = skilling;

//                    baseAoESpell.transform.position = _target;
//                    break;
//            }

//            // Weapon.BaseArrow baseArrow = shootingObject.GetComponent<Outlander.Weapon.BaseArrow>();

//            // baseArrow.PlayerOwner = playerObjectRef;
//            // baseArrow.ArrowDamage = AttackDamage;
//            // baseArrow.ArrowDirection = _dir;
//            // baseArrow.ArrowSpeed = _speed;
//            // baseArrow.ArrowCrit = Crit;
//            // baseArrow.Skilling = skilling;

//            NetworkServer.Spawn(shootingObject);
//        }

//        // [Command(requiresAuthority = false)]
//        // private void CmdShootingAction(Vector3 _dir, Quaternion _rot, float _speed)
//        // {
//        //     GameObject shootingObject = Instantiate(ArrowObject, ShootingPoint.position, _rot);

//        //     switch (true)
//        //     {
//        //         case true when shootingObject.TryGetComponent<Outlander.Weapon.BaseArrow>(out Outlander.Weapon.BaseArrow baseArrow):
//        //             baseArrow.PlayerOwner = playerObjectRef;
//        //             baseArrow.ArrowDamage = AttackDamage;
//        //             baseArrow.ArrowDirection = _dir;
//        //             baseArrow.ArrowSpeed = _speed;
//        //             baseArrow.ArrowCrit = Crit;
//        //             baseArrow.Skilling = skilling;
//        //             break;
//        //         case true when shootingObject.TryGetComponent<Outlander.Weapon.BaseAoEMagic>(out Outlander.Weapon.BaseAoEMagic baseAoESpell):
//        //             baseAoESpell.PlayerOwner = playerObjectRef;
//        //             baseAoESpell.SpellDamage = AttackDamage;
//        //             baseAoESpell.SpellDirection = _dir;
//        //             baseAoESpell.SpellSpeed = _speed;
//        //             baseAoESpell.SpellCrit = Crit;
//        //             baseAoESpell.Skilling = skilling;
//        //             break;
//        //     }
//        //     NetworkServer.Spawn(shootingObject);
//        // }

//        private void OnAimingTargetCheck()
//        {
//            // RaycastHit hit;
//            // var camRay = Camera.main.transform;
//            var camRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

//            // if (Physics.Raycast(camRay.position, camRay.transform.forward, out RaycastHit hit, Mathf.Infinity))
//            if (Physics.Raycast(camRay, out RaycastHit hit))
//            {
//                // if (hit.collider.gameObject != null)
//                // {
//                TargetObject = hit.point;
//                // Debug.Log(TargetObject);
//                if (IsAiming)
//                {
//                    TargetDirection = camRay.direction;
//                    TargetRotation = Camera.main.transform.rotation;
//                }
//                else
//                {
//                    TargetDirection = transform.forward;
//                    TargetRotation = transform.rotation;
//                }

//                // #if UNITY_EDITOR
//                //                 //                 Debug.Log($"{TargetObject.name} : {hit.point} : {TargetDirection}");
//                //                 //                 Debug.DrawRay(shootingPoint.transform.position, camRay.direction * 100, Color.green, 3);
//                // #endif

//                // }
//            }
//        }

//        public void Damage(string enemy, float damageAmount)
//        {
//            if (health <= 0) return;
//            if (onWeaponAction) damageAmount = damageAmount / 2;
//            damageBufferLog.Enqueue(damageAmount);
//            GameObject temp = new GameObject(enemy);
//            Destroy(temp, 1f);
//            enemyBufferLog.Enqueue(temp);
//            //Debug.Log($"Queue Damage : {damageAmount}");
//        }

//        [Command(requiresAuthority = false)]
//        public void CmdDamageIgnoreDefense(string environment, float damageAmount)
//        {
//            if (health <= 0) return;
//            if (onWeaponAction)
//                damageBufferLog.Enqueue(damageAmount * 2 * ((def + 100) / 100));
//            else
//                damageBufferLog.Enqueue(damageAmount * ((def + 100) / 100));
//            GameObject temp = new GameObject(environment);
//            Destroy(temp, 1f);
//            enemyBufferLog.Enqueue(temp);
//        }

//        public void Damage(GameObject enemy, float damageAmount)
//        {
//            //NetworkIdentity opponentIdentity = enemy.GetComponent<NetworkIdentity>();
//            //TargetDamageBuffer(opponentIdentity.connectionToClient, damageAmount);
//            if (health <= 0) return;
//            if (onWeaponAction) damageAmount = damageAmount / 2;
//            damageBufferLog.Enqueue(damageAmount);
//            enemyBufferLog.Enqueue(enemy);
//        }

//        [TargetRpc]
//        public void TargetDamageBuffer(NetworkConnection enemy, float damage)
//        {

//        }
//        [TargetRpc]
//        void RpcSetCounter()
//        {
//            Player.Animator.SetBool("isHit", true);
//        }
//        private void CheckDamageBuffer()
//        {
//            if (onGodmode) return;
//            if (Player.PlayerMatchManager?.myManager?.matchStatus == MatchManager.MatchStatus.IsEnd) return;
//            if (isCounter && damageBufferLog.Count > 0)
//            {
//                isCounter = false;
//                damageBufferLog.Dequeue();
//                enemyBufferLog.Dequeue();
//                if (isServer)
//                {
//                    RpcSetCounter();
//                    TargetShowDamagePopUp("Block", Color.white);
//                }

//            }
//            while (damageBufferLog.Count > 0)
//            {
//                //Debug.Log($"Before : {this.health}");
//                //isHit = true;
//                //Debug.Log($"Before : {this.health}");
//                //Debug.Log("dmg");
//                float tempDamage = damageBufferLog.Dequeue();

//                tempDamage = tempDamage / ((def + 100) / 100);
//                if (onWeaponAction) tempDamage = tempDamage / 2;
//                tempDamage = Mathf.Clamp(tempDamage, 1, float.MaxValue);

//                health -= tempDamage;

//                if (enemyBufferLog.Count > 1)
//                    enemyBufferLog.Dequeue();
//                if (isServer)
//                {
//                    RpcSetPlayerHealth(health);
//                    TargetShowDamagePopUp(tempDamage, Color.red);
//                    //Debug.Log($"name:{enemyBufferLog.Peek().name} layer:{enemyBufferLog.Peek().layer} tag:{enemyBufferLog.Peek().tag}");
//                    if (enemyBufferLog.Peek() != null)
//                        if (!enemyBufferLog.Peek().name.Equals("REDZONE"))
//                            TargetIsHit();
//                }
//                //Debug.Log($"Damage:{tempDamage} def:{def} health:{health}");
//            }
//        }

//        [TargetRpc]
//        private void TargetIsHit()
//        {
//            isHit = true;
//        }

//        public void IncreasePlayerHealth(float amount)
//        {
//            if (health <= maxHealth)
//                health += amount;
//            if (health > maxHealth)
//                health = maxHealth;
//            if (isServer)
//                RpcSetPlayerHealth(health);
//            if (isClient)
//                CmdSetPlayerHealth(health);
//        }

//        public void IncreasePlayerMana(float amount)
//        {
//            if (mana <= maxMana)
//                mana += amount;
//            if (mana > maxMana)
//                mana = maxMana;
//            if (isServer)
//                RpcSetPlayerMana(mana);
//            if (isClient)
//                CmdSetPlayerMana(mana);
//        }

//        #endregion

//        #region Main
//        private void Awake()
//        {
//            //character = GetComponent<PlayerAppearance>().character;
//            //if (isserver) 
//            //if (character != null) 
//            playerInputAction = new PlayerInputAction();

//            Player.PlayerAnimationEvent.OnAttackHit += AttackHit;
//            // playerAnimationEvent.OnCombo += Combo;
//            // playerAnimationEvent.OnComboPossible += ComboPossible;
//            // playerAnimationEvent.OnComboReset += ComboReset;
//            Player.PlayerAnimationEvent.OnEndSkill += EndSkill;
//            Player.PlayerAnimationEvent.OnResetStage += ResetStage;
//            Player.PlayerAnimationEvent.OnIdle += OnIdle;
//            // playerAnimationEvent.OnShooting += ShootingAction;

//            Player.PlayerUIActionHandler.OnUILeftItem += LeftItem;
//            Player.PlayerUIActionHandler.OnUIRightItem += RightItem;

//            if (isLocalPlayer) playerInputAction.Enable();
//        }

//        private void Start()
//        {
//            //playerLayer = gameObject.layer;
//            if (isLocalPlayer || isServer)
//            {
//                /*StatusEffectModifier sem = new StatusEffectModifier("mhp", "+", 100, 900);
//                StartCoroutine(sem.ServerStartTime());
//                playerStatModify.Add(sem.GetBuffName(), sem);
//                sem = new StatusEffectModifier("atk", "+", 100, 900);
//                StartCoroutine(sem.ServerStartTime());
//                playerStatModify.Add(sem.GetBuffName(), sem);
//                sem = new StatusEffectModifier("def", "+", 100, 900);
//                StartCoroutine(sem.ServerStartTime());
//                playerStatModify.Add(sem.GetBuffName(), sem);*/
//                LoadCharacterData();
//                InitializingPlayer();
//            }
//            //if (isServer)
//            //gameObject.layer = LayerMask.NameToLayer("Pvp");
//            if (isClient && !isLocalPlayer)
//                gameObject.layer = LayerMask.NameToLayer("Enemy");
//            if (!isLocalPlayer) return;
//            // playerOutlanderMovement = GetComponent<PlayerOutlanderMovement>();
//            // hpIndicator = GetComponent<EnemyHpIndicator>();

//            // playerLayer.value = LayerMask.NameToLayer("Player");
//            // enemyLayer.value = LayerMask.NameToLayer("Enemy");

//            // chatMode = false;
//            //LoadCharacterData();
//        }

//        private void Update()
//        {
//            float healthLeft = health / maxHealth;
//            float manaleft = mana / maxMana;
//            //uiPlayerInfo.UpdatePlayerHealth(healthLeft);

//            if (isLocalPlayer)
//            {
//                Player.PlayerUIManager.UpdatePlayerHeath(healthLeft);
//                Player.PlayerUIManager.UpdatePlayerMana(manaleft);
//                // playerUI.UpdatePlayerFP(GetComponent<IStatus>().Fpoint);
//                InputHandler();
//                OnAimingTargetCheck();
//                // if (Health <= 0) return;
//                if (global::UIManagers.Instance.playerCanvas.uiInventory.inventoryPanel.activeInHierarchy)
//                    Player.PlayerStatisticManager.UpdateHpAndMp(Health, Mana);
//            }

//            if ((isLocalPlayer && hasAuthority) || isServer)
//            {
//                CheckDamageBuffer();

//                if (ResetSkillActionHandler != null) ResetSkillActionHandler();
//                if (health <= 0 || Die)
//                {
//                    DieAction();
//                }
//            }
//            // Attack();
//        }

//        protected virtual void FixedUpdate()
//        {
//            // if (!hasAuthority) { return; }

//            // if (!chatMode)
//            // {
//            // Movement();
//            // MovementanimationUpdate();
//            // animation_tranfrom_position();
//            // }


//            //StatusModifyUpdate();
//        }
//        #endregion

//        #region Input
//        [Client]
//        private void InputHandler()
//        {
//            if (Die) return;
//            if (!Player.PlayerMovement.ENABLE_INPUT_SYSTEM) return;
//            if (GetCursorCheck()) return;

//            if (Player.PlayerMovement.climbing) return;
//            if (Player.PlayerMovement.swimming) return;
//            if (Player.PlayerMovement.drowned) return;

//            //OnAimingTargetCheck();

//            if (OnFireInput)
//            {
//                if (Player.PlayerMovement.Stamina <= 20.0f) return;
//                Attack();
//            }

//            // if (IsAiming)
//            // {
//            // OnAimingTargetCheck();
//            // }

//            // ps.gameObject.SetActive(attacking || playerOutlanderMovement.skilling);

//            if (!onSkill) return;
//            if (!Player.PlayerMovement.grounded) return;

//            switch (skillIndex)
//            {
//                case 0:
//                    // if (isSkillReady[skillIndex - 1])
//                    SkillAction(skillIndex);
//                    break;
//                case 1:
//                    // if (isSkillReady[skillIndex - 1])
//                    SkillAction(skillIndex);
//                    break;
//                case 2:
//                    // if (isSkillReady[skillIndex - 1])
//                    SkillAction(skillIndex);
//                    break;
//                case 3:
//                    // if (isSkillReady[skillIndex - 1])
//                    SkillAction(skillIndex);
//                    break;
//            }
//        }

//        //Reset Stage
//        public void ResetStage()
//        {
//            // Debug.Log("ResetStage");

//            attacking = false;
//            // onFire = attacking;

//            pickingup = false;
//            // onPickUp = pickingup;

//            skilling = false;
//            // onSkill = skilling;

//            hitting = false;

//            if (isLocalPlayer)
//            {
//                Player.PlayerSkillManager.ResetMultiHit();
//                CmdResetStage();
//            }
//        }

//        [Command]
//        public void CmdResetStage()
//        {
//            // Debug.Log("ResetStage");

//            attacking = false;
//            // onFire = attacking;

//            pickingup = false;
//            // onPickUp = pickingup;

//            skilling = false;
//            // onSkill = skilling;

//            hitting = false;

//            // ComboReset();
//        }

//        //picking up
//        private void PickingUp()
//        {
//            pickingup = true;
//            Player.Animator.Play("picking up");
//        }

//        public void EndPickingUp()
//        {
//            pickingup = false;
//            Player.Animator.Play("Idle");
//        }
//        #endregion

//        #region Level
//        float pps_atkDamage = 0, pps_maxHealth = 0, pps_def = 0, pps_maxMana = 0;
//        float ps_atkDamage = 0, ps_maxHealth = 0, ps_def = 0, ps_maxMana = 0, ps_crit = 0;

//        public void IncreaseStats(float _hp, float _atk, float _def, float _mp)
//        {
//            atkDamage = atkDamage - pps_atkDamage + _atk;
//            maxHealth = maxHealth - pps_maxHealth + _hp;
//            maxMana = maxMana - pps_maxMana + _mp;
//            def = def - pps_def + _def;

//            pps_atkDamage = _atk;
//            pps_maxHealth = _hp;
//            pps_maxMana = _mp;
//            pps_def = _def;

//            //RpcSendStat();
//        }

//        public void SetCharacterStats(PlayerStatisticManager.PlayerStatistic playerStat)
//        {
//            /*atkDamage = atkDamage - ps_atkDamage + playerStat.ATK;
//            maxHealth = maxHealth - ps_maxHealth + playerStat.MHP;
//            maxMana = maxMana - ps_maxMana + playerStat.MMP;
//            def = def - ps_def + playerStat.DEF;
//            crit = crit - ps_crit + playerStat.CRIT;

//            ps_atkDamage = playerStat.ATK;
//            ps_maxHealth = playerStat.MHP;
//            ps_maxMana = playerStat.MMP;
//            ps_def = playerStat.DEF;
//            ps_crit = playerStat.CRIT;*/

//            atkDamage = playerStat.ATK;
//            maxHealth = playerStat.MHP;
//            maxMana = playerStat.MMP;
//            def = playerStat.DEF;
//            crit = playerStat.CRIT;
//            critDamage = playerStat.CRITDAMAGE;
//            attackSpeed = playerStat.ATKSPD;
//            moveSpeed = playerStat.MOVSPD;

//            Player.PlayerMovement.SetSpeedPlayer(moveSpeed);
//            //RpcSendStat();
//        }

//        private void CalculateDefaultStatPlayer()
//        {
//            // Debug.Log($"player:{name} maxHealth={maxHealth}-{ps_maxHealth}-{pps_maxHealth}");
//            atkDamage = atkDamage - ps_atkDamage - pps_atkDamage;
//            // Debug.Log($"player:{name} atkDamage={atkDamage}-{ps_atkDamage}-{pps_atkDamage}");
//            maxHealth = maxHealth - ps_maxHealth - pps_maxHealth;
//            maxMana = maxMana - ps_maxMana - pps_maxMana;
//            def = def - ps_def - pps_def;
//            crit = crit - ps_crit;
//        }

//        /*private void RpcSendStat()
//        {
//            if (isServer)
//            {
//                RpcSetPlayerMaxHealth(maxHealth);
//                RpcSetPlayerMaxMana(maxMana);
//                RpcSetPlayerAttackDamage(atkDamage);
//                RpcSetPlayerDefense(def);
//                RpcSetPlayerCrit(crit);
//                if (health > maxHealth)
//                {
//                    health = maxHealth;
//                    //Debug.Log($"Player Health : {health}");
//                    RpcSetPlayerHealth(health);
//                }
//                if (mana > maxMana)
//                {
//                    mana = maxMana;
//                    RpcSetPlayerMana(mana);
//                }
//                RpcSetPlayerAttackSpeed(attackSpeed);
//                RpcSetPlayerMoveSpeed(moveSpeed);
//            }
//        }*/

//        #endregion

//        #region Combat
//        public void InitializingPlayer() // for init player
//        {
//            // Debug.Log("InitializingPlayer");
//            //health = maxHealth;
//            if (isLocalPlayer && hasAuthority) CmdInitializingPlayer();
//            if ((isLocalPlayer && hasAuthority) || isServer)
//            {
//                ResetStage();

//                // ComboReset();
//                gameObject.layer = 8;

//                //GetComponent<PlayerUIManager>().OnPlayerSpawn();
//                //playerOutlanderMovement.ResetStage();
//                Player.PlayerStatisticManager.InitializingPlayerStat();
//                Player.InventoryManager.ClearAllItemsInInventory();
//                Player.RuneSystemManager.ResetPlayerBag();
//                //GetComponent<PlayerSkillManager>().ClearCooldownSkills();

//                //if (health <= 0)
//                {
//                    health = maxHealth;
//                    mana = maxMana;
//                }

//                if (isServer)
//                {
//                    //Debug.Log($"Player Health : {health}");
//                    RpcSetPlayerHealth(health);
//                    RpcSetPlayerMaxHealth(maxHealth);
//                    RpcSetPlayerMana(mana);
//                    RpcSetPlayerMaxMana(maxMana);
//                    RpcSetPlayerAttackDamage(atkDamage);
//                    RpcSetPlayerDefense(def);
//                }

//                //Debug.Log($"Rpc Set Health: {health} | maxHealth {maxHealth}");
//                Die = false;
//                OnIdle();
//                //if (isLocalPlayer)
//                //playerOutlanderMovement.ENABLE_INPUT_SYSTEM = true;
//            }
//        }

//        [Command]
//        public virtual void CmdInitializingPlayer() // for init player
//        {
//            InitializingPlayer();
//        }

//        /*[Command]
//        private void CmdSendCurrentKillCount(int killCount)
//        {
//            GameBattleRoyalManager.singleton.KillCountRank(name, killCount);
//        }*/

//        public void OnIdle()
//        {
//            Player.Animator.Play("Idle");
//        }

//        public void Attack()
//        {
//            //AttackDamage = (Random.Range(0.0f, 100.0f) <= Crit) ? AttackDamage * 2.0f : AttackDamage;
//            //CurAttackDamage = AttackDamage - Random.Range(0, AttackDamage * 0.2f);
//        }
//        void SetCounterState(int state)
//        {
//            if (!isLocalPlayer) return;
//            CmdOnCounter(System.Convert.ToBoolean(state));
//        }
//        [Command]
//        void CmdOnCounter(bool state)
//        {
//            isCounter = state;
//        }

//        public void OnAttackInput()
//        {
//            // Debug.Log("OnAttackInput");
//            if (!CanReceiveFireInput)
//            {
//                // if (playerOutlanderMovement.DecreaseStamina <= 20.0f) return;
//                CanReceiveFireInput = true;
//                OnFireInput = false;
//                // Attack();
//            }
//            else
//            {
//                CanReceiveFireInput = false;
//            }

//            // CanReceiveFireInput = _state;
//        }

//        public void EndSkill() => ResetStage();

//        //hit
//        public void HitAction()
//        {
//            ResetStage();
//            hitting = true;
//            Player.Animator.Play("hit");
//        }

//        public void EndHitAction()
//        {
//            ResetStage();
//            hitting = false;
//        }
//        //die
//        public void DieAction()
//        {
//            if (isLocalPlayer) DropAllItemOnDie();
//            ResetStage();
//            Die = true;
//            if (!Player.PlayerMovement.swimming)
//            {
//                Player.PlayerMovement.dashing = false;
//                Player.Animator.Play("die");
//            }
                

//            PlayerInput playerInput = GetComponent<PlayerInput>();
//            playerInput.enabled = false;

//            gameObject.layer = 0;

//            if (isLocalPlayer && Health <= 0)
//            {
//                //playerUI.OnGameSummary(false);
//                //DropAllItemOnDie();
//                //runeSystemManager.ResetPlayerBag();
//                Health = 1;
//                //Camera.main.transform.GetChild(0).gameObject.SetActive(false);
//                global::ClientTriggerEventManager.Instance.IsKilled();
//            }

//            if (isServer)
//            {
//                //Debug.Log($"[Server] Player {name} Die. {enemyBufferLog.Count}");
//                if (enemyBufferLog.TryDequeue(out GameObject lastPlayerHit))
//                {
//                    //Debug.Log($"[Server] Player {lastPlayerHit.name} kill {name}");
//                    if (lastPlayerHit.TryGetComponent(out PlayerComponents killerComp))
//                    {
//                        //TargetIncreaseKillCount(ni.connectionToClient);
//                        Player.PlayerMatchManager.myManager.IncreaseKillRank(killerComp, Player);
//                        RpcBroadcastKill(lastPlayerHit.name, killerComp.WeaponManager.currentWeaponType.ToString(), name);
//                        if (Player.PlayerMatchManager.myManager.selectedBounty == netIdentity)
//                            killerComp.PlayerMatchManager.myManager.GetBountyReward(killerComp.PlayerIdentity.connectionToClient);
//                    }
//                    else if (lastPlayerHit.TryGetComponent(out IDamagable id))
//                    {
//                        string enemy = id.IsBoss ? "BOSS" : "MONSTER";
//                        RpcBroadcastKill(enemy, enemy, name);
//                    }
//                    else if (lastPlayerHit.TryGetComponent(out IMonster iMons))
//                    {
//                        string enemy = iMons.IsBoss ? "BOSS" : "MONSTER";
//                        RpcBroadcastKill(enemy, enemy, name);
//                    }
//                    else
//                    {
//                        if (lastPlayerHit.name.Equals("MONSTER"))
//                            RpcBroadcastKill("MONSTER", "MONSTER", name);
//                        else
//                            RpcBroadcastKill("", lastPlayerHit.name, name);
//                    }

//                    Player.PlayerMatchManager.myManager?.RemovePlayerFromMatchManger(Player);
//                    //Player.RuneSystemManager.ResetPlayerBag();
//                }
//            }
//            //if (isLocalPlayer)
//            //playerOutlanderMovement.ENABLE_INPUT_SYSTEM = false;
//        }

//        //[Command] private void CmdSendPlayerDie() => GameBattleRoyalManager.singleton.RemovePlayerFromList(gameObject);

//        public void DropAllItemOnDie()
//        {
//            if (Player.InventoryManager.itemObjectInBag.Count > 0)
//            {
//                List<RemoveItemInfo> removitemList = new List<RemoveItemInfo>();
//                List<ItemScriptable> items = new List<ItemScriptable>();
//                List<int> amounts = new List<int>();
//                foreach (InventoryItemBehavior item in Player.InventoryManager.itemObjectInBag)
//                {
//                    //Debug.Log($"Drop:{iib.thisItem.itemName}");
//                    if (item.thisItem.mainType == Type.Equipment)
//                    {
//                        if (item.IsEquiped)
//                            Player.EquipmentManager.ForceUnequipItem(item);
//                    }
//                    else if (item.thisItem.mainType == Type.MainWeapon)
//                    {
//                        if (item.IsEquiped)
//                            Player.EquipmentManager.ForceUnequipItem(item);
//                    }
//                    else if (item.thisItem.mainType == Type.SubWeapon)
//                    {
//                        if (item.IsEquiped)
//                            Player.EquipmentManager.ForceUnequipItem(item);
//                    }
//                    else if (item.thisItem.mainType == Type.Rune)
//                    {
//                        if (item.IsEquiped)
//                            Player.RuneSystemManager.UnEquipRune(item);
//                    }
//                    items.Add(item.thisItem);
//                    amounts.Add(item.Amount);
//                    removitemList.Add(new RemoveItemInfo
//                    {
//                        removeItemObject = item,
//                        qty = item.Amount
//                    });
//                }

//                if (Player.PlayerInventoryController.Bronze > 0)
//                {
//                    items.Add(OutlanderDB.singleton.GetItemScriptable("000"));
//                    amounts.Add(Player.PlayerInventoryController.Bronze);
//                }

//                Player.InventoryManager.RemoveItemFromInventory(removitemList);
//                Player.InventoryManager.equipedGeneralDictionary.Clear();
//                Player.InventoryManager.SetItemSlotToDefaultSprite();
//                //playerObjectRef.GetComponent<PlayerOutlander>().CmdInstantiateDropAllObj(items, amounts);
//                CmdInstantiateDropAllObj(items, amounts);
//            }
//            Player.PlayerInventoryController.Bronze = 0;
//        }

//        [Command]
//        private void CmdInstantiateDropAllObj(List<ItemScriptable> items, List<int> amounts)
//        {
//            Vector3 objectposition = gameObject.transform.position + new Vector3(0, 0, 1);
//            GameObject newDropItem = Instantiate(deadBox, objectposition, Quaternion.identity);

//            newDropItem.GetComponent<DeadBox>().AssignDropItemData(items, amounts);
//            newDropItem.GetComponent<NetworkMatch>().matchId = Player.PlayerMatchManager.MatchID.ToGuid();

//            NetworkServer.Spawn(newDropItem);
//        }

//        public void AttackHit()
//        {
//            if (PlayerManagers.Instance.matchManager == null) return;
//            if (!PlayerManagers.Instance.matchManager.canInteract) return;
//            if (ArrowObject != null) return;
//            if (!isLocalPlayer) return;
//            if (Player.PlayerMovement.Stamina - 20 < 0) return;

//            if (!skilling)
//            {
//                if (Player.PlayerMovement.Stamina - 20f < 0f)
//                    return;
//                else
//                    Player.PlayerMovement.Stamina -= 20.0f;
//            }
//            else
//            {
//                Player.PlayerSkillManager.MultiHitSkill();
//            }

//            Player.PlayerMovement.ResetRechargeCooldown();

//            CmdDamage();

//            /*Collider[] hitnEnemies = Physics.OverlapBox(attackPoint.bounds.center, attackPoint.bounds.size, attackPoint.transform.rotation, enemyLayer);

//            // currentAtkDmg = AttackDamage;
//            foreach (Collider col in hitnEnemies)
//            {
//                // col.TryGetComponent<IDamagable>(out IDamagable iDamagable);
//                // col.TryGetComponent<IPlayer>(out IPlayer iPlayer);
//                // col.TryGetComponent<Outlander.ResourecesObject.OreResoureObject>(out Outlander.ResourecesObject.OreResoureObject oreResoure);
//                // col.TryGetComponent<Outlander.ResourecesObject.TreeResoureObject>(out Outlander.ResourecesObject.TreeResoureObject treeResoure);

//                switch (true)
//                {
//                    case true when col.gameObject.layer == 9 || col.gameObject.layer == 8:
//                        if (GameBattleRoyalManager.singleton.isSelectSpawnPhase)
//                            if (isLocalPlayer)
//                                CmdDamage(col.gameObject);
//                        break;
//                    case true when (col.TryGetComponent(out Outlander.ResourecesObject.OreResoureObject oreResoure)):
//                        oreResoure.PlayerHitting(playerObjectRef.gameObject);
//                        break;
//                    case true when (col.TryGetComponent(out Outlander.ResourecesObject.TreeResoureObject treeResoure)):
//                        treeResoure.PlayerHitting(playerObjectRef.gameObject);
//                        break;
//                }

//                if (col.name.Equals(name)) return;
//                GameObject Fx = Instantiate(hitFX, attackCollider.transform.position, Quaternion.identity);
//                Destroy(Fx, 0.5f);
//            }*/
//        }

//        [Command]
//        private void CmdDamage() //GameObject target
//        {
//            //Debug.Log($"[Client] target:{target} damage:{damageAmount} from player:{player}");
//            //Debug.Log($"[Server] target:{target} damage:{CurAttackDamage} from player:{player}");
//            //if (!ReferenceEquals(target, null) ? false : (target ? false : true)) return;

//            //if (!attackPoint.bounds.Intersects(target.GetComponent<CapsuleCollider>().bounds)) return;
//            //if (Vector3.Distance(transform.position, target.transform.position) > 2f) return;

//            bool isCrit = Random.Range(0.0f, 100.0f) <= Crit;
//            if (!skilling)
//            {
//                float isDamageCrit = isCrit ? AttackDamage * CritDamage : AttackDamage;
//                CurAttackDamage = isDamageCrit - Random.Range(0, isDamageCrit * 0.2f);
//            }
//            else
//            {
//                float isDamageCrit = isCrit ? CurAttackDamage * CritDamage : CurAttackDamage;
//                CurAttackDamage = isDamageCrit - Random.Range(0, isDamageCrit * 0.2f);
//            }

//            Collider[] hitnEnemies = Physics.OverlapBox(attackPoint.bounds.center, attackPoint.bounds.size, attackPoint.transform.rotation, 3 << 8);

//            foreach (Collider col in hitnEnemies)
//            {
//                //Debug.Log($"col InstanceID:{col.gameObject.GetInstanceID()} == {gameObject.GetInstanceID()} InstanceID");
//                if (col.gameObject.GetInstanceID() == gameObject.GetInstanceID()) continue;

//                switch (true)
//                {
//                    case true when (col.TryGetComponent(out IPlayer targetPlayer)):
//                        targetPlayer.Damage(gameObject, CurAttackDamage);
//                        float damageDef = CurAttackDamage / ((targetPlayer.Defense + 100) / 100);
//                        if (targetPlayer.GetWeaponAction())
//                            damageDef = Mathf.Clamp(damageDef / 2, 1, float.MaxValue);
//                        if (targetPlayer.IsCounter)
//                        {
//                            TargetShowDamagePopUp("Block", Color.white);
//                            return;
//                        }
//                        TargetShowDamagePopUp(damageDef, isCrit ? Color.yellow : Color.white);
//                        break;
//                    case true when (col.TryGetComponent(out IDamagable targetEnemy)):
//                        targetEnemy.Damage(CurAttackDamage, gameObject);
//                        float damageEnemyDef = CurAttackDamage / ((targetEnemy.Def + 100) / 100);
//                        TargetShowDamagePopUp(damageEnemyDef, isCrit ? Color.yellow : Color.white);
//                        break;
//                    case true when (col.TryGetComponent(out IMonster targetEnemy)):
//                        targetEnemy.Damage(CurAttackDamage, gameObject);
//                        float _damageEnemyDef = CurAttackDamage / ((targetEnemy.Def + 100) / 100);
//                        TargetShowDamagePopUp(_damageEnemyDef, isCrit ? Color.yellow : Color.white);
//                        break;
//                    case true when (col.TryGetComponent(out ResourecesObject.OreResoureObject oreResoure)):
//                        oreResoure.PlayerHitting(playerObjectRef.gameObject);
//                        break;
//                    case true when (col.TryGetComponent(out ResourecesObject.TreeResoureObject treeResoure)):
//                        treeResoure.PlayerHitting(playerObjectRef.gameObject);
//                        break;
//                }
//            }

//            /*if (target.TryGetComponent(out IPlayer targetPlayer))
//            {
//                targetPlayer.Damage(gameObject, CurAttackDamage);
//                float damageDef = CurAttackDamage / ((targetPlayer.Defense + 100) / 100);
//                if (targetPlayer.GetWeaponAction())
//                    damageDef = Mathf.Clamp(damageDef / 2, 1, float.MaxValue);
//                TargetShowDamagePopUp(damageDef, isCrit ? Color.yellow : Color.white);
//            }
//            else if (target.TryGetComponent(out IDamagable targetEnemy))
//            {
//                targetEnemy.Damage(CurAttackDamage, gameObject);
//                float damageDef = CurAttackDamage / ((targetEnemy.Def + 100) / 100);
//                TargetShowDamagePopUp(damageDef, isCrit ? Color.yellow : Color.white);
//            }*/

//            /*if (target.GetComponent<IPlayer>().Health <= 0)
//            {
//                NetworkIdentity enemyIdentity = target.GetComponent<NetworkIdentity>();
//                TargetWasKill(enemyIdentity.connectionToClient, damageAmount, player);
//                killCount++;
//            }*/
//        }

//        [TargetRpc]
//        private void TargetShowDamagePopUp(float damageamount, Color color)
//        {
//            if (color == Color.red)
//            {
//                DamagePopUp damagePop = Instantiate(damagePopUp, transform.position + (Vector3.up * 2), Quaternion.identity);
//                damagePop.Setup(damageamount, color);
//            }
//            else
//            {
//                DamagePopUp damagePop = Instantiate(damagePopUp, attackPoint.bounds.center, Quaternion.identity);
//                damagePop.Setup(damageamount, color);

//                GameObject Fx = Instantiate(hitFX, attackPoint.bounds.center, Quaternion.identity);
//                Destroy(Fx, 0.5f);
//            }
//        }
//        [TargetRpc]
//        private void TargetShowDamagePopUp(string text, Color color)
//        {
//            DamagePopUp damagePop = Instantiate(damagePopUp, transform.position + (Vector3.up * 2), Quaternion.identity);
//            damagePop.Setup(text, color);
//        }

//        [ClientRpc]
//        private void RpcBroadcastKill(string killer, string type, string victim)
//        {
//            PlayerManagers.Instance.matchManager?.OnReceiveKillActivity(killer, type, victim);
//        }

//        // public void IsDead()
//        // {
//        //     //GetComponent<PlayerSpawnManager>().isPlayerDead = die;
//        // }
//        #endregion

//        #region Player Skill
//        // private void Skill_1()
//        // {

//        // }

//        // private void Skill_2()
//        // {

//        // }

//        // private void Skill_3()
//        // {

//        // }

//        // private void Skill_4()
//        // {

//        // }

//        // private void Skill_5()
//        // {

//        // }

//        private void LeftItem()
//        {
//            //Debug.Log("in LeftPotion !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
//            if (!isLocalPlayer) return;
//            if (isUseConsumable) return;

//            if (Player.InventoryManager.equipedGeneralDictionary.ContainsKey(1))
//            {
//                isUseConsumable = true;
//                Player.PlayerMovement.useConsumable = true;
//                Player.Animator.SetBool("useConsumable", true);

//                isHit = false;
//                UIManagers.Instance.playerCanvas.potionIconGO.SetActive(true);
//                Player.PlayerMovement.SetSpeedForUsingConsumable();

//                StartCoroutine(DelayConsumable(1));
//            }
//        }

//        public virtual void RightItem()
//        {
//            if (!isLocalPlayer) return;
//            if (isUseConsumable) return;

//            if (Player.InventoryManager.equipedGeneralDictionary.ContainsKey(2))
//            {
//                isUseConsumable = true;
//                Player.PlayerMovement.useConsumable = true;
//                Player.Animator.SetBool("useConsumable", true);

//                isHit = false;
//                UIManagers.Instance.playerCanvas.potionIconGO.SetActive(true);
//                Player.PlayerMovement.SetSpeedForUsingConsumable();

//                StartCoroutine(DelayConsumable(2));
//            }

//        }

//        [SerializeField] private float time = 3f;
//        //public float timeRemaining;
//        private bool isUseConsumable = false;

//        private IEnumerator DelayConsumable(int slotnumber)
//        {
//            bool isBreak = false;
//            float timeRemaining = time;
//            //playerOutlanderMovement.setSpeedForUsingConsumable();
//            while (timeRemaining > 0)
//            {
//                timeRemaining -= Time.deltaTime;
//                UIManagers.Instance.playerCanvas.potionProgessBar.fillAmount = 1 - (timeRemaining / time);
//                //Debug.Log("DelayConsumable");
//                if (isHit || Player.PlayerMovement.IsAction() || Die || OnFireInput)
//                {
//                    isBreak = true;
//                    break;
//                }

//                yield return null;
//            }

//            //Debug.Log("DelayConsumable finish !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

//            isUseConsumable = false;
//            Player.PlayerMovement.useConsumable = false;
//            Player.AnimationScript.SetRigWeight(1);
//            Player.Animator.SetBool("useConsumable", false);
//            UIManagers.Instance.playerCanvas.potionIconGO.SetActive(false);

//            Player.PlayerMovement.SetSpeedForUsingConsumableBack();

//            if (!isBreak)
//            {
//                switch (slotnumber)
//                {
//                    default:
//                        break;
//                    case 1:
//                        //itemslot 1
//                        Player.InventoryManager.UseIteminSlot(1);
//                        break;
//                    case 2:
//                        //itemslot 2
//                        Player.InventoryManager.UseIteminSlot(2);
//                        break;
//                }
//            }
//        }


//        // private void WeaponAction()
//        // {
//        //     //if (onWeaponAction)
//        //     //{
//        //     //}

//        // }
//        #endregion

//        #region Chat
//        protected virtual void OnChat()
//        {

//            // Debug.Log("OnChat");
//            //if (onChat)
//            //{
//            //    chatInputField.interactable = true;
//            //    chatInputField.ActivateInputField();
//            //    chatMode = true;
//            //}
//            //else
//            //{
//            //    chatInputField.interactable = false;
//            //    // chatInputField.ActivateInputField();
//            //    chatMode = false;
//            //}

//            // anim.CrossFade("Block", 0.1f);

//        }
//        #endregion

//        #region Input System
//        [Client]
//        private void OnFire(InputValue value)
//        {
//            if (!CanReceiveFireInput) return;
//            if (onWeaponAction && !IsAiming) return;
//            if (GetCursorCheck()) return;
//            if (Player.PlayerMovement.climbing) return;
//            if (Player.PlayerMovement.swimming) return;
//            if (Player.PlayerMovement.onCrouch)
//                Player.PlayerMovement.SetCrouchState(false);

//            // Debug.Log(value.Get<float>());
//            OnFireInput = value.isPressed;
//            // OnFireInput = true;

//            // if (CanReceiveFireInput)
//            // {
//            //     // OnFireInput = value.isPressed;

//            //     // OnFireInput = true;
//            //     CanReceiveFireInput = false;
//            // }
//            // else
//            // {
//            //     return;
//            // }
//        }

//        [Client]
//        private void OnSkill_1(InputValue value)
//        {
//            if (onWeaponAction) return;
//            if (GetCursorCheck()) return;
//            if (!Player.PlayerMovement.grounded) return;
//            if (Player.PlayerMovement.climbing) return;
//            if (Player.PlayerMovement.swimming) return;

//            onSkill = value.isPressed;
//            SkillHandler_1();
//        }

//        [Client]
//        private void OnSkill_2(InputValue value)
//        {
//            if (onWeaponAction) return;
//            if (GetCursorCheck()) return;
//            if (!Player.PlayerMovement.grounded) return;
//            if (Player.PlayerMovement.climbing) return;
//            if (Player.PlayerMovement.swimming) return;

//            onSkill = value.isPressed;
//            SkillHandler_2();
//            // Debug.Log("Onskill");
//        }

//        [Client]
//        private void OnSkill_3(InputValue value)
//        {
//            if (onWeaponAction) return;
//            if (GetCursorCheck()) return;
//            if (!Player.PlayerMovement.grounded) return;
//            if (Player.PlayerMovement.climbing) return;
//            if (Player.PlayerMovement.swimming) return;

//            onSkill = value.isPressed;
//            SkillHandler_3();
//            // Debug.Log("Onskill");
//        }

//        [Client]
//        private void OnSkill_4(InputValue value)
//        {
//            if (onWeaponAction) return;
//            if (GetCursorCheck()) return;
//            if (!Player.PlayerMovement.grounded) return;
//            if (Player.PlayerMovement.climbing) return;
//            if (Player.PlayerMovement.swimming) return;

//            onSkill = value.isPressed;
//            SkillHandler_4();
//            // Debug.Log("Onskill");
//        }

//        // [Client]
//        // private void OnSkill_5(InputValue value)
//        // {
//        //     if (onWeaponAction) return;
//        //     if (!playerOutlanderMovement.grounded) return;
//        //     // onSkill = value.isPressed;
//        //     // Skill_5();
//        // }

//        // [Client]
//        private void OnWeaponAction(InputValue value)
//        {
//            if (Player.PlayerMovement.IsAction() && !GetWeaponAction()) return;
//            if (GetCursorCheck())
//            {
//                CmdSetWeaponAction(false);
//                return;
//            }

//            // var onTempWeaponAction = value.isPressed;
//            // if (isServer)
//            //     RpcSetWeaponAction(onTempWeaponAction);
//            if (isLocalPlayer)
//            {
//                CmdSetWeaponAction(value.isPressed);
//            }
//            // WeaponAction();
//        }

//        public bool GetWeaponAction() => onWeaponAction;
//        public bool OnWeaponActionActive { get => onWeaponAction; set => onWeaponAction = value; }

//        // [Server, TargetRpc]
//        // private void RpcSetWeaponAction(bool newWeaponaction)
//        // {
//        //     Debug.Log("RpcSetWeaponAction");
//        //     onWeaponAction = newWeaponaction;
//        // }

//        [Client]
//        private void OnLeftItem(InputValue value)
//        {
//            if (onWeaponAction) return;
//            onLeftItem = value.isPressed;
//            // Debug.Log("OnLeftItem");
//            LeftItem();
//        }

//        [Client]
//        private void OnRightItem(InputValue value)
//        {
//            if (onWeaponAction) return;
//            onRightItem = value.isPressed;
//            // Debug.Log("OnRightItem");
//            RightItem();
//        }

//        [Client]
//        private void OnOpenChat(InputValue value)
//        {
//            onChat = !onChat;
//            // Debug.Log($"Open Chat command {onChat}");
//        }

//        // [Client]
//        // private void OnOpenMap(InputValue value)
//        // {

//        // }

//        // [Client]
//        // private void OnEscape(InputValue value)
//        // {
//        //     // onEscape = value.isPressed;
//        //     if (onChat) onChat = false;
//        // }

//        [Client]
//        private void OnInteraction(InputValue value)
//        {
//            // Debug.Log("OnInteraction");
//            onInteract = value.isPressed;
//            PlayerInteractCheck();
//        }
//        #endregion

//        /*private void OnDrawGizmos()
//        {
//            if (attackPoint == null)
//                return;
//            Gizmos.color = Color.red;
//            Gizmos.DrawCube(attackPoint.bounds.center, attackPoint.bounds.size);
//        }*/

//        #region Save/Load Drom Database

//        public void LoadCharacterData()
//        {
//            // Debug.Log($"player:{name} load data");
//            //Health = character.playerData.hp;
//            MaxHealth = 100;
//            //Mana = character.playerData.mp;
//            MaxMana = 100;

//            /*PlayerFP playerFP = GetComponent<PlayerFP>();
//            playerFP.Fpoint = character.playerData.fpPoint; //100.0f;

//            LevelSystem playerLevel = GetComponent<LevelSystem>();
//            playerLevel.Level = character.playerData.level;
//            playerLevel.currentXp = character.playerData.experience;

//            ProficiencySystem playerProficiency = GetComponent<ProficiencySystem>();
//            //playerProficiency.proWeapon.Clear();
//            ProficiencySystem.ProficiencyWeapon pspw;
//            foreach (var cp in character.proficiency)
//            {
//                switch (cp.className)
//                {
//                    case "none":
//                        pspw = new ProficiencySystem.ProficiencyWeapon
//                        {
//                            level = cp.level,
//                            currentXp = cp.exp,
//                            weaponType = WeaponManager.WeaponType.None
//                        };
//                        playerProficiency.proWeapon.Add(pspw.weaponType, pspw);
//                        break;
//                    case "sword":
//                        pspw = new ProficiencySystem.ProficiencyWeapon
//                        {
//                            level = cp.level,
//                            currentXp = cp.exp,
//                            weaponType = WeaponManager.WeaponType.Sword
//                        };
//                        playerProficiency.proWeapon.Add(pspw.weaponType, pspw);
//                        break;
//                    case "bow_quiver":
//                        pspw = new ProficiencySystem.ProficiencyWeapon
//                        {
//                            level = cp.level,
//                            currentXp = cp.exp,
//                            weaponType = WeaponManager.WeaponType.BowQuiver
//                        };
//                        playerProficiency.proWeapon.Add(pspw.weaponType, pspw);
//                        break;
//                }
//            }
//            if(isServer)
//                playerProficiency.ProficiencySetStats();

//            PlayerStatisticManager playerStat = GetComponent<PlayerStatisticManager>();
//            playerStat.playerStatistic.STR = character.playerData.status.STR;
//            playerStat.playerStatistic.AGI = character.playerData.status.AGI;
//            playerStat.playerStatistic.VIT = character.playerData.status.VIT;
//            playerStat.playerStatistic.INT = character.playerData.status.INT;
//            playerStat.playerStatistic.DEX = character.playerData.status.DEX;
//            playerStat.playerStatistic.LUK = character.playerData.status.LUK;
//            playerStat.playerStatistic.point = character.playerData.status.point;
//            playerStat.playerStatistic.used_point = character.playerData.status.usedPoint;
//            playerStat.Dummy_Point = playerStat.playerStatistic.point;
//            playerStat.CalculateStatusToPlayerByStat();*/
//        }

//        //public PlayerDataMsg GetCharacterData()
//        //{
//        //    // Debug.Log($"Save Database:{name}");
//        //    PlayerFP playerFP = GetComponent<PlayerFP>();
//        //    LevelSystem playerLevel = GetComponent<LevelSystem>();
//        //    PlayerStatisticManager playerStat = GetComponent<PlayerStatisticManager>();
//        //    /*ProficiencySystem playerProficiency = GetComponent<ProficiencySystem>();
//        //    UpdateProficiency updateProficiency = new UpdateProficiency();
//        //    updateProficiency.playerClass = new List<JSONUpdateProficiency>();
//        //    foreach (KeyValuePair<WeaponManager.WeaponType, ProficiencySystem.ProficiencyWeapon> pspw in playerProficiency.proWeapon)
//        //    {
//        //        Debug.Log($"{pspw.Value.weaponType} {pspw.Value.level} {pspw.Value.currentXp}");
//        //        JSONUpdateProficiency jsonUpPro = new JSONUpdateProficiency
//        //        {
//        //            className = pspw.Value.weaponType switch
//        //            {
//        //                WeaponManager.WeaponType.None => "none",
//        //                WeaponManager.WeaponType.Sword => "sword",
//        //                WeaponManager.WeaponType.BowQuiver => "bow_quiver",
//        //                _ => "none",
//        //            },
//        //            level = pspw.Value.level,
//        //            exp = pspw.Value.currentXp
//        //        };
//        //        Debug.Log($"{jsonUpPro.className} {jsonUpPro.level} {jsonUpPro.exp}");
//        //        updateProficiency.playerClass.Add(jsonUpPro);
//        //    }*/
//        //    CalculateDefaultStatPlayer();

//        //    PlayerDataMsg playerDataMsg = new PlayerDataMsg
//        //    {
//        //        email = character.email,
//        //        level = 1,
//        //        experience = 0,
//        //        point = 0,
//        //        money = 0,

//        //        mapName = character.location.mapName,
//        //        posX = Die ? character.checkpoint.posX : gameObject.transform.position.x,
//        //        posY = Die ? character.checkpoint.posY : gameObject.transform.position.y,
//        //        posZ = Die ? character.checkpoint.posZ : gameObject.transform.position.z,
//        //    };
//        //    return playerDataMsg;
//        //}

//        //[Command]
//        //public void CmdSaveMap(string newMap)
//        //{
//        //    character.location.mapName = newMap;
//        //}
//        #endregion

//        #region Triggered Check
//        private void OnTriggerEnter(Collider other)
//        {
//            //  ------------------------------------------------------ chest ------------------------------------------------------------------------------
//            /*if (other.tag == "Chest" && isLocalPlayer)
//            {
//                other.gameObject.GetComponent<OnlineChest>().OpenInteractPanel();
//                if (isLocalPlayer && onInteract)
//                {
//                    if (other.gameObject.GetComponent<OnlineChest>().isOpen != true)
//                    {
//                        other.gameObject.GetComponent<OnlineChest>().OpenChest(playerObjectRef.gameObject);
//                        onInteract = false;
//                    }
//                    else
//                    {
//                        other.gameObject.GetComponent<OnlineChest>().CloseChest();
//                        onInteract = false;
//                    }
//                }
//            }*/


//            //if (other.gameObject.CompareTag("DeadBox") && isLocalPlayer)
//            //{
//            //    other.gameObject.GetComponent<DeadBox>().OpenInteractPanel();
//            //    if (isLocalPlayer && onInteract)
//            //    {
//            //        other.gameObject.GetComponent<DeadBox>().OpenChest();
//            //        onInteract = false;
//            //    }
//            //}

//            /*
//            if (other.tag == "Chest" && isLocalPlayer && other.gameObject.GetComponent<TreasureBoxScript>().isReceive !=  true)
//            {
//                other.gameObject.GetComponent<TreasureBoxScript>().PlayerTirgger(playerObjectRef.gameObject);
//                if (isLocalPlayer && onInteract)
//                {
//                    other.gameObject.GetComponent<TreasureBoxScript>().onInteraction = true;
//                }
//            }
//            */
//            //  ------------------------------------------------------ resoure ----------------------------------------------------------------------------


//            //  ----------------------------------------------------- NPC --------------------------------------------------------------------------------
//            if (other.tag == "NPC_Shop")
//            {
//                if (isLocalPlayer)
//                {
//                    other.gameObject.GetComponent<NpcShop>().OpenInteractPanel();
//                }
//            }
//            if (other.tag == "NPC_Blacksmith")
//            {
//                if (isLocalPlayer)
//                {
//                    other.gameObject.GetComponent<NpcBlack>().IdentifyPlayer(playerObjectRef.gameObject);
//                    other.gameObject.GetComponent<NpcBlack>().OpenInteractPanel();
//                    other.gameObject.GetComponent<NpcBlack>().SetUpCoin();
//                }
//            }

//            //  ------------------------------------------------------ DropedItem ----------------------------------------------------------------------------
//            /*
//                      if (other.tag == "DropedItem")
//                      {
//                          if (isLocalPlayer)
//                          {
//                              other.gameObject.GetComponent<DropedItemBehavior>().OpenPickUpPanel();
//                          }
//                          if (isLocalPlayer && onInteract)
//                          {
//                              DropedItemBehavior dropitem = other.GetComponent<DropedItemBehavior>();
//                              GetComponent<InventoryManager>().CmdPickUpitem(dropitem.iteminfo, dropitem.itemData, dropitem.gameObject);
//                              onInteract = false;
//                          }
//                      }
//          */
//            //  ------------------------------------------------------ devil fruit  ------------------------------------------------------------------------------
//            /*
//            if (other.gameObject.CompareTag("DevilFruit") && isLocalPlayer)
//            {
//                other.gameObject.GetComponent<DevilFruit.DevilFruitScript>().OpenPickUpPanel();
//                if (isLocalPlayer && onInteract)
//                {
//                    other.gameObject.GetComponent<DevilFruit.DevilFruitScript>().CmdPlayerPickUp(playerObjectRef.gameObject);
//                    onInteract = false;
//                }
//            }
//            */
//        }

//        private void OnTriggerStay(Collider other)
//        {
//            //  ------------------------------------------------------ chest ------------------------------------------------------------------------------
//            /* if (other.tag == "Chest" && isLocalPlayer)
//             {
//                 if (isLocalPlayer && onInteract)
//                 {
//                     if (other.gameObject.GetComponent<OnlineChest>().isOpen != true)
//                     {
//                         other.gameObject.GetComponent<OnlineChest>().OpenChest(playerObjectRef.gameObject);
//                         onInteract = false;
//                     }
//                     else
//                     {
//                         other.gameObject.GetComponent<OnlineChest>().CloseChest();
//                         onInteract = false;
//                     }
//                 }

//                 if (isLocalPlayer && other.gameObject.GetComponent<OnlineChest>().isOpen)
//                 {
//                     other.gameObject.GetComponent<OnlineChest>().showItemInChest();
//                 }


//                 //if (other.tag == "Chest" && isLocalPlayer && other.gameObject.GetComponent<OnlineChest>().isOpen)
//                 //{
//                 //    other.gameObject.GetComponent<OnlineChest>().CloseChestInteractPanal();
//                 //}
//             }

//             */
//            //if (other.gameObject.CompareTag("DeadBox") && isLocalPlayer && onInteract)
//            //{
//            //    other.gameObject.GetComponent<DeadBox>().OpenChest();
//            //    onInteract = false;
//            //}
//            /*
//            if (other.tag == "Chest" && isLocalPlayer && other.gameObject.GetComponent<TreasureBoxScript>().isReceive != true)
//            {
//                if (isLocalPlayer && onInteract)
//                {
//                    other.gameObject.GetComponent<TreasureBoxScript>().onInteraction = true;
//                }
//            }
//            */
//            //  ------------------------------------------------------ resoure ----------------------------------------------------------------------------
//            //  ----------------------------------------------------- NPC --------------------------------------------------------------------------------
//            if (other.tag == "NPC_Shop")
//            {
//                if (onInteract)
//                {
//                    if (isLocalPlayer)
//                    {
//                        other.gameObject.GetComponent<NpcShop>().CloseInteractPanel();
//                        other.gameObject.GetComponent<NpcShop>().OpenShop();
//                        CmdSetWeaponAction(false);
//                    }
//                    onInteract = false;
//                }
//            }
//            if (other.tag == "NPC_Blacksmith")
//            {
//                if (onInteract)
//                {
//                    if (isLocalPlayer)
//                    {
//                        other.gameObject.GetComponent<NpcBlack>().CloseInteractPanel();
//                        other.gameObject.GetComponent<NpcBlack>().OpenRepairPanal();

//                    }
//                    onInteract = false;
//                }
//            }

//            //  ------------------------------------------------------ DropedItem ----------------------------------------------------------------------------
//            //if (other.tag == "DropedItem")
//            //{
//            //    if (isLocalPlayer && onInteract)
//            //    {
//            //        DropedItemBehavior dropitem = other.GetComponent<DropedItemBehavior>();
//            //        inventoryManager.CmdPickUpitem(dropitem.iteminfo, dropitem.itemData, dropitem.gameObject);
//            //        onInteract = false;
//            //    }
//            //}

//            //  ------------------------------------------------------ devil fruit  ------------------------------------------------------------------------------

//        }

//        private void OnTriggerExit(Collider other)
//        {
//            //  ------------------------------------------------------ chest ------------------------------------------------------------------------------
//            /*if (other.tag == "Chest" && isLocalPlayer)
//            {
//                other.gameObject.GetComponent<OnlineChest>().CloseInteractPanel();
//            }
//            if (other.gameObject.CompareTag("DeadBox") && isLocalPlayer)
//            {
//                other.gameObject.GetComponent<DeadBox>().CloseInteractPanel();

//            }*/
//            /*
//            if (other.tag == "Chest" && isLocalPlayer && other.gameObject.GetComponent<TreasureBoxScript>().isReceive != true)
//            {
//                other.gameObject.GetComponent<TreasureBoxScript>().playerExitTirgger();
//            }
//            */

//            //  ------------------------------------------------------ resoure ----------------------------------------------------------------------------

//            //  ----------------------------------------------------- NPC ---------------------------------------------------------------------------------
//            if (other.tag == "NPC_Shop")
//            {
//                if (isLocalPlayer)
//                {
//                    other.gameObject.GetComponent<NpcShop>().CloseInteractPanel();
//                    GetComponent<ShopManagerController>().CloseShop();
//                }
//            }
//            if (other.tag == "NPC_Blacksmith")
//            {
//                if (isLocalPlayer)
//                {
//                    other.gameObject.GetComponent<NpcBlack>().CloseInteractPanel();
//                }
//            }
//            //  ------------------------------------------------------ DropedItem ----------------------------------------------------------------------------

//            /*if (other.tag == "DropedItem")
//            {
//                if (isLocalPlayer)
//                {
//                    other.gameObject.GetComponent<DropedItemBehavior>().ClosePickUpPanel();
//                }
//            }
//            */
//            //  ------------------------------------------------------ devil fruit  ------------------------------------------------------------------------------

//        }
//        #endregion

//        #region Player Interaction
//        void PlayerInteractCheck()
//        {
//            GameObject currentInteractRayCastTarget = InteractObjectOutliner.Instance.GetCurrentObject();
//            if (currentInteractRayCastTarget == null) return;
//            if ((currentInteractRayCastTarget.CompareTag("Tree") || currentInteractRayCastTarget.CompareTag("Herb") || currentInteractRayCastTarget.CompareTag("Ore")) && onInteract && isLocalPlayer)
//            {
//                currentInteractRayCastTarget.gameObject.layer = 16;
//                currentInteractRayCastTarget.GetComponent<Outlander.ResourecesObject.PickUpResoureObjectScripts>().CmdPlayerPickUp(Player);
//            }
//            else if (currentInteractRayCastTarget.CompareTag("Chest") && onInteract && isLocalPlayer)
//            {
//                if (!currentInteractRayCastTarget.GetComponent<OnlineChest>().isOpen)
//                {
//                    currentInteractRayCastTarget.GetComponent<OnlineChest>().OpenChest();
//                }

//                if (Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().unlockChest)
//                {
//                    Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().unlockChest = false;
//                }
//                else
//                {
//                    Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().unlockChest = true;
//                }

//                Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().DestoryPanal();
//                CmdSetWeaponAction(false);
//            }
//            else if (currentInteractRayCastTarget.CompareTag("DeadBox") && onInteract && isLocalPlayer)
//            {
//                if (Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().unlockChest)
//                {
//                    Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().unlockChest = false;
//                }
//                else
//                {
//                    Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().unlockChest = true;
//                }
//                Camera.main.gameObject.GetComponent<Outlander.InteractObjectOutliner>().DestoryPanal();
//                CmdSetWeaponAction(false);
//            }
//            else if (currentInteractRayCastTarget.CompareTag("DevilFruit") && onInteract && isLocalPlayer)
//            {
//                currentInteractRayCastTarget.gameObject.layer = 16;
//                currentInteractRayCastTarget.GetComponent<Outlander.ResourecesObject.DevilFruitScript>().CmdPlayerPickUp(Player);
//            }
//            else if (currentInteractRayCastTarget.CompareTag("DropedItem") && onInteract && isLocalPlayer)
//            {
//                currentInteractRayCastTarget.gameObject.layer = 16;
//                currentInteractRayCastTarget.GetComponent<DropedItemBehavior>().CmdPlayerPickUp(Player);
//                //inventoryManager.CmdPickUpitem(dropitem.iteminfo, dropitem.itemData, dropitem.gameObject);
//            }
//            /*else if(currentInteractRayCastTarget.CompareTag("NPC_Shop") && onInteract && isLocalPlayer)
//            {
//                currentInteractRayCastTarget.GetComponent<NpcShop>().OpenShop(playerObjectRef.gameObject);
//            }*/
//            onInteract = false;
//            currentInteractRayCastTarget = null;
//        }
//        #endregion

//        [Command]
//        public void SwapCam(string matchId, string email)
//        {
//            GameObject spectatorCam = GetComponent<NotSusScript>().spectatorCam;
//            spectatorCam.GetComponent<NetworkMatch>().matchId = matchId.ToGuid();
//            NetworkServer.ReplacePlayerForConnection(netIdentity.connectionToClient, spectatorCam);
//            SpectatorCam sc = spectatorCam.GetComponent<SpectatorCam>();
//            sc.originPlayer = gameObject;
//            sc.email = email;
//            sc.RpcInitCam(gameObject);
//        }
//    }
//}
