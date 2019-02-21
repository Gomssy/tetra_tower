using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 이글거리는 약초
/// 번호: 14
/// </summary>
public class GlowingHerb : Addon
{
    public override void Declare()
    {
        id = 14; name = "이글거리는 약초";
        quality = ItemQuality.Ordinary;
        type = AddonType.Matter;
        sprite = Resources.Load<Sprite>("Sprites/Addons/Glowing Herb"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/Glowing Herb"); ;
        sizeInventory = new Vector2(80, 80);
        addonDescription = "활활 타오르며 매콤한 맛이 날 것 같지만 평범하게 쓰다. - 하부 료진";
        addonInfo = "타격시 적을 3초간 화상 상태로 만듭니다.";
    }

    public override float[] DebuffAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        float[] varArray = new float[(int)EnemyDebuffCase.END_POINTER];
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++) varArray[i] = 0f;

        varArray[(int)EnemyDebuffCase.Fire] = 3f;

        return varArray;
    }
}
