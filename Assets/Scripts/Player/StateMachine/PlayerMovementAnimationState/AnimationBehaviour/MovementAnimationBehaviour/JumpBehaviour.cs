using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlander.Player;
public class JumpBehaviour : StateMachineBehaviour
{
    protected PlayerMovementStateMachine playerMovement;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.gameObject.TryGetComponent<PlayerMovementStateMachine>(out playerMovement);
        // if (playerMovement.Player.PlayerStamina.IsMin)
        //     playerMovement.Player.PlayerStamina.DecreaseStamina(20);
        // Debug.Log(animator.gameObject.name);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // playerMovement.IsFall = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if (playerMovement.IsGround)
        //playerMovement.IsJump = false;
        // playerMovement.ResetJump();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
