using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : BaseMgr<InputMgr>
{
    //键的容器
    private Dictionary<string, KeyCode> keyCodeDic = new Dictionary<string, KeyCode>();
    //是否开启输入检测
    private bool isOpen;
    
   public InputMgr()
   {
        MonoMgr.Instance.AddUpdateListener(InputUpdate);
        keyCodeDic.Add("MoveUp", KeyCode.W);
        keyCodeDic.Add("MoveLeft", KeyCode.A);
        keyCodeDic.Add("MoveDown", KeyCode.S);
        keyCodeDic.Add("MoveRight", KeyCode.D);
    }
    /// <summary>
    /// 设置开启输入检测
    /// </summary>
    /// <param name="isOpen"></param>
    public void IsOpen(bool isOpen)
    {
        this.isOpen = isOpen;
    }


    private void InputUpdate()
    {
        if (!isOpen)
            return;

        //遍历注册所有键的分发函数

        foreach(string s in keyCodeDic.Keys)
        {
            InputKeyAndDownUp(s, keyCodeDic[s]);
        }
     
        InputMouseAndDownUp(0);
        InputMouseAndDownUp(1);
        InputMouseAndDownUp(2);
    }

    /// <summary>
    /// 分发键盘的按下、按键中、抬起的函数
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="keyCode"></param>
    private void InputKeyAndDownUp(string eventName,KeyCode keyCode)
    {
        if(Input.GetKeyDown(keyCode))
        {
            EventMgr.Instance.EventTrigger(eventName+"KeyDown", keyCode);
        }
        if(Input.GetKey(keyCode))
        {
            EventMgr.Instance.EventTrigger(eventName + "Key", keyCode);
        }

        if(Input.GetKeyUp(keyCode))
        {
            EventMgr.Instance.EventTrigger(eventName + "KeyUp", keyCode);
        }
    }

    /// <summary>
    /// 分发鼠标的按下、按键中、抬起函数
    /// </summary>
    /// <param name="button"></param>
    private void InputMouseAndDownUp(int button)
    {
        if(Input.GetMouseButtonDown(button))
        {
            EventMgr.Instance.EventTrigger("MouseDown", button);
        }
        if (Input.GetMouseButton(button))
        {
            EventMgr.Instance.EventTrigger("Mouse", button);
        }
        if (Input.GetMouseButtonUp(button))
        {
            EventMgr.Instance.EventTrigger("MouseUp", button);
        }
    }

    /// <summary>
    /// 更改按键设置
    /// </summary>
    /// <param name="eventName">按下后要执行的事件</param>
    /// <param name="keyCode">按下哪个键</param>
    public void ChangeKeyCode(string eventName, KeyCode keyCode)
    {
        if(keyCodeDic.ContainsKey(eventName))
        keyCodeDic[eventName] = keyCode;
    }
}
