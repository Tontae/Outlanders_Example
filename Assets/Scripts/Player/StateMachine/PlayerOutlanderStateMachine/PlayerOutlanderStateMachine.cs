using Mirror;
using Outlander.Enemy;
using Outlander.Enemy.Bot;
using Outlander.Item;
using Outlander.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

namespace Outlander.Player
{
    public class PlayerOutlanderStateMachine : PlayerElements, IPOutData, IPDamageCommand
    {
        [Header("Handler")]
        [SerializeField, ReadOnlyInspector] public string nameOfState;

        [Space(10)][SerializeField, ReadOnlyInspector] private bool onAction;
        [SerializeField, ReadOnlyInspector] private bool isGod;

        private bool isImmortal;

        // public PlayerDieHelper DieHelper { get; set; }
        public PlayerInteractionHelper InteractionHelper { get; set; }
        public PlayerItemContainer ItemContainer { get; set; }
        public PlayerParameterHelper ParameterHelper { get; set; }

        // public PlayerComponents Player {get => PlayerManagers.Instance.PlayerComponents;}


        [Header("OutlanderState")]
        private OutlanderBaseState currentState;
        private OutlanderStateData stateData;


        #region Player Atrribute
        // Bool
        private bool onAttackState;
        private bool onDie;
        private bool onInteract;
        private bool onSkill;
        private bool onSkillMove;
        private bool onStun;
        private bool onUseConsumable;
        private bool isHit;
        private bool isCounter;

        // Float
        private float playerHP;
        private float playerMaxHP;
        private float playerMP;
        private float playerMaxMP;
        private float playerAtkDmg;
        private float playerAtkSpeed;
        private float playerSkillDmg;
        private float playerDef;
        private float playerCritRate;
        private float playerCritDmg;
        #endregion


        #region Buffer Log
        public Queue<float> playerDamageBufferLog = new Queue<float>();
        public Queue<GameObject> enemyDamageBufferLog = new Queue<GameObject>();
        #endregion


        #region Inspector Assigned Fields
        [Header("Inspecter Attach")]
        [SerializeField] private Collider attackPoint;
        [SerializeField] private GameObject fxHit;
        [SerializeField] private DeadBox deadBox;
        [SerializeField] private LayerMask enemyLayer;
        #endregion


        #region Player Action Parameter
        private bool canReceiveFireInput;
        private bool isAiming;
        private bool onFireInput;
        // private bool onLeftItem;
        // private bool onRightItem;
        private bool onWeaponAction;
        private Dictionary<int, SkillScriptable> playerSkill = new Dictionary<int, SkillScriptable>();
        private int onSkillIndex;
        #endregion


        #region Public Accessor
        // IFrame Handler
        public bool IsGod { get => isGod; set => isGod = value; }
        public bool IsImmortal
        {
            get => isImmortal;
            set
            {
                isImmortal = (IsGod) ? true : value;
                if (isLocalPlayer)
                    CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.IMMORTAL, isImmortal);
            }
        }

        public bool OnAttackState { get => onAttackState; set => onAttackState = value; }
        public bool OnInteract { get => onInteract; set => onInteract = value; }


        // Die
        public bool OnDie
        {
            get => onDie;
            set
            {
                if (onDie != value && value)
                    ClientTriggerEventManager.Instance.IsKilled();
                onDie = value;
                if (Player.PlayerMatchManager.myManager == null) return;
                if (onDie) OnPlayerDie();
            }
        }


        // Combat
        public bool CanReceiveFireInput { get => canReceiveFireInput; set => canReceiveFireInput = value; }
        public bool IsAiming
        {
            get => isAiming;
            set => isAiming = (OnWeaponAction) ? value : false;
        }
        public bool OnAction
        {
            get => onAction;
            set
            {
                onAction = value;
                // OnAction = (Player.MovementStateMachine.IsClimb
                //             || Player.MovementStateMachine.IsDodge
                //             || Player.MovementStateMachine.IsJump
                //             || Player.MovementStateMachine.IsSprint
                //             || Player.MovementStateMachine.IsSwim
                //             || Player.OutlanderStateMachine.OnFireInput
                //             || Player.OutlanderStateMachine.OnStun
                //             || Player.OutlanderStateMachine.OnSkill
                //             || Player.OutlanderStateMachine.OnWeaponAction
                // );
            }
        }
        public bool OnFireInput { get => onFireInput; set => onFireInput = value; }
        // public bool OnLeftItem { get => onLeftItem; set => onLeftItem = value; }
        // public bool OnRightItem { get => onRightItem; set => onRightItem = value; }
        public bool OnUseConsumable { get => onUseConsumable; set => onUseConsumable = value; }
        public bool OnWeaponAction
        {
            get => onWeaponAction; set
            {
                onWeaponAction = value;
                CanReceiveFireInput = !onWeaponAction;
                if (Player.WeaponManager.currentWeaponType != WeaponManager.WeaponType.BowQuiver)
                {
                    Player.Animator.SetBool("weaponaction", onWeaponAction);
                    if (onWeaponAction)
                    {
                        if (Player.WeaponManager.currentWeaponType != WeaponManager.WeaponType.None)
                            Player.Animator.CrossFade("weapon_action", 0.1f);
                    }
                }
            }
        }

        public bool OnSkill { get => onSkill; set => onSkill = value; }
        public bool OnSkillMove { get => onSkillMove; set => onSkillMove = value; }
        public bool OnStun { get => onStun; set => onStun = value; }
        public Dictionary<int, SkillScriptable> PlayerSkill { get => playerSkill; set => playerSkill = value; }
        public int OnSkillIndex { get => onSkillIndex; set => onSkillIndex = value; }
        public bool IsHit { get => isHit; set => isHit = value; }
        public bool IsCounter { get => isCounter; set => isCounter = value; }


