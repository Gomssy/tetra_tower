using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedLifeStoneFrame : DroppedObject, IPlayerInteraction
{
    int rowNum;
    public GameObject[] frames;
    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    GameObject[] obj;
    public void Init(int rowNum, Vector3 pos)
    {
        transform.position = pos;
        this.rowNum = rowNum;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        obj = new GameObject[rowNum + 2];

        obj[0] = Instantiate(frames[0], transform.position + new Vector3(0, frames[0].GetComponent<SpriteRenderer>().bounds.extents.y,0) , Quaternion.identity ,transform);

        for(int i = 0; i<rowNum; i++)
        {
            obj[i+1] = Instantiate(frames[1], transform.position + new Vector3(0, frames[0].GetComponent<SpriteRenderer>().bounds.extents.y * 2 + frames[1].GetComponent<SpriteRenderer>().bounds.extents.y * (i * 2 + 1), 0), Quaternion.identity, transform);
        }
        obj[rowNum + 1] = Instantiate(frames[2], transform.position + new Vector3(0, frames[0].GetComponent<SpriteRenderer>().bounds.extents.y * 2 + frames[1].GetComponent<SpriteRenderer>().bounds.extents.y * rowNum * 2 + frames[2].GetComponent<SpriteRenderer>().bounds.extents.y, 0), Quaternion.identity, transform);


        bc2D.size = new Vector2(frames[0].GetComponent<SpriteRenderer>().bounds.extents.y * 2, obj[rowNum + 1].transform.position.y - obj[0].transform.position.y + frames[0].GetComponent<SpriteRenderer>().bounds.extents.y + frames[2].GetComponent<SpriteRenderer>().bounds.extents.y);
        bc2D.offset = new Vector2(0, bc2D.size.y / 2f);
    }
    public void Apply()
    {
        LifeStoneManager.Instance.ExpandRow(rowNum);
        Destroy(gameObject);
    }
    public void HighlightSwitch(bool enabled)
    {
    }
}