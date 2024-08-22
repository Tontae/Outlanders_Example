using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlander.Player;
using Mirror;

public class IdleBehaviour : StateMachineBehaviour
{
    private PlayerComponents Player { get => PlayerManagers.Instance.PlayerComponents; }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!NetworkServer.active)
        {
            Player.OutlanderStateMachine.CanReceiveFireInput = true;
            // Player.OutlanderStateMachine.OnSkill = false;
            Player.Animator.SetBool("NA1", false);
            Player.Animator.SetBool("NA2", false);
            // Player.MovementStateMachine.PlayerVelocity = Vector3.zero;
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!NetworkServer.active)
        {
            if (Player.OutlanderStateMachine.OnFireInput)
            {
                // Debug.Log("IdleOnStateUpdate");
                if (Player.PlayerStamina.IsMin)
                    return;
                if (Cursor.visible)
                    return;

                Player.Animator.SetBool("NA1", true);
                // Player.OutlanderStateMachine.PlayerStamina.DecreaseStamina(20);
                Player.OutlanderStateMachine.CanRecieveAttackInput();
                Player.OutlanderStateMachine.OnAction = true;
                // player.OnFireInput = false;
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!NetworkServer.active)
        {
            // Player.OutlanderStateMachine.OnAction = false;

            Player.OutlanderStateMachine.OnAttackState = false;
        }
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
