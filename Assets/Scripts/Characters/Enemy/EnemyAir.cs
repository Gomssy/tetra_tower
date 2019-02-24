using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAir : Enemy {
    protected override IEnumerator OnIce(float duration)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator OnStun(float duration)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Knockback(float knockbackDist, float knockbackTime)
    {
        throw new System.NotImplementedException();
    }

    public void ChangeAngleZ_noOption(float val)
    {
        ChangeAngleZ(val, new bool[] { MovementLock, KnockbackLock });
    }

    public void ChangeVelocityXY_noOption(Vector2 val)
    {
        ChangeVelocityXY(val, new bool[] { MovementLock, KnockbackLock });
    }

    private void ChangeAngleZ(float val, bool[] lockArray)
    {
        foreach (var Lock in lockArray) { if (Lock) return; }
        Vector3 tempAngle = transform.parent.eulerAngles;
        tempAngle.z = val;
        transform.parent.eulerAngles = tempAngle;
    }

    private void ChangeVelocityXY(Vector2 val, bool[] lockArray)
    {
        foreach (var Lock in lockArray) { if (Lock) return; }
        Vector3 tempVelocity = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
        tempVelocity.x = val.x;
        tempVelocity.y = val.y;
        transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = tempVelocity;
    }
}
