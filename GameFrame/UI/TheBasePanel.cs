using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class TheBasePanel : MonoBehaviour
{
    //�ؼ�����
    public Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    private CanvasGroup canvasGroup;

    //�Ƿ��ڴ�״̬
    private bool isOpen;

    //�������ٶ�
    private float fadeSpeed=4f;

    //�������ʱִ�е�ί��
    private UnityAction hideAction;

    protected virtual void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        FindChildsControl<Button>();
        FindChildsControl<Image>();
        FindChildsControl<Text>();
        FindChildsControl<Slider>();
        FindChildsControl<Toggle>();

       
    }

    public virtual void ShowPanel()
    {
        isOpen = true;
        canvasGroup.alpha = 0;
      
    }

    public virtual void HidePanel(UnityAction callBack = null)
    {
        isOpen = false;
        canvasGroup.alpha = 1f;
        hideAction += callBack;
    }

    protected virtual void OnClick(string name)
    {

    }

    protected virtual void OnValueChanged(string name,bool value)
    {

    }

    /// <summary>
    /// �ҵ��������Ӧ���͵Ŀؼ������Ҽ����ֵ�
    /// </summary>
    /// <typeparam name="T">�ؼ�������</typeparam>
    private void FindChildsControl<T>() where T:UIBehaviour
    {
        T[] controls=GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            string objName = controls[i].name;
            if (controlDic.ContainsKey(controls[i].name))
            {
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                controlDic.Add(objName, new List<UIBehaviour> { controls[i] });
            }

            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                }); 
            }
            else if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(controls[i].name, value);
                });
            }
        }
    }

    /// <summary>
    /// �õ���Ӧ���壬��Ӧ���͵Ŀؼ�
    /// </summary>
    /// <typeparam name="T">�ؼ�������</typeparam>
    /// <param name="controlName">��Ӧ���������</param>
    /// <returns></returns>
    public T GetControl<T>(string controlName) where T:UIBehaviour
    {
        if(controlDic.ContainsKey(controlName))
        {
            for (int i = 0; i < controlDic[controlName].Count; i++)
            {
                if (controlDic[controlName][i] is T)
                {
                    return controlDic[controlName][i] as T;
                }
            }
        }
        return null;
    }

    protected virtual void Update()
    {
        if (isOpen)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            if (canvasGroup.alpha > 1f)
                canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            if (canvasGroup.alpha <= 0f)
            {
                canvasGroup.alpha = 0;
                hideAction?.Invoke();
            }
                
        }
    }
}
