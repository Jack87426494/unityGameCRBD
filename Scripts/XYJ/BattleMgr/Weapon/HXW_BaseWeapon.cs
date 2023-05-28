using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IShootAble
{
    public string GetCurrentBulletPath();
    public Vector3 GetFireDirection(Vector3 starPos);
}
public class HXW_BaseWeapon : MonoBehaviour
{
    [SerializeField]
    [Header("开火的位置")]
    protected Transform firePos;
    [SerializeField]
    [Header("开枪CD")]
    protected float fireCD = 0.3f;//射速，开火间隔
    protected float time;//计时器，用于计算开火间隔时间
    [SerializeField]
    protected float updateBulletNumTime;//换弹时间
    [SerializeField]
    protected float bulletMaxNum;//子弹的最大数量

    public float bulletCurrentNum;//当前的子弹数量

    public float bulletDamage=30;//子弹伤害

    public Transform bulletUIText;//玩家子弹数字UI

    public IShootAble shooter;//获取玩家的shoot调用函数

    public event Action huanDanEvent;//换弹事件

    //是否锁定射击
    public bool lockShoot;

    protected BasePlayerController basePlayerController;
    
    //public PlayerStatus playerStatus;

    public WeaponData weaponData;

    private SkillChange_Data skillChange_Data;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        SkillManager.Instance.updateDataEvent += UpdateWeaponData;//武器更新函数订阅到skillmanager的事件
        skillChange_Data = SkillManager.Instance.skillChange_Data;
        
    }
    protected virtual void Update()
    {
        
    }

    public virtual void UpdateWeaponData()//初始化武器数据
    {
        bulletMaxNum = weaponData.bulletMaxNum+ skillChange_Data.bulletMaxNum_Change;
        updateBulletNumTime = weaponData.updateBulletNumTime* skillChange_Data.updateBulletNumTime_Change;
        fireCD = weaponData.fireCD* skillChange_Data.fireCD_Change;
        bulletDamage = weaponData.bulletDamage * skillChange_Data.bulletDamage_Change;
        
        bulletCurrentNum = bulletMaxNum;
    }
    public virtual void Shoot(IShootAble shooter)
    {
        if (lockShoot)
            return;
        DoShoot(shooter.GetCurrentBulletPath(), shooter.GetFireDirection(firePos.position));
    }

    bool isFirst;

    protected virtual void DoShoot(string bulletPath,Vector3 direction)
    {
        

        if(isFirst)
        {
            for (int i = 0; i < 40; i++)
            {
                ObjectPoolMgr.Instance.GetObject(bulletPath, (obj) =>
                {
                    MusicMgr.Instance.PlaySound("Music/Audio/Fire1");
                });
            }
            isFirst = true;
        }
        
        ObjectPoolMgr.Instance.GetObject(bulletPath, (obj) =>
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Fire1");
            Vector3 angel = skillChange_Data.isSplit ? new Vector3(0, 0, 5) : new Vector3(0, 0, 0);
            PlayerBullet bullet = obj.GetComponent<PlayerBullet>();
            //print(firePos.right);
            bullet.Fire(firePos, direction,angel);
        });

        if(skillChange_Data.isSplit)
        {
            ObjectPoolMgr.Instance.GetObject(bulletPath, (obj) =>
            {
                MusicMgr.Instance.PlaySound("Music/Audio/Fire1");
                PlayerBullet bullet = obj.GetComponent<PlayerBullet>();
                //print(firePos.right);
                bullet.Fire(firePos, direction, new Vector3(0, 0, -5));
            });
        }
        if (skillChange_Data.isBackShoot)
        {
            ObjectPoolMgr.Instance.GetObject(bulletPath, (obj) =>
            {
                MusicMgr.Instance.PlaySound("Music/Audio/Fire1");
                PlayerBullet bullet = obj.GetComponent<PlayerBullet>();
                //print(firePos.right);
                bullet.Fire(firePos, direction, new Vector3(0, 0, 180));
            });
        }
    }

    protected virtual void DoHuanDanEvent()
    {
        if(huanDanEvent!=null)
        {
            huanDanEvent();
        }
    }

    private void OnDisable()
    {
        SkillManager.Instance.updateDataEvent -= UpdateWeaponData;
    }
}

