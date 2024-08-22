using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class OutlanderBowState : OutlanderBaseState
    {
        // protected override bool OnSkill
        // {
        //     get => _context.OnSkill;
        //     set => Skill();
        // }
        private float penaltyAnimSpeed = 0f;

        public OutlanderBowState(PlayerOutlanderStateMachine currentContext, OutlanderStateData outlanderStateData) : base(currentContext, outlanderStateData)
        {
        }

        public override void CheckSwitchState()
        {
            if (_context.OnDie)
            {
                SwitchState(_StateData.Die());
                return;
            }

            if (_context.OnStun)
            {
                SwitchState(_StateData.Stun());
                return;
            }
        }

        public override void EnterState()
        {
            _context.nameOfState = this.GetType().ToString();

            // if (Player == null)
            //     return;

            var weaponAnimatorController = Player.WeaponManager.WeaponAnimatorList.Find(x => x.weaponType == WeaponManager.WeaponType.BowQuiver).animator;
            Player.Animator.runtimeAnimatorController = weaponAnimatorController;
            Player.Animator.SetFloat("speedModify", _context.PlayerAtkSpeed + penaltyAnimSpeed);
        }

        public override void ExitPlayerState()
        {
        }

        public override void UpdatePlayerState()
        {
            CheckAiming();
            CheckSwitchState();
        }

        // protected override void Skill()
        // {
        //     base.Skill();
        // }

        private void CheckAiming()
        {
            if (!_context.OnWeaponAction) return;
            _context.IsAiming = _context.OnWeaponAction;
        }
    }
}
