using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Fix
public class PunPoolManager : MonoBehaviour, IPunPrefabPool
{
    private void Start()
    {
        PhotonNetwork.PrefabPool = this;
    }

    public GameObject Instantiate(string key, Vector3 pos, Quaternion rotation)
    {
        GameObject obj = PoolManager.instance.Get(key, pos);        
        obj.SetActive(false);
        obj.transform.SetParent(null);
        return obj;
    }

    public void Destroy(GameObject gameObject)
    {
        PoolManager.instance.Return(gameObject);
    }
}
