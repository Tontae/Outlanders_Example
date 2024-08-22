using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerRunState : PlayerBaseState
    {
        private Vector3 movement;
        public PlayerRunState(PlayerMovementStateMachine currentContext, PlayerMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
        }

        public override void CheckSwitchState()
        {
            if (!_context.IsSprint || _context.Player.PlayerStamina.IsMin)
            {
                SwitchState(_stateData.Walk());
            }
            else if (!_context.IsMoving)
            {
                SwitchState(_stateData.Idle());
            }
            /*else if (_context.IsCrouch)
            {
                SwitchState(_stateData.Crouch());
            }*/

            //if (_context.IsDodge)
            //{
            //    SwitchState(_stateData.Dodge());
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
            // Debug.Log($"Enter : {this.GetType().ToString()}");
            _context.PlayerState = this.GetType().ToString();
            _context.MovementSubStateIndex = PlayerMovementState.RUN;

            _context.CurrentSpeed = _context.SprintSpeed;
        }

        public override void ExitPlayerState()
        {
            // Debug.Log($"Exit : {this.GetType().ToString()}");
            _context.IsSprint = false;
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            // Debug.Log($"Update : {this.GetType().ToString()} {_currentSubState == null}");
            //GravityHandler();
            //if (_context.Player.OutlanderStateMachine.OnWeaponAction) return;

            if (!_context.Player.OutlanderStateMachine.OnWeaponAction && !(_context.Player.OutlanderStateMachine.OnSkill && !_context.Player.OutlanderStateMachine.OnSkillMove))
                Movement();

            CheckSwitchState();
        }

        private void Movement()
        {
            //if (_context.IsGround && _context.PlayerVelocity.y < 0)
            //_context.PlayerVelocity = new Vector3(_context.PlayerVelocity.x, 0, _context.PlayerVelocity.z);

            movement = new Vector3(_context.MovementInput.x, 0, _context.MovementInput.y);

            movement = _context.m_Camera.transform.forward * movement.z + _context.m_Camera.transform.right * movement.x;
            movement.y = 0;

            if (!_context.IsMoving) return;
            // if (_context.Player.OutlanderStateMachine.OnAttackState) return;

            _context.Player.CharacterController.Move(AdjustVelocityToSlope(movement) * Time.deltaTime * _context.CurrentSpeed);
            // _context.PlayerStamina.Stamina -= Time.deltaTime;
            _context.Player.PlayerStamina.DecreaseStamina(4f * Time.deltaTime);
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
