using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResText : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ResLoadMgr.Instance.Load<GameObject>("Sphere");
        }
        if(Input.GetMouseButtonDown(1))
        {
            ResLoadMgr.Instance.LoadAsyn<GameObject>("Sphere", (obj) =>
            {
                Debug.Log(obj.name);
            });
        }
    }
}
