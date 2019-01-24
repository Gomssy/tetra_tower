using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 수습생의 표식
/// 번호: 1
/// </summary>
public class ApprenticesMark : Addon
{
    public override void Declare()
    {
        id = 1; name = "apprentice's mark";
        quality = ItemQuality.Study;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/apprentice's mark"); ;
        sizeInventory = new Vector2(80, 80);
    }
}
