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

    GameObject[] coolList;

    private void Awake()
    {
        coolList = new GameObject[27];
        for (int i = 0; i < coolList.Length; i++)
        {
            coolList[i] = Instantiate(coolPrefab, transform);
            
            coolList[i].transform.GetChild(0).GetComponent<Image>().color = coolingColor;
            coolList[i].transform.GetChild(0).GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(size, size);
            coolList[i].GetComponent<RectTransform>().localPosition = -startPos - new Vector3(interval.x * (i % maxCol), interval.y * (i / 4));
            coolList[i].SetActive(false);
        }
    }

    private void Update()
    {
        SetCoolOnPosition();
    }

    public void SetCoolOnPosition()
    {
        List<Item> itemList = InventoryManager.Instance.itemList;
        int index = 0;

        foreach (Item item in itemList)
        {
            for (int i = 0; i < item.skillNum; i++)
            {
                if (item.comboCurrentCool[i] < item.comboCool[i])
                {
                    coolList[index].SetActive(true);

                    coolList[index].GetComponent<Image>().sprite = item.coolSprite[i];
                    coolList[index].GetComponent<Image>().enabled = true;
                    coolList[index].GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);

                    coolList[index].transform.GetChild(0).GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(size * (1 - (item.comboCurrentCool[i] / item.comboCool[i])), size);

                    index++;
                }
            }
        }

        for (; index < coolList.Length; index++)
        {
            coolList[index].SetActive(false);
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