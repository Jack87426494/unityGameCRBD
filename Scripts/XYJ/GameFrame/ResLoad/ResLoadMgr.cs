using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResLoadMgr : BaseMgr<ResLoadMgr>
{
    /// <summary>
    /// 资源同步加载
    /// </summary>
    /// <typeparam name="T">资源的类型</typeparam>
    /// <param name="resPath">资源的路径</param>
    /// <returns>加载的资源</returns>
    public T Load<T>(string resPath) where T:Object
    {
        T res=Resources.Load<T>(resPath);

        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }
    
    /// <summary>
    /// 资源异步加载
    /// </summary>
    /// <typeparam name="T">资源的类型</typeparam>
    /// <param name="resPath">资源的路径</param>
    /// <param name="callBack">回调函数，在资源加载完毕后调用</param>
    public void LoadAsyn<T>(string resPath,UnityAction<T> callBack) where T:Object
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadAsyn<T>(resPath, callBack));
    }

    private IEnumerator ReallyLoadAsyn<T>(string resPath, UnityAction<T> callBack) where T:Object
    {
        ResourceRequest r=Resources.LoadAsync<T>(resPath);
        yield return r;
        if (r.asset is GameObject)
        {
            callBack(GameObject.Instantiate(r.asset) as T);
        }
        else
            callBack(r.asset as T);
    }
}
