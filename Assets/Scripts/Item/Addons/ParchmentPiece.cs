using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 양피지 조각
/// 번호: 2
/// </summary>
public class ParchmentPiece : Addon
{
    public override void Declare()
    {
        id = 2; name = "양피지 조각";
        quality = ItemQuality.Study;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/parchment piece_border"); ;
        sizeInventory = new Vector2(70, 77.5f);
        addonInfo = "피해량이 3 미만이면 피해량이 100% 증가한다.";
    }
    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        if (attackInfo.damage < 3) return 2f;
        else return 1f;
    }
}