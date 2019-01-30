using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 운석검
/// 번호: 32
/// </summary>
public class MeteorSword : Item
{
    public override void Declare()
    {
        id = 32; name = "meteor sword";
        quality = ItemQuality.Superior;
        skillNum = 2;
        combo = new string[3] { "ABAAC", "ABACC", "" };
        attachable = new bool[4] { true, false, false, false };
        sprite = Resources.Load<Sprite>("Sprites/Items/meteor sword");
        sizeInventory = new Vector2(90, 160);
    }
}
