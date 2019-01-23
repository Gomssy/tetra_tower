using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public List<Item> itemList = new List<Item>();
    public List<Addon> addonList = new List<Addon>();
    InventoryUI ui;
    public List<string> itemPool = new List<string>();
    public List<string> addonPool = new List<string>();

    private void Start()
    {
        ui = GameObject.Find("InventoryCanvas").GetComponent<InventoryUI>();

        //itemPool
        itemPool.Add("Bow");
        itemPool.Add("BambooSpear");
        itemPool.Add("Ksanife");

        //addonPool
        addonPool.Add("ApprenticesMark");
        addonPool.Add("ParchmentPiece");

        StartCoroutine("TestCoroutine");
    }

    IEnumerator TestCoroutine()
    {
        yield return null;
        PushItem((Item)System.Activator.CreateInstance(System.Type.GetType(itemPool[0])));
        yield return new WaitForSeconds(1f);
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
    public void PushItem(Item item)
    {
        if(itemList.Count < 9)
            itemList.Add(item);
        ui.SetOnPosition(itemList, addonList);
    }
    public void PushAddon(Addon addon)
    {
        if (addonList.Count < 9)
            addonList.Add(addon);
        ui.SetOnPosition(itemList, addonList);
    }
    public void DiscardItem(int index)
    {
        if (itemList.Count > index)
            itemList.RemoveAt(index);
        ui.SetOnPosition(itemList, addonList);
    }
    public void DiscardAddon(int index)
    {
        if (addonList.Count > index)
            addonList.RemoveAt(index);
        ui.SetOnPosition(itemList, addonList);
    }
    public void DiscardAddon(int itemIndex, AddonType addonType)
    {
        if (itemList[itemIndex].addons[(int)addonType] != null)
            itemList[itemIndex].addons[(int)addonType] = null;
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
