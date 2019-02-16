using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent (typeof (Animator))]

public class LifeUnitInFrame : MonoBehaviour {
	int type;
	Animator animator;
	Vector2Int pos,startPos;
	Vector2 zeroPos;
	float size;
	float v, accel;
    bool isFall;
	float vibration;

	/// <summary>
	/// Create LifeStoneUnit from above. Starts to fall
	/// </summary>
	/// <param name="_type"> type of lifestone unit</param>
	/// <param name="_size"> size of lifestone unit</param>
	/// <param name="_pos"> destination point in lifestoneFrame</param>
	/// <param name="_startPos"> starting point above the lifestoneFrame</param>
	/// <param name="_zeroPos"> base position of (0,0) lifestoneFrame</param>
	public void Init(int _type, float _size, Vector2Int _pos, Vector2Int _startPos, Vector2 _zeroPos, bool _isFall , float _vibration)
	{
		animator = GetComponent<Animator>();
		size = _size;	type = _type;	pos = _pos;	startPos = _startPos;	zeroPos = _zeroPos; isFall = _isFall;   vibration = _vibration;
		GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
		transform.localPosition = new Vector2(zeroPos.x + startPos.x * size, zeroPos.y + startPos.y * size);
		v = 0;
		accel = size * 0.01f;
		animator.SetInteger("type", type);
		StartCoroutine("FadeInEnumerator");
	}

	public void unitDestroy()
	{
		animator.SetBool("destroy", true);
	}

	/// <summary>
	/// Change type of this unit
	/// </summary>
	/// <param name="_type">target type</param>
	/// <returns></returns>
	public bool ChangeType(int _type)
	{
		if(type == 1 && _type != 1 || type != 1 && _type == 1)
		{
			type = _type;
			animator.SetInteger("type", type);
			return true;
		}
		return false;
	}
	IEnumerator FadeInEnumerator()
	{
		StartCoroutine("FallEnumerator");
		float alpha = 0;
		float fadeTime = 0.3f;
		while (alpha <= 1f)
		{
			GetComponent<Image>().color = new Color(255, 255, 255, alpha);
			alpha += 1f / fadeTime * Time.deltaTime;
			yield return null;
		}
		GetComponent<Image>().color = new Color(255, 255, 255, 1f);
		
	}
	IEnumerator FallEnumerator()
	{
		while (isFall)
		{
			float vtmp = transform.localPosition.y - v;
			if (vtmp <= zeroPos.y + pos.y * size)
				break;
			
			transform.localPosition = new Vector2(transform.localPosition.x, vtmp);
			v += accel;
			yield return null;
		}
        transform.localPosition = new Vector2(transform.localPosition.x, zeroPos.y + pos.y * size);

        if (vibration != 0)
			StartCoroutine(GameObject.Find("LifeStoneUI").GetComponent<LifeStoneManager>().VibrateEnumerator(vibration));
	}
	
}
