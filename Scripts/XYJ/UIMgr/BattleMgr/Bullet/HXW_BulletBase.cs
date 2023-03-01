using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//被击退者
public interface IKnockBackAble
{
    public void KnockBack(Vector3 direction, IBeatBackAble beatBackAble);
}
//击退者
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
    [Header("对目标的击退力量")]
    [SerializeField] protected float force = 1;
    [Header("对目标的击退时间")]
    [SerializeField] protected float beatBackTime = 0.15f;
    [Header("子弹移动速度")]
    [SerializeField] protected float moveSpeed = 5;
    [Header("存活时间")]
    [SerializeField] protected float existTime = 2;
    [Header("子弹伤害")]
    [SerializeField] protected float bulletDamage = 30;
    protected float currentExistTime = 0;
    //子弹碰撞体
    protected BoxCollider2D coll;
    //子弹的刚体
    protected Rigidbody2D rb;
    

    protected Vector3 direction;

    protected bool isHide;
    protected SkillChange_Data skillChange_Data;

    protected RaycastHit2D raycastHit2D;//子弹击中的碰撞体
    protected int penetrateNum;//穿透敌人数量
    protected GameObject enemyObj;//子弹击中的enemy
    protected GameObject weaponObj;//当前武器实例
    protected HXW_BaseWeapon hXW_BaseWeapon;//当前武器脚本
    protected virtual void Awake()
    {
        TryGetComponent<BoxCollider2D>(out coll);
        if (coll == null)
        {
            Debug.Log(gameObject.name + "缺失BoxCollider2D组件");
            return;
        }
        coll.isTrigger = true;
        TryGetComponent<Rigidbody2D>(out rb);
        if (rb == null)
        {
            Debug.Log(gameObject.name + "缺失Rigidbody2D组件");
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
    /// 发射子弹
    /// </summary>
    /// <param name="startPos">发射子弹的开始位置</param>
    /// <param name="targetDir">发射子弹的方向</param>
    /// <param name="time">子弹飞行多久消失</param>
    /// <param name="angle">子弹的偏移角度</param>
    public virtual void Fire(Vector3 startPos, Vector3 targetDir,float time = 2f,float angle=0f)
    {
        if (rb == null)
        {
            Debug.Log(gameObject.name + "缺失Rigidbody2D组件");
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
    /// 造成伤害
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
