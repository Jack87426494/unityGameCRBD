using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteEnemy : HXW_EnemyBase
{
    public override int GetExcelIndex()
    {
        return 2;
    }
    private bool isAttacking;
    protected override void Attack()
    {
        if (isAttacking == true)
            return;
        ForbidMove();
        isAttacking = true;
        DelayAction(0.7f, ()=> { OpenMove(); isAttacking = false; });
        //currentAtkIntervalTime = enemyData.attackIntervalTime;
        ObjectPoolMgr.Instance.GetObject(EnemyBullet.prefabsPath_1, (GameObject obj)=>
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Fire7",this.gameObject);
            EnemyBullet bullet = obj.GetComponent<EnemyBullet>();
            if (bullet != null)
                bullet.Fire(transform.position,direction);
        });
    }
}
