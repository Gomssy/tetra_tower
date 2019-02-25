using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 야누스의 동전
/// 번호: 24
/// </summary>
public class JanusCoin : Addon
{
    public override void Declare()
    {
        id = 24; name = "야누스의 동전";
        quality = ItemQuality.Superior;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        sizeInventory = new Vector2(80, 80);
        addonDescription = "결국 모든 확률은 50%. 되냐, 안 되냐 뿐.";
        addonInfo = "타격 시 50% 확률로 2 피해를 입고 50% 확률로 적의 현재 체력의 25% 만큼의 추가 피해를 줍니다.";
    }
    public override float DamageFinalAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        LifeStoneManager lifeStoneManager = LifeStoneManager.Instance;
        if(Random.Range(0,2) == 0)
        {
            lifeStoneManager.DestroyStone(2);
            return 0;
        }
        else
        {
            return enemyInfo.currHealth * 0.25f;
        }
    }
}