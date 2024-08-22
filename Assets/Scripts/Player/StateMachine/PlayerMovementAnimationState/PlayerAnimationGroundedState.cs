using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationGroundedState : PlayerAnimationBaseState
    {
        public PlayerAnimationGroundedState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
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
            _context.AnimStateIndex = PlayerMovementState.GROUNDED;
            // Debug.Log("ENTER_GROUND_ANIM_STATE");
            
        }

        public override void ExitPlayerState()
        {
        }

        public override void InitializeSubState()
        {
            SetSubState(_stateData.SetState(_context.AnimSubStateIndex));
        }

        public override void UpdatePlayerState()
        {
            CheckSwitchState();

            //if (_context.Player.MovementStateMachine.MovementInput.y != 0)
            //{
            //    _context.SetAnimationParameter<float>("moveZ", Mathf.Abs(_context.Player.MovementStateMachine.MovementInput.y));
            //}
            //else if (_context.Player.MovementStateMachine.MovementInput.x != 0)
            //{
            //    _context.SetAnimationParameter<float>("moveZ", Mathf.Abs(_context.Player.MovementStateMachine.MovementInput.x));
            //}
        }
    }

}
