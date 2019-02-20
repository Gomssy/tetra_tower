using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 식탐
/// 번호: 17
/// </summary>
public class Gluttony : Addon
{
    LifeStoneManager lifeStoneManager;
    public override void Declare()
    {
        id = 17; name = "식탐";
        quality = ItemQuality.Ordinary;
        type = AddonType.Theory;
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        sizeInventory = new Vector2(80, 80);
        lifeStoneManager = LifeStoneManager.Instance;
        addonDescription = "식욕은 누구에게나 존재한다. - 토미 다라바.";
        addonInfo = "적을 처치하면 생명석이 1개 회복됩니다.";
    }
    public override void OtherEffect(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        if(attackInfo.damage >= enemyInfo.currHealth)
        {
            lifeStoneManager.FillLifeStone(1, LifeStoneType.Normal);
        }
    }
}