using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadFire : MonoBehaviour
{
    public static string prefabsPath = "Items/EnemyDeadFire";
    private Vector3 fireDir;
    
    public void StartFire()
    {
        for (int i = 0; i < 3; i++)
        {
            fireDir = new Vector3(Random.Range(-10,10), Random.Range(-10,10), 0);
            ObjectPoolMgr.Instance.GetObject(EnemyDeadBullet.prefabsPath, (obj) =>
            {
                EnemyDeadBullet bullet = obj.GetComponent<EnemyDeadBullet>();

                bullet.Fire(transform, fireDir);
            });

        }
        
    }
    
}
