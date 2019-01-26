using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrack : StateMachineBehaviour {
	GameObject player;
	float trackSpeed;
	float attackRange;
	Vector3 leftsideAngle = new Vector3(0, 0, 0);
	Vector3 rightsideAngle = new Vector3(0, 180, 0);
    readonly float dirChangeTime = 0.5f;
    Transform animatorRoot;
    int maxFrame = 10;
    int frameCounter;
    Vector2 centerOfBody;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        trackSpeed = animator.GetComponent<Enemy>().trackSpeed;
        attackRange = animator.GetComponent<Enemy>().attackRange;
        player = EnemyManager.Instance.player;

        animatorRoot = animator.transform.parent;
        float halfHeight = animatorRoot.gameObject.GetComponent<BoxCollider2D>().size.y / 2.0f;
        Vector2 rootPosition2D = animatorRoot.position;
        centerOfBody = new Vector2(0, halfHeight) + rootPosition2D;

        frameCounter = 0;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        frameCounter += 1;
        if (Vector2.Distance(player.transform.position, centerOfBody) < attackRange)
		{
			animator.SetTrigger("AttackTrigger");
			return;
		}

        Vector2 currPosition = animatorRoot.position;
		Vector2 movingDistance = animatorRoot.right * trackSpeed * Time.deltaTime * -1;
        if(frameCounter >= maxFrame)
        {
            animatorRoot.eulerAngles = (player.transform.position.x - animatorRoot.position.x < 0) ? leftsideAngle : rightsideAngle;
            frameCounter = 0;
        }
        
        animatorRoot.gameObject.GetComponent<Rigidbody2D>().MovePosition(currPosition + movingDistance);
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
