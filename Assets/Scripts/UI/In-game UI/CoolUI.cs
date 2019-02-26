using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolUI : MonoBehaviour {

    public Vector3 startPos;
    public Vector2 interval;
    public int maxCol;
    public GameObject coolPrefab;
    public float size;
    public Color coolingColor;

    List<CoolUIUnit> coolList;

    private void Awake()
    {
        coolList = new List<CoolUIUnit>();
    }

    private void Update()
    {
        SetCoolOnPosition();
    }

    public void InitCool(Item item, int skillNo)
    {
        CoolUIUnit tmpUnit = new CoolUIUnit(Instantiate(coolPrefab, transform), item, skillNo);
        tmpUnit.obj.GetComponent<Image>().sprite = item.coolSprite[skillNo];
        tmpUnit.obj.GetComponent<Image>().enabled = true;
        tmpUnit.obj.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);

        tmpUnit.obj.transform.GetChild(0).GetComponent<Image>().color = coolingColor;
        tmpUnit.obj.transform.GetChild(0).GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(size, size);
        coolList.Add(tmpUnit);
    }

    public void SetCoolOnPosition()
    {
        for(int i=0; i<coolList.Count; i++)
        {
            if (coolList[i].item.comboCurrentCool[coolList[i].skillNo] >= coolList[i].item.comboCool[coolList[i].skillNo])
            {
                Destroy(coolList[i].obj);
                coolList.RemoveAt(i);
                continue;
            }
            Debug.Log(coolList[i].item.comboCurrentCool[coolList[i].skillNo]);
            coolList[i].obj.GetComponent<RectTransform>().localPosition =  - startPos - new Vector3(interval.x * (i % maxCol), interval.y * (i / 4));
            coolList[i].obj.transform.GetChild(0).GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(size * (1-(coolList[i].item.comboCurrentCool[coolList[i].skillNo] / coolList[i].item.comboCool[coolList[i].skillNo])), size);
        }
    }
}

class CoolUIUnit
{
    public GameObject obj;
    public Item item;
    public int skillNo;

    public CoolUIUnit(GameObject obj, Item item, int skillNo)
    {
        this.obj = obj; this.item = item;   this.skillNo = skillNo;
    }
}