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
    public Sprite[] infoAddonType = new Sprite[4];
    GameObject[] items;
    GameObject[] addons;
    public GameObject[] infoAddonsFrame;
    public GameObject[] comboStringFrame;
    public GameObject[] comboCharPrefab;
    public GameObject[] comboNameFrame;
    public float pixelBetweenChar;
    public GameObject addonInfoPrefab;
    GameObject[,] comboChars = new GameObject[3, 8];
    GameObject[] infoAddons;
    GameObject addonInfo;
    public int selectedItem = -1;
    string[] qualityString = new string[4] { "습작", "범작", "수작", "걸작" };

    InventoryManager inventoryManager;

	void Awake () {

        items = new GameObject[9];
        addons = new GameObject[9];
        infoAddons = new GameObject[4];
        for (int i = 0; i < 9; i++)
        {
            items[i] = Instantiate(itemPrefab, itemCell[i].transform.position, itemCell[i].transform.rotation, transform);
            items[i].SetActive(false);
            items[i].GetComponent<ItemDrag>().num = i;
            addons[i] = Instantiate(addonPrefab, addonCell[i].transform.position, addonCell[i].transform.rotation, transform);
            addons[i].SetActive(false);
            addons[i].GetComponent<AddonDrag>().num = i;
        }
        for (int i = 0; i < 4; i++)
        {
            infoAddonsFrame[i].GetComponent<Image>().sprite = infoAddonType[i];
            infoAddonsFrame[i].transform.position = infoAddonSpace[i].transform.position;
            infoAddonsFrame[i].SetActive(false);
            infoAddons[i] = Instantiate(addonPrefab, infoAddonsFrame[i].transform.Find("AddonCell").position, Quaternion.identity, transform);
            infoAddons[i].GetComponent<AddonDrag>().num = 9 + i;
            infoAddons[i].SetActive(false);
            
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                comboChars[i, j] = Instantiate(comboCharPrefab[0], comboStringFrame[i].transform);
                comboChars[i, j].SetActive(false);
            }
            comboStringFrame[i].SetActive(false);
            comboNameFrame[i].SetActive(false);
        }
        
        infoSpace.transform.Find("Frame").gameObject.SetActive(false);

        addonInfo = Instantiate(addonInfoPrefab, transform);
        addonInfo.SetActive(false);
    }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }


    public void SetAddonInfo()
    {
        addonInfo.SetActive(false);
    }

    public void SetAddonInfo(int addonNum)
    {
        if (addons[addonNum].activeSelf)
        {
            addonInfo.SetActive(true);
            addonInfo.transform.position = addonCell[addonNum].transform.position + new Vector3(0, 200, 0);
            addonInfo.transform.position += new Vector3(-addonInfo.transform.position.x + Mathf.Min(addonInfo.transform.position.x, 1650), 0, 0);

            Addon currentAddon = inventoryManager.addonList[addonNum];
            GameObject tmpObj = addonInfo.transform.Find("AddonPrefab").gameObject;
            tmpObj.GetComponent<Image>().sprite = addonFrameQuality[(int)currentAddon.type * 4 + (int)currentAddon.quality];
            tmpObj.transform.Find("Sprite").gameObject.GetComponent<Image>().sprite = currentAddon.sprite;
            tmpObj.transform.Find("Sprite").gameObject.GetComponent<RectTransform>().sizeDelta = currentAddon.sizeInventory;
            tmpObj.SetActive(true);
            addonInfo.transform.Find("Quality").GetComponent<Text>().text = qualityString[(int)currentAddon.quality];
            addonInfo.transform.Find("Name").GetComponent<Text>().text = currentAddon.name;
            addonInfo.transform.Find("Description").GetComponent<Text>().text = currentAddon.addonDescription;
            addonInfo.transform.Find("Info").GetComponent<Text>().text = currentAddon.addonInfo;
        }
    }

    public void SetOnPosition(List<Item> itemList, List<Addon> addonList)
    {
        SetAddonInfo();
        for(int i=0; i<itemList.Count; i++)
        {
            items[i].transform.position = itemCell[i].transform.position;
            items[i].GetComponent<Image>().sprite = itemFrameQuality[(int)itemList[i].quality];
            items[i].transform.Find("Sprite").gameObject.GetComponent<Image>().sprite = itemList[i].sprite;
            items[i].transform.Find("Sprite").gameObject.GetComponent<RectTransform>().sizeDelta = itemList[i].sizeInventory;
            items[i].SetActive(true);
        }
        for(int i=itemList.Count; i<9; i++)
            items[i].SetActive(false);
        for(int i=0; i<addonList.Count; i++)
        {
            addons[i].transform.position = addonCell[i].transform.position;
            addons[i].GetComponent<Image>().sprite = addonFrameQuality[(int)addonList[i].type * 4 + (int)addonList[i].quality];
            addons[i].transform.Find("Sprite").gameObject.GetComponent<Image>().sprite = addonList[i].sprite;
            addons[i].transform.Find("Sprite").gameObject.GetComponent<RectTransform>().sizeDelta = addonList[i].sizeInventory;
            addons[i].SetActive(true);
        }
        for (int i = addonList.Count; i < 9; i++)
            addons[i].SetActive(false);

        GameObject frameObj = infoSpace.transform.Find("Frame").gameObject;
        if (selectedItem >= 0)
        {
            frameObj.SetActive(true);
            frameObj.GetComponent<Image>().sprite = infoFrameQuality[(int)itemList[selectedItem].quality];
            frameObj.transform.Find("ItemSprite").gameObject.GetComponent<Image>().sprite = itemFrameQuality[(int)itemList[selectedItem].quality];
            frameObj.transform.Find("ItemSprite").Find("Sprite").gameObject.GetComponent<Image>().sprite = itemList[selectedItem].sprite;
            frameObj.transform.Find("ItemSprite").Find("Sprite").gameObject.GetComponent<RectTransform>().sizeDelta = itemList[selectedItem].sizeInventory;
            frameObj.transform.Find("ItemDescription").gameObject.GetComponent<Text>().text = itemList[selectedItem].itemInfo;
            frameObj.transform.Find("ItemDescription").Find("ItemQuality").gameObject.GetComponent<Text>().text = qualityString[(int)itemList[selectedItem].quality];
            frameObj.transform.Find("ItemDescription").Find("ItemName").gameObject.GetComponent<Text>().text = itemList[selectedItem].name;
            for (int i = 0; i < 3; i++)
            {
                if (i < itemList[selectedItem].skillNum)
                {
                    comboStringFrame[i].SetActive(true);
                    comboNameFrame[i].SetActive(true);
                    comboNameFrame[i].GetComponent<Text>().text = itemList[selectedItem].comboName[i];
                    float tmpx = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        if(j < itemList[selectedItem].combo[i].Length)
                        {
                            comboChars[i, j].SetActive(true);
                            comboChars[i, j].GetComponent<Image>().sprite = comboCharPrefab[itemList[selectedItem].combo[i][j] - 'A'].GetComponent<Image>().sprite;
                            comboChars[i, j].GetComponent<RectTransform>().sizeDelta = comboCharPrefab[itemList[selectedItem].combo[i][j] - 'A'].GetComponent<RectTransform>().sizeDelta;
                            comboChars[i, j].GetComponent<RectTransform>().localPosition = new Vector3(tmpx, 0, 0);
                            tmpx += comboChars[i, j].GetComponent<RectTransform>().sizeDelta.x + pixelBetweenChar;
                            
                        }
                        else
                        {
                            comboChars[i, j].SetActive(false);
                            
                        }
                    }
                }
                else
                {
                    comboNameFrame[i].GetComponent<Text>().text = "";
                    comboStringFrame[i].SetActive(false);
                }
            }
            for (int i=0; i<4; i++)
            {
                
                infoAddonSpace[i].GetComponent<Image>().sprite = addonLockSprite[i * 2 + (itemList[selectedItem].attachable[i] ? 1 : 0)];
                if(itemList[selectedItem].addons[i] != null)
                {
                    infoAddonsFrame[i].transform.position = infoAddonSpace[i].transform.position;
                    infoAddonsFrame[i].SetActive(true);
                    infoAddons[i].transform.position = infoAddonsFrame[i].transform.Find("AddonCell").position;
                    infoAddons[i].GetComponent<Image>().sprite = addonFrameQuality[(int)itemList[selectedItem].addons[i].type * 4 + (int)itemList[selectedItem].addons[i].quality];
                    infoAddons[i].transform.Find("Sprite").gameObject.GetComponent<Image>().sprite = itemList[selectedItem].addons[i].sprite;
                    infoAddons[i].transform.Find("Sprite").gameObject.GetComponent<RectTransform>().sizeDelta = itemList[selectedItem].addons[i].sizeInventory;
                    infoAddons[i].SetActive(true);
                    infoAddonsFrame[i].transform.Find("Quality").GetComponent<Text>().text = qualityString[(int)itemList[selectedItem].addons[i].quality];
                    infoAddonsFrame[i].transform.Find("Name").GetComponent<Text>().text = itemList[selectedItem].addons[i].name;
                    infoAddonsFrame[i].transform.Find("Description").GetComponent<Text>().text = itemList[selectedItem].addons[i].addonDescription;
                    infoAddonsFrame[i].transform.Find("Info").GetComponent<Text>().text = itemList[selectedItem].addons[i].addonInfo;


                }
                else
                {
                    infoAddons[i].SetActive(false);
                    infoAddonsFrame[i].SetActive(false);
                }
                    
                
            }
        }
        else
        {
            frameObj.SetActive(false);
            for (int i = 0; i < 4; i++)
            {
                infoAddons[i].SetActive(false);
                infoAddonsFrame[i].SetActive(false);
            }
        }
    }
}
