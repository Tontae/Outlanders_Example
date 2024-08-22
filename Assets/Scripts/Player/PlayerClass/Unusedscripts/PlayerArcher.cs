// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Mirror;

// namespace Outlander.Player
// {
//     public class PlayerArcher : PlayerOutlander
//     {
//         //         public static PlayerArcher playerArcher;
//         //         protected delegate void Callback();
//         //         private ParticleSystem ps;
//         //         private Gradient attackFXcolor = new Gradient();
//         //         private Gradient skillFXcolor = new Gradient();

//         //         [Header("Arrow Prefabs")]
//         //         //[SerializeField] private GameObject arrowObject;
//         //         private bool isShooting;

//         //         #region Main
//         //         public override void OnStartAuthority()
//         //         {
//         //             base.OnStartAuthority();

//         //             PlayerInput playerInput = playerObjectRef.GetComponent<PlayerInput>();
//         //             playerInput.enabled = true;
//         //         }

//         //         protected override void OnEnable()
//         //         {
//         //             //base.OnEnable();

//         //             // playerAnimationEvent = new PlayerAnimationEvent();
//         //             playerAnimationEvent = playerObjectRef.GetComponent<PlayerAnimationEvent>();
//         //             playerUIActionHandler = playerObjectRef.GetComponent<Outlander.UI.PlayerUIActionHandler>();

//         //             playerAnimationEvent.OnAttackHit += AttackHit;
//         //             playerAnimationEvent.OnCombo += Combo;
//         //             playerAnimationEvent.OnComboPossible += ComboPossible;
//         //             playerAnimationEvent.OnComboReset += ComboReset;
//         //             playerAnimationEvent.OnEndSkill += EndSkill;
//         //             playerAnimationEvent.OnResetStage += ResetStage;
//         //             playerAnimationEvent.OnIdle += OnIdle;
//         //             playerAnimationEvent.OnShooting += ShootingAction;

//         //             playerUIActionHandler.OnUILeftItem += LeftItem;
//         //             playerUIActionHandler.OnUIRightItem += RightItem;

//         //             anim = playerObjectRef.GetComponent<Animator>();
//         //             anim.runtimeAnimatorController = runtimeAnimator;
//         //             playerObjectRef.GetComponent<IPlayer>().RuntimeAnimator = runtimeAnimator;
//         //         }

//         //         protected override void OnDisable()
//         //         {
//         //             //base.OnDisable();

//         //             playerAnimationEvent.OnAttackHit -= AttackHit;
//         //             playerAnimationEvent.OnCombo -= Combo;
//         //             playerAnimationEvent.OnComboPossible -= ComboPossible;
//         //             playerAnimationEvent.OnComboReset -= ComboReset;
//         //             playerAnimationEvent.OnEndSkill -= EndSkill;
//         //             playerAnimationEvent.OnResetStage -= ResetStage;
//         //             playerAnimationEvent.OnIdle -= OnIdle;
//         //             playerAnimationEvent.OnShooting -= ShootingAction;

//         //             playerUIActionHandler.OnUILeftItem -= LeftItem;
//         //             playerUIActionHandler.OnUIRightItem -= RightItem;
//         //         }

//         //         protected override void Awake()
//         //         {
//         //             //base.Awake();

//         //             playerUI = playerObjectRef.GetComponent<PlayerUIManager>();
//         //             islocal = playerObjectRef.GetComponent<NetworkIdentity>().isLocalPlayer;
//         //             hasauthor = playerObjectRef.GetComponent<NetworkIdentity>().hasAuthority;
//         //             isserver = playerObjectRef.GetComponent<NetworkIdentity>().isServer;
//         //             isclient = playerObjectRef.GetComponent<NetworkIdentity>().isClient;

//         //             playerArcher = this;

