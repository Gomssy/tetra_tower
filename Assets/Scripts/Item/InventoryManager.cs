using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager> {

    public List<Item> itemList = new List<Item>();
    public List<Addon> addonList = new List<Addon>();
    public InventoryUI ui;
    public List<string>[] itemPool = new List<string>[4];
    public List<string>[] addonPool = new List<string>[4];
    public GameObject droppedPrefab;
    public float popoutStrengthMultiplier;
    public float popoutTime;
    public Text price;
    public GameObject woodSign;
    GameObject player;

    public GameObject coolUI;

    private void Start()
    {
        player = GameManager.Instance.player;
        SetPool();
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
        
        itemPool[1].Add("ExplosionGloves");
        itemPool[1].Add("OilCask");

        itemPool[2].Add("ShockStick");
        itemPool[2].Add("FeatherFan");

        itemPool[3].Add("BitSword");


        addonPool[0].Add("ParchmentPiece");
        addonPool[0].Add("ApprenticesMark");

        addonPool[1].Add("BlacksmithsBrooch");
        addonPool[1].Add("CoollyPride");
        addonPool[1].Add("GlowingHerb");
        addonPool[1].Add("Gluttony");
        addonPool[1].Add("SmallLens");

        addonPool[2].Add("JanusCoin");
        addonPool[2].Add("DesignofRagur");
        addonPool[2].Add("Sandbag");

        addonPool[3].Add("BitMemory");


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
        ItemInstantiate("ShockStick", player.transform.position, 1f);
        yield return new WaitForSeconds(0.3f);
        ItemInstantiate("BitSword", player.transform.position, 1f);
        yield return new WaitForSeconds(0.3f);
        ItemInstantiate("Bow", player.transform.position, 1f);
        yield return new WaitForSeconds(0.3f);
        ItemInstantiate("Dagger", player.transform.position, 1f);
        yield return new WaitForSeconds(0.3f);
        ItemInstantiate("ExplosionGloves", player.transform.position, 1f);
        yield return new WaitForSeconds(0.3f);
        AddonInstantiate("GlowingHerb", player.transform.position, 1f);
        yield return new WaitForSeconds(0.3f);
        AddonInstantiate("BlacksmithsBrooch", player.transform.position, 1f);
        yield return new WaitForSeconds(0.3f);
        AddonInstantiate("BitMemory", player.transform.position, 1f);
        yield return new WaitForSeconds(0.3f);

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
    public GameObject ItemInstantiate(ItemQuality quality, Vector3 pos, float popoutStrength)
    {
        if(itemPool[(int)quality].Count > 0)
        {
            return ItemInstantiate(itemPool[(int)quality][0], pos, popoutStrength);
        }
        else if (addonPool[(int)quality].Count > 0)
        {
            return AddonInstantiate(quality, pos, popoutStrength);
        }
        else if(quality != ItemQuality.Masterpiece)
        {
            return ItemInstantiate(quality + 1, pos, popoutStrength);
        }
        return null;
    }
    /// <summary>
    /// Instantiate item by name on pos, also Instantiate Item class 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pos"></param>
    public GameObject ItemInstantiate(string str, Vector3 pos, float popoutStrength)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init((Item)System.Activator.CreateInstance(System.Type.GetType(str)), pos);

        for (int i = 0; i < 4; i++)
            if (itemPool[i].Contains(str))
                itemPool[i].Remove(str);

        tmpItem.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        PopoutGenerator(tmpItem, popoutStrength);
        return tmpItem;
    }
    /// <summary>
    /// Instantiate item by Item Instance on pos
    /// </summary>
    /// <param name="item"></param>
    /// <param name="pos"></param>
    public GameObject ItemInstantiate(Item item, Vector3 pos, float popoutStrength)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init(item, pos);
        tmpItem.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        PopoutGenerator(tmpItem, popoutStrength);
        return tmpItem;
    }
    /// <summary>
    /// Instantiate random item by quality and assign price.
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="pos"></param>
    /// <param name="price"></param>
    /// <param name="popoutStrength">0:no popout, 1:normal popout</param>
    public GameObject ItemInstantiate(ItemQuality quality, Vector3 pos, int _price, float popoutStrength)
    {
        if (itemPool[(int)quality].Count > 0)
        {
            GameObject tmpItem = ItemInstantiate(itemPool[(int)quality][0], pos, popoutStrength);
            tmpItem.GetComponent<DroppedObject>().price = _price;
            tmpItem.GetComponent<DroppedObject>().priceTag = Instantiate(price, new Vector3(0, 0, 0), Quaternion.identity, GameManager.Instance.textCanvas.transform);
            return tmpItem;
        }
        return null;
    }

    /// <summary>
    /// Instantiate random addon by quality
    /// </summary>
    /// <param name="quality"></param>
    /// <param name="pos"></param>
    public GameObject AddonInstantiate(ItemQuality quality, Vector3 pos, float popoutStrength)
    {
        if (addonPool[(int)quality].Count > 0)
        {
            return AddonInstantiate(addonPool[(int)quality][0], pos, popoutStrength);
        }
        else if (itemPool[(int)quality].Count > 0)
        {
            return ItemInstantiate(quality, pos, popoutStrength);
        }
        else if (quality != ItemQuality.Masterpiece)
        {
            return AddonInstantiate(quality + 1, pos, popoutStrength);
        }
        return null;
    }
    /// <summary>
    /// Instantiate addon by name on pos, also Instantiate Addon class 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pos"></param>
    public GameObject AddonInstantiate(string str, Vector3 pos, float popoutStrength)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init((Addon)System.Activator.CreateInstance(System.Type.GetType(str)), pos);

        for (int i = 0; i < 4; i++)
            if (addonPool[i].Contains(str))
                addonPool[i].Remove(str);

        tmpItem.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        PopoutGenerator(tmpItem, popoutStrength);
        return tmpItem;
    }
    /// <summary>
    /// Instantiate addon by Addon Instance on pos
    /// </summary>
    /// <param name="item"></param>
    /// <param name="pos"></param>
    public GameObject AddonInstantiate(Addon addon, Vector3 pos, float popoutStrength)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init(addon, pos);
        tmpItem.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        PopoutGenerator(tmpItem, popoutStrength);
        return tmpItem;
    }
    /// <summary>
    /// Instantiate random addon by quality and assign price.
    /// </summary>
    /// <param name="addon">The addon.</param>
    /// <param name="pos">The position.</param>
    /// <param name="price">The price.</param>
    /// <param name="popoutStrength">The popout strength.</param>
    /// <returns></returns>
    public GameObject AddonInstantiate(ItemQuality quality, Vector3 pos, int _price, float popoutStrength)
    {
        if (addonPool[(int)quality].Count > 0)
        {
            GameObject tmpItem = AddonInstantiate(addonPool[(int)quality][0], pos, popoutStrength);
            tmpItem.GetComponent<DroppedObject>().price = _price;
            tmpItem.GetComponent<DroppedObject>().priceTag = Instantiate(price, new Vector3(0, 0, 0), Quaternion.identity, GameManager.Instance.textCanvas.transform);
            return tmpItem;
        }
        return null;
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
                    {
                        StartCoroutine(GameManager.Instance.player.GetComponent<Player>().DisplayText(tmpItem.name + "와(과) 콤보가 중복됩니다!"));
                        return false;
                    }


        if (itemList.Count > 8)
        {
            StartCoroutine(GameManager.Instance.player.GetComponent<Player>().DisplayText("아이템이 너무 많습니다!"));
            return false;
        }

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
        if (addonList.Count > 8)
        {
            StartCoroutine(GameManager.Instance.player.GetComponent<Player>().DisplayText("예비 애드온이 너무 많습니다!"));
            return false;
        }

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
