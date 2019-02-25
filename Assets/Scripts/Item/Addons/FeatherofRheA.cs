using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 레아의 깃털
/// 번호: 21
/// </summary>
public class FeatherofRheA : Addon
{
    public override void Declare()
    {
        id = 21; name = "레아의 깃털";
        quality = ItemQuality.Ordinary;
        type = AddonType.Matter;
        sprite = Resources.Load<Sprite>("Sprites/Addons/Glowing Herb"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/Glowing Herb"); ;
        sizeInventory = new Vector2(80, 80);
        addonDescription = "라거 섬에서 쉽게 볼 수 있었던 자유의 상징. 하지만 이제는 흩날리지 않는다.";
        addonInfo = "체공 중에 피해량이 75% 증가합니다.";
            
    }

    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        if (!GameObject.Find("Player").GetComponent<PlayerController>().IsGrounded())
            return 1.75f;
        else
            return 1f;
    }
}
