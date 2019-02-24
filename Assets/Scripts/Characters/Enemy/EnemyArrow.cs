using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour {

    Enemy enemy;
    public float moveSpeed;
    public float damage = 0;
    public float knockBackMultiplier = 1f;
    public LayerMask vanishLayer;
    public LayerMask stopLayer;
    Rigidbody2D rb;
    Player target;
    Vector2 moveDirection;
    Vector2 changeAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindObjectOfType<Player>();
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }

   /* void FixedUpdate()
    {
        Vector2 dir = rb.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            EnemyAttackInfo attack = new EnemyAttackInfo(damage, knockBackMultiplier, 0, null, null);
            collision.gameObject.GetComponent<PlayerAttack>().TakeDamage(attack);
        }

        if ((vanishLayer == (vanishLayer | 1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
        
        if (stopLayer == (stopLayer | 1 << collision.gameObject.layer))
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

