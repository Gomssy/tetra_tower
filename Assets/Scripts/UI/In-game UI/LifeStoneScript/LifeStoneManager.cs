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
    [HideInInspector]public LifeStoneFrame lifeStoneFrame;

	
	void Start () {
        transform.position = new Vector3(lifeStoneLocation.x, lifeStoneLocation.y, 0);
        lifeStoneFrame = new LifeStoneFrame(frameSuper.transform, standardImage, lifeStoneRowNum, lifeStoneSize, sprites);
        lifeStoneArray = new int[50, 3];
		lifeStoneUnit = new GameObject[50, 3];
        for (int i = 0; i < 50; i++) for (int j = 0; j < 3; j++) lifeStoneArray[i, j] = 0;
		StartCoroutine("TestEnumerator");
	}
	IEnumerator TestEnumerator()
	{
		//PushLifeStone(new LifeStoneInfo(new Vector2Int(3, 8), "AAAAAAAAAAAAAAAAAAAAAAAA"));
		PushLifeStone(new LifeStoneInfo(new Vector2Int(2, 5), " AAAABA A "));
		yield return new WaitForSeconds(2);
		PushLifeStone(new LifeStoneInfo(new Vector2Int(2, 3), " AAA A"));
		yield return new WaitForSeconds(2);
		ChangeFromNormal(2, 5);
		yield return new WaitForSeconds(2);
		ChangeToNormal(2, 3);
		yield return new WaitForSeconds(2);
		DestroyStone(3);
	}

	/// <summary>
	/// push LifeStone in LifeStoneFrame
	/// </summary>
	/// <param name="pushInfo"></param>
	void PushLifeStone(LifeStoneInfo pushInfo)
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

		for (int i = 0; i <= 3 - pSize.x; i++)
			if (minRow[i] == selectedRow) selColCand.Add(i);


		selectedCol = (int)selColCand[rnd.Next(selColCand.Count)];

		float vibration = pushInfo.getAmount() * vibrationVariable * lifeStoneSize;
		for (int pj = 0; pj < pSize.y; pj++)
		{
			if (selectedRow + pj >= lifeStoneRowNum) break;
			for (int pi = 0; pi < pSize.x; pi++)
				if (pFill[pj * pSize.x + pi] != ' ')
				{
					int xtmp = selectedCol + pi, ytmp = selectedRow + pj;
					lifeStoneArray[ytmp, xtmp] = pFill[pj * pSize.x + pi] - 'A' + 1;
					lifeStoneUnit[ytmp, xtmp] = Instantiate(lifeUnitPrefab, stoneSuper.transform);

					lifeStoneUnit[ytmp, xtmp].GetComponent<LifeUnitInFrame>().Init(
						lifeStoneArray[ytmp, xtmp], 
						lifeStoneSize, 
						new Vector2Int(xtmp, ytmp), 
						new Vector2Int(xtmp, lifeStoneRowNum + pj), 
						new Vector2(0.2f * lifeStoneSize, 0.2f * lifeStoneSize),
						vibration);
					vibration = 0;
				}
		}
	}

	public int CountType(int type)
	{
		int count = 0;
		for (int j = 0; j < 3; j++)
			for (int i = 0; i < lifeStoneRowNum; i++)
				if (lifeStoneArray[j, i] == type)
					count++;
		return count;
	}

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
	}

	IEnumerator DestroyInPhase(ArrayList candArray)
	{
		for (int i = 0; i < candArray.Count; i++)
		{
			Vector2Int vtmp = (Vector2Int)candArray[i];
			lifeStoneUnit[vtmp.y, vtmp.x].GetComponent<LifeUnitInFrame>().unitDestroy();
			lifeStoneUnit[vtmp.y, vtmp.x] = null;
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void ChangeFromNormal(int type, int num)
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
			lifeStoneArray[vtmp.y, vtmp.x] = type;
		}
		StartCoroutine(ChangeInPhase(candArray,type));
	}

	public void ChangeToNormal(int type, int num)
	{
		System.Random rnd = new System.Random();
		ArrayList candArray = new ArrayList();
		for (int j = 0; j < lifeStoneRowNum; j++)
			for (int i = 0; i < 3; i++)
				if (lifeStoneArray[j, i] == type)
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

	IEnumerator ChangeInPhase(ArrayList candArray, int type)
	{
		System.Random rnd = new System.Random();
		while (candArray.Count > 0)
		{
			int tmp = rnd.Next(candArray.Count);
			Vector2Int vtmp = (Vector2Int)candArray[tmp];
			lifeStoneUnit[vtmp.y, vtmp.x].GetComponent<LifeUnitInFrame>().ChangeType(type);
			candArray.RemoveAt(tmp);
			yield return new WaitForSeconds(0.1f);
		}
	}

	public IEnumerator vibrateEnumerator(float vibration)
	{
		while(vibration > lifeStoneSize * 0.05f)
		{
			Vector2 tmpVector = Random.insideUnitCircle;
			transform.position = new Vector3(lifeStoneLocation.x + tmpVector.x * vibration * 0.3f, lifeStoneLocation.y + tmpVector.y * vibration, 0);
			vibration *= 0.8f;
			yield return null;
		}
		transform.position = new Vector3(lifeStoneLocation.x, lifeStoneLocation.y, 0);
	}		
}
