using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Addon {
    public int id;
    public string name;
    public ItemQuality quality;
    public AddonType type;
    public Sprite sprite;
    public Sprite highlight;
    public Vector2 sizeInventory;
    public string addonDescription;

    public Addon()
    {
        Declare();
    }
    public virtual void Declare()
    {
        id = 0; name = "itemname";
        quality = ItemQuality.Study;
        type = AddonType.Prop;
        sprite = null;
        highlight = null;
        sizeInventory = new Vector2(0, 0);
        addonDescription = null;
    }
    public virtual float DamageAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 0f;
    }
    public virtual float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 1f;
    }
    public virtual float DamageFinalAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 0f;
    }
    public virtual float[] DebuffAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        float[] varArray = new float[(int)EnemyDebuffCase.END_POINTER];
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++) varArray[i] = 0f;

        return varArray;
    }
    public virtual float[] DebuffMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        float[] varArray = new float[(int)EnemyDebuffCase.END_POINTER];
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++) varArray[i] = 1f;

        return varArray;
    }
    public virtual float[] DebuffFinalAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        float[] varArray = new float[(int)EnemyDebuffCase.END_POINTER];
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++) varArray[i] = 0f;

        return varArray;
    }
    public virtual float KnockBackAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 0f;
    }
    public virtual float KnockBackMultiplier(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 1f;
    }
    public virtual float KnockBackFinalAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        return 0f;
    }
    public virtual void OtherEffect(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {

    }
    public virtual void OtherEffect(string combo)
    {

    }
}
