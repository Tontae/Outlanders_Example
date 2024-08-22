namespace Outlander.Player
{
    public class BotMovementStateData
    {
        private BotPSM context;
        private BotIdleState idleState;
        private BotWalkState walkState;
        private BotRunState runState;
        private BotGroundedState groundedState;

        public BotMovementStateData(BotPSM currentContext)
        {
            context = currentContext;
            idleState = new BotIdleState(context, this);
            walkState = new BotWalkState(context, this);
            runState = new BotRunState(context, this);
            groundedState = new BotGroundedState(context, this);
        }

        public BotBaseState Idle() => idleState;
        public BotBaseState Walk() => walkState;
        public BotBaseState Run() => runState;
        public BotBaseState Grounded() => groundedState;
    }
}
