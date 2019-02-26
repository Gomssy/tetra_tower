using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeTrack : StateMachineBehaviour {
    float trackSpeed;
    float attackRange;
    GameObject player;
    Transform animatorRoot;
    EnemyGround enemy;

    readonly int maxFrame = 10;
    int frameCounter = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animatorRoot = animator.transform.parent;
        enemy = animator.GetComponent<EnemyGround>();
        player = GameManager.Instance.player;

        trackSpeed = enemy.trackSpeed;
        attackRange = enemy.attackRange;

        NumeratedDir trackDir = (animatorRoot.position.x - player.transform.position.x > 0) ? NumeratedDir.Left : NumeratedDir.Right;
        enemy.ChangeDir_noOption(trackDir);
        if (enemy.CliffTest[(enemy.MoveDir + 1) / 2] || animator.GetComponent<Enemy>().PlayerDistance < attackRange)
        {
            enemy.ChangeVelocityX_noOption(0.0f);
        }
        else
        {
            enemy.ChangeVelocityX_noOption(enemy.MoveDir * trackSpeed);
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.GetComponent<Enemy>().PlayerDistance < attackRange)
        {
            animator.SetTrigger("AttackTrigger");
            return;
        }
        int integerDir = enemy.MoveDir;
        if (enemy.WallTest[(integerDir + 1) / 2] || enemy.CliffTest[(integerDir + 1) / 2])
        {
            enemy.ChangeVelocityX_noOption(0.0f);
        }
        else
        {
            enemy.ChangeVelocityX_noOption(enemy.MoveDir * trackSpeed);
        }

        frameCounter += 1;
        if (frameCounter >= maxFrame)
        {
            NumeratedDir trackDir = (animatorRoot.position.x - player.transform.position.x > 0) ? NumeratedDir.Left : NumeratedDir.Right;
            enemy.ChangeDir_noOption(trackDir);
            frameCounter = 0;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
