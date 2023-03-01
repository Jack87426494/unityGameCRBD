using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class HXW_EnemyBase : MonoBehaviour, IKnockBackAble, IHit, IHiter
{
    //[Header("巡逻范围")]
    //[SerializeField]
    //protected float findRadius = 50;
    //[Header("最小靠近距离")]
    //[SerializeField]
    //protected float nearDistance = 1;
    //[Header("最小距离缓冲区")]
    //protected float moveBuffer = 1;
    //[Header("移动速度")]
    //[SerializeField]
    //protected float moveSpeed = 4;
    //[Header("攻击检测范围")]
    //[SerializeField]
    //protected float attackRadius = 10;
    //[Header("攻击间隙")]
    //[SerializeField]
    //protected float attackIntervalTime = 1f;

    public static string normalClipName = "Idle";
    [SerializeField]
    protected int indexOfExcel;
    protected EnemyData enemyData;

    protected BoxCollider2D coll;//怪物碰撞体
    protected Rigidbody2D rb;//怪物的刚体
    protected SpriteRenderer sr;//图片
    protected Collider2D hit2D;//检测到的玩家碰撞体
    protected Vector3 direction;//朝向玩家的直线方向
    protected float distance;//与玩家的距离

    private Vector3 extraVelocity;
    protected float currentAtkIntervalTime;//当前攻击时间"
    protected float currentKnockBackTime;//当前击退时间

    protected bool isForbidMove = false;
    //是不是死了
    public bool isDead;

    [SerializeField]
    //血量
    protected float hp;

    //动画器
    protected Animator animator;

    //获得技能管理数据
    protected SkillChange_Data skillChange_Data;

    //private bool isKnockBack;
    public event Action<GameObject> deadEvent;
    private void Awake()
    {

        animator = GetComponentInChildren<Animator>();
        //Debug.Log(gameObject.name);
      
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

        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Debug.Log(gameObject.name + "缺失SpriteRenderer组件");
            return;
        }
        gameObject.layer = LayerMask.NameToLayer("Enemy");


    }

    protected virtual void OnEnable()
    {
        //要放对象池，激活的时候重新复活把血回满
        isDead = false;
        if (enemyData!=null)
        hp = enemyData.hp;
        //currentAtkIntervalTime = enemyData.attackIntervalTime;
    }
    private void OnDisable()
    {

    }

    protected virtual void Start()
    {
        EnemyDataContainer container = BinaryDataMgr.Instance.GetTable<EnemyDataContainer>();
        if (container == null)
        {
            Debug.Log("加载失败");
            return;
        }
        if (!container.EnemyDatadic.ContainsKey(GetExcelIndex()))
        {
            Debug.Log("索引错误，请检查excel表中数据，是否对应");
            return;
        }
        //Debug.Log(container);
        //Debug.Log(container.EnemyDatadic.Count);
        //Debug.Log(gameObject.name + ":" + GetExcelIndex());
        enemyData = container.EnemyDatadic[GetExcelIndex()];
        currentAtkIntervalTime = enemyData.attackIntervalTime;
        hp = enemyData.hp;

        skillChange_Data = SkillManager.Instance.skillChange_Data;
        //print(sr.color);
    }
    protected virtual void ForbidMove()
    {
        isForbidMove = true;
        rb.velocity = Vector2.zero;
    }
    protected virtual void OpenMove()
    {
        isForbidMove = false;
    }
    protected virtual void Update()
    {
        if (isDead)
            return;
        InitialCheckInfo();
        MoveController();

        ControlDir();
        AttackHandler();
    }

    public abstract int GetExcelIndex();

    //获取一些信息并赋值，方便子类使用
    protected virtual void InitialCheckInfo()
    {

        hit2D = Physics2D.OverlapCircle(transform.position, enemyData.findRadius, 1 << LayerMask.NameToLayer("Player"));
        if (hit2D != null && hit2D.CompareTag("Player"))
        {
            distance = Vector3.Distance(hit2D.gameObject.transform.position, transform.position);
            direction = (hit2D.gameObject.transform.position - transform.position).normalized;
        }
        currentKnockBackTime -= Time.deltaTime;
    }
    //移动控制
    protected virtual void MoveController()
    {
        if (hit2D == null || !hit2D.CompareTag("Player"))
            return;
        if (isForbidMove == true)
            return;
        NormalMove();
    }
    //普通移动
    protected void NormalMove()
    {
        if (currentKnockBackTime > 0)
            return;
        rb.velocity = GetVelocity();
        //print("rb.velocity:" + rb.velocity);
    }
    //攻击管理
    protected virtual void AttackHandler()
    {
        currentAtkIntervalTime -= Time.deltaTime;
        if (hit2D != null && hit2D.CompareTag("Player"))
        {
            if (distance <= enemyData.attackRadius)
            {
                if (currentAtkIntervalTime <= 0)
                {
                    currentAtkIntervalTime = enemyData.attackIntervalTime;
                    SetDir(direction);//以攻击方向为物体方向
                    Attack();
                }
            }
        }
    }
    //攻击的具体实现，由子类重写
    protected virtual void Attack()
    {

    }
    //获取当前帧应该到达的移动速度
    protected virtual Vector3 GetVelocity()
    {
        Vector3 targetVelocity = Vector3.zero;
        float buffer_Dictance = distance - enemyData.nearDistance;
        if (Mathf.Abs(buffer_Dictance) <= enemyData.moveBuffer)
        {
            targetVelocity = Vector2.zero;
        }
        else
        {
            if (distance - enemyData.nearDistance > 0)
                targetVelocity = direction * enemyData.moveSpeed;
            else if (distance - enemyData.nearDistance < 0)
                targetVelocity = -direction * enemyData.moveSpeed;
        }
        //targetVelocity += extraVelocity;
        return targetVelocity;
    }
    //自动控制怪物方向，但是在最小距离缓冲区无效
    protected virtual void ControlDir()
    {
        if (hit2D == null || !hit2D.CompareTag("Player"))
            return;

        //if (isKnockBack == true)
        //    return;
        if (currentKnockBackTime > 0)
            return;

        bool targetFlipX = false;
        if (rb.velocity.x > 0)
        {
            targetFlipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            targetFlipX = true;
        }
        if (IsOriginalRight() == false)
            targetFlipX = !targetFlipX;
        sr.flipX = targetFlipX;
    }

    //手动控制怪物方向
    protected virtual void SetDir(Vector3 targetDir)
    {
        bool targetFlipX = false;
        if (targetDir.x > 0)
        {
            targetFlipX = false;
        }
        else if (targetDir.x < 0)
        {
            targetFlipX = true;
        }
        if (IsOriginalRight() == false)
            targetFlipX = !targetFlipX;
        sr.flipX = targetFlipX;
    }
    public virtual bool IsOriginalRight()
    {
        return true;
    }
    //击退
    public void KnockBack(Vector3 direction, IBeatBackAble beatBackAble)
    {
        if (isDead)
            return;
        currentKnockBackTime = beatBackAble.GetBeatBackTime();
        rb.velocity = direction.normalized * beatBackAble.GetForce();
    }
    private void AddExtraVelocity(Vector3 velocity)
    {
        extraVelocity += velocity;
    }

    private void RemoveExtraVelocity(Vector3 velocity)
    {
        extraVelocity -= velocity;
    }

    //颜色值
    protected float redValue = 1;
    //是不是开启了变色协程
    protected bool isOpenColorI;

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public virtual void GetHit(float damage)
    {
        if (isDead == true) return;

        MusicMgr.Instance.PlaySound("Music/Audio/Hit", this.gameObject);
        //Debug.Log("EnemyGetHit" + damage);

        if (damage > enemyData.def)
        {
            hp -= (damage - enemyData.def);

            //判断玩家05_03技能是否启用
            if (skillChange_Data._05_03 && (hp / enemyData.hp) < 0.2f)
            {
                hp = 0;
                Debug.Log("秒杀");
            }
        }
        if (!isOpenColorI)
        {
            redValue = 1f;

            //改变颜色
            StartCoroutine("ChangeColorRed");
        }

        if (hp > 0)
        {
            //animator.SetTrigger("hit");

            OnGetHit();
        }
        else
        {
            //判断玩家07_02技能是否启用
            if (skillChange_Data._07_02)
            {
                ObjectPoolMgr.Instance.GetObject(EnemyDeadFire.prefabsPath, (obj) =>
                {
                    obj.transform.position = transform.position;
                    obj.GetComponent<EnemyDeadFire>().StartFire();
                    ObjectPoolMgr.Instance.PutObject(obj);
                });

            }
            Dead();

        }

    }

    protected virtual void OnGetHit()
    {

    }

    /// <summary>
    /// 改变为红色的协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeColorRed()
    {
        isOpenColorI = true;
        //print("协程开始");
        while (redValue > 0)
        {
            redValue -= Time.deltaTime * 6;
            sr.color = new Color(1, redValue, redValue, 1);

            yield return null;
        }
        while (redValue < 1)
        {
            redValue += Time.deltaTime * 6;
            sr.color = new Color(1, redValue, redValue, 1);
            yield return null;
        }

        isOpenColorI = false;
        //print("协程结束");
        StopCoroutine("ChangeColorRed");
    }


    protected virtual void Dead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("dead");
        rb.velocity = Vector2.zero;
        Invoke("PutPool", 0.3f);
        deadEvent?.Invoke(gameObject);//启动死亡事件

    }

    protected virtual void PutPool()
    {
        ObjectPoolMgr.Instance.GetObject("Items/Exp5", (a) =>
        {
            a.transform.position = this.transform.position;
        });
        ObjectPoolMgr.Instance.PutObject(this.gameObject);

    }

    //public void TakeDamage(Collider2D collision)
    //{
    //    IHit iHit=collision.GetComponent<IHit>();
    //    iHit.GetHit(enemyData.atk);
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.CompareTag("Player"))
        //{
        //    //角色受伤，同步面板
        //    EventMgr.Instance.EventTrigger<float>("PlayerHit", enemyData.atk);
        //    //beatOperator(collision);
        //}
    }

    public void TakeDamage(Collider2D collision)
    {

    }

    //public void KnockBack(IKnockBackAble target)
    //{
    //    //target.KnockBack(direction, this);
    //}

    //protected virtual void beatOperator(Collider2D collider2D)
    //{
    //    IKnockBackAble knockBackAble = collider2D.gameObject.GetComponent<IKnockBackAble>();
    //    KnockBack(knockBackAble);
    //}

    public float GetBeatBackTime()
    {
        return enemyData.beatBackTime;
    }

    public float GetForce()
    {
        return enemyData.beatBackForce;
    }

    public void Skill09_02()
    {

        if (skillChange_Data._09_02)
        {
            Debug.Log("冰冻伤害！");
            GetHit(enemyData.hp * 0.15f);
        }
    }


    protected void DelayAction(float time, Action action)
    {
        if (action == null)
            return;
        StartCoroutine(Delay(time, action));
    }

    protected IEnumerator Delay(float time, Action action)
    {
        yield return new WaitForSeconds(time);
            action?.Invoke();
    }

    //protected virtual Vector3 GetRandomPosition()
    //{
    //    int edge = Random.Range(0, 2) == 0 ? -1 : 1;
    //    int x = edge * Random.Range(2, 4);

    //    edge = Random.Range(0, 2) == 0 ? -1 : 1;
    //    int y = edge * Random.Range(2, 4);
    //    Vector3 targetPos = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
    //    return targetPos;
    //}
}

