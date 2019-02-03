using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    InventoryManager inventoryManager;
    public bool itemAddon; //false: item true: addon
    public Item item;
    public Addon addon;
    public GameObject highlight;
    float itemSizeMultiplier = 0.0077f;
    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    SpriteRenderer sprt;
    public void Init(Item _item, Vector3 pos)
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        item = _item;
        itemAddon = false;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        sprt = GetComponent<SpriteRenderer>();

        transform.position = pos;
        sprt.sprite = item.sprite;
        highlight.GetComponent<SpriteRenderer>().sprite = item.highlight;
        highlight.SetActive(false);
        bc2D.size = sprt.size;
        transform.localScale = new Vector3((item.sizeInventory.x * itemSizeMultiplier) / sprt.size.x, (item.sizeInventory.y * itemSizeMultiplier) / sprt.size.y, 1);


        
    }
    public void Init(Addon _addon, Vector3 pos)
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        addon = _addon;
        itemAddon = true;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        sprt = GetComponent<SpriteRenderer>();

        transform.position = pos;
        sprt.sprite = addon.sprite;
        highlight.GetComponent<SpriteRenderer>().sprite = addon.highlight;
        highlight.SetActive(false);
        bc2D.size = sprt.size;
        transform.localScale = new Vector3((addon.sizeInventory.x * itemSizeMultiplier) / sprt.size.x, (addon.sizeInventory.y * itemSizeMultiplier) / sprt.size.y, 1);
    }
    public bool PushItem()
    {
        if (!itemAddon && inventoryManager.PushItem(item))
        {
            Destroy(gameObject);
            return true;
        }
        else if (itemAddon && inventoryManager.PushAddon(addon))
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    public void HighlightSwitch(bool enabled)
    {
        highlight.SetActive(enabled);
    }
}
