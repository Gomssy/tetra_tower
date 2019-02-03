using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Addon {
    public int id;
    public string name;
    public ItemQuality quality;
    public AddonType type;
    public Sprite sprite;
    public Sprite highlight;
    public Vector2 sizeInventory;

    public Addon()
    {
        Declare();
    }
    public virtual void Declare()
    {
        id = 0; name = "itemname";
        quality = ItemQuality.Study;
        type = AddonType.Prop;
        sprite = null;
        highlight = null;
        sizeInventory = new Vector2(0, 0);
    }
}
