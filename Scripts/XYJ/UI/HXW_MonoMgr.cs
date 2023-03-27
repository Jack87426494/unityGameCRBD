using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;


public class CountdownTimerInfo
{
    private float currentTime;
    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }
    public bool isBreak = false;
}

public class HXW_MonoMgr : HXW_BaseManager<HXW_MonoMgr>
{
    private HXW_MonoController controller;
    public HXW_MonoMgr() {
        //新建一个物体
        GameObject obj = new GameObject("MonoController");
        //给物体添加组件
        controller = obj.AddComponent<HXW_MonoController>();
    }
    public void AddUpdateListener(UnityAction func)
    {
        controller.AddUpdateListener(func);
    }
    public void RemoveUpdateListener(UnityAction func)
    {
        controller.RemoveUpdateListener(func);
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value) {
        return controller.StartCoroutine(methodName,value);
    }
    public Coroutine StartCoroutine(string methodName) {
        return controller.StartCoroutine(methodName);
    }
    public void StopCoroutine(Coroutine Coroutine)
    {
        controller.StopCoroutine(Coroutine);
    }
    
    //public void NormalMove(Transform trans, Vector3 targetPos, float speed, Action action = null)
    //{
    //    StartCoroutine(NormalMoveCouroutine(trans, targetPos, speed, action));
    //}
    //public void CountdownTimer(float allTime, Action startAction, Action<CountdownTimerInfo> everyAction,Action endAction)
    //{
    //    StartCoroutine(CountdownTimerCouroutine(allTime, startAction, everyAction, endAction));
    //}
    private IEnumerator CountdownTimerCouroutine(float allTime, Action startAction, Action<CountdownTimerInfo> everyAction, Action endAction)
    {
        CountdownTimerInfo countdownTimeInfo = new CountdownTimerInfo();
        startAction();

        yield return new WaitUntil(()=> 
        {
            if (countdownTimeInfo.CurrentTime >= allTime)
                return true;
            everyAction(countdownTimeInfo);
            countdownTimeInfo.CurrentTime += Time.deltaTime;

            return countdownTimeInfo.isBreak;
        });

        endAction();
    }

    IEnumerator NormalMoveCouroutine(Transform trans,Vector3 targetPos,float speed,Action action = null)//听说UnityAction性能没C#自带的Action好
    {
        yield return new WaitUntil(() =>
        {
            trans.position = Vector3.MoveTowards(trans.position, targetPos, Time.deltaTime * speed);
            if (trans.position == targetPos)
                return true;
            else
                return false;
        });

        if (action != null)//移动结束以后需要做什么
            action.Invoke();
    }
    //public void WaitSomeTime(float time, Action action)
    //{
    //    StartCoroutine(WaitCoroutine(time, action));
    //}

    private IEnumerator WaitCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    #region 延时系统
    ////事件流
    //private static Dictionary<IEventInfo, float> delayActionDic = new Dictionary<IEventInfo, float>();
    ////private List<Coroutine> ies=new List<Coroutine>();
    //private Dictionary<int, Coroutine> coroutines = new Dictionary<int, Coroutine>();//******************修改*********************
    //private int key = 0;

    //public void AddDelayAction(UnityAction action,float time)
    //{
    //    delayActionDic.Add(new EventInfo(action), time);
    //}
    //public void AddDelayAction<T>(UnityAction<T> action, float time)
    //{
    //    delayActionDic.Add(new EventInfo<T>(action), time);
    //}
    //public void ClearDelayActions()
    //{
    //    delayActionDic.Clear();
    //}

    ////这个工具只能一次执行所有相同事件，也就是有参数和无参数不能一起执行，如果需要仅加入一个参数判断即可

    ///// <summary>
    ///// 延时触发事件流
    ///// </summary>
    //public void InvokeDelayActions()
    //{
    //    StopDelayEnumerator();
    //    List<UnityAction> actionList = new List<UnityAction>();
    //    foreach(IEventInfo e in delayActionDic.Keys)
    //    {
    //        actionList.Add((e as EventInfo).actions);
    //    }
    //    List<float> timeList = new List<float>(delayActionDic.Values);
    //    for(int i=0;i<actionList.Count;i++)
    //    {
    //        StartCoroutine(DelayToCallEnumerator(actionList[i],Sum(timeList,i)));
    //    }
    //    ClearDelayActions();
    //}
    ///// <summary>
    ///// 延时触发带参数的事件
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="info">参数组</param>
    //public void InvokeDelayActions<T>(T[] info)
    //{
    //    StopDelayEnumerator();
    //    controller.StopAllCoroutines();
    //    List<UnityAction<T>> actionList = new List<UnityAction<T>>();
    //    foreach (IEventInfo e in delayActionDic.Keys)
    //    {
    //        actionList.Add((e as EventInfo<T>).actions);
    //    }

    //    List<float> timeList = new List<float>(delayActionDic.Values);

    //    for (int i = 0; i < actionList.Count; i++)
    //    {
    //        StartCoroutine(DelayToCallEnumerator(actionList[i],info[i],Sum(timeList, i)));
    //    }
    //    ClearDelayActions();
    //}

    //private float Sum(List<float> list, int index)
    //{
    //    float x = 0;
    //    for(int i=0;i<=index;i++)
    //    {
    //            x += list[i];
    //    }
    //    return x;
    //}

    //private void AddCoroutine(Coroutine coroutine)//******************修改*********************
    //{
    //    coroutines.Add(key, coroutine);
    //    key++;
    //}
    //private void RemoveCoroutine(int key)//******************修改*********************
    //{
    //    coroutines.Remove(key);
    //}
    ///// <summary>
    ///// 对于经常需要的延时操作进行了封装
    ///// </summary>
    ///// <param name="action">事件</param>
    ///// <param name="time">延时事件</param>
    //private void DelayToCall(UnityAction action, float time)//******************修改*********************
    //{
    //    //ies.Add(StartCoroutine(DelayToCallEnumerator(action, time)));
    //    AddCoroutine(StartCoroutine(DelayToCallEnumerator(key,action, time)));
    //}
    //private IEnumerator DelayToCallEnumerator( UnityAction action, float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    action();
    //}

    ////******************修改*********************
    //private IEnumerator DelayToCallEnumerator(int key,UnityAction action, float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    action();
    //    RemoveCoroutine(key);
    //}
    //private void DelayToCall<T>(UnityAction<T> action,T info,float time)//******************修改*********************
    //{
    //    //ies.Add(StartCoroutine(DelayToCallEnumerator(action,info,time)));
    //    AddCoroutine(StartCoroutine(DelayToCallEnumerator(key,action, info, time)));
    //}
    //private IEnumerator DelayToCallEnumerator<T>(UnityAction<T> action,T info,float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    action(info);
    //}

    ////******************修改*********************
    //private IEnumerator DelayToCallEnumerator<T>(int key,UnityAction<T> action, T info, float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    action(info);
    //    RemoveCoroutine(key);
    //}
    //private void StopDelayEnumerator()//******************修改*********************
    //{
    //    //foreach(Coroutine e in ies)
    //    //{
    //    //    controller.StopCoroutine(e);   
    //    //}
    //    //ies.Clear();

    //    foreach(var c in coroutines)
    //    {
    //        controller.StopCoroutine(c.Value);
    //    }
    //    coroutines.Clear();
    //    key = 0;
    //}
    #endregion
}

