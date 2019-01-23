using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : Singleton<EnemyManager>
{
    // data
    // data of drop item
    public TextAsset dropTableData;
    public Dictionary<int, int[]> dropTableByID = new Dictionary<int, int[]>();
    public GameObject[] dropItemList; // insert drop item here(on right order)

    // enemy prefab
    public GameObject[] enemyPrefab;

    // method
    // Constructor - protect calling raw constructor
    protected EnemyManager() { }

    // Awake
    private void Awake()
    {
        LoadDropTable(dropTableData);
    }

    // Spawn Enemy to Map
    public void SpawnEnemy()
    {
        GameObject spawnLocation = MapManager.currentRoom.roomInGame.transform.Find("enemy location").gameObject;
    }

    // Load Dictionary
    private void LoadDropTable(TextAsset dataFile)
    {
        string[] linesFromText = dataFile.text.Split('\n');
        string[] cellValue = null;

        int IDindex = 1; // index of monster ID is 1
        int skipDistance = 3; // (Reason for 3): skip StageName, MonsterID, MonsterName (3 elements)

        for (int i = 1; i < linesFromText.Length; i++) // (Reason i = 1): skip first line 
        {
            cellValue = linesFromText[i].Split(',');

            int enemyID = -1;
            int.TryParse(cellValue[IDindex], out enemyID);
            Assert.AreNotEqual(-1, enemyID); // case -1: read error
            if (enemyID == 0) { continue; } // case 0: blank

            int dropTableLength = cellValue.Length - skipDistance;
            Assert.AreEqual(dropTableLength, dropItemList.Length);
            int[] dropTable = new int[dropTableLength];

            int cumulated = 0;

            for (int j = 0; j < dropTableLength; j++)
            {
                int weight = -1;
                int.TryParse(cellValue[j + skipDistance], out weight);
                Assert.AreNotEqual(-1, weight);
                cumulated += weight;
                dropTable[j] = cumulated;
            }
            
            dropTableByID.Add(enemyID, dropTable);
        }
    }
}
