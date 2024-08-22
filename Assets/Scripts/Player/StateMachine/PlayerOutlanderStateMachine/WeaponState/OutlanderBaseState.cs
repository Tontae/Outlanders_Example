namespace Outlander.Player
{
    public class OutlanderBaseState : IPOutState
    {
        // protected virtual bool OnSkill
        // {
        //     get => _context.OnSkill;
        //     set => Skill();
        // }

        protected PlayerComponents Player { get => _context.Player; }

        protected PlayerOutlanderStateMachine _context;
        protected OutlanderStateData _StateData;
        private OutlanderBaseState _currentState;

        public OutlanderBaseState(PlayerOutlanderStateMachine currentContext, OutlanderStateData outlanderStateData)
        {
            _context = currentContext;
            _StateData = outlanderStateData;
        }

        public OutlanderBaseState SetStateData(OutlanderStateData outlanderStateData)
        {
            _StateData = outlanderStateData;
            return this;
        }

        public virtual void CheckSwitchState()
        {
        }

        public virtual void EnterState()
        {
        }

        public virtual void ExitPlayerState()
        {
        }

        public virtual void UpdatePlayerState()
        {
        }

        public virtual void SwitchingCurrentWeaponState()
        {
            if (_context.OnDie) return;
            switch (Player.WeaponManager.currentWeaponType)
            {
                case WeaponManager.WeaponType.Axe:
                    SwitchState(_StateData.Axe());
                    break;
                case WeaponManager.WeaponType.BowQuiver:
                    SwitchState(_StateData.Bow());
                    break;
                case WeaponManager.WeaponType.Lance:
                    SwitchState(_StateData.Lance());
                    break;
                case WeaponManager.WeaponType.Sword:
                    SwitchState(_StateData.Sword());
                    break;
                case WeaponManager.WeaponType.None:
                    SwitchState(_StateData.Hand());
                    break;
                case WeaponManager.WeaponType.Die:
                    SwitchState(_StateData.Die());
                    break;
                default:
                    SwitchState(_StateData.Hand());
                    break;
            }
        }

        public void UpdateState()
        {
            UpdatePlayerState();
            if (_currentState != null)
            {
                _currentState.UpdatePlayerState();
            }
        }
        public void ExitState()
        {
            ExitPlayerState();
            if (_currentState != null)
            {
                _currentState.ExitPlayerState();
            }
        }
        protected void SwitchState(OutlanderBaseState newState)
        {
            ExitPlayerState();
            _context.CurrentState = newState;
            newState.EnterState();
        }

        // protected virtual void Skill()
        // {
        //     if (!_context.PlayerSkill.ContainsKey(_context.OnSkillIndex))
        //         return;
        //     if (_context.PlayerSkill[_context.OnSkillIndex] == null)
        //         return;
        //     if (_context.OnWeaponAction)
        //         return;

        //     if (_context.PlayerMP < _context.PlayerSkill[_context.OnSkillIndex].manaUsage)
        //     {
        //         ClientTriggerEventManager.Instance.ManaEmpty();
        //         return;
        //     }

        //     if (Player.PlayerSkillManager.UseSkill(_context.PlayerSkill[_context.OnSkillIndex]))
        //     {
        //         _context.CalculatePlayerAttributeValue(PlayerAttributeFloatType.MP, -_context.PlayerSkill[_context.OnSkillIndex].manaUsage);
        //         _context.OnSkill = true;
        //         Player.Animator.CrossFade(_context.PlayerSkill[_context.OnSkillIndex].skillAnimName, 0.1f);
        //         if (UIManagers.Instance.playerCanvas.uiInventory.inventoryPanel.activeInHierarchy)
        //             Player.PlayerStatisticManager.UpdateHpAndMp(_context.PlayerHP, _context.PlayerMP);
        //     }
        // }
        
        //public void ConvertAnimParameter()
        //{
        //    bool isPlayerSwimming = _context.Player.Animator.GetBool("swimming");
        //    bool isPlayerSwimMove = _context.Player.Animator.GetBool("swimMove");

        //    Player.Animator.SetBool("swimming", isPlayerSwim);
        //}
    }

}
