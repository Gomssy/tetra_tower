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
    }

    public override float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        if (!GameObject.Find("Player").GetComponent<PlayerController>().IsGrounded())
            return 1.75f;
        else
            return 1f;
    }
}
