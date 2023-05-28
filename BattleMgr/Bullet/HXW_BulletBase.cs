using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��������
public interface IKnockBackAble
{
    public void KnockBack(Vector3 direction, IBeatBackAble beatBackAble);
}
//������
public interface IBeatBackAble
{
    public void KnockBack(IKnockBackAble target);
    public float GetBeatBackTime();
    public float GetForce();
}

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class HXW_BulletBase : MonoBehaviour, IBeatBackAble,IHiter
{
    [Header("��Ŀ��Ļ�������")]
    [SerializeField] protected float force = 1;
    [Header("��Ŀ��Ļ���ʱ��")]
    [SerializeField] protected float beatBackTime = 0.15f;
    [Header("�ӵ��ƶ��ٶ�")]
    [SerializeField] protected float moveSpeed = 5;
    [Header("���ʱ��")]
    [SerializeField] protected float existTime = 2;
    [Header("�ӵ��˺�")]
    [SerializeField] protected float bulletDamage = 30;
    protected float currentExistTime = 0;
    //�ӵ���ײ��
    protected BoxCollider2D coll;
    //�ӵ��ĸ���
    protected Rigidbody2D rb;
    

    protected Vector3 direction;

    protected bool isHide;
    protected SkillChange_Data skillChange_Data;

    protected RaycastHit2D raycastHit2D;//�ӵ����е���ײ��
    protected int penetrateNum;//��͸��������
    protected GameObject enemyObj;//�ӵ����е�enemy
    protected GameObject weaponObj;//��ǰ����ʵ��
    protected HXW_BaseWeapon hXW_BaseWeapon;//��ǰ�����ű�
    protected virtual void Awake()
    {
        TryGetComponent<BoxCollider2D>(out coll);
        if (coll == null)
        {
            Debug.Log(gameObject.name + "ȱʧBoxCollider2D���");
            return;
        }
        coll.isTrigger = true;
        TryGetComponent<Rigidbody2D>(out rb);
        if (rb == null)
        {
            Debug.Log(gameObject.name + "ȱʧRigidbody2D���");
            return;
        }
        rb.gravityScale = 0;
    }
    protected virtual void Start()
    {
        //bulletDamage = 10;
        skillChange_Data = SkillManager.Instance.skillChange_Data;
        SkillManager.Instance.updateDataEvent += UpdateBulletData;
        //UpdateBulletData();
    }
    protected virtual void Update()
    {
        currentExistTime -= Time.deltaTime;
        if(currentExistTime < 0)
            Destroy();
    }
    public virtual void KnockBack(IKnockBackAble target)
    {
        target.KnockBack(direction, this);
    }

    protected void KnockBackOperate(Collider2D collision)
    {
        IKnockBackAble target = collision.gameObject.GetComponent<IKnockBackAble>();
        if (target != null)
            KnockBack(target);
    }

    protected virtual void Destroy()
    {
        if (isHide == true)
            return;
        isHide = true;
        ObjectPoolMgr.Instance.PutObject(gameObject);
    }
    /// <summary>
    /// �����ӵ�
    /// </summary>
    /// <param name="startPos">�����ӵ��Ŀ�ʼλ��</param>
    /// <param name="targetDir">�����ӵ��ķ���</param>
    /// <param name="time">�ӵ����ж����ʧ</param>
    /// <param name="angle">�ӵ���ƫ�ƽǶ�</param>
    public virtual void Fire(Vector3 startPos, Vector3 targetDir,float time = 2f,float angle=0f)
    {
        if (rb == null)
        {
            Debug.Log(gameObject.name + "ȱʧRigidbody2D���");
            return;
        }


       

        transform.position = startPos;
        Vector3 dir = targetDir.normalized;
        direction = dir;
        rb.velocity = dir * moveSpeed;
        transform.right = dir;
        currentExistTime = existTime;
        isHide = false;
    }

    public float GetBeatBackTime()
    {
        return beatBackTime;
    }

    public float GetForce()
    {
        return this.force;
    }

    /// <summary>
    /// ����˺�
    /// </summary>
    /// <param name="hitTarget"></param>
    public void TakeDamage(Collider2D collision)
    {
        IHit iHit = collision.gameObject.GetComponent<IHit>();
        Damage(iHit);
        
    }

    protected virtual void Damage(IHit iHit)
    {
        
    }

    protected virtual void UpdateBulletData()
    {
        weaponObj = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayerController>().weapenObj;
        hXW_BaseWeapon = weaponObj.GetComponent<HXW_BaseWeapon>();
    }

    protected virtual void OnDestroy()
    {
        SkillManager.Instance.updateDataEvent -= UpdateBulletData;
        ObjectPoolMgr.Instance.ClearObjectPool();
    }

    
}
