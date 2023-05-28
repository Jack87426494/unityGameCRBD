using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadBullet : HXW_BulletBase
{
    public static string prefabsPath = "Weapon/EnemyDeadBullet";

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        UpdateBulletData();
    }

    protected override void Update()
    {
        base.Update();
        CheckCollider();
    }
    public void Fire(Transform fireTr, Vector3 targetDir)
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
        bulletDamage = hXW_BaseWeapon.weaponData.bulletDamage * skillChange_Data.bulletDamage_Change * 0.1f;
        moveSpeed = hXW_BaseWeapon.weaponData.bulletMoveSpeed * skillChange_Data.bulletMoveSpeed_Change *0.1f ;
        force = 0;//没有击退效果
        penetrateNum = 2;
    }
    private void OnDisable()
    {
        penetrateNum = 2;
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
