using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public GameObject itemObject;
    public int itemNum;
    public string Tag => itemObject.tag;
}
