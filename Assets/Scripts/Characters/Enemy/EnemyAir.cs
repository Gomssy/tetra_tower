using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir : Enemy {
    // for bumping attack
    public bool bumped = false;
    public bool prevBumped = false;
    protected override void Start()
    {
        base.Start();
        prevBumped = bumped;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(prevBumped != bumped && bumped && movementLock == EnemyMovementLock.Free)
        {
            StartCoroutine(Knockback(0.0f, 2.0f));
            StartCoroutine(RecoverBump());
        }
        prevBumped = bumped;
    }

    public void ChangeVelocityXY_zero() // 망할 유니티 애니메이션 이벤트 Vec2를 parameter로 받는 함수를 못집어넣음
    {
        ChangeVelocityXY_movement(Vector2.zero);
    }

    public void ChangeAngleZ_movement(float val)
    {
        if (movementLock != EnemyMovementLock.Free) { return; }
        ChangeAngleZ(val);
    }

    public void ChangeVelocityXY_movement(Vector2 val)
    {
        if (movementLock != EnemyMovementLock.Free) { return; }
        ChangeVelocityXY(val);
    }
    
    private void ChangeAngleZ(float val)
    {
        Vector3 tempAngle = transform.parent.eulerAngles;
        tempAngle.z = val;
        transform.parent.eulerAngles = tempAngle;
    }

    private void ChangeVelocityXY(Vector2 val)
    {
        Vector3 tempVelocity = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
        tempVelocity.x = val.x;
        tempVelocity.y = val.y;
        transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = tempVelocity;
    }

    // - Knockback coroutine
    protected override IEnumerator Knockback(float knockbackDist, float knockbackTime)
    {
        Vector2 knockbackDir = (transform.parent.position - enemyManager.Player.transform.position).normalized;
        Vector2 knockbackVelocity = (knockbackDist / knockbackTime) * knockbackDir;

        ChangeAngleZ(90 + Mathf.Rad2Deg * Mathf.Atan2(knockbackDir.y, knockbackDir.x));
        ChangeVelocityXY(knockbackVelocity);
        yield return new WaitForSeconds(knockbackTime);
        ChangeVelocityXY(Vector2.zero);
        if (movementLock != EnemyMovementLock.Debuffed) movementLock = EnemyMovementLock.Free;
    }

    protected override IEnumerator OnIce(float duration)
    {
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
        bumped = true;
        animator.SetTrigger("StunnedTrigger");
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        bumped = false;
        OffDebuff(EnemyDebuffCase.Ice);
    }

    protected override IEnumerator OnStun(float duration)
    {
        bumped = true;
        animator.SetTrigger("StunnedTrigger");
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Stun);
        bumped = false;
        yield return null;
    }

    IEnumerator RecoverBump()
    {
        yield return new WaitForSeconds(2.0f);
        bumped = false;
    }
}
