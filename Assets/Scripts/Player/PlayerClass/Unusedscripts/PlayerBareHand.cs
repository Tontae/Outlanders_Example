// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Mirror;
// using Outlander.Manager;
// using Outlander.UI;
// using Outlander.Player;

// namespace Outlander.Player
// {
//     public class PlayerBareHand : PlayerOutlander
//     {
//         // public static PlayerBareHand playerBareHand;
//         // protected delegate void Callback();

//         // //[Header("Animator")]// change animator //
//         // //[SerializeField] protected Animator anim;
//         // //[SerializeField] protected RuntimeAnimatorController runtimeAnimator;

//         // #region Main
//         // public override void OnStartAuthority()
//         // {
//         //     base.OnStartAuthority();

//         //     PlayerInput playerInput = playerObjectRef.GetComponent<PlayerInput>();
//         //     playerInput.enabled = true;
//         // }

//         // protected override void OnEnable()
//         // {
//         //     // base.OnEnable();

//         //     // playerAnimationEvent = new PlayerAnimationEvent();
//         //     playerAnimationEvent = playerObjectRef.GetComponent<PlayerAnimationEvent>();
//         //     playerUIActionHandler = playerObjectRef.GetComponent<Outlander.UI.PlayerUIActionHandler>();

//         //     playerAnimationEvent.OnAttackHit += AttackHit;
//         //     playerAnimationEvent.OnCombo += Combo;
//         //     playerAnimationEvent.OnComboPossible += ComboPossible;
//         //     playerAnimationEvent.OnComboReset += ComboReset;
//         //     playerAnimationEvent.OnEndSkill += EndSkill;
//         //     playerAnimationEvent.OnResetStage += ResetStage;
//         //     playerAnimationEvent.OnIdle += OnIdle;

//         //     playerUIActionHandler.OnUILeftItem += LeftPotion;
//         //     playerUIActionHandler.OnUIRightItem += RightPotion;

//         //     anim = playerObjectRef.GetComponent<Animator>();
//         //     anim.runtimeAnimatorController = runtimeAnimator;
//         //     playerObjectRef.GetComponent<IPlayer>().RuntimeAnimator = runtimeAnimator;

//         //     // Debug.Log(runtimeAnimator.name);
//         // }

//         // protected override void OnDisable()
//         // {
//         //     //base.OnDisable();

//         //     playerAnimationEvent.OnAttackHit -= AttackHit;
//         //     playerAnimationEvent.OnCombo -= Combo;
//         //     playerAnimationEvent.OnComboPossible -= ComboPossible;
//         //     playerAnimationEvent.OnComboReset -= ComboReset;
//         //     playerAnimationEvent.OnEndSkill -= EndSkill;
//         //     playerAnimationEvent.OnResetStage -= ResetStage;
//         //     playerAnimationEvent.OnIdle -= OnIdle;

//         //     playerUIActionHandler.OnUILeftItem -= LeftPotion;
//         //     playerUIActionHandler.OnUIRightItem -= RightPotion;
//         // }

//         // protected override void Awake()
//         // {
//         //     //base.Awake();
//         //     playerUI = playerObjectRef.GetComponent<PlayerUIManager>();
//         //     islocal = playerObjectRef.GetComponent<NetworkIdentity>().isLocalPlayer;
//         //     hasauthor = playerObjectRef.GetComponent<NetworkIdentity>().hasAuthority;
//         //     isserver = playerObjectRef.GetComponent<NetworkIdentity>().isServer;
//         //     isclient = playerObjectRef.GetComponent<NetworkIdentity>().isClient;

//         //     playerBareHand = this;

