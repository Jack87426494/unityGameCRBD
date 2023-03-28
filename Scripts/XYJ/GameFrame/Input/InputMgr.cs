using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : BaseMgr<InputMgr>
{
    //��������
    private Dictionary<string, KeyCode> keyCodeDic = new Dictionary<string, KeyCode>();
    //�Ƿ���������
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
    /// ���ÿ���������
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

        //����ע�����м��ķַ�����

        foreach(string s in keyCodeDic.Keys)
        {
            InputKeyAndDownUp(s, keyCodeDic[s]);
        }
     
        InputMouseAndDownUp(0);
        InputMouseAndDownUp(1);
        InputMouseAndDownUp(2);
    }

    /// <summary>
    /// �ַ����̵İ��¡������С�̧��ĺ���
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
    /// �ַ����İ��¡������С�̧����
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
    /// ���İ�������
    /// </summary>
    /// <param name="eventName">���º�Ҫִ�е��¼�</param>
    /// <param name="keyCode">�����ĸ���</param>
    public void ChangeKeyCode(string eventName, KeyCode keyCode)
    {
        if(keyCodeDic.ContainsKey(eventName))
        keyCodeDic[eventName] = keyCode;
    }
}
