namespace Outlander.Player
{
    public abstract class PlayerBaseState
    {
        protected PlayerMovementStateMachine _context;
        protected PlayerMovementStateData _stateData;
        //protected PlayerBaseState _currentSuperState;
        protected PlayerBaseState _currentSubState;
        protected bool _isRootState = false;
        public PlayerBaseState(PlayerMovementStateMachine currentContext, PlayerMovementStateData playerMovementStateData)
        {
            _context = currentContext;
            _stateData = playerMovementStateData;
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
        protected void SwitchState(PlayerBaseState newState)
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
        /*protected void SetSuperState(PlayerBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }*/
        protected void SetSubState(PlayerBaseState newSubState)
        {
            _currentSubState = newSubState;
            //newSubState.SetSuperState(_context.CurrentState);
        }

        /*protected void InitSubState(PlayerBaseState initSubState, PlayerBaseState ownerSuperState)
        {
            _currentSubState = initSubState;
            //_context.CurrentState = ownerSuperState;
            //initSubState.SetSuperState(ownerSuperState);
        }*/
    }

}
