using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoMgr : AutoMonoBaseMgr<MonoMgr>
{
    //帧更新委托
    private event UnityAction updateEvent; 
    private void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }

    /// <summary>
    /// 添加帧更新函数
    /// </summary>
    /// <param name="unityAction">要帧更新的函数</param>
    public void AddUpdateListener(UnityAction unityAction)
    {
        updateEvent += unityAction;
    }
    /// <summary>
    /// 取消帧更新函数
    /// </summary>
    /// <param name="unityAction">要取消的帧更新函数</param>
    public void CanselUpdateListener(UnityAction unityAction)
    {
        updateEvent -= unityAction;
    }
}
