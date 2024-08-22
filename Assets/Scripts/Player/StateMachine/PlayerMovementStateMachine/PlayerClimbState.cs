using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Outlander.Player
{

    public class PlayerClimbState : PlayerBaseState
    {
        private Vector3 movement;
        private int delayFrameMoveToTop = 10;
        private Vector3 topHit;
        private Vector2 enterMovementInput = Vector2.zero;
        private Vector2 previousMovementInput = Vector2.zero;

        public PlayerClimbState(PlayerMovementStateMachine currentContext, PlayerMovementStateData playerMovementStateData) : base(currentContext, playerMovementStateData)
        {
            _isRootState = true;
            InitializeSubState();
        }

        public override void CheckSwitchState()
        {
            if (_context.IsClimbToTop) return;

            if (_context.Player.PlayerStamina.IsMin)
            {
                SwitchState(_stateData.Fall());
            }
            else if (_context.IsGround && !_context.IsClimb)
            {
                SwitchState(_stateData.Grounded());
            }
            else if (!_context.IsClimb)
            {
                SwitchState(_stateData.Fall());
            }
            else if (_context.IsJump)
            {
                _context.transform.Rotate(Vector3.up, 180f);
                SwitchState(_stateData.Fall());
            }
            else if (_context.IsFall)
            {
                SwitchState(_stateData.Fall());
            }
        }

        public override void EnterState()
        {
            // Debug.Log($"Enter : {this.GetType().ToString()}");
            _context.PlayerState = this.GetType().ToString();
            _context.MovementStateIndex = PlayerMovementState.CLIMB;
            //_context.Gravity = 0f;
            _context.PlayerVelocity = Vector3.zero;
            // InitClimbState();
            if (_context.MovementInput.x != 0 && _context.MovementInput.y == 0)
            {
                enterMovementInput = _context.MovementInput;
                _context.MovementInput = new Vector2(0f, 1f);
                previousMovementInput = _context.MovementInput;
            }
            else if (_context.MovementInput.y < 0)
            {
                enterMovementInput = _context.MovementInput;
                _context.MovementInput = new Vector2(-_context.MovementInput.x, -_context.MovementInput.y);
                previousMovementInput = _context.MovementInput;
            }
            VerticalMovement();

            _context.IsJump = false;
            _context.IsSprint = false;

            delayFrameMoveToTop = 30;
        }

        public override void ExitPlayerState()
        {
            // Debug.Log($"Exit : {this.GetType().ToString()}");

            //_context.Gravity = Physics.gravity.y;
            _context.IsClimb = false;
            _context.PlayerVelocity = Vector3.zero;
        }

        public override void InitializeSubState()
        {
        }

        public override void UpdatePlayerState()
        {
            //Debug.Log($"Update : {this.GetType().ToString()} {_currentSubState?.GetType().ToString()}");
            //ClimbHandler(Time.deltaTime);
            //WallDectection();
            if (enterMovementInput != Vector2.zero)
            {
                if (previousMovementInput != _context.MovementInput)
                    enterMovementInput = Vector2.zero;
                previousMovementInput = _context.MovementInput;
            }

            if (_context.IsClimbToTop)
                MoveToTop();
            else
                VerticalMovement();

            CheckSwitchState();
        }

        private float ClimbSpeed() => _context.IsSprint ? _context.ClimbSpeed * 2f : _context.ClimbSpeed;

        private void MovePlayer(RaycastHit raycastHit)
        {
            //Debug.Log($"Inverse Normal:{raycastHit.normal}");
            Quaternion lookRotation = Quaternion.LookRotation(-raycastHit.normal, Vector3.up);
            //Quaternion lookRotationYAxis = Quaternion.Euler(_context.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, _context.transform.rotation.eulerAngles.z);
            _context.transform.rotation = Quaternion.Euler(_context.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, _context.transform.rotation.eulerAngles.z);

            //Debug.Log($"{Mathf.Sin(_context.transform.rotation.eulerAngles.y * Mathf.Deg2Rad)} {Mathf.Cos(_context.transform.rotation.eulerAngles.y * Mathf.Deg2Rad)} {_context.transform.rotation.eulerAngles.y} {_context.MovementInput}");
            if (_context.MovementInput.x == 0)
                movement = new Vector3(0f, _context.MovementInput.y, 0f);
            else
                movement = new Vector3(Mathf.Sin((90f * _context.MovementInput.x + _context.transform.rotation.eulerAngles.y) * Mathf.Deg2Rad), _context.MovementInput.y, Mathf.Cos((90f * _context.MovementInput.x + _context.transform.rotation.eulerAngles.y) * Mathf.Deg2Rad));

            _context.Player.CharacterController.Move(movement * Time.deltaTime * ClimbSpeed());

            if (_context.IsMoving)
                _context.Player.PlayerStamina.DecreaseStamina(_context.IsSprint ? 50f * Time.deltaTime : 20f * Time.deltaTime);
        }

        private void VerticalMovement()
        {
            movement = Vector3.zero;
            if (Physics.SphereCast(_context.transform.position + (Vector3.up * _context.Player.CharacterController.height), _context.radiusToDebug, _context.transform.forward, out RaycastHit topHit, 1.0f, _context.WallLayer))
            {
                // _context.transform.rotation = Quaternion.LookRotation(-topHit.normal, _context.transform.up); 
                MovePlayer(topHit);

                if (_context.MovementInput.x != 0)
                {
                    if (Physics.SphereCast(_context.transform.position + (Vector3.up * _context.Player.CharacterController.height), _context.radiusToDebug, _context.MovementInput.x < 0 ? -_context.transform.right : _context.transform.right, out RaycastHit rightHit, 1.0f, _context.WallLayer))
                    {
                        _context.transform.rotation = Quaternion.Euler(_context.transform.rotation.eulerAngles.x, Quaternion.LookRotation(-rightHit.normal, Vector3.up).eulerAngles.y, _context.transform.rotation.eulerAngles.z);
                    }
                }
                if (_context.MovementInput.y < 0f && _context.Player.CharacterController.isGrounded)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(topHit.normal, Vector3.up);
                    _context.transform.rotation = Quaternion.Euler(_context.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, _context.transform.rotation.eulerAngles.z);
                    //Debug.Log(Quaternion.Angle(lookRotation, Quaternion.Euler(0f, _context.m_Camera.transform.eulerAngles.y, 0f)));
                    if (Quaternion.Angle(lookRotation, Quaternion.Euler(0f, _context.m_Camera.transform.eulerAngles.y, 0f)) <= 60f)
                        _context.MovementInput = new Vector2(_context.MovementInput.x, -_context.MovementInput.y);
                    _context.IsClimb = false;
                    _context.IsGround = true;
                    return;
                }
            }
            else
            //if(!Physics.SphereCast(_context.transform.position + (Vector3.up * _context.Player.CharacterController.height), 0.2f, _context.transform.forward, out RaycastHit tophitted, 0.5f, _context.WallLayer))
            {
                if (Physics.Raycast(_context.transform.position + (Vector3.up * _context.Player.CharacterController.height), (_context.transform.forward - _context.transform.up).normalized, out RaycastHit hit, 3f, _context.WallLayer))
                {
                    //Debug.Log($"Hit Point:{hit.point}");
                    if (hit.normal.y >= 0.25f)
                    {
                        if (_context.MovementInput.y > 0)
                        {
                            _context.IsClimbToTop = true;
                            this.topHit = hit.point;
                        }
                        else
                        {
                            MovePlayer(hit);
                        }
                    }
                    else
                    {
                        MovePlayer(hit);
                    }
                }
                else
                {
                    _context.IsClimb = false;
                }
            }

        }


        private void MoveToTop()
        {
            delayFrameMoveToTop--;
            if (delayFrameMoveToTop != 0) return;
            _context.transform.position = topHit;
            Physics.SyncTransforms();

            if (enterMovementInput != Vector2.zero)
                _context.MovementInput = enterMovementInput;

            _context.IsClimb = false;
            _context.IsClimbToTop = false;

            /*if (_context.Player.CharacterController.collisionFlags == CollisionFlags.CollidedSides)
            {
                _context.PlayerVelocity = _context.transform.up;
                _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime * _context.ClimbSpeed);
            }
            else if (_context.Player.CharacterController.collisionFlags == CollisionFlags.CollidedAbove)
            {
                _context.IsClimb = false;
                _context.IsClimbToTop = false;
            }
            else
            {
                _context.PlayerVelocity = _context.transform.forward + new Vector3(0f, -0.1f, 0f);
                _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime * _context.ClimbSpeed);

                if (_context.IsGround)
                {
                    if (delayFrameMoveToTop > 0)
                    {
                        delayFrameMoveToTop--;
                        return;
                    }

                    _context.IsClimb = false;
                    _context.IsClimbToTop = false;
                    _context.IsGround = true;
                }
                else
                {
                    if (_context.Player.CharacterController.collisionFlags == CollisionFlags.None)
                    {
                        _context.IsClimb = false;
                        _context.IsClimbToTop = false;
                        _context.IsFall = true;
                    }
                }
            }*/
            /*if (Physics.SphereCast(new Ray(_context.transform.position, _context.transform.forward), 0.2f, 1f, _context.WallLayer))
            {
                _context.PlayerVelocity = _context.transform.up;
                _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime * _context.ClimbSpeed);

                if (Physics.SphereCast(new Ray(_context.transform.position, _context.transform.up), 0.2f, 2f, _context.WallLayer))
                    _context.IsClimbToTop = false;
            }
            else
            {
                _context.PlayerVelocity = _context.transform.forward + new Vector3(0f, -0.1f, 0f);
                _context.Player.CharacterController.Move(_context.PlayerVelocity * Time.deltaTime * _context.ClimbSpeed);

                if (_context.IsGround)
                {
                    if (delayFrameMoveToTop > 0)
                    {
                        delayFrameMoveToTop--;
                        return;
                    }

                    _context.IsClimb = false;
                    _context.IsClimbToTop = false;
                }
                else
                {
                    _context.IsClimb = false;
                    _context.IsClimbToTop = false;
                    _context.IsFall = true;
                }
            }*/
        }

    }
}