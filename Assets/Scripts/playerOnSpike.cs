using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class playerOnSpike : MonoBehaviour {
    public TileBase[] spikeTile;
    public Rigidbody2D rb;
    public int SpikeLayer = 10; //10 is for spike layer
    public float spikeDamage = 5f;
    public float spikeKnockBackx = 5f;
    public float spikeKnockBacky = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == SpikeLayer) 
        {
            Tilemap t = coll.gameObject.GetComponent<Tilemap>();
            Vector3 hitPosition = Vector3.zero;
            ContactPoint2D hit = coll.contacts[0];
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
           //충돌 위치 중심 계산


            TileBase colTile = t.GetTile(t.WorldToCell(hitPosition));

            if (colTile != null)
            {
                EnemyAttackInfo attack = new EnemyAttackInfo(spikeDamage, 0f, 0, null, null); //넉백은 따로 구현
                GetComponent<PlayerAttack>().TakeDamage(attack); 
                 
                //방향에 따른 넉백 구현
                if (colTile == spikeTile[0]) //spike up tile
                {
                   // print("uuch!");
                    rb.velocity = new Vector2(rb.velocity.x, spikeKnockBacky);
                }
                else if (colTile == spikeTile[1]) //spike down tile
                {
                   // print("duch!");
                    rb.velocity = new Vector2(rb.velocity.x, -spikeKnockBacky);
                }
                if (colTile == spikeTile[2]) //spike l tile
                {
                   // print("luch!");
                    rb.velocity = new Vector2( -spikeKnockBackx, rb.velocity.y);
                }
                else if (colTile == spikeTile[3]) //spike r tile
                {
                   // print("ruch!");
                    rb.velocity = new Vector2( spikeKnockBackx, rb.velocity.y);
                }
            }
        }

    }
}
