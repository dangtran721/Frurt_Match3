using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    #region Singleton
    private static ObjectPooling _instance;
    public static ObjectPooling Instance => _instance;
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    #endregion

    Dictionary<GameObject, List<GameObject>> Dict = new();

    public GameObject GetObj(GameObject key)
    {
        if (!Dict.ContainsKey(key))
        {
            Dict[key] = new List<GameObject>();
        }
        List<GameObject> PoolObj = Dict[key];

        foreach (GameObject a in PoolObj)
        {
            if(a.gameObject.activeSelf)
            {
                continue;
            }
            return a;
        }
        GameObject Obj = Instantiate(key,this.transform);
        Obj.SetActive(true);
        PoolObj.Add(Obj);
        return Obj;
    }
}
