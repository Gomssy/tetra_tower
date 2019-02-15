using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 시간 톱니바퀴
/// 번호: 32
/// </summary>
public class ToothedWheelofTime : Addon
{
    public override void Declare()
    {
        id = 32; name = "시간 톱니바퀴";
        quality = ItemQuality.Superior;
        type = AddonType.Component;
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        sizeInventory = new Vector2(80, 80);
    }
    
}