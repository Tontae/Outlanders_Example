namespace Outlander.Player
{
    public abstract class BotBaseState
    {
        protected BotPSM _context;
        protected BotMovementStateData _stateData;
        //protected PlayerBaseState _currentSuperState;
        protected BotBaseState _currentSubState;
        protected bool _isRootState = false;
        public BotBaseState(BotPSM currentContext, BotMovementStateData botMovementStateData)
        {
            _context = currentContext;
            _stateData = botMovementStateData;
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
        protected void SwitchState(BotBaseState newState)
        {
            ExitState();

            if (newState._isRootState)
            {
                newState.SetSubState(_context.CurrentState._currentSubState);
                _context.CurrentState = newState;
                //_currentSuperState = newState;
            }
            else
            {
                _context.CurrentState.SetSubState(newState);
            }

            newState.EnterState();

        }
        /*protected void SetSuperState(PlayerBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }*/
        protected void SetSubState(BotBaseState newSubState)
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
