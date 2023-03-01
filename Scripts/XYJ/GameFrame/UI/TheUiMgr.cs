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
    //�������
    private Dictionary<string, TheBasePanel> panelDic = new Dictionary<string, TheBasePanel>(); 

    //Canvas��Rectλ��
    private RectTransform canvasRect;
    ////Canvas��λ��
    //private Transform canvasPos;
    //�ײ�λ��
    private Transform bottomPos;
    //�в�λ��
    private Transform middlePos;
    //�ϲ�λ��
    private Transform TopPos;
    //ϵͳ��λ��
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
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">������������</typeparam>
    /// <param name="posType">λ�õ�����</param>
    /// <param name="isFade">�Ƿ�����</param>
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
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="isFade">�Ƿ�����</param>
    /// <param name="callBack">�ص�����</param>
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
    /// �õ����ڳ����ϵ����
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="panelName">�������</param>
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
    /// �����Զ����¼�
    /// </summary>
    /// <param name="uIBehaviour">�ؼ�</param>
    /// <param name="eventTriggerType">�ؼ�����</param>
    /// <param name="customEvent">Ҫ��ӵ�ʱ��</param>
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
