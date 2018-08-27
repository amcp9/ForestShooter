using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----------------------对象池管理器-------------------------*/
public class ObjectPool : MonoBehaviour 
{
    static Dictionary<string, ObjectPool> SharedPools = new Dictionary<string, ObjectPool>();
    static GameObject Marker;

    public GameObject Temp;
    public string PoolName;
    public List<GameObject> ObjectList;
    public bool AutoResize = false;
    public int PoolSize = 100;
    public bool InstantiateOnAwake;
    public bool Shared;

    private List<GameObject> AvailableObjects;

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <returns>返回该对象池实例.</returns>
    /// <param name="template">该GameObject的对象池</param>
    /// <param name="name">该对象池名称</param>
    /// <param name="size">该对象池游戏物体数量</param>
    /// <param name="autoResize">当数量达到最大时，是否使用Instantiate</param>
    /// <param name="instantiateImmediate">是否立即创建</param>
    /// <param name="shared">是否加入字典共享</param>
    public static ObjectPool CreateObjectPool(GameObject template,string name,int size,bool autoResize,bool instantiateImmediate,bool shared)
    {
        if(Marker == null)
        {
            Marker = new GameObject("ObjectPool Manager");
            SharedPools.Clear();
        }
        if(shared)
        {
            if(SharedPools.ContainsKey(name))
            {
                return SharedPools[name];
            }
            else
            {
                GameObject g = new GameObject(name);
                ObjectPool pool = g.AddComponent<ObjectPool>();
                pool.InstantiateOnAwake = false;
                pool.SetProperties(template, size, name, autoResize);
                SharedPools.Add(name, pool);

                if(instantiateImmediate) 
                {
                    pool.InstantiatePool();
                }
                g.transform.parent = Marker.transform;

                return pool;
            }
        }
        else
        {
            GameObject g = new GameObject(name);
            ObjectPool pool = g.AddComponent<ObjectPool>();
            pool.InstantiateOnAwake = false;
            pool.SetProperties(template, size, name, autoResize);

            if (instantiateImmediate)
            {
                pool.InstantiatePool();
            }
            g.transform.parent = Marker.transform;
            return pool;
        }
    }

private void Awake()
    {
        if(Marker == null)
        {
            Marker = new GameObject("ObjectPool Manager");
            SharedPools.Clear();
        }

        if(InstantiateOnAwake)
        {
            ObjectList = new List<GameObject>(PoolSize);
            AvailableObjects = new List<GameObject>(PoolSize);
            InstantiatePool();
        }
        if(Shared)
        {
            SharedPools.Add(this.name, this);
        }
    }
    /// <summary>
    /// 设置该对象池的各种属性
    /// </summary>
    /// <param name="temp">Temp.</param>
    /// <param name="size">Size.</param>
    /// <param name="name">Name.</param>
    /// <param name="autoResize">If set to <c>true</c> auto resize.</param>
    public void SetProperties(GameObject temp,int size,string name,bool autoResize)
    {
        Temp = temp;
        PoolSize = size;
        ObjectList = new List<GameObject>(size);
        AvailableObjects = new List<GameObject>(size);
        PoolName = name;
        this.AutoResize = autoResize;
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    public void InstantiatePool()
    {
        if(Temp == null)
        {
            return;
        }
        ClearPool();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject g = NewActiveObject();
            g.SetActive(false);
            ObjectList.Add(g);
        }
    }

    /// <summary>
    /// 尝试获取该对象池的下一个可用游戏物体,并获得其引用
    /// </summary>
    public bool TryGetNextObject(Vector3 pos, Quaternion rot, out GameObject obj)
    {
        if (ObjectList.Count == 0)
        {
            Debug.LogError("EZ Object Pool " + PoolName + ", the pool has not been instantiated but you are trying to retrieve an object!");
        }

        int lastIndex = AvailableObjects.Count - 1;

        if (AvailableObjects.Count > 0)
        {
            if (AvailableObjects[lastIndex] == null)
            {
                Debug.LogError("EZObjectPool " + PoolName + " has missing objects in its pool! Are you accidentally destroying any GameObjects retrieved from the pool?");
                obj = null;
                return false;
            }

            AvailableObjects[lastIndex].transform.position = pos;
            AvailableObjects[lastIndex].transform.rotation = rot;
            AvailableObjects[lastIndex].SetActive(true);
            obj = AvailableObjects[lastIndex];
            AvailableObjects.RemoveAt(lastIndex);
            return true;
        }

        if (AutoResize)
        {
            GameObject g = NewActiveObject();
            g.transform.position = pos;
            g.transform.rotation = rot;
            ObjectList.Add(g);
            obj = g;
            return true;
        }
        else
        {
            obj = null;
            return false;
        }
    }


    /// <summary>
    /// 尝试获取该对象池的下一个可用游戏物体
    /// </summary>
    public void TryGetNextObject(Vector3 pos, Quaternion rot)
    {
        if (ObjectList.Count == 0)
        {
            Debug.LogError("EZ Object Pool " + PoolName + ", the pool has not been instantiated but you are trying to retrieve an object!");
        }

        int lastIndex = AvailableObjects.Count - 1;

        if (AvailableObjects.Count > 0)
        {
            if (AvailableObjects[lastIndex] == null)
            {
                Debug.LogError("EZObjectPool " + PoolName + " has missing objects in its pool! Are you accidentally destroying any GameObjects retrieved from the pool?");
                return;
            }

            AvailableObjects[lastIndex].transform.position = pos;
            AvailableObjects[lastIndex].transform.rotation = rot;
            AvailableObjects[lastIndex].SetActive(true);
            AvailableObjects.RemoveAt(lastIndex);
            return;
        }

        if (AutoResize)
        {
            GameObject g = NewActiveObject();
            g.transform.position = pos;
            g.transform.rotation = rot;
            ObjectList.Add(g);
        }
    }

    /// <summary>
    /// Instantiate一个游戏物体，设置其父对象，并给其添加用于对象池管理的PoolObject脚本
    /// </summary>
    private GameObject NewActiveObject()
    {
        GameObject g = (GameObject)Instantiate(Temp);
        g.transform.parent = transform;

        PooledObject p = g.GetComponent<PooledObject>();

        if (p)
            p.ParentPool = this;
        else
            g.AddComponent<PooledObject>().ParentPool = this;

        return g;
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void ClearPool()
    {
        for (int i = 0; i < ObjectList.Count; i++)
        {
            Destroy(ObjectList[i]);
        }

        ObjectList.Clear();
        AvailableObjects.Clear();
    }

    public void DeletePool(bool deleteActiveObjects)
    {
        for (int i = 0; i < ObjectList.Count; i++)
        {
            if (!ObjectList[i].activeInHierarchy || (ObjectList[i].activeInHierarchy && deleteActiveObjects))
                Destroy(ObjectList[i]);
        }
    }

    public void AddToAvailableObjects(GameObject obj)
    {
        AvailableObjects.Add(obj);
    }

    public int ActiveObjectCount()
    {
        return ObjectList.Count - AvailableObjects.Count;
    }

    public int AvailableObjectCount()
    {
        return AvailableObjects.Count;
    }


}
