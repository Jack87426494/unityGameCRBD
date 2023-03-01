using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class BlastEnemy : HXW_EnemyBase
{
    public override int GetExcelIndex()
    {
        return 4;
    }

    
    protected override void Attack()
    {
        ////currentAtkIntervalTime = enemyData.attackIntervalTime;
        ////ObjectPoolMgr.Instance.GetObject(EnemyBullet.prefabsPath, (GameObject obj) =>
        ////{
        ////    EnemyBullet bullet = obj.GetComponent<EnemyBullet>();
        ////    if (bullet != null)
        ////        bullet.Fire(transform.position, direction);
        ////});

        //Dead();

        hit2Ds = Physics2D.OverlapCircleAll(transform.position, enemyData.attackRadius-0.5f, 1 << LayerMask.NameToLayer("Player"));
        foreach (Collider2D hit in hit2Ds)
        {
            if (hit != null)
            {
                if (hit.CompareTag("Player"))
                    Dead();
            }

        }

    }
    private Collider2D[] hit2Ds;
    protected override void Dead()
    {
        rb.velocity = Vector2.zero;
        isDead = true;
        animator.SetTrigger("dead");
        
        Invoke("PutPool", 1f);

    }

    protected void Blast()
    {
        MusicMgr.Instance.PlaySound("Music/Audio/Blast");
        hit2Ds = Physics2D.OverlapCircleAll(transform.position, enemyData.attackRadius);
        foreach (Collider2D hit in hit2Ds)
        {
            if (hit != null)
            {
                if (hit.CompareTag("Player"))
                    //角色受伤，同步面板
                    EventMgr.Instance.EventTrigger<float>("PlayerHit", enemyData.atk);
                else if (hit.CompareTag("Enemy"))
                {
                    IHit iHit = hit.gameObject.GetComponent<IHit>();
                    iHit.GetHit(enemyData.atk);
                }
            }

        }
    }


    //#region a星寻路

    ////目前寻路的目标
    //private Vector3 target=Vector3.zero;
    ////寻路的最终目标
    //private Vector3 endTarget=Vector3.zero;


    ////寻路结果
    //private List<AStarNode> aStarList = new List<AStarNode>();

    //protected override Vector3 GetVelocity()
    //{
    //    float tmp_distance = Vector2.Distance(transform.position, target);
    //    //如果到了下一个目标点，遍历下一个点的位置
    //    if (target != Vector3.zero && tmp_distance < 0.01)
    //    {
    //        //移除到达过的位置
    //        aStarList.RemoveAt(0);
    //        target = new Vector2(aStarList[1].x, aStarList[1].y);
    //        //测试位置
    //        print("下一个移动的位置" + target);
    //    }


    //    //如果最终位置变化
    //    if (hit2D.gameObject.transform.position != endTarget || endTarget == Vector3.zero)
    //    {

    //        //更新玩家的位置
    //        //hit2D = Physics2D.OverlapCircle(transform.position, enemyData.findRadius, 1 << LayerMask.NameToLayer("Player"));
    //        //A星寻路,返回一个路径列表
    //        aStarList = AStarMgr.Instance.FindWay(transform.position, hit2D.transform.position);
    //        if (aStarList == null)
    //        {
    //            target = hit2D.gameObject.transform.position;
    //            Debug.Log("aStarList 为空");
    //        }
    //        else
    //        {
    //            //下一个移动的位置
    //            target = new Vector2(aStarList[1].x, aStarList[1].y);
    //            print("下一个移动的位置" + target);
    //            //设置最终移动的位置
    //            endTarget = hit2D.gameObject.transform.position;
    //            print("最终移动的位置" + endTarget);
    //        }
    //    }


    //    direction = (target - transform.position).normalized;


    //    Vector3 targetVelocity = Vector3.zero;

    //    float buffer_Dictance = distance - enemyData.nearDistance;
    //    if (Mathf.Abs(buffer_Dictance) <= enemyData.moveBuffer)
    //    {
    //        targetVelocity = Vector2.zero;
    //    }
    //    else
    //    {
    //        if (distance > enemyData.nearDistance)
    //            targetVelocity = direction * enemyData.moveSpeed;
    //        else if (distance < enemyData.nearDistance)
    //            targetVelocity = -direction * enemyData.moveSpeed;
    //    }

    //    //targetVelocity += extraVelocity;

    //    return targetVelocity;
    //}

    //private void FindWay()
    //{

    //}
    //#endregion


    protected override void ControlDir()
    {
        //base.ControlDir();
        if (hit2D == null || !hit2D.CompareTag("Player"))
            return;
        //if (isKnockBack == true)
        //    return;
        if (currentKnockBackTime > 0)
            return;
        if (rb.velocity.x < 0)
        {
            sr.flipX = false;
        }
        else if (rb.velocity.x > 0)
        {
            sr.flipX = true;
        }
    }

}
