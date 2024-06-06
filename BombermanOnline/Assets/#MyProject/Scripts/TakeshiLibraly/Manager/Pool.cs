using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : IPoolableObject<T> where T : class
{
    // // ===変数====================================================
    public readonly List<T> list = new List<T>();

    // ===プロパティ=================================================
    public int Count => list.Count;

    // ===関数====================================================
    /// <summary>
    /// プールから取り出す
    /// 条件に当てはまっていない場合は
    /// 新しく生成します。
    /// </summary>
    /// <param name="isTakeble">条件</param>
    /// <param name="generator">生成メソッド</param>
    /// <returns>取り出したもの</returns>
    public T Get(Func<T,bool> isTakeble,Func<T> generator)
    {
        T t = list.Where(t => isTakeble(t)).FirstOrDefault();

        if(t == null)
        {
            t = generator();
            Add(t);
        }
        return t;
    }


    /// <summary>
    /// プールから取り出します。
    /// ない場合はnull
    /// </summary>
    /// <param name="isTakeble">条件</param>
    /// <returns>取り出した物</returns>
    public T Take(Func<T,bool> isTakeble)
    {
        T t = list.Where(t => isTakeble(t)).FirstOrDefault();

        if (t == null)
        {
            Debug.Log("取り出せませんでした。");
        }
        return t;
    }


    /// <summary>
    /// プールに追加します
    /// </summary>
    /// <param name="prefab">追加するプレハブ</param>
    public void Add(T prefab) => list.Add(prefab);

}
