﻿using System.Collections;
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
        id = 1; name = "dagger";
        quality = ItemQuality.Study;
        skillNum = 2;
        combo = new string[3] { "A", "AA", "" };
        attachable = new bool[4] { true, true, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/dagger");
        highlight = Resources.Load<Sprite>("Sprites/Items/dagger_border");
        animation[0] = Resources.Load<AnimationClip>("Animations/chainSickleAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/chainSickleAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(127.5f, 125);
    }
}