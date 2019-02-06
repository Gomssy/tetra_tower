using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProperty : MonoBehaviour{
    public float damage = 0;
    public float knockBackMultiplier = 1f;
    public int debuffNum = 0;
    public EnemyDebuffCase[] debuffType = new EnemyDebuffCase[10];
    public int[] debuffTime = new int[10];
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy") && !collision.transform.GetChild(0).GetComponent<Enemy>().untouchable)
        {
            Debug.Log(damage);
            PlayerAttackInfo curAttack = new PlayerAttackInfo(damage, knockBackMultiplier, debuffNum, debuffType, debuffTime);
            Enemy enemyInfo = collision.transform.GetChild(0).GetComponent<Enemy>();
            collision.transform.GetChild(0).GetComponent<Enemy>().GetDamaged(curAttack);
        }
    }
}
