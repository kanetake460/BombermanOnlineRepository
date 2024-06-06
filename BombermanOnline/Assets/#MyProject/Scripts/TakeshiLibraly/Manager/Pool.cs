using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : IPoolableObject<T> where T : class
{
    // // ===�ϐ�====================================================
    public readonly List<T> list = new List<T>();

    // ===�v���p�e�B=================================================
    public int Count => list.Count;

    // ===�֐�====================================================
    /// <summary>
    /// �v�[��������o��
    /// �����ɓ��Ă͂܂��Ă��Ȃ��ꍇ��
    /// �V�����������܂��B
    /// </summary>
    /// <param name="isTakeble">����</param>
    /// <param name="generator">�������\�b�h</param>
    /// <returns>���o��������</returns>
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
    /// �v�[��������o���܂��B
    /// �Ȃ��ꍇ��null
    /// </summary>
    /// <param name="isTakeble">����</param>
    /// <returns>���o������</returns>
    public T Take(Func<T,bool> isTakeble)
    {
        T t = list.Where(t => isTakeble(t)).FirstOrDefault();

        if (t == null)
        {
            Debug.Log("���o���܂���ł����B");
        }
        return t;
    }


    /// <summary>
    /// �v�[���ɒǉ����܂�
    /// </summary>
    /// <param name="prefab">�ǉ�����v���n�u</param>
    public void Add(T prefab) => list.Add(prefab);

}
