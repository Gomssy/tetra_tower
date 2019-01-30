using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 사슬낫
/// 번호: 3
/// </summary>
public class ChainSickle : Item
{
    public override void Declare()
    {
        id = 3; name = "chain sickle";
        quality = ItemQuality.Ordinary;
        skillNum = 2;
        combo = new string[3] { "AAB", "AABC", "" };
        attachable = new bool[4] { true, false, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/chain sickle");
        sizeInventory = new Vector2(90, 160);
    }
}
