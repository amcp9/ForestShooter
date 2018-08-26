using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public ObjectPool ParentPool;

    /// <summary>
    /// 当该游戏物体被对象池回收(SetActive为false)调用
    /// </summary>
    void OnDisable()
    {
        transform.position = Vector3.zero;

        if (ParentPool)
            ParentPool.AddToAvailableObjects(this.gameObject);
        else
            Debug.LogWarning("PooledObject " + gameObject.name + " does not have a parent pool. If this occurred during a scene transition, ignore this. Otherwise reoprt to developer.");
    }
}

