using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationClimbState : PlayerAnimationBaseState
    {
        public PlayerAnimationClimbState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
        {
            _isRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (_context.Player.MovementStateMachine.IsClimbToTop) return;

            if (_context.AnimStateIndex != _context.Player.MovementStateMachine.MovementStateIndex)
                SwitchState(_stateData.SetState(_context.Player.MovementStateMachine.MovementStateIndex));
        }

        public override void EnterState()
        {
            // Debug.Log("ENTER_CLIMB_ANIM_STATE");
            _context.PlayerAnimState = this.GetType().ToString();
            _context.AnimStateIndex = PlayerMovementState.CLIMB;

            _context.SetRigWeight(0);
            // _context.SetAnimation("moving", false);
            _context.SetAnimationCrossfade("Climb Blend Tree", 0.1f);
            _context.SetAnimationParameter("climbing", true);
        }

        public override void ExitPlayerState()
        {
            _context.SetRigWeight(1);
            _context.SetAnimationParameter("climbtoTop", false);
            _context.SetAnimationParameter("climbing", false);
            _context.SetAnimationParameter("moving", false);
            _context.SetAnimationCrossfade("Idle", 0.1f);
        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdatePlayerState()
        {
            _context.SetRigWeight(0);

            _context.SetAnimationParameter("climbing", true);

            if (_context.Player.MovementStateMachine.IsClimbToTop)
                _context.SetAnimationParameter("climbtoTop", true);
            // AnimSpeed();

            /*int _speed = 0;

            if (_context.transform.hasChanged && _context.Player.MovementStateMachine.IsClimb)
            {
                _speed = 1;
                _context.transform.hasChanged = false;
            }

            _context.SetAnimationSpeed(_speed);*/

            CheckSwitchState();
        }
    }

}
