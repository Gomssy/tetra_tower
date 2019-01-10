using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStoneInfo : MonoBehaviour {
    /// <summary>
    /// width, height of LifeStone
    /// ex)  □
    ///     □□
    ///     □
    ///   (2,3)
    /// </summary>
    Vector2Int size;
    /// <summary>
    /// contents of LifeStone from bottom left.
    /// A: Normal lifestone
    /// B: Gold lifestone
    /// C: Amethyst lifestone
    /// 
    /// ex)  A
    ///     BC
    ///     A
    ///     "A BC A"
    /// </summary>
    string fill;
	
	LifeStoneInfo(Vector2Int size, string fill)
    {
        this.size = size;
        this.fill = fill;
    }
    public Vector2Int getSize()
    {
        return size;
    }
    public string getFill()
    {
        return fill;
    }
}
