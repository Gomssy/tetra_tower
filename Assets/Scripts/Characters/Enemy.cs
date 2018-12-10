using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour {

    private readonly float maxHealth;
    private readonly float weight;
    private float currHealth;
    private EnemyManager.State currState;
    private List<int[]> dropTable; // [item ID, numerator]
    private Dictionary<EnemyManager.State, EnemyManager.Action<int>> actionByState;
    public Enemy(uint id, float maxHealth, float weight) {
        this.maxHealth = maxHealth;
        this.weight = weight;
        this.currHealth = maxHealth;
        this.dropTable = GetDropTable(id);
        this.actionByState = GetActionByState(id);
        this.currState = EnemyManager.State.Idle;
    }
    public void GetDamaged(float damage) {
        float unitDist = 3;
        currHealth -= damage;
        if(currHealth <= 0) {
            mamaWooWooWoo_I_Dont_Wanna_Die();
            return;
        }
        float knockback_dist = damage * unitDist / weight;
        // do something - knockback animation
    }
    private List<int[]> GetDropTable(uint id) {
        List<int[]> resultList = new List<int[]>();
        return resultList;
    }
    private Dictionary<EnemyManager.State, EnemyManager.Action<int>> GetActionByState(uint id) {
        var resultDictionary = new Dictionary<EnemyManager.State, EnemyManager.Action<int>>();
        return resultDictionary;
    }
    private void mamaWooWooWoo_I_Dont_Wanna_Die() {
        int dropItemId = -1;
        if (dropTable != null) {
            float denominator = dropTable[dropTable.Count - 1][1];
            float numerator = Random.Range(0f, denominator);

            foreach (var drop in dropTable) {
                if (numerator <= drop[1]) {
                    dropItemId = drop[0];
                    break;
                }
            }
        }
        // spawn a item that has ID

        // destroy itself (or, deactivate for pooling)
        Destroy(gameObject);
    }
}

