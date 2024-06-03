using SoftGear.Strix.Unity.Runtime;
using System;
using UnityEngine;

[Serializable]
public class Item
{
    public GameObject itemObject;
    public int itemNum;
    public int maxItem;
    public string Tag => itemObject.tag;

    public void IncItem()
    {
        if (itemNum >= maxItem) return;
        itemNum++;
    }
    public void DecItem()
    {
        if (itemNum <= 0) return;
        itemNum--;
    }
}
