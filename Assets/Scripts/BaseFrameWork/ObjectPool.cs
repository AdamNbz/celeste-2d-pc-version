using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool: MonoBehaviour
{
    private class PooledItem
    {
        public string poolName;
        public List<GameObject> pooledObjects;

        public PooledItem(string name)
        {
            poolName = name;
            pooledObjects = new List<GameObject>();
        }

        public void ReturnToPool (GameObject obj)
        {
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        public GameObject GetObjectFromPool()
        {
            for(int i=0; i<pooledObjects.Count; i++)
            {
                if(pooledObjects[i].active==false)
                {
                    GameObject obj = pooledObjects[i];
                    pooledObjects.RemoveAt(i);
                    obj.SetActive(true);
                    return obj;
                } 
            }
            return null;
        }
    }

    private static ObjectPool __instance;

    public static ObjectPool GetInstance()
    {
        if (__instance==null)
        {
            __instance = FindAnyObjectByType<ObjectPool>();
            if(__instance==null)
            {
                GameObject obj = new GameObject("ObjectPool");
                __instance = obj.AddComponent<ObjectPool>();
            }
        }
        return __instance;
    }

    private List<PooledItem> pooledTypes = new List<PooledItem>();

    public void ReturnToPool(GameObject preference)
    {
        PooledItem item = pooledTypes.Find(x => x.poolName == preference.name);
        if (item == null)
        {
            item = new PooledItem(preference.name);
            pooledTypes.Add(item);
        }
        item.ReturnToPool(preference);
    }

    public GameObject GetObjectFromPool(GameObject preference, Vector2 position,Quaternion rotation)
    {
        PooledItem item = pooledTypes.Find(x => x.poolName == preference.name);
        if (item == null)
        {
            item = new PooledItem(preference.name);
            pooledTypes.Add(item);
        }
        GameObject obj=item.GetObjectFromPool();
        if(obj==null)
        {
            obj=Instantiate(preference, position,rotation);
            obj.name=preference.name;
        }
        else
        {
            obj.transform.position=position;
            obj.transform.rotation=rotation;
        }   
        return obj;
    }
}

