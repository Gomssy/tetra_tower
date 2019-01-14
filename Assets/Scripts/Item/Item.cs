using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {
    public int id;
    public ItemQuality quality;
    public int skillNum;
    public string[] combo = new string[3];  //Capital Letters A B C
    public bool[] attachable = new bool[4]; //0: prop 1: matter 2: component 3:theory
    public Addon[] addons = new Addon[4];   //0: prop 1: matter 2: component 3:theory
    public Sprite sprite;

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

    protected void PlaySkill1()
    {
    }
    protected void PlaySkill2()
    {
    }
    protected void PlaySkill3()
    {
    }

}
