﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {
    public int id;
    public string name;
    public ItemQuality quality;
    public int skillNum;
    public string[] combo = new string[3];  //Capital Letters A B C
    public AnimationClip[] animation = new AnimationClip[3];
    public bool[] attachable = new bool[4]; //0: prop 1: matter 2: component 3:theory
    public Addon[] addons = new Addon[4];   //0: prop 1: matter 2: component 3:theory
    public Sprite sprite;
    public Sprite highlight;
    public Vector2 sizeInventory;

    public bool ComboAction(string currentCombo)
    {
        for(int i=0; i<skillNum; i++)
        {
            if (combo[i].Equals(currentCombo))
            {
                if (i == 0) PlaySkill1();
                else if (i == 1) PlaySkill2();
                else if (i == 2) PlaySkill3();
                return true;
            }
        }
        return false;
    }
    public bool ComboAction(int currenSkill)
    {
        if (currenSkill == 0) PlaySkill1();
        else if (currenSkill == 1) PlaySkill2();
        else if (currenSkill == 2) PlaySkill3();
        return true;
    }
    public Item()
    {
        Declare();
    }
    public virtual void Declare()
    {
        id = 0; name = "itemname";
        quality = ItemQuality.Study;
        skillNum = 0;
        combo = new string[3] { "", "", "" };
        attachable = new bool[4] { false, false, false, false };
        sprite = null;
        highlight = null;
        animation[0] = null;
        animation[1] = null;
        animation[2] = null;
        sizeInventory = new Vector2(0, 0);
    }
    protected virtual void PlaySkill1()
    {
    }
    protected virtual void PlaySkill2()
    {
    }
    protected virtual void PlaySkill3()
    {
    }

    public virtual void AttackCalculation(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        PlayerAttackInfo originInfo = new PlayerAttackInfo(attackInfo);
        float[] tmpArray;

        attackInfo.damage += DamageAdder(originInfo, enemyInfo, combo);
        attackInfo.knockBackMultiplier += KnockBackAdder(originInfo, enemyInfo, combo);
        tmpArray = DebuffAdder(originInfo, enemyInfo, combo);
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++)
            attackInfo.debuffTime[i] += tmpArray[i];

        for (int j = 0; j < attachable.Length; j++)
        {
            if (attachable[j] && addons[j] != null)
            {
                attackInfo.damage += addons[j].DamageAdder(originInfo, enemyInfo, combo);
                attackInfo.knockBackMultiplier += addons[j].KnockBackAdder(originInfo, enemyInfo, combo);
                tmpArray = addons[j].DebuffAdder(originInfo, enemyInfo, combo);
                for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++)
                    attackInfo.debuffTime[i] += tmpArray[i];
            }
        }

        attackInfo.damage *= DamageMultiplier(originInfo, enemyInfo, combo);
        attackInfo.knockBackMultiplier *= KnockBackMultiplier(originInfo, enemyInfo, combo);
        tmpArray = DebuffMultiplier(originInfo, enemyInfo, combo);
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++)
            attackInfo.debuffTime[i] *= tmpArray[i];

        for (int j = 0; j < attachable.Length; j++)
        {
            if (attachable[j] && addons[j] != null)
            {
                attackInfo.damage *= addons[j].DamageMultiplier(originInfo, enemyInfo, combo);
                attackInfo.knockBackMultiplier *= addons[j].KnockBackMultiplier(originInfo, enemyInfo, combo);
                tmpArray = addons[j].DebuffMultiplier(originInfo, enemyInfo, combo);
                for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++)
                    attackInfo.debuffTime[i] *= tmpArray[i];
            }
        }
    }
    public virtual float DamageAdder(PlayerAttackInfo attackInfo, Enemy enemInfo, string combo)
    {
        return 0f;
    }
    public virtual float DamageMultiplier(PlayerAttackInfo attackInfo, Enemy enemInfo, string combo)
    {
        return 1f;
    }
    public virtual float[] DebuffAdder(PlayerAttackInfo attackInfo, Enemy enemInfo, string combo)
    {
        float[] varArray = new float[(int)EnemyDebuffCase.END_POINTER];
        for (int i = 0; i< (int)EnemyDebuffCase.END_POINTER; i++) varArray[i] = 0f;

        return varArray;
    }
    public virtual float[] DebuffMultiplier(PlayerAttackInfo attackInfo, Enemy enemInfo, string combo)
    {
        float[] varArray = new float[(int)EnemyDebuffCase.END_POINTER];
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++) varArray[i] = 1f;

        return varArray;
    }
    public virtual float KnockBackAdder(PlayerAttackInfo attackInfo, Enemy enemInfo, string combo)
    {
        return 0f;
    }
    public virtual float KnockBackMultiplier(PlayerAttackInfo attackInfo, Enemy enemInfo, string combo)
    {
        return 1f;
    }
}
