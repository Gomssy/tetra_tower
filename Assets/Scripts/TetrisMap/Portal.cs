using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerInteraction  {

    public GameObject highlight;
    bool isPortalUsed = false;

    public void Apply()
    {
        if (GameManager.Instance.isTutorial && !isPortalUsed)
        {
            StartCoroutine(GameManager.Instance.EndTutorial());
            isPortalUsed = true;
        }
        else if (!GameManager.Instance.isTutorial && GameManager.gameState == GameState.Ingame)
            StartCoroutine(Camera.main.GetComponent<CameraController>().ChangeScene(GameState.Portal));
    }

    public void HighlightSwitch(bool enabled)
    {
        if (highlight)
        {
            highlight.SetActive(enabled);
            highlight.GetComponent<SpriteRenderer>().sortingOrder = -1 + (enabled ? 2 : 0);
            GetComponent<SpriteRenderer>().sortingOrder = (enabled ? 2 : 0);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