//         //     if (!islocal) return;
//         //     _uiManager = playerObjectRef.GetComponentInChildren<PlayerSkillUI>();
//         //     // skill_1_Cooldown = _uiManager.GetSkillCooldown(0);
//         //     // skill_2_Cooldown = _uiManager.GetSkillCooldown(1);
//         //     // skill_3_Cooldown = _uiManager.GetSkillCooldown(2);
//         //     // skill_4_Cooldown = _uiManager.GetSkillCooldown(3);
//         //     // skill_5_Cooldown = 30;
//         //     // leftItem_Cooldown = _uiManager.GetCooldownItem(0);
//         //     // rightItem_Cooldown = _uiManager.GetCooldownItem(1);

//         //     // ultimate_Cooldown = 60;
//         //     // skill_1_Ready = true;
//         //     // skill_2_Ready = true;
//         //     // skill_3_Ready = true;
//         //     // skill_4_Ready = true;
//         //     // skill_5_Ready = true;
//         //     // ultimate_Ready = true;
//         //     // leftItem_Ready = true;
//         //     // rightItem_Ready = true;
//         // }

//         // protected override void Start()
//         // {
//         //     playerOutlanderMovement = playerObjectRef.GetComponent<PlayerOutlanderMovement>();
//         // }

//         // protected override void Update()
//         // {
//         //     if (!islocal) return;
//         //     //base.Update();
//         //     if (playerObjectRef.GetComponent<IPlayer>().Health <= 0) return;
//         //     SkillCooldownReset();
//         //     InputHandler();
//         // }
//         // #endregion

//         // #region InputHandler
//         // protected override void InputHandler()
//         // {
//         //     //Debug.Log($"InputHandler die:{die} onFire:{onFire} onPickUp:{onPickUp}");
//         //     if (die) return;
//         //     if (!playerOutlanderMovement.ENABLE_INPUT_SYSTEM) return;
//         //     if (Cursor.visible) return;

//         //     if (playerOutlanderMovement.climbing) return;
//         //     if (playerOutlanderMovement.swimming) return;
//         //     if (playerOutlanderMovement.drowned) return;

//         //     if (onFire)
//         //     {
//         //         if (playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina <= 10.0f) return;

//         //         Attack();
//         //     }

//         //     if (onPickUp)
//         //     {
//         //         PickingUp();
//         //     }

//         //     if (!onSkill) return;
//         //     if (!playerOutlanderMovement.grounded) return;
//         // }
//         // #endregion

//         // #region Anim
//         // public override void OnIdle()
//         // {
//         //     int Pose = Random.Range(0, 3);
//         //     switch (Pose)
//         //     {
//         //         case 0:
//         //             anim.Play("idle_nonweapon2");
//         //             Debug.Log("Anim Play Idle");
//         //             break;
//         //         case 1:
//         //             anim.Play("idle_nonweapon3");
//         //             Debug.Log("Anim Play Idle");
//         //             break;
//         //         case 2:
//         //             anim.Play("idle_nonweapon4");
//         //             Debug.Log("Anim Play Idle");
//         //             break;
//         //     }
//         // }

//         // public override void ComboReset()
//         // {
//         //     base.ComboReset();
//         // }
//         // #endregion

//         // #region Combat
//         // public override void Attack()
//         // {
//         //     onFire = false;
//         //     atkDamage = playerObjectRef.GetComponent<IPlayer>().AttackDamage;
//         //     atkDamage = (Random.Range(0.0f, 100.0f) <= playerObjectRef.GetComponent<IPlayer>().Crit) ? atkDamage * 2.0f : atkDamage;
//         //     currentAtkDmg = atkDamage - Random.Range(0, atkDamage * 0.2f);
//         //     if (comboStep == 0)
//         //     {
//         //         attacking = true;
//         //         anim.CrossFade("normal_atk_1", 0.1f);
//         //         comboStep = 1;
//         //         return;
//         //     }
//         //     if (comboStep != 0)
//         //     {
//         //         if (comboPossible)
//         //         {
//         //             comboPossible = false;
//         //             comboStep += 1;
//         //         }
//         //     }
//         // }

//         // public override void AttackHit()
//         // {
//         //     playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina = 10.0f;
//         //     base.AttackHit();
//         // }

