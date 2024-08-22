using Outlander.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Outlander.Manager
{
    public class PlayerInputManager : PlayerElements
    {
        private PlayerInputAction playerInput;

        public PlayerInputAction PlayerInput { get => playerInput; set => playerInput = value; }

        private void Start()
        {
            if (!isLocalPlayer) return;
            PlayerInput = new PlayerInputAction();

            InitCombatInput();
            InitMovementInput();
            InitUtilsInput();

            PlayerInput.Enable();
        }

        // private void Update()
        // {
        //     if (playerInput.Player.Jump.triggered) OnJump();
        // }

        private void OnEnable()
        {
            if (!isLocalPlayer) return;
            PlayerInput.Enable();
        }

        private void OnDisable()
        {
            if (!isLocalPlayer) return;
            PlayerInput.Disable();
        }

        private void InitCombatInput()
        {
            PlayerInput.Player.Fire.started += OnFire;
            PlayerInput.Player.Fire.performed += OnFire;
            PlayerInput.Player.Fire.canceled += OnFire;
            PlayerInput.Player.LeftItem.started += OnLeftItem;
            PlayerInput.Player.LeftItem.performed += OnLeftItem;
            PlayerInput.Player.LeftItem.canceled += OnLeftItem;
            PlayerInput.Player.RightItem.started += OnRightItem;
            PlayerInput.Player.RightItem.performed += OnRightItem;
            PlayerInput.Player.RightItem.canceled += OnRightItem;
            PlayerInput.Player.Skill_1.started += OnSkill_1;
            //PlayerInput.Player.Skill_1.performed += OnSkill_1;
            //PlayerInput.Player.Skill_1.canceled += OnSkill_1;
            PlayerInput.Player.Skill_2.started += OnSkill_2;
            //PlayerInput.Player.Skill_2.performed += OnSkill_2;
            //PlayerInput.Player.Skill_2.canceled += OnSkill_2;
            PlayerInput.Player.Skill_3.started += OnSkill_3;
            //PlayerInput.Player.Skill_3.performed += OnSkill_3;
            //PlayerInput.Player.Skill_3.canceled += OnSkill_3;
            PlayerInput.Player.Skill_4.started += OnSkill_4;
            //PlayerInput.Player.Skill_4.performed += OnSkill_4;
            //PlayerInput.Player.Skill_4.canceled += OnSkill_4;
            PlayerInput.Player.SwapWeapon.started += OnSwapWeapon;
            //PlayerInput.Player.SwapWeapon.performed += OnSwapWeapon;
            //PlayerInput.Player.SwapWeapon.canceled += OnSwapWeapon;
            PlayerInput.Player.WeaponAction.started += OnWeaponAction;
            PlayerInput.Player.WeaponAction.performed += OnWeaponAction;
            PlayerInput.Player.WeaponAction.canceled += OnWeaponAction;
        }

        private void InitMovementInput()
        {
            PlayerInput.Player.Crouch.started += OnCrouch;
            PlayerInput.Player.Crouch.performed += OnCrouch;
            PlayerInput.Player.Crouch.canceled += OnCrouch;
            // playerInput.Player.Dodge.started += OnDodge;
            PlayerInput.Player.Dodge.performed += OnDodge;
            // playerInput.Player.Dodge.canceled += OnDodge;
            // playerInput.Player.Jump.started += OnJump;
            PlayerInput.Player.Jump.performed += OnJump;
            // playerInput.Player.Jump.canceled += OnJump;
            PlayerInput.Player.Look.started += OnLook;
            PlayerInput.Player.Look.performed += OnLook;
            PlayerInput.Player.Look.canceled += OnLook;
            PlayerInput.Player.Movement.started += OnMovement;
            PlayerInput.Player.Movement.performed += OnMovement;
            PlayerInput.Player.Movement.canceled += OnMovement;
            PlayerInput.Player.Sprint.started += OnSprint;
            PlayerInput.Player.Sprint.performed += OnSprint;
            PlayerInput.Player.Sprint.canceled += OnSprint;
            // playerInput.Player.Zoom.started += OnZoom;
            // playerInput.Player.Zoom.performed += OnZoom;
            // playerInput.Player.Zoom.canceled += OnZoom;
        }

        private void InitUtilsInput()
        {
            PlayerInput.Camera.IncreaseSensitivity.started += OnIncreaseCameraSensitivity;
            PlayerInput.Camera.DecreaseSensitivity.started += OnDecreaseCameraSensitivity;

            PlayerInput.Player.EnableMouse.started += OnEnableMouse;
            PlayerInput.Player.EnableMouse.performed += OnEnableMouse;
            PlayerInput.Player.EnableMouse.canceled += OnEnableMouse;
            // playerInput.Player.Escape.started += OnEscape;
            // playerInput.Player.Escape.performed += OnEscape;
            // playerInput.Player.Escape.canceled += OnEscape;
            // playerInput.Player.Help.started += OnHelp;
            // playerInput.Player.Help.performed += OnHelp;
            // playerInput.Player.Help.canceled += OnHelp;
            PlayerInput.Player.Interaction.started += OnInteraction;
            //PlayerInput.Player.Interaction.performed += OnInteraction;
            PlayerInput.Player.Interaction.canceled += OnInteraction;
            PlayerInput.Player.OpenInventory.started += OnOpenInventory;
            PlayerInput.Player.OpenInventory.performed += OnOpenInventory;
            PlayerInput.Player.OpenInventory.canceled += OnOpenInventory;
            // playerInput.Player.OpenChat.started += OnOpenChat;
            // playerInput.Player.OpenChat.performed += OnOpenChat;
            // playerInput.Player.OpenChat.canceled += OnOpenChat;
            PlayerInput.Player.OpenMap.started += OnOpenMap;
            PlayerInput.Player.OpenMap.performed += OnOpenMap;
            PlayerInput.Player.OpenMap.canceled += OnOpenMap;
            // playerInput.Player.OpenOption.started += OnOpenOption;
            // playerInput.Player.OpenOption.performed += OnOpenOption;
            // playerInput.Player.OpenOption.canceled += OnOpenOption;
            PlayerInput.Player.Unstuck.started += OnUnstuck;
            PlayerInput.Player.Unstuck.performed += OnUnstuck;
            PlayerInput.Player.Unstuck.canceled += OnUnstuck;
        }


        // For Send Message Behaviour
        #region Movement For Send Message Behaviour
        // public void OnMovement(InputValue value)
        // {
        //     playerMovement.MoveInput(value.Get<Vector2>());
        // }

        // private void OnLook(InputValue value)
        // {
        //     playerMovement.LookInput(value.Get<Vector2>());
        // }

        // public void OnJump(InputValue value)
        // {
        //     playerMovement.JumpInput(value.isPressed);
        // }

        // private void OnSprint(InputValue value)
        // {
        //     playerMovement.SprintInput(value.isPressed);
        // }

        // private void OnCrouch(InputValue value)
        // {
        //     playerMovement.CrouchInput();
        // }

        // public void OnDodge(InputValue value)
        // {
        //     playerMovement.DodgeInput();
        // }
        // #endregion


        // #region Combat
        // public void OnFire(InputValue value)
        // {
        //     playerOutlander.FireInput(value.isPressed);
        // }

        // public void OnSkill_1(InputValue value)
        // {
        //     playerOutlander.SkillInput(1, value.isPressed);
        // }

        // public void OnSkill_2(InputValue value)
        // {
        //     playerOutlander.SkillInput(2, value.isPressed);
        // }

        // public void OnSkill_3(InputValue value)
        // {
        //     playerOutlander.SkillInput(3, value.isPressed);
        // }

        // public void OnSkill_4(InputValue value)
        // {
        //     playerOutlander.SkillInput(4, value.isPressed);
        // }

        // public void OnWeaponAction(InputValue value)
        // {
        //     playerOutlander.WeaponActionInput(value.isPressed);
        // }

        // public void OnLeftItem(InputValue value)
        // {
        //     playerOutlander.LeftItemInput(value.isPressed);
        // }

        // public void OnRightItem(InputValue value)
        // {
        //     playerOutlander.RightItemInput(value.isPressed);
        // }
        // #endregion


        // #region Utilities
        // public void OnEnableMouse(InputValue value)
        // {
        //     CursorManager.instance.alt_interact = !CursorManager.instance.alt_interact;
        // }

        // public void OnHelp(InputValue value)
        // {
        //     playerUIManager.HelpInput();
        // }

        // public void OnOpenOption(InputValue value)
        // {
        //     optionManager.SystemOptionInput();
        // }

        // public void OnOpenMap(InputValue value)
        // {
        //     playerUIManager.MapInput();
        // }

        // public void OnInteraction(InputValue value)
        // {
        //     playerOutlander.InteractInput(value.isPressed);
        // }
        // #endregion


        // #region Inventory
        // public void OnOpenInventory(InputValue value)
        // {
        //     Player.InventoryManager.InventoryInput();
        // }

        // public void OnSwapWeapon(InputValue value)
        // {
        //     Player.InventoryManager.SwapWeaponInput();
        // }
        #endregion


        #region SetActive for InputAction
        public void SetInputEnable() => PlayerInput.Enable();
        public void SetInputDisable() => PlayerInput.Disable();
        #endregion


        #region PlayerInputAction.IPlayer Movement
        public void OnMovement(InputAction.CallbackContext context)
        {
            Player.MovementStateMachine.MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Player.MovementStateMachine.LookInput(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            Player.MovementStateMachine.JumpInput(context.performed);
        }
        // public void OnJump()
        // {
        //     Player.MovementStateMachine.JumpInput();
        // }

        public void OnSprint(InputAction.CallbackContext context)
        {
            Player.MovementStateMachine.SprintInput(context.performed);
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            Player.MovementStateMachine.CrouchInput();
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            Player.MovementStateMachine.DodgeInput(context.performed);
        }
        #endregion


        #region PlayerInputAction.IPlayer Combat        
        public void OnFire(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.FireInput(context.performed);
        }

        public void OnSkill_1(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.SkillInput(0, context.performed);
        }

        public void OnSkill_2(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.SkillInput(1, context.performed);
        }

        public void OnSkill_3(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.SkillInput(2, context.performed);
        }

        public void OnSkill_4(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.SkillInput(3, context.performed);
        }

        public void OnWeaponAction(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.WeaponActionInput(context.performed);
        }

        public void OnLeftItem(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.LeftItemInput(context.performed);
        }

        public void OnRightItem(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.RightItemInput(context.performed);
        }
        #endregion


        #region PlayerInputAction.IPlayer Inventory
        public void OnSwapWeapon(InputAction.CallbackContext context)
        {
            Player.PlayerInventoryController.SwapWeaponInput();
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            Player.PlayerInventoryController.InventoryInput();
        }

        #endregion


        #region PlayerInputAction.IPlayer Utilities
        public void OnEnableMouse(InputAction.CallbackContext context)
        {
            CursorManager.Instance.alt_interact = !CursorManager.Instance.alt_interact;
        }

        // public void OnOpenChat(InputAction.CallbackContext context)
        // {

        // }

        public void OnOpenMap(InputAction.CallbackContext context)
        {
            Player.PlayerUIManager.MapInput();
        }

        // public void OnEscape(InputAction.CallbackContext context)
        // {

        // }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            Player.OutlanderStateMachine.InteractInput(context.started);
        }

        public void OnUnstuck(InputAction.CallbackContext context)
        {
            Player.PlayerUIManager.UnstuckInput();
        }

        public void OnDecreaseCameraSensitivity(InputAction.CallbackContext context)
        {
            Player.PlayerCamera.GetDecreaseSensitivity();
        }

        public void OnIncreaseCameraSensitivity(InputAction.CallbackContext context)
        {
            Player.PlayerCamera.GetIncreaseSensitivity();
        }
        #endregion
    }
}

