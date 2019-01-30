using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 바쿨루스
/// 번호: 15
/// </summary>
public class Baculus : Item
{
    public override void Declare()
    {
        id = 15; name = "Baculus";
        quality = ItemQuality.Ordinary;
        skillNum = 2;
        combo = new string[3] { "BCB", "ACBC", "" };
        attachable = new bool[4] { true, true, false, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/Baculus");
        sizeInventory = new Vector2(90, 160);
    }
}
