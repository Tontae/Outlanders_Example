using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationJumpedState : PlayerAnimationBaseState
    {
        public PlayerAnimationJumpedState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
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
            _context.AnimStateIndex = PlayerMovementState.JUMP;

            _context.SetRigWeight(0);
            _context.SetAnimationCrossfade("jump", 0.1f);
        }

        public override void ExitPlayerState()
        {
            // _context.SetAnimation("OnGround", true);
        }

        public override void InitializeSubState()
        {
            SetSubState(_stateData.SetState(_context.AnimSubStateIndex));
        }

        public override void UpdatePlayerState()
        {
            CheckSwitchState();
        }
    }

}
