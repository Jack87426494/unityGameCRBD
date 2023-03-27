using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoMgr : AutoMonoBaseMgr<MonoMgr>
{
    //֡����ί��
    private event UnityAction updateEvent; 
    private void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }

    /// <summary>
    /// ���֡���º���
    /// </summary>
    /// <param name="unityAction">Ҫ֡���µĺ���</param>
    public void AddUpdateListener(UnityAction unityAction)
    {
        updateEvent += unityAction;
    }
    /// <summary>
    /// ȡ��֡���º���
    /// </summary>
    /// <param name="unityAction">Ҫȡ����֡���º���</param>
    public void CanselUpdateListener(UnityAction unityAction)
    {
        updateEvent -= unityAction;
    }
}
