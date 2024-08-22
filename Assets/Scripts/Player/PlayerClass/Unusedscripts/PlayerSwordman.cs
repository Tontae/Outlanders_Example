// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;
// // using UnityEngine.InputSystem;
// // using Mirror;

// // namespace Outlander.Player
// // {
// //     public class PlayerSwordman : PlayerOutlander
// //     {
// //         // public static PlayerSwordman playerSwordman;
// //         // protected delegate void Callback();
// //         // private ParticleSystem ps;
// //         // private Gradient attackFXcolor = new Gradient();
// //         // private Gradient skillFXcolor = new Gradient();

// //         #region Main
// //         // public override void OnStartAuthority()
// //         // {
// //         //     base.OnStartAuthority();

// //         //     PlayerInput playerInput = playerObjectRef.GetComponent<PlayerInput>();
// //         //     playerInput.enabled = true;
// //         // }

// //         protected override void OnEnable()
// //         {
// //             //base.OnEnable();

// //             // playerAnimationEvent = new PlayerAnimationEvent();
// //             // playerAnimationEvent = playerObjectRef.GetComponent<PlayerAnimationEvent>();
// //             // playerUIActionHandler = playerObjectRef.GetComponent<Outlander.UI.PlayerUIActionHandler>();

// //             // playerAnimationEvent.OnAttackHit += AttackHit;
// //             // playerAnimationEvent.OnCombo += Combo;
// //             // playerAnimationEvent.OnComboPossible += ComboPossible;
// //             // playerAnimationEvent.OnComboReset += ComboReset;
// //             // playerAnimationEvent.OnEndSkill += EndSkill;
// //             // playerAnimationEvent.OnResetStage += ResetStage;
// //             // playerAnimationEvent.OnIdle += OnIdle;

// //             // playerUIActionHandler.OnUILeftItem += LeftPotion;
// //             // playerUIActionHandler.OnUIRightItem += RightPotion;

// //             // anim = playerObjectRef.GetComponent<Animator>();
// //             // anim.runtimeAnimatorController = runtimeAnimator;
// //             // playerObjectRef.GetComponent<IPlayer>().RuntimeAnimator = runtimeAnimator;

// //             // Debug.Log(runtimeAnimator.name);
// //             //playerObjectRef.GetComponent<IPlayer>().AttackDamage = atkDamage;
// //             //Debug.Log("Swordman Enable");
// //         }

// //         protected override void OnDisable()
// //         {
// //             //base.OnDisable();

// //             // playerAnimationEvent.OnAttackHit -= AttackHit;
// //             // playerAnimationEvent.OnCombo -= Combo;
// //             // playerAnimationEvent.OnComboPossible -= ComboPossible;
// //             // playerAnimationEvent.OnComboReset -= ComboReset;
// //             // playerAnimationEvent.OnEndSkill -= EndSkill;
// //             // playerAnimationEvent.OnResetStage -= ResetStage;
// //             // playerAnimationEvent.OnIdle -= OnIdle;

// //             // playerUIActionHandler.OnUILeftItem -= LeftPotion;
// //             // playerUIActionHandler.OnUIRightItem -= RightPotion;
// //             //Debug.Log("Swordman Disable");
// //         }

// //         protected override void Awake()
// //         {
// //             // playerUI = playerObjectRef.GetComponent<PlayerUIManager>();
// //             // islocal = playerObjectRef.GetComponent<NetworkIdentity>().isLocalPlayer;
// //             // hasauthor = playerObjectRef.GetComponent<NetworkIdentity>().hasAuthority;
// //             // isserver = playerObjectRef.GetComponent<NetworkIdentity>().isServer;
// //             // isclient = playerObjectRef.GetComponent<NetworkIdentity>().isClient;

// //             // playerSwordman = this;

// //             // if (!islocal) return;
// //             // _uiManager = playerObjectRef.GetComponentInChildren<PlayerSkillUI>();
// //         }

// //         protected override void Start()
// //         {
// //             //Set paticles color
// //             // playerOutlanderMovement = playerObjectRef.GetComponent<PlayerOutlanderMovement>();

// //             // ps = playerOutlanderMovement.TrailFX;

