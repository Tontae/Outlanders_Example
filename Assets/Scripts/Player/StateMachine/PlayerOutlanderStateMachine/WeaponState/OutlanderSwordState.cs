using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class OutlanderSwordState : OutlanderBaseState
    {
        // protected override bool OnSkill
        // {
        //     get => _context.OnSkill;
        //     set => Skill();
        // }
        private float penaltyAnimSpeed = -0.4f;

        public OutlanderSwordState(PlayerOutlanderStateMachine currentContext, OutlanderStateData outlanderStateData) : base(currentContext, outlanderStateData)
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
            //Debug.Log($"Enter : {_context.nameOfState}");

            // if (Player == null)
            //     return;

            var weaponAnimatorController = Player.WeaponManager.WeaponAnimatorList.Find(x => x.weaponType == WeaponManager.WeaponType.Sword).animator;
            Player.Animator.runtimeAnimatorController = weaponAnimatorController;
            Player.Animator.SetFloat("speedModify", _context.PlayerAtkSpeed + penaltyAnimSpeed);
        }

        public override void ExitPlayerState()
        {
            //Debug.Log($"Exit : {_context.nameOfState}");
        }

        public override void UpdatePlayerState()
        {
            //Debug.Log($"Update : {_context.nameOfState}");
            CheckSwitchState();
        }

        public override void SwitchingCurrentWeaponState()
        {
            base.SwitchingCurrentWeaponState();
        }

        // protected override void Skill()
        // {
        //     base.Skill();
        // }
    }
}

