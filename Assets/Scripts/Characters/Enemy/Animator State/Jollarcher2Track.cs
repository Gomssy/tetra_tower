using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jollarcher2Track : StateMachineBehaviour
{
    float trackSpeed;
    float attackRange;
    GameObject player;
    Transform animatorRoot;
    Enemy enemy;
    GameObject jollarcher_arrow;
    EnemyArrow enemyArrow;


    readonly int maxFrame = 10;
    int frameCounter = 0;

    float waitBetweenShots;
    private float shotCounter;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        jollarcher_arrow = Resources.Load<GameObject>("Prefabs/Projectiles/jollarcher2_arrow");
        waitBetweenShots = 0.1f;
        shotCounter = waitBetweenShots;

        animatorRoot = animator.transform.parent;
        enemy = animator.GetComponent<Enemy>();
        player = EnemyManager.Instance.Player;


        trackSpeed = enemy.trackSpeed;
        attackRange = enemy.attackRange;

        NumeratedDir trackDir = (animatorRoot.position.x - player.transform.position.x > 0) ? NumeratedDir.Left : NumeratedDir.Right;
        enemy.ChangeDir(trackDir);
        if (enemy.CliffTest[(enemy.MoveDir + 1) / 2] || animator.GetComponent<Enemy>().PlayerDistance < attackRange)
        {
            enemy.ChangeVelocityX(0.0f);
        }
        else
        {
            enemy.ChangeVelocityX(enemy.MoveDir * trackSpeed);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        shotCounter -= Time.deltaTime;
        if (animator.GetComponent<Enemy>().PlayerDistance < attackRange && shotCounter < 0)
        {
            animator.SetTrigger("AttackTrigger");
         
                Instantiate(jollarcher_arrow, enemy.transform.GetChild(0).position, Quaternion.identity);
     
            shotCounter = waitBetweenShots;
            return;
        }
        int integerDir = enemy.MoveDir;
        if (enemy.WallTest[(integerDir + 1) / 2] || enemy.CliffTest[(integerDir + 1) / 2])
        {
            enemy.ChangeVelocityX(0.0f);
        }
        else
        {
            enemy.ChangeVelocityX(enemy.MoveDir * trackSpeed);
        }

        frameCounter += 1;
        if (frameCounter >= maxFrame)
        {
            NumeratedDir trackDir = (animatorRoot.position.x - player.transform.position.x > 0) ? NumeratedDir.Left : NumeratedDir.Right;
            enemy.ChangeDir(trackDir);
            frameCounter = 0;
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
