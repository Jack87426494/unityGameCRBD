using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBullet : HXW_BulletBase
{
    public static string prefabsPath = "Weapon/GhostBullet";

    private Skill_Data skill_Data;//该幽灵技能数据
    
    protected override void Start()
    {
        base.Start();
        
        skill_Data = GameDataMgr.Instance.GetSkill_Data("Ghost");
        UpdateBulletData();
    }

    protected override void Update()
    {
        base.Update();
        CheckCollider();
    }
    
    

    public void Fire(Transform fireTr,Vector3 targetDir)
    {
        transform.position = fireTr.position;
        direction = targetDir.normalized;
        rb.velocity = direction * moveSpeed;
        currentExistTime = existTime;
        isHide = false;
    }

    protected override void UpdateBulletData()
    {
        base.UpdateBulletData();
        bulletDamage = skill_Data.attackDamage * skillChange_Data.ghostBulletDamage_Change;
        moveSpeed = skill_Data.bulletSpeed;
        force = skill_Data.bulletForce;
        penetrateNum = 1;
        if (skillChange_Data.ghostSkill3)
        {
            bulletDamage *= skillChange_Data.bulletDamage_Change;
            moveSpeed *= skillChange_Data.bulletMoveSpeed_Change;
            penetrateNum = skillChange_Data.penetrateNum;
            force *= skillChange_Data.bulletForce_Change;
        }
    }

    private void OnDisable()
    {
        penetrateNum = skillChange_Data.penetrateNum;
        enemyObj = null;
    }

    private void CheckCollider()//OnTriggerEnter2D不太好用，用这个来检测碰撞
    {

        Ray ray = new Ray(transform.position, transform.forward);
        raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, 0.5f, 1 << LayerMask.NameToLayer("Enemy"));


        if (raycastHit2D.collider != null)
        {
            if (enemyObj == raycastHit2D.collider.gameObject)
                return;

            enemyObj = raycastHit2D.collider.gameObject;
            KnockBackOperate(raycastHit2D.collider);
            TakeDamage(raycastHit2D.collider);
            penetrateNum--;
            if (penetrateNum == 0)//穿透数为0时，Destroy子弹
            {

                Destroy();

            }

        }


    }

    protected override void Damage(IHit iHit)
    {

        iHit.GetHit(bulletDamage);

    }
}
