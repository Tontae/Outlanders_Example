using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class BotWalkState : BotBaseState
    {
        private Vector3 movement;
        public BotWalkState(BotPSM currentContext, BotMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
        }

        public override void CheckSwitchState()
        {
            if (_context.IsSprint)
            {
                SwitchState(_stateData.Run());
            }
            else if (!_context.IsMoving)
            {
                SwitchState(_stateData.Idle());
            }
            //else if (_context.IsCrouch)
            //{
            //    SwitchState(_stateData.Crouch());
            //}
            //else if (_context.IsDodge)
            //{
            //    SwitchState(_stateData.Dodge());
            //}

            //else if (_context.IsClimb)
            //{
            //    SwitchState(_stateData.Climb());
            //}

            // if (_context.IsJump)
            // {
            //     SwitchState(_stateData.Jumped());
            // }
        }

        public override void EnterState()
        {
            // Debug.Log($"Enter : {this.GetType().ToString()}");
            _context.PlayerState = this.GetType().ToString();
            _context.CurrentSpeed = _context.WalkSpeed;
            _context.Player.Animator.SetBool("moving", true);
        }

        public override void ExitPlayerState()
        {
            // Debug.Log($"Exit : {this.GetType().ToString()}");
            _context.Player.Animator.SetBool("moving", false);
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            // Debug.Log($"Update : {this.GetType().ToString()} {_currentSubState == null}");
            //GravityHandler();
            // if (_context.Player.OutlanderStateMachine.OnWeaponAction) return;
            if (_context.IsClimb) return;

            Movement();

            CheckSwitchState();
        }

        private void Movement()
        {
            if (_context.IsGround && _context.PlayerVelocity.y < 0)
                _context.PlayerVelocity = new Vector3(_context.PlayerVelocity.x, 0, _context.PlayerVelocity.z);

            movement = new Vector3(_context.MovementInput.x, 0, _context.MovementInput.y);

            movement = _context.m_Camera.transform.forward * movement.z + _context.m_Camera.transform.right * movement.x;
            movement.y = 0;

            if (!_context.IsMoving) return;
            _context.Player.CharacterController.Move(AdjustVelocityToSlope(movement) * Time.deltaTime * (!_context.IsCrouch ? _context.CurrentSpeed : _context.CurrentSpeed / 2f));
        }

        private void GravityHandler()
        {
            _context.PlayerVelocity += new Vector3(0, _context.Gravity * Time.deltaTime, 0);
            _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime);
        }

        private Vector3 AdjustVelocityToSlope(Vector3 velocity)
        {
            Ray ray = new Ray(_context.transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
            {
                Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                Vector3 adjustedVelocity = slopeRotation * velocity;

                if (adjustedVelocity.y < 0)
                {
                    return adjustedVelocity;
                }
            }

            return velocity;
        }
    }
}
