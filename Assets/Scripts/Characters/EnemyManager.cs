using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class EnemyManager : Singleton<EnemyManager>
{
    // static variable
    // about action
    public enum State
    {
        Idle,
        Track,
        Attack
    } // 상속을 통해 수정할 가능성 높음. 염두만 해 두자.

    public enum EnemyData { Health, Weight, Height, Width, DetectRange,
        AtkRange, AtkDistance, AtkDelay, PjtSpeed, MoveSpeed,
        Damage } //Atk = Attack, Pjt = Projectile(투사체)
    /* 기본적으로 각 경우에 대해 가지는 값은 양수일 것
     * 하지만 특별한 경우가 있음
     * 1. DetectRange(감지 범위) : DetectRange가 -1이면 방 전체를 감지한다는 뜻. -2이면 현재 에너미가 있는 플랫폼 전체, -3이면 플랫폼 전방만 감지를 뜻 함.
     * 2. AtkRange(공격 범위) : -1이면 현재 위치한 플랫폼 전체를 의미, -2이면 플랫폼 전방만 의미.
     * 3. AtkDistance(공격 사거리) : -1이면 현재 위치한 플랫폼 끝까지 의미.
     * 4. AtkDelay(공격 딜레이) : -1이면 무한 즉, 단 1회 공격함.
     */
    public delegate void Action();


    // data
    // dictionary
    public readonly Dictionary<int, Dictionary<ItemType, int>> dropTableByID;
    public readonly Dictionary<int, Dictionary<State, Action>> actionDictByID;
    public readonly Dictionary<int, Dictionary<EnemyData, float>> enemyDataByID;

    public GameObject enemyPrefab;
    // method
    // constructor
    protected EnemyManager()
    {
        string dropTableDataPath = "";
        string actionTableDataPath = "";

        LoadDropTable(dropTableDataPath);
    }

    // Load Dictionary
    private void LoadDropTable(string dataPath)
    {
        StreamReader strReader = new StreamReader(dataPath, Encoding.UTF8);
        string[] cellValue = null;
        string tableLine = null;
        strReader.ReadLine();
        Dictionary<ItemType, int> dropItemInfo = new Dictionary<ItemType, int>();

        while ((tableLine = strReader.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(tableLine)) return;

            cellValue = tableLine.Split(',');

            int enemyID = -1;
            int[] weight = {-1};
            int sum = 0;
            int[] cumulatedWeight = { -1 };

            int.TryParse(cellValue[0], out enemyID);
            for(int i=1;i<17;i++)
            {
                int.TryParse(cellValue[i+1], out weight[i]);
            }

            for(int i=0;i<16;i++)
            {
                sum += weight[i];
                cumulatedWeight[i] = sum;
            }
            
            for(int i=0;i<16;i++)
            {
                dropItemInfo.Add((ItemType)i, cumulatedWeight[i]);
            }

            
            dropTableByID.Add(enemyID, dropItemInfo);
        }
    }

   

    // Load ALL CSV Data
    private void LoadEnemyData(string dataPath)
    {
        StreamReader strReader = new StreamReader(dataPath, Encoding.UTF8);
        string[] cellValue = null;
        string tableLine = null;
        strReader.ReadLine();
        Dictionary<EnemyData, float> EnemyInfo = new Dictionary<EnemyData, float>();
        while ((tableLine = strReader.ReadLine()) != null)
        {
            cellValue = tableLine.Split(',');

            int enemyID = -1;
            float[] enemyData = { 0.0f };
           
            int.TryParse(cellValue[0], out enemyID);
            for(int i=0;i<11;i++)
            {
                float.TryParse(cellValue[i + 2], out enemyData[i]);
            }

            for(int i=0;i<12;i++)
            {
                EnemyInfo.Add((EnemyData)i, enemyData[i]);
            }

            enemyDataByID.Add(enemyID, EnemyInfo);
        }
    }
    // called by gameManager to Spawn enemy
    // little temporary. Many change will be exist.
    public void SpawnEnemy()
    {
        Vector2 playerPosition = GameObject.Find("Player").transform.position;
        Instantiate(enemyPrefab, playerPosition + new Vector2(7, 0), Quaternion.identity);
    }
}
