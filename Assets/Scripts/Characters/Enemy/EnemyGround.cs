using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyGround : Enemy {

    public float attackRange;

    public int MoveDir { get; private set; }
    public bool[] WallTest { get; private set; }
    public bool[] CliffTest { get; private set; }

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
        Vector2 velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
        Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;

        foreach (int Dir in Enum.GetValues(typeof(NumeratedDir)))
        {
            Vector2 origin = (Vector2)transform.parent.position + Dir * new Vector2(colliderSize.x / 2.0f, 0);
            Vector2 direction = Vector2.down;
            float distance = colliderSize.y / 4.0f;
            int layerMask = LayerMask.NameToLayer("platform");
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);

            CliffTest[(Dir + 1) / 2] = (hit.collider == null);
        }
    }

    // - Check whether enemy is touching wall
    private void CheckWall()
    {
        Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;

        foreach (int Dir in Enum.GetValues(typeof(NumeratedDir)))
        {
            Vector2 origin = (Vector2)transform.parent.position + new Vector2(Dir * colliderSize.x / 2.0f, colliderSize.y);
            Vector2 direction = Vector2.right * Dir;
            float distance = 0.5f;
            LayerMask layerMask = LayerMask.GetMask("Wall", "OuterWall");
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);

            WallTest[(Dir + 1) / 2] = (hit.collider != null);
        }
    }


    // - Change direction, and speed of rigidbody of enemy
    public void ChangeVelocityX_noOption(float val)
    {
        ChangeVelocityX(val, new bool[] { MovementLock, KnockbackLock });
    }

    public void ChangeDir_noOption(object dir)
    {
        ChangeDir(dir, new bool[] { MovementLock, KnockbackLock });
    }

    private void ChangeVelocityX(float val, bool[] lockArray)
    {
        foreach (var Lock in lockArray) { if (Lock) return; }
        Vector2 tempVelocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
        tempVelocity.x = val;
        transform.parent.GetComponent<Rigidbody2D>().velocity = tempVelocity;
    }

    private void ChangeDir(object dir, bool[] lockArray)
    {
        foreach (var Lock in lockArray) { if (Lock) return; }
        MoveDir = (int)dir;
        transform.parent.eulerAngles = ((NumeratedDir)dir == NumeratedDir.Left) ? new Vector2(0, 0) : new Vector2(0, 180);
    }

    // - Knockback coroutine
    protected override IEnumerator Knockback(float knockbackDist, float knockbackTime)
    {
        MovementLock = true;
        bool[] lockArray = new bool[] { false, KnockbackLock };
        int knockbackDir = (enemyManager.Player.transform.position.x - transform.parent.position.x >= 0) ? -1 : 1;
        float knockbackVelocity = knockbackDir * knockbackDist / knockbackTime;
        ChangeDir(knockbackDir * -1, new bool[] { MovementLock, KnockbackLock });
        ChangeVelocityX(knockbackVelocity, lockArray);

        for (float timer = 0; timer <= knockbackTime; timer += Time.deltaTime)
        {
            if (CliffTest[(knockbackDir + 1) / 2])
            {
                ChangeVelocityX(0.0f, lockArray);
                yield return new WaitForSeconds(knockbackTime - timer);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        MovementLock = false;
        ChangeVelocityX(0.0f, new bool[] { MovementLock, KnockbackLock });
    }

    protected override IEnumerator OnIce(float duration)
    {
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
        ChangeVelocityX(0.0f, new bool[] { });
        KnockbackLock = true;
        animator.SetTrigger("StunnedTrigger");
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Ice);
    }

    protected override IEnumerator OnStun(float duration)
    {
        ChangeVelocityX(0.0f, new bool[] { });
        animator.SetTrigger("StunnedTrigger");
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Stun);
        yield return null;
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
