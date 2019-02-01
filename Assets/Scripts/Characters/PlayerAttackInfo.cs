using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackInfo {
    public float damage = 0;
    public float knockBackMultiplier = 1f;
    public int debuffNum = 0;
    public EnemyDebuffCase[] debuffType = new EnemyDebuffCase[10];
    public int[] debuffTime = new int[10];
    public PlayerAttackInfo(float damage, float knockBackMultiplier, int debuffNum, EnemyDebuffCase[] debuffType, int[] debuffTime)
    {
        this.damage = damage;
        this.knockBackMultiplier = knockBackMultiplier;
        this.debuffNum = debuffNum;
        this.debuffType = debuffType;
        this.debuffTime = debuffTime;
    }
}
