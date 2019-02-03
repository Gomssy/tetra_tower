using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaged : StateMachineBehaviour {

    RuntimeAnimatorController ac;
    float knockbackTime;
    float knockbackSpeed;
    float knockbackDir; // 1: right \ -1: left
    Vector3 leftsideAngle = new Vector3(0, 0, 0);
    Vector3 rightsideAngle = new Vector3(0, 180, 0);
    Transform pivotTransform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        ac = animator.runtimeAnimatorController;
        foreach(var clip in ac.animationClips)
        {
            if (clip.name.Contains("Damaged"))
            {
                knockbackTime = clip.length;
            }
        }
        knockbackSpeed = animator.GetFloat("knockbackDistance") / knockbackTime;

        Transform playerTransform = EnemyManager.Instance.player.transform;
        pivotTransform = animator.transform.parent;
        pivotTransform.eulerAngles = (playerTransform.position.x - pivotTransform.position.x < 0) ? leftsideAngle : rightsideAngle;
        knockbackDir = (playerTransform.position.x - pivotTransform.position.x < 0) ? 1 : -1;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Vector2 currPosition = pivotTransform.position;
        Vector2 movingDistance = new Vector2(knockbackSpeed * Time.deltaTime, 0) * knockbackDir;
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
