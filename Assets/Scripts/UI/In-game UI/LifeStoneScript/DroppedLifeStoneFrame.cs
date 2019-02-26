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
    public void Init(int rowNum, Vector3 pos)
    {
        this.rowNum = rowNum;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();


    }
    public void Apply()
    {
        LifeStoneManager.Instance.ExpandRow(rowNum);
    }
    public void HighlightSwitch(bool enabled)
    {
    }
}