using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    private Queue<T> poolQueue = new Queue<T>();
    private T prefab;
    private Transform parent;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T newObject = GameObject.Instantiate(prefab, parent);
            newObject.gameObject.SetActive(false);
            poolQueue.Enqueue(newObject);
        }
    }

    public T GetObject()
    {
        if (poolQueue.Count > 0)
        {
            T obj = poolQueue.Dequeue();
            obj.gameObject.SetActive(true);
            obj.OnObjectSpawn();
            return obj;
        }
        else
        {
            T newObject = GameObject.Instantiate(prefab, parent);
            newObject.OnObjectSpawn();
            return newObject;
        }
    }

    public void ReturnObject(T obj)
    {
        obj.OnObjectDespawn();
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
