using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class OutlanderStunState : OutlanderBaseState
    {
        public OutlanderStunState(PlayerOutlanderStateMachine currentContext, OutlanderStateData outlanderStateData) : base(currentContext, outlanderStateData)
        {
        }

        public override void CheckSwitchState()
        {
            if (_context.OnDie)
            {
                SwitchState(_StateData.Die());
                return;
            }

            if (!_context.OnStun)
            {
                SwitchingCurrentWeaponState();
            }
        }

        public override void EnterState()
        {
            _context.nameOfState = this.GetType().ToString();

            var PlayerComponents = PlayerManagers.Instance.PlayerComponents;
            var weaponAnimatorController = PlayerComponents.WeaponManager.WeaponAnimatorList.Find(x => x.weaponType == WeaponManager.WeaponType.None).animator;
            PlayerComponents.Animator.runtimeAnimatorController = weaponAnimatorController;
        }

        public override void ExitPlayerState()
        {
        }

        public override void UpdatePlayerState()
        {
            CheckSwitchState();
        }

        public override void SwitchingCurrentWeaponState()
        {
            base.SwitchingCurrentWeaponState();
        }

        // protected override void Skill()
        // {
        //     return;
        // }
    }
}

