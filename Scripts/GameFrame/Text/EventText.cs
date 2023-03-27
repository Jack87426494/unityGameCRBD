using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventText : MonoBehaviour
{
    public string textname = "Ð¡ºì";

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            EventMgr.Instance.EventTrigger("Wound", this);
    }
}
