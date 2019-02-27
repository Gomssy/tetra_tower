using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyGround : Enemy {

    public float attackRange;

    public int MoveDir { get; private set; }
    public bool[] WallTest;
    public bool[] CliffTest;

    protected override void Awake()
    {
        base.Awake();

        WallTest = new bool[] { false, false };
        CliffTest = new bool[] { false, false };
    }

    protected override void Start()
    {
        base.Start();

        MoveDir = (int)NumeratedDir.Left;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckCliff(); CheckWall();
    }

    // Movement & Physics

    // - Check whether enemy is near to cliff
    private void CheckCliff()
    {
        Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;

        foreach (int Dir in Enum.GetValues(typeof(NumeratedDir)))
        {
            Vector2 origin = (Vector2)transform.parent.position + Dir * new Vector2(colliderSize.x / 2.0f, 0);
            Vector2 direction = Vector2.down;
            float distance = colliderSize.y / 4.0f;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, enemyManager.layerMaskPlatform);

            CliffTest[(Dir + 1) / 2] = (hit.collider == null);
        }
    }

    // - Check whether enemy is touching wall
    private void CheckWall()
    {
        Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;

        foreach (int Dir in Enum.GetValues(typeof(NumeratedDir)))
        {
            Vector2 origin = (Vector2)transform.parent.position + new Vector2(Dir * colliderSize.x / 2.0f, colliderSize.y / 2.0f);
            Vector2 direction = Vector2.right * Dir;
            float distance = 0.2f;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, enemyManager.layerMaskWall);

            WallTest[(Dir + 1) / 2] = (hit.collider != null);
        }
    }


    // - Change direction, and speed of rigidbody of enemy
    public void ChangeVelocityX_movement(float val)
    {
        if(movementLock != EnemyMovementLock.Free) { return; }
        ChangeVelocityX(val);
    }

    public void ChangeDir_movement(object dir)
    {
        if (movementLock != EnemyMovementLock.Free) { return; }
        ChangeDir(dir);
    }

    private void ChangeVelocityX(float val)
    {
        Vector2 tempVelocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
        tempVelocity.x = val;
        transform.parent.GetComponent<Rigidbody2D>().velocity = tempVelocity;
    }

    private void ChangeDir(object dir)
    {
        MoveDir = (int)dir;
        transform.parent.eulerAngles = ((NumeratedDir)dir == NumeratedDir.Left) ? new Vector2(0, 0) : new Vector2(0, 180);
    }

    // - Knockback coroutine
    protected override IEnumerator Knockback(float knockbackDist, float knockbackTime)
    {
        int knockbackDir = (enemyManager.Player.transform.position.x - transform.parent.position.x >= 0) ? -1 : 1;
        float knockbackVelocity = knockbackDir * knockbackDist / knockbackTime;
        ChangeDir_movement(knockbackDir * -1);
        ChangeVelocityX(knockbackVelocity);

        for (float timer = 0; timer <= knockbackTime; timer += Time.deltaTime)
        {
            if (CliffTest[(knockbackDir + 1) / 2])
            {
                ChangeVelocityX(0.0f);
                yield return new WaitForSeconds(knockbackTime - timer);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        ChangeVelocityX(0.0f);
        if (movementLock != EnemyMovementLock.Debuffed) movementLock = EnemyMovementLock.Free;
    }

    protected override IEnumerator OnIce(float duration)
    {
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
        animator.SetTrigger("StunnedTrigger");
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Ice);
    }

    protected override IEnumerator OnStun(float duration)
    {
        animator.SetTrigger("StunnedTrigger");
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Stun);
    }
}
