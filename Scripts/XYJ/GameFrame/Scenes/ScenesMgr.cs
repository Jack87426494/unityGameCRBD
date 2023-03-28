using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ScenesMgr : BaseMgr<ScenesMgr>
{
    /// <summary>
    /// ͬ�����س���
    /// </summary>
    /// <param name="sceneName">����������</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// �첽���س���
    /// </summary>
    /// <param name="sceneName">����������</param>
    /// <param name="unityAction">�����곡����Ҫʵ�ֵķ���</param>
    public void LoadSceneAsyn(string sceneName,UnityAction unityAction)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsync(sceneName, unityAction));
    }

    private IEnumerator ReallyLoadSceneAsync(string sceneName, UnityAction unityAction)
    {

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);

        
        while(!ao.isDone)
        {
            //�ַ��������ⲿִ�У����½�����
            EventMgr.Instance.EventTrigger("Load",ao.progress);
            yield return ao.progress;
        }
        

        yield return null;
        
        unityAction?.Invoke();
    }
}
