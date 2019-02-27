using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTrack : StateMachineBehaviour
{
    float trackSpeed;
    float attackRange;
    GameObject player;
    Transform animatorRoot;
    EnemyGround enemy;
   // GameObject enemy_arrow;



    readonly int maxFrame = 10;
    int frameCounter = 0;

    float waitBetweenShots;
    private float shotCounter;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // enemy_arrow = Resources.Load<GameObject>("Prefabs/Projectiles/enemy_arrow");
      //  waitBetweenShots = 1f;
       // shotCounter = waitBetweenShots;

        animatorRoot = animator.transform.parent;
        enemy = animator.GetComponent<EnemyGround>();
        player = GameManager.Instance.player;


        trackSpeed = enemy.trackSpeed;
        attackRange = enemy.attackRange;

        NumeratedDir trackDir = (animatorRoot.position.x - player.transform.position.x > 0) ? NumeratedDir.Left : NumeratedDir.Right;
        enemy.ChangeDir_noOption(trackDir);
        if (enemy.CliffTest[(enemy.MoveDir + 1) / 2] || animator.GetComponent<Enemy>().PlayerDistance < attackRange)
        {
            enemy.ChangeVelocityX_noOption(0.0f);
        }
        else
        {
            enemy.ChangeVelocityX_noOption(enemy.MoveDir * trackSpeed);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //shotCounter -= Time.deltaTime;
        if (animator.GetComponent<Enemy>().PlayerDistance < attackRange/* && shotCounter < 0*/)
        {
            animator.SetTrigger("AttackTrigger");
          //  GameManager.Instance.StartCoroutine(WaitforShot());
            return;

        }
        int integerDir = enemy.MoveDir;
        if (enemy.WallTest[(integerDir + 1) / 2] || enemy.CliffTest[(integerDir + 1) / 2])
        {
            enemy.ChangeVelocityX_noOption(0.0f);
        }
        else
        {
            enemy.ChangeVelocityX_noOption(enemy.MoveDir * trackSpeed);
        }

        frameCounter += 1;
        if (frameCounter >= maxFrame)
        {
            NumeratedDir trackDir = (animatorRoot.position.x - player.transform.position.x > 0) ? NumeratedDir.Left : NumeratedDir.Right;
            enemy.ChangeDir_noOption(trackDir);
            frameCounter = 0;
        }
    }

   /* IEnumerator WaitforShot()
    {
        yield return new WaitForSeconds(0.25f);
        Vector2 direction = enemy.transform.GetChild(0).position - player.transform.position;

        yield return new WaitForSeconds(0.5f);
       
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(enemy_arrow, enemy.transform.GetChild(0).position, rotation);

        shotCounter = waitBetweenShots;
    }
    */
 


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
