using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProperty : MonoBehaviour{
    public float damage=0;
    public float knockBackMultiplier = 1f;
    public int debuffNum = 0;
    public EnemyDebuffCase[] debuffType = new EnemyDebuffCase[10];
    public int[] debuffTime = new int[10];
}
