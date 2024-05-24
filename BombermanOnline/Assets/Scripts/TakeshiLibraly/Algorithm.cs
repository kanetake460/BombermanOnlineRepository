using System;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class Algorithm : MonoBehaviour
    {
        public static void Shuffle<T>(IList<T> arry)
        {
            int rand;
            for (int i = 0; i < arry.Count; i++)
            {
                rand = UnityEngine.Random.Range(0, arry.Count);
                T swap = arry[i];
                arry[i] = arry[rand];
                arry[rand] = swap;
            }
        }
        public static void Shuffle<T>(T[,] arry)
        {
            int rand;
            for (int i = 0; i < arry.GetLength(0); i++)
            {
                for (int j = 0; j < arry.GetLength(1); j++)
                {
                    rand = UnityEngine.Random.Range(0, arry.Length);
                    //Swap(arry[i], arry[rand]);
                    T swap = arry[i,j];
                    arry[i,j] = arry[i,rand];
                    arry[i,rand] = swap;
                }
            }
        }

        public static void Swap<T>(T lhs, T rhs)
        {
            T swap = lhs;
            lhs = rhs;
            rhs = swap;
        }

        public static bool AllElement<T>(T[] array, T value)
        {
            foreach (T i in array)
            {
                if (EqualityComparer<T>.Default.Equals(i, value)) continue;

                return false;
            }
            return true;
        }

        public static bool AllElement<T>(T[,] array, T value)
        {
            foreach (T i in array)
            {
                if (EqualityComparer<T>.Default.Equals(i, value)) continue;

                return false;
            }
            return true;
        }


    }
}
