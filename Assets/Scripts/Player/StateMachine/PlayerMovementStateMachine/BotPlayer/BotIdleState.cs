using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class BotIdleState : BotBaseState
    {
        public BotIdleState(BotPSM currentContext, BotMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
        }

        public override void CheckSwitchState()
        {
            if (_context.IsMoving)
            {
                SwitchState(_stateData.Walk());
            }
            else if (_context.IsSprint)
            {
                SwitchState(_stateData.Run());
            }
            //else if (_context.IsDodge)
            //{
            //    SwitchState(_stateData.Dodge());
            //}
            //else if (_context.IsCrouch)
            //{
            //    SwitchState(_stateData.Crouch());
            //}

            // if (_context.IsClimb)
            // {
            //     SwitchState(_stateData.Climb());
            // }

            // if (_context.IsJump)
            // {
            //     SwitchState(_stateData.Jumped());
            // }
        }

        public override void EnterState()
        {
            _context.PlayerState = this.GetType().ToString();
            // Debug.Log($"Enter : {this.GetType().ToString()}");
            _context.CurrentSpeed = 0;
            _context.Player.Animator.SetBool("moving", false);
        }

        public override void ExitPlayerState()
        {
            // Debug.Log($"Exit : {this.GetType().ToString()}");
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            // Debug.Log($"Update : {this.GetType().ToString()} {_currentSubState == null}");
            //if (!_context.IsClimb)
            //GravityHandler();
            if (_context.Player.CharacterController.isGrounded && _context.PlayerVelocity.y < 0)
                _context.PlayerVelocity = new Vector3(_context.PlayerVelocity.x, 0, _context.PlayerVelocity.z);

            CheckSwitchState();
        }

        private void GravityHandler()
        {
            _context.PlayerVelocity += new Vector3(0, _context.Gravity * Time.deltaTime, 0);
            _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime);
        }
    }
}
