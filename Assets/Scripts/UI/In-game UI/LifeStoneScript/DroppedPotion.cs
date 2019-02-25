using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedPotion : DroppedObject, IPlayerInteraction{

    public GameObject highlight;

    public void Apply()
    {
        LifeStoneManager.Instance.ChangeFromNormal(LifeStoneType.Gold, 3);
        Destroy(gameObject);
    }

    public void HighlightSwitch(bool enabled)
    {
        highlight.SetActive(enabled);
        highlight.GetComponent<SpriteRenderer>().sortingOrder = -1 + (enabled ? 2 : 0);
        GetComponent<SpriteRenderer>().sortingOrder = (enabled ? 2 : 0);
    }
}
