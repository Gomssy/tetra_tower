using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : StateMachineBehaviour {
    float timer;
    float attackDelay;
    float attackDuration;
    public Sprite castingSprite;
    public Sprite attackingSprite;
    Sprite prevSprite;

    enum SubState
    {
        BEFOREATTACK,
        CASTING,
        ATTACKING
    }
    SubState subState;
    
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        prevSprite = animator.GetComponent<SpriteRenderer>().sprite;
        attackDelay = animator.GetFloat("attackDelay");
        attackDuration = animator.GetFloat("attackDuration");
        subState = SubState.BEFOREATTACK;
        timer = 0.0f;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        timer += Time.deltaTime;
        if (timer < attackDelay)
        {
            if (subState == SubState.BEFOREATTACK)
            {
                subState = SubState.CASTING;
                animator.GetComponent<SpriteRenderer>().sprite = castingSprite;
            }
            // action during casting attack
        }
        else if (timer < attackDelay + attackDuration)
        {
            if (subState == SubState.CASTING)
            {
                subState = SubState.ATTACKING;
                animator.GetComponent<SpriteRenderer>().sprite = attackingSprite;
                animator.transform.GetChild(0).GetComponents<BoxCollider2D>()[0].enabled = true;
            }
        }
        else
        {
            animator.SetTrigger("TrackTrigger");
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<SpriteRenderer>().sprite = prevSprite;
        animator.transform.GetChild(0).GetComponents<BoxCollider2D>()[0].enabled = false;
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
