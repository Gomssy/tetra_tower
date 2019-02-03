using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 이글거리는 약초
/// 번호: 14
/// </summary>
public class GlowingHerb : Addon
{
    public override void Declare()
    {
        id = 14; name = "Glowing Herb";
        quality = ItemQuality.Ordinary;
        type = AddonType.Matter;
        sprite = Resources.Load<Sprite>("Sprites/Addons/Glowing Herb"); ;
        sizeInventory = new Vector2(80, 80);
    }
}
