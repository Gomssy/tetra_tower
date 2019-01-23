using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 활
/// 번호: 2
/// </summary>
public class Bow : Item {

	public override void Declare()
    {
        id = 2; name = "bow";
        quality = ItemQuality.Study;
        skillNum = 2;
        combo = new string[3] { "BB", "BC", "" };
        attachable = new bool[4] { true, true, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/bow");
        sizeInventory = new Vector2(90, 160);
    }
}
