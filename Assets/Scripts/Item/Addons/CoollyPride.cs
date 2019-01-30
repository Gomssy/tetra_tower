using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 차갑게 식은 긍지
/// 번호: 10
/// </summary>
public class CoollyPride : Addon
{
    public override void Declare()
    {
        id = 10; name = "Coolly Pride";
        quality = ItemQuality.Ordinary;
        type = AddonType.Theory;
        sprite = Resources.Load<Sprite>("Sprites/Addons/Coolly Pride"); ;
        sizeInventory = new Vector2(80, 80);
    }
}
