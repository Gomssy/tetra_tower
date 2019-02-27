using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoomInGame : RoomInGame {

    GameObject portal;

	// Use this for initialization
	void Start () {
        EnemyManager.Instance.SpawnEnemyToMap();
        foreach (Transform child in transform.Find("item spot"))
        {
            if (child.name.Contains("LifeStone1"))
                LifeStoneManager.Instance.InstantiateDroppedLifeStone(new LifeStoneInfo(new Vector2Int(1, 2), "BA"), child.position, 0);
            else if (child.name.Contains("LifeStone2"))
                LifeStoneManager.Instance.InstantiateDroppedLifeStone(new LifeStoneInfo(new Vector2Int(1, 1), "A"), child.position, 0);
            else if (child.name.Contains("Item"))
                InventoryManager.Instance.ItemInstantiate("Dagger", child.position, 0);
            else if (child.name.Contains("Addon"))
                InventoryManager.Instance.AddonInstantiate("ParchmentPiece", child.position, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