        // GameObj
        public Collider AttackPoint { get => attackPoint; set => attackPoint = value; }
        public GameObject FxHit { get => fxHit; set => fxHit = value; }
        public DeadBox DeadBox { get => deadBox; set => deadBox = value; }


        // State
        public OutlanderBaseState CurrentState { get => currentState; set => currentState = value; }
        public OutlanderStateData StateData { get => stateData; set => stateData = value; }
        #endregion


        #region Atrribute Interface
        public float PlayerHP
        {
            get => playerHP;
            set
            {
                playerHP = (value >= playerMaxHP) ? playerMaxHP : value;
                OnDie = (PlayerHP <= 0) ? true : false;
                //if (isServer) return;
                //RpcSetPlayerFloatAttribute(PlayerAttributeFloatType.HP, PlayerHP);
                UIManagers.Instance.playerCanvas.playerHealthImage.fillAmount = playerHP / PlayerMaxHP;
                UIManagers.Instance.playerCanvas.hpTxt.text = $"{playerHP.ToString("0")}/{PlayerMaxHP.ToString()}";
            }
        }
        public float PlayerMP
        {
            get => playerMP;
            set
            {
                playerMP = (value >= playerMaxMP) ? playerMaxMP : value;
                //if (isServer) return;
                //RpcSetPlayerFloatAttribute(PlayerAttributeFloatType.MP, PlayerMP);
                UIManagers.Instance.playerCanvas.playerManaImage.fillAmount = playerMP / PlayerMaxMP;
                UIManagers.Instance.playerCanvas.mpTxt.text = $"{PlayerMP.ToString("0")}/{PlayerMaxMP.ToString()}";
            }
        }
        public float PlayerMaxHP 
        { 
            get => playerMaxHP;
            set 
            { 
                playerMaxHP = value;
                UIManagers.Instance.playerCanvas.playerHealthImage.fillAmount = PlayerHP / playerMaxHP;
                UIManagers.Instance.playerCanvas.hpTxt.text = $"{PlayerHP.ToString("0")}/{playerMaxHP.ToString()}";
            }
        }
        public float PlayerMaxMP 
        { 
            get => playerMaxMP;
            set
            { 
                playerMaxMP = value;
                UIManagers.Instance.playerCanvas.playerManaImage.fillAmount = PlayerMP / playerMaxMP;
                UIManagers.Instance.playerCanvas.mpTxt.text = $"{PlayerMP.ToString("0")}/{playerMaxMP.ToString()}";
            }
        }
        public float PlayerAtkDmg { get => playerAtkDmg; set => playerAtkDmg = value; }
        public float PlayerAtkSpeed { get => playerAtkSpeed; set => playerAtkSpeed = value; }
        public float PlayerSkillDmg { get => playerSkillDmg; set => playerSkillDmg = value; }
        public float PlayerDef { get => playerDef; set => playerDef = value; }
        public float PlayerCritRate { get => playerCritRate; set => playerCritRate = value; }
        public float PlayerCritDmg { get => playerCritDmg; set => playerCritDmg = value; }
        #endregion


        #region BufferLog Interface
        public Queue<float> PlayerDamageBufferLog { get => playerDamageBufferLog; set => playerDamageBufferLog = value; }
        public Queue<GameObject> EnemyDamageBufferLog { get => enemyDamageBufferLog; set => enemyDamageBufferLog = value; }
        #endregion


        #region Network Event message
        [TargetRpc]
        private void RpcSetPlayerFloatAttribute(PlayerAttributeFloatType _type, float _newValue)
        {
            switch (_type)
            {
                case PlayerAttributeFloatType.HP:
                    PlayerHP = _newValue;
                    break;
                case PlayerAttributeFloatType.MP:
                    PlayerMP = _newValue;
                    break;
                case PlayerAttributeFloatType.MAXHP:
                    PlayerMaxHP = _newValue;
                    break;
                case PlayerAttributeFloatType.MAXMP:
                    PlayerMaxMP = _newValue;
                    break;
                case PlayerAttributeFloatType.ATK:
                    PlayerAtkDmg = _newValue;
                    break;
                case PlayerAttributeFloatType.DEF:
                    PlayerDef = _newValue;
                    break;
                case PlayerAttributeFloatType.CRITICAL_RATE:
                    PlayerCritRate = _newValue;
                    break;
                case PlayerAttributeFloatType.CRITICAL_DAMAGE:
                    PlayerCritDmg = _newValue;
                    break;
            }

            //ParameterHelper.SetParameter(ParameterHelper.GetParameter(_type), _newValue);
        }

        [Command]
        public void CmdSetPlayerFloatAttribute(PlayerAttributeFloatType _type, float _newValue)
        {
            switch (_type)
            {
                case PlayerAttributeFloatType.HP:
                    PlayerHP = _newValue;
                    break;
                case PlayerAttributeFloatType.MP:
                    PlayerMP = _newValue;
                    break;
                case PlayerAttributeFloatType.MAXHP:
                    PlayerMaxHP = _newValue;
                    break;
                case PlayerAttributeFloatType.MAXMP:
                    PlayerMaxMP = _newValue;
                    break;
                case PlayerAttributeFloatType.ATK:
                    PlayerAtkDmg = _newValue;
                    break;
                case PlayerAttributeFloatType.DEF:
                    PlayerDef = _newValue;
                    break;
                case PlayerAttributeFloatType.CRITICAL_RATE:
                    PlayerCritRate = _newValue;
                    break;
                case PlayerAttributeFloatType.CRITICAL_DAMAGE:
                    PlayerCritDmg = _newValue;
                    break;
            }

            //ParameterHelper.SetParameter(ParameterHelper.GetParameter(_type), _newValue);
        }

