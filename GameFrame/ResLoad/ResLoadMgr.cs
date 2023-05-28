using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResLoadMgr : BaseMgr<ResLoadMgr>
{
    /// <summary>
    /// ��Դͬ������
    /// </summary>
    /// <typeparam name="T">��Դ������</typeparam>
    /// <param name="resPath">��Դ��·��</param>
    /// <returns>���ص���Դ</returns>
    public T Load<T>(string resPath) where T:Object
    {
        T res=Resources.Load<T>(resPath);

        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }
    
    /// <summary>
    /// ��Դ�첽����
    /// </summary>
    /// <typeparam name="T">��Դ������</typeparam>
    /// <param name="resPath">��Դ��·��</param>
    /// <param name="callBack">�ص�����������Դ������Ϻ����</param>
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
