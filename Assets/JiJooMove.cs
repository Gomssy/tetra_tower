using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiJooMove : StateMachineBehaviour {
    float horizontalSpeed;
    float verticalSpeed;
    GameObject player;
    Transform animatorRoot;
    JiJoo enemy;
    Vector2Int dir;
    Vector2Int destination;

    float time;
    float timer = 0;

     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animatorRoot = animator.transform.parent;
        enemy = animator.GetComponent<JiJoo>();
        player = GameManager.Instance.player;

        horizontalSpeed = enemy.horizontalSpeed;
        verticalSpeed = enemy.verticalSpeed;

        dir = enemy.MoveDirection();
        enemy.transform.eulerAngles = new Vector3(0, 0, JiJoo.Vector2ToZAngle(dir));
        enemy.transform.parent.GetComponent<Rigidbody2D>().velocity = dir * new Vector2(horizontalSpeed, verticalSpeed);

        destination = enemy.gridPosition + dir;

        Vector2 realVector = JiJoo.RealPosition(destination) - JiJoo.RealPosition(enemy.gridPosition);
        time = realVector.x / horizontalSpeed + realVector.y / verticalSpeed;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (timer > time)
        {
            enemy.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            enemy.gridPosition = destination;
            enemy.transform.position = JiJoo.RealPosition(destination);
            animator.SetTrigger("IdleTrigger");
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
