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
        if(prevBumped != bumped && bumped && !MovementLock)
        {
            StartCoroutine(Knockback(0.0f, 1.0f));
            StartCoroutine(RecoverBump());
        }
        prevBumped = bumped;
    }

    public void ChangeAngleZ_noOption(float val)
    {
        ChangeAngleZ(val, new bool[] { MovementLock, KnockbackLock });
    }

    public void ChangeVelocityXY_zero() // 망할 유니티 애니메이션 이벤트 Vec2를 parameter로 받는 함수를 못집어넣음
    {
        ChangeVelocityXY(Vector2.zero, new bool[] { MovementLock, KnockbackLock });
    }

    public void ChangeVelocityXY_noOption(Vector2 val)
    {
        ChangeVelocityXY(val, new bool[] { MovementLock, KnockbackLock });
    }

    private void ChangeAngleZ(float val, bool[] lockArray)
    {
        foreach (var Lock in lockArray) { if (Lock) { return; } }
        Vector3 tempAngle = transform.parent.eulerAngles;
        tempAngle.z = val;
        transform.parent.eulerAngles = tempAngle;
    }

    private void ChangeVelocityXY(Vector2 val, bool[] lockArray)
    {
        foreach (var Lock in lockArray) { if (Lock) { return; } }
        Vector3 tempVelocity = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
        tempVelocity.x = val.x;
        tempVelocity.y = val.y;
        transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = tempVelocity;
    }

    protected override IEnumerator OnIce(float duration)
    {
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
        ChangeVelocityXY(Vector2.zero, new bool[] { });
        KnockbackLock = true;
        animator.SetTrigger("StunnedTrigger");
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Ice);
    }

    protected override IEnumerator OnStun(float duration)
    {
        ChangeVelocityXY(Vector2.zero, new bool[] { });
        animator.SetTrigger("StunnedTrigger");
        animator.speed = stunnedAnimLength / duration;
        yield return new WaitForSeconds(duration);
        OffDebuff(EnemyDebuffCase.Stun);
        yield return null;
    }

    // - Knockback coroutine
    protected override IEnumerator Knockback(float knockbackDist, float knockbackTime)
    {
        MovementLock = true;
        bool[] lockArray = new bool[] { false, KnockbackLock };
        Vector2 knockbackDir = (transform.parent.position - enemyManager.Player.transform.position).normalized;
        Vector2 knockbackVelocity = (knockbackDist / knockbackTime) * knockbackDir;
        ChangeAngleZ(Mathf.Atan2(knockbackDir.y, knockbackDir.x) * -1, new bool[] { MovementLock, KnockbackLock });
        ChangeVelocityXY(knockbackVelocity, lockArray);
        
        yield return new WaitForSeconds(knockbackTime);
        MovementLock = false;
        ChangeVelocityXY(Vector2.zero, new bool[] { MovementLock, KnockbackLock });
    }

    IEnumerator RecoverBump()
    {
        yield return new WaitForSeconds(1.0f);
        bumped = false;
    }
}
