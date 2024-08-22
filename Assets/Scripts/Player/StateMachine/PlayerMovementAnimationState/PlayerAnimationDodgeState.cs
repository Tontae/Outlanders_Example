using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerAnimationDodgeState : PlayerAnimationBaseState
    {
        public PlayerAnimationDodgeState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData playerStateFactory) : base(currentContext, playerStateFactory)
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
            _context.AnimStateIndex = PlayerMovementState.DODGE;

            _context.SetRigWeight(0);
            _context.SetAnimationParameter<bool>("onDodge", true);
            _context.SetAnimationCrossfade("Dodge", 0.1f);
        }

        public override void ExitPlayerState()
        {
            // _context.SetAnimation("Idle", 0.1f);
            _context.SetAnimationParameter<bool>("onDodge", false);
            _context.SetRigWeight(1f);
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
