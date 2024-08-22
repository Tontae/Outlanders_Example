namespace Outlander.Player
{
    public class PlayerMovementStateData
    {
        private PlayerMovementStateMachine context;
        private PlayerIdleState idleState;
        private PlayerWalkState walkState;
        private PlayerRunState runState;
        private PlayerJumpedState jumpedState;
        private PlayerGroundedState groundedState;
        private PlayerFallState fallState;
        private PlayerClimbState climbState;
        private PlayerSwimState swimState;
        private PlayerDodgeState dodgeState;
        private PlayerCrouchState crouchState;

        public PlayerMovementStateData(PlayerMovementStateMachine currentContext)
        {
            context = currentContext;
            idleState = new PlayerIdleState(context, this);
            walkState = new PlayerWalkState(context, this);
            runState = new PlayerRunState(context, this);
            jumpedState = new PlayerJumpedState(context, this);
            groundedState = new PlayerGroundedState(context, this);
            fallState = new PlayerFallState(context, this);
            climbState = new PlayerClimbState(context, this);
            swimState = new PlayerSwimState(context, this);
            dodgeState = new PlayerDodgeState(context, this);
            crouchState = new PlayerCrouchState(context, this);
        }

        //public PlayerBaseState Idle() => new PlayerIdleState(context, this);
        //public PlayerBaseState Walk() => new PlayerWalkState(context, this);
        //public PlayerBaseState Run() => new PlayerRunState(context, this);
        //public PlayerBaseState Jumped() => new PlayerJumpedState(context, this);
        //public PlayerBaseState Grounded() => new PlayerGroundedState(context, this);
        //public PlayerBaseState Fall() => new PlayerFallState(context, this);
        //public PlayerBaseState Climb() => new PlayerClimbState(context, this);
        //public PlayerBaseState Swim() => new PlayerSwimState(context, this);
        //public PlayerBaseState Dodge() => new PlayerDodgeState(context, this);
        //public PlayerBaseState Crouch() => new PlayerCrouchState(context, this);

        public PlayerBaseState Idle() => idleState;
        public PlayerBaseState Walk() => walkState;
        public PlayerBaseState Run() => runState;
        public PlayerBaseState Jumped() => jumpedState;
        public PlayerBaseState Grounded() => groundedState;
        public PlayerBaseState Fall() => fallState;
        public PlayerBaseState Climb() => climbState;
        public PlayerBaseState Swim() => swimState;
        public PlayerBaseState Dodge() => dodgeState;
        public PlayerBaseState Crouch() => crouchState;

        //public PlayerBaseState Idle() => idleState.SetStateData(context, this);
        //public PlayerBaseState Walk() => walkState.SetStateData(context, this);
        //public PlayerBaseState Run() => runState.SetStateData(context, this);
        //public PlayerBaseState Jumped() => jumpedState.SetStateData(context, this);
        //public PlayerBaseState Grounded() => groundedState.SetStateData(context, this);
        //public PlayerBaseState Fall() => fallState.SetStateData(context, this);
        //public PlayerBaseState Climb() => climbState.SetStateData(context, this);
        //public PlayerBaseState Swim() => swimState.SetStateData(context, this);
        //public PlayerBaseState Dodge() => dodgeState.SetStateData(context, this);
        //public PlayerBaseState Crouch() => crouchState.SetStateData(context, this);
    }
}
