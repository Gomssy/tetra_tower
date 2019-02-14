using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProperty : MonoBehaviour{
    public float damage = 0;
    public float knockBackMultiplier = 1f;
    public float[] debuffTime = new float[(int)EnemyDebuffCase.END_POINTER];
    EffectManager effectManager;
    PlayerAttack playerAttack;
    InventoryManager inventoryManager;

    private void Awake()
    {
        effectManager = EffectManager.Instance;
        playerAttack = transform.parent.GetComponentInChildren<PlayerAttack>();
        inventoryManager = InventoryManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Bounds tmpBounds = new Bounds();
        if (collision.CompareTag("Enemy") && !collision.transform.GetChild(0).GetComponent<Enemy>().Invisible)
        {
            PlayerAttackInfo curAttack = new PlayerAttackInfo(damage, knockBackMultiplier, debuffTime);
            Enemy enemyInfo = collision.transform.GetChild(0).GetComponent<Enemy>();

            string combo = playerAttack.comboArray;
            foreach (Item tmpItem in inventoryManager.itemList)
                for (int i = 0; i < tmpItem.skillNum; i++)
                {
                    if (tmpItem.combo[i].Equals(combo))
                    {
                        tmpItem.AttackCalculation(curAttack, enemyInfo, combo);
                        break;
                    }
                }

            collision.transform.GetChild(0).GetComponent<Enemy>().GetDamaged(curAttack);
            
            //make effect
            foreach (Collider2D col in GetComponents<Collider2D>())
                if (col.isActiveAndEnabled)
                    tmpBounds = col.bounds;

            if (!tmpBounds.Equals(new Bounds()))
                effectManager.StartEffect(0, tmpBounds, collision.bounds);
        }
    }
}
