using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UIMgr : MonoBehaviour
{
    private static UIMgr instance = new UIMgr();
    public static UIMgr Instance => instance;

    //panel存放的文件路径
    private string Panel_Path = Application.dataPath + "/Resources/Panel/";

    //存放panel的字典
    private Dictionary<string, BasePanel> panelsDic = new Dictionary<string, BasePanel>();

    private Transform canvasTransform;

    public UIMgr()
    {
        GameObject canvesObj = GameObject.Instantiate(Resources.Load<GameObject>("Panel/Canvas"));
        canvasTransform = canvesObj.transform;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板的脚本名</typeparam>
    /// <param name="isFade">是够显隐</param>
    /// <returns>面板脚本</returns>
    public T ShowPanel<T>(bool isFade = true) where T : BasePanel
    {
        //如果不存在文件夹路径,就创建一个文件夹
        if (!Directory.Exists(Panel_Path))
            Directory.CreateDirectory(Panel_Path);

        string panelName = typeof(T).Name;

        //如果字典中存在此panel，直接返回
        if (panelsDic.ContainsKey(panelName))
            return panelsDic[panelName] as T;
        else
        {
            //直接实例化一个panel对象
            GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("Panel/" + panelName));
            //将panel对象放在正确的位置
            panelObj.transform.SetParent(canvasTransform, false);
            T panelCs = panelObj.GetComponent<T>();
            if (isFade)
            {
                //设置显隐
                panelCs.ShowPanel();
            }
            //将新实例化的panel加入字典
            panelsDic.Add(panelName, panelCs);
            return panelCs;
        }
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板的类</typeparam>
    /// <param name="isFade">是否显隐</param>
    /// <param name="isDelete">是否删除面板</param>
    public void HidePanel<T>(bool isFade = true, bool isDelete = true) where T : BasePanel
    {
        //如果不存在文件夹路径,就创建一个文件夹
        if (!Directory.Exists(Panel_Path))
            Directory.CreateDirectory(Panel_Path);

        string panelName = typeof(T).Name;

        if (panelsDic.ContainsKey(panelName))
        {
            T panelCs = panelsDic[panelName] as T;

            if (isFade)
            {
                panelsDic[panelName].HidePanel(() =>
                {
                    if (isDelete)
                        //删除panel对象
                        GameObject.Destroy(panelCs.gameObject);

                    //将panel从字典中移除
                    panelsDic.Remove(panelName);
                });
            }
            else
            {
                if (isDelete)
                    //删除panel对象
                    GameObject.Destroy(panelCs.gameObject);

                //将panel从字典中移除
                panelsDic.Remove(panelName);
            }
        }
    }

    /// <summary>
    /// 获得面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <returns>返回面板类</returns>
    public T GetPanel<T>() where T : BasePanel
    {
        //如果不存在文件夹路径,就创建一个文件夹
        if (!Directory.Exists(Panel_Path))
            Directory.CreateDirectory(Panel_Path);

        string panelName = typeof(T).Name;

        //如果字典中存在这个面板则返回，否则返回一个默认的类
        if (panelsDic.ContainsKey(panelName))
        {
            return panelsDic[panelName] as T;
        }
        else
            return default(T);
    }
}
