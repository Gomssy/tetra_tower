using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 감전봉
/// 번호: 52
/// </summary>
public class ElectricShockRod : Item {

    public override void Declare()
    {
        id = 52; name = "감전봉";
        quality = ItemQuality.Masterpiece;
        skillNum = 2;
        combo = new string[3] { "ABA", "CBABB", "" };
        attachable = new bool[4] { false, false, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/electric shock rod");
        highlight = Resources.Load<Sprite>("Sprites/Items/electric shock rod");
        animation[0] = Resources.Load<AnimationClip>("Animations/electricShockRodAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/electricShockRodAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(90, 160);
        itemInfo = "몸이 타는 것 같은 수준으로 아프다. 명령을 내릴 때 효과적일 것 같다.";
        comboName = new string[3] { "충전", "방전", "" };
    }
}
