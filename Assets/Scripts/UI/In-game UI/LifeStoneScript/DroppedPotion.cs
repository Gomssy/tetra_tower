using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedPotion : DroppedObject, IPlayerInteraction{

    public GameObject highlight;

    public void Apply()
    {
        if (LifeStoneManager.Instance.CountType(LifeStoneType.Gold) < price)
            StartCoroutine(GameManager.Instance.player.GetComponent<Player>().DisplayText("Not enough gold"));
        LifeStoneManager.Instance.ChangeFromNormal(LifeStoneType.Gold, 3);
        if (priceTag)
            Destroy(priceTag.gameObject);
        Destroy(gameObject);
    }

    public void HighlightSwitch(bool enabled)
    {
        highlight.SetActive(enabled);
        highlight.GetComponent<SpriteRenderer>().sortingOrder = -1 + (enabled ? 2 : 0);
        GetComponent<SpriteRenderer>().sortingOrder = (enabled ? 2 : 0);
    }
}
