using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class HXW_EnemyBase : MonoBehaviour, IKnockBackAble, IHit, IHiter
{
    //[Header("Ѳ�߷�Χ")]
    //[SerializeField]
    //protected float findRadius = 50;
    //[Header("��С��������")]
    //[SerializeField]
    //protected float nearDistance = 1;
    //[Header("��С���뻺����")]
    //protected float moveBuffer = 1;
    //[Header("�ƶ��ٶ�")]
    //[SerializeField]
    //protected float moveSpeed = 4;
    //[Header("������ⷶΧ")]
    //[SerializeField]
    //protected float attackRadius = 10;
    //[Header("������϶")]
    //[SerializeField]
    //protected float attackIntervalTime = 1f;

    public static string normalClipName = "Idle";
    [SerializeField]
    protected int indexOfExcel;
    protected EnemyData enemyData;

    protected BoxCollider2D coll;//������ײ��
    protected Rigidbody2D rb;//����ĸ���
    protected SpriteRenderer sr;//ͼƬ
    protected Collider2D hit2D;//��⵽�������ײ��
    protected Vector3 direction;//������ҵ�ֱ�߷���
    protected float distance;//����ҵľ���

    private Vector3 extraVelocity;
    protected float currentAtkIntervalTime;//��ǰ����ʱ��"
    protected float currentKnockBackTime;//��ǰ����ʱ��

    protected bool isForbidMove = false;
    //�ǲ�������
    public bool isDead;

    [SerializeField]
    //Ѫ��
    protected float hp;

    //������
    protected Animator animator;

    //��ü��ܹ�������
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

        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Debug.Log(gameObject.name + "ȱʧSpriteRenderer���");
            return;
        }
        gameObject.layer = LayerMask.NameToLayer("Enemy");


    }

    protected virtual void OnEnable()
    {
        //Ҫ�Ŷ���أ������ʱ�����¸����Ѫ����
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
            Debug.Log("����ʧ��");
            return;
        }
        if (!container.EnemyDatadic.ContainsKey(GetExcelIndex()))
        {
            Debug.Log("������������excel�������ݣ��Ƿ��Ӧ");
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

    //��ȡһЩ��Ϣ����ֵ����������ʹ��
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
    //�ƶ�����
    protected virtual void MoveController()
    {
        if (hit2D == null || !hit2D.CompareTag("Player"))
            return;
        if (isForbidMove == true)
            return;
        NormalMove();
    }
    //��ͨ�ƶ�
    protected void NormalMove()
    {
        if (currentKnockBackTime > 0)
            return;
        rb.velocity = GetVelocity();
        //print("rb.velocity:" + rb.velocity);
    }
    //��������
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
                    SetDir(direction);//�Թ�������Ϊ���巽��
                    Attack();
                }
            }
        }
    }
    //�����ľ���ʵ�֣���������д
    protected virtual void Attack()
    {

    }
    //��ȡ��ǰ֡Ӧ�õ�����ƶ��ٶ�
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
    //�Զ����ƹ��﷽�򣬵�������С���뻺������Ч
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

    //�ֶ����ƹ��﷽��
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
    //����
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

    //��ɫֵ
    protected float redValue = 1;
    //�ǲ��ǿ����˱�ɫЭ��
    protected bool isOpenColorI;

    /// <summary>
    /// ����
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

            //�ж����05_03�����Ƿ�����
            if (skillChange_Data._05_03 && (hp / enemyData.hp) < 0.2f)
            {
                hp = 0;
                Debug.Log("��ɱ");
            }
        }
        if (!isOpenColorI)
        {
            redValue = 1f;

            //�ı���ɫ
            StartCoroutine("ChangeColorRed");
        }

        if (hp > 0)
        {
            //animator.SetTrigger("hit");

            OnGetHit();
        }
        else
        {
            //�ж����07_02�����Ƿ�����
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
    /// �ı�Ϊ��ɫ��Э��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeColorRed()
    {
        isOpenColorI = true;
        //print("Э�̿�ʼ");
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
        //print("Э�̽���");
        StopCoroutine("ChangeColorRed");
    }


    protected virtual void Dead()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("dead");
        rb.velocity = Vector2.zero;
        Invoke("PutPool", 0.3f);
        deadEvent?.Invoke(gameObject);//���������¼�

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
        //    //��ɫ���ˣ�ͬ�����
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
            Debug.Log("�����˺���");
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

