using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ji : JiJoo {
    public float attackRange;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsAttackable()
    {
        return PlayerDistance < attackRange;
    }

    public override Vector2Int MoveDirection()
    {
        Vector2Int dir;
        if (Mathf.Abs(playerDirection.x) > Mathf.Abs(playerDirection.y))
            dir = (playerDirection.x > 0) ? Vector2Int.right : Vector2Int.left;
        else
            dir = (playerDirection.y > 0) ? Vector2Int.up : Vector2Int.down;
        return dir;
    }
}
