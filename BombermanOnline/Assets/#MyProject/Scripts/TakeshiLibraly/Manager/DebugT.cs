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
                Debug.LogError(obj + "��Null�ł��B");
                return false;
            }
            Debug.Log(obj + "��Null�ł͂Ȃ��ł��B");
            return true;
        }
    }
}