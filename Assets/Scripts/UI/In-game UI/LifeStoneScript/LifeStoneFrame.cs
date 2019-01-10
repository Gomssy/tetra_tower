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

    public LifeStoneFrame(Transform superGO, GameObject standardImage, int lifeStoneRow, float lifeStoneSize, Sprite[] _sprites)
    {
        sprites = new Sprite[_sprites.GetLength(0)];
        _sprites.CopyTo(sprites, 0);
        this.lifeStoneRow = lifeStoneRow;
        this.lifeStoneSize = lifeStoneSize;

        frameBottom = Instantiate(standardImage,superGO);
        frameBottom.name = "FrameBottom";
        frameBottom.GetComponent<Image>().sprite = sprites[0];
        frameBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(lifeStoneSize * 3.4f, lifeStoneSize * 0.2f);
        frameBottom.transform.localPosition = new Vector3(0, 0);

        frameRows = new GameObject[50];
        for(int i=0; i<lifeStoneRow; i++)
        {
            frameRows[i] = Instantiate(standardImage, superGO);
            frameRows[i].name = "FrameRow" + i.ToString("D2");
            frameRows[i].GetComponent<Image>().sprite = sprites[1];
            frameRows[i].GetComponent<RectTransform>().sizeDelta = new Vector2(lifeStoneSize * 3.4f, lifeStoneSize);
            frameRows[i].transform.localPosition = new Vector3(0, lifeStoneSize * (0.2f + i));
        }

        frameTop = Instantiate(standardImage, superGO);
        frameTop.name = "FrameTop";
        frameTop.GetComponent<Image>().sprite = sprites[2];
        frameTop.GetComponent<RectTransform>().sizeDelta = new Vector2(lifeStoneSize * 3.4f, lifeStoneSize * 0.2f);
        frameTop.transform.localPosition = new Vector3(0, lifeStoneSize * (0.2f + lifeStoneRow));
    }

}
