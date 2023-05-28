using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//拥有查找所有子物体和部分类型子物体UI的功能
//对于子物体也含有MapPanel的情况会中断连接，在子物体中有MapPanel组件时，只能获取到这个子物体，无法在继续向下获取
//无法获取动态加载的物体

//对于这种情况，可以使用继承自MapPanel的类进行特殊处理。一般有以下几种情况
//1.需要动态加载名字不同的子面板的Panel。                      此时使用BasePanel
//2.需要动态加载某个名字重复的子面板的Panel。如：背包格子。    此时使用ContainerPanel
//3.一开始就有的Panel，如装备格子                              不需要特殊处理

public enum ShowType
{
    First,
    Always
}

public class HXW_MapPanel : MonoBehaviour
{
    //已有的Panel,也就是非动态加载的
    //protected Dictionary<string, TargetPanel> childPanelDic = new Dictionary<string, TargetPanel>();
    protected List<HXW_MapPanel> childMapPanels = new List<HXW_MapPanel>();

    private TargetPanel thisPanel;
    protected TargetPanel ThisPanel
    {
        get 
        {
            if(thisPanel == null)//对于UIManager的特殊处理
                thisPanel = new TargetPanel(this, childMapPanels);//先初始化TargetPanel
            return thisPanel;
        }
    }

    //更具当前Panel的名字通过路径查找子物体，路径不包括自己，直接从第一级子物体开始
    //路径可以通过菜单栏window左边的MyCommond的GetNamePath来快速获取
    public TargetPanel GetPanel(string path)
    {
        return ThisPanel.GetPanel(path);
    }

    protected void InitialPanl(GameObject obj, Transform father)
    {
        //设置父对象
        obj.transform.SetParent(father);

        //设置相对位置和大小
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        (obj.transform as RectTransform).offsetMax = Vector2.zero;
        (obj.transform as RectTransform).offsetMin = Vector2.zero;
    }

    protected void Show<T>(ShowType showType,T panel, UnityAction<T> callback = null) where T : HXW_MapPanel
    {
        if (!panel.gameObject.activeInHierarchy)//如果这个面板已经正在显示。就不需要显示
            panel.gameObject.SetActive(true);//******************修改*********************

        if(showType == ShowType.First)
            panel.ShowMeFirstHandle();               //先初始化父类，再初始化子类，有时子类需要父类提供一些变量
        else
            panel.ShowMeEveryTimesHandler();

        if (callback != null)
            callback(panel as T);
    }

    /// <summary>
    /// 相当于Awake。仅对面板一开始就存在的子面板有效。动态加载的面板需要使用工具类BasePanel，ContainerPanel
    /// </summary>
    public virtual void ShowMeFirst()
    {

    }
    /// <summary>
    /// 相当于OnEnable。仅对面板一开始就存在的子面板有效。动态加载的面板应使用工具类BasePanel，ContainerPanel
    /// </summary>
    public virtual void ShowMeEveryTime()
    {

    }

    //应当由工具类使用或重写。如：BasePanel，ContainerPanel
    public virtual void ShowMeFirstHandle()
    {
        thisPanel = new TargetPanel(this, childMapPanels);//先初始化TargetPanel


        for (int i = 0; i < childMapPanels.Count; i++)
        {
            childMapPanels[i].ShowMeFirstHandle();
        }

        ShowMeFirst();

    }
    //应当由工具类使用或重写。如：BasePanel，ContainerPanel
    public virtual void ShowMeEveryTimesHandler()
    {
        for (int i = 0; i < childMapPanels.Count; i++)
        {
            childMapPanels[i].ShowMeEveryTimesHandler();
        }

        ShowMeEveryTime();
    }

    //用于面板在隐藏/销毁时的准备
    public virtual void HideMe()
    {

    }
    public virtual void DestroyMe()
    {

    }

    /// <summary>
    /// 非管理类不可重写该方法
    /// </summary>
    public virtual void HideMeHandler()
    {
        HideMe();
        for (int i = 0; i < childMapPanels.Count; i++)
        {
            childMapPanels[i].HideMe();
        }
    }
    /// <summary>
    /// 非管理类不可重写该方法
    /// </summary>
    public virtual void DestroyHandler()
    {
        HideMeHandler();

        DestroyMe();
        for (int i = 0; i < childMapPanels.Count; i++)
        {
            childMapPanels[i].DestroyMe();
        }
    }
}

public class TargetPanel
{
    public Transform transform;//虽然可以通过UIBehaciour获取GameObject但是有时候。需要的物体可能没有特定的组件
    public List<UIBehaviour> allUI = new List<UIBehaviour>();//该ChilePanel的所有UI组件
    public Dictionary<string, TargetPanel> childPanelDic = new Dictionary<string, TargetPanel>();//该ChilePanel的所有ChildPanel

    //第一次不检测BasePanel，之后遇见BasePanel则不在继续向下寻找
    public TargetPanel(HXW_MapPanel fatherMapPanel, List<HXW_MapPanel> childMapPanels) : this(fatherMapPanel.transform, childMapPanels,false) { }
    //构造函数
    private TargetPanel(Transform transfrom, List<HXW_MapPanel> childMapPanels, bool isCheck = true)
    {
        this.transform = transfrom;
        this.SetAllUIBehaviour();

        if (transform.childCount <= 0)
            return;
        
        if (isCheck )//避免新增的Panel加入计算
        {
            HXW_MapPanel childMapPanel = transform.GetComponent<HXW_MapPanel>();
            if (childMapPanel != null)
            {
                childMapPanels.Add(childMapPanel);
                return;
            }
        }
        FindChild(this.transform, childMapPanels);
    }

    //路径名字不需要包含从BasePanel，直接从BasePanel的第一级子物体开始，以/划分
    public TargetPanel GetPanel(string path)
    {
        Queue<string> pathQue = new Queue<string>(path.Split('/'));
        return this.GetPanelByQue(pathQue);
    }

    //给外部提供的查找目标Panel的函数
    private TargetPanel GetPanelByQue(Queue<string> paths)
    {
        if (paths.Count <= 0)
            return this;
        string childName = paths.Dequeue();

        if (!childPanelDic.ContainsKey(childName))
        {
            Debug.Log("查找失败,请检查路径是否错误或者路径中有子物体含有继承自MapPanel的组件");
            return null;
        }

        return childPanelDic[childName].GetPanelByQue(paths);//递归获取childPanel
    }

    public T GetUI<T>() where T : UIBehaviour
    {
        for (int i = 0; i < allUI.Count; i++)
        {
            if (allUI[i] is T)
                return allUI[i] as T;
        }
        return null;
    }

    //初始化查找所有childPanel
    private void FindChild(Transform father,List<HXW_MapPanel> childMapPanel)
    {
        for (int i = 0; i < father.childCount; i++)
        {
            Transform child = father.GetChild(i);
            childPanelDic[child.name] = new TargetPanel(child, childMapPanel);//同一父物体下，不可以有重名的子物体。否则后来的会覆盖之前的
        }
    }
    //初始化当前Panel的所有组件
    private void SetAllUIBehaviour()
    {
        AddUI<Button>();
        AddUI<Image>();
        AddUI<Text>();
        AddUI<Toggle>();
        AddUI<ScrollRect>();
        AddUI<Slider>();
        AddUI<TMP_Text>();
        AddUI<RawImage>();
    }

    private void AddUI<T>() where T : UIBehaviour
    {
        T ui = transform.GetComponent<T>();

        if (ui != null)
            allUI.Add(ui);
    }
}