        [TargetRpc]
        public void RpcSetPlayerBoolAttribute(PlayerAttributeBoolType _type, bool _newValue)
        {
            switch (_type)
            {
                case PlayerAttributeBoolType.DIE:
                    OnDie = _newValue;
                    break;
                case PlayerAttributeBoolType.GODMODE:
                    IsGod = _newValue;
                    break;
                case PlayerAttributeBoolType.IMMORTAL:
                    IsImmortal = _newValue;
                    break;
                case PlayerAttributeBoolType.NORMAL_ATTACK:
                    OnFireInput = _newValue;
                    break;
                case PlayerAttributeBoolType.WEAPON_ACTION:
                    OnWeaponAction = _newValue;
                    break;
                case PlayerAttributeBoolType.HIT:
                    IsHit = _newValue;
                    break;
                case PlayerAttributeBoolType.COUNTER:
                    IsCounter = _newValue;
                    break;
                case PlayerAttributeBoolType.ONSKILL:
                    OnSkill = _newValue;
                    break;
            }

            //ParameterHelper.SetParameter(ParameterHelper.GetParameter(_type), _newValue);
        }

        [Command]
        public void CmdSetPlayerBoolAttribute(PlayerAttributeBoolType _type, bool _newValue)
        {
            switch (_type)
            {
                case PlayerAttributeBoolType.DIE:
                    OnDie = _newValue;
                    break;
                case PlayerAttributeBoolType.GODMODE:
                    IsGod = _newValue;
                    break;
                case PlayerAttributeBoolType.IMMORTAL:
                    IsImmortal = _newValue;
                    break;
                case PlayerAttributeBoolType.NORMAL_ATTACK:
                    OnFireInput = _newValue;
                    break;
                case PlayerAttributeBoolType.WEAPON_ACTION:
                    OnWeaponAction = _newValue;
                    break;
                case PlayerAttributeBoolType.HIT:
                    IsHit = _newValue;
                    break;
                case PlayerAttributeBoolType.COUNTER:
                    IsCounter = _newValue;
                    break;
                case PlayerAttributeBoolType.ONSKILL:
                    OnSkill = _newValue;
                    break;
            }

            //ParameterHelper.SetParameter(ParameterHelper.GetParameter(_type), _newValue);
        }

        [ClientRpc]
        public void RpcBroadcastKill(string _killer, string _type, string _victim)
        {
            PlayerManagers.Instance.matchManager.OnReceiveKillActivity(_killer, _type, _victim);
        }

        [TargetRpc]
        void RpcSetCounter()
        {
            Player.Animator.SetBool("isHit", true);
        }
        #endregion

        #region Unity Event message
        private void Start()
        {
            // playerCamera = GetComponent<PlayerCamera>();
            // playerMovement = GetComponent<PlayerMovementStateMachine>();
            // weaponManager = GetComponent<WeaponManager>();
            // DieHelper = new PlayerDieHelper(this);
            //if (isClient)

            ParameterHelper = new PlayerParameterHelper(this);
            if (!isLocalPlayer) return;

            StateData = new OutlanderStateData(this);
            InteractionHelper = new PlayerInteractionHelper(this);
            ItemContainer = new Item.PlayerItemContainer(this);

            InitializeAnimEvent();
            InitializeCharacterData();

            CurrentState = StateData.Hand();
            CurrentState.EnterState();
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            CurrentState.UpdateState();
        }
        #endregion

        #region SetData
        public bool IsPlayerOnAction()
        {
            return OnAction = (Player.MovementStateMachine.IsClimb
                            || Player.MovementStateMachine.IsDodge
                            || Player.MovementStateMachine.IsCrouch
                            || Player.MovementStateMachine.IsJump
                            || Player.MovementStateMachine.IsSprint
                            || Player.MovementStateMachine.IsSwim
                            || Player.OutlanderStateMachine.OnFireInput
                            || Player.OutlanderStateMachine.OnStun
                            || Player.OutlanderStateMachine.OnSkill
                            || Player.OutlanderStateMachine.OnWeaponAction
                );
        }

        [Command]
        public void CmdInitializingPlayer()
        {
            InitializeCharacterData();
        }

        public void InitializeAnimEvent()
        {
            if (!isLocalPlayer) return;
            Player.PlayerAnimationEvent.OnAttackHit += AttackHit;
        }

        public void InitializeCharacterData()
        {
            if (isLocalPlayer)
            {
                if (!NetworkClient.ready)
                    NetworkClient.Ready();
                CmdInitializingPlayer();
            }
            if (isLocalPlayer || isServer)
            {
                
                ResetPlayerBoolData();

                gameObject.layer = LayerMask.NameToLayer("Player");

                Player.PlayerStatisticManager.InitializingPlayerStat();
                Player.InventoryManager.ClearAllItemsInInventory();
                Player.RuneSystemManager.ResetPlayerBag();
                Player.PlayerStamina.InitializeStamina();

                PlayerHP = PlayerMaxHP;
                PlayerMP = PlayerMaxMP;
            }
        }

        public void ResetPlayerBoolData()
        {
            CanReceiveFireInput = true;

            OnDie = false;
            OnSkill = false;
            OnWeaponAction = false;
        }

