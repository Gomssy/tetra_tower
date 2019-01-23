using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
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
        manager.SetOnPosition();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ui.selectedItem = num;
        manager.SetOnPosition();
    }
}
