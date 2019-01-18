using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public GameObject[] itemCell;
    public GameObject[] addonCell;
    public GameObject infoSpace;
    public GameObject[] infoAddonSpace;
    public GameObject itemPrefab;
    public GameObject addonPrefab;
    public GameObject infoAddonPrefab;
    /// <summary>
    /// index: addonType * 2 + a
    /// a{
    /// 0: when addon can't be attached
    /// 1: when addon can be attached
    /// }
    /// </summary>
    public Sprite[] addonLockSprite = new Sprite[8];
    /// <summary>
    /// index: addonType * 4 + addonQuaility
    /// </summary>
    public Sprite[] addonFrameQuality = new Sprite[16];
    public Sprite[] itemFrameQuality = new Sprite[4];
    public Sprite[] infoFrameQuality = new Sprite[4];
    GameObject[] items;
    GameObject[] addons;
    GameObject[] infoAddons;
    int selectedItem = -1;

	void Start () {
        items = new GameObject[9];
        for (int i = 0; i < 9; i++)
        {
            items[i] = Instantiate(itemPrefab, itemCell[i].transform.position, itemCell[i].transform.rotation, transform);
            items[i].SetActive(false);
            addons[i] = Instantiate(addonPrefab, addonCell[i].transform.position, addonCell[i].transform.rotation, transform);
            addons[i].SetActive(false);
        }
        for (int i = 0; i < 4; i++)
        {
            infoAddons[i] = Instantiate(infoAddonPrefab, infoAddonSpace[i].transform.position, infoAddonSpace[i].transform.rotation, transform);
            infoAddons[i].SetActive(false);
        }
        infoSpace.transform.Find("Frame").gameObject.SetActive(false);
    }
	
    public void SetOnPosition(List<Item> itemList, List<Addon> addonList)
    {
        for(int i=0; i<itemList.Count; i++)
        {
            items[i].transform.position = itemCell[i].transform.position;
            items[i].GetComponent<Image>().sprite = itemFrameQuality[(int)itemList[i].quality];
            items[i].transform.Find("Sprite").gameObject.GetComponent<Image>().sprite = itemList[i].sprite;
            items[i].SetActive(true);
        }
        for(int i=itemList.Count; i<9; i++)
            items[i].SetActive(false);
        for(int i=0; i<addonList.Count; i++)
        {
            addons[i].transform.position = addonCell[i].transform.position;
            addons[i].GetComponent<Image>().sprite = addonFrameQuality[(int)addonList[i].type * 4 + (int)addonList[i].quality];
            addons[i].transform.Find("Sprite").gameObject.GetComponent<Image>().sprite = addonList[i].sprite;
            addons[i].SetActive(true);
        }
        for (int i = itemList.Count; i < 9; i++)
            addons[i].SetActive(false);
        
        if(selectedItem >= 0)
        {
            GameObject frameObj = infoSpace.transform.Find("Frame").gameObject;
            frameObj.GetComponent<Image>().sprite = infoFrameQuality[(int)itemList[selectedItem].quality];
            frameObj.transform.Find("ItemSprite").gameObject.GetComponent<Image>().sprite = itemFrameQuality[(int)itemList[selectedItem].quality];
            frameObj.transform.Find("ItemSprite").Find("Sprite").gameObject.GetComponent<Image>().sprite = itemList[selectedItem].sprite;
            for (int i=0; i<4; i++)
            {
                infoAddonSpace[i].GetComponent<Image>().sprite = addonLockSprite[i * 2 + (itemList[selectedItem].attachable[i] ? 1 : 0)];
                if(itemList[selectedItem].addons[i] != null)
                {
                    infoAddons[i].GetComponent<Image>().sprite = addonFrameQuality[(int)itemList[selectedItem].addons[i].type * 4 + (int)itemList[selectedItem].addons[i].quality];
                    infoAddons[i].transform.Find("AddonSprite").gameObject.GetComponent<Image>().sprite = itemList[selectedItem].addons[i].sprite;
                    infoAddons[i].transform.Find("AddonSprite").Find("Sprite").gameObject.GetComponent<Image>().sprite = itemList[selectedItem].addons[i].sprite;
                    infoAddons[i].SetActive(true);
                }
                else
                    infoAddons[i].SetActive(false);
                
            }
        }
    }
}
