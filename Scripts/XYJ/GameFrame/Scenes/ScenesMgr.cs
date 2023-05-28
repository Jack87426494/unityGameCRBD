using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ScenesMgr : BaseMgr<ScenesMgr>
{
    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <param name="sceneName">场景的名字</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName">场景的名字</param>
    /// <param name="unityAction">加载完场景需要实现的方法</param>
    public void LoadSceneAsyn(string sceneName,UnityAction unityAction)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsync(sceneName, unityAction));
    }

    private IEnumerator ReallyLoadSceneAsync(string sceneName, UnityAction unityAction)
    {

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);

        
        while(!ao.isDone)
        {
            //分发函数给外部执行，跟新进度条
            EventMgr.Instance.EventTrigger("Load",ao.progress);
            yield return ao.progress;
        }
        

        yield return null;
        
        unityAction?.Invoke();
    }
}
