namespace Outlander.Player
{
    public class PlayerAnimationStateData
    {
        PlayerAnimationStateMachine context;
        PlayerAnimationIdleState idleState;
        PlayerAnimationWalkState walkState;
        PlayerAnimationRunState runState;
        PlayerAnimationJumpedState jumpedState;
        PlayerAnimationGroundedState groundedState;
        PlayerAnimationFallState fallState;
        PlayerAnimationClimbState climbState;
        PlayerAnimationSwimState swimState;
        PlayerAnimationDodgeState dodgeState;
        PlayerAnimationCrouchState crouchState;

        public PlayerAnimationStateData(PlayerAnimationStateMachine currenntContext)
        {
            context = currenntContext;
            idleState = new PlayerAnimationIdleState(context, this);
            walkState = new PlayerAnimationWalkState(context, this);
            runState = new PlayerAnimationRunState(context, this);
            jumpedState = new PlayerAnimationJumpedState(context, this);
            groundedState = new PlayerAnimationGroundedState(context, this);
            fallState = new PlayerAnimationFallState(context, this);
            climbState = new PlayerAnimationClimbState(context, this);
            swimState = new PlayerAnimationSwimState(context, this);
            dodgeState = new PlayerAnimationDodgeState(context, this);
            crouchState = new PlayerAnimationCrouchState(context, this);
        }

        public PlayerAnimationBaseState SetState(PlayerMovementState _state)
        {
            context.Player.MovementStateMachine.SetWeaponVisible();
            switch (_state)
            {
                case PlayerMovementState.CLIMB:
                    return climbState;
                case PlayerMovementState.CROUCH:
                    return crouchState;
                case PlayerMovementState.DODGE:
                    return dodgeState;
                case PlayerMovementState.FALL:
                    return fallState;
                case PlayerMovementState.GROUNDED:
                    return groundedState;
                case PlayerMovementState.IDLE:
                    return idleState;
                case PlayerMovementState.JUMP:
                    return jumpedState;
                case PlayerMovementState.RUN:
                    return runState;
                case PlayerMovementState.SWIM:
                    return swimState;
                case PlayerMovementState.WALK:
                    return walkState;
                default:
                    return groundedState;
            }
        }

        //public PlayerAnimationBaseState Idle() => idleState;
        //public PlayerAnimationBaseState Walk() => walkState;
        //public PlayerAnimationBaseState Run() => runState;
        //public PlayerAnimationBaseState Jumped() => jumpedState;
        //public PlayerAnimationBaseState Grounded() => groundedState;
        //public PlayerAnimationBaseState Fall() => fallState;
        //public PlayerAnimationBaseState Climb() => climbState;
        //public PlayerAnimationBaseState Swim() => swimState;
        //public PlayerAnimationBaseState Dodge() => dodgeState;
        //public PlayerAnimationBaseState Crouch() => crouchState;
    }

}
