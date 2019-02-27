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
        //transitionUpdate[1] += Phase2Transition;
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
        Debug.Log("aaa");
        animator.runtimeAnimatorController = animators[bossRoom.CurPhase];
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

    public abstract Vector2Int MoveDirection();

    public IEnumerator Heal(float hp, float time)
    {
        float delta = hp - CurrHealth;
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            yield return null;
            CurrHealth += (delta * t / time);
        }
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