        public void SetCharacterStats(PlayerStatisticManager.PlayerStatistic playerStat)
        {
            PlayerAtkDmg = playerStat.ATK;
            PlayerMaxHP = playerStat.MHP;
            PlayerMaxMP = playerStat.MMP;
            PlayerDef = playerStat.DEF;
            PlayerCritRate = playerStat.CRIT;
            PlayerCritDmg = playerStat.CRITDAMAGE;
            PlayerAtkSpeed = playerStat.ATKSPD;

            Player.MovementStateMachine.UpdateMovementSpeed();
        }

        public void CalculatePlayerAttributeValue(PlayerAttributeFloatType _type, float _amount)
        {
            float _param = ParameterHelper.SetParameter(_type, _amount);
            if (isServer)
                RpcSetPlayerFloatAttribute(_type, _param);
            if (isClient)
                CmdSetPlayerFloatAttribute(_type, _param);
        }

        void SetCounterState(int state)
        {
            if (!isLocalPlayer) return;
            CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.COUNTER, System.Convert.ToBoolean(state));
        }
        #endregion


        #region Combat

        public void CanRecieveAttackInput()
        {
            if (!CanReceiveFireInput)
            {
                CanReceiveFireInput = true;
                OnFireInput = false;
            }
            else
            {
                CanReceiveFireInput = false;
            }
        }
        public void CheckDamageBuffer()
        {
            if (IsGod)
                return;
            if (Player.PlayerMatchManager.myManager.matchStatus == MatchStatus.IsEnd) 
                return;
            if (isCounter && PlayerDamageBufferLog.Count > 0)
            {
                isCounter = false;
                PlayerDamageBufferLog.Dequeue();
                EnemyDamageBufferLog.Dequeue();
                if (isServer)
                {
                    RpcSetCounter();
                    Player.PlayerUIManager.TargetShowDamagePopUp("Block", Color.white);
                }

            }

            if (PlayerDamageBufferLog.Count > 0)
            {
                if (EnemyDamageBufferLog.Count > 1)
                    EnemyDamageBufferLog.Dequeue();

                float tempDamage = PlayerDamageBufferLog.Dequeue();

                tempDamage = tempDamage / ((PlayerDef + 100f) * 0.01f);
                if (onWeaponAction) tempDamage = tempDamage * 0.5f;
                tempDamage = Mathf.Clamp(tempDamage, 1f, float.MaxValue);

                //PlayerHP -= tempDamage;
                //RpcSetPlayerFloatAttribute(PlayerAttributeFloatType.HP, PlayerHP);
                CalculatePlayerAttributeValue(PlayerAttributeFloatType.HP, -tempDamage);

                if (PlayerHP <= 0) return;

                if (EnemyDamageBufferLog.Peek() == null) return;

                if (!EnemyDamageBufferLog.Peek().name.Equals("REDZONE"))
                    RpcSetPlayerBoolAttribute(PlayerAttributeBoolType.HIT, true);
                
                Player.PlayerUIManager.TargetShowDamagePopUp(tempDamage, Color.red);

                //Player.EquipmentManager.equipedDic.TryGetValue(Character.SuitParts.Main_Weapon, out InventoryItemBehavior playerWeapon);
                ItemScriptable playerWeapon = OutlanderDB.singleton.GetItemScriptable(Player.PlayerScriptable.equipeditemList[0] == null ? string.Empty : Player.PlayerScriptable.equipeditemList[0]);

                if (Player.PlayerMatchManager.myManager.GameObjectComponents.TryGetValue(EnemyDamageBufferLog.Peek(), out object targetComponent))
                {
                    ItemScriptable targetWeapon;
                    switch (targetComponent)
                    {
                        case PlayerComponents targetPlayer:
                            //PlayerComponents targetPlayer = targetComponent as PlayerComponents;
                            targetWeapon = OutlanderDB.singleton.GetItemScriptable(targetPlayer.PlayerScriptable.equipeditemList[0] == null ? string.Empty : targetPlayer.PlayerScriptable.equipeditemList[0]);
                            Player.PlayerUIManager.TargetPlayerIsHitSfx(playerWeapon?.itemName, targetWeapon?.itemName, onWeaponAction, false);
                            Player.PlayerStatisticsData.UpdatePlayerIsDamaged("Player", tempDamage, onWeaponAction);
                            return;
                        case BotBehaviorManager targetBot:
                            //BotBehaviorManager targetBot = targetComponent as BotBehaviorManager;
                            targetWeapon = targetBot.Inventory.Weapons_inventory.Count == 0 ? null : targetBot.Inventory.Weapons_inventory[0];
                            Player.PlayerUIManager.TargetPlayerIsHitSfx(playerWeapon?.itemName, targetWeapon?.itemName, onWeaponAction, false);
                            Player.PlayerStatisticsData.UpdatePlayerIsDamaged("Bot", tempDamage, onWeaponAction);
                            return;
                        case Monster_Base targetMonster:
                            //Monster_Base targetMonster = targetComponent as Monster_Base;
                            Player.PlayerStatisticsData.UpdatePlayerIsDamaged(targetMonster.ui.enemyName, tempDamage, onWeaponAction);
                            return;
                        default:
                            Player.PlayerStatisticsData.UpdatePlayerIsDamaged(EnemyDamageBufferLog.Peek().name, tempDamage, false);
                            break;
                    }
                }

                Player.PlayerUIManager.TargetPlayerIsHitSfx(playerWeapon?.itemName, string.Empty, onWeaponAction, EnemyDamageBufferLog.Peek().name.Equals("REDZONE"));
            }
        }

        public void AttackHit()
        {
            if (PlayerManagers.Instance.matchManager == null)
                return;
            if (!isLocalPlayer)
                return;
            if (Player.WeaponManager.currentWeaponType == WeaponManager.WeaponType.BowQuiver)
                return;
            if (Player.PlayerStamina.IsMin)
                return;
            if (!PlayerManagers.Instance.matchManager.canInteract)
                return;

            if (!OnSkill)
            {
                Player.PlayerStamina.DecreaseStamina(20);
            }
            else
            {
                Player.PlayerSkillManager.MultiHitSkill();
            }

            Player.PlayerStamina.RegenCooldown = 2f;

            Collider[] hitnEnemies = Physics.OverlapBox(AttackPoint.bounds.center, AttackPoint.bounds.extents, AttackPoint.transform.rotation, enemyLayer);

            foreach (Collider col in hitnEnemies)
            {
                if (col.name.Equals(name)) 
                    return;
                else if (col.gameObject.layer == 9 || col.gameObject.layer == 8)
                    CmdDamage(col.gameObject.GetComponent<NetworkIdentity>().netId);
            }
        }

        [Command]
        public void CmdDamage(GameObject target)
        {
            if (target == null) return;
            //Debug.Log($"{target}", target);

            if (target.layer == 8)
                if (Vector3.Distance(transform.position, target.transform.position) > 5f) return;

            bool isCrit = Random.Range(0.0f, 100.0f) <= PlayerCritRate;
            float outDamage = OnSkill ? PlayerSkillDmg : PlayerAtkDmg;
            outDamage = isCrit ? outDamage * PlayerCritDmg : outDamage;
            outDamage -= Random.Range(0, outDamage * 0.2f);

            if(Player.PlayerMatchManager.myManager.GameObjectComponents.TryGetValue(target, out object targetComponent))
            {
                ItemScriptable playerWeapon = OutlanderDB.singleton.GetItemScriptable(Player.PlayerScriptable.equipeditemList[0] == null ? string.Empty : Player.PlayerScriptable.equipeditemList[0]);
                switch (targetComponent)
                {
                    case PlayerComponents targetPlayer:
                        //PlayerComponents targetPlayer = targetComponent as PlayerComponents;
                        targetPlayer.OutlanderStateMachine.Damage(gameObject, outDamage);
                        if (targetPlayer.OutlanderStateMachine.IsCounter)
                        {
                            Player.PlayerUIManager.TargetShowDamagePopUp("Block", Color.white, AttackPoint.bounds.center);
                            return;
                        }
                        float damageDef = outDamage / ((targetPlayer.OutlanderStateMachine.PlayerDef + 100f) * 0.01f);
                        if (targetPlayer.OutlanderStateMachine.OnWeaponAction)
                            damageDef = Mathf.Clamp(damageDef * 0.5f, 1, float.MaxValue);

                        Player.PlayerUIManager.TargetShowDamagePopUp(damageDef, isCrit ? Color.yellow : Color.white, target.transform.position);

                        //ItemScriptable playerWeapon = OutlanderDB.singleton.GetItemScriptable(Player.playerScriptable.equipeditemList[0] == null ? string.Empty : Player.playerScriptable.equipeditemList[0]);
                        ItemScriptable targetWeapon = OutlanderDB.singleton.GetItemScriptable(targetPlayer.PlayerScriptable.equipeditemList[0] == null ? string.Empty : targetPlayer.PlayerScriptable.equipeditemList[0]);

                        Player.PlayerUIManager.TargetPlayerHitSfx(playerWeapon?.itemName, targetWeapon?.itemName, targetPlayer.OutlanderStateMachine.OnWeaponAction, target.transform.position);
                        break;
                    case BotBehaviorManager targetBot:
                        //BotBehaviorManager targetBot = targetComponent as BotBehaviorManager;
                        targetBot.Damage(gameObject, outDamage);
                        float damageBotDef = outDamage / ((targetBot.Stat.GetFinalStat(StatusType.DEF) + 100f) * 0.01f);
                        Player.PlayerUIManager.TargetShowDamagePopUp(damageBotDef, isCrit ? Color.yellow : Color.white, target.transform.position);

                        //Player.EquipmentManager.equipedDic.TryGetValue(Character.SuitParts.Main_Weapon, out InventoryItemBehavior playerWeapon2);
                        Player.PlayerUIManager.TargetPlayerHitSfx(playerWeapon?.itemName, string.Empty, false, target.transform.position);
                        break;
                    case Monster_Base targetEnemy:
                        //Monster_Base targetEnemy = targetComponent as Monster_Base;
                        targetEnemy.Damage(outDamage, gameObject);
                        float damageEnemyDef = outDamage / ((targetEnemy.Def + 100f) * 0.01f);
                        Player.PlayerUIManager.TargetShowDamagePopUp(damageEnemyDef, isCrit ? Color.yellow : Color.white, target.transform.position);

                        //Player.EquipmentManager.equipedDic.TryGetValue(Character.SuitParts.Main_Weapon, out InventoryItemBehavior playerWeapon1);
                        Player.PlayerUIManager.TargetPlayerHitSfx(playerWeapon?.itemName, string.Empty, false, target.transform.position);
                        break;
                }
            }
        }

        [Command]
        public void CmdDamage(uint targetId)
        {
            if (!NetworkServer.spawned.TryGetValue(targetId, out NetworkIdentity ni))
                return;
            GameObject target = ni.gameObject;
            if (target.layer == 8)
                if (Vector3.Distance(transform.position, target.transform.position) > 5f) return;

            bool isCrit = Random.Range(0.0f, 100.0f) <= PlayerCritRate;
            float outDamage = OnSkill ? PlayerSkillDmg : PlayerAtkDmg;
            outDamage = isCrit ? outDamage * PlayerCritDmg : outDamage;
            outDamage -= Random.Range(0, outDamage * 0.2f);

            if (Player.PlayerMatchManager.myManager.GameObjectComponents.TryGetValue(target, out object targetComponent))
            {
                ItemScriptable playerWeapon = OutlanderDB.singleton.GetItemScriptable(Player.PlayerScriptable.equipeditemList[0] == null ? string.Empty : Player.PlayerScriptable.equipeditemList[0]);
                switch (targetComponent)
                {
                    case PlayerComponents targetPlayer:
                        //PlayerComponents targetPlayer = targetComponent as PlayerComponents;
                        targetPlayer.OutlanderStateMachine.Damage(gameObject, outDamage);
                        if (targetPlayer.OutlanderStateMachine.IsCounter)
                        {
                            Player.PlayerUIManager.TargetShowDamagePopUp("Block", Color.white, AttackPoint.bounds.center);
                            return;
                        }
                        float damageDef = outDamage / ((targetPlayer.OutlanderStateMachine.PlayerDef + 100f) * 0.01f);
                        if (targetPlayer.OutlanderStateMachine.OnWeaponAction)
                            damageDef = Mathf.Clamp(damageDef * 0.5f, 1, float.MaxValue);

                        Player.PlayerUIManager.TargetShowDamagePopUp(damageDef, isCrit ? Color.yellow : Color.white, target.transform.position);

                        //ItemScriptable playerWeapon = OutlanderDB.singleton.GetItemScriptable(Player.playerScriptable.equipeditemList[0] == null ? string.Empty : Player.playerScriptable.equipeditemList[0]);
                        ItemScriptable targetWeapon = OutlanderDB.singleton.GetItemScriptable(targetPlayer.PlayerScriptable.equipeditemList[0] == null ? string.Empty : targetPlayer.PlayerScriptable.equipeditemList[0]);

                        Player.PlayerUIManager.TargetPlayerHitSfx(playerWeapon?.itemName, targetWeapon?.itemName, targetPlayer.OutlanderStateMachine.OnWeaponAction, target.transform.position);

                        Player.PlayerStatisticsData.UpdatePlayerDoDamage("Player", Player.WeaponManager.currentWeaponType, isCrit, damageDef,false,false);
                        //if (!Player.PlayerMatchManager.playerDamageType.TryAdd(Player.WeaponManager.currentWeaponType, damageDef))
                            //Player.PlayerMatchManager.playerDamageType[Player.WeaponManager.currentWeaponType] += damageDef;
                        
                        break;
                    case BotBehaviorManager targetBot:
                        //BotBehaviorManager targetBot = targetComponent as BotBehaviorManager;
                        targetBot.Damage(gameObject, outDamage);
                        float damageBotDef = outDamage / ((targetBot.Stat.GetFinalStat(StatusType.DEF) + 100f) * 0.01f);
                        Player.PlayerUIManager.TargetShowDamagePopUp(damageBotDef, isCrit ? Color.yellow : Color.white, target.transform.position);

                        //ItemScriptable playerWeapon2 = OutlanderDB.singleton.GetItemScriptable(Player.playerScriptable.equipeditemList[0] == null ? string.Empty : Player.playerScriptable.equipeditemList[0]);

                        Player.PlayerUIManager.TargetPlayerHitSfx(playerWeapon?.itemName, string.Empty, false, target.transform.position);

                        Player.PlayerStatisticsData.UpdatePlayerDoDamage("Bot", Player.WeaponManager.currentWeaponType, isCrit, damageBotDef, false, false);
                        break;
                    case Monster_Base targetEnemy:
                        //Monster_Base targetEnemy = targetComponent as Monster_Base;
                        targetEnemy.Damage(outDamage, gameObject);
                        float damageEnemyDef = outDamage / ((targetEnemy.Def + 100f) * 0.01f);
                        Player.PlayerUIManager.TargetShowDamagePopUp(damageEnemyDef, isCrit ? Color.yellow : Color.white, target.transform.position);

                        //ItemScriptable playerWeapon1 = OutlanderDB.singleton.GetItemScriptable(Player.playerScriptable.equipeditemList[0] == null ? string.Empty : Player.playerScriptable.equipeditemList[0]);

                        Player.PlayerUIManager.TargetPlayerHitSfx(playerWeapon?.itemName, string.Empty, false, AttackPoint.bounds.center);

                        Player.PlayerStatisticsData.UpdatePlayerDoDamage(targetEnemy.ui.enemyName, Player.WeaponManager.currentWeaponType, isCrit, damageEnemyDef,targetEnemy.IsBoss,targetEnemy.IsMiniBoss);
                        break;
                }
            }

        }

        [Command(requiresAuthority = false)]
        public void CmdDamageIgnoreDefense(PlayerDamageRecieveType _type, float _damage)
        {
            if (PlayerHP <= 0) return;

            float redzoneDamadge = OnWeaponAction ? _damage * 2f * ((PlayerDef + 100f) * 0.01f) : _damage * ((PlayerDef + 100f) * 0.01f);
            GameObject temp = new GameObject(_type.ToString());
            Destroy(temp, 1f);

            Damage(temp, redzoneDamadge);
        }

        public void Damage(GameObject _obj, float _damage)
        {
            if (PlayerHP <= 0) return;
            PlayerDamageBufferLog.Enqueue(_damage);
            EnemyDamageBufferLog.Enqueue(_obj);
            CheckDamageBuffer();

            Player.PlayerStatisticsData.DamageNames.Add(new PlayerStatisticsData.DamageName { time = Player.PlayerMatchManager.myManager.gameTime, name = _obj.name, damage = _damage });
        }

        public void Damage(PlayerDamageRecieveType _type, float _damage)
        {
            if (PlayerHP <= 0) return;
            GameObject temp = new GameObject(_type.ToString());
            Destroy(temp, 1f);

            Damage(temp, _damage);
        }

        [Command]
        public void CmdSendDamage(uint _owner, float _damage)
        {
            if (PlayerHP <= 0) return;

            Damage(NetworkServer.spawned[_owner].gameObject, _damage);
        }
        #endregion


        #region Die
        public void OnPlayerDie()
        {
            if (isLocalPlayer)
                DropAllItemOnDie();

            gameObject.layer = 0;

            if (!isServer) return;
            //Debug.Log($"Server last enemy hit:{EnemyDamageBufferLog.Peek()} {EnemyDamageBufferLog.Count}");

            if (EnemyDamageBufferLog.TryDequeue(out GameObject lastPlayerHit))
            {
                if (Player.PlayerMatchManager.myManager.GameObjectComponents.TryGetValue(lastPlayerHit, out object targetComponent))
                {
                    switch (targetComponent)
                    {
                        case PlayerComponents killerComp:
                            //PlayerComponents killerComp = targetComponent as PlayerComponents;
                            Player.PlayerMatchManager.myManager.IncreaseKillRank(killerComp, Player);
                            RpcBroadcastKill(lastPlayerHit.name, killerComp.WeaponManager.currentWeaponType.ToString(), name);
                            //if (!Player.PlayerMatchManager.myManager.isBountyEnd)
                            //    if (Player.PlayerMatchManager.myManager.selectedBounty == netIdentity)
                            //        if (Player.PlayerMatchManager.myManager.GameObjectComponents[Player.PlayerMatchManager.myManager.selectedBounty.gameObject] as PlayerComponents != null)
                            //            killerComp.PlayerMatchManager.myManager.GetBountyReward(killerComp.PlayerIdentity.connectionToClient);
                            //if (!killerComp.PlayerMatchManager.playerKillType.TryAdd(killerComp.WeaponManager.currentWeaponType, 1))
                            //    killerComp.PlayerMatchManager.playerKillType[killerComp.WeaponManager.currentWeaponType] += 1;
                            break;
                        case Monster_Base iMons:
                            //Monster_Base iMons = targetComponent as Monster_Base;
                            string enemy = iMons.IsBoss ? PlayerDamageRecieveType.BOSS.ToString() : PlayerDamageRecieveType.MONSTER.ToString();
                            RpcBroadcastKill(iMons.ui.enemyName, enemy, name);
                            break;
                        case BotBehaviorManager bot:
                            //BotBehaviorManager bot = targetComponent as BotBehaviorManager;
                            Player.PlayerMatchManager.myManager.IncreaseKillRank(bot.netIdentity, netIdentity);
                            RpcBroadcastKill(lastPlayerHit.name, bot.Stat.curWeaponType.ToString(), name);
                            break;
                    }
                }
                else
                {
                    RpcBroadcastKill("", lastPlayerHit.name, name);
                }

                Player.PlayerMatchManager.myManager?.RemovePlayerFromMatchManger(Player);
            }
        }

        public void DropAllItemOnDie()
        {
            //Debug.Log($"Drop:{Player.InventoryManager.itemObjectInBag.Count}");
            if (Player.InventoryManager.itemObjectInBag.Count == 0) return;

            List<RemoveItemInfo> removitemList = new List<RemoveItemInfo>();
            List<ItemScriptable> items = new List<ItemScriptable>();
            List<int> amounts = new List<int>();
            foreach (InventoryItemBehavior item in Player.InventoryManager.itemObjectInBag)
            {
                //Debug.Log($"Drop:{item.thisItem.itemName} {item.thisItem.mainType.ToString()}");
                if (item.thisItem.mainType == Type.Equipment)
                {
                    if (item.IsEquiped)
                        Player.EquipmentManager.ForceUnequipItem(item);
                }
                else if (item.thisItem.mainType == Type.MainWeapon)
                {
                    if (item.IsEquiped)
                        Player.EquipmentManager.ForceUnequipItem(item);
                }
                else if (item.thisItem.mainType == Type.SubWeapon)
                {
                    if (item.IsEquiped)
                        Player.EquipmentManager.ForceUnequipItem(item);
                }
                else if (item.thisItem.mainType == Type.Rune)
                {
                    if (item.IsEquiped)
                        Player.RuneSystemManager.UnEquipRune(item);
                }
                items.Add(item.thisItem);
                amounts.Add(item.Amount);
                removitemList.Add(new RemoveItemInfo
                {
                    removeItemObject = item,
                    qty = item.Amount
                });
            }

            if (Player.PlayerInventoryController.Bronze > 0)
            {
                items.Add(OutlanderDB.singleton.GetItemScriptable("000"));
                amounts.Add(Player.PlayerInventoryController.Bronze);
                Player.PlayerInventoryController.Bronze = 0;
            }

            Player.InventoryManager.RemoveItemFromInventory(removitemList);
            Player.InventoryManager.equipedGeneralDictionary.Clear();
            Player.InventoryManager.SetItemSlotToDefaultSprite();
            //playerObjectRef.GetComponent<PlayerOutlander>().CmdInstantiateDropAllObj(items, amounts);
            CmdInstantiateDropAllObj(items, amounts);

            Player.PlayerInventoryController.Bronze = 0;
        }

        [Command]
        private void CmdInstantiateDropAllObj(List<ItemScriptable> items, List<int> amounts)
        {
            Vector3 objectposition = gameObject.transform.position + new Vector3(0, 0, 1);
            DeadBox newDropItem = Instantiate(DeadBox, objectposition, Quaternion.identity);

            newDropItem.AssignDropItemData(items, amounts);

            NetworkServer.Spawn(newDropItem.gameObject);

            MatchMaker.Instance.matchData.matchManager.chestSpawner.matchChests.Add(newDropItem.gameObject);
            if (MatchMaker.Instance.matchData.matchManager != null)
                BotDataHub.instance.distanceChests.Add(newDropItem.gameObject, Vector3.Distance(new Vector3(MatchMaker.Instance.matchData.matchManager.redZoneRef.CurZone.x, 0f, MatchMaker.Instance.matchData.matchManager.redZoneRef.CurZone.z), new Vector3(newDropItem.transform.position.x, 0f, newDropItem.transform.position.z)));
            else
                BotDataHub.instance.distanceChests.Add(newDropItem.gameObject, 0f);
        }
        #endregion


        #region Interaction
        public void OnTriggerEnter(Collider other)
        {
            if (!isLocalPlayer) return;
            InteractionHelper.OnPlayerTriggerEnter(other);
        }

        /*private void OnTriggerStay(Collider other)
        {
            if (!isLocalPlayer) return;
            InteractionHelper.OnPlayerTriggerStay(other);
        }*/

        private void OnTriggerExit(Collider other)
        {
            if (!isLocalPlayer) return;
            InteractionHelper.OnPlayerTriggerExit(other);
        }
        #endregion


        #region Input message
        public void FireInput(bool value)
        {
            if (Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CLIMB
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.DODGE
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.SWIM
                ) return;

            if (!CanReceiveFireInput)
            {
                OnFireInput = false;
                return;
            }
            if (Cursor.visible)
            {
                OnFireInput = false;
                return;
            }

            OnFireInput = value;
            OnAttackState = true;

            if (Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CROUCH)
                Player.MovementStateMachine.IsCrouch = false;

            // if (PlayerStamina.IsMin) return;
            // PlayerStamina.DecreaseStamina(20);
        }

        public void SkillInput(int skillIndex, bool value)
        {
            if (Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CLIMB
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.DODGE
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.SWIM
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.FALL
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.JUMP
                || OnWeaponAction
                || Cursor.visible
                ) return;
            // Debug.Log(value);
            // OnSkillIndex = skillIndex;
            if (Cursor.visible)
                return;
            
            if (!PlayerSkill.ContainsKey(skillIndex))
                return;
            if (PlayerSkill[skillIndex] == null)
                return;

            if (Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CROUCH)
                Player.MovementStateMachine.IsCrouch = false;
            if (Player.MovementStateMachine.IsSprint)   
                Player.MovementStateMachine.IsSprint = false;

            if (Player.PlayerSkillManager.UseSkill(PlayerSkill[skillIndex]))
            {
                CalculatePlayerAttributeValue(PlayerAttributeFloatType.MP, -PlayerSkill[skillIndex].manaUsage);
                OnSkill = true;
                OnSkillMove = PlayerSkill[skillIndex].canMove;
                OnAttackState = true;
                Player.Animator.CrossFade(PlayerSkill[skillIndex].skillAnimName, 0.1f);

                Player.PlayerStatisticsData.SkillUseCount += 1;
            }
        }

        public void WeaponActionInput(bool value)
        {
            //if (IsPlayerOnAction()) return;
            if (Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CLIMB
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.DODGE
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CROUCH
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.JUMP
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.SWIM
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.FALL
                || OnFireInput
                || OnStun
                || OnSkill
                || Cursor.visible
                || !CanReceiveFireInput)
            {
                if (OnWeaponAction)
                {
                    Player.OutlanderStateMachine.OnWeaponAction = false;
                    CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.WEAPON_ACTION, false);
                }
                return;
            }

            if (isLocalPlayer)
            {
                Player.OutlanderStateMachine.OnWeaponAction = value;
                CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.WEAPON_ACTION, value);
            }
        }

        public void LeftItemInput(bool value)
        {
            if (Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CLIMB
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.DODGE
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.FALL
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.JUMP
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.SWIM
                || Player.MovementStateMachine.MovementSubStateIndex == PlayerMovementState.RUN
                || OnFireInput
                || OnStun
                || OnSkill
                || OnWeaponAction
                ) return;
            // OnLeftItem = value;

            ItemContainer.UseItem(1);
        }

        public void RightItemInput(bool value)
        {
            if (Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CLIMB
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.DODGE
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.FALL
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.JUMP
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.SWIM
                || Player.MovementStateMachine.MovementSubStateIndex == PlayerMovementState.RUN
                || OnFireInput
                || OnStun
                || OnSkill
                || OnWeaponAction
                ) return;
            // OnRiOnStunghtItem = value;

            ItemContainer.UseItem(2);
        }

        public void InteractInput(bool value)
        {
            //Debug.Log($"OnInteract:{OnInteract} > {value}");
            OnInteract = value;
            InteractionHelper.PlayerInteractCheck();
        }
        #endregion
    }
}

