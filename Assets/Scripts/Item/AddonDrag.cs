﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddonDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int num;
    InventoryUI ui;
    InventoryManager manager;
    void Start()
    {
        ui = GameObject.Find("InventoryCanvas").GetComponent<InventoryUI>();
        manager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        if (num < 9)
        {
            int type = (int)manager.addonList[num].type;
            if (manager.itemList[ui.selectedItem].attachable[type])
            {
                if (CheckBetween(Input.mousePosition, ui.infoAddonsFrame[type].transform.position, ui.infoAddonsFrame[type].GetComponent<RectTransform>().sizeDelta))
                {
                    if (manager.itemList[ui.selectedItem].addons[type] != null) manager.DetachAddon(ui.selectedItem, (AddonType)type);
                    manager.AttachAddon(ui.selectedItem, num);
                }
            }
        }
        else
        {
            if (CheckBetween(Input.mousePosition, ui.gameObject.transform.Find("AddonGroup").position, ui.gameObject.transform.Find("AddonGroup").GetComponent<RectTransform>().sizeDelta))
                manager.DetachAddon(ui.selectedItem, (AddonType)(num - 9));
        }
        manager.SetOnPosition();
    }
    bool CheckBetween(Vector3 mouse, Vector3 center, Vector2 size)
    {
        return Mathf.Abs(mouse.x - center.x) <= size.x / 2f && Mathf.Abs(mouse.y - center.y) <= size.y / 2f;
    }
}