using System.Collections.Generic;
using UnityEngine;

public interface IPOutData
{
    public bool OnDie { get; set; }

    public float PlayerHP { get; set; }
    public float PlayerMaxHP { get; set; }
    public float PlayerMP { get; set; }
    public float PlayerMaxMP { get; set; }
    public float PlayerAtkDmg { get; set; }
    public float PlayerAtkSpeed { get; set; }
    public float PlayerSkillDmg { get; set; }
    public float PlayerDef { get; set; }
    public float PlayerCritRate { get; set; }
    public float PlayerCritDmg { get; set; }
}

public interface IPDamageCommand
{
    public Queue<float> PlayerDamageBufferLog { get; set; }
    public Queue<GameObject> EnemyDamageBufferLog { get; set; }

    public void CmdDamageIgnoreDefense(PlayerDamageRecieveType _type, float damageAmount);
    public void Damage(GameObject _obj, float _damage);
    public void Damage(PlayerDamageRecieveType _type, float _damage);
}

public interface IPAttributeCommand
{
    public void CmdInitializingPlayer();
    public void CmdSetGodmode();
    public void CmdSetPlayerBoolAttribute();
    public void CmdSetPlayerFloatAttribute();
}

public interface IPStamina
{
    public bool IsMin { get; }
    public bool IsMax { get; }
    public bool IsRegen { get; set; }

    public float MinStamina { get; set; }
    public float MaxStamina { get; set; }
    public float Stamina { get; set; }
}
