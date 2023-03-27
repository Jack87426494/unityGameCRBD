using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    //�Ƿ��ڴ�״̬
    private bool isOpen;

    //�������ٶ�
    private float fadeSpeed = 4f;

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
    }

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// ��ʼ�����ؼ�
    /// </summary>
    protected abstract void Init();


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
