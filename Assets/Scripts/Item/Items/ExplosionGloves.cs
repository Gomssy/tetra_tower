using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 폭탄마의 장갑
/// 번호: 21
/// </summary>
public class ExplosionGloves : Item
{
    public override void Declare()
    {
        id = 21; name = "폭탄마의 장갑";
        quality = ItemQuality.Ordinary;
        skillNum = 2;
        combo = new string[3] { "CAC", "CA", "" };
        attachable = new bool[4] { true, false, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/explosion gloves");
        highlight = Resources.Load<Sprite>("Sprites/Items/explosion gloves");
        animation[0] = Resources.Load<AnimationClip>("Animations/explosionGlovesAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/explosionGlovesAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(90, 160);
    }
}
