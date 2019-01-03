using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class EnemyManager : Singleton<EnemyManager>
{
    public enum State
    {
        Idle,
        Track,
        Attack
    } // 상속을 통해 수정할 가능성 높음. 염두만 해 두자.

    public delegate void Action<T>();

    private Dictionary<uint, int[]> dropTableByID;
    private Dictionary<uint, List<int[]>> actionDictByID;

    protected EnemyManager()
    {
        string dropTableDataPath = null;
        string actionTableDataPath = null;

        LoadDropTable(dropTableDataPath);
        LoadActionTable(actionTableDataPath);
    }

    private void LoadDropTable(string dataPath)
    {
        StreamReader strReader = new StreamReader(dataPath, Encoding.UTF8);
        string[] cellValue = null; //csv파일 한 행에 포함되는 칸들의 값 넣을 배열
        string tableLine = null; //파일 한 행
        strReader.ReadLine(); //첫 줄 스킵

        while ((tableLine = strReader.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(tableLine)) return; //행이 비었는지 체크

            cellValue = tableLine.Split(',');

            uint monsterID = 0;
            int itemID = 0, dropWeight = 0;

            uint.TryParse(cellValue[0], out monsterID);
            int.TryParse(cellValue[2], out itemID);
            int.TryParse(cellValue[3], out dropWeight);

            int[] itemDrop = new int[] { itemID, dropWeight };
            dropTableByID.Add(monsterID, itemDrop);
        }
    }

    private void LoadActionTable(string dataPath)
    {

    }
}
