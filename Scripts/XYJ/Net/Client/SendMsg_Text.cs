using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendMsg_Text : MonoBehaviour
{
    public Button button;
    public InputField inputField;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if(inputField.text!="")
            {
                UdpMgr.Instance.Send(inputField.text);
                inputField.text = "";
            }

        });
    }
}
