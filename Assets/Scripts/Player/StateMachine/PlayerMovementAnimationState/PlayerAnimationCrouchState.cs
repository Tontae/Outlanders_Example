using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationCrouchState : PlayerAnimationBaseState
    {
        public PlayerAnimationCrouchState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
        {
            _isRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (_context.AnimStateIndex != _context.Player.MovementStateMachine.MovementStateIndex)
                SwitchState(_stateData.SetState(_context.Player.MovementStateMachine.MovementStateIndex));
        }

        public override void EnterState()
        {
            _context.PlayerAnimState = this.GetType().ToString();
            _context.AnimStateIndex = PlayerMovementState.CROUCH;

            _context.SetRigWeight(0);
        }

        public override void ExitPlayerState()
        {
            _context.SetRigWeight(1f);
            _context.SetAnimationParameter("onCrouch", false);
        }

        public override void InitializeSubState()
        {
            SetSubState(_stateData.SetState(_context.AnimSubStateIndex));
        }

        public override void UpdatePlayerState()
        {
            _context.SetRigWeight(0);
            _context.SetAnimationParameter("onCrouch", true);
            CheckSwitchState();
        }
    }

}
