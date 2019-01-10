using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(RectTransform))]
public class LifeStoneManager : MonoBehaviour {
    /// <summary>
    /// Location of lifeStoneFrame on Canvas
    /// </summary>
    public Vector2 lifeStoneLocation;
    /// <summary>
    /// standard prefab of every image
    /// </summary>
    public GameObject standardImage;
    /// <summary>
    /// The number of lifeStoneRows
    /// </summary>
    public int lifeStoneRow;
    /// <summary>
    /// Size of lifeStone
    /// </summary>
    public float lifeStoneSize;
    /// <summary>
    /// The sprites
    /// </summary>
    public Sprite[] sprites;
    /// <summary>
    /// super Object of frames
    /// </summary>
    public GameObject frameSuper;
    /// <summary>
    /// super Object fo stones
    /// </summary>
    public GameObject stoneSuper;
    /// <summary>
    /// Array of lifestone
    /// 0 row is the bottom
    /// 0: empty
    /// 1: normal lifestone
    /// 2: gold lifestone
    /// 3: amethyst lifestone
    /// </summary>
    public int[,] lifeStoneArray;
    [HideInInspector]public LifeStoneFrame lifeStoneFrame;

	
	void Start () {
        transform.position = new Vector3(lifeStoneLocation.x, lifeStoneLocation.y, 0);
        lifeStoneFrame = new LifeStoneFrame(frameSuper.transform, standardImage, lifeStoneRow, lifeStoneSize, sprites);
        lifeStoneArray = new int[50, 3];
        for (int i = 0; i < 50; i++) for (int j = 0; j < 3; j++) lifeStoneArray[i, j] = 0;
        
	}
	
	void PushLifeStone(LifeStoneInfo pushInfo)
    {
        Vector2Int pSize = pushInfo.getSize();
        string pFill = pushInfo.getFill();
        int[] minRow = new int[] { lifeStoneRow, lifeStoneRow, lifeStoneRow};

        {
            int i, j, pi, pj;
            for (i = 0; i < 3 - pSize.x; i++)
            {
                for (j = lifeStoneRow - 1; j >= 0; j--)
                {
                    for (pi = 0; pi < pSize.x; pi++)
                    {
                        for (pj = 0; pj < pSize.y; pj++)
                        {
                            if (pFill[pj * pSize.x + pi] != ' ' && lifeStoneArray[j + pj, i + pi] != 0) break;
                        }
                        if (pj != pSize.y) break;
                    }
                    if (pi != pSize.x) break;
                }
            }
        }
    }

}
