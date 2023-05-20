using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : Singleton<PoolManager>
{
    public List<GameObject> objectPrefabs = new List<GameObject>();

    public List<GameObject>[] pooledObjects;

    public List<int> amountToBuffer = new List<int>();

    public int defaultBufferAmount = 3;

    protected GameObject containerObject;

    public override void Awake()
    {
        base.Awake();
        StartPool();
    }
    void StartPool()
    {
        containerObject = new GameObject("ObjectPool");

        pooledObjects = new List<GameObject>[objectPrefabs.Count];

        int i = 0;
        foreach (GameObject objectPrefab in objectPrefabs)
        {
            objectPrefab.SetActive(false);
            pooledObjects[i] = new List<GameObject>();

            int bufferAmount;

            if (i < amountToBuffer.Count) bufferAmount = amountToBuffer[i];
            else
                bufferAmount = defaultBufferAmount;

            for (int n = 0; n < bufferAmount; n++)
            {
                GameObject newObj = Instantiate(objectPrefab) as GameObject;
                newObj.name = objectPrefab.name;
                Despawn(newObj);
            }

            i++;
            objectPrefab.SetActive(true);
        }
    }
    public GameObject Spawn(string objectType, bool onlyPooled)
    {
        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            GameObject prefab = objectPrefabs[i];
            if (prefab.name == objectType)
            {
                if (pooledObjects[i].Count > 0)
                {
                    GameObject pooledObject = pooledObjects[i][0];
                    pooledObjects[i].RemoveAt(0);
                    pooledObject.transform.parent = null;
                    pooledObject.SetActive(true);

                    return pooledObject;
                }
                else if (!onlyPooled)
                {
                    return Instantiate(objectPrefabs[i]) as GameObject;
                }
                break;
            }
        }
        return null;
    }
    public GameObject Spawn(string objectType, Vector3 objectPosition, Quaternion objectRotation, bool onlyPooled)
    {
        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            GameObject prefab = objectPrefabs[i];
            if (prefab.name == objectType)
            {
                if (pooledObjects[i].Count > 0)
                {
                    GameObject pooledObject = pooledObjects[i][0];
                    pooledObjects[i].RemoveAt(0);
                    if (pooledObject.GetComponent<RectTransform>() != null)
                    {
                        pooledObject.GetComponent<RectTransform>().SetParent(containerObject.transform, false);
                    }
                    else
                    {
                        pooledObject.transform.parent = containerObject.transform;
                    }
                    pooledObject.SetActive(true);
                    pooledObject.transform.position = objectPosition;
                    pooledObject.transform.rotation = objectRotation;

                    return pooledObject;
                }
                else if (!onlyPooled)
                {
                    return Instantiate(objectPrefabs[i]) as GameObject;
                }
                break;
            }
        }
        return null;
    }
    public void Despawn(GameObject obj)
    {
        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            if (objectPrefabs[i].name == obj.name)
            {
                obj.SetActive(false);
                if (obj.GetComponent<RectTransform>() != null)
                {
                    obj.GetComponent<RectTransform>().SetParent(containerObject.transform, false);
                }
                else
                {
                    obj.transform.parent = containerObject.transform;
                }
                pooledObjects[i].Add(obj);
                return;
            }
        }
    }
}