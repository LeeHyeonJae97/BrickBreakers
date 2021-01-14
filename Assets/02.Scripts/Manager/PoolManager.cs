using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fix
public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int initAmount;
        private Transform holder;
        private List<GameObject> objs = new List<GameObject>();

        public void Init(Transform poolHolder)
        {
            holder = new GameObject(key).transform;
            holder.SetParent(poolHolder);

            for (int i = 0; i < initAmount; i++)
            {
                GameObject obj = Instantiate(prefab, holder);
                obj.name = key;
                objs.Add(obj);
                obj.SetActive(false);
            }
        }

        private void Expand()
        {
            for (int i = 0; i < initAmount; i++)
            {
                GameObject obj = Instantiate(prefab, holder);
                obj.name = key;
                objs.Add(obj);
                obj.SetActive(false);
            }
        }

        public GameObject Get()
        {
            if (objs.Count == 0) Expand();

            GameObject obj = objs[0];
            obj.SetActive(true);
            objs.RemoveAt(0);
            return obj;
        }

        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(holder);
            objs.Add(obj);
        }
    }

    public static PoolManager instance;

    public Transform poolHolder;
    public Pool[] pools;
    private Dictionary<string, Pool> poolDic = new Dictionary<string, Pool>();

    private void Awake()
    {
        if (instance == null) instance = this;

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].Init(poolHolder);
            poolDic.Add(pools[i].key, pools[i]);
        }
    }

    #region GET
    public GameObject Get(string key)
    {
        return poolDic[key].Get();
    }

    public GameObject Get(string key, Vector2 pos)
    {
        GameObject obj = poolDic[key].Get();
        obj.transform.position = pos;

        return obj;
    }

    public GameObject Get(string key, Transform parent)
    {
        GameObject obj = poolDic[key].Get();
        obj.transform.SetParent(parent);

        return obj;
    }

    public GameObject Get(string key, Transform parent, Vector2 pos)
    {
        GameObject obj = poolDic[key].Get();
        obj.transform.position = pos;
        obj.transform.SetParent(parent);

        return obj;
    }

    public GameObject[] Get(string key, int amount)
    {
        GameObject[] objs = new GameObject[amount];
        for (int i = 0; i < amount; i++)
            objs[i] = poolDic[key].Get();

        return objs;
    }

    public GameObject[] Get(string key, int amount, Transform parent)
    {
        GameObject[] objs = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = poolDic[key].Get();
            obj.transform.SetParent(parent);
            objs[i] = obj;
        }

        return objs;
    }

    public GameObject[] Get(string key, int amount, Transform[] parents)
    {
        GameObject[] objs = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = poolDic[key].Get();
            obj.transform.SetParent(parents[i]);
            objs[i] = obj;
        }

        return objs;
    }

    public GameObject[] Get(string key, int amount, Vector2 pos)
    {
        GameObject[] objs = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = poolDic[key].Get();
            obj.transform.position = pos;
            objs[i] = obj;
        }

        return objs;
    }

    public GameObject[] Get(string key, int amount, Vector2[] poses)
    {
        GameObject[] objs = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = poolDic[key].Get();
            obj.transform.position = poses[i];
            objs[i] = obj;
        }

        return objs;
    }
    #endregion

    #region RETURN
    public void Return(GameObject obj)
    {
        poolDic[obj.name].Return(obj);
    }

    public void Return(GameObject[] objs)
    {
        for (int i = 0; i < objs.Length; i++)
            poolDic[objs[i].name].Return(objs[i]);
    }
    #endregion
}
