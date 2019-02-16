using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerInteraction  {

    bool isPortalUsed;

    public void Apply()
    {
        if (GameManager.gameState == GameState.Ingame)
            StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Portal));
        else if (GameManager.gameState == GameState.Tutorial && !isPortalUsed)
        {
            isPortalUsed = true;
            StartCoroutine(GameManager.Instance.StartGame());
        }
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
