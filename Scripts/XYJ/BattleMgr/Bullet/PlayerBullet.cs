using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : HXW_BulletBase
{
    public static string prefabsPath = "Weapon/PlayerBullet";


    private GameObject iceObj;

    protected override void Awake()
    {
        base.Awake();
        
    }
    
    protected override void Start()
    {
        base.Start();
        //bulletDamage = BasePlayerController.Instance.weapenObj.GetComponent<HXW_BaseWeapon>().bulletDamage;
        UpdateBulletData();
        
    }
    
    protected override void Update()
    {
        base.Update();
        CheckCollider();
    }
    

    
    public void Fire(Transform firepos, Vector3 targetDir,Vector3 angleChange)
    {

        if (rb == null)
        {
            Debug.Log(gameObject.name + "ȱʧRigidbody2D���");
            return;
        }
        Vector3 euler = firepos.rotation.eulerAngles;
        transform.position = firepos.position;
        transform.rotation = Quaternion.Euler(euler +angleChange);
        Vector3 dir = transform.right.normalized;
        direction = dir;
        rb.velocity = dir * moveSpeed;
        //transform.right = dir;
        currentExistTime = existTime;
        isHide = false;

    }
    protected override void Damage(IHit iHit)
    {
        
        iHit.GetHit(bulletDamage);
        
    }

    protected override void UpdateBulletData()
    {
        base.UpdateBulletData();
        bulletDamage = hXW_BaseWeapon.weaponData.bulletDamage *
                                                                skillChange_Data.bulletDamage_Change;
        moveSpeed = hXW_BaseWeapon.weaponData.bulletMoveSpeed *
                                                             skillChange_Data.bulletMoveSpeed_Change;
        force = hXW_BaseWeapon.weaponData.bulletForce * skillChange_Data.bulletForce_Change;
        penetrateNum = skillChange_Data.penetrateNum;
        transform.localScale =new Vector3(1,1,1)*skillChange_Data.bulletSize_Change;
    }
    private void OnDisable()
    {
        penetrateNum = skillChange_Data.penetrateNum;
        enemyObj = null;
    }

    private void CheckCollider()//OnTriggerEnter2D��̫���ã�������������ײ
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
            //07_03����Ч��
            if (raycastHit2D.collider.GetComponent<HXW_EnemyBase>().isDead&&skillChange_Data._07_03)
            {
                penetrateNum++;
            }

            //09_01����Ч��
            Skill09_01(raycastHit2D.collider.gameObject);


            if (penetrateNum == 0)//��͸��Ϊ0ʱ��Destroy�ӵ�
            {

                Destroy();

            }

        }
    }

    void Skill09_01(GameObject Enemy)
    {
        if(skillChange_Data._09_01)
        {
            float RandomNum = Random.Range(0, 10);
            if (RandomNum > 2.5)
                return;

            ObjectPoolMgr.Instance.GetObject("Items/IceObj", (obj) =>
            {
                obj.transform.position = Enemy.transform.position;
                obj.transform.parent = Enemy.transform;
                obj.GetComponent<IceObj>().StartFreezing(Enemy);
            });
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
    }
}
