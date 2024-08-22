using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerJumpedState : PlayerBaseState
    {
        private Vector3 movement;

        public PlayerJumpedState(PlayerMovementStateMachine currentContext, PlayerMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
            _isRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (_context.IsGround && !_context.IsJump)
            {
                SwitchState(_stateData.Grounded());
            }
            else if (_context.IsSwim)
            {
                SwitchState(_stateData.Swim());
            }
            else if (_context.IsClimb)
            {
                SwitchState(_stateData.Climb());
            }
            else if (_context.IsFall)
            {
                SwitchState(_stateData.Fall());
            }
        }

        public override void EnterState()
        {
            _context.PlayerState = this.GetType().ToString();
            _context.MovementStateIndex = PlayerMovementState.JUMP;
            // Debug.Log($"Enter : {this.GetType().ToString()}");

            _context.Player.PlayerStamina.DecreaseStamina(20);

            JumpVelocityHandler();
        }

        public override void ExitPlayerState()
        {
            // Debug.Log($"Exit : {this.GetType().ToString()}");
            // _context.CurrentSpeed = (_context.IsSprint) ? _context.SprintSpeed : _context.WalkSpeed;

            _context.PlayerVelocity = new Vector3(0, _context.PlayerVelocity.y, 0);
            _context.IsJump = false;
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
            //Debug.Log($"Update : {this.GetType().ToString()} {_currentSubState.GetType().ToString()}");
            JumpHandler(Time.deltaTime);
            WallDectection();

            CheckSwitchState();
        }

        private void JumpVelocityHandler()
        {
            _context.CurrentSpeed = (_context.IsSprint) ? _context.SprintSpeed * 0.66f : _context.WalkSpeed;

            _context.PlayerVelocity = new Vector3(0f, Mathf.Sqrt(_context.JumpForce * _context.Gravity), 0f);

            _context.IsGround = false;
        }

        private void JumpHandler(float timeDelta)
        {
            // if (!_context.IsJump && _context.Player.CharacterController.isGrounded)
            //     _context.IsGround = _context.Player.CharacterController.isGrounded;

            if (_context.PlayerVelocity.y < 0)
                _context.IsFall = true;


            movement = new Vector3(_context.MovementInput.x, 0, _context.MovementInput.y);

            movement = _context.m_Camera.transform.forward * movement.z + _context.m_Camera.transform.right * movement.x;
            movement.y = 0;

            Vector3 airMovement = movement * timeDelta * _context.CurrentSpeed;
            //_context.Player.CharacterController.Move(airMovement);

            _context.PlayerVelocity += new Vector3(airMovement.x, _context.Gravity * timeDelta, airMovement.y);
            _context.Player.CharacterController.Move(_context.PlayerVelocity * timeDelta);
        }

        private void WallDectection()
        {
            if (Physics.SphereCast(_context.transform.position + (Vector3.up * _context.Player.CharacterController.height * 0.5f), 0.2f, _context.transform.forward, out RaycastHit wallHit, 0.5f, _context.WallLayer))
            {
                _context.WallHit = wallHit;
                if (_context.MovementInput == Vector2.zero) return;

                _context.IsClimb = true;
                _context.IsGround = false;
            }
        }
    }

}
