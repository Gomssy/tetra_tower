using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedLifeStone : MonoBehaviour
{
    LifeStoneInfo info;
    public Sprite[] sprites;
    public GameObject unitSprite, highlightSprite;
    public LayerMask playerLayer;
    float unitSize;
    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    /// <summary>
    /// unitSprite Objects
    /// </summary>
    GameObject[] unitObj;
    /// <summary>
    /// highlightSprite Objects
    /// </summary>
    GameObject[] highObj;

    public void Init(LifeStoneInfo _info, Vector3 pos)
    {
        info = _info;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        unitSize = unitSprite.GetComponent<SpriteRenderer>().bounds.size.x;
        Vector2Int inSize = info.getSize();
        string inFill = info.getFill();

        unitObj = new GameObject[inSize.x * inSize.y];
        highObj = new GameObject[inSize.x * inSize.y];

        transform.position = pos - new Vector3(inSize.x * unitSize, 0, 0);

        for (int i = 0; i < inSize.x * inSize.y; i++)
        {
            if (inFill[i] != ' ')
            {
                unitObj[i] = Instantiate(unitSprite, transform);
                unitObj[i].transform.localPosition = new Vector3((i % inSize.x) * unitSize, ((int)(i / inSize.x)) * unitSize, 0);
                unitObj[i].GetComponent<SpriteRenderer>().sprite = sprites[inFill[i] - 'A'];

                highObj[i] = Instantiate(highlightSprite, transform);
                highObj[i].transform.localPosition = unitObj[i].transform.localPosition;
                highObj[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        bc2D.offset = new Vector2(unitSize * inSize.x / 2f, unitSize * inSize.y / 2f);
        bc2D.size = new Vector2(unitSize * inSize.x, unitSize * inSize.y);
    }
    public void ApplyLifeStone()
    {
        GameObject.Find("LifeStoneUI").GetComponent<LifeStoneManager>().PushLifeStone(info);
        Destroy(gameObject);
    }
    public void HighlightSwitch(bool enabled)
    {
        Vector2Int inSize = info.getSize();
        string inFill = info.getFill();
        for (int i = 0; i < inSize.x * inSize.y; i++)
        {
            if (inFill[i] != ' ')
                highObj[i].GetComponent<SpriteRenderer>().enabled = enabled;
        }
    }
}