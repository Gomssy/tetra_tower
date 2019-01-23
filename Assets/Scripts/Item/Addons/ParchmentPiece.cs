using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 양피지 조각
/// 번호: 1
/// </summary>
public class ParchmentPiece : Addon
{
    public override void Declare()
    {
        id = 0; name = "parchment piece";
        quality = ItemQuality.Study;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        sizeInventory = new Vector2(80, 80);
    }
}