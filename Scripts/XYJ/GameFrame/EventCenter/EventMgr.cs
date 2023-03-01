using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 接口骚操作，避免装箱拆箱
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
    //事件的容器
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加没有参数的函数到事件中
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="unityAction">要添加的函数</param>
    public void AddEventListener(string eventName,UnityAction unityAction)
    {
        
        if(eventDic.ContainsKey(eventName))
        {
            //如果事件容器包含这个事件,直接添加这个函数到事件中
            (eventDic[eventName] as EventInfo).unityAction += unityAction;
        }
        else
        {
            //如果事件容器不包含这个事件，新创建一个事件然后将函数添加到事件中去
            eventDic.Add(eventName, new EventInfo(unityAction));
        }

    }
    /// <summary>
    /// 添加有一个参数的函数到事件中
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="unityAction">要添加的函数</param>
    public void AddEventListener<T>(string eventName, UnityAction<T> unityAction)
    {

        if (eventDic.ContainsKey(eventName))
        {
            //如果事件容器包含这个事件,直接添加这个函数到事件中
            (eventDic[eventName] as EventInfo<T>).unityAction +=unityAction;
        }
        else
        {
            //如果事件容器不包含这个事件，新创建一个事件然后将函数添加到事件中去
            eventDic.Add(eventName, new EventInfo<T>(unityAction));
        }

    }

    /// <summary>
    /// 触发没有参数的事件
    /// </summary>
    /// <param name="eventName">要触发的事件的名字</param>
    public void EventTrigger(string eventName)
    {
        if(eventDic.ContainsKey(eventName))
        {
            //如果事件中心包含这个事件，则触发事件
            (eventDic[eventName] as EventInfo).unityAction();
        }
    }
    /// <summary>
    /// 触发有一个参数的事件
    /// </summary>
    /// <param name="eventName">要触发的事件的名字</param>
    public void EventTrigger<T>(string eventName,T obj)
    {
        if (eventDic.ContainsKey(eventName))
        {
            //如果事件中心包含这个事件，则触发事件
            (eventDic[eventName] as EventInfo<T>).unityAction(obj);
        }
    }

    /// <summary>
    /// 注销没有参数的事件中的函数
    /// </summary>
    /// <param name="eventName">要注销函数的名字</param>
    /// <param name="unityAction">要注销的函数</param>
    public void CanselEventListener(string eventName,UnityAction unityAction)
    {
        if(eventDic.ContainsKey(eventName))
        {
            //如果事件中心包含这个事件，则注销要注销的函数
            (eventDic[eventName] as EventInfo).unityAction -= unityAction;
        }
    }

    /// <summary>
    /// 注销有一个参数的事件中的函数
    /// </summary>
    /// <param name="eventName">要注销函数的名字</param>
    /// <param name="unityAction">要注销的函数</param>
    public void CanselEventListener<T>(string eventName, UnityAction<T> unityAction)
    {
        if (eventDic.ContainsKey(eventName))
        {
            //如果事件中心包含这个事件，则注销要注销的函数
            (eventDic[eventName] as EventInfo<T>).unityAction -= unityAction;
        }
    }

    /// <summary>
    /// 清空事件中心
    /// </summary>
    public void ClearEventDic()
    {
        eventDic.Clear();
    }
}
