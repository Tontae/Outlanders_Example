using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class OutlanderDieState : OutlanderBaseState
    {
        public OutlanderDieState(PlayerOutlanderStateMachine currentContext, OutlanderStateData outlanderStateData) : base(currentContext, outlanderStateData)
        {
        }

        public override void CheckSwitchState()
        {
            if (_context.OnDie) return;
        }

        public override void EnterState()
        {
            _context.nameOfState = this.GetType().ToString();
            //Debug.Log($"Enter : {_context.nameOfState}");

            _context.Player.PlayerInputManager.SetInputDisable();

            _context.DropAllItemOnDie();

            //_context.Player.WeaponManager.currentWeaponType = WeaponManager.WeaponType.None;

            if (!_context.Player.MovementStateMachine.IsSwim)
                _context.Player.Animator.CrossFade("die", 0.1f);
            else
                _context.Player.AnimationStateMachine.SetAnimationCrossfade("Drown", 0.1f);
        }

        public override void ExitPlayerState()
        {
            //Debug.Log($"Exit : {_context.nameOfState}");
            _context.Player.PlayerInputManager.SetInputEnable();

            _context.DropAllItemOnDie();

            _context.Player.OutlanderStateMachine.ResetPlayerBoolData();
            _context.Player.MovementStateMachine.ResetMovementBoolData();
        }

        public override void UpdatePlayerState()
        {

        }

        // public override void SwitchingCurrentWeaponState()
        // {
        //     base.SwitchingCurrentWeaponState();
        // }

        // protected override void Skill()
        // {
        //     return;
        // }
    }
}

