using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMonoBaseMgr<T> : MonoBehaviour where T:AutoMonoBaseMgr<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }
            return instance as T;
        }
    }
}
