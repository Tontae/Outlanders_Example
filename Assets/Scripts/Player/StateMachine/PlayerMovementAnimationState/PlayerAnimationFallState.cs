using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationFallState : PlayerAnimationBaseState
    {
        public PlayerAnimationFallState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
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
            _context.AnimStateIndex = PlayerMovementState.FALL;

            _context.SetAnimationCrossfade("floating", 0.1f);
            _context.SetRigWeight(0);
            // _context.SetAnimation("OnGround", true);
        }

        public override void ExitPlayerState()
        {
            // if (!_context.Player.MovementStateMachine.IsClimb)
            // _context.SetAnimation("Idle", 0.1f);
            // _context.SetAnimation("OnGround", false);
            // _context.SetAnimation("moving", false);
            _context.SetRigWeight(1);
        }

        public override void InitializeSubState()
        {
            SetSubState(_stateData.SetState(_context.AnimSubStateIndex));
        }

        public override void UpdatePlayerState()
        {
            _context.SetRigWeight(0);

            if(!_context.Player.OutlanderStateMachine.OnDie)
                _context.SetAnimationCrossfade("floating", 0.1f);

            CheckSwitchState();
        }
    }

}
