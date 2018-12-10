using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager> {
    public enum State {
        Idle,
        Track,
        Attack
    } // 상속을 통해 수정할 가능성 높음. 염두만 해 두자.

    public delegate void Action<T>();

    private Dictionary<uint, List<int[]>> dropTableByID;
    private Dictionary<uint, List<int[]>> actionDictByID;

    protected EnemyManager() {
        string dropTableDataPath = null;
        string actionTableDataPath = null;

        LoadDropTable(dropTableDataPath);
        LoadActionTable(actionTableDataPath);
    }
    private void LoadDropTable(string dataPath) {

    }
    private void LoadActionTable(string dataPath) {

    }
}
