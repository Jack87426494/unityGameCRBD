using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePoolText : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("PutObjectToObjectPool", 2f);
    }
    
    private void PutObjectToObjectPool()
    {
        ObjectPoolMgr.Instance.PutObject(this.gameObject);
    }
}
