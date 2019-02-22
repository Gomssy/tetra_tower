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
        id = 15; name = "바쿨루스";
        quality = ItemQuality.Ordinary;
        skillNum = 2;
        combo = new string[3] { "BCB", "ACBC", "" };
        attachable = new bool[4] { true, true, false, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/Baculus");
        highlight = Resources.Load<Sprite>("Sprites/Items/Baculus");
        animation[0] = Resources.Load<AnimationClip>("Animations/baculusAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/baculusAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(90, 160);
        itemInfo = "신을 숭배하는 자들이 들고 있던 청백색의 주교 지팡이. 영험한 힘이 느껴진다.";
        comboName = new string[3] { "기도", "징벌", "" };
    }
}
