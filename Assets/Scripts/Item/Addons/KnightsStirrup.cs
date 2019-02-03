using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 기사의 등자
/// 번호: 8
/// </summary>
public class KnightsStirrup : Addon
{
    public override void Declare()
    {
        id = 8; name = "Knight's stirrup";
        quality = ItemQuality.Study;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/Knight's stirrup"); ;
        sizeInventory = new Vector2(80, 80);
    }
}
