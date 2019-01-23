using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {
    public int id;
    public string name;
    public ItemQuality quality;
    public int skillNum;
    public string[] combo = new string[3];  //Capital Letters A B C
    public bool[] attachable = new bool[4]; //0: prop 1: matter 2: component 3:theory
    public Addon[] addons = new Addon[4];   //0: prop 1: matter 2: component 3:theory
    public Sprite sprite;
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

}
