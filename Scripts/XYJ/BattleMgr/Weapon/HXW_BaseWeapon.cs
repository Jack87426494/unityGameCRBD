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
    [Header("�����λ��")]
    protected Transform firePos;
    [SerializeField]
    [Header("��ǹCD")]
    protected float fireCD = 0.3f;//���٣�������
    protected float time;//��ʱ�������ڼ��㿪����ʱ��
    [SerializeField]
    protected float updateBulletNumTime;//����ʱ��
    [SerializeField]
    protected float bulletMaxNum;//�ӵ����������

    public float bulletCurrentNum;//��ǰ���ӵ�����

    public float bulletDamage=30;//�ӵ��˺�

    public Transform bulletUIText;//����ӵ�����UI

    public IShootAble shooter;//��ȡ��ҵ�shoot���ú���

    public event Action huanDanEvent;//�����¼�

    //�Ƿ��������
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
        SkillManager.Instance.updateDataEvent += UpdateWeaponData;//�������º������ĵ�skillmanager���¼�
        skillChange_Data = SkillManager.Instance.skillChange_Data;
        
    }
    protected virtual void Update()
    {
        
    }

    public virtual void UpdateWeaponData()//��ʼ����������
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

