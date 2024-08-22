using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationIdleState : PlayerAnimationBaseState
    {
        public PlayerAnimationIdleState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
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
            // Debug.Log("ENTER_IDLE_ANIM_STATE");
            _context.PlayerAnimState = this.GetType().ToString();
            _context.AnimSubStateIndex = PlayerMovementState.IDLE;
            // if (!_context.Player.MovementStateMachine.IsClimb)

            if (!_context.Player.MovementStateMachine.IsFall)
            {
                //_context.SetAnimationCrossfade("Idle", 0.1f);
                _context.SetAnimationParameter("moving", false);
            }
        }

        public override void ExitPlayerState()
        {
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            CheckSwitchState();
        }
    }

}
