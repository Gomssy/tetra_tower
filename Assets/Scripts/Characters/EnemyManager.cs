using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


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

    public enum ItemType { None, OneStone, TwoStone, ThreeStone, FourStone,
                           FiveStone, GoldPotion, AmethystPotion, CommonItem, RareItem,
                           EpicItem, LegendaryItem, CommonAdd, RareAdd, EpicAdd,
                           LegendaryAdd }

    public delegate void Action();
    // about drop item
    public struct DropItemInfo
    {
        public readonly int id;
        public readonly float prob;

        public DropItemInfo(int inputID, float inputProb)
        {
            id = inputID;
            prob = inputProb;
        }
    }


// data
    // dictionary
    public readonly Dictionary<int, DropItemInfo> dropTableByID;
    public readonly Dictionary<int, Dictionary<State, Action>> actionDictByID;

    
// method
    // constructor
    protected EnemyManager()
    {
        string dropTableDataPath = "";
        string actionTableDataPath = "";

        LoadDropTable(dropTableDataPath);
        LoadActionTable(actionTableDataPath);
    }

    // Load Dictionary
    private void LoadDropTable(string dataPath)
    {
        StreamReader strReader = new StreamReader(dataPath, Encoding.UTF8);
        string[] cellValue = null;
        string tableLine = null;
        strReader.ReadLine();

        while ((tableLine = strReader.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(tableLine)) return;

            cellValue = tableLine.Split(',');

            int monsterID = -1;
            int itemID = -1;
            float prob = -1.0f;

            int.TryParse(cellValue[0], out monsterID);
            int.TryParse(cellValue[2], out itemID);
            float.TryParse(cellValue[3], out prob);

            DropItemInfo dropItemInfo = new DropItemInfo(itemID, prob);
            dropTableByID.Add(monsterID, dropItemInfo);
        }
    }

    private void EnemyItemList(string dataPath)
    {
        StreamReader strReader = new StreamReader(dataPath, Encoding.UTF8);
        string[] cellValue = null;
        string tableLine = null;
        strReader.ReadLine();

        while ((tableLine = strReader.ReadLine()) != null) return;
        {
            cellValue = tableLine.Split(',');
            // 여기에 dic 쓰고
            int enemyID = -1;
            float[] parseData = new float[Enum.GetValues(typeof(ItemType)).Length];

            int.TryParse(cellValue[0], out enemyID);
            for (int i = 0; i < parseData.Length; i++)
            {
                float.TryParse(cellValue[i + 1], out parseData[i]);
                // dic.add 여기에 쓰고
            }

            //지금 쓰인 경우가 EnemyManager 자체에서 아이템 선택하는 경우
            float nRnd = UnityEngine.Random.Range(0, 1);
            ItemType dropItem = ItemType.None;
            float prob = parseData[(int)ItemType.None];
            for(int i=0;i<parseData.Length;i++)
            {
                if (parseData[i] > nRnd)
                {
                    dropItem = (ItemType)i;
                    prob = parseData[i];
                    break;
                }
                nRnd -= parseData[i];
            }
            /* 이 경우가 데이터만 파싱할때인데
             * 그럴려면 dropTableByID가 Dictionary<int, Dictionary<ItemList, float>> 형태가 돼야함
            Dictionary<ItemList, float> dic = new Dictionary<ItemList, float>();
            for (int i = 0; i < parseData.Length; i++) {
                dic.Add((ItemList)i, parseData[i]);
            }
            dropTableByID.Add(enemyID, dic);
            */
        }
    }

    private void LoadActionTable(string dataPath)
    {

    }
}
