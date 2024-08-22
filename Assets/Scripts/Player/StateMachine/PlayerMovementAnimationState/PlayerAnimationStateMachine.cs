using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Outlander.Player
{
    public class PlayerAnimationStateMachine : PlayerElements
    {
        [Header("Reference")]
        [SerializeField] private Animator anim;
        [Space(10), SerializeField, ReadOnlyInspector] private string playerAnimState;

        // Rig
        private Rig rig;
        private RigLayer axeRig;
        private RigLayer lanceRig;

        //target
        public Transform target;
        public Transform hint;

        // Anim State Machine
        private PlayerAnimationBaseState currentState;
        private PlayerAnimationStateData state;
        private PlayerMovementState animStateIndex = PlayerMovementState.GROUNDED;
        private PlayerMovementState animSubStateIndex = PlayerMovementState.IDLE;

        // public properties
        public PlayerAnimationBaseState CurrentState { get => currentState; set => currentState = value; }
        public PlayerAnimationStateData State { get => state; set => state = value; }
        public PlayerMovementState AnimStateIndex { get => animStateIndex; set => animStateIndex = value; }
        public PlayerMovementState AnimSubStateIndex { get => animSubStateIndex; set => animSubStateIndex = value; }
        public string PlayerAnimState { get => playerAnimState; set => playerAnimState = value; }
        public float RigWeight { get => rig ? rig.weight : 0f; }

        private void Awake()
        {

            // playerMovement = GetComponent<PlayerMovementStateMachine>();
            // playerOutlander = GetComponent<Outlander.Player.PlayerOutlander>();

            // CurrentState = State.Grounded();
            // CurrentState.EnterState();
        }

        private void Start()
        {
            if (!isLocalPlayer) return;
            State = new PlayerAnimationStateData(this);
            CurrentState = State.SetState(PlayerMovementState.GROUNDED);
            CurrentState.EnterState();
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            OnGround();

            currentState.UpdateState();
        }

        private void OnGround()
        {
            if (Player.MovementStateMachine.IsGround)
            {
                SetAnimationParameter("OnGround", true);
                SetAnimationParameter("moveX", 0f);
                SetAnimationParameter("moveZ", Mathf.Clamp(Mathf.Abs(Player.MovementStateMachine.MovementInput.y) + Mathf.Abs(Player.MovementStateMachine.MovementInput.x), 0f, 1f));
            }
            else
            {
                SetAnimationParameter("OnGround", false);
                SetAnimationParameter("moveX", Player.MovementStateMachine.MovementInput.x);
                SetAnimationParameter("moveZ", Player.MovementStateMachine.MovementInput.y);
            }
        }

        public void SetAnimationCrossfade(string _anim, float _args)
        {
            Player.Animator.CrossFade(_anim, _args);
        }

        // public void SetAnimationBoolParameter(string _anim, bool _args)
        // {
        //     Player.Animator.SetBool(_anim, _args);
        // }

        // public void SetAnimationFloatParameter(string _anim, float _args)
        // {
        //     Player.Animator.SetFloat(_anim, _args);
        // }

        public void SetAnimationPlay(string _anim)
        {
            Player.Animator.SetTrigger(_anim);
        }

        public void SetAnimationParameter<T>(string _anim, T _args)
        {
            switch (typeof(T))
            {
                case var param when param == typeof(bool):
                    Player.Animator.SetBool(_anim, System.Convert.ToBoolean(_args));
                    break;
                case var param when param == typeof(float):
                    Player.Animator.SetFloat(_anim, System.Convert.ToSingle(_args));
                    break;
                case var param when param == typeof(int):
                    Player.Animator.SetInteger(_anim, System.Convert.ToInt32(_args));
                    break;
                default:
                    break;
            }
        }

        public void SetAnimationSpeed(float _speed)
        {
            Player.Animator.speed = _speed;
        }

        [Command]
        private void CmdSetRigWeight(float weight) => RpcSetRigWeight(weight);

        [ClientRpc]
        public void RpcSetRigWeight(float weight)
        {
            if (!rig) return;
            if (isLocalPlayer) return;
            StartCoroutine(SetWeight(weight));
        }

        private IEnumerator SetWeight(float _weight)
        {
            yield return new WaitForEndOfFrame();
            rig.weight = _weight;
        }

        public void SetRigWeight(float _weight)
        {
            if (!rig) return;
            if (rig.weight == _weight) return;
            CmdSetRigWeight(_weight);
            StartCoroutine(SetWeight(_weight));
        }

        public void GetWeaponType()
        {
            if(isClient)
            {
                if (!isLocalPlayer)
                {
                    bool isClientWalk = Player.Animator.GetBool("moving");
                    bool isClientSwim = Player.Animator.GetBool("swimming");
                    bool isClientSwimMove = Player.Animator.GetBool("swimMove");
                    bool isClientClimb = Player.Animator.GetBool("climbing");
                    bool isClientCrouch = Player.Animator.GetBool("onCrouch");

                    Player.Animator.runtimeAnimatorController = Player.WeaponManager.WeaponAnimatorList.Find(x => x.weaponType == Player.WeaponManager.currentWeaponType).animator;

                    Player.Animator.SetBool("moving", isClientWalk);
                    Player.Animator.SetBool("swimming", isClientSwim);
                    Player.Animator.SetBool("swimMove", isClientSwimMove);
                    Player.Animator.SetBool("climbing", isClientClimb);
                    Player.Animator.SetBool("onCrouch", isClientCrouch);
                }
                if (lanceRig == null || axeRig == null)
                {
                    axeRig = Player.RigBuilder.layers[0];
                    lanceRig = Player.RigBuilder.layers[1];
                }
                switch (Player.WeaponManager.currentWeaponType)
                {
                    case WeaponManager.WeaponType.Lance:
                        axeRig.active = false;
                        lanceRig.active = true;
                        rig = lanceRig.rig;
                        target.localPosition = new Vector3(0.109f, 0.182f, -0.578f);
                        target.localEulerAngles = new Vector3(354.3f, 326.27f, 306.8f);
                        hint.localPosition = new Vector3(-0.05f, 0.027f, -0.741f);
                        break;
                    case WeaponManager.WeaponType.Axe:
                        axeRig.active = true;
                        lanceRig.active = false;
                        rig = axeRig.rig;
                        target.localPosition = new Vector3(-0.249f, 0.063f, 0.482f);
                        target.localEulerAngles = new Vector3(332.356f, 307.995f, 44.826f);
                        hint.localPosition = new Vector3(-0.219f, -0.101f, 0.683f);
                        break;
                    default:
                        lanceRig.active = false;
                        axeRig.active = false;
                        break;
                }
                if (Player.AnimationStateMachine.AnimStateIndex == PlayerMovementState.CLIMB
                    || Player.AnimationStateMachine.AnimStateIndex == PlayerMovementState.CROUCH
                    || Player.AnimationStateMachine.AnimStateIndex == PlayerMovementState.DODGE
                    || Player.AnimationStateMachine.AnimStateIndex == PlayerMovementState.SWIM)
                {
                    SetRigWeight(0);
                }
            }
        }
    }
}
