using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//ӵ�в�������������Ͳ�������������UI�Ĺ���
//����������Ҳ����MapPanel��������ж����ӣ�������������MapPanel���ʱ��ֻ�ܻ�ȡ����������壬�޷��ڼ������»�ȡ
//�޷���ȡ��̬���ص�����

//�����������������ʹ�ü̳���MapPanel����������⴦��һ�������¼������
//1.��Ҫ��̬�������ֲ�ͬ��������Panel��                      ��ʱʹ��BasePanel
//2.��Ҫ��̬����ĳ�������ظ���������Panel���磺�������ӡ�    ��ʱʹ��ContainerPanel
//3.һ��ʼ���е�Panel����װ������                              ����Ҫ���⴦��

public enum ShowType
{
    First,
    Always
}

public class HXW_MapPanel : MonoBehaviour
{
    //���е�Panel,Ҳ���ǷǶ�̬���ص�
    //protected Dictionary<string, TargetPanel> childPanelDic = new Dictionary<string, TargetPanel>();
    protected List<HXW_MapPanel> childMapPanels = new List<HXW_MapPanel>();

    private TargetPanel thisPanel;
    protected TargetPanel ThisPanel
    {
        get 
        {
            if(thisPanel == null)//����UIManager�����⴦��
                thisPanel = new TargetPanel(this, childMapPanels);//�ȳ�ʼ��TargetPanel
            return thisPanel;
        }
    }

    //���ߵ�ǰPanel������ͨ��·�����������壬·���������Լ���ֱ�Ӵӵ�һ�������忪ʼ
    //·������ͨ���˵���window��ߵ�MyCommond��GetNamePath�����ٻ�ȡ
    public TargetPanel GetPanel(string path)
    {
        return ThisPanel.GetPanel(path);
    }

    protected void InitialPanl(GameObject obj, Transform father)
    {
        //���ø�����
        obj.transform.SetParent(father);

        //�������λ�úʹ�С
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        (obj.transform as RectTransform).offsetMax = Vector2.zero;
        (obj.transform as RectTransform).offsetMin = Vector2.zero;
    }

    protected void Show<T>(ShowType showType,T panel, UnityAction<T> callback = null) where T : HXW_MapPanel
    {
        if (!panel.gameObject.activeInHierarchy)//����������Ѿ�������ʾ���Ͳ���Ҫ��ʾ
            panel.gameObject.SetActive(true);//******************�޸�*********************

        if(showType == ShowType.First)
            panel.ShowMeFirstHandle();               //�ȳ�ʼ�����࣬�ٳ�ʼ�����࣬��ʱ������Ҫ�����ṩһЩ����
        else
            panel.ShowMeEveryTimesHandler();

        if (callback != null)
            callback(panel as T);
    }

    /// <summary>
    /// �൱��Awake���������һ��ʼ�ʹ��ڵ��������Ч����̬���ص������Ҫʹ�ù�����BasePanel��ContainerPanel
    /// </summary>
    public virtual void ShowMeFirst()
    {

    }
    /// <summary>
    /// �൱��OnEnable���������һ��ʼ�ʹ��ڵ��������Ч����̬���ص����Ӧʹ�ù�����BasePanel��ContainerPanel
    /// </summary>
    public virtual void ShowMeEveryTime()
    {

    }

    //Ӧ���ɹ�����ʹ�û���д���磺BasePanel��ContainerPanel
    public virtual void ShowMeFirstHandle()
    {
        thisPanel = new TargetPanel(this, childMapPanels);//�ȳ�ʼ��TargetPanel


        for (int i = 0; i < childMapPanels.Count; i++)
        {
            childMapPanels[i].ShowMeFirstHandle();
        }

        ShowMeFirst();

    }
    //Ӧ���ɹ�����ʹ�û���д���磺BasePanel��ContainerPanel
    public virtual void ShowMeEveryTimesHandler()
    {
        for (int i = 0; i < childMapPanels.Count; i++)
        {
            childMapPanels[i].ShowMeEveryTimesHandler();
        }

        ShowMeEveryTime();
    }

    //�������������/����ʱ��׼��
    public virtual void HideMe()
    {

    }
    public virtual void DestroyMe()
    {

    }

    /// <summary>
    /// �ǹ����಻����д�÷���
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
    /// �ǹ����಻����д�÷���
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
    public Transform transform;//��Ȼ����ͨ��UIBehaciour��ȡGameObject������ʱ����Ҫ���������û���ض������
    public List<UIBehaviour> allUI = new List<UIBehaviour>();//��ChilePanel������UI���
    public Dictionary<string, TargetPanel> childPanelDic = new Dictionary<string, TargetPanel>();//��ChilePanel������ChildPanel

    //��һ�β����BasePanel��֮������BasePanel���ڼ�������Ѱ��
    public TargetPanel(HXW_MapPanel fatherMapPanel, List<HXW_MapPanel> childMapPanels) : this(fatherMapPanel.transform, childMapPanels,false) { }
    //���캯��
    private TargetPanel(Transform transfrom, List<HXW_MapPanel> childMapPanels, bool isCheck = true)
    {
        this.transform = transfrom;
        this.SetAllUIBehaviour();

        if (transform.childCount <= 0)
            return;
        
        if (isCheck )//����������Panel�������
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

    //·�����ֲ���Ҫ������BasePanel��ֱ�Ӵ�BasePanel�ĵ�һ�������忪ʼ����/����
    public TargetPanel GetPanel(string path)
    {
        Queue<string> pathQue = new Queue<string>(path.Split('/'));
        return this.GetPanelByQue(pathQue);
    }

    //���ⲿ�ṩ�Ĳ���Ŀ��Panel�ĺ���
    private TargetPanel GetPanelByQue(Queue<string> paths)
    {
        if (paths.Count <= 0)
            return this;
        string childName = paths.Dequeue();

        if (!childPanelDic.ContainsKey(childName))
        {
            Debug.Log("����ʧ��,����·���Ƿ�������·�����������庬�м̳���MapPanel�����");
            return null;
        }

        return childPanelDic[childName].GetPanelByQue(paths);//�ݹ��ȡchildPanel
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

    //��ʼ����������childPanel
    private void FindChild(Transform father,List<HXW_MapPanel> childMapPanel)
    {
        for (int i = 0; i < father.childCount; i++)
        {
            Transform child = father.GetChild(i);
            childPanelDic[child.name] = new TargetPanel(child, childMapPanel);//ͬһ�������£��������������������塣��������ĻḲ��֮ǰ��
        }
    }
    //��ʼ����ǰPanel���������
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