// //             // attackFXcolor.SetKeys(
// //             //     new GradientColorKey[]
// //             //     {
// //             //         new GradientColorKey(new Color(1, 1, 1), 1.0f),
// //             //         new GradientColorKey(Color.white, 0.0f)
// //             //     },
// //             //     new GradientAlphaKey[]
// //             //     {
// //             //         new GradientAlphaKey(0.1f, 0.0f),
// //             //         new GradientAlphaKey(0.0f, 1.0f)
// //             //     });
// //             // skillFXcolor.SetKeys(
// //             //     new GradientColorKey[]
// //             //     {
// //             //         new GradientColorKey(Color.yellow, 1.0f),
// //             //         new GradientColorKey(Color.red, 0.0f)
// //             //     },
// //             //     new GradientAlphaKey[]
// //             //     {
// //             //         new GradientAlphaKey(0.4f, 0.0f),
// //             //         new GradientAlphaKey(0.0f, 1.0f)
// //             //     });
// //         }

// //         protected override void Update()
// //         {
// //             // if (!islocal) return;

// //             // if (playerObjectRef.GetComponent<IPlayer>().Health <= 0) return;
// //             // SkillCooldownReset();
// //             // InputHandler();
// //         }
// //         #endregion

// //         #region InputHandler
// //         // protected override void InputHandler()
// //         // {
// //         //     if (die) return;
// //         //     if (!playerOutlanderMovement.ENABLE_INPUT_SYSTEM) return;
// //         //     if (Cursor.visible) return;

// //         //     if (playerOutlanderMovement.climbing) return;
// //         //     if (playerOutlanderMovement.swimming) return;
// //         //     if (playerOutlanderMovement.drowned) return;

// //         //     if (onFire)
// //         //     {
// //         //         if (playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina <= 20.0f) return;
// //         //         Attack();
// //         //     }

// //         //     ps.gameObject.SetActive(attacking || playerOutlanderMovement.skilling);

// //         //     if (!onSkill) return;
// //         //     if (!playerOutlanderMovement.grounded) return;

// //         //     switch (skillIndex)
// //         //     {
// //         //         case 1:
// //         //             if (isSkillReady[skillIndex])
// //         //                 Skill(skillIndex);
// //         //             break;
// //         //         case 2:
// //         //             if (isSkillReady[skillIndex])
// //         //                 Skill(skillIndex);
// //         //             break;
// //         //         case 3:
// //         //             if (isSkillReady[skillIndex])
// //         //                 Skill(skillIndex);
// //         //             break;
// //         //         case 4:
// //         //             if (isSkillReady[skillIndex])
// //         //                 Skill(skillIndex);
// //         //             break;
// //         //         case 5:
// //         //             if (isSkillReady[skillIndex])
// //         //                 Skill(skillIndex);
// //         //             break;
// //         //     }
// //         // }
// //         #endregion

// //         #region Anim
// //         // public override void OnIdle()
// //         // {
// //         //     anim.Play("Idle");
// //         // }

// //         // public override void ComboReset()
// //         // {
// //         //     base.ComboReset();
// //         // }
// //         #endregion

// //         #region Combat
// //         // public override void Attack()
// //         // {
// //         //     onFire = false;

// //         //     atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage;
// //         //     atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
// //         //     currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
// //         //     var col = ps.colorOverLifetime;
// //         //     col.color = attackFXcolor;

// //         //     if (comboStep == 0)
// //         //     {
// //         //         attacking = true;
// //         //         anim.CrossFade("normal_atk_1", 0.1f);
// //         //         comboStep = 1;
// //         //         return;
// //         //     }
// //         //     if (comboStep != 0)
// //         //     {
// //         //         if (comboPossible)
// //         //         {
// //         //             comboPossible = false;
// //         //             comboStep += 1;
// //         //         }
// //         //     }
// //         // }

// //         // public override void ComboPossible()
// //         // {
// //         //     comboPossible = true;
// //         // }

// //         // public override void Combo()
// //         // {
// //         //     if (comboStep == 2)
// //         //     {
// //         //         attacking = true;

// //         //         anim.CrossFade("normal_atk_2", 0.3f);
// //         //     }
// //         // }

// //         // protected void Skill(int index)
// //         // {
// //         //     switch (index)
// //         //     {
// //         //         case 1:
// //         //             if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
// //         //             skilling = true;

// //         //             playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
// //         //             anim.CrossFade("01_charge_slash", 0.1f);
// //         //             break;
// //         //         case 2:
// //         //             if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
// //         //             skilling = true;

//             // if (onFire)
//             // {
//             //     if (playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina <= 10.0f) return;
// //         //             playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
// //         //             anim.CrossFade("02_dash_slash", 0.1f);
// //         //             // playerRigidbody.velocity = Quaternion.AngleAxis(0, transform.up) * transform.forward * 8;
// //         //             break;
// //         //         case 3:
// //         //             if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
// //         //             skilling = true;

