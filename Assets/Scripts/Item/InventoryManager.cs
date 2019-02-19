using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public List<Item> itemList = new List<Item>();
    public List<Addon> addonList = new List<Addon>();
    InventoryUI ui;
    public List<string>[] itemPool = new List<string>[4];
    public List<string>[] addonPool = new List<string>[4];
    public GameObject droppedPrefab;
    public float popoutStrengthMultiplier;
    public float popoutTime;
    GameObject player;

    private void Start()
    {
        ui = GameObject.Find("InventoryCanvas").GetComponent<InventoryUI>();
        GameObject.Find("InventoryCanvas").SetActive(false);

        player = GameObject.Find("Player");

        SetPool();

        //ItemInstantiate("Dagger", player.transform.position, 0f);

        StartCoroutine(TestCoroutine());
    }
    /// <summary>
    /// Set Item, Addon Pool and shuffle them
    /// </summary>
    void SetPool()
    {
        for(int i=0; i<4; i++)
        {
            itemPool[i] = new List<string>();
            addonPool[i] = new List<string>();
        }

        itemPool[0].Add("Bow");
        itemPool[0].Add("Dagger");

        itemPool[1].Add("Baculus");
        itemPool[1].Add("BambooSpear");
        itemPool[1].Add("ChainSickle");
        itemPool[1].Add("ExplosionGloves");
        itemPool[1].Add("Festo");
        itemPool[1].Add("FrostShield");

        itemPool[2].Add("Ksanife");
        itemPool[2].Add("MeteorSword");
        itemPool[2].Add("Morgenstern");

        //itemPool[3].Add("");


        addonPool[0].Add("ParchmentPiece");
        addonPool[0].Add("KnightsStirrup");
        addonPool[0].Add("ApprenticesMark");

        addonPool[1].Add("GlowingHerb");
        addonPool[1].Add("CoollyPride");

        //addonPool[2].Add("");

        //addonPool[3].Add("");


        for (int i = 0; i < 4; i++)
        {
            ShuffleList(itemPool[i]);
            ShuffleList(addonPool[i]);
        }
    }
    void ShuffleList(List<string> list)
    {
        System.Random random = new System.Random();
        for(int i= list.Count - 1; i>=0; i--)
        {
            string tmp = list[i];
            int n = random.Next(i + 1);
            list[i] = list[n];
            list[n] = tmp;
        }
    }
    IEnumerator TestCoroutine()
    {
        yield return null;

    }

    IEnumerator PopoutCoroutine(GameObject obj)
    {
        float endTime = Time.time + popoutTime;
        Vector2 orgScale = obj.transform.localScale;
        SpriteRenderer[] sprtArr = obj.GetComponents<SpriteRenderer>();
        
        while(Time.time < endTime)
        {
            obj.transform.localScale = (1 - ((endTime - Time.time) / popoutTime)) * orgScale;
            foreach (SpriteRenderer sprt in sprtArr)
                sprt.color = new Color(sprt.color.r, sprt.color.g, sprt.color.b, 1 - ((endTime - Time.time) / popoutTime));
            yield return null;
        }

        obj.transform.localScale = orgScale;
        foreach (SpriteRenderer sprt in sprtArr)
            sprt.color = new Color(sprt.color.r, sprt.color.g, sprt.color.b, 1f);
    }
    void PopoutGenerator(GameObject obj, float popoutStrength)
    {
        popoutStrength *= popoutStrengthMultiplier;
        float angle = Mathf.Deg2Rad * Random.Range(80f, 100f);
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * popoutStrength;
        StartCoroutine(PopoutCoroutine(obj));
    }

    /// <summary>
    /// Instantiate random item by quality
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="pos"></param>
    /// <param name="popoutStrength">0:no popout, 1:normal popout</param>
    public void ItemInstantiate(ItemQuality quality, Vector3 pos, float popoutStrength)
    {
        if(itemPool[(int)quality].Count > 0)
        {
            ItemInstantiate(itemPool[(int)quality][0], pos, popoutStrength);
            itemPool[(int)quality].RemoveAt(0);
        }
    }
    /// <summary>
    /// Instantiate item by name on pos, also Instantiate Item class 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pos"></param>
    public void ItemInstantiate(string str, Vector3 pos, float popoutStrength)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init((Item)System.Activator.CreateInstance(System.Type.GetType(str)), pos);
        tmpItem.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        PopoutGenerator(tmpItem, popoutStrength);
    }
    /// <summary>
    /// Instantiate item by Item Instance on pos
    /// </summary>
    /// <param name="item"></param>
    /// <param name="pos"></param>
    public void ItemInstantiate(Item item, Vector3 pos, float popoutStrength)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init(item, pos);
        tmpItem.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        PopoutGenerator(tmpItem, popoutStrength);
    }

    /// <summary>
    /// Instantiate random addon by quality
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="pos"></param>
    public void AddonInstantiate(ItemQuality quality, Vector3 pos, float popoutStrength)
    {
        if (addonPool[(int)quality].Count > 0)
        {
            AddonInstantiate(addonPool[(int)quality][0], pos, popoutStrength);
            addonPool[(int)quality].RemoveAt(0);
        }
    }
    /// <summary>
    /// Instantiate addon by name on pos, also Instantiate Addon class 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pos"></param>
    public void AddonInstantiate(string str, Vector3 pos, float popoutStrength)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init((Addon)System.Activator.CreateInstance(System.Type.GetType(str)), pos);
        tmpItem.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        PopoutGenerator(tmpItem, popoutStrength);
    }
    /// <summary>
    /// Instantiate addon by Addon Instance on pos
    /// </summary>
    /// <param name="item"></param>
    /// <param name="pos"></param>
    public void AddonInstantiate(Addon addon, Vector3 pos, float popoutStrength)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init(addon, pos);
        tmpItem.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        PopoutGenerator(tmpItem, popoutStrength);
    }

    /// <summary>
    /// reset inventory canvas
    /// </summary>
    public void SetOnPosition()
    {
        ui.SetOnPosition(itemList, addonList);
    }

    /// <summary>
    /// call when item has been clicked
    /// </summary>
    /// <param name="itemIndex"></param>
    public void ItemSelect(int itemIndex)
    {
        ui.selectedItem = itemIndex;
        ui.SetOnPosition(itemList, addonList);
    }
    /// <summary>
    /// push item in inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool PushItem(Item item)
    {
        for(int i = 0; i<item.skillNum; i++)
            foreach (Item tmpItem in itemList)
                for (int j = 0; j < tmpItem.skillNum; j++)
                    if (item.combo[i].Equals(tmpItem.combo[j]))
                        return false;

        if (itemList.Count > 8) return false;

        itemList.Add(item);
        ui.SetOnPosition(itemList, addonList);
        return true;
    }
    /// <summary>
    /// push addon in inventory
    /// </summary>
    /// <param name="addon"></param>
    /// <returns></returns>
    public bool PushAddon(Addon addon)
    {
        if (addonList.Count > 8) return false;

        addonList.Add(addon);
        ui.SetOnPosition(itemList, addonList);
        return true;
    }
    /// <summary>
    /// call when item has been discarded. instantiate dropped item object
    /// </summary>
    /// <param name="index"></param>
    public void DiscardItem(int index)
    {
        if (itemList.Count > index)
        {
            ItemInstantiate(itemList[index], player.transform.position, 1f);
            itemList.RemoveAt(index);
            if (index == ui.selectedItem)
                ui.selectedItem = -1;
        }
        ui.SetOnPosition(itemList, addonList);
    }
    /// <summary>
    /// call when addon(not attached on item) has been discarded. instantiate dropped addon object
    /// </summary>
    /// <param name="index"></param>
    public void DiscardAddon(int index)
    {
        if (addonList.Count > index)
        {
            AddonInstantiate(addonList[index], player.transform.position, 1f);
            addonList.RemoveAt(index);
        }
        ui.SetOnPosition(itemList, addonList);
    }
    /// <summary>
    /// call when addon(attached on item) has been discarded. instantiate dropped addon object
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="addonType"></param>
    public void DiscardAddon(int itemIndex, AddonType addonType)
    {
        if (itemList[itemIndex].addons[(int)addonType] != null)
        {
            AddonInstantiate(itemList[itemIndex].addons[(int)addonType], player.transform.position, 1f);
            itemList[itemIndex].addons[(int)addonType] = null;
        }
        ui.SetOnPosition(itemList, addonList);
    }
    /// <summary>
    /// attach addon to item
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="addonIndex"></param>
    public void AttachAddon(int itemIndex, int addonIndex)
    {
        if(itemList[itemIndex].attachable[(int)addonList[addonIndex].type] && itemList[itemIndex].addons[(int)addonList[addonIndex].type] == null)
        {
            itemList[itemIndex].addons[(int)addonList[addonIndex].type] = addonList[addonIndex];
            addonList.RemoveAt(addonIndex);
        }
        ui.SetOnPosition(itemList, addonList);
    }
    /// <summary>
    /// dettach addon from item
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="addonType"></param>
    public void DetachAddon(int itemIndex, AddonType addonType)
    {
        if(addonList.Count < 9 && itemList[itemIndex].addons[(int)addonType] != null)
        {
            addonList.Add(itemList[itemIndex].addons[(int)addonType]);
            itemList[itemIndex].addons[(int)addonType] = null;
        }
        ui.SetOnPosition(itemList, addonList);
    }
    
}
