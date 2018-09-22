using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCrystal : MonoBehaviour
{
    /// <summary>
    /// Integer array representation of LifeCrystal shape
    /// <para/> First index: width, 0 on the left
    /// <para/> Second index: height, o on the bottom
    /// </summary>
    public LifeCrystalUI.CellType[][] grid;


	/// <summary>
    /// Initiaization
    /// </summary>
	void Start ()
    {
        int cellCount = transform.childCount;
        LifeCrystalUI.CellType[][] gridCopy = new LifeCrystalUI.CellType[3][]
        {
            new LifeCrystalUI.CellType[3],
            new LifeCrystalUI.CellType[3],
            new LifeCrystalUI.CellType[3]
        };
        int maxX = 1, maxY = 1;

        for(int i = 0; i < cellCount; i++)
        {
            Transform cell = transform.GetChild(i);
            Vector2Int cellPosition = new Vector2Int(Mathf.RoundToInt(cell.localPosition.x), Mathf.RoundToInt(cell.localPosition.y));
            gridCopy[cellPosition.x][cellPosition.y] = LifeCrystalUI.CellType.Life;
            if (maxX < cellPosition.x) maxX = cellPosition.x;
            if (maxY < cellPosition.y) maxY = cellPosition.y;
        }
        grid = new LifeCrystalUI.CellType[maxX + 1][];
        for (int i = 0; i <= maxX; i++)
        {
            grid[i] = new LifeCrystalUI.CellType[maxY + 1];
            for (int j = 0; j <= maxY; j++)
            {
                grid[i][j] = gridCopy[i][j];
            }
        }
        
	}

    /// <summary>
    /// Randomly upgrade cells
    /// </summary>
    /// <param name="tier">Tier</param>
    public void Upgrade(int tier)
    {
    }

}
