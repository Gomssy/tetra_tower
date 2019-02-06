using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class LifeStoneTest : MonoBehaviour {

    public GameObject numText, ameNumText, goldPerText, goldText, amethystText, destroyText, rowText;
    public GameObject ui;
    LifeStoneManager uii;

    private void Start()
    {
        uii = ui.GetComponent<LifeStoneManager>();
    }

    public void push()
    {
        uii.InstantiateDroppedLifeStone(Convert.ToInt32(numText.GetComponent<InputField>().text), Convert.ToSingle(goldPerText.GetComponent<InputField>().text), Convert.ToInt32(ameNumText.GetComponent<InputField>().text),GameObject.Find("Player").transform.position);
    }
    public void gold()
    {
        int tmp = Convert.ToInt32(goldText.GetComponent<InputField>().text);
        if (tmp > 0) uii.ChangeFromNormal(LifeStoneType.Gold, tmp);
        else if (tmp < 0) uii.ChangeToNormal(LifeStoneType.Gold, -tmp);
    }
    public void amethyst()
    {
        int tmp = Convert.ToInt32(amethystText.GetComponent<InputField>().text);
        if (tmp > 0) uii.ChangeFromNormal(LifeStoneType.Amethyst, tmp);
        else if (tmp < 0) uii.ChangeToNormal(LifeStoneType.Amethyst, -tmp);
    }
    public void dest()
    {
        uii.DestroyStone(Convert.ToInt32(destroyText.GetComponent<InputField>().text));
    }
    public void row()
    {
        uii.ExpandRow(Convert.ToInt32(rowText.GetComponent<InputField>().text));
    }
    public void goldPotion()
    {
        uii.InstantiatePotion(GameObject.Find("Player").transform.position);
    }
}
