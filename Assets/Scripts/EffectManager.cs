using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : Singleton<EffectManager> {
    public GameObject effectPrefab;
    public GameObject numberPrefab;
    public GameObject numberCanvasPrefab;

    GameObject[] effectArray;
    GameObject[] numberArray;
    GameObject numberCanvas;

    protected EffectManager() { }

    private void Awake()
    {
        numberCanvas = Instantiate(numberCanvasPrefab);

        effectArray = new GameObject[20];
        for (int i = 0; i < effectArray.Length; i++)
        {
            effectArray[i] = Instantiate(effectPrefab, transform);
            effectArray[i].SetActive(false);
        }

        numberArray = new GameObject[30];
        for (int i = 0; i < numberArray.Length; i++)
        {
            numberArray[i] = Instantiate(numberPrefab, numberCanvas.transform);
            numberArray[i].SetActive(false);
        }
    }

    public bool StartEffect(int type, Bounds bound1, Bounds bound2)
    {
        foreach (GameObject obj in effectArray)
        {
            if(!obj.activeSelf)
            {
                obj.transform.position = GenerateRandomPosition(bound1, bound2);
                obj.SetActive(true);
                obj.GetComponent<Animator>().SetTrigger("start");
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// generate number effect
    /// </summary>
    /// <param name="type">0: A, 1: B, 2: C, 3: PlayerHit</param>
    /// <param name="bound1"></param>
    /// <param name="bound2"></param>
    /// <returns></returns>
    public bool StartNumber(int type, Bounds bound1, Bounds bound2, float damage)
    {
        return StartNumber(type, GenerateRandomPosition(bound1, bound2), damage);
    }
    public bool StartNumber(int type, Vector3 pos, float damage)
    {
        Color[] typeColor = new Color[4] { new Color(1, 0, 0), new Color(0, 0, 1), new Color(0, 1, 0), new Color(1, 0, 1) };
        foreach (GameObject obj in numberArray)
        {
            if (!obj.activeSelf)
            {
                obj.GetComponent<Text>().text = ((int)Mathf.Round(damage)).ToString();
                obj.GetComponent<Text>().color = typeColor[type];
                obj.transform.position = pos;
                obj.SetActive(true);
                StartCoroutine(NumberFadeOut(obj));
                return true;
            }
        }
        return false;
    }

    IEnumerator NumberFadeOut(GameObject obj)
    {
        float popAngle = Random.Range(45f, 135f) * Mathf.Deg2Rad;
        float velocity = 0.8f;
        float accel = -0.4f;
        Vector3 direction = new Vector3(Mathf.Cos(popAngle), Mathf.Sin(popAngle), 0);
        
        for (float timer = 0f; timer < 0.5f; timer += Time.deltaTime)
        {
            obj.transform.Translate(direction * velocity * Time.deltaTime);
            velocity += accel * Time.deltaTime;
            obj.GetComponent<Text>().color -= new Color(0, 0, 0, 2f * Time.deltaTime);
            yield return null;
        }
        obj.SetActive(false);
    }


    Vector3 GenerateRandomPosition(Bounds bound1, Bounds bound2)
    {
        float[] x = new float[4] { bound1.center.x - bound1.extents.x, bound1.center.x + bound1.extents.x, bound2.center.x - bound2.extents.x, bound2.center.x + bound2.extents.x };
        float[] y = new float[4] { bound1.center.y - bound1.extents.y, bound1.center.y + bound1.extents.y, bound2.center.y - bound2.extents.y, bound2.center.y + bound2.extents.y };
        System.Array.Sort(x);
        System.Array.Sort(y);
        return new Vector3(Random.Range(x[1], x[2]), Random.Range(y[1], y[2]), 0);
    }
}
