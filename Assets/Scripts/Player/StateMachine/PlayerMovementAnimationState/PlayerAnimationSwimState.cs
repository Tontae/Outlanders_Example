using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationSwimState : PlayerAnimationBaseState
    {
        public PlayerAnimationSwimState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
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
            _context.AnimStateIndex = PlayerMovementState.SWIM;

            _context.SetRigWeight(0);
            _context.SetAnimationParameter("swimming", true);
        }

        public override void ExitPlayerState()
        {
            _context.SetAnimationParameter("swimMove", false);
            _context.SetAnimationParameter("swimming", false);
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            _context.SetRigWeight(0);
            if (_context.Player.MovementStateMachine.IsMoving)
            {
                _context.SetAnimationParameter("swimMove", true);
            }
            else
            {
                _context.SetAnimationParameter("swimMove", false);
            }
            _context.SetAnimationParameter("swimming", true);
            CheckSwitchState();
        }
    }

}
