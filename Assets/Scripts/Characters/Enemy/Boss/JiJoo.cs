using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JiJoo : Boss {
    public static int[] webGrid = { 4, 7, 10, 14, 17, 20 };
    public Vector2Int gridPosition;

    public Vector2 playerDirection;

    public float horizontalSpeed;
    public float verticalSpeed;

    public abstract bool IsAttackable();

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        playerDirection = GameManager.Instance.player.transform.position - transform.position;

        bossRoom.transitionUpdate[0] += Phase1Transition;
        bossRoom.transitionUpdate[1] += Phase2Transition;
        bossRoom.phaseUpdate[0] += Phase1;
        //phaseUpdate[1] += Phase2;
    }

    protected virtual void Update()
    {
        if(bossRoom.CurPhase == 0)
            animator.SetBool("Attackable", IsAttackable());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        playerDirection = GameManager.Instance.player.transform.position - transform.position;
    }

    protected void Phase1Transition()
    {
        animator.runtimeAnimatorController = animators[bossRoom.CurPhase];
        bossRoom.isTransitionFinished = true;
    }
    protected void Phase2Transition()
    {
        StartCoroutine(Heal(maxHealth, 2));
    }
    protected void Phase1()
    {
        
    }
    protected void Phase2()
    {
        
    }

    public override void GetDamaged(PlayerAttackInfo attack)
    {
        if (Invisible) { return; }
        float prevHealth = currHealth;
        currHealth -= attack.damage;

        if (currHealth <= 0)
        {
            Invisible = true;
            animator.SetTrigger("DeadTrigger");
            StopCoroutine("OnFire");
            GetComponent<SpriteRenderer>().color = Color.white;
            if (bossRoom.CurPhase == bossRoom.totalPhase - 1)
            {
                currHealth = 1;
            }
        }

        DebuffApply(attack.debuffTime);

        float knockbackDist = attack.damage * attack.knockBackMultiplier / weight;
        float knockbackTime = (knockbackDist >= 0.5f) ? 0.5f : knockbackDist;

        if (MovementLock) // 넉백이 진행 중
        {
            StopCoroutine("Knockback");
        }
        StartCoroutine(Knockback(knockbackDist, knockbackTime));

        float currHealthPercentage = currHealth / maxHealth;
        float prevHealthPercentage = prevHealth / maxHealth;

        foreach (float percentage in knockbackPercentage)
        {
            if (currHealthPercentage > percentage) { break; }
            if (prevHealthPercentage > percentage)
            {
                animator.SetTrigger("DamagedTrigger");
                break;
            }
        }
        animator.SetTrigger("TrackTrigger");
    }

    public abstract Vector2Int MoveDirection();

    public IEnumerator Heal(float hp, float time)
    {
        float delta = hp - currHealth;
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            yield return null;
            currHealth += (delta * t / time);
        }
        currHealth = hp;
    }

    public static float Vector2ToZAngle(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            return 0;
        else if (dir == Vector2Int.left)
            return 90;
        else if (dir == Vector2Int.down)
            return 180;
        else if (dir == Vector2Int.right)
            return 270;

        return 0;
    }
    public static Vector2 RealPosition(Vector2Int gridPosition)
    {
        return new Vector2(webGrid[gridPosition.x], webGrid[gridPosition.y]);
    }
}
