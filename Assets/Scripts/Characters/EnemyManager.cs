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
        string dropTableDataPath = null;
        string actionTableDataPath = null;

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

    private void LoadActionTable(string dataPath)
    {

    }
}
