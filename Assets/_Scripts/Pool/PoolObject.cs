using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class PoolObject<T> : MonoBehaviour where T : PoolObject<T>
{
    public ObjectPool<T> pool;

    /// <summary>
    /// Spawn a new Object
    /// </summary>
    public virtual void Init(ObjectPool<T> _pool)
    {
        pool = _pool;
    }

    /// <summary>
    /// Activate the Object and reset the position and rotation to the given ones
    /// </summary>
    public virtual void ResetObj(Vector3 _position, Vector3 _eulerAngle)
    {
        gameObject.SetActive(true);
        transform.position = _position;
        transform.eulerAngles = _eulerAngle;
    }

    /// <summary>
    /// Activate the Object and reset the local position and rotation to the given ones
    /// </summary>
    public virtual void ResetObjLocal(Vector3 _position, Vector3 _eulerAngle)
    {
        gameObject.SetActive(true);
        transform.localPosition = _position;
        transform.eulerAngles = _eulerAngle;
    }

    /// <summary>
    /// Put the object back to the ObjectPool and deactivate it
    /// </summary>
    public virtual void Deactivate()
    {
        pool.AddObject((T)this);        // GameObject added to the queue
        gameObject.SetActive(false);
    }
}
