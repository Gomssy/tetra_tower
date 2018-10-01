using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Healthbar manager
/// </summary>
public class LifeCrystalUI : MonoBehaviour
{
    ////////////////////////////////////////////////// Private variables
    /// <summary>
    /// Enum for cell types
    /// <para/> [Empty = 0, Life = 1, Gold = 2, Ame = 3]
    /// </summary>
    public enum CellType{Empty, Life, Gold, Ame };
    /// <summary>
    /// Image renderers of cells
    /// <para/> First index = horizontal position (0 on the bottom)
    /// <para/> Second index = vertical position (0 on the left)
    /// </summary>
    Image[][] cellImg;
    /// <summary>
    /// Distance between cells
    /// </summary>
    float gridSize;
    /// <summary>
    /// Position of bottom left corner (pivot point)
    /// </summary>
    Vector3 pivotPosition;
    /// <summary>
    /// Current cell state (enum Color)
    /// <para/> First index = horizontal position (0 on the bottom)
    /// <para/> Second index = vertical position (0 on the left)
    /// </summary>
    CellType[][] cell;
    ////////////////////////////////////////////////// Public variables
    /// <summary>
    /// Cell sprites
    /// </summary>
    public Sprite[] cellSprites;
    /// <summary>
    /// Cell object prefab
    /// </summary>
    public GameObject cellObj;
    /// <summary>
    /// Row object prefab
    /// </summary>
    public GameObject rowObj;
    /// <summary>
    /// LifeCrystal enabled if isDead == false
    /// </summary>
    public bool isDead;
    /// <summary>
    /// Text output, for debugging purpose
    /// </summary>
    public Text textUI;
    ////////////////////////////////////////////////// Protected variables
    private int lifeCount, goldCount, height;
    /// <summary>
    /// Current life total
    /// </summary>
    int LifeCount
    {
        get { return lifeCount; }
        set { }
    }
    /// <summary>
    /// Current gold total
    /// </summary>
    int GoldCount
    {
        get { return goldCount; }
        set { }
    }
    /// <summary>
    /// Current height of LifeCrystalUI
    /// </summary>
	int Height
    {
        get { return height; }
        set { }
    }
    ////////////////////////////////////////////////// Public functions
    /// <summary>
    /// Take given amount of damage
    /// </summary>
    /// <param name="dmg">Amount of damage to take</param>
    public void TakeDamege(int dmg)
    {
        if (isDead) return;

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 2; x >= 0; x--)
            {
                if (dmg > 0 && lifeCount > 0 && cell[x][y] != CellType.Empty)
                {
                    if (cell[x][y] == CellType.Gold)
                        goldCount--;
                    updateCell(x, y, CellType.Empty);
                    dmg--;
                    lifeCount--;
                }
            }
            if (dmg == 0)
                break;
        }
        if (lifeCount <= 0)
        {
            lifeCount = 0;
            GameOver();
        }
    }
    /// <summary>
    /// Gain given amount of gold
    /// </summary>
    /// <param name="gold">Amount of gold to gain</param>
    public void GainGold(int gold)
    {
        if (isDead) return;

        int redLife = lifeCount - goldCount;
        if (gold > redLife)
            gold = redLife;
        goldCount += gold;

        int[] mixer = new int[redLife];
        for (int i = 0; i < gold; i++)
            mixer[i] = 1;
        for (int i = gold; i < redLife; i++)
            mixer[i] = 0;
        for (int i = 0; i < redLife - 1; i++)
        {
            int rand = Random.Range(i, redLife);
            int temp = mixer[i];
            mixer[i] = mixer[rand];
            mixer[rand] = temp;
        }

        int redLifeIndex = 0;
        for (int y = 0; y < height; y++)
            for (int x = 0; x < 3; x++)
            {
                if (cell[x][y] == CellType.Life)
                {
                    if (mixer[redLifeIndex] == 1)
                        updateCell(x, y, CellType.Gold);
                    redLifeIndex++;
                }
            }
    }
    /// <summary>
    /// Testing purpose only
    /// </summary>
    public void GainSampleLifeCrystal(LifeCrystal frag)
    {
        GainLifeCrystal(frag);
    }
    /// <summary>
    /// Testing purpose only
    /// </summary>
    public void Use3Gold()
    {
        UseGold(3);
    }
    /// <summary>
    /// Gain a LifeCrystal
    /// </summary>
    /// <param name="frag">LifeCrystal to gain</param>
    /// <returns>returns false if no cell taken, true otherwise</returns>
    public bool GainLifeCrystal(LifeCrystal frag)
    {
        if (isDead) return false;
        
        Vector2Int dropxy = dropSimulation(frag.grid);
        if (dropxy.y == 0) return false;

        StartCoroutine(DropBlock(frag, dropxy));

        return true;
    }
    /// <summary>
    /// Use given amount of gold
    /// </summary>
    /// <param name="gold">Amount of gold to use</param>
    /// <returns>Returns false when money not used, true when used</returns>
    public bool UseGold(int gold)
    {
        if (isDead) return false;

        if (goldCount < gold)
            return false;
        goldCount -= gold;
        for (int y = height - 1; y >= 0; y--)
            for (int x = 2; x >= 0; x--)
                if (cell[x][y] == CellType.Gold && gold > 0)
                {
                    updateCell(x, y, CellType.Life);
                    gold--;
                }
        return true;
    }
    ////////////////////////////////////////////////// Basic functions
    /// <summary>
    /// Initialization
    /// </summary>
	void Start ()
    {
        /// Initializing variables
        gridSize = transform.Find("Frame").Find("Row1").localPosition.y - transform.Find("Frame").Find("Row0").localPosition.y;
        pivotPosition = transform.Find("Frame").Find("Row0").localPosition;
        pivotPosition.z = 0;
        isDead = false;
        height = 8;
        lifeCount = 24;
        goldCount = 0;
        cell = new CellType[3][] { new CellType[height], new CellType[height], new CellType[height] };
        for (int y = 0; y < height; y++)
            for (int x = 0; x < 3; x++)
                cell[x][y] = CellType.Life;
        cellImg = new Image[3][] { new Image[height], new Image[height], new Image[height] };
        for (int i = 0; i < 3 * height; i++)
            cellImg[i % 3][i / 3] = transform.Find("Cells").Find("LC" + (i + 1).ToString("00")).GetComponent<Image>();
    }
	/// <summary>
    /// Runs each frame
    /// </summary>
	void FixedUpdate ()
    {
        //textUI.text = "Life: " + lifeCount + ", Gold: " + goldCount;
    }
    ////////////////////////////////////////////////// Hidden functions
    /// <summary>
    /// Updates image of a cell; When empty, disable it
    /// </summary>
    /// <param name="x">X position of the cell to update: 0~2 (0 on the left)</param>
    /// <param name="y">Y position of the cell to update: 0~(height-1) (0 on the bottom)</param>
    /// <param name="value"></param>
    void updateCell(int x, int y, CellType value)
    {
        cell[x][y] = value;
        cellImg[x][y].sprite = cellSprites[(int)value];
        cellImg[x][y].transform.localPosition = pivotPosition + gridSize * new Vector3(x, y, 0);
        cellImg[x][y].enabled = value == CellType.Empty ? false : true;
    }
    /// <summary>
    /// Runs when life goes below zero
    /// </summary>
    void GameOver()
    {
        isDead = true;
    }
    /// <summary>
    /// Simulates a block drop and find where to drop it
    /// </summary>
    /// <param name="grid">Shape of the dropping block
    /// <para/> grid[X][Y]
    /// <para/> X: 0~2 (0 on the bottom)
    /// <para/> Y: 0~(height-1) (0 on the left)</param>
    /// <returns>Vector2(drop position(0~2), drop depth(0~height))</returns>
    Vector2Int dropSimulation(CellType[][] grid)
    {
        int bWidth = grid.Length;
        int bHeight = grid[0].Length;

        if (bWidth >= 4 || bWidth <= 0)
            return Vector2Int.zero;

        ///////////////////////////////calculates texture
        int[] emptyTexture = new int[3];
        int[] blockTexture = new int[bWidth];

        for (int x = 0; x < bWidth; x++)
        {
            int depth = 0;
            for (depth = 0; depth < bHeight; depth++)
                if (grid[x][depth] != CellType.Empty)
                    break;
            if (depth >= height)
                depth = 0;
            blockTexture[x] = depth;
        }
        for (int x = 0; x <= 2; x++)
        {
            int depth = 0;
            for (int y = height - 1; y >= 0; y--)
            {
                if (cell[x][y] == 0)
                    depth++;
                else
                    break;
            }
            emptyTexture[x] = depth;
        }

        ///////////////////////////////calculates possible drops
        int[] dropDepth = new int[4 - bWidth];
        for (int x = 0; x < 4 - bWidth; x++)
        {
            int maxDepth = emptyTexture[x] + blockTexture[0];
            for (int bx = 1; bx < bWidth; bx++)
            {
                if (maxDepth > emptyTexture[x + bx] + blockTexture[bx])
                    maxDepth = emptyTexture[x + bx] + blockTexture[bx];
            }
            dropDepth[x] = maxDepth;
        }
        int realPosition = 0, realDepth = dropDepth[0];
        for (int x = 1; x < 4 - bWidth; x++)
            if (realDepth < dropDepth[x])
            {
                realPosition = x;
                realDepth = dropDepth[x];
            }

        return new Vector2Int(realPosition, realDepth);
    }
    /// <summary>
    /// Coroutine for dropping LifeCrystal fragment; Animation purpose
    /// </summary>
    /// <param name="frag">LifeCrystal script object</param>
    /// <param name="dropxy">Drop info: Vector2Int(drop position, drop depth)</param>
    /// <returns>Null</returns>
    IEnumerator DropBlock(LifeCrystal frag, Vector2Int dropxy)
    {
        int bWidth = frag.grid.Length;
        int bHeight = frag.grid[0].Length;
        List<Vector2Int> droppingCells = new List<Vector2Int>(0);
        
        for (int y = 0; y < bHeight && height - dropxy.y + y <= height - 1; y++)
            for (int x = 0; x < bWidth; x++)
            {
                if (frag.grid[x][y] != CellType.Empty)
                {
                    updateCell(dropxy.x + x, height - dropxy.y + y, frag.grid[x][y]);
                    droppingCells.Add(new Vector2Int(dropxy.x + x, height - dropxy.y + y));
                    lifeCount++;
                }
            }
        foreach (Vector2Int block in droppingCells)
        {
            cellImg[block.x][block.y].transform.localPosition = pivotPosition + gridSize * new Vector3(block.x, block.y + dropxy.y, 0);
        }
        yield return new WaitForSeconds(0.05f);

        for (int i = 2 * dropxy.y; i > 0; i--)
        {
            foreach (Vector2Int block in droppingCells)
            {
                cellImg[block.x][block.y].transform.position -= gridSize * new Vector3(0, 0.5f, 0);
            }
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }
}