//         //             _uiManager = playerObjectRef.GetComponentInChildren<PlayerSkillUI>();
//         //             // skill_1_Cooldown = _uiManager.GetSkillCooldown(0);
//         //             // skill_2_Cooldown = _uiManager.GetSkillCooldown(1);
//         //             // skill_3_Cooldown = _uiManager.GetSkillCooldown(2);
//         //             // skill_4_Cooldown = _uiManager.GetSkillCooldown(3);
//         //             // skill_5_Cooldown = 30;
//         //             // leftItem_Cooldown = _uiManager.GetCooldownItem(0);
//         //             // rightItem_Cooldown = _uiManager.GetCooldownItem(1);

//         //             // skill_1_Cooldown = //PLAYER_SKILL_CD_DB//
//         //             // skill_2_Cooldown = //PLAYER_SKILL_CD_DB//
//         //             // skill_3_Cooldown = //PLAYER_SKILL_CD_DB//
//         //             // skill_4_Cooldown = //PLAYER_SKILL_CD_DB//

//         //             // ultimate_Cooldown = 60;
//         //             // skill_1_Ready = true;
//         //             // skill_2_Ready = true;
//         //             // skill_3_Ready = true;
//         //             // skill_4_Ready = true;
//         //             // skill_5_Ready = true;
//         //             // ultimate_Ready = true;
//         //             // leftItem_Ready = true;
//         //             // rightItem_Ready = true;
//         //         }

//         //         protected override void Start()
//         //         {
//         //             //base.Start();

//         //             //Set paticles color
//         //             playerOutlanderMovement = playerObjectRef.GetComponent<PlayerOutlanderMovement>();

//         //             ps = playerOutlanderMovement.TrailFX;

//         //             attackFXcolor.SetKeys(
//         //                 new GradientColorKey[]
//         //                 {
//         //                     new GradientColorKey(new Color(1, 1, 1), 1.0f),
//         //                     new GradientColorKey(Color.white, 0.0f)
//         //                 },
//         //                 new GradientAlphaKey[]
//         //                 {
//         //                     new GradientAlphaKey(0.1f, 0.0f),
//         //                     new GradientAlphaKey(0.0f, 1.0f)
//         //                 });
//         //             skillFXcolor.SetKeys(
//         //                 new GradientColorKey[]
//         //                 {
//         //                     new GradientColorKey(Color.yellow, 1.0f),
//         //                     new GradientColorKey(Color.red, 0.0f)
//         //                 },
//         //                 new GradientAlphaKey[]
//         //                 {
//         //                     new GradientAlphaKey(0.4f, 0.0f),
//         //                     new GradientAlphaKey(0.0f, 1.0f)
//         //                 });
//         //         }

//         //         protected override void Update()
//         //         {
//         //             if (!islocal) return;
//         //             //base.Update();
//         //             if (playerObjectRef.GetComponent<IPlayer>().Health <= 0) return;
//         //             SkillCooldownReset();
//         //             InputHandler();

//         //         }

//         //         // protected override void FixedUpdate()
//         //         // {
//         //         // base.FixedUpdate();
//         //         // }
//         //         #endregion

//         //         #region InputHandler
//         //         protected override void InputHandler()
//         //         {
//         //             //Debug.Log($"InputHandler die:{die} onFire:{onFire} onPickUp:{onPickUp}");
//         //             if (die) return;
//         //             if (!playerOutlanderMovement.ENABLE_INPUT_SYSTEM) return;
//         //             if (Cursor.visible) return;

//         //             if (playerOutlanderMovement.climbing) return;
//         //             if (playerOutlanderMovement.swimming) return;
//         //             if (playerOutlanderMovement.drowned) return;

//         //             if (onFire)
//         //             {
//         //                 if (playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina <= 20.0f) return;
//         //                 Attack();
//         //             }

//         //             ps.gameObject.SetActive(attacking || playerOutlanderMovement.skilling);

//         //             if (onPickUp)
//         //             {
//         //                 PickingUp();
//         //             }