//         // public override void AttackHit()
//         // {
//         //     playerObjectRef.GetComponent<PlayerOutlanderMovement>().DecreaseStamina = 20.0f;
//         //     base.AttackHit();
//         // }

//         // public override void ComboPossible()
//         // {
//         //     comboPossible = true;
//         // }

//         // public override void Combo()
//         // {
//         //     if (comboStep == 2)
//         //     {
//         //         attacking = true;

//         //         anim.CrossFade("normal_atk_2", 0.1f);
//         //     }
//         // }

//         // protected override void LeftPotion()
//         // {
//         //     if (!islocal) return;
//         //     if (!onLeftItem) return;
//         //     if (!leftItem_Ready) return;

//         //     // Debug.Log("LeftPotion");
//         //     InventoryManager inventoryManager = GetComponentInParent<InventoryManager>();
//         //     if (inventoryManager.IsEnoughHealthPotion())
//         //     {
//         //         leftItem_Ready = false;
//         //         playerObjectRef.GetComponent<IPlayer>().IncreasePlayerHealth(50);
//         //         _uiManager.UsePotion(0);
//         //         inventoryManager.UseHealthPotion();
//         //     }
//         //     // StartCoroutine(SkillCooldown(leftItem_Cooldown, () => leftItem_Ready = true));
//         // }

//         // protected override void RightPotion()
//         // {
//         //     if (!islocal) return;
//         //     if (!onRightItem) return;
//         //     if (!rightItem_Ready) return;

//         //     // Debug.Log("RightPotion");
//         //     InventoryManager inventoryManager = GetComponentInParent<InventoryManager>();
//         //     if (inventoryManager.IsEnoughManaPotion())
//         //     {
//         //         rightItem_Ready = false;
//         //         playerObjectRef.GetComponent<IPlayer>().IncreasePlayerMana(50);
//         //         _uiManager.UsePotion(1);
//         //         inventoryManager.UseManaPotion();
//         //     }
//         //     // StartCoroutine(SkillCooldown(rightItem_Cooldown, () => rightItem_Ready = true));
//         // }

//         // // protected IEnumerator SkillCooldown(float _cooldown, Callback _callback)
//         // // {
//         // //     yield return new WaitForSeconds(_cooldown);
//         // //     if (_callback != null) _callback();
//         // //     // _ready = true;
//         // //     // Debug.Log($"skill : {skill_1_Ready} {_ready}");
//         // // }


//         // private void SkillCooldownReset()
//         // {
//         //     skill_1_Ready = _uiManager.GetBoolSkillCooldown(0);
//         //     skill_2_Ready = _uiManager.GetBoolSkillCooldown(1);
//         //     skill_3_Ready = _uiManager.GetBoolSkillCooldown(2);
//         //     skill_4_Ready = _uiManager.GetBoolSkillCooldown(3);

//         //     leftItem_Ready = _uiManager.GetBoolItemCooldown(0);
//         //     rightItem_Ready = _uiManager.GetBoolItemCooldown(1);

//         //     // if (!skill_1_Ready) StartCoroutine(SkillCooldown(skill_1_Cooldown, () => skill_1_Ready = true));
//         //     // if (!skill_2_Ready) StartCoroutine(SkillCooldown(skill_2_Cooldown, () => skill_2_Ready = true));
//         //     // if (!skill_3_Ready) StartCoroutine(SkillCooldown(skill_3_Cooldown, () => skill_3_Ready = true));
//         //     // if (!skill_4_Ready) StartCoroutine(SkillCooldown(skill_4_Cooldown, () => skill_4_Ready = true));
//         //     // if (!leftItem_Ready) StartCoroutine(SkillCooldown(leftItem_Cooldown, () => leftItem_Ready = true));
//         //     // if (!rightItem_Ready) StartCoroutine(SkillCooldown(rightItem_Cooldown, () => rightItem_Ready = true));
//         // }
//         // #endregion
//     }
// }


