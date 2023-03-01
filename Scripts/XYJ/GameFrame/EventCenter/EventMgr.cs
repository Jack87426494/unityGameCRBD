using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �ӿ�ɧ����������װ�����
/// </summary>
interface IEventInfo
{

}

public class EventInfo<T>:IEventInfo
{
    public UnityAction<T> unityAction;
    public EventInfo(UnityAction<T> unityAction)
    {
        this.unityAction += unityAction;
    }
}

public class EventInfo: IEventInfo
{
    public UnityAction unityAction;
    public EventInfo(UnityAction unityAction)
    {
        this.unityAction += unityAction;
    }
}


public class EventMgr : BaseMgr<EventMgr>
{
    //�¼�������
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// ���û�в����ĺ������¼���
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="unityAction">Ҫ��ӵĺ���</param>
    public void AddEventListener(string eventName,UnityAction unityAction)
    {
        
        if(eventDic.ContainsKey(eventName))
        {
            //����¼�������������¼�,ֱ���������������¼���
            (eventDic[eventName] as EventInfo).unityAction += unityAction;
        }
        else
        {
            //����¼���������������¼����´���һ���¼�Ȼ�󽫺�����ӵ��¼���ȥ
            eventDic.Add(eventName, new EventInfo(unityAction));
        }

    }
    /// <summary>
    /// �����һ�������ĺ������¼���
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="unityAction">Ҫ��ӵĺ���</param>
    public void AddEventListener<T>(string eventName, UnityAction<T> unityAction)
    {

        if (eventDic.ContainsKey(eventName))
        {
            //����¼�������������¼�,ֱ���������������¼���
            (eventDic[eventName] as EventInfo<T>).unityAction +=unityAction;
        }
        else
        {
            //����¼���������������¼����´���һ���¼�Ȼ�󽫺�����ӵ��¼���ȥ
            eventDic.Add(eventName, new EventInfo<T>(unityAction));
        }

    }

    /// <summary>
    /// ����û�в������¼�
    /// </summary>
    /// <param name="eventName">Ҫ�������¼�������</param>
    public void EventTrigger(string eventName)
    {
        if(eventDic.ContainsKey(eventName))
        {
            //����¼����İ�������¼����򴥷��¼�
            (eventDic[eventName] as EventInfo).unityAction();
        }
    }
    /// <summary>
    /// ������һ���������¼�
    /// </summary>
    /// <param name="eventName">Ҫ�������¼�������</param>
    public void EventTrigger<T>(string eventName,T obj)
    {
        if (eventDic.ContainsKey(eventName))
        {
            //����¼����İ�������¼����򴥷��¼�
            (eventDic[eventName] as EventInfo<T>).unityAction(obj);
        }
    }

    /// <summary>
    /// ע��û�в������¼��еĺ���
    /// </summary>
    /// <param name="eventName">Ҫע������������</param>
    /// <param name="unityAction">Ҫע���ĺ���</param>
    public void CanselEventListener(string eventName,UnityAction unityAction)
    {
        if(eventDic.ContainsKey(eventName))
        {
            //����¼����İ�������¼�����ע��Ҫע���ĺ���
            (eventDic[eventName] as EventInfo).unityAction -= unityAction;
        }
    }

    /// <summary>
    /// ע����һ���������¼��еĺ���
    /// </summary>
    /// <param name="eventName">Ҫע������������</param>
    /// <param name="unityAction">Ҫע���ĺ���</param>
    public void CanselEventListener<T>(string eventName, UnityAction<T> unityAction)
    {
        if (eventDic.ContainsKey(eventName))
        {
            //����¼����İ�������¼�����ע��Ҫע���ĺ���
            (eventDic[eventName] as EventInfo<T>).unityAction -= unityAction;
        }
    }

    /// <summary>
    /// ����¼�����
    /// </summary>
    public void ClearEventDic()
    {
        eventDic.Clear();
    }
}
