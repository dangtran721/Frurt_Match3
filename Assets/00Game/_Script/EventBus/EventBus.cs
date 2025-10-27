using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
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
