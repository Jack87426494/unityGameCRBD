using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventText : MonoBehaviour
{
    public string textname = "С��";

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            EventMgr.Instance.EventTrigger("Wound", this);
    }
}
