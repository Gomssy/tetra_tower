using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {

    // data for ignoring collision
    Vector2 lastPosition;
    Vector2 lastVelocity;
    float lastAngularVelocity;

	// Update is called once per frame
	void FixedUpdate () {
        lastPosition = transform.position;
        lastVelocity = GetComponent<Rigidbody2D>().velocity;
        lastAngularVelocity = GetComponent<Rigidbody2D>().angularVelocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            transform.position = lastPosition;
            GetComponent<Rigidbody2D>().velocity = lastVelocity;
            GetComponent<Rigidbody2D>().angularVelocity = lastAngularVelocity;
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
