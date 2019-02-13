using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeTrack : StateMachineBehaviour {
	GameObject player;
	float trackSpeed;
	float attackRange;
	Vector3 leftsideAngle = new Vector3(0, 0, 0);
	Vector3 rightsideAngle = new Vector3(0, 180, 0);
    Transform pivotTransform;
    readonly int maxFrame = 10;
    int frameCounter;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        trackSpeed = animator.GetComponent<Enemy>().trackSpeed;
        attackRange = animator.GetComponent<Enemy>().attackRange;
        player = EnemyManager.Instance.Player;

        pivotTransform = animator.transform.parent;
        float halfHeight = pivotTransform.gameObject.GetComponent<BoxCollider2D>().size.y / 2.0f;
        Vector2 rootPosition2D = pivotTransform.position;
        frameCounter = 0;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.GetComponent<Enemy>().PlayerDistance < attackRange)
        {
            animator.SetTrigger("AttackTrigger");
            return;
        }
        frameCounter += 1;
        if (frameCounter >= maxFrame)
        {
            pivotTransform.eulerAngles = (player.transform.position.x - pivotTransform.position.x < 0) ? leftsideAngle : rightsideAngle;
            frameCounter = 0;
        }

        Vector2 currPosition = pivotTransform.position;
		Vector2 movingDistance = pivotTransform.right * trackSpeed * Time.deltaTime * -1;
        pivotTransform.gameObject.GetComponent<Rigidbody2D>().MovePosition(currPosition + movingDistance);

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
