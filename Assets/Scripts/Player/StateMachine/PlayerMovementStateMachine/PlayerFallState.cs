using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 movement;
        private float enterFallPosY;
        //private float fallDamage = 10f;
        //private float fallDamageMultiplier = 2.5f;
        private float fallDamageDistanceTrigger = 6f;

        public PlayerFallState(PlayerMovementStateMachine currentContext, PlayerMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
            _isRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (_context.IsGround)
            {
                //_context.IsGround = true;
                SwitchState(_stateData.Grounded());
            }
            /*else if (_context.IsGround && !_context.IsSwim)
            {
                SwitchState(_stateData.Grounded());
            }*/
            else if (_context.IsSwim)
            {
                SwitchState(_stateData.Swim());
            }
            else if (_context.IsClimb)
            {
                SwitchState(_stateData.Climb());
            }
        }

        public override void EnterState()
        {
            // Debug.Log($"Enter : {this.GetType().ToString()}");
            _context.PlayerState = this.GetType().ToString();
            _context.MovementStateIndex = PlayerMovementState.FALL;
            // _context.IsGround = false;
            _context.IsJump = false;
            _context.IsFall = true;

            enterFallPosY = _context.transform.position.y;
        }

        public override void ExitPlayerState()
        {
            // Debug.Log($"Exit : {this.GetType().ToString()}");
            _context.IsFall = false;
            _context.PlayerVelocity = Vector3.zero;
            _context.Player.CharacterController.Move(Vector3.down);

            if (_context.Player.PlayerMatchManager.myManager == null) return;
            if (!_context.Player.PlayerMatchManager.myManager.canInteract) return;
            if (!_context.IsGround || _context.IsSwim) return;
            float tempY = enterFallPosY - _context.transform.position.y - fallDamageDistanceTrigger;
            //Debug.Log($"Distance maxY:{enterFallPosY} - curY:{_context.transform.position.y} - moY:{fallDamageDistanceTrigger} = {tempY}");
            if (tempY > 25f)
                _context.Player.OutlanderStateMachine.CmdDamageIgnoreDefense(PlayerDamageRecieveType.FALL, _context.Player.PlayerStatisticManager.GetFinalStat(StatusType.MHP));
            else if (tempY > 20f)
                _context.Player.OutlanderStateMachine.CmdDamageIgnoreDefense(PlayerDamageRecieveType.FALL, _context.Player.PlayerStatisticManager.GetFinalStat(StatusType.MHP) * 0.7f);
            else if (tempY > 10f)
                _context.Player.OutlanderStateMachine.CmdDamageIgnoreDefense(PlayerDamageRecieveType.FALL, _context.Player.PlayerStatisticManager.GetFinalStat(StatusType.MHP) * 0.5f);
            else if (tempY > 4f)
                _context.Player.OutlanderStateMachine.CmdDamageIgnoreDefense(PlayerDamageRecieveType.FALL, _context.Player.PlayerStatisticManager.GetFinalStat(StatusType.MHP) * 0.3f);
            else if (tempY > 0f)
                _context.Player.OutlanderStateMachine.CmdDamageIgnoreDefense(PlayerDamageRecieveType.FALL, _context.Player.PlayerStatisticManager.GetFinalStat(StatusType.MHP) * 0.1f);
        }

        public override void InitializeSubState()
        {
            if (_context.IsMoving && _context.IsSprint)
            {
                SetSubState(_stateData.Run());
            }
            if (_context.IsMoving && !_context.IsSprint)
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

        private void AirMovement()
        {
            // _context.IsGround = _context.Player.CharacterController.isGrounded;

            if (_context.IsGround && _context.PlayerVelocity.y < 0)
                _context.PlayerVelocity = new Vector3(_context.PlayerVelocity.x, 0, _context.PlayerVelocity.z);

            movement = new Vector3(_context.MovementInput.x, 0, _context.MovementInput.y);

            movement = _context.m_Camera.transform.forward * movement.z + _context.m_Camera.transform.right * movement.x;
            movement.y = 0;

            _context.Player.CharacterController.Move(movement * Time.deltaTime * _context.CurrentSpeed);
        }

        private void GravityHandler()
        {
            
            //Debug.Log($"{Physics.SphereCast(_context.transform.position, _context.Player.CharacterController.radius, Vector3.down, out RaycastHit temp, float.MaxValue, _context.GroundLayer, QueryTriggerInteraction.Ignore)}");
            if (Physics.SphereCast(_context.transform.position + Vector3.up, _context.Player.CharacterController.radius * 0.5f, Vector3.down, out RaycastHit hit, 2f, _context.GroundLayer + (3 << 8)))
            {
                //Debug.Log($"Hit angle:{Vector3.Angle(Vector3.up, hit.normal)} > {_context.Player.CharacterController.slopeLimit}");
                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (hit.collider.gameObject.layer is 8 or 9)
                {

                    Vector3 slipVector = new Vector3(hit.normal.x, 0f, hit.normal.z).normalized * _context.WalkSpeed * Time.deltaTime;
                    _context.PlayerVelocity += slipVector;
                    _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime);
                    //return;
                }

                if (angle > _context.Player.CharacterController.slopeLimit)
                {
                    //var normal = hit.normal;
                    //var yInverse = 1f - normal.y;
                    //_context.PlayerVelocity += new Vector3(yInverse * normal.x, 0f, yInverse * normal.z);
                    float angleDiff = Mathf.Clamp(angle - _context.Player.CharacterController.slopeLimit, 1f, float.MaxValue);

                    Vector3 leftVector = Vector3.Cross(hit.normal, Vector3.down).normalized;
                    Vector3 slopeVector = Vector3.Cross(leftVector, hit.normal).normalized;
                    slopeVector = slopeVector * angleDiff * Time.deltaTime;
                    _context.PlayerVelocity += new Vector3(slopeVector.x, 0f, slopeVector.z);
                }
            }
            _context.PlayerVelocity += new Vector3(0, _context.Gravity * Time.deltaTime, 0);
            _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime);
        }

        private void WallDectection()
        {
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
