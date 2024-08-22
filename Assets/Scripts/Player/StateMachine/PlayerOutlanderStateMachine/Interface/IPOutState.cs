using UnityEngine;

public interface IPOutState
{
    public void EnterState();
    public void UpdatePlayerState();
    public void ExitPlayerState();
    public void CheckSwitchState();
    public void SwitchingCurrentWeaponState();
}
