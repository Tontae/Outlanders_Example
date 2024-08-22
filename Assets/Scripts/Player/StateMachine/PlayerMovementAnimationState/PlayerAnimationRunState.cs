using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationRunState : PlayerAnimationBaseState
    {
        public PlayerAnimationRunState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
        {
        }

        public override void CheckSwitchState()
        {
            //Debug.Log($"Switch:{_context.AnimSubStateIndex} > {_context.Player.MovementStateMachine.MovementSubStateIndex}");
            if (_context.AnimSubStateIndex != _context.Player.MovementStateMachine.MovementSubStateIndex)
                SwitchState(_stateData.SetState(_context.Player.MovementStateMachine.MovementSubStateIndex));
        }

        public override void EnterState()
        {
            // Debug.Log("ENTER_RUN_ANIM_STATE");
            _context.PlayerAnimState = this.GetType().ToString();
            _context.AnimSubStateIndex = PlayerMovementState.RUN;

            if (_context.Player.OutlanderStateMachine.OnWeaponAction)
                _context.SetAnimationParameter("moving", false);
            else
                _context.SetAnimationParameter("moving", true);
        }

        public override void ExitPlayerState()
        {
            if (!_context.Player.MovementStateMachine.IsClimb)
                _context.SetAnimationParameter("moving", false);
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            if (_context.Player.OutlanderStateMachine.OnWeaponAction)
                _context.SetAnimationParameter("moving", false);
            else
                _context.SetAnimationParameter("moving", true);
            // _context.SetFloatAnimation("moveX", _context.Player.MovementStateMachine.MovementInput.x);
            // _context.SetFloatAnimation("moveZ", _context.Player.MovementStateMachine.MovementInput.y);

            CheckSwitchState();
        }
    }

}
