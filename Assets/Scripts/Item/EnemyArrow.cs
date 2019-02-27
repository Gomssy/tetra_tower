using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{

    EnemyGround enemy;
    public float moveSpeed;
    public float damage = 0;
    public float knockBackMultiplier = 1f;
    public LayerMask vanishLayer;
    public LayerMask stopLayer;
    Rigidbody2D rb;
    Player target;
    Vector3 moveDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindObjectOfType<Player>();
        moveDirection = transform.rotation.eulerAngles; 
        rb.velocity = -moveSpeed *new Vector2(Mathf.Cos(Mathf.Deg2Rad * moveDirection.z), Mathf.Sin(Mathf.Deg2Rad * moveDirection.z));
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

