using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//UI层级枚举
public enum PanelLayer
{
    Bot,
    Mid,
    Top,
    Max,
}


//UI管理器（管理面板）
//管理所有显示的面板
//提供给外部 显示和隐藏
public class HXW_UIManager : HXW_BaseManager<HXW_UIManager>
{
    private HXW_BasePanel basePanel;
    private HXW_PoorPanel poorPanel;

    //这是几个UI面板
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform max;

    public HXW_UIManager()
    {
        //去找Canvas（做成了预设体在Resources/UI下面）
        GameObject obj = HXW_ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        basePanel = obj.AddComponent<HXW_BasePanel>();
        basePanel.InitialForUIManager();
        poorPanel = obj.AddComponent<HXW_PoorPanel>();

        Transform canvas = obj.transform;
        //创建Canvas，让其过场景的时候不被移除
        GameObject.DontDestroyOnLoad(canvas);

        //找到各层
        bot = basePanel.GetPanel("Bot").transform;
        mid = basePanel.GetPanel("Mid").transform;
        top = basePanel.GetPanel("Top").transform;
        max = basePanel.GetPanel("Max").transform;

        //加载EventSystem，有了它，按钮等组件才能响应
        obj = GameObject.Find("EventSystem");
        if (obj == null)
        {
            obj = HXW_ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");
        }

        //创建Canvas，让其过场景的时候不被移除
        GameObject.DontDestroyOnLoad(obj);
    }

    public HXW_MapPanel GetNewPanel(string name)
    {
        return basePanel.GetNewPanel(name);
    }

    //用于游戏提示的快捷方法，暂时无法使用，需要根据项目具体来看
    //public void ShowTips(string content)
    //{
    //    Transform father = GetFather(PanelLayer.Max);
    //    poorPanel.ShowPoorPanelAsync<GameTip>("GameTip", father, null, (panel)=> { panel.SetContent(content); });
    //}

    //ShowPoorPanelAsync与ShowPanelAsync的区别在于这个可以展示名字相同的面板，

    public void ShowPoorPanelAsync<T>(string panelName, PanelLayer layer = PanelLayer.Top, UnityAction<T> firstTimeCallback = null, UnityAction<T> everyTimeCallBack = null) where T : HXW_MapPanel
    {
        Transform father = GetFather(layer);
        poorPanel.ShowPoorPanelAsync<T>(panelName, father, firstTimeCallback, everyTimeCallBack);
    }
    //隐藏面板
    public void HidePoorPanel(string panelName, HXW_MapPanel mapPanel)//此方法会让panel回到池中
    {
        poorPanel.HidePanel(panelName, mapPanel);
    }
    //删除重复面板
    public void DestroyPoorPanel(string panelName, HXW_MapPanel mapPanel)
    {
        poorPanel.DestoryPanel(panelName, mapPanel);
    }

    //用于游戏提示的快捷方法，暂时无法使用，需要根据项目具体来看
    //public void HideTips(MapPanel mapPanel)
    //{
    //    poorPanel.HidePanel("GameTip", mapPanel);
    //}

    /// <summary>
    /// 立即显示面板
    /// </summary>
    /// <typeparam name="T">面板上挂在的脚本类名</typeparam>
    /// <param name="panelName">面板在 “Resources/UI/”后的路径。建议直接通过T.prefabsPath来获取</param>
    /// <param name="layer">面板的层级</param>
    /// <param name="firstTimeCallback">第一次展现面板时调用的方法，传入的参数就是面板的脚本实例</param>
    /// <param name="everyTimeCallBack">每一次展现面板时调用的方法，传入的参数就是面板的脚本实例</param>
    public void ShowPanel<T>(string panelName, PanelLayer layer = PanelLayer.Top, UnityAction<T> firstTimeCallback = null, UnityAction<T> everyTimeCallBack = null) where T : HXW_MapPanel
    {
        Transform father = GetFather(layer);
        basePanel.ShowPanel(panelName, father, firstTimeCallback, everyTimeCallBack);
    }
    /// <summary>
    /// 延缓加载面板的资源时间后显示面板
    /// </summary>
    /// <typeparam name="T">面板上挂在的脚本类名</typeparam>
    /// <param name="panelName">面板在 “Resources/UI/”后的路径。建议直接通过T.prefabsPath来获取</param>
    /// <param name="layer">面板的层级</param>
    /// <param name="firstTimeCallback">第一次展现面板时调用的方法，传入的参数就是面板的脚本实例</param>
    /// <param name="everyTimeCallBack">每一次展现面板时调用的方法，传入的参数就是面板的脚本实例</param>
    public void ShowPanelAsync<T>(string panelName, PanelLayer layer = PanelLayer.Top, UnityAction<T> firstTimeCallback = null, UnityAction<T> everyTimeCallBack = null) where T : HXW_MapPanel
    {
        Transform father = GetFather(layer);
        basePanel.ShowPanelAsync(panelName, father, firstTimeCallback, everyTimeCallBack);
    }

    private Transform GetFather(PanelLayer layer)
    {
        Transform father = bot;
        switch (layer)
        {
            case PanelLayer.Mid:
                father = mid;
                break;
            case PanelLayer.Top:
                father = top;
                break;
            case PanelLayer.Max:
                father = max;
                break;
            default:
                break;
        }
        return father;
    }

    /// <summary>
    /// 仅隐藏，不删除
    /// </summary>
    /// <param name="panelName">面板在 “Resources/UI/”后的路径。建议直接通过 类名.prefabsPath来获取</param>
    public void HidePanel(string panelName)
    {
        basePanel.HidePanel(panelName);
    }
    /// <summary>
    /// 删除面板
    /// </summary>
    /// <param name="panelName">面板在 “Resources/UI/”后的路径。建议直接通过 类名.prefabsPath来获取</param>
    public void DestroyPanel(string panelName)
    {
        basePanel.DestroyPanel(panelName);
    }

}
