using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 단검
/// 번호: 1
/// </summary>
public class Dagger : Item
{
    public override void Declare()
    {
        id = 1; name = "단검";
        quality = ItemQuality.Study;
        skillNum = 2;
        combo = new string[3] { "A", "AA", "" };
        attachable = new bool[4] { true, true, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/dagger");
        highlight = Resources.Load<Sprite>("Sprites/Items/dagger_border");
        animation[0] = Resources.Load<AnimationClip>("Animations/daggerAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/daggerAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(127.5f, 125);
        itemInfo = "옛날 옛적 호랑이 담배 피던 시절부터 존재하던 단검이다.";
        comboName = new string[3] { "베기", "찌르기", "" };
    }
}