// //         //             playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
// //         //             anim.CrossFade("03_block", 0.1f);
// //         //             break;
// //         //         case 4:
// //         //             if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
// //         //             skilling = true;

// //         //             playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
// //         //             anim.CrossFade("04_bump", 0.1f);
// //         //             break;
// //         //         case 5:
// //         //             if (playerObjectRef.GetComponent<IPlayer>().Mana < 20.0f) return;
// //         //             skilling = true;

// //         //             playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(-20.0f);
// //         //             anim.speed *= 1.2f;
// //         //             anim.CrossFade("05_get_attention", 0.1f);
// //         //             break;
// //         //     }
// //         //     playerObjectRef.GetComponent<PlayerOutlanderMovement>().skilling = true;
// //         //     var col = ps.colorOverLifetime;
// //         //     col.color = skillFXcolor;
// //         // }

// //         // protected override void Skill_1()
// //         // {
// //         //     base.Skill_1();
// //         // if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 5) return;

// //         //     skillIndex = 1;
// //         //     atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage * 3;
// //         //     atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
// //         //     currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
// //         //     _uiManager.ActiveSkill(0);
// //         // }

// //         // protected override void Skill_2()
// //         // {
// //         //     base.Skill_2();
// //         //     if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 10) return;

// //         //     skillIndex = 2;
// //         //     atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage * 2;
// //         //     atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
// //         //     currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
// //         //     _uiManager.ActiveSkill(1);
// //         // }

// //         // protected override void Skill_3()
// //         // {
// //         //     base.Skill_3();
// //         //     if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 15) return;

// //         //     skillIndex = 3;
// //         //     playerObjectRef.GetComponent<IPlayer>().CurAttackDamage = 10;
// //         //     _uiManager.ActiveSkill(2);
// //         // }

// //         // protected override void Skill_4()
// //         // {
// //         //     base.Skill_4();
// //         //     if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 20) return;

// //         //     skillIndex = 4;
// //         //     playerObjectRef.GetComponent<IPlayer>().CurAttackDamage = 25;
// //         //     _uiManager.ActiveSkill(3);
// //         // }

// //         // protected override void LeftPotion()
// //         // {
// //         //     if (!islocal) return;
// //         //     if (!onLeftItem) return;
// //         //     if (!isItemReady[0]) return;

// //         //     InventoryManager inventoryManager = GetComponentInParent<InventoryManager>();
// //         //     if (inventoryManager.IsEnoughHealthPotion())
// //         //     {
// //         //         isItemReady[0] = false;
// //         //         playerObjectRef.GetComponent<IPlayer>().IncreasePlayerHealth(50);
// //         //         _uiManager.UsePotion(0);
// //         //         inventoryManager.UseHealthPotion();
// //         //     }
// //         // }

// //         // protected override void RightPotion()
// //         // {
// //         //     if (!islocal) return;
// //         //     if (!onRightItem) return;
// //         //     if (!isItemReady[1]) return;

// //         //     InventoryManager inventoryManager = GetComponentInParent<InventoryManager>();
// //         //     if (inventoryManager.IsEnoughManaPotion())
// //         //     {
// //         //         isItemReady[1] = false;
// //         //         playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(50);
// //         //         _uiManager.UsePotion(1);
// //         //         inventoryManager.UseManaPotion();
// //         //     }
// //         // }

// //         // protected override void WeaponAction()
// //         // {
// //         //     if (onWeaponAction)
// //         //     {
// //         //         // anim.CrossFade("Block", 0.1f);
// //         //     }
// //         // }

// //         // public override void AttackHit()
// //         // {
// //         //     if (!playerOutlanderMovement.skilling)
// //         //         playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina = 20.0f;
// //         //     base.AttackHit();
// //         // }

// //         // private void SkillCooldownReset()
// //         // {
// //         //     for (int i = 0; i < isSkillReady.Length; i++)
// //         //     {
// //         //         isSkillReady[i] = _uiManager.GetBoolSkillCooldown(i);
// //         //     }

// //         //     for (int i = 0; i < isItemReady.Length; i++)
// //         //     {
// //         //         isItemReady[i] = _uiManager.GetBoolItemCooldown(i);
// //         //     }
// //         // }

//             skillIndex = 1;
//             atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage * 3;
//             atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
//             currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
//             _uiManager.ActiveSkill(0);
//             // skill_1_Ready = false;
//             // StartCoroutine(SkillCooldown(skill_1_Cooldown, () => skill_1_Ready = true));
//             //Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         }

