using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParticle : MonoBehaviour {

    Vector3 spawn;

    public void Init(Vector3 pos)
    {
        spawn = pos;
        shoot();
    }

    void shoot()
    {
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();

        transform.position = spawn;
        rb2D.velocity = new Vector2(0, Random.Range(-5f, 3f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        shoot();
    }
}