//         //             if (!onSkill) return;
//         //             if (!playerOutlanderMovement.grounded) return;
//         //             // if (dodging) return;
//         //             // if (jumping) return;
//         //             // if (skilling) return;
//         //             // if (hitting) return;
//         //             // if (anim.runtimeanimatorController == weaponanimator[0] as RuntimeanimatorController) return;
//         //             switch (skillIndex)
//         //             {
//         //                 case 1:
//         //                     if (skill_1_Ready)
//         //                         Skill(skillIndex);
//         //                     break;
//         //                 case 2:
//         //                     if (skill_2_Ready)
//         //                         Skill(skillIndex);
//         //                     break;
//         //                 case 3:
//         //                     if (skill_3_Ready)
//         //                         Skill(skillIndex);
//         //                     break;
//         //                 case 4:
//         //                     if (skill_4_Ready)
//         //                         Skill(skillIndex);
//         //                     break;
//         //                 case 5:
//         //                     if (skill_5_Ready)
//         //                         Skill(skillIndex);
//         //                     break;
//         //             }
//         //         }
//         //         #endregion

//         //         #region Anim
//         //         public override void OnIdle()
//         //         {
//         //             anim.Play("Idle");
//         //         }
//         //         #endregion

//         //         #region Combat
//         //         public override void Attack()
//         //         {
//         //             onFire = false;
//         //             // base.Attack();
//         //             atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage;
//         //             atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
//         //             currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);

//         //             var col = ps.colorOverLifetime;
//         //             col.color = attackFXcolor;
//         //             // Debug.Log($"comboStep:{comboStep}");

//         //             attacking = true;
//         //             anim.CrossFade("normal_atk_1", 0.1f);


//         //             // if (comboStep == 0)
//         //             // {
//         //             //     attacking = true;
//         //             //     anim.CrossFade("normal_atk_1", 0.1f);
//         //             //     ShootingArrow(currentAtkDmg, 3);
//         //             //     comboStep = 1;
//         //             //     return;
//         //             // }
//         //             // if (comboStep != 0)
//         //             // {
//         //             //     // Debug.Log($"comboPossible:{comboPossible}");
//         //             //     if (comboPossible)
//         //             //     {
//         //             //         comboPossible = false;
//         //             //         comboStep += 1;
//         //             //     }
//         //             // }
//         //         }

//         //         public void ShootingAction()
//         //         {
//         //             if (!playerOutlanderMovement.skilling)
//         //                 playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina = 20.0f;
//         //         }

//         //         public override void ComboPossible()
//         //         {
//         //             //base.ComboPossible();
//         //             comboPossible = true;
//         //             //Debug.Log("Swordman ComboPossible");
//         //         }

//         //         public override void Combo()
//         //         {
//         //             if (comboStep == 2)
//         //             {
//         //                 attacking = true;

//         //                 anim.CrossFade("normal_atk_2", 0.3f);
//         //                 ShootingArrow(currentAtkDmg, 30);
//         //             }
//         //         }

