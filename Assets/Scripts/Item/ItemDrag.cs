using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public int num;
    InventoryUI ui;
    InventoryManager manager;
    Transform discardBin;
    void Start()
    {
        ui = GameObject.Find("InventoryCanvas").GetComponent<InventoryUI>();
        manager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        discardBin = ui.gameObject.transform.Find("DiscardBin");
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
        if (CheckBetween(Input.mousePosition, discardBin.position, discardBin.GetComponent<RectTransform>().sizeDelta))
            manager.DiscardItem(num);

        manager.SetOnPosition();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ui.selectedItem = num;
        manager.SetOnPosition();
    }
    bool CheckBetween(Vector3 mouse, Vector3 center, Vector2 size)
    {
        return Mathf.Abs(mouse.x - center.x) <= size.x / 2f && Mathf.Abs(mouse.y - center.y) <= size.y / 2f;
    }
}