//         protected override void Skill_2()
//         {
//             base.Skill_2();
//             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 10) return;
//             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//             skillIndex = 2;
//             atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage * 2;
//             atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
//             currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
//             _uiManager.ActiveSkill(1);
//             // skill_2_Ready = false;
//             // StartCoroutine(SkillCooldown(skill_2_Cooldown, () => skill_2_Ready = true));
//             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         }

//         protected override void Skill_3()
//         {
//             base.Skill_3();
//             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 15) return;
//             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//             skillIndex = 3;
//             playerObjectRef.GetComponent<IPlayer>().CurAttackDamage = 10;
//             _uiManager.ActiveSkill(2);
//             // skill_3_Ready = false;
//             // StartCoroutine(SkillCooldown(skill_3_Cooldown, () => skill_3_Ready = true));
//             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         }

//         protected override void Skill_4()
//         {
//             base.Skill_4();
//             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 20) return;
//             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//             skillIndex = 4;
//             playerObjectRef.GetComponent<IPlayer>().CurAttackDamage = 25;
//             _uiManager.ActiveSkill(3);
//             // skill_4_Ready = false;
//             // StartCoroutine(SkillCooldown(skill_4_Cooldown, () => skill_4_Ready = true));
//             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         }

//         protected override void Skill_5()
//         {
//             Debug.Log($"Gain attack speed buff");
//             base.Skill_5();
//             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 20) return;
//             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//             skillIndex = 5;

//             // skill_5_Ready = false;
//             // StartCoroutine(BuffDuration(10, SkillCooldown(skill_5_Cooldown, () => skill_5_Ready = true)));
//         }


//         protected override void WeaponAction()
//         {
//             if (onWeaponAction)
//             {
//                 // anim.CrossFade("Block", 0.1f);
//             }
//         }

//         // protected IEnumerator SkillCooldown(float _cooldown, Callback _callback)
//         // {
//         //     yield return new WaitForSeconds(_cooldown);
//         //     if (_callback != null) _callback();
//         //     // _ready = true;
//         //     // Debug.Log($"skill : {skill_1_Ready} {_ready}");
//         // }

//         // protected IEnumerator BuffDuration(float _timer, IEnumerator _ienum)
//         // {
//         //     // Debug.Log($"Gain attack speed buff");
//         //     // anim.speed *= 1.2f;
//         //     yield return new WaitForSeconds(_timer);
//         //     // Debug.Log($"Attack speed return to default");
//         //     anim.speed = 1f;
//         // }

//         protected override void UltimateSkill()
//         {
//             base.UltimateSkill();
//         }

//         public override void AttackHit()
//         {
//             if (!playerOutlanderMovement.skilling)
//                 playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina = 10.0f;
//             base.AttackHit();
//         }

//         private void SkillCooldownReset()
//         {
//             skill_1_Ready = _uiManager.GetBoolSkillCooldown(0);
//             skill_2_Ready = _uiManager.GetBoolSkillCooldown(1);
//             skill_3_Ready = _uiManager.GetBoolSkillCooldown(2);
//             skill_4_Ready = _uiManager.GetBoolSkillCooldown(3);

//             leftItem_Ready = _uiManager.GetBoolItemCooldown(0);
//             rightItem_Ready = _uiManager.GetBoolItemCooldown(1);

//             // if (!skill_1_Ready) StartCoroutine(SkillCooldown(skill_1_Cooldown, () => skill_1_Ready = true));
//             // if (!skill_2_Ready) StartCoroutine(SkillCooldown(skill_2_Cooldown, () => skill_2_Ready = true));
//             // if (!skill_3_Ready) StartCoroutine(SkillCooldown(skill_3_Cooldown, () => skill_3_Ready = true));
//             // if (!skill_4_Ready) StartCoroutine(SkillCooldown(skill_4_Cooldown, () => skill_4_Ready = true));
//             // if (!leftItem_Ready) StartCoroutine(SkillCooldown(leftItem_Cooldown, () => leftItem_Ready = true));
//             // if (!rightItem_Ready) StartCoroutine(SkillCooldown(rightItem_Cooldown, () => rightItem_Ready = true));
//         }

//         #endregion
//     }
// }
//             skillIndex = 1;
//             atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage * 3;
//             atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
//             currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
//             _uiManager.ActiveSkill(0);
//             // skill_1_Ready = false;
//             // StartCoroutine(SkillCooldown(skill_1_Cooldown, () => skill_1_Ready = true));
//             //Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         }

