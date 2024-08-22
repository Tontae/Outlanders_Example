using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlander.Player
{
    public class PlayerStamina : PlayerElements, IPStamina
    {
        // public PlayerStamina()
        // {
        //     Stamina = MaxStamina;
        // }

        // public PlayerComponents Player { get; }


        [Header("Stamina")]
        [SerializeField] private float stamina;
        [SerializeField] private float minStamina;
        [SerializeField] private float maxStamina;
        [SerializeField] private float regenCooldown = 2f;
        [SerializeField, ReadOnlyInspector] private bool isMin;
        [SerializeField, ReadOnlyInspector] private bool isMax;
        [SerializeField, ReadOnlyInspector] private bool isRegen;

        // Interface Properties
        public bool IsMin
        {
            get => (Stamina <= MinStamina) ? true : false;
            // isMin = (Stamina <= MinStamina) ? true : false;
            // if (isMin) isRegen = true;
        }
        public bool IsMax
        {
            get => (Stamina >= MaxStamina) ? true : false;
        }
        public bool IsRegen { get => isRegen; set => isRegen = value; }

        public float MinStamina { get => minStamina; set => minStamina = value; }
        public float MaxStamina { get => maxStamina; set => maxStamina = value; }
        public float Stamina { get => stamina;
            set 
            {
                if (Player.PlayerMatchManager.myManager == null) return;
                if (!Player.PlayerMatchManager.myManager.canInteract) return;

                stamina = value;

                if (stamina <= 0f)
                    Player.MovementStateMachine.IsSprint = false;

                if (IsMax)
                    UIManagers.Instance.playerCanvas.staminaBarFrame.gameObject.SetActive(false);
                else
                    UIManagers.Instance.playerCanvas.staminaBarFrame.gameObject.SetActive(true);

                UIManagers.Instance.playerCanvas.staminaBar.rectTransform.sizeDelta = new Vector2(360f * stamina / MaxStamina, UIManagers.Instance.playerCanvas.staminaBar.rectTransform.sizeDelta.y);
            } 
        }
        public float RegenCooldown
        {
            get => regenCooldown;
            set => regenCooldown = value;
            // {
            //     if (!IsMax)
            //     {
            //         regenCooldown = (!IsRegen) ? 2 : value;
            //     }
            // }
        }


        private void Start()
        {
            if (!isLocalPlayer) return;
                InitializeStamina();
        }

        public void InitializeStamina()
        {
            IsRegen = false;

            // Stamina = MaxStamina;
            //IncreaseStamina(MaxStamina);
            stamina = MaxStamina;
        }

        private void Update()
        {
            if (IsMax 
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.CLIMB
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.JUMP
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.FALL
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.DODGE
                || Player.MovementStateMachine.MovementStateIndex == PlayerMovementState.SWIM
                || Player.MovementStateMachine.MovementSubStateIndex == PlayerMovementState.RUN
                || Player.OutlanderStateMachine.OnWeaponAction
                || Player.OutlanderStateMachine.OnFireInput
                || Player.OutlanderStateMachine.OnSkill
                || Player.OutlanderStateMachine.OnStun)
            {
                IsRegen = false;
                RegenCooldown = 2f;
                return;
            }

            IsRegen = true;
            RegenCooldown = Mathf.Clamp(RegenCooldown - Time.deltaTime, 0f, 2f);
            RegenStamina();
        }


        // Calculate Stamina
        public void DecreaseStamina(float _amount) => Stamina = Mathf.Max(Stamina - _amount, 0f);
        public void IncreaseStamina(float _amount) => Stamina = Mathf.Min(Stamina + _amount, MaxStamina);


        // Regen Stamina
        public void RegenStamina()
        {
            if (!IsRegen) return;
            // if (RegenCooldown > 0)
            //     RegenCooldown -= Time.deltaTime;

            var _amount = (Player.MovementStateMachine.IsMoving) ? ((RegenCooldown <= 0f) ? 15f : 10f) : ((RegenCooldown <= 0f) ? 20f : 10f);
            IncreaseStamina(_amount * Time.deltaTime);
        }
    }
}
