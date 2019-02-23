using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProperty : MonoBehaviour {
    public float damage = 0;
    public float knockBackMultiplier = 1f;
    public float[] debuffTime = new float[(int)EnemyDebuffCase.END_POINTER];
    public int projectileType; //0: melee attack, 1: vanish after hit
    EffectManager effectManager;
    InventoryManager inventoryManager;
    public LayerMask enemyLayer;
    public LayerMask vanishLayer;
    public LayerMask stopLayer;
    public string attackCombo;

    private void Awake()
    {
        effectManager = EffectManager.Instance;
        inventoryManager = InventoryManager.Instance;
    }

    public void Init(string combo)
    {
        attackCombo = combo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Bounds tmpBounds = new Bounds();
        if ((enemyLayer == (enemyLayer | 1 << collision.gameObject.layer)) && !collision.transform.GetChild(0).GetComponent<Enemy>().Invisible)
        {
            PlayerAttackInfo curAttack = new PlayerAttackInfo(damage, knockBackMultiplier, debuffTime);
            Enemy enemyInfo = collision.transform.GetChild(0).GetComponent<Enemy>();

            foreach (Item tmpItem in inventoryManager.itemList)
                for (int i = 0; i < tmpItem.skillNum; i++)
                {
                    if (tmpItem.combo[i].Equals(attackCombo))
                    {
                        tmpItem.AttackCalculation(curAttack, enemyInfo, attackCombo);
                        break;
                    }
                }

            collision.transform.GetChild(0).GetComponent<Enemy>().GetDamaged(curAttack);

            //make effect
            foreach (Collider2D col in GetComponents<Collider2D>())
                if (col.isActiveAndEnabled)
                    tmpBounds = col.bounds;

            if (!tmpBounds.Equals(new Bounds()))
            {
                effectManager.StartEffect(0, tmpBounds, collision.bounds);
                effectManager.StartNumber(attackCombo[attackCombo.Length - 1] - 'A', tmpBounds, collision.bounds, curAttack.damage);
            }

        }
        if (projectileType == 1 && (vanishLayer == (vanishLayer | 1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
        if (projectileType == 1 && (stopLayer == (stopLayer | 1 << collision.gameObject.layer)))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(WaitVanish(10f));
        }
    }
    IEnumerator WaitVanish(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
