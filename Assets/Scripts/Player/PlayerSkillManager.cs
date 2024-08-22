using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Outlander.Character;


namespace Outlander.Player
{
    public class PlayerSkillManager : PlayerElements
    {
        public int skillHitCount = 0;
        public SkillScriptable curSkillUse;

        public void MultiHitSkill()
        {
            //Debug.Log($"While skilling hit:{++skillHitCount}");
            skillHitCount += 1;
            switch (curSkillUse.skillId)
            {
                case "SKLA001":
                    if (skillHitCount == 3)
                    {
                        float skillDamage = Mathf.Clamp((curSkillUse.skillDamageModifier.fixedDamage + 3f) + (Player.OutlanderStateMachine.PlayerAtkDmg * curSkillUse.skillDamageModifier.multiplierDamage), 1f, float.MaxValue);
                        CmdUseSkillDamage(skillDamage);
                        //Debug.Log($"Damge skill change to:{skillDamage}");
                    }
                    break;
                case "SKAX003":
                    if (skillHitCount == 3)
                    {
                        float skillDamage = Mathf.Clamp((curSkillUse.skillDamageModifier.fixedDamage + 5f) + (Player.OutlanderStateMachine.PlayerAtkDmg * curSkillUse.skillDamageModifier.multiplierDamage), 1f, float.MaxValue);
                        CmdUseSkillDamage(skillDamage);
                        //Debug.Log($"Damge skill change to:{skillDamage}");
                    }
                    break;
            }
        }

        public void ResetMultiHit()
        {
            skillHitCount = 0;
            curSkillUse = null;
        }

        //[SerializeField] private List<SkillScriptable> skills = new List<SkillScriptable>();
        //[SerializeField] public List<string> skillReady = new List<string>();
        //public Dictionary<int, SkillScriptable> skillsDictionary = new Dictionary<int, SkillScriptable>();

        /*public void AssignSkill(SkillScriptable skill)
        {
            switch (skill.skillType)
            {
                case SkillScriptable.SkillType.Passive:
                    PlayerStatisticManager psm = GetComponent<PlayerStatisticManager>();
                    psm.skillPlayerStatistic.Clear();
                    skill.skillStatuses.ForEach(x => psm.skillPlayerStatistic.AddStat(x.statusType, x.value));
                    CmdSetSkillStatistic(psm.skillPlayerStatistic);
                    break;

                case SkillScriptable.SkillType.Active:
                case SkillScriptable.SkillType.Support:
                    if (!skills.Contains(skill))
                    {
                        skills.Add(skill);
                    }
                    break;
            }
        }

        public void RemoveSkill(SkillScriptable skill)
        {
            switch (skill.skillType)
            {
                case SkillScriptable.SkillType.Passive:
                    PlayerStatisticManager psm = GetComponent<PlayerStatisticManager>();
                    skill.skillStatuses.ForEach(x => psm.skillPlayerStatistic.AddStat(x.statusType, -x.value));
                    CmdSetSkillStatistic(psm.skillPlayerStatistic);
                    break;

                case SkillScriptable.SkillType.Active:
                case SkillScriptable.SkillType.Support:
                    if (skills.Contains(skill))
                    {
                        skills.Remove(skill);
                    }
                    break;
            }
        }

        public void ClearSkill()
        {
            skills.Clear();
        }*/

        public void SetSkillsToWeapon()
        {
            /*if (Player.EquipmentManager.equipedDic.TryGetValue("main_weapon", out InventoryItemBehavior iib))
            {
                if (iib.EquipedSkillDictionary != null)
                {
                    _ = iib.thisItem.item_type switch
                    {
                        ItemType.Sword => transform.Find($"Armature/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/[Weapon]_Right_Hand/{iib.thisItem.itemName}").TryGetComponent(out Weapon.BaseSword wb) ? wb.skills = iib.GetEquipedSkillDictionary() : null,
                        ItemType.Bow => transform.Find($"Armature/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/[Weapon]_Right_Hand/{iib.thisItem.itemName}").TryGetComponent(out Weapon.BaseBow wb) ? wb.skills = iib.GetEquipedSkillDictionary() : null,
                        ItemType.Axe => transform.Find($"Armature/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/[Weapon]_Right_Hand/AxeHandle/{iib.thisItem.itemName}").TryGetComponent(out Weapon.BaseAxe wb) ? wb.skills = iib.GetEquipedSkillDictionary() : null,
                        ItemType.Lance => transform.Find($"Armature/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/[Weapon]_Right_Hand/LanceHandle/{iib.thisItem.itemName}").TryGetComponent(out Weapon.BaseLance wb) ? wb.skills = iib.GetEquipedSkillDictionary() : null,
                        _ => throw new System.Exception()
                    };
                }

            }*/
            if (Player.EquipmentManager.equipedDic.ContainsKey(SuitParts.Main_Weapon))
                Player.OutlanderStateMachine.PlayerSkill = Player.EquipmentManager.equipedDic[SuitParts.Main_Weapon]?.GetEquipedSkillDictionary();
        }

