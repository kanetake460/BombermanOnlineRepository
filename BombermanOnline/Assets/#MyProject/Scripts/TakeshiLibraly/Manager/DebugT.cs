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
                Debug.LogError(obj + "ÇÕNullÇ≈Ç∑ÅB");
                return false;
            }
            Debug.Log(obj + "ÇÕNullÇ≈ÇÕÇ»Ç¢Ç≈Ç∑ÅB");
            return true;
        }
    }
}