using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 명석이
/// 번호: 47
/// </summary>
public class Festo : Item
{
    public override void Declare()
    {
        id = 47; name = "Festo";
        quality = ItemQuality.Ordinary;
        skillNum = 0;
        combo = new string[3] { "", "", "" };
        attachable = new bool[4] { true, true, false, false };
        sprite = Resources.Load<Sprite>("Sprites/Items/Festo");
        sizeInventory = new Vector2(90, 160);
    }
}
