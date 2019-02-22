using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 모닝스타
/// 번호: 16
/// </summary>
public class Morgenstern : Item
{
    public override void Declare()
    {
        id = 16; name = "모닝스타";
        quality = ItemQuality.Superior;
        skillNum = 2;
        combo = new string[3] { "BBCAA", "BBB", "" };
        attachable = new bool[4] { false, true, false, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/Morgenstern");
        highlight = Resources.Load<Sprite>("Sprites/Items/Morgenstern");
        animation[0] = Resources.Load<AnimationClip>("Animations/morgensternAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/morgensternAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(90, 160);
    }
}
