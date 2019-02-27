using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyGround : Enemy {

    public float attackRange;
    public int MoveDir { get; private set; }
    public bool[] WallTest;
    public bool[] CliffTest;

    GameObject target;
    private Quaternion _rotation;

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

    private void ChangeDir_forAnimation()
    {
        int knockbackDir = (GameManager.Instance.player.transform.position.x - transform.parent.position.x >= 0) ? -1 : 1;
        ChangeDir(knockbackDir * -1);
    }

    // - Knockback coroutine
    protected override IEnumerator Knockback(float knockbackDist, float knockbackTime)
    {
        int knockbackDir = (GameManager.Instance.player.transform.position.x - transform.parent.position.x >= 0) ? -1 : 1;
        float knockbackVelocity = knockbackDir * knockbackDist / knockbackTime;
         //ChangeDir(knockbackDir * -1);
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
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned"));
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Stun);
    }

    public void SetTarget()
    {
        
        target = GameManager.Instance.player;
        Vector2 direction = transform.GetChild(0).position - target.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _rotation = rotation;
        Flip();
    }

    public void ArrowShot()
    {
        GameObject enemy_arrow = Resources.Load<GameObject>("Prefabs/Projectiles/enemy_arrow");
        Instantiate(enemy_arrow, transform.GetChild(0).position, _rotation);
    }

    public void Flip()
    {
        NumeratedDir trackDir = (transform.GetChild(0).position.x - target.transform.position.x > 0) ? NumeratedDir.Left : NumeratedDir.Right;
        ChangeDir_noOption(trackDir);
    }
}
