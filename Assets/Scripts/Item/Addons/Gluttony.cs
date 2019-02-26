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
        sprite = Resources.Load<Sprite>("Sprites/Addons/gluttony"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/gluttony_border"); ;
        sizeInventory = new Vector2(80, 80);
        lifeStoneManager = LifeStoneManager.Instance;
        addonInfo = "적을 처치하면 생명석이 1개 회복된다.";
    }
    public override void OtherEffect(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        if(attackInfo.damage >= enemyInfo.currHealth)
        {
            lifeStoneManager.FillLifeStone(1, LifeStoneType.Normal);
        }
    }
}