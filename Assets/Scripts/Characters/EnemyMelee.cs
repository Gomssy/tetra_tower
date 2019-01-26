using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy {
    public void AttackOn()
    {
        transform.parent.Find("Hitbox").GetComponent<BoxCollider2D>().enabled = true;
    }
    public void AttackOff()
    {
        transform.parent.Find("Hitbox").GetComponent<BoxCollider2D>().enabled = false;
    }
}
