using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 서리방패
/// 번호: 27
/// </summary>
public class FrostShield : Item
{
    public override void Declare()
    {
        id = 27; name = "FrostSheild";
        quality = ItemQuality.Ordinary;
        skillNum = 1;
        combo = new string[3] { "C", "", "" };
        attachable = new bool[4] { true, true, true, false };
        sprite = Resources.Load<Sprite>("Sprites/Items/frost shield");
        sizeInventory = new Vector2(90, 160);
    }
}
