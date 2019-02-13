using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeIdle : StateMachineBehaviour {
	float patrolRange;
	float patrolSpeed;
	float noticeRange;
    Vector2 origin;
    Transform animatorRoot;
    Enemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		origin = animator.transform.position;
        animatorRoot = animator.transform.parent;
        enemy = animator.GetComponent<Enemy>();

        patrolRange = enemy.patrolRange;
        noticeRange = enemy.noticeRange;
        patrolSpeed = enemy.patrolSpeed;

        enemy.ChangeDir(NumeratedDir.Left);
        enemy.ChangeVelocityX(enemy.MoveDir * patrolSpeed);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (enemy.PlayerDistance < noticeRange)
		{
			animator.SetTrigger("TrackTrigger");
			return;
		}
        float span = animatorRoot.position.x - origin.x;

        if ((Mathf.Abs(span) > patrolRange && span * enemy.MoveDir > 0) ||
            enemy.WallTest[(enemy.MoveDir + 1) / 2]                     ||
            enemy.CliffTest[(enemy.MoveDir + 1) / 2]
        )
		{
            enemy.ChangeDir(enemy.MoveDir * -1);
            enemy.ChangeVelocityX(enemy.MoveDir * patrolSpeed);
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
