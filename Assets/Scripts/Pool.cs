using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private List<GameObject> pool;
    private Transform parent;

    public ObjectPool(GameObject prefab, int size, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        InitializePool(size);
    }

    private void InitializePool(int size)
    {
        pool = new List<GameObject>(size);

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Object.Instantiate(prefab);
            obj.gameObject.SetActive(false);

            if (parent != null)
                obj.transform.SetParent(parent);

            pool.Add(obj);
        }
    }

    public GameObject GetObjectFromPool(Vector3 position, Quaternion rotation)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.gameObject.activeSelf)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Object.Instantiate(prefab, position, rotation);

        if (parent != null)
            newObj.transform.SetParent(parent);

        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        if (pool.Contains(obj))
            obj.gameObject.SetActive(false);
    }
}