using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : HXW_BulletBase
{
    public static string prefabsPath = "Weapon/EnemyBullet";
    public static string prefabsPath_1 = "Weapon/EnemyBullet_1";
    public static string prefabsPath_Boss= "Weapon/EnemyBullet_Boss";

    //public override void KnockBack(IKnockBackAble target)
    //{
    //    //不击退
    //    //base.KnockBack(target);
    //}

    //protected void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //if (collision.CompareTag("Protection"))//检测是否有保护罩
    //    //{
    //    //    collision.GetComponent<ProtectionObj>().CloseProtect();
    //    //    ObjectPoolMgr.Instance.PutObject(this.gameObject);
    //    //    return;
    //    //}

    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        KnockBackOperate(collision);
    //        //同步面板
    //        EventMgr.Instance.EventTrigger<float>("PlayerHit", Random.Range(3,6));
    //        ObjectPoolMgr.Instance.PutObject(this.gameObject);
    //    }
    //}
    protected override void Update()
    {
        base.Update();
        CheckCollider();
    }

    public override void Fire(Vector3 startPos, Vector3 targetDir, float time = 2, float angle = 0)
    {
        base.Fire(startPos, targetDir, time, angle);
    }

    public void FireAround(Vector3 startPos, Vector3 targetDir, float time = 2, float angle = 0)
    {
        if (rb == null)
        {
            Debug.Log(gameObject.name + "缺失Rigidbody2D组件");
            return;
        }
        transform.position = startPos;
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector2)targetDir.normalized;
        direction = dir;

        transform.right = dir;
        rb.velocity = dir * moveSpeed;
        currentExistTime = existTime;
        isHide = false;
    }


    private void CheckCollider()//OnTriggerEnter2D不太好用，用这个来检测碰撞
    {

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, 0.5f,
            1 << LayerMask.NameToLayer("Player"));


        if (raycastHit2D.collider != null)
        {
            
            KnockBackOperate(raycastHit2D.collider);
            EventMgr.Instance.EventTrigger<float>("PlayerHit", Random.Range(3, 6));
            ObjectPoolMgr.Instance.PutObject(this.gameObject);
        }
    }
}
