using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 활
/// 번호: 7
/// </summary>
public class Ksanife : Item
{
    public override void Declare()
    {
        id = 2; name = "ksanife";
        quality = ItemQuality.Superior;
        skillNum = 2;
        combo = new string[3] { "A", "AABAA", "" };
        attachable = new bool[4] { true, true, false, false };
        sprite = Resources.Load<Sprite>("Sprites/Items/ksanife");
        highlight = Resources.Load<Sprite>("Sprites/Items/ksanife");
        animation[0] = Resources.Load<AnimationClip>("Animations/ksanifeAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/ksanifeAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(160, 160);
    }
}
