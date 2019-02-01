using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public List<Item> itemList = new List<Item>();
    public List<Addon> addonList = new List<Addon>();
    InventoryUI ui;
    public List<string> itemPool = new List<string>();
    public List<string> addonPool = new List<string>();
    public GameObject droppedPrefab;
    GameObject player;

    private void Start()
    {
        ui = GameObject.Find("InventoryCanvas").GetComponent<InventoryUI>();
        GameObject.Find("InventoryCanvas").SetActive(false);

        player = GameObject.Find("Player");

        //itemPool
        itemPool.Add("Baculus");
        itemPool.Add("BambooSpear");
        itemPool.Add("Bow");
        itemPool.Add("ChainSickle");
        itemPool.Add("Festo");

        //addonPool
        addonPool.Add("ApprenticesMark");
        addonPool.Add("ParchmentPiece");
        addonPool.Add("GlowingHerb");

        StartCoroutine("TestCoroutine");
    }

    IEnumerator TestCoroutine()
    {
        yield return null;
        /*PushItem((Item)System.Activator.CreateInstance(System.Type.GetType(itemPool[0])));
        PushItem((Item)System.Activator.CreateInstance(System.Type.GetType(itemPool[2])));
        yield return new WaitForSeconds(1.5f);
        ItemInstantiate(itemPool[0], player.transform.position);
        yield return new WaitForSeconds(1.5f);
        ItemInstantiate(itemPool[1], player.transform.position);
        yield return new WaitForSeconds(1.5f);
        ItemInstantiate(itemPool[2], player.transform.position);
        yield return new WaitForSeconds(1.5f);
        ItemInstantiate(itemPool[3], player.transform.position);
        yield return new WaitForSeconds(1.5f);
        AddonInstantiate(addonPool[0], player.transform.position);
        yield return new WaitForSeconds(1.5f);
        AddonInstantiate(addonPool[1], player.transform.position);
        ItemSelect(0);
        yield return new WaitForSeconds(1f);
        PushItem((Item)System.Activator.CreateInstance(System.Type.GetType(itemPool[1])));
        yield return new WaitForSeconds(1f);
        PushItem((Item)System.Activator.CreateInstance(System.Type.GetType(itemPool[2])));
        yield return new WaitForSeconds(1f);
        PushAddon((Addon)System.Activator.CreateInstance(System.Type.GetType(addonPool[0])));
        yield return new WaitForSeconds(1f);
        PushAddon((Addon)System.Activator.CreateInstance(System.Type.GetType(addonPool[1])));
        yield return new WaitForSeconds(1f);
        AttachAddon(0, 0);
        yield return new WaitForSeconds(1f);
        */
    }
    public void ItemInstantiate(string str, Vector3 pos)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init((Item)System.Activator.CreateInstance(System.Type.GetType(str)), pos);
    }
    public void ItemInstantiate(Item item, Vector3 pos)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init(item, pos);
    }
    public void AddonInstantiate(string str, Vector3 pos)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init((Addon)System.Activator.CreateInstance(System.Type.GetType(str)), pos);
    }
    public void AddonInstantiate(Addon addon, Vector3 pos)
    {
        GameObject tmpItem = Instantiate(droppedPrefab);
        tmpItem.GetComponent<DroppedItem>().Init(addon, pos);
    }
    public void SetOnPosition()
    {
        ui.SetOnPosition(itemList, addonList);
    }
    public void ItemSelect(int itemIndex)
    {
        ui.selectedItem = itemIndex;
        ui.SetOnPosition(itemList, addonList);
    }
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
    public bool PushAddon(Addon addon)
    {
        if (addonList.Count > 8) return false;

        addonList.Add(addon);
        ui.SetOnPosition(itemList, addonList);
        return true;
    }
    public void DiscardItem(int index)
    {
        if (itemList.Count > index)
        {
            ItemInstantiate(itemList[index], player.transform.position);
            itemList.RemoveAt(index);
            if (index == ui.selectedItem)
                ui.selectedItem = -1;
        }
        ui.SetOnPosition(itemList, addonList);
    }
    public void DiscardAddon(int index)
    {
        if (addonList.Count > index)
        {
            AddonInstantiate(addonList[index], player.transform.position);
            addonList.RemoveAt(index);
        }
        ui.SetOnPosition(itemList, addonList);
    }
    public void DiscardAddon(int itemIndex, AddonType addonType)
    {
        if (itemList[itemIndex].addons[(int)addonType] != null)
        {
            AddonInstantiate(itemList[itemIndex].addons[(int)addonType], player.transform.position);
            itemList[itemIndex].addons[(int)addonType] = null;
        }
        ui.SetOnPosition(itemList, addonList);
    }
    public void AttachAddon(int itemIndex, int addonIndex)
    {
        if(itemList[itemIndex].attachable[(int)addonList[addonIndex].type] && itemList[itemIndex].addons[(int)addonList[addonIndex].type] == null)
        {
            itemList[itemIndex].addons[(int)addonList[addonIndex].type] = addonList[addonIndex];
            addonList.RemoveAt(addonIndex);
        }
        ui.SetOnPosition(itemList, addonList);
    }
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
