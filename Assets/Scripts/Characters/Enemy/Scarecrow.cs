using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Enemy {
    public bool neverDie;

    public override void GetHit(PlayerAttackInfo attack)
    {
        if (Invisible) { return; }
        float prevHealth = CurrHealth;
        CurrHealth -= attack.damage;

        if (CurrHealth <= 0)
        {
            if (neverDie)
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
}
