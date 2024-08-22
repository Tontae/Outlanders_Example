namespace Outlander.Player
{
    public abstract class PlayerAnimationBaseState
    {
        protected PlayerAnimationStateMachine _context;
        protected PlayerAnimationStateData _stateData;
        //protected PlayerAnimationBaseState _currentSuperState;
        protected PlayerAnimationBaseState _currentSubState;
        protected bool _isRootState = false;

        public PlayerAnimationBaseState(PlayerAnimationStateMachine currentContext, PlayerAnimationStateData animationStateData)
        {
            _context = currentContext;
            _stateData = animationStateData;
        }

        public abstract void EnterState();
        public abstract void UpdatePlayerState();
        public abstract void ExitPlayerState();
        public abstract void CheckSwitchState();
        public abstract void InitializeSubState();
        public void UpdateState()
        {
            UpdatePlayerState();
            if (_currentSubState != null)
            {
                _currentSubState.UpdatePlayerState();
            }
        }

        public void ExitState()
        {
            ExitPlayerState();
            /*if (_currentSubState != null)
            {
                _currentSubState.ExitPlayerState();
            }*/
        }

        protected void SwitchState(PlayerAnimationBaseState newState)
        {
            ExitState();

            if (newState._isRootState)
            {
                if (newState._currentSubState != null)
                {
                    if (_context.CurrentState._currentSubState != null)
                    {
                        newState.SetSubState(_context.CurrentState._currentSubState);
                    }
                }
                _context.CurrentState = newState;
                //_currentSuperState = newState;
            }
            else
            {
                if (_context.CurrentState._currentSubState != null)
                {
                    _context.CurrentState.SetSubState(newState);
                }
            }

            newState.EnterState();
            newState._currentSubState?.EnterState();
        }

        /*protected void SetSuperState(PlayerAnimationBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }*/

        protected void SetSubState(PlayerAnimationBaseState newSubState)
        {
            _currentSubState = newSubState;
            //newSubState.SetSuperState(this);
        }
    }

}
