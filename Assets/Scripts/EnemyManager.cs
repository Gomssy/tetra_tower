﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : Singleton<EnemyManager>
{
    // data
    // static
    private readonly int poolSize = 10;

    public static readonly float goldPer = 0.5f;
    public static readonly int ameNum = 0;
    public static readonly float dropObjStrength = 1f;

    // hold player for animation
    public GameObject player;

    // data of drop item
    public TextAsset dropTableData;
    public Dictionary<int, int[]> dropTableByID = new Dictionary<int, int[]>();

    // enemy prefab
    public GameObject[] enemyPrefab;
    public Dictionary<GameObject, GameObject[]> enemyPool = new Dictionary<GameObject, GameObject[]>();

    // method
    // Constructor - protect calling raw constructor
    protected EnemyManager() { }

    // Awake
    private void Awake()
    {
        player = GameObject.Find("Player");
        LoadDropTable(dropTableData);
        CreateEnemyPool();
    }

    // Spawn Enemy to Map
    public void SpawnEnemy()
    {
        Transform enemySpots = MapManager.currentRoom.roomInGame.transform.Find("enemy spot");
        foreach(Transform enemySpot in enemySpots)
        {
            GameObject enemy = enemySpot.gameObject.GetComponent<enemySpot>().enemyPrefab;
            foreach(Transform location in enemySpot)
            {
                GameObject clone = PickFromPool(enemy);
                clone.transform.position = location.position;
            }
        }
    }

    // Object Pool
    private void CreateEnemyPool()
    {
        foreach(GameObject eachEnemy in enemyPrefab)
        {
            GameObject[] pool = new GameObject[poolSize];
            for(int i = 0; i < pool.Length; i++)
            {
                pool[i] = Instantiate(eachEnemy);
                pool[i].SetActive(false);
            }
            enemyPool.Add(eachEnemy, pool);
        }
    }

    private GameObject PickFromPool(GameObject enemy)
    {
        Debug.Log(enemy.name);
        GameObject[] pool = enemyPool[enemy];
        foreach(GameObject obj in pool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        int beforeExtend = pool.Length;
        Array.Resize(ref pool, pool.Length + poolSize);
        for(int i = beforeExtend; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemy);
            pool[i].SetActive(false);
        }
        enemyPool[enemy] = pool;

        pool[beforeExtend].SetActive(true);
        return pool[beforeExtend];
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