using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    //是否在打开状态
    private bool isOpen;

    //显隐的速度
    private float fadeSpeed = 4f;

    //隐藏面板时执行的委托
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
    /// 初始化面板控件
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
