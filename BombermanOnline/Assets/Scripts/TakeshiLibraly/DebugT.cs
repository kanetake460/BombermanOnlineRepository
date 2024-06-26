using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class DebugT
    {
        static public bool NullCheck(object obj)
        {
            if (obj == null)
            {
                Debug.LogError(obj + "はNullです。");
                return false;
            }
            Debug.Log(obj + "はNullではないです。");
            return true;
        }
    }
}