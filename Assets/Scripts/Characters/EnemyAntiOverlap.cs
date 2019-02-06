using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAntiOverlap : MonoBehaviour {
    Collider2D myCol;

    public float pushForce;
    private void Start()
    {
        myCol = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Enemy") // Anti-Overlap
        {
            Vector3 pushForceVec = new Vector3(pushForce, 0.0f, 0.0f);
            int pushDirection = (col.bounds.center.x - myCol.bounds.center.x > 0) ? 1 : -1; // right: 1 \ left: -1
            col.GetComponent<Rigidbody2D>().MovePosition(col.transform.position + pushDirection * pushForceVec);
        }
    }
}
