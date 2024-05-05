using System;
using UnityEngine;

[Serializable]
public class Item
{
    public GameObject itemObject;
    public int itemNum;
    public string Tag => itemObject.tag;
}
