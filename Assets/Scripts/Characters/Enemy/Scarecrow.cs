using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Enemy {
    public bool neverDie;

    public override void GetHit(PlayerAttackInfo attack)
    {
        TakeDamage(attack.damage);
        DebuffApply(attack.debuffTime);
    }

    public override void TakeDamage(float damage)
    {
        if (Invisible) { return; }
        float prevHealth = CurrHealth;
        CurrHealth -= damage;
        if (CurrHealth <= 0)
        {
            if (neverDie)
            {
                CurrHealth = maxHealth;
            }
            else
            {
                MakeDead();
                return;
            }
        }
        animator.SetTrigger("DamagedTrigger");
    }
}