        public bool UseSkill(SkillScriptable skill)
        {
            if (!Player.InventorySkillSystems.IsThisSkillIsOnCoolingDown(skill))
            {
                if (Player.OutlanderStateMachine.PlayerMP >= skill.manaUsage)
                {
                    switch (skill.skillType)
                    {
                        case SkillScriptable.SkillType.Active:
                            //if (!skillReady.Contains(skill.skillName))

                            /*bool isCrit = Random.Range(0.0f, 100.0f) <= po.Crit;
                            float isDamageCrit = skill.skillDamageModifier.operatorType switch
                            {
                                SkillScriptable.OperatorType.Addition => isCrit ? (po.AttackDamage + skill.skillDamageModifier.value) * 2f : po.AttackDamage + skill.skillDamageModifier.value,
                                SkillScriptable.OperatorType.Multiplication => isCrit ? po.AttackDamage * skill.skillDamageModifier.value * 2f : po.AttackDamage * skill.skillDamageModifier.value,
                                SkillScriptable.OperatorType.Subtraction => isCrit ? Mathf.Clamp(po.AttackDamage - skill.skillDamageModifier.value, 1, float.MaxValue) * 2f : Mathf.Clamp(po.AttackDamage - skill.skillDamageModifier.value, 1, float.MaxValue),
                                SkillScriptable.OperatorType.Division => isCrit ? po.AttackDamage / skill.skillDamageModifier.value * 2f : po.AttackDamage / skill.skillDamageModifier.value,
                                _ => throw new System.NotImplementedException()
                            };
                            po.CurAttackDamage = isDamageCrit + Random.Range(0.0f, isDamageCrit * 0.2f);*/
                            curSkillUse = skill;
                            float skillDamage = Mathf.Clamp(skill.skillDamageModifier.fixedDamage + (Player.OutlanderStateMachine.PlayerAtkDmg * skill.skillDamageModifier.multiplierDamage), 1f, float.MaxValue);
                            /*skill.skillDamageModifier.operatorType switch
                            {
                                SkillScriptable.OperatorType.Addition => Mathf.Clamp(po.AttackDamage + skill.skillDamageModifier.value, 1, float.MaxValue),
                                SkillScriptable.OperatorType.Multiplication => Mathf.Clamp(po.AttackDamage * skill.skillDamageModifier.value, 1, float.MaxValue),
                                SkillScriptable.OperatorType.Subtraction => Mathf.Clamp(po.AttackDamage - skill.skillDamageModifier.value, 1, float.MaxValue),
                                SkillScriptable.OperatorType.Division => Mathf.Clamp(po.AttackDamage / skill.skillDamageModifier.value, 1, float.MaxValue),
                                _ => throw new System.NotImplementedException()
                            };*/
                            CmdUseSkillDamage(skillDamage);

                            Player.InventorySkillSystems.StartCountingCoolDown(skill);
                            Player.InventorySkillSystems.DisplayCoolDownSkill(skill);
                            return true;


                        case SkillScriptable.SkillType.Support:
                            //if (!skillReady.Contains(skill.skillName))

                            skill.skillStatuses.ForEach(x => Player.PlayerStatisticManager.StatModifier(skill.skillName + x.statusType.ToString(), x.statusType, x.operatorType switch
                            {
                                SkillScriptable.OperatorType.Addition => "+",
                                SkillScriptable.OperatorType.Subtraction => "-",
                                SkillScriptable.OperatorType.Multiplication => "+%",
                                SkillScriptable.OperatorType.Division => "-%",
                                _ => throw new System.NotImplementedException()
                            }, x.value, skill.skillDuration));

                            CmdUseSupportSkill(skill);
                            Player.InventorySkillSystems.StartCountingCoolDown(skill);
                            Player.InventorySkillSystems.DisplayCoolDownSkill(skill);
                            return true;

                    }
                }
                else
                {
                    ClientTriggerEventManager.Instance.ManaEmpty();
                }
            }
            else
            {
                ClientTriggerEventManager.Instance.Cooldown();
            }
            return false;
        }

        /*public void ClearCooldownSkills()
        {
            //skillReady.Clear();
            StopAllCoroutines();
        }*/



        /*IEnumerator WaitForCooldown(SkillScriptable skill)
        {
            float time = skill.skillCooldown;
            while (time > 0f)
            {
                yield return new WaitForSeconds(1f);
                time -= 1f;
            }
            skillReady.Remove(skill.skillName);
        }*/

        //[Command]
        //private void CmdSetSkillStatistic(PlayerStatisticManager.PlayerStatistic skillStatistic)
        //{
            //PlayerStatisticManager psm = GetComponent<PlayerStatisticManager>();
            //psm.skillPlayerStatistic = skillStatistic;
        //}

        [Command]
        private void CmdUseSupportSkill(SkillScriptable skill)
        {
            skill.skillStatuses.ForEach(x => Player.PlayerStatisticManager.StatModifier(skill.skillName + x.statusType.ToString(), x.statusType, x.operatorType switch
            {
                SkillScriptable.OperatorType.Addition => "+",
                SkillScriptable.OperatorType.Subtraction => "-",
                SkillScriptable.OperatorType.Multiplication => "+%",
                SkillScriptable.OperatorType.Division => "-%",
                _ => throw new System.NotImplementedException()
            }, x.value, skill.skillDuration));
            //skillReady.Add(skill.skillName);
        }

        [Command]
        private void CmdUseSkillDamage(float damage)
        {
            Player.OutlanderStateMachine.OnSkill = true;
            Player.OutlanderStateMachine.PlayerSkillDmg = damage;
        }
    }
}

