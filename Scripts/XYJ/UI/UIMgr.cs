using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UIMgr : MonoBehaviour
{
    private static UIMgr instance = new UIMgr();
    public static UIMgr Instance => instance;

    //panel��ŵ��ļ�·��
    private string Panel_Path = Application.dataPath + "/Resources/Panel/";

    //���panel���ֵ�
    private Dictionary<string, BasePanel> panelsDic = new Dictionary<string, BasePanel>();

    private Transform canvasTransform;

    public UIMgr()
    {
        GameObject canvesObj = GameObject.Instantiate(Resources.Load<GameObject>("Panel/Canvas"));
        canvasTransform = canvesObj.transform;
    }

    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">���Ľű���</typeparam>
    /// <param name="isFade">�ǹ�����</param>
    /// <returns>���ű�</returns>
    public T ShowPanel<T>(bool isFade = true) where T : BasePanel
    {
        //����������ļ���·��,�ʹ���һ���ļ���
        if (!Directory.Exists(Panel_Path))
            Directory.CreateDirectory(Panel_Path);

        string panelName = typeof(T).Name;

        //����ֵ��д��ڴ�panel��ֱ�ӷ���
        if (panelsDic.ContainsKey(panelName))
            return panelsDic[panelName] as T;
        else
        {
            //ֱ��ʵ����һ��panel����
            GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("Panel/" + panelName));
            //��panel���������ȷ��λ��
            panelObj.transform.SetParent(canvasTransform, false);
            T panelCs = panelObj.GetComponent<T>();
            if (isFade)
            {
                //��������
                panelCs.ShowPanel();
            }
            //����ʵ������panel�����ֵ�
            panelsDic.Add(panelName, panelCs);
            return panelCs;
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">������</typeparam>
    /// <param name="isFade">�Ƿ�����</param>
    /// <param name="isDelete">�Ƿ�ɾ�����</param>
    public void HidePanel<T>(bool isFade = true, bool isDelete = true) where T : BasePanel
    {
        //����������ļ���·��,�ʹ���һ���ļ���
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
                        //ɾ��panel����
                        GameObject.Destroy(panelCs.gameObject);

                    //��panel���ֵ����Ƴ�
                    panelsDic.Remove(panelName);
                });
            }
            else
            {
                if (isDelete)
                    //ɾ��panel����
                    GameObject.Destroy(panelCs.gameObject);

                //��panel���ֵ����Ƴ�
                panelsDic.Remove(panelName);
            }
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <returns>���������</returns>
    public T GetPanel<T>() where T : BasePanel
    {
        //����������ļ���·��,�ʹ���һ���ļ���
        if (!Directory.Exists(Panel_Path))
            Directory.CreateDirectory(Panel_Path);

        string panelName = typeof(T).Name;

        //����ֵ��д����������򷵻أ����򷵻�һ��Ĭ�ϵ���
        if (panelsDic.ContainsKey(panelName))
        {
            return panelsDic[panelName] as T;
        }
        else
            return default(T);
    }
}
