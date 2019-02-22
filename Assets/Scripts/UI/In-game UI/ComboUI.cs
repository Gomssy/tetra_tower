using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour {

    public GameObject charUI;
    public GameObject timeUI;

    public GameObject charPrefab;

    public Sprite[] normalCombo = new Sprite[3];

    public float charSpaceR;

    float timeUILength;
    float charSpace;

    GameObject[] charObj;

    string currentCombo;

	void Awake () {
        RectTransform rtt = timeUI.GetComponent<RectTransform>();

        timeUILength = rtt.sizeDelta.x;
        rtt.sizeDelta = new Vector2(0, rtt.sizeDelta.y);

        charObj = new GameObject[8];

        for (int i = 0; i < 8; i++)
        {
            charObj[i] = Instantiate(charPrefab, charUI.transform);
            charObj[i].SetActive(false);
        }

        currentCombo = "";

        charSpace = charUI.GetComponent<RectTransform>().sizeDelta.y * charSpaceR;

    }
	
    public void SetCombo(string combo)
    {
        for(int i=0; i<8; i++)
        {
            if(combo.Length <= i && currentCombo.Length <= i)
            {
                break;
            }
            else if(combo.Length <= i)
            {
                charObj[i].SetActive(false);
            }
            else
            {
                Sprite currentSprite = normalCombo[combo[i] - 'A'];
                charObj[i].SetActive(true);
                charObj[i].GetComponent<Image>().sprite = currentSprite;
                charObj[i].GetComponent<RectTransform>().sizeDelta = new Vector2(
                    currentSprite.bounds.extents.x / currentSprite.bounds.extents.y * charUI.GetComponent<RectTransform>().sizeDelta.y,
                    charUI.GetComponent<RectTransform>().sizeDelta.y);
                charObj[i].transform.localPosition = new Vector3((i==0) ? 0 : charObj[i-1].transform.localPosition.x + charObj[i-1].GetComponent<RectTransform>().sizeDelta.x / 2f + charObj[i].GetComponent<RectTransform>().sizeDelta.x / 2f + charSpace, 0, 0);

            }
        }
        if (combo.Length > 0)
        {
            Vector3 shift = new Vector3((charObj[0].transform.position.x - charObj[combo.Length - 1].transform.position.x)/2f, 0, 0);
            for (int i = 0; i < 8; i++)
            {
                if (charObj[i].activeSelf)
                {
                    charObj[i].transform.localPosition = charObj[i].transform.localPosition + shift;
                    charObj[i].transform.localScale = new Vector3((i == combo.Length - 1) ? 1.3f : 1, (i == combo.Length - 1) ? 1.3f : 1, 1);
                }
            }
            StartCoroutine(EmphasizeCoroutine(combo.Length - 1));
        }
        currentCombo = combo;
    }

    IEnumerator EmphasizeCoroutine(int n)
    {
        for (float timer = 0f; timer < 0.5f; timer += Time.deltaTime)
        {
            charObj[n].transform.localScale = new Vector3(-2.5f * Mathf.Pow(timer * 2 - 0.45f,2f) + 1.75625f, -2.5f * Mathf.Pow(timer * 2 - 0.45f, 2f) + 1.75625f, 1);
            yield return null;
        }
        charObj[n].transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetTime()
    {
        RectTransform rtt = timeUI.GetComponent<RectTransform>();
        rtt.sizeDelta = new Vector2(0, rtt.sizeDelta.y);
    }

    public void SetTime(float currentTime, float fullTime)
    {
        RectTransform rtt = timeUI.GetComponent<RectTransform>();

        rtt.sizeDelta = new Vector2(timeUILength * (currentTime / fullTime), rtt.sizeDelta.y);
    }
}
