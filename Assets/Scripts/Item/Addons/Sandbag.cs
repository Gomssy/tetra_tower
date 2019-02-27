using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 모래 주머니
/// 번호: 29
/// </summary>
public class Sandbag : Addon
{
    public override void Declare()
    {
        id = 29; name = "모래 주머니";
        quality = ItemQuality.Superior;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/sandbag"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/sandbag_border"); ;
        sizeInventory = new Vector2(80, 80);
        addonInfo = "직후의 연계 시간이 50% 감소하지만, 피해량이 150% 증가한다.";
    }
    public override void OtherEffect(string combo)
    {
        GameManager.Instance.player.GetComponent<PlayerAttack>().comboTime *= 0.5f;
    }
    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 2.5f;
    }
}