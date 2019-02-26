using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 백익 부채
/// 번호: 37
/// </summary>
public class FeatherFan : Item
{

    public override void Declare()
    {
        id = 37; name = "백익 부채";
        quality = ItemQuality.Superior;
        skillNum = 0;
        combo = new string[3] { "", "", "" };
        attachable = new bool[4] { false, false, false, false };
        sprite = Resources.Load<Sprite>("Sprites/Items/feather fan");
        highlight = Resources.Load<Sprite>("Sprites/Items/feather fan_border");
        animation[0] = null;
        animation[1] = null;
        animation[2] = null;
        sizeInventory = new Vector2(140f, 140f);
        itemInfo = "적을 밀어내는 거리가 900% 증가한다.";
        comboName = new string[3] { "", "", "" };
    }

    public override float GlobalKnockBackMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 10f;
    }
}