//         protected override void Skill_2()
//         {
//             base.Skill_2();
//             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 10) return;
//             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//             skillIndex = 2;
//             atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage * 2;
//             atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
//             currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
//             _uiManager.ActiveSkill(1);
//             // skill_2_Ready = false;
//             // StartCoroutine(SkillCooldown(skill_2_Cooldown, () => skill_2_Ready = true));
//             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         }

//         protected override void Skill_3()
//         {
//             base.Skill_3();
//             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 15) return;
//             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//             skillIndex = 3;
//             playerObjectRef.GetComponent<IPlayer>().CurAttackDamage = 10;
//             _uiManager.ActiveSkill(2);
//             // skill_3_Ready = false;
//             // StartCoroutine(SkillCooldown(skill_3_Cooldown, () => skill_3_Ready = true));
//             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         }

//         protected override void Skill_4()
//         {
//             base.Skill_4();
//             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 20) return;
//             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//             skillIndex = 4;
//             playerObjectRef.GetComponent<IPlayer>().CurAttackDamage = 25;
//             _uiManager.ActiveSkill(3);
//             // skill_4_Ready = false;
//             // StartCoroutine(SkillCooldown(skill_4_Cooldown, () => skill_4_Ready = true));
//             // Debug.Log($"Skill {skillIndex} damage : {currentAtkDmg}");
//         }

//         protected override void Skill_5()
//         {
//             Debug.Log($"Gain attack speed buff");
//             base.Skill_5();
//             if (playerObjectRef.GetComponent<ProficiencySystem>().GetLevelWeapon() < 20) return;
//             // if (playerObjectRef.GetComponent<IPlayer>().Mana < 20) return;


//             skillIndex = 5;

//             // skill_5_Ready = false;
//             // StartCoroutine(BuffDuration(10, SkillCooldown(skill_5_Cooldown, () => skill_5_Ready = true)));
//         }


//         protected override void WeaponAction()
//         {
//             if (onWeaponAction)
//             {
//                 // anim.CrossFade("Block", 0.1f);
//             }
//         }

//         // protected IEnumerator SkillCooldown(float _cooldown, Callback _callback)
//         // {
//         //     yield return new WaitForSeconds(_cooldown);
//         //     if (_callback != null) _callback();
//         //     // _ready = true;
//         //     // Debug.Log($"skill : {skill_1_Ready} {_ready}");
//         // }

//         // protected IEnumerator BuffDuration(float _timer, IEnumerator _ienum)
//         // {
//         //     // Debug.Log($"Gain attack speed buff");
//         //     // anim.speed *= 1.2f;
//         //     yield return new WaitForSeconds(_timer);
//         //     // Debug.Log($"Attack speed return to default");
//         //     anim.speed = 1f;
//         // }

//         protected override void UltimateSkill()
//         {
//             base.UltimateSkill();
//         }

//         public override void AttackHit()
//         {
//             if (!playerOutlanderMovement.skilling)
//                 playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina = 20.0f;
//             base.AttackHit();
//         }

//         private void SkillCooldownReset()
//         {
//             skill_1_Ready = _uiManager.GetBoolSkillCooldown(0);
//             skill_2_Ready = _uiManager.GetBoolSkillCooldown(1);
//             skill_3_Ready = _uiManager.GetBoolSkillCooldown(2);
//             skill_4_Ready = _uiManager.GetBoolSkillCooldown(3);

//             leftItem_Ready = _uiManager.GetBoolItemCooldown(0);
//             rightItem_Ready = _uiManager.GetBoolItemCooldown(1);

//             // if (!skill_1_Ready) StartCoroutine(SkillCooldown(skill_1_Cooldown, () => skill_1_Ready = true));
//             // if (!skill_2_Ready) StartCoroutine(SkillCooldown(skill_2_Cooldown, () => skill_2_Ready = true));
//             // if (!skill_3_Ready) StartCoroutine(SkillCooldown(skill_3_Cooldown, () => skill_3_Ready = true));
//             // if (!skill_4_Ready) StartCoroutine(SkillCooldown(skill_4_Cooldown, () => skill_4_Ready = true));
//             // if (!leftItem_Ready) StartCoroutine(SkillCooldown(leftItem_Cooldown, () => leftItem_Ready = true));
//             // if (!rightItem_Ready) StartCoroutine(SkillCooldown(rightItem_Cooldown, () => rightItem_Ready = true));
//         }

//         #endregion
//     }
// }
// //         #endregion
// //     }
// // }
