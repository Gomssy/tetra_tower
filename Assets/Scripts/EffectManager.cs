using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager> {
    public GameObject effectPrefab;

    GameObject[] effectArray;

    protected EffectManager() { }

    private void Awake()
    {
        effectArray = new GameObject[20];
        for (int i = 0; i < effectArray.Length; i++)
        {
            effectArray[i] = Instantiate(effectPrefab);
            effectArray[i].SetActive(false);
        }
        
    }

    public bool StartEffect(int type, Bounds bound1, Bounds bound2)
    {
        for (int i = 0; i < effectArray.Length; i++)
        {
            if(!effectArray[i].activeSelf)
            {
                effectArray[i].transform.position = GenerateRandomPosition(bound1, bound2);
                effectArray[i].SetActive(true);
                effectArray[i].GetComponent<Animator>().SetTrigger("start");
                return true;
            }
        }
        return false;
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
