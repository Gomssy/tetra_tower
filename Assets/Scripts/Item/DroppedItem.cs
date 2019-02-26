using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : DroppedObject, IPlayerInteraction
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
    Color[] qualityColor = new Color[4]
    {
        new Color(102 / 255f, 65 / 255f, 48 / 255f),
        new Color( 15 / 255f,  2 / 255f,  8 / 255f),
        new Color(  3 / 255f, 93 / 255f, 65 / 255f),
        new Color(173 / 255f, 26 / 255f, 38 / 255f), 
    };

    public void Init(Item _item, Vector3 pos)
    {
        inventoryManager = InventoryManager.Instance;
        item = _item;
        itemAddon = false;
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        sprt = GetComponent<SpriteRenderer>();

        transform.position = pos;
        sprt.sprite = item.sprite;
        highlight.GetComponent<SpriteRenderer>().sprite = item.highlight;
        highlight.GetComponent<SpriteRenderer>().color = qualityColor[(int)item.quality];
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
        highlight.GetComponent<SpriteRenderer>().color = qualityColor[(int)addon.quality];
        highlight.SetActive(false);
        bc2D.size = sprt.size;
        transform.localScale = new Vector3((addon.sizeInventory.x * itemSizeMultiplier) / sprt.size.x, (addon.sizeInventory.y * itemSizeMultiplier) / sprt.size.y, 1);
    }
    public void Apply()
    {
        if(LifeStoneManager.Instance.CountType(LifeStoneType.Gold) < price)
            StartCoroutine(GameManager.Instance.player.GetComponent<Player>().DisplayText("금 생명석이 부족합니다!"));
        else if (!itemAddon && inventoryManager.PushItem(item))
        {
            LifeStoneManager.Instance.ChangeToNormal(LifeStoneType.Gold, price);
            if(priceTag)
                Destroy(priceTag.gameObject);
            Destroy(gameObject);
        }
        else if (itemAddon && inventoryManager.PushAddon(addon))
        {
            LifeStoneManager.Instance.ChangeToNormal(LifeStoneType.Gold, price);
            if (priceTag)
                Destroy(priceTag.gameObject);
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
