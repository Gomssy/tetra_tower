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
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        sizeInventory = new Vector2(80, 80);
        addonDescription = "이게 보기보다 쓸모 있더라구요";
        addonInfo = "스킬 시전 후 콤보 연계 시간이 75% 증가합니다.";
    }

    public override void OtherEffect(string combo)
    {
        GameObject.Find("Player").GetComponent<PlayerAttack>().comboTime *= 1.75f;
    }
}