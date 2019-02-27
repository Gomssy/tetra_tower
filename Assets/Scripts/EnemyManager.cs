using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : Singleton<EnemyManager>
{
    // data
    // static
    public static readonly float goldPer = 0.5f;
    public static readonly int ameNum = 0;
    public static readonly float dropObjStrength = 1f;
    // hold player for animation
    public GameObject player { get; private set; }

    public LayerMask layerMaskPlatform;
    public LayerMask layerMaskWall;

    // data of drop item
    [SerializeField]
    private TextAsset dropTableData;
    public Dictionary<int, int[]> DropTableByID { get; private set; }

    // enemy prefab and pool
    [SerializeField]
    private GameObject[] enemyPrefab;
    private readonly int poolSize = 10;
    public Dictionary<GameObject, GameObject[]> EnemyPool { get; private set; }


    // enemy count
    [SerializeField]
    private uint EnemySpawnCount;
    public uint EnemyDeadCount;


    // method
    // Constructor - protect calling raw constructor
    protected EnemyManager() { }

    // Awake
    private void Awake()
    {
        LoadDropTable(dropTableData);
        CreateEnemyPool();
    }
    private void Start()
    {
        player = GameManager.Instance.player;
    }

    // Spawn Enemy to Map
    public void SpawnEnemyToMap()
    {
        EnemySpawnCount = EnemyDeadCount = 0;
        Transform enemySpots = MapManager.currentRoom.roomInGame.transform.Find("enemy spot");
        foreach(Transform enemySpot in enemySpots)
        {
            GameObject enemy = enemySpot.gameObject.GetComponent<enemySpot>().enemyPrefab;
            foreach(Transform location in enemySpot)
            {
                GameObject clone = PickFromPool(enemy);
                clone.transform.position = location.position;
                clone.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
            }
        }
    }

    // Spawn Enemy to Map
    public void SpawnEnemyToMap_forTest()
    {
        EnemySpawnCount = EnemyDeadCount = 0;
        Transform enemySpots = GameObject.Find("Grid").transform.GetChild(0).GetChild(0).Find("enemy spot");
        foreach (Transform enemySpot in enemySpots)
        {
            if (!enemySpot.gameObject.activeSelf) continue;
            GameObject enemy = enemySpot.gameObject.GetComponent<enemySpot>().enemyPrefab;
            foreach (Transform location in enemySpot)
            {
                GameObject clone = PickFromPool(enemy);
                clone.transform.position = location.position;
                clone.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
            }
        }
    }

    public bool IsClear()
    {
        return (EnemyDeadCount == EnemySpawnCount);
    }

    // Object Pool
    private void CreateEnemyPool()
    {
        EnemyPool = new Dictionary<GameObject, GameObject[]>();
        foreach (GameObject eachEnemy in enemyPrefab)
        {
            GameObject[] pool = new GameObject[poolSize];
            for(int i = 0; i < pool.Length; i++)
            {
                pool[i] = Instantiate(eachEnemy);
                pool[i].SetActive(false);
            }
            EnemyPool.Add(eachEnemy, pool);
        }
    }

    private GameObject PickFromPool(GameObject enemy)
    {
        EnemySpawnCount += 1;
        GameObject[] pool = EnemyPool[enemy];
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
        EnemyPool[enemy] = pool;

        pool[beforeExtend].SetActive(true);
        return pool[beforeExtend];
    }

    // Load Dictionary
    private void LoadDropTable(TextAsset dataFile)
    {
        DropTableByID = new Dictionary<int, int[]>();
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
            
            DropTableByID.Add(enemyID, dropTable);
        }
    }
}
