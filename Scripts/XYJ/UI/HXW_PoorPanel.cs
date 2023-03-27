using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HXW_PoorPanel : HXW_MapPanel
{
    Dictionary<string, List<HXW_MapPanel>> panelListDic = new Dictionary<string, List<HXW_MapPanel>>();

    public void ShowPoorPanelAsync<T>(string panelName, Transform father, UnityAction<T> firstTimeCallback = null, UnityAction<T> everyTimeCallBack = null) where T : HXW_MapPanel
    {
        if (panelListDic.ContainsKey(panelName) && panelListDic[panelName].Count > 0)
        {
            List<HXW_MapPanel> panelList = panelListDic[panelName];
            T panel = panelList[panelList.Count - 1] as T;

            RectTransform rectTrans = panel.GetComponent<RectTransform>();

            panel.transform.SetParent(father);
            panel.transform.localScale = Vector3.one;

            Show(ShowType.Always, panel, everyTimeCallBack);
            panelList.RemoveAt(panelList.Count - 1);
        }
        else
        {
            HXW_ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
            {
                //InitialPanl(obj, father);//��ʼ��UI��Ϣ
                obj.transform.SetParent(father);
                obj.transform.localScale = Vector3.one;

                T panel = obj.GetComponent<T>();//�õ�Ԥ�������ϵĽű����̳���MapPanel��
                if (!panelListDic.ContainsKey(panelName))
                    panelListDic[panelName] = new List<HXW_MapPanel>();//����һ���ء����ǲ�������ݡ�ֻ�����屻���زŻ�ص����б�ֱ������

                Show(ShowType.First, panel, firstTimeCallback);
                Show(ShowType.Always, panel, everyTimeCallBack);

            });
        }
    }

    //�������
    public void HidePanel(string panelName, HXW_MapPanel mapPanel)//�˷�������panel�ص�����
    {
        if (panelListDic.ContainsKey(panelName))
        {
            panelListDic[panelName].Add(mapPanel);
            mapPanel.HideMeHandler();
            mapPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("������" + panelName + "���");
        }
    }

    //�������
    public void DestoryPanel(string panelName, HXW_MapPanel mapPanel)//�˷�����ֱ��ɾ��������
    {
        if (!panelListDic.ContainsKey(panelName))
            return;

        List<HXW_MapPanel> panelList = panelListDic[panelName];
        panelList.Remove(mapPanel);//����ȱ������ٱ�ɾ��������Ҫ�Ƴ��б��е�����

        mapPanel.HideMeHandler();
        GameObject.Destroy(mapPanel.gameObject);
    }

}
