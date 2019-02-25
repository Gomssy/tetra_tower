using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirTrack : StateMachineBehaviour {
    float trackSpeed;
    float trackRange;
    float angle;
    GameObject player;
    Transform animatorRoot;
    EnemyAir enemy;
    Vector2 direction;

    int maxFrame = 10;
    int frameCount;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animatorRoot = animator.transform.parent;
        enemy = animator.GetComponent<EnemyAir>();
        player = EnemyManager.Instance.Player;
        trackSpeed = enemy.trackSpeed;
        frameCount = 0;

        SetDirection();
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (enemy.PlayerDistance > enemy.noticeRange)
        {
            animator.ResetTrigger("TrackTrigger");
            animator.SetTrigger("IdleTrigger");
            enemy.ChangeVelocityXY_noOption(Vector2.zero);
            return;
        }

        SetDirection();

        Vector2 vel = direction.normalized * trackSpeed;
        enemy.ChangeVelocityXY_noOption(vel);
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

    private void SetDirection()
    {
        direction = player.transform.position - animatorRoot.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemy.ChangeAngleZ_noOption(angle - 90.0f);
    }
}
