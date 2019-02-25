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
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        sizeInventory = new Vector2(80, 80);
        addonDescription = "하루 25분! 의 투자로 최고의 효율을 자랑합니다!";
        addonInfo = "스킬 시전 후 콤보 연계 시간이 50 % 감소합니다.피해량이 100 % 증가합니다.";
    }
    public override void OtherEffect(string combo)
    {
        GameObject.Find("Player").GetComponent<PlayerAttack>().comboTime *= 0.5f;
    }
    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 2f;
    }
}