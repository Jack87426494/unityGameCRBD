using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerEnemy : HXW_EnemyBase
{

    public override int GetExcelIndex()
    {
        return 3;
    }

    protected override void Attack()
    {
        animator.SetTrigger("attack");
        MusicMgr.Instance.PlaySound("Music/Audio/Summon",this.gameObject);
    }

    public void SummonEnemy()
    {
        for(int i=0;i<3;++i)
        {
            ObjectPoolMgr.Instance.GetObject("Enemyprefab/ShortRangeMonster", (GameObject obj) =>
            {
                obj.transform.position = GetNearPos(transform.position);
            });
        }
        
    }

    private Vector3 GetNearPos(Vector3 pos)
    {
        int edge = Random.Range(0, 2) == 0 ? -1:1;
        int x = edge * Random.Range(2,4);

        edge = Random.Range(0, 2) == 0 ? -1 : 1;
        int y = edge * Random.Range(2, 4);

        Vector3 targetPos = new Vector3(pos.x+x, pos.y + y, pos.z);
        return targetPos;
    }

    protected override void PutPool()
    {
        ObjectPoolMgr.Instance.GetObject("Items/Exp40", (a) =>
        {
            a.transform.position = this.transform.position;
        });
        ObjectPoolMgr.Instance.PutObject(this.gameObject);
    }
}
