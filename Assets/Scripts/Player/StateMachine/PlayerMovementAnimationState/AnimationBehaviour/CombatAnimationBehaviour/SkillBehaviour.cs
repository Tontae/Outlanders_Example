using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlander.Player;
using Mirror;

public class SkillBehaviour : StateMachineBehaviour
{
    private PlayerComponents Player { get => PlayerManagers.Instance.PlayerComponents; }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     if (!NetworkServer.active)
    //     {
    //         Player.OutlanderStateMachine.OnSkill = false;
    //     }
    // }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {

    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!NetworkServer.active)
        {
            Player.OutlanderStateMachine.OnSkill = false;
            Player.OutlanderStateMachine.OnSkillMove = false;
            Player.OutlanderStateMachine.OnAttackState = false;
            Player.OutlanderStateMachine.CmdSetPlayerBoolAttribute(PlayerAttributeBoolType.ONSKILL, false);
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
