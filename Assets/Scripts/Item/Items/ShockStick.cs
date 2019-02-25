using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 감전봉
/// 번호: 52
/// </summary>
public class ShockStick : Item {

    public override void Declare()
    {
        id = 52; name = "감전 봉";
        quality = ItemQuality.Masterpiece;
        skillNum = 2;
        combo = new string[3] { "ABA", "CBABB", "" };
        attachable = new bool[4] { false, false, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/shock stick");
        highlight = Resources.Load<Sprite>("Sprites/Items/shock stick_border");
        animation[0] = Resources.Load<AnimationClip>("Animations/shockStickAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/shockStickAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(140f, 140f);
        itemInfo = "몸이 타는 것 같은 수준으로 아프다. 명령을 내릴 때 효과적일 것 같다.";
        comboName = new string[3] { "충전", "방전", "" };
    }
}
