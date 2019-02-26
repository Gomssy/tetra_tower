using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Enemy {
    public bool neverDie;

    public override void GetDamaged(PlayerAttackInfo attack)
    {
        if (Invisible) { return; }
        float prevHealth = currHealth;
        currHealth -= attack.damage;

        if (currHealth <= 0)
        {
            if (neverDie)
            {
                currHealth = maxHealth;
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
