using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerInteraction  {
    
    bool isPortalUsed = false;

    public void Apply()
    {
        if (GameManager.Instance.isTutorial && !isPortalUsed)
        {
            StartCoroutine(GameManager.Instance.EndTutorial());
            isPortalUsed = true;
        }
        else if (!GameManager.Instance.isTutorial && GameManager.gameState == GameState.Ingame)
            StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Portal));
    }

    public void HighlightSwitch(bool enabled)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
