using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerMovementStateMachine currentContext, PlayerMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
            _isRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (_context.IsJump)
            {
                SwitchState(_stateData.Jumped());
            }
            else if (_context.IsSwim)
            {
                SwitchState(_stateData.Swim());
            }
            else if (!_context.IsGround && !_context.IsClimb && !_context.IsSwim)
            {
                //Physics.Raycast(_context.transform.position, Vector3.down, out RaycastHit hit, 100);
                //if (hit.distance >= 0.5)
                SwitchState(_stateData.Fall());
            }
            else if (_context.IsClimb)
            {
                SwitchState(_stateData.Climb());
            }
            else if (_context.IsCrouch)
            {
                SwitchState(_stateData.Crouch());
            }
            else if (_context.IsDodge)
            {
                SwitchState(_stateData.Dodge());
            }
        }

        public override void EnterState()
        {
            // Debug.Log($"Enter : {this.GetType().ToString()}");
            _context.PlayerState = this.GetType().ToString();
            _context.MovementStateIndex = PlayerMovementState.GROUNDED;

            _context.IsFall = false;
        }

        public override void ExitPlayerState()
        {
            // Debug.Log($"Exit : {this.GetType().ToString()}");
        }

        public override void InitializeSubState()
        {
            if (_context.IsMoving && _context.IsSprint)
            {
                SetSubState(_stateData.Run());
            }
            else if (_context.IsMoving && !_context.IsSprint)
            {
                SetSubState(_stateData.Walk());
            }
            else
            {
                SetSubState(_stateData.Idle());
            }
        }

        public override void UpdatePlayerState()
        {
            //Debug.Log($"Update : {this.GetType().ToString()} {_currentSubState?.GetType().ToString()}");
            GravityHandler();
            WallDectection();

            CheckSwitchState();
        }

        private void GravityHandler()
        {
            _context.PlayerVelocity = new Vector3(0, _context.Gravity * Time.deltaTime, 0);
            _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime);
        }

        private void WallDectection()
        {
            if (_context.Player.PlayerStamina.Stamina <= 15f) return;

            if (Physics.SphereCast(_context.transform.position + (Vector3.up * _context.Player.CharacterController.height * 0.5f), 0.2f, _context.transform.forward, out RaycastHit wallHit, 0.5f, _context.WallLayer))
            {
                _context.WallHit = wallHit;
                if (_context.MovementInput == Vector2.zero) return;
                if (_context.Player.OutlanderStateMachine.OnSkill) return;

                _context.IsClimb = true;
                _context.IsGround = false;
            }
        }
    }
}
