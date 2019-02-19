using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerInteraction  {

    public bool isPortalTutorial = false;
    bool isPortalUsed = false;

    public void Apply()
    {
        if (isPortalTutorial && !isPortalUsed)
        {
            StartCoroutine(GameManager.Instance.EndTutorial());
            isPortalUsed = true;
        }
        else if (GameManager.gameState == GameState.Ingame && !isPortalTutorial)
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
