using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 기름 통
/// 번호: 36
/// </summary>
public class OilCask : Item
{

    public override void Declare()
    {
        id = 36; name = "기름 통";
        quality = ItemQuality.Ordinary;
        skillNum = 0;
        combo = new string[3] { "", "", "" };
        attachable = new bool[4] { false, false, false, false };
        sprite = Resources.Load<Sprite>("Sprites/Items/oil cask");
        highlight = Resources.Load<Sprite>("Sprites/Items/oil cask_border");
        animation[0] = null;
        animation[1] = null;
        animation[2] = null;
        sizeInventory = new Vector2(105f, 127.5f);
        itemInfo = "";
        comboName = new string[3] { "", "", "" };
    }

    public override float GlobalFireDamageMultiplier()
    {
        return 2f;
    }
}
