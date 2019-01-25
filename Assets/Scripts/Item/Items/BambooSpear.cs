using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 죽창
/// 번호: 26
/// </summary>
public class BambooSpear : Item
{
    public override void Declare()
    {
        id = 26; name = "bamboo spear";
        quality = ItemQuality.Ordinary;
        skillNum = 2;
        combo = new string[3] { "BAA", "BAC", "" };
        attachable = new bool[4] { true, false, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/bamboo spear");
        animation[0] = Resources.Load<AnimationClip>("Animations/bambooSpearAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/bambooSpearAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(90, 160);
    }
}
