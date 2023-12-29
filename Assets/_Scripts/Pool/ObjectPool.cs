using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Maya

public class ObjectPool<T> where T : PoolObject<T>  //ObjectPool<PoolObject<T>>
{
    private GameObject prefab;
    private Queue<T> inactiveObjects = new Queue<T>();

    public ObjectPool(GameObject _prefab)
    {
        this.prefab = _prefab;
    }

    /// <summary>
    /// Get the Object from the Pool or instantiate a new one
    /// </summary>
    /// <returns></returns>
    public T GetObject()
    {
        T temp;

        // if an object is left in the queue
        if (inactiveObjects.Count > 0)
        {
            temp = inactiveObjects.Dequeue();  // take it 
        }
        // else spawn a new one (from the prefab)
        else
        {
            temp = GameObject.Instantiate(prefab).GetComponent<T>();
            temp.Init(this);
        }
        return temp;
    }

    /// <summary>
    /// Add the Object to the ObjectPool
    /// </summary>
    /// <param name="_obj"></param>
    public void AddObject(T _obj)
    {
        inactiveObjects.Enqueue(_obj);
    }
}
