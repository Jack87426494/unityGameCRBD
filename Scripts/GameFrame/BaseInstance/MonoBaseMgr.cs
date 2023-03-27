using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBaseMgr<T> : MonoBehaviour where T:MonoBaseMgr<T>
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        instance = this as T;
    }
}
