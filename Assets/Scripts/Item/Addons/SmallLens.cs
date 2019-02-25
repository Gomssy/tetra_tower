using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 소형 렌즈
/// 번호: 18
/// </summary>
public class SmallLens : Addon
{
    public override void Declare()
    {
        id = 18; name = "소형 렌즈";
        quality = ItemQuality.Ordinary;
        type = AddonType.Component;
        sprite = Resources.Load<Sprite>("Sprites/Addons/small lens");
        highlight = Resources.Load<Sprite>("Sprites/Addons/small lens_border");
        sizeInventory = new Vector2(72.5f, 77.5f);
    }

    public override void OtherEffect(string combo)
    {
        GameManager.Instance.player.GetComponent<PlayerAttack>().comboTime *= 1.75f;
    }
}