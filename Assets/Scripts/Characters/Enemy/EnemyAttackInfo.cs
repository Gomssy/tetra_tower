using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackInfo {
    public float damage = 0;
    public float knockBackMultiplier = 1f;
    public int debuffNum = 0;
    public PlayerDebuffCase[] debuffType = new PlayerDebuffCase[10];
    public int[] debuffTime = new int[10];
    public EnemyAttackInfo(float damage, float knockBackMultiplier, int debuffNum, PlayerDebuffCase[] debuffType, int[] debuffTime)
    {
        this.damage = damage;
        this.knockBackMultiplier = knockBackMultiplier;
        this.debuffNum = debuffNum;
        this.debuffType = debuffType;
        this.debuffTime = debuffTime;
    }
}
