using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceObj : MonoBehaviour
{
    private GameObject EnemyObj;
    
    
    public void StartFreezing(GameObject Enemy)
    {
        
        
        Debug.Log("冰冻");
        EnemyObj = Enemy;
        EnemyObj.GetComponent<Rigidbody2D>().Sleep();
        EnemyObj.GetComponent<Collider2D>().enabled=false;
        EnemyObj.GetComponent<HXW_EnemyBase>().enabled=false;
        EnemyObj.GetComponent<HXW_EnemyBase>().Skill09_02();//判断是否执行09_02技能
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
