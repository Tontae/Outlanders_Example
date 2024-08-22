using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerSwimState : PlayerBaseState
    {
        private Vector3 movement;
        private float waterSurface;
        private const float offsetHeight = 1.2f;
        public PlayerSwimState(PlayerMovementStateMachine currentContext, PlayerMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
            _isRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (_context.IsGround && !_context.IsSwim)
                SwitchState(_stateData.Grounded());
        }

        public override void EnterState()
        {
            _context.PlayerState = this.GetType().ToString();
            _context.MovementStateIndex = PlayerMovementState.SWIM;

            waterSurface = Physics.OverlapSphere(_context.transform.position + Vector3.up, 1f, 1 << 4, QueryTriggerInteraction.Collide)[0].transform.position.y + offsetHeight;
            //_context.Gravity = 0;
            //_context.CurrentSpeed = SwimSpeed();
        }

        public override void ExitPlayerState()
        {
            //_context.Gravity = Physics.gravity.y;
            //_context.CurrentSpeed = _context.WalkSpeed;
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            //Debug.Log($"Update : {this.GetType().ToString()} {_currentSubState?.GetType().ToString()}");

            Bouyancy();
            Swimming();
            CheckSwitchState();
        }

        private float SwimSpeed() => (_context.IsSprint) ? _context.SwimSpeed * 2f : _context.SwimSpeed;

        private void Bouyancy()
        {
            float bouyancy = Mathf.Lerp(0f, 1f, Mathf.Abs(_context.transform.position.y - waterSurface) * Time.deltaTime);
            _context.PlayerVelocity = new Vector3(0f, _context.transform.position.y < waterSurface ? bouyancy : -bouyancy, 0f);
            _context.Player.CharacterController.Move(_context.PlayerVelocity);
        }

        private void Swimming()
        {
            movement = new Vector3(_context.MovementInput.x, 0, _context.MovementInput.y);

            movement = _context.m_Camera.transform.forward * movement.z + _context.m_Camera.transform.right * movement.x;
            movement.y = 0;

            if (!_context.IsMoving) return;
            _context.Player.CharacterController.Move(movement * Time.deltaTime * SwimSpeed());
            _context.Player.PlayerStamina.DecreaseStamina(_context.IsSprint ? 5f * Time.deltaTime : 2f * Time.deltaTime);
            if (_context.Player.PlayerStamina.Stamina <= 0f)
                _context.Player.OutlanderStateMachine.CmdDamageIgnoreDefense(PlayerDamageRecieveType.DROWN, _context.Player.PlayerStatisticManager.GetFinalStat(StatusType.MHP));
        }
    }
}