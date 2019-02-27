using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Enemy {
    public bool scarecrow_or_statue; // true: scarecrow

    public override void GetHit(PlayerAttackInfo attack)
    {
        if (Invisible) { return; }
        CurrHealth -= attack.damage;

        if (CurrHealth <= 0)
        {
            if (scarecrow_or_statue)
            {
                CurrHealth = maxHealth;
            }
            else
            {
                Invisible = true;
                animator.SetTrigger("DeadTrigger");
                return;
            }
        }

        animator.SetTrigger("DamagedTrigger");
    }

    public override void DeadEvent()
    {
        if (scarecrow_or_statue)
        {
            base.DeadEvent();
        }
        else
        {
            MapManager.Instance.UpgradeRoom(RoomType.Gold);
            Destroy(transform.parent.gameObject);
        }
    }
}
