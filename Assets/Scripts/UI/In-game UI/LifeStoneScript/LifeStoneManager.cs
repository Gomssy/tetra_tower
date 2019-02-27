using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LifeStoneManager : Singleton<LifeStoneManager> {
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
    public int lifeStoneRowNum;
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
	/// lifeStoneUnit Prefab
	/// </summary>
	public GameObject lifeUnitPrefab;
    public GameObject goldPotionPrefab;
	/// <summary>
	/// strength of vibration when Lifestone falls
	/// </summary>
	public float vibrationVariable;
    /// <summary>
    /// Array of lifestone
    /// 0 row is the bottom
    /// 0: empty
    /// 1: normal lifestone
    /// 2: gold lifestone
    /// 3: amethyst lifestone
    /// </summary>
    public int[,] lifeStoneArray;
	/// <summary>
	/// Array of lifestone GameObject
	/// </summary>
	[HideInInspector]public GameObject[,] lifeStoneUnit;

	public GameObject droppedLifeStonePrefab;

    public GameObject lifeStoneUI;

    public float frameBorder;

    public float popoutStrengthMultiplier;
    public float popoutTime;

    bool stoneCut;

    void Start () {
        lifeStoneUI.transform.position = new Vector3(lifeStoneLocation.x, lifeStoneLocation.y, 0);
        frameSuper.GetComponent<LifeStoneFrame>().Init(frameSuper.transform, standardImage, lifeStoneRowNum, lifeStoneSize, sprites, frameBorder);
        lifeStoneArray = new int[50, 3];
		lifeStoneUnit = new GameObject[50, 3];
        for (int i = 0; i < 50; i++) for (int j = 0; j < 3; j++) lifeStoneArray[i, j] = 0;
        PushLifeStone(CreateLifeStoneInfo(new Vector2Int(3, 6), 0, 0));
        StartCoroutine("TestEnumerator");
    }
	IEnumerator TestEnumerator()
	{
        yield return null;

    }

    /// <summary>
    /// expand lifestoneframe row
    /// </summary>
    /// <param name="rowNum"></param>
    public void ExpandRow(int rowNum)
    {
        lifeStoneRowNum += rowNum;
        frameSuper.GetComponent<LifeStoneFrame>().AddRow(lifeStoneRowNum);
    }

    public GameObject InstantiatePotion(Vector3 pos, float popoutStrength)
    {
        GameObject tmpPotion = Instantiate(goldPotionPrefab, pos, Quaternion.identity);
        PopoutGenerator(tmpPotion, popoutStrength);
        return tmpPotion;
    }
    public GameObject InstantiatePotion(Vector3 pos, int _price, float popoutStrength)
    {
        GameObject tmpPotion = Instantiate(goldPotionPrefab, pos, Quaternion.identity);
        PopoutGenerator(tmpPotion, popoutStrength);
        tmpPotion.GetComponent<DroppedObject>().price = _price;
        tmpPotion.GetComponent<DroppedObject>().priceTag = Instantiate(InventoryManager.Instance.price, new Vector3(0, 0, 0), Quaternion.identity, GameManager.Instance.textCanvas.transform);
        return tmpPotion;
    }

    IEnumerator PopoutCoroutine(GameObject obj)
    {
        float endTime = Time.time + popoutTime;
        Vector2 orgScale = obj.transform.localScale;
        SpriteRenderer[] sprtArr = obj.GetComponents<SpriteRenderer>();

        while (Time.time < endTime)
        {
            obj.transform.localScale = (1 - ((endTime - Time.time) / popoutTime)) * orgScale;
            foreach (SpriteRenderer sprt in sprtArr)
                sprt.color = new Color(sprt.color.r, sprt.color.g, sprt.color.b, 1 - ((endTime - Time.time) / popoutTime));
            yield return null;
        }

        obj.transform.localScale = orgScale;
        foreach (SpriteRenderer sprt in sprtArr)
            sprt.color = new Color(sprt.color.r, sprt.color.g, sprt.color.b, 1f);
    }
    void PopoutGenerator(GameObject obj, float popoutStrength)
    {
        popoutStrength *= popoutStrengthMultiplier;
        float angle = Mathf.Deg2Rad * Random.Range(80f, 100f);
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * popoutStrength;
        StartCoroutine(PopoutCoroutine(obj));
    }



    /// <summary>
    /// Instantiate Dropped LifeStone
    /// </summary>
    /// <param name="info"></param>
    /// <param name="pos"></param>
    public GameObject InstantiateDroppedLifeStone(Vector2Int size, int num, float goldPer, int ameNum, Vector3 pos, float popoutStrength)
    {
        GameObject tmpObj = Instantiate(droppedLifeStonePrefab);
        tmpObj.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        tmpObj.GetComponent<DroppedLifeStone>().Init(CreateLifeStoneInfo(size, num, goldPer, ameNum), pos);
        PopoutGenerator(tmpObj, popoutStrength);
        return tmpObj;
    }
    public GameObject InstantiateDroppedLifeStone(Vector2Int size, float goldPer, int ameNum, Vector3 pos, float popoutStrength)
    {
        GameObject tmpObj = Instantiate(droppedLifeStonePrefab);
        tmpObj.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        tmpObj.GetComponent<DroppedLifeStone>().Init(CreateLifeStoneInfo(size, goldPer, ameNum), pos);
        PopoutGenerator(tmpObj, popoutStrength);
        return tmpObj;
    }
    public GameObject InstantiateDroppedLifeStone(int num, float goldPer, int ameNum, Vector3 pos, float popoutStrength)
    {
        GameObject tmpObj = Instantiate(droppedLifeStonePrefab);
        tmpObj.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        tmpObj.GetComponent<DroppedLifeStone>().Init(CreateLifeStoneInfo(num, goldPer, ameNum), pos);
        PopoutGenerator(tmpObj, popoutStrength);
        return tmpObj;
    }
    public GameObject InstantiateDroppedLifeStone(LifeStoneInfo info, Vector3 pos, float popoutStrength)
    {
        GameObject tmpObj = Instantiate(droppedLifeStonePrefab);
        tmpObj.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        tmpObj.GetComponent<DroppedLifeStone>().Init(info, pos);
        PopoutGenerator(tmpObj, popoutStrength);
        return tmpObj;
    }
    public GameObject InstantiateDroppedLifeStone(Vector2Int size, float goldPer, int ameNum, Vector3 pos, int _price, float popoutStrength)
    {
        GameObject tmpObj = Instantiate(droppedLifeStonePrefab);
        tmpObj.transform.SetParent(MapManager.currentRoom.roomInGame.transform);
        tmpObj.GetComponent<DroppedLifeStone>().Init(CreateLifeStoneInfo(size, goldPer, ameNum), pos);
        PopoutGenerator(tmpObj, popoutStrength);
        tmpObj.GetComponent<DroppedObject>().price = _price;
        tmpObj.GetComponent<DroppedObject>().priceTag = Instantiate(InventoryManager.Instance.price, new Vector3(0, 0, 0), Quaternion.identity, GameManager.Instance.textCanvas.transform);return tmpObj;
    }

    /// <summary>
    /// Randomize LifeStone by size, num, gold probablity, number of ametyst 
    /// </summary>
    /// <param name="size"></param>
    /// <param name="num"></param>
    /// <param name="goldPer"></param>
    /// <param name="ameNum"></param>
    /// <returns></returns>
    public LifeStoneInfo CreateLifeStoneInfo(Vector2Int size, int num, float goldPer, int ameNum)
    {
        System.Random rnd = new System.Random();
        num = Mathf.Max(1, num);
        size.x = Mathf.Min(3, size.x);
        if (num > size.x * size.y)
            return CreateLifeStoneInfo(size, goldPer, ameNum);

        int[,] tmpArray = new int[size.y, size.x] ;
        for (int j = 0; j < size.y; j++)
            for (int i = 0; i < size.x; i++)
                tmpArray[j, i] = 0;

        tmpArray[rnd.Next(size.y), rnd.Next(size.x)] = 1;

        //making shape of lifestone
        for(int n = 1; n < num; n++)
        {
            ArrayList candArray = new ArrayList();
            for (int j = 0; j < size.y; j++)
                for (int i = 0; i < size.x; i++)
                    //check if adjacent cell is lifestone
                    if(tmpArray[j,i] == 0 &&
                        (j - 1 >= 0 && tmpArray[j - 1, i] == 1 ||
                        j + 1 < size.y && tmpArray[j + 1, i] == 1 ||
                        i - 1 >= 0 && tmpArray[j, i - 1] == 1 ||
                        i + 1 < size.x && tmpArray[j, i + 1] == 1))
                        candArray.Add(new Vector2Int(i, j));
            if (candArray.Count == 0) break;
            Vector2Int vtmp = (Vector2Int)candArray[rnd.Next(candArray.Count)];
            tmpArray[vtmp.y, vtmp.x] = 1;
        }

        //recalibrate the size
        Vector2Int maxPoint = new Vector2Int(-1, -1);
        Vector2Int minPoint = new Vector2Int(size.x + 1, size.y + 1);
        for (int j = 0; j < size.y; j++)
            for (int i = 0; i < size.x; i++)
                if(tmpArray[j,i] == 1)
                {
                    maxPoint.x = Mathf.Max(i, maxPoint.x);
                    maxPoint.y = Mathf.Max(j, maxPoint.y);
                    minPoint.x = Mathf.Min(i, minPoint.x);
                    minPoint.y = Mathf.Min(j, minPoint.y);
                }
        size = maxPoint - minPoint + Vector2Int.one;

        //making fill string
        string fill = "";
        for (int j = minPoint.y; j <= maxPoint.y; j++)
            for (int i = minPoint.x; i <= maxPoint.x; i++)
                if (tmpArray[j, i] == 1) fill += 'A';
                else fill += ' ';

        //change to amethyst
        ArrayList sCandArray = new ArrayList();
        for (int i = 0; i < fill.Length; i++)
            if (fill[i] == 'A')
                sCandArray.Add(i);
        char[] repChar = fill.ToCharArray();
        for(int i = 0; i < ameNum && sCandArray.Count > 0; i++)
        {
            int tmp = rnd.Next(sCandArray.Count);
            repChar[(int)sCandArray[tmp]] = 'C';
            sCandArray.RemoveAt(tmp);
        }
        for (int i = 0; i < fill.Length; i++)
            if (repChar[i] == 'A' && Random.Range(0f, 1f) < goldPer)
                repChar[i] = 'B';
        fill = new string(repChar);
        
        return new LifeStoneInfo(size, fill);

    }
    public LifeStoneInfo CreateLifeStoneInfo(Vector2Int size, float goldPer, int ameNum)
    {
        return CreateLifeStoneInfo(size, size.x * size.y, goldPer, ameNum);
    }
    public LifeStoneInfo CreateLifeStoneInfo(int num, float goldPer, int ameNum)
    {
        return CreateLifeStoneInfo(new Vector2Int(3, 20), num, goldPer, ameNum);
    }
    public LifeStoneInfo CreateLifeStoneInfo(LifeStoneInfo lifeStoneInfo)
    {
        Vector2Int size = lifeStoneInfo.getSize();
        Vector2Int newSize;
        string fill = lifeStoneInfo.getFill();
        string newFill = "";
        Vector2Int maxPoint = new Vector2Int(-1, -1);
        Vector2Int minPoint = new Vector2Int(size.x + 1, size.y + 1);
        for (int j = 0; j < size.y; j++)
            for (int i = 0; i < size.x; i++)
                if (fill[j * size.x + i] != ' ')
                {
                    maxPoint.x = Mathf.Max(i, maxPoint.x);
                    maxPoint.y = Mathf.Max(j, maxPoint.y);
                    minPoint.x = Mathf.Min(i, minPoint.x);
                    minPoint.y = Mathf.Min(j, minPoint.y);
                }
        newSize = maxPoint - minPoint + Vector2Int.one;
        
        for(int j = minPoint.y; j <= maxPoint.y; j++)
            newFill += fill.Substring(j * size.x + minPoint.x, newSize.x);


        return new LifeStoneInfo(newSize, newFill);
    }

	/// <summary>
	/// push LifeStone in LifeStoneFrame
	/// </summary>
	/// <param name="pushInfo"></param>
	public bool PushLifeStone(LifeStoneInfo pushInfo)
	{
		System.Random rnd = new System.Random();
		Vector2Int pSize = pushInfo.getSize();
		string pFill = pushInfo.getFill();
		int[] minRow = new int[] { lifeStoneRowNum, lifeStoneRowNum, lifeStoneRowNum };
		int selectedCol = 0, selectedRow = lifeStoneRowNum;
		ArrayList selColCand = new ArrayList();
		{
			int i, j, pi, pj;
			for (i = 0; i <= 3 - pSize.x; i++)
			{
				for (j = lifeStoneRowNum - 1; j >= 0; j--)
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
					minRow[i] = j;
				}
			}
		}

		for (int i = 0; i <= 3 - pSize.x; i++)
			if (minRow[i] < selectedRow) selectedRow = minRow[i];

        if (selectedRow == lifeStoneRowNum)
        {
            GameManager.Instance.DisplayText("생명석 자리가 없습니다!");
            return false;
        }

		for (int i = 0; i <= 3 - pSize.x; i++)
			if (minRow[i] == selectedRow) selColCand.Add(i);


		selectedCol = (int)selColCand[rnd.Next(selColCand.Count)];

		float vibration = pushInfo.getAmount() * vibrationVariable * lifeStoneSize;
        int cutRow = pSize.y;

		for (int pj = 0; pj < pSize.y; pj++)
		{
            if (cutRow == pSize.y && selectedRow + pj >= lifeStoneRowNum)
            {
                cutRow = pj;
                //break;
            }
			for (int pi = 0; pi < pSize.x; pi++)
				if (pFill[pj * pSize.x + pi] != ' ')
				{
					int xtmp = selectedCol + pi, ytmp = selectedRow + pj;
                    GameObject tmpObj;
                    tmpObj = Instantiate(lifeUnitPrefab, stoneSuper.transform);

                    if (pj < cutRow)
                    {
                        lifeStoneArray[ytmp, xtmp] = pFill[pj * pSize.x + pi] - 'A' + 1;
                        lifeStoneUnit[ytmp, xtmp] = tmpObj;
                    }
                    
                    tmpObj.GetComponent<LifeUnitInFrame>().Init(
                        pFill[pj * pSize.x + pi] - 'A' + 1, 
						lifeStoneSize, 
						new Vector2Int(xtmp, ytmp), 
						new Vector2Int(xtmp, lifeStoneRowNum + pj), 
						new Vector2(frameBorder * lifeStoneSize, frameBorder * lifeStoneSize),
                        true,
						vibration);
					vibration = 0;
				}
		}
        
        stoneCut = false;
        if (cutRow < pSize.y)
        {
            StartCoroutine(CutCoroutine(cutRow, pSize, pFill));
        }
        return true;
	}
    
    public void StoneCutStart()
    {
        stoneCut = true;
    }

    IEnumerator CutCoroutine(int cutRow, Vector2Int pSize, string pFill)
    {
        while (!stoneCut)
        {
            yield return null;
        }

        char[] chFill = pFill.ToCharArray();
        for (int i = 0; i < pSize.x; i++)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            char[] newFill = new char[pSize.x * (pSize.y - cutRow)];
            for (int t = 0; t < pSize.x * (pSize.y - cutRow); t++) newFill[t] = ' ';
            if (chFill[cutRow * pSize.x + i] != ' ')
            {
                queue.Enqueue(new Vector2Int(i, cutRow));
                while (queue.Count > 0)
                {
                    Vector2Int vtmp = queue.Dequeue();
                    newFill[(vtmp.y - cutRow) * pSize.x + vtmp.x] = chFill[vtmp.y * pSize.x + vtmp.x];
                    chFill[vtmp.y * pSize.x + vtmp.x] = ' ';
                    if (vtmp.x + 1 < pSize.x && chFill[vtmp.y * pSize.x + (vtmp.x + 1)] != ' ') queue.Enqueue(new Vector2Int(vtmp.x + 1, vtmp.y));
                    if (vtmp.x - 1 >= 0 && chFill[vtmp.y * pSize.x + (vtmp.x - 1)] != ' ') queue.Enqueue(new Vector2Int(vtmp.x - 1, vtmp.y));
                    if (vtmp.y + 1 < pSize.y && chFill[(vtmp.y + 1) * pSize.x + vtmp.x] != ' ') queue.Enqueue(new Vector2Int(vtmp.x, vtmp.y + 1));
                    if (vtmp.y - 1 >= cutRow && chFill[(vtmp.y - 1) * pSize.x + vtmp.x] != ' ') queue.Enqueue(new Vector2Int(vtmp.x, vtmp.y - 1));
                }
                InstantiateDroppedLifeStone(CreateLifeStoneInfo(
                    new LifeStoneInfo(new Vector2Int(pSize.x, pSize.y - cutRow), new string(newFill))),
                    GameObject.Find("Player").transform.position + new Vector3(droppedLifeStonePrefab.GetComponent<DroppedLifeStone>().unitSprite.GetComponent<SpriteRenderer>().bounds.size.x * i, 0, 0),
                    1f);
            }
        }
    }

    /// <summary>
    /// count lifestoneunit in lifestoneframe by type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
	public int CountType(LifeStoneType type)
	{
		int count = 0;
		for (int i = 0; i < 3; i++)
			for (int j = 0; j < lifeStoneRowNum; j++)
				if (lifeStoneArray[j, i] == (int)type)
					count++;
		return count;
	}
    /// <summary>
    /// count lifestoneunit in lifestoneframe
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int CountType()
    {
        int count = 0;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < lifeStoneRowNum; j++)
                if (lifeStoneArray[j, i] > 0)
                    count++;
        return count;
    }

    /// <summary>
    /// destroy lifestone by number
    /// </summary>
    /// <param name="num"></param>
    public void DestroyStone(int num)
	{
		System.Random rnd = new System.Random();
		ArrayList candArray = new ArrayList();
		for (int i = 0; i < num; i++)
		{
			for(int pj = lifeStoneRowNum-1; pj>=0;pj--)
			{
				ArrayList sCandArray = new ArrayList();
				for(int pi = 0; pi < 3; pi++)
				{
					if (lifeStoneArray[pj, pi] != 0)
						sCandArray.Add(new Vector2Int(pi, pj));
				}

				if (sCandArray.Count > 0)
				{
					int tmp = rnd.Next(sCandArray.Count);
					Vector2Int vtmp = (Vector2Int)sCandArray[tmp];
					candArray.Add(vtmp);
					lifeStoneArray[vtmp.y, vtmp.x] = 0;
					break;
				}
			}
		}
		StartCoroutine(DestroyInPhase(candArray));
        StartCoroutine(HitRedEffect(num));
	}
    IEnumerator HitRedEffect(int damage)
    {
        float startTime = Time.time, endTime = startTime + 0.3f;
        SpriteRenderer sprt = GameManager.Instance.player.GetComponent<SpriteRenderer>();
        sprt.color = new Color(1, 0, 0);
        
        while(Time.time < endTime)
        {
            sprt.color = new Color(1, 1 - (endTime - Time.time) / (endTime - startTime), 1 - (endTime - Time.time) / (endTime - startTime));
            yield return null;
        }
        sprt.color = new Color(1, 1, 1);
    }

    /// <summary>
    /// make term among lifestone destroy
    /// </summary>
    /// <param name="candArray"></param>
    /// <returns></returns>
	IEnumerator DestroyInPhase(ArrayList candArray)
	{
		for (int i = 0; i < candArray.Count; i++)
		{
			Vector2Int vtmp = (Vector2Int)candArray[i];
			lifeStoneUnit[vtmp.y, vtmp.x].GetComponent<LifeUnitInFrame>().unitDestroy();
			lifeStoneUnit[vtmp.y, vtmp.x] = null;
			yield return new WaitForSeconds(0.02f);
		}
	}

    /// <summary>
    /// change normal lifestone unit to type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="num"></param>
	public void ChangeFromNormal(LifeStoneType type, int num)
	{
		System.Random rnd = new System.Random();
		ArrayList candArray = new ArrayList();
		for (int j = 0; j < lifeStoneRowNum; j++)
			for (int i = 0; i < 3; i++)
				if (lifeStoneArray[j, i] == 1)
					candArray.Add(new Vector2Int(i, j));
		while (candArray.Count > num)
			candArray.RemoveAt(rnd.Next(candArray.Count));
		for(int i=0; i<candArray.Count; i++)
		{
			Vector2Int vtmp = (Vector2Int)candArray[i];
			lifeStoneArray[vtmp.y, vtmp.x] = (int)type;
		}
		StartCoroutine(ChangeInPhase(candArray,(int)type));
	}

    /// <summary>
    /// change type lifestone unit to normal
    /// </summary>
    /// <param name="type"></param>
    /// <param name="num"></param>
	public void ChangeToNormal(LifeStoneType type, int num)
	{
		System.Random rnd = new System.Random();
		ArrayList candArray = new ArrayList();
		for (int j = 0; j < lifeStoneRowNum; j++)
			for (int i = 0; i < 3; i++)
				if (lifeStoneArray[j, i] == (int)type)
					candArray.Add(new Vector2Int(i, j));
		while (candArray.Count > num)
			candArray.RemoveAt(rnd.Next(candArray.Count));
		for (int i = 0; i < candArray.Count; i++)
		{
			Vector2Int vtmp = (Vector2Int)candArray[i];
			lifeStoneArray[vtmp.y, vtmp.x] = 1;
		}
		StartCoroutine(ChangeInPhase(candArray, 1));
	}

    /// <summary>
    /// make term among changing
    /// </summary>
    /// <param name="candArray"></param>
    /// <param name="type"></param>
    /// <returns></returns>
	IEnumerator ChangeInPhase(ArrayList candArray, int type)
	{
		System.Random rnd = new System.Random();
		while (candArray.Count > 0)
		{
			int tmp = rnd.Next(candArray.Count);
			Vector2Int vtmp = (Vector2Int)candArray[tmp];
			lifeStoneUnit[vtmp.y, vtmp.x].GetComponent<LifeUnitInFrame>().ChangeType(type);
			candArray.RemoveAt(tmp);
			yield return new WaitForSeconds(0.02f);
		}
	}

    public void FillLifeStone(int num, LifeStoneType type)
    {
        List<Vector2Int> fillCand;
        List<KeyValuePair<Vector2Int, LifeStoneType>> fillArray = new List<KeyValuePair<Vector2Int, LifeStoneType>>();
        Vector2Int selectedPos;

        for (int n = 0; n < num; n++)
        {
            fillCand = new List<Vector2Int>();
            for (int j = 0; j < lifeStoneRowNum; j++)
                for (int i = 0; i < 3; i++)
                    if (
                        lifeStoneArray[j, i] == 0 &&
                        ((i - 1 >= 0 && lifeStoneArray[j, i - 1] > 0) ||
                        (i + 1 < 3 && lifeStoneArray[j, i + 1] > 0) ||
                        (j - 1 >= 0 && lifeStoneArray[j - 1, i] > 0) ||
                        (j + 1 < lifeStoneRowNum && lifeStoneArray[j + 1, i] > 0)
                        ))
                        fillCand.Add(new Vector2Int(i, j));
            if (fillCand.Count == 0) break;
            selectedPos = fillCand[Random.Range(0, fillCand.Count)];

            fillArray.Add(new KeyValuePair<Vector2Int, LifeStoneType>(selectedPos, type));
            lifeStoneArray[selectedPos.y, selectedPos.x] = (int)type;

            lifeStoneUnit[selectedPos.y, selectedPos.x] = Instantiate(lifeUnitPrefab, stoneSuper.transform);

            lifeStoneUnit[selectedPos.y, selectedPos.x].SetActive(false);
        }

        StartCoroutine(FillInPhase(fillArray));
    }

    IEnumerator FillInPhase(List<KeyValuePair<Vector2Int, LifeStoneType>> fillArray)
    {
        for(int i=0; i<fillArray.Count; i++)
        {
            lifeStoneUnit[fillArray[i].Key.y, fillArray[i].Key.x].SetActive(true);

            lifeStoneUnit[fillArray[i].Key.y, fillArray[i].Key.x].GetComponent<LifeUnitInFrame>().Init(
                (int)fillArray[i].Value,
                lifeStoneSize,
                new Vector2Int(fillArray[i].Key.x, fillArray[i].Key.y),
                new Vector2Int(fillArray[i].Key.x, fillArray[i].Key.y),
                new Vector2(frameBorder * lifeStoneSize, frameBorder * lifeStoneSize),
                false,
                0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// lifestoneframe vibration
    /// </summary>
    /// <param name="vibration">strength of vibration</param>
    /// <returns></returns>
	public IEnumerator VibrateEnumerator(float vibration)
	{
		while(vibration > lifeStoneSize * 0.005f)
		{
			Vector2 tmpVector = Random.insideUnitCircle;
			lifeStoneUI.transform.position = new Vector3(lifeStoneLocation.x + tmpVector.x * vibration * 0.3f, lifeStoneLocation.y + tmpVector.y * vibration, 0);
			vibration *= 0.8f;
			yield return null;
		}
        lifeStoneUI.transform.position = new Vector3(lifeStoneLocation.x, lifeStoneLocation.y, 0);
	}		
}