//         //         protected override void Skill(int index)
//         //         {
//         //             switch (index)
//         //             {
//         //                 case 1:
//         //                     if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
//         //                     // case "01_charge_slash":
//         //                     skilling = true;
//         //                     // skill_1_Ready = false;
//         //                     // Debug.Log($"Use Skill : {index}");
//         //                     playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
//         //                     anim.CrossFade("01_multiple", 0.1f);
//         //                     ShootingArrow(currentAtkDmg, 30);
//         //                     break;
//         //                 case 2:
//         //                     if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
//         //                     // case "02_dash_slash":
//         //                     skilling = true;
//         //                     // skill_2_Ready = false;
//         //                     // Debug.Log($"Use Skill : {index}");
//         //                     playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
//         //                     anim.CrossFade("02_charge", 0.1f);
//         //                     ShootingArrow(currentAtkDmg, 30);
//         //                     // playerRigidbody.velocity = Quaternion.AngleAxis(0, transform.up) * transform.forward * 8;
//         //                     break;
//         //                 case 3:
//         //                     if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
//         //                     // case "03_block":
//         //                     skilling = true;
//         //                     // skill_3_Ready = false;
//         //                     // Debug.Log($"Use Skill : {index}");
//         //                     playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
//         //                     anim.CrossFade("03_jump_shot", 0.1f);
//         //                     ShootingArrow(currentAtkDmg, 30);
//         //                     break;
//         //                 case 4:
//         //                     if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
//         //                     // case "04_bump":
//         //                     skilling = true;
//         //                     // skill_4_Ready = false;
//         //                     // Debug.Log($"Use Skill : {index}");
//         //                     playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
//         //                     anim.CrossFade("04_shooting_star", 0.1f);
//         //                     ShootingArrow(currentAtkDmg, 30);
//         //                     break;
//         //                 case 5:
//         //                     if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
//         //                     // case "05_get_attention":
//         //                     skilling = true;
//         //                     // skill_5_Ready = false;
//         //                     // Debug.Log($"Use Skill : {index}");
//         //                     playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
//         //                     anim.speed *= 1.2f;
//         //                     anim.CrossFade("05_aim_lock", 0.1f);
//         //                     ShootingArrow(currentAtkDmg, 30);
//         //                     break;
//         //             }
//         //             playerObjectRef.GetComponent<PlayerOutlanderMovement>().skilling = true;
//         //             var col = ps.colorOverLifetime;
//         //             col.color = skillFXcolor;
//         //         }

//         //         protected override void Skill_1()
//         //         {
//         //             base.Skill_1();
//         //             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 5) return;
//         //             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;


//         //             skillIndex = 1;
//         //             atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage * 3;
//         //             currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
//         //             _uiManager.ActiveSkill(0);
//         //             // skill_1_Ready = false;
//         //             // StartCoroutine(SkillCooldown(skill_1_Cooldown, () => skill_1_Ready = true));
//         //             //Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         //         }

//         //         protected override void Skill_2()
//         //         {
//         //             base.Skill_2();
//         //             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 10) return;
//         //             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//         //             skillIndex = 2;
//         //             atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage * 2;
//         //             currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
//         //             _uiManager.ActiveSkill(1);
//         //             // skill_2_Ready = false;
//         //             // StartCoroutine(SkillCooldown(skill_2_Cooldown, () => skill_2_Ready = true));
//         //             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         //         }

//         //         protected override void Skill_3()
//         //         {
//         //             base.Skill_3();
//         //             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 15) return;
//         //             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//         //             skillIndex = 3;
//         //             playerObjectRef.GetComponent<IPlayer>().CurAttackDamage = 10;
//         //             _uiManager.ActiveSkill(2);
//         //             // skill_3_Ready = false;
//         //             // StartCoroutine(SkillCooldown(skill_3_Cooldown, () => skill_3_Ready = true));
//         //             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         //         }

//         //         protected override void Skill_4()
//         //         {
//         //             base.Skill_4();
//         //             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 20) return;
//         //             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//         //             skillIndex = 4;
//         //             playerObjectRef.GetComponent<IPlayer>().CurAttackDamage = 25;
//         //             _uiManager.ActiveSkill(3);
//         //             // skill_4_Ready = false;
//         //             // StartCoroutine(SkillCooldown(skill_4_Cooldown, () => skill_4_Ready = true));
//         //             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         //         }

//         //         protected override void Skill_5()
//         //         {
//         //             Debug.Log($"Gain attack speed buff");
//         //             base.Skill_5();
//         //             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 20) return;
//         //             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//         //             skillIndex = 5;

//         //             // skill_5_Ready = false;
//         //             // StartCoroutine(BuffDuration(10, SkillCooldown(skill_5_Cooldown, () => skill_5_Ready = true)));
//         //         }
//         //         /*
//         //         protected override void LeftPotion()
//         //         {
//         //             if (!islocal) return;
//         //             if (!onLeftItem) return;
//         //             if (!leftItem_Ready) return;

