using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiJooMove : StateMachineBehaviour {
    float horizontalSpeed;
    float verticalSpeed;
    GameObject player;
    Transform animatorRoot;
    Rigidbody2D rb2D;
    JiJoo enemy;
    Vector2Int dir;
    Vector2Int destination;
    Vector2 velocity;

    float time;
    float timer = 0;

     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animatorRoot = animator.transform.parent;
        enemy = animator.GetComponent<JiJoo>();
        player = GameManager.Instance.player;
        rb2D = enemy.transform.parent.GetComponent<Rigidbody2D>();

        horizontalSpeed = enemy.horizontalSpeed;
        verticalSpeed = enemy.verticalSpeed;

        dir = enemy.MoveDirection();
        destination = enemy.gridPosition + dir;
        if (destination.x < 0 || destination.x >= 6 || destination.y < 0 || destination.y >= 6)
        {
            animator.SetTrigger("IdleTrigger");
            return;
        }

        enemy.transform.eulerAngles = new Vector3(0, 0, JiJoo.Vector2ToZAngle(dir));
        velocity = dir * new Vector2(horizontalSpeed, verticalSpeed);
        Vector2 realVector = JiJoo.RealPosition(destination) - JiJoo.RealPosition(enemy.gridPosition);
        time = Mathf.Abs(realVector.x) / horizontalSpeed + Mathf.Abs(realVector.y) / verticalSpeed;
        timer = 0;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        rb2D.MovePosition(rb2D.position + velocity * Time.deltaTime);
        if (timer > time)
        {
            Debug.Log("end");
            enemy.gridPosition = destination;
            enemy.transform.parent.transform.localPosition = JiJoo.RealPosition(destination);
            animator.SetTrigger("IdleTrigger");
        }
        timer += Time.deltaTime;
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
