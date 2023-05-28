using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceObj : MonoBehaviour
{
    private GameObject EnemyObj;
    
    
    public void StartFreezing(GameObject Enemy)
    {
        
        
        Debug.Log("����");
        EnemyObj = Enemy;
        EnemyObj.GetComponent<Rigidbody2D>().Sleep();
        EnemyObj.GetComponent<Collider2D>().enabled=false;
        EnemyObj.GetComponent<HXW_EnemyBase>().enabled=false;
        EnemyObj.GetComponent<HXW_EnemyBase>().Skill09_02();//�ж��Ƿ�ִ��09_02����
        Invoke("EndFreezing", 2);
    }

    private void EndFreezing()
    {
        EnemyObj.GetComponent<Rigidbody2D>().WakeUp();
        EnemyObj.GetComponent<HXW_EnemyBase>().enabled = true;
        EnemyObj.GetComponent<Collider2D>().enabled = true;
        ObjectPoolMgr.Instance.PutObject(gameObject);
    }
    
}
