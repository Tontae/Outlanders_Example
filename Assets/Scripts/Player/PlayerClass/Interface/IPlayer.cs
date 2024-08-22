using UnityEngine;

public interface IPlayer
{
    public bool Die { get; set; }
    public bool OnInteract { get; set; }
    public bool GetWeaponAction();
    public bool Skilling { get; set; }
    public bool IsCounter { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; }
    public float Mana { get; set; }
    public float MaxMana { get; }
    public float AttackDamage { get; set; }
    public float CurAttackDamage { get; set; }
    public float Defense { get; set; }
    public float Crit { get; set; }
    public float CritDamage { get; set; }
    public float AttackSpeed { get; set; }
    public float MoveSpeed { get; set; }
    // public Transform Spawnpoint { get; set; }
    public void Damage(string enemy, float damageAmount);
    public void Damage(GameObject enemy, float damageAmount);
    public void CmdDamageIgnoreDefense(string environment, float damageAmount);
    public void IncreasePlayerHealth(float _healAmount);
    public void IncreasePlayerMana(float _regenAmount);
    public void InitializingPlayer();
    public void IncreaseStats(float _hp, float _atk, float _def, float _mp);
    public void SetCharacterStats(PlayerStatisticManager.PlayerStatistic playerStatistic);
    public RuntimeAnimatorController RuntimeAnimator { get; set; }
    public PlayerScriptable Character { get; set; }
}