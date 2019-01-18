using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    List<Item> itemList = new List<Item>();
    List<Addon> addonList = new List<Addon>();

    public void PushItem(Item item)
    {
        if(itemList.Count < 9)
            itemList.Add(item);
    }
    public void PushAddon(Addon addon)
    {
        if (addonList.Count < 9)
            addonList.Add(addon);
    }
    public void DiscardItem(int index)
    {
        if (itemList.Count > index)
            itemList.RemoveAt(index);
    }
    public void DiscardAddon(int index)
    {
        if (addonList.Count > index)
            addonList.RemoveAt(index);
    }
    public void DiscardAddon(int itemIndex, AddonType addonType)
    {
        if (itemList[itemIndex].addons[(int)addonType] != null)
            itemList[itemIndex].addons[(int)addonType] = null;
    }
    public void AttachAddon(int itemIndex, int addonIndex)
    {
        if(itemList[itemIndex].attachable[(int)addonList[addonIndex].type] && itemList[itemIndex].addons[(int)addonList[addonIndex].type] == null)
        {
            itemList[itemIndex].addons[(int)addonList[addonIndex].type] = addonList[addonIndex];
            addonList.RemoveAt(addonIndex);
        }
    }
    public void DetachAddon(int itemIndex, AddonType addonType)
    {
        if(addonList.Count < 9 && itemList[itemIndex].addons[(int)addonType] != null)
        {
            addonList.Add(itemList[itemIndex].addons[(int)addonType]);
            itemList[itemIndex].addons[(int)addonType] = null;
        }
    }
    
}
