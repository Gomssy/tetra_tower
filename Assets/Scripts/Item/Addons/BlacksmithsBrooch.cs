using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 대장장이의 브롯치
/// 번호: 9
/// </summary>
public class BlacksmithsBrooch : Addon
{
    public override void Declare()
    {
        id = 9; name = "대장장이의 브롯치";
        quality = ItemQuality.Ordinary;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/Coolly Pride"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/Coolly Pride"); ;
        sizeInventory = new Vector2(80, 80);
        addonDescription = "주인을 닮아 까맣고, 둔탁하고 굳세고 영롱해요!";
        addonInfo = "피해량이 55% 증가합니다.";
    }

    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 1.55f;
    }
}
