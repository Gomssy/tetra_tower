using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrack : StateMachineBehaviour {
	GameObject player;
	float movementSpeed;
	float attackRange;
	Vector3 leftsideAngle = new Vector3(0, 0, 0);
	Vector3 rightsideAngle = new Vector3(0, 180, 0);
    readonly float dirChangeTime = 0.5f;

    IEnumerator dirChange;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		movementSpeed = animator.GetFloat("movementSpeedTrack");
		attackRange = animator.GetFloat("attackRange");
		player = GameObject.Find("Player");
        dirChange = DirChange(animator);
        animator.GetComponent<MonoBehaviour>().StartCoroutine(dirChange);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (Vector2.Distance(player.transform.position, animator.transform.position) < attackRange)
		{
			animator.SetTrigger("AttackTrigger");
			return;
		}
        Vector2 currPosition = animator.transform.position;
		Vector2 movingDistance = animator.transform.right * movementSpeed * Time.deltaTime * -1;
		animator.GetComponent<Rigidbody2D>().MovePosition(currPosition + movingDistance);
	}

    IEnumerator DirChange(Animator animator)
    {
        while (true)
        {
            animator.transform.eulerAngles = (player.transform.position.x - animator.transform.position.x < 0) ? leftsideAngle : rightsideAngle;
            yield return new WaitForSeconds(dirChangeTime);
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<MonoBehaviour>().StopCoroutine(dirChange);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
