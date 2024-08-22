using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerDodgeState : PlayerBaseState
    {
        private Vector3 movement;
        public PlayerDodgeState(PlayerMovementStateMachine currentContext, PlayerMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
            _isRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (_context.IsSwim)
                SwitchState(_stateData.Swim());
            else if (!_context.IsDodge)
            {
                if (_context.IsGround)
                    SwitchState(_stateData.Grounded());
                else
                    SwitchState(_stateData.Fall());
            }


            // if (_context.IsJump)
            // {
            //     SwitchState(_stateData.Jumped());
            // }
        }

        public override void EnterState()
        {
            _context.PlayerState = this.GetType().ToString();
            _context.MovementStateIndex = PlayerMovementState.DODGE;

            _context.Player.PlayerStamina.DecreaseStamina(20f);
        }

        public override void ExitPlayerState()
        {
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            //Debug.Log($"Update : {this.GetType().ToString()} {_currentSubState?.GetType().ToString()}");
            GravityHandler();
            Dodge();

            CheckSwitchState();
        }

        private void Dodge()
        {
            if (_context.IsGround && _context.PlayerVelocity.y < 0)
                _context.PlayerVelocity = new Vector3(_context.PlayerVelocity.x, 0, _context.PlayerVelocity.z);

            if (_context.IsMoving)
            {
                movement = new Vector3(_context.MovementInput.x, 0, _context.MovementInput.y);

                movement = _context.m_Camera.transform.forward * movement.z + _context.m_Camera.transform.right * movement.x;
                movement.y = 0;
            }
            else
            {
                movement = _context.transform.forward;
            }

            // if (!_context.IsMoving) return;
            _context.Player.CharacterController.Move(AdjustVelocityToSlope(movement) * Time.deltaTime * _context.DodgeSpeed);
            // _context.PlayerStamina.Stamina -= Time.deltaTime;
            // _context.Player.PlayerStamina.DecreaseStamina(20f);
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