//         //             // Debug.Log("LeftPotion");
//         //             InventoryManager inventoryManager = GetComponentInParent<InventoryManager>();
//         //             if (inventoryManager.IsEnoughHealthPotion())
//         //             {
//         //                 leftItem_Ready = false;
//         //                 playerObjectRef.GetComponent<IPlayer>().IncreasePlayerHealth(50);
//         //                 _uiManager.UsePotion(0);
//         //                 inventoryManager.UseHealthPotion();
//         //             }
//         //             // StartCoroutine(SkillCooldown(leftItem_Cooldown, () => leftItem_Ready = true));
//         //         }

//         //         protected override void RightPotion()
//         //         {

//         //             if (!islocal) return;
//         //             if (!onRightItem) return;
//         //             if (!rightItem_Ready) return;

//         //             // Debug.Log("RightPotion");
//         //             InventoryManager inventoryManager = GetComponentInParent<InventoryManager>();
//         //             if (inventoryManager.IsEnoughManaPotion())
//         //             {
//         //                 rightItem_Ready = false;
//         //                 playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(50);
//         //                 _uiManager.UsePotion(1);
//         //                 inventoryManager.UseManaPotion();
//         //             }

//         //             // StartCoroutine(SkillCooldown(rightItem_Cooldown, () => rightItem_Ready = true));
//         //         }
//         //         */
//         //         // protected IEnumerator SkillCooldown(float _cooldown, Callback _callback)
//         //         // {
//         //         //     yield return new WaitForSeconds(_cooldown);
//         //         //     if (_callback != null) _callback();
//         //         //     // _ready = true;
//         //         //     // Debug.Log($"skill : {skill_1_Ready} {_ready}");
//         //         // }

//         //         // protected IEnumerator BuffDuration(float _timer, IEnumerator _ienum)
//         //         // {
//         //         //     // Debug.Log($"Gain attack speed buff");
//         //         //     // anim.speed *= 1.2f;
//         //         //     yield return new WaitForSeconds(_timer);
//         //         //     // Debug.Log($"Attack speed return to default");
//         //         //     anim.speed = 1f;
//         //         // }

//         //         // protected override void UltimateSkill()
//         //         // {
//         //         //     base.UltimateSkill();
//         //         // }

//         //         // public override void AttackHit()
//         //         // {
//         //         //     if (!playerOutlanderMovement.skilling)
//         //         //     {
//         //         //         playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina = 20.0f;
//         //         //         playerObjectRef.GetComponent<PlayerOutlander>().ShootingArrow(currentAtkDmg, 30);
//         //         //     }
//         //         //     base.AttackHit();
//         //         // }


//         //         // private void SkillCooldownReset()
//         //         // {
//         //         //     skill_1_Ready = _uiManager.GetBoolSkillCooldown(0);
//         //         //     skill_2_Ready = _uiManager.GetBoolSkillCooldown(1);
//         //         //     skill_3_Ready = _uiManager.GetBoolSkillCooldown(2);
//         //         //     skill_4_Ready = _uiManager.GetBoolSkillCooldown(3);

//         //         //     leftItem_Ready = _uiManager.GetBoolItemCooldown(0);
//         //         //     rightItem_Ready = _uiManager.GetBoolItemCooldown(1);

//         //         //     // if (!skill_1_Ready) StartCoroutine(SkillCooldown(skill_1_Cooldown, () => skill_1_Ready = true));
//         //         //     // if (!skill_2_Ready) StartCoroutine(SkillCooldown(skill_2_Cooldown, () => skill_2_Ready = true));
//         //         //     // if (!skill_3_Ready) StartCoroutine(SkillCooldown(skill_3_Cooldown, () => skill_3_Ready = true));
//         //         //     // if (!skill_4_Ready) StartCoroutine(SkillCooldown(skill_4_Cooldown, () => skill_4_Ready = true));
//         //         //     // if (!leftItem_Ready) StartCoroutine(SkillCooldown(leftItem_Cooldown, () => leftItem_Ready = true));
//         //         //     // if (!rightItem_Ready) StartCoroutine(SkillCooldown(rightItem_Cooldown, () => rightItem_Ready = true));
//         //         // }
//         //         // #endregion
//     }
// }

