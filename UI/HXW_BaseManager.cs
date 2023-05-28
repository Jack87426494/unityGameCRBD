using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HXW_BaseManager<T> where T:new()
{
    private static T _instance;
    public static T Instance
    {
        get { return GetInstance(); }
    }
    public static T GetInstance() {
        if (_instance == null) {
            _instance = new T();
        }
        return _instance;
    }
}
