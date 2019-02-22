using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class playerOnSpike : MonoBehaviour {
    public TileBase[] spikeTileu;
    public TileBase[] spikeTiled;
    public TileBase[] spikeTilel;
    public TileBase[] spikeTiler;
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
                foreach (TileBase tile in spikeTileu) {
                    if (colTile == tile) //spike up tile
                    {
                        // print("uuch!");
                        rb.velocity = new Vector2(rb.velocity.x, spikeKnockBacky);
                    }
                }
                foreach (TileBase tile in spikeTiled)
                {
                    if (colTile == tile) //spike down tile
                    {
                        // print("duch!");
                        rb.velocity = new Vector2(rb.velocity.x, -spikeKnockBacky);
                    }
                }
                foreach (TileBase tile in spikeTilel)
                {
                    if (colTile == tile) //spike l tile
                    {
                        // print("luch!");
                        rb.velocity = new Vector2(-spikeKnockBackx, rb.velocity.y);
                    }
                }
                foreach (TileBase tile in spikeTiler)
                {
                    if (colTile == tile) //spike r tile
                    {
                        // print("ruch!");
                        rb.velocity = new Vector2(spikeKnockBackx, rb.velocity.y);
                    }
                }
            }
        }

    }
}
