using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolText : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ObjectPoolMgr.Instance.GetObject("Cube", (obj) =>
            {
                obj.transform.localScale = new Vector3(2, 2, 2);
            });
            //ObjectPoolMgr.Instance.GetObject("Cube").transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        }    

    }
}
