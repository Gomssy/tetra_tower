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
        sprite = Resources.Load<Sprite>("Sprites/Addons/feather of rhea"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/feather of rhea_border"); ;
        sizeInventory = new Vector2(77.5f, 77.5f);
        addonInfo = "체공 중에 공격할 때, 피해량이 75% 증가한다.";
    }

    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        if (!GameManager.Instance.player.GetComponent<PlayerController>().IsGrounded())
            return 1.75f;
        else
            return 1f;
    }
}
