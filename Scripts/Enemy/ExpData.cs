using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpData:MonoBehaviour
{
    public float expNum;

    [SerializeField]
    private float existTime = 60;

    private void OnEnable()
    {
        existTime = 60;
    }
    private void Update()
    {
        existTime -= Time.deltaTime;
        if(existTime<=0)
        {
            ObjectPoolMgr.Instance.PutObject(gameObject);
        }
    }
}
