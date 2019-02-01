using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToPlayer : MonoBehaviour {
    public int damage;
    // enum debuff
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnemyAttackInfo attack = new EnemyAttackInfo(damage, 1f, 0, null, null);
            collision.gameObject.GetComponent<PlayerAttack>().TakeDamage(attack);
        }
    }
}
