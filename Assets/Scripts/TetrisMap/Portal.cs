using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                for (int x = 0; x < MapManager.width; x++)
                    MapManager.portalDistributedHorizontal[x].Clear();
                for (int y = 0; y <= MapManager.realHeight; y++)
                    MapManager.portalDistributedVertical[y].Clear();
                for (int x = 0; x < MapManager.width; x++)
                    for (int y = 0; y <= MapManager.realHeight; y++)
                        if (MapManager.mapGrid[x, y] != null && MapManager.mapGrid[x, y].isPortal == true)
                        {
                            MapManager.portalGrid[x, y] = true;
                            MapManager.portalDistributedHorizontal[x].Add(y);
                            MapManager.portalDistributedVertical[y].Add(x);
                        }
                MapManager.portalDestination = MapManager.currentRoom.mapCoord;
                StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene(GameState.Portal));
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
