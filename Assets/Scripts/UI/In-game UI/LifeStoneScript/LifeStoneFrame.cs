using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeStoneFrame : MonoBehaviour {
    /// <summary>
    /// The frame top image
    /// </summary>
    GameObject frameTop;
    /// <summary>
    /// The frame rows images
    /// </summary>
    GameObject[] frameRows;
    /// <summary>
    /// The frame bottom image
    /// </summary>
    GameObject frameBottom;
    /// <summary>
    /// The number of lifeStoneRows
    /// </summary>
    int lifeStoneRow;
    /// <summary>
    /// Size of lifeStone
    /// </summary>
    float lifeStoneSize;
    /// <summary>
    /// The sprites
    /// </summary>
    Sprite[] sprites;
    float frameBorder;

    public void Init(Transform superGO, GameObject standardImage, int lifeStoneRow, float lifeStoneSize, Sprite[] _sprites, float _frameBorder)
    {
        frameBorder = _frameBorder;
        sprites = new Sprite[_sprites.GetLength(0)];
        _sprites.CopyTo(sprites, 0);
        this.lifeStoneRow = lifeStoneRow;
        this.lifeStoneSize = lifeStoneSize;
        frameTop = Instantiate(standardImage, superGO);
        frameTop.name = "FrameTop";
        frameTop.GetComponent<Image>().sprite = sprites[2];
        frameTop.GetComponent<RectTransform>().sizeDelta = new Vector2(lifeStoneSize * (3f + frameBorder * 2), lifeStoneSize * frameBorder);
        frameTop.transform.localPosition = new Vector3(0, lifeStoneSize * (frameBorder + lifeStoneRow));

        frameRows = new GameObject[50];
        for (int i = 49; i >= lifeStoneRow; i--)
        {
            frameRows[i] = Instantiate(standardImage, superGO);
            frameRows[i].name = "FrameRow" + i.ToString("D2");
            frameRows[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
            frameRows[i].GetComponent<Image>().sprite = sprites[1];
            frameRows[i].GetComponent<RectTransform>().sizeDelta = new Vector2(lifeStoneSize * (3f + frameBorder * 2), lifeStoneSize);
            frameRows[i].transform.localPosition = new Vector3(0, lifeStoneSize * (frameBorder + lifeStoneRow - 1));
        }
        for(int i=lifeStoneRow - 1; i>=0; i--)
        {
            frameRows[i] = Instantiate(standardImage, superGO);
            frameRows[i].name = "FrameRow" + i.ToString("D2");
            frameRows[i].GetComponent<Image>().sprite = sprites[1];
            frameRows[i].GetComponent<RectTransform>().sizeDelta = new Vector2(lifeStoneSize * (3f + frameBorder * 2), lifeStoneSize);
            frameRows[i].transform.localPosition = new Vector3(0, lifeStoneSize * (frameBorder + i));
        }

        frameBottom = Instantiate(standardImage,superGO);
        frameBottom.name = "FrameBottom";
        frameBottom.GetComponent<Image>().sprite = sprites[0];
        frameBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(lifeStoneSize * (3f + frameBorder * 2), lifeStoneSize * frameBorder);
        frameBottom.transform.localPosition = new Vector3(0, 0);

    }

    public void AddRow(int afterRow)
    {
        int prevLifeStoneRow = lifeStoneRow;
        lifeStoneRow = afterRow;
        StartCoroutine(ExpandEnumerator(prevLifeStoneRow, lifeStoneRow));
    }
    IEnumerator ExpandEnumerator(int prev, int after)
    {
        float d = 0;
        float v = 0;
        float a = lifeStoneSize * 0.02f;
        while (true)
        {
            d += v;
            v += a;
            if(d >= lifeStoneSize * (after - prev))
            {
                d = lifeStoneSize * (after - prev);
                a = 0;
            }
            frameTop.transform.localPosition = new Vector3(0, lifeStoneSize * (frameBorder + prev) + d);
            for (int i=prev; i< after; i++)
            {
                if(d > (after - i - 1) * lifeStoneSize)
                {
                    frameRows[i].GetComponent<Image>().color = new Color(255, 255, 255, 1);
                    frameRows[i].transform.localPosition = new Vector3(0, lifeStoneSize * (frameBorder + i - after + prev) + d);
                }
            }
            if (a == 0) break;
            yield return null;
        }
        StartCoroutine(GameObject.Find("LifeStoneUI").GetComponent<LifeStoneManager>().VibrateEnumerator(30));

    }

}
