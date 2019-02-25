using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour, IPlayerInteraction
{
    InventoryManager inventoryManager;
    public bool itemAddon; //false: item true: addon
    public Item item;
    public Addon addon;
    public GameObject highlight;
    public float itemSizeMultiplier;
    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    SpriteRenderer sprt;
    public int price = 0;
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
        inventoryManager = InventoryManager.Instance;
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
    public void Apply()
    {
        if(LifeStoneManager.Instance.CountType(LifeStoneType.Gold) < price)
        {
            Debug.Log("Not enough gold");
            return;
        }
        else if (!itemAddon && inventoryManager.PushItem(item))
        {
            LifeStoneManager.Instance.ChangeToNormal(LifeStoneType.Gold, price);
            Destroy(gameObject);
        }
        else if (itemAddon && inventoryManager.PushAddon(addon))
        {
            LifeStoneManager.Instance.ChangeToNormal(LifeStoneType.Gold, price);
            Destroy(gameObject);
        }
    }
    public void HighlightSwitch(bool enabled)
    {
        highlight.SetActive(enabled);
        highlight.GetComponent<SpriteRenderer>().sortingOrder = -1 + (enabled ? 2 : 0);
        GetComponent<SpriteRenderer>().sortingOrder = (enabled ? 2 : 0);
    }
}
