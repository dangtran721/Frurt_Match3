using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    #region Singleton
    private static EventBus _instance;
    public static EventBus Instance => _instance;
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

    Dictionary<string, List<Action<object[]>>> busEvent = new();

    public void Sub(string key, Action<object[]> action)
    {
        if (!busEvent.ContainsKey(key))
        {
            busEvent[key] = new List<Action<object[]>>();
        }
        busEvent[key].Add(action);
    }

    public void UnSub(string key, Action<object[]> action)
    {
        if (!busEvent.ContainsKey(key)) return;
        busEvent[key].Remove(action);
    }

    public void Notify(string key, params object[] action)
    {
        if (!busEvent.ContainsKey(key)) return;
        foreach (Action<object[]> item in busEvent[key])
        {
            item.Invoke(action);
        }
    }
}