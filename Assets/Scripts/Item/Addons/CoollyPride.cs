using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 차갑게 식은 긍지
/// 번호: 10
/// </summary>
public class CoollyPride : Addon
{
    public override void Declare()
    {
        id = 10; name = "차갑게 식은 긍지";
        quality = ItemQuality.Ordinary;
        type = AddonType.Theory;
        sprite = Resources.Load<Sprite>("Sprites/Addons/Coolly Pride"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/Coolly Pride"); ;
        sizeInventory = new Vector2(80, 80);
        addonDescription = "꺼지지 않을 것처럼 타올랐지만, 이제는 완전히 식어버린 료의 긍지.";
        addonInfo = "기본 피해량이 4 미만이면 적을 2초간 빙결시킵니다.";
    }

    public override float[] DebuffAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        float[] varArray = new float[(int)EnemyDebuffCase.END_POINTER];
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++) varArray[i] = 0f;

        if (attackInfo.damage < 4) varArray[(int)EnemyDebuffCase.Ice] = 2f;

        return varArray;
    }
}
