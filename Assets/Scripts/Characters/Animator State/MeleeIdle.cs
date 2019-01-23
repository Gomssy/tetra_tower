using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeIdle : StateMachineBehaviour {

	Vector2 origin;
	float patrolRange;
	float movementSpeed;
	float noticeRange;
	GameObject player;
	Vector3 leftsideAngle = new Vector3(0, 0, 0);
	Vector3 rightsideAngle = new Vector3(0, 180, 0);
	

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		origin = animator.transform.position;
		patrolRange = animator.GetFloat("patrolRange");
		noticeRange = animator.GetFloat("noticeRange");
		movementSpeed = animator.GetFloat("movementSpeedPatrol");
		player = GameObject.Find("Player");
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (Vector2.Distance(player.transform.position, animator.transform.position) < noticeRange)
		{
			animator.SetTrigger("TrackTrigger");
			return;
		}
		Vector2 currPosition = animator.transform.position;
		Vector2 movingDistance = -1 * animator.transform.right * movementSpeed * Time.deltaTime; // go left first
		animator.GetComponent<Rigidbody2D>().MovePosition(currPosition + movingDistance);
		if(Mathf.Abs(animator.transform.position.x - origin.x) > patrolRange)
		{
			animator.transform.eulerAngles = (origin.x < animator.transform.position.x) ? leftsideAngle : rightsideAngle;
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
