using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//拥有对某个子物体动态添加面板的能力。这些新增面板不能重名
public class HXW_BasePanel : HXW_MapPanel
{
    //新增的Panel，也就是动态加载的
    private Dictionary<string, HXW_MapPanel> newPanelDic = new Dictionary<string, HXW_MapPanel>();
    private HashSet<string> hideNewPanelDic = new HashSet<string>();

    //通过ShowPanel等方式获得的继承自MapPanel的对象(包括其子物体的MapPanel对象)会自动初始化
    //但是其他途径获得的无法初始化。比如UIManager的AddComponent
    /// <summary>
    /// 仅为UIManager提供初始化的方法。除了UIManager，不应该被其他类使用。应当使用ShowPanel等方法处理
    /// </summary>
    public void InitialForUIManager()//主要用于TargetPanel的初始化
    {
        ShowMeFirstHandle();
    }

    //获取基础面板
    public HXW_MapPanel GetNewPanel(string name)
    {
        if (!newPanelDic.ContainsKey(name))
            return null;

        return newPanelDic[name];
    }

    public override void ShowMeEveryTimesHandler()
    {
        base.ShowMeEveryTimesHandler();
        foreach (var item in newPanelDic)
        {
            if(!hideNewPanelDic.Contains(item.Key))//如果是被隐藏的Panel就不需要调用他的“OnEnable”（ShowMeAlwaysHandler）
                item.Value.ShowMeEveryTimesHandler();
        }
    }

    public override void HideMeHandler()
    {
        base.HideMeHandler();
        foreach (var item in newPanelDic)
        {
            if (!hideNewPanelDic.Contains(item.Key))
                item.Value.HideMeHandler();
        }
    }

    public override void DestroyHandler()
    {
        base.DestroyHandler();
        foreach (var item in newPanelDic)
        {
            item.Value.DestroyHandler();
        }
    }


    public void ShowPanel<T>(string panelName, Transform father, UnityAction<T> firstTimeCallback = null, UnityAction<T> everyTimeCallBack = null) where T : HXW_MapPanel
    {
        //已经显示过此面板
        if (newPanelDic.ContainsKey(panelName))
        {
            OperiateOldPanel(panelName, everyTimeCallBack);
        }
        else
        {
            GameObject obj = HXW_ResMgr.GetInstance().Load<GameObject>("UI/" + panelName);
            OperiateFirstPanel(obj, panelName, father, firstTimeCallback, everyTimeCallBack);
        }
    }

    public void ShowPanelAsync<T>(string panelName, Transform father, UnityAction<T> firstTimeCallback = null, UnityAction<T> everyTimeCallBack = null) where T : HXW_MapPanel
    {
        //已经显示过此面板
        if (newPanelDic.ContainsKey(panelName))
        {
            OperiateOldPanel(panelName, everyTimeCallBack);
        }
        else
        {
            HXW_ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
            {
                OperiateFirstPanel(obj, panelName, father, firstTimeCallback, everyTimeCallBack);
            });
        }
    }

    private void OperiateOldPanel<T>(string panelName, UnityAction<T> everyTimeCallBack = null) where T : HXW_MapPanel
    {
        if (hideNewPanelDic.Contains(panelName))
            hideNewPanelDic.Remove(panelName);

        T panel = newPanelDic[panelName].GetComponent<T>();//得到预设体身上的脚本（继承自BasePanel）
        Show(ShowType.Always, panel, everyTimeCallBack);
    }

    private void OperiateFirstPanel<T>(GameObject obj, string panelName, Transform father, UnityAction<T> firstTimeCallback = null, UnityAction<T> everyTimeCallBack = null) where T : HXW_MapPanel
    {
        InitialPanl(obj, father);//初始化UI信息
        T panel = obj.GetComponent<T>();//得到预设体身上的脚本（继承自MapPanel）
        newPanelDic[panelName] = panel;//在字典中添加此面板

        Show(ShowType.First, panel, firstTimeCallback);
        Show(ShowType.Always, panel, everyTimeCallBack);
    }

    //隐藏面板
    public void HidePanel(string panelName)
    {
        if (newPanelDic.ContainsKey(panelName))
        {
            newPanelDic[panelName].HideMeHandler();
            newPanelDic[panelName].gameObject.SetActive(false);//******************修改*********************

            if(!hideNewPanelDic.Contains(panelName))
                hideNewPanelDic.Add(panelName);
        }
        else
        {
            //Debug.LogError("不存在" + panelName + "面板");
        }
    }
    //销毁面板
    public void DestroyPanel(string panelName)
    {
        if (newPanelDic.ContainsKey(panelName))
        {
            //调用重写方法，具体内容自己添加
            newPanelDic[panelName].DestroyHandler();
            GameObject.Destroy(newPanelDic[panelName].gameObject);
            newPanelDic.Remove(panelName);

            if (hideNewPanelDic.Contains(panelName))
                hideNewPanelDic.Remove(panelName);
        }
        else
        {
            Debug.LogError("不存在" + panelName + "面板");
        }
    }
}

