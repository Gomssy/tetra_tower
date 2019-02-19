using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackInfo {
    public float damage = 0;
    public float knockBackMultiplier = 1f;
    public float[] debuffTime = new float[(int)EnemyDebuffCase.END_POINTER];
    public PlayerAttackInfo(float damage, float knockBackMultiplier, float[] debuffTime)
    {
        this.damage = damage;
        this.knockBackMultiplier = knockBackMultiplier;
        this.debuffTime = debuffTime;
    }
    public PlayerAttackInfo(PlayerAttackInfo origin)
    {
        damage = origin.damage;
        knockBackMultiplier = origin.knockBackMultiplier;
        debuffTime = origin.debuffTime;
    }
}

