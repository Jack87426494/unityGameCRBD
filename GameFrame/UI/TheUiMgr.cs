using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum PosType
{
    Bot,
    Mid,
    Top,
    Sys
}

public class TheUiMgr : BaseMgr<TheUiMgr>
{
    //面板容器
    private Dictionary<string, TheBasePanel> panelDic = new Dictionary<string, TheBasePanel>(); 

    //Canvas的Rect位置
    private RectTransform canvasRect;
    ////Canvas的位置
    //private Transform canvasPos;
    //底层位置
    private Transform bottomPos;
    //中层位置
    private Transform middlePos;
    //上层位置
    private Transform TopPos;
    //系统层位置
    private Transform SystemPos;
    
    public TheUiMgr()
    {
        GameObject obj = ResLoadMgr.Instance.Load<GameObject>("Ui/Canvas");
        ResLoadMgr.Instance.Load<GameObject>("Ui/EventSystem");
        
        canvasRect = obj.transform as RectTransform;
        bottomPos = canvasRect.Find("Bot");
        middlePos = canvasRect.Find("Mid");
        TopPos = canvasRect.Find("Top");
        SystemPos = canvasRect.Find("Sys");
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板的类型名称</typeparam>
    /// <param name="posType">位置的类型</param>
    /// <param name="isFade">是否显隐</param>
    /// <returns></returns>
    public void ShowPanel<T>(PosType posType=PosType.Bot,UnityAction<T> callBack=null,
        bool isFade = true) where T:TheBasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            callBack(panelDic[panelName] as T);
        else
        {
            ResLoadMgr.Instance.LoadAsyn<GameObject>("Ui/" + panelName, (panel) =>
            {
               
                T panelScrip = panel.GetComponent<T>();
                switch (posType)
                {
                    case PosType.Bot:
                        panel.gameObject.transform.SetParent(bottomPos);
                        break;
                    case PosType.Mid:
                        panel.gameObject.transform.SetParent(middlePos);
                        break;
                    case PosType.Top:
                        panel.gameObject.transform.SetParent(TopPos);
                        break;
                    case PosType.Sys:
                        panel.gameObject.transform.SetParent(SystemPos);
                        break;
                }

                panel.transform.localPosition = Vector3.zero;
                panel.transform.localScale = Vector3.one;
                (panel.transform as RectTransform).offsetMax = Vector2.zero;
                (panel.transform as RectTransform).offsetMin = Vector2.zero;

                if (isFade)
                {
                    panelScrip.ShowPanel();
                }
                panelDic.Add(panelName, panelScrip);

                if(callBack!=null)
                callBack(panelDic[panelName] as T);
            });
        }
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="isFade">是否显隐</param>
    /// <param name="callBack">回调函数</param>
    public void HidePanel<T>(bool isFade=true,UnityAction<T> callBack=null) where T:TheBasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
                (panelDic[panelName] as T).HidePanel(() =>
                {
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    callBack(panelDic[panelName] as T);
                    panelDic.Remove(panelName);
                });
            else
            {
                GameObject.Destroy(panelDic[panelName].gameObject);
                if (callBack != null)
                    callBack(panelDic[panelName] as T);
                panelDic.Remove(panelName);
            }
        }
    }
    /// <summary>
    /// 得到存在场景上的面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="panelName">面板名字</param>
    /// <returns></returns>
    public T GetPanel<T>(string panelName)where T:TheBasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return default(T);
    }
    /// <summary>
    /// 加入自定义事件
    /// </summary>
    /// <param name="uIBehaviour">控件</param>
    /// <param name="eventTriggerType">控件类型</param>
    /// <param name="customEvent">要添加的时间</param>
    public void AddCustomEvent(UIBehaviour uIBehaviour,EventTriggerType eventTriggerType,UnityAction<BaseEventData> customEvent)
    {

        EventTrigger eventTrigger = uIBehaviour.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
            eventTrigger = uIBehaviour.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID= eventTriggerType;
        entry.callback.AddListener(customEvent);
        eventTrigger.triggers.Add(entry);
    }
}
