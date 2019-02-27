using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToPlayer : MonoBehaviour {
    public int damage;
    public bool isBumpAttack;
    // enum debuff
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBumpAttack)
        {
            EnemyAttackInfo attack = new EnemyAttackInfo(damage, 1f, 0, null, null);
            collision.gameObject.GetComponent<PlayerAttack>().TakeDamage(attack);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isBumpAttack && !transform.parent.gameObject.GetComponent<EnemyAir>().bumped)
        {
            EnemyAttackInfo attack = new EnemyAttackInfo(damage, 1f, 0, null, null);
            collision.gameObject.GetComponent<PlayerAttack>().TakeDamage(attack);
            transform.parent.gameObject.GetComponent<EnemyAir>().bumped = true;
        }
    }
}
