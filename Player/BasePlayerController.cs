using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BasePlayerController : MonoBehaviour,IShootAble
{
    [Header("��Ҽ�ʰ����ķ�Χ")]
    public float randius;

    [Header("��ҵ��ƶ��ٶ�")]
    public float moveSpeed;

    //[Header("���ﳯ���ұ�ʱǹ�ڿ���λ��")]
    //public Transform rightFirePos;
    //[Header("���ﳯ�����ʱǹ�ڵĿ���λ��")]
    //public Transform leftFirePos;
    [Header("����λ��")]
    public Transform weapenPos;

    public Transform bulletUIText;

    public HXW_BaseWeapon hxw_weapon;
    //[Header("�ֵ�Ͳλ��")]
    //public Transform flashLightTransform;

    //�����ֵ�Ͳ��ת���ֶ�
    //������ֵ�Ͳ֮�������
    private Vector2 vector2;
    private float angle;
    private Vector3 mousePos;

    private SpriteRenderer spriteRenderer;
    ////�ӵ�Ԥ����
    //private GameObject bulletObj;

    //���̵��������¼���ֵ
    private float x;
    private float y;

    //ǹеʵ��
    public GameObject weapenObj;
    //������
    public Transform ProtectiveCover;
    ////�����ű�
    //private BaseWeapen baseWeapen;

    //����
    private Animator animator;

    //��ɫ����
    public PlayerData playerData;

    //��Ϸ���
    private GamePanel gamePanel;

    //�Ƿ�����
    public bool isDead;

    private SpriteRenderer sr;

    private static BasePlayerController instance;

    public static BasePlayerController Instance => instance;
   
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            if(instance!=this)
            {
                Destroy(gameObject);
            }
            
        }
        DontDestroyOnLoad(gameObject);
        //weapenObj = Instantiate(Resources.Load<GameObject>("Weapon/HXW_RedDevil"), weapenPos);
        //weapenObj.GetComponent<HXW_BaseWeapon>().bulletUIText = bulletUIText;
        //weapenObj.GetComponent<HXW_BaseWeapon>().shooter = this;
        //hxw_weapon = weapenObj.GetComponent<HXW_BaseWeapon>();
        //baseWeapen = weapenObj.GetComponent<BaseWeapen>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        

        HXW_UIManager.Instance.GetNewPanel("GamePanel");
        sr = GetComponent<SpriteRenderer>();
        
    }


    private void Start()
    {
        playerData = BinaryDataMgr.Instance.GetTable<PlayerDataContainer>().PlayerDatadic[1];
        EventMgr.Instance.AddEventListener<string>("LoadWeapon", LoadWeapon);

        HXW_UIManager.GetInstance().ShowPanelAsync<UITipPanel>(UITipPanel.prefabsPath);
        TalkMgr.Instance.TalkSelf("������񾭹���һ����ɱ����Ϊʲô�����˻��ͷ��ʹ��ʲô���벻�����ˡ�;");
        
        isDead = true;
    }

    private void OnEnable()
    {
        //����
        EventMgr.Instance.AddEventListener<float>("PlayerHit", GetHit);
    }

    private void FixedUpdate()
    {
        #region �˷�λ�ƶ�
        //���ֻ��ס���¼�
        //if (x == 0 && y != 0)
        //    rigidbody2D.velocity = new Vector2(0,y);
        //if (x != 0 && y == 0)
        //    rigidbody2D.velocity = new Vector2(x,0);
        //if (x != 0 && y != 0)
        //{
        //    rigidbody2D.velocity = new Vector2(x/Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)), 
        //        y/Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)));
        //}
        #endregion
    }
    private void Update()
    {
        ////����ʱ��
        //if (beatTime > 0)
        //{
        //    beatTime -= Time.deltaTime;
        //    if (beatTime <= 0)
        //        rigidbody2D.velocity = Vector2.zero;
        //}
        #region ����
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            animator.SetBool("isMove", true);
        else
            animator.SetBool("isMove", false);
        #endregion

        if (isDead)
            return;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        //��ת
        Filp();
        
        #region �˷�λ�ƶ�
        //���ֻ��ס���¼�
        if (x == 0 && y != 0)
            transform.Translate(transform.up * y * Time.deltaTime * moveSpeed);

        //���ֻ��ס���Ҽ�
        if (x != 0 && y == 0)
            transform.Translate(transform.right * x * Time.deltaTime * moveSpeed);

        //�������Ϊ0
        if (x != 0 && y != 0)
        {
            transform.Translate(((transform.right * x + transform.up * y) / Mathf.Sqrt((Mathf.Pow(x, 2) +
                Mathf.Pow(y, 2))))
                * moveSpeed * Time.deltaTime);
        }
        #endregion

        #region ���
        //if (Input.GetMouseButton(0))
        //{
        //    baseWeapen.Attack();
        //}
        #endregion

        #region �ֵ�Ͳ��ǹе����ת
        if(weapenObj!=null)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vector2 = mousePos -
                transform.position;
            angle = Vector2.Angle(Vector2.up, vector2);
            if (mousePos.x > transform.position.x)
            {
                angle = -angle;
            }

            //flashLightTransform.localEulerAngles = Vector3.Lerp(flashLightTransform.localEulerAngles,
            //    new Vector3(0, 0, angle), Time.deltaTime*2);

            //flashLightTransform.localEulerAngles = new Vector3(0, 0, angle);
            weapenObj.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
        
        #endregion

        //��þ���
        getExp();
    }


    protected void StandUp()
    {
        isDead = false;
    }

    private void Filp()//��ת
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(pos.x>transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if(pos.x<transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }
    

    public string GetCurrentBulletPath()
    {
        return "Weapon/PlayerBullet";
    }

    public Vector3 GetFireDirection(Vector3 startPos)
    {
        Vector3 mousScreenPos = Input.mousePosition;
        Vector3 mousWorldPos = Camera.main.ScreenToWorldPoint(mousScreenPos);
        mousWorldPos.z = 0;
        return (mousWorldPos - startPos);
    }


    //��ɫֵ
    private float redValue = 1;
    //�ǲ��ǿ����˱�ɫЭ��
    private bool isOpenColorI;

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="enemyData">��������</param>
    private void GetHit(float damage)
    {
        //��Ᵽ����
        ProtectiveCover=transform.Find("Protective Cover(Clone)");
        if (ProtectiveCover != null && ProtectiveCover.gameObject.activeInHierarchy)
        {
            ProtectionObj protectionScript = ProtectiveCover.GetComponent<ProtectionObj>();
            protectionScript.CloseProtect();
            print("����������");
            return;
        }

        //print("����aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        if (isDead)
            return;

        if (damage > playerData.def)
        {
            playerData.hp -= damage;
            EventMgr.Instance.EventTrigger<float>("UpdateHp", damage);
        }
            

        if(!isOpenColorI)
        {
            redValue = 1f;
            //�ı���ɫ
            StartCoroutine("ChangeColorRed");
        }
        
        if (playerData.hp > 0)
        {
            //animator.SetTrigger("hit");
          
        }
        else
        {

            animator.SetTrigger("dead");
            isDead = true;
            MusicMgr.Instance.PlaySound("Music/Audio/Fail");
            playerData = null;
            Invoke("OpenFailPanel", 0.5f);
            
        }
        
        
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
            redValue -= Time.deltaTime * 5;
            sr.color = new Color(1, redValue, redValue, 1);

            yield return null;
        }
        while (redValue < 1)
        {
            redValue += Time.deltaTime * 5;
            sr.color = new Color(1, redValue, redValue, 1);
            yield return null;
        }
        isOpenColorI = false;
        //print("Э�̽���");
        StopCoroutine("ChangeColorRed");
    }

    private void OpenFailPanel()
    {
        EventMgr.Instance.EventTrigger("ReSetData");
        HXW_UIManager.Instance.HidePanel(GamePanel.prefabsPath);
        HXW_UIManager.GetInstance().ShowPanelAsync<GameOverPanel_Fail>(GameOverPanel_Fail.prefabsPath);
        Time.timeScale = 0f;
        EventMgr.Instance.CanselEventListener<float>("PlayerHit", GetHit);
    }

    private void OnDisable()
    {
        EventMgr.Instance.CanselEventListener<float>("PlayerHit", GetHit);
    }
    

    private void OnDestroy()
    {
        EventMgr.Instance.CanselEventListener<float>("PlayerHit", GetHit);
        EventMgr.Instance.CanselEventListener<string>("LoadWeapon", LoadWeapon);
    }

    //����ʱ��
    //private float beatTime;
    //public void KnockBack(Vector3 direction, IBeatBackAble beatBackAble)
    //{
    //    if (isDead||beatTime>0)
    //        return;

    //    beatTime = beatBackAble.GetBeatBackTime();
    //    rigidbody2D.velocity = direction.normalized * beatBackAble.GetForce();


    //}
    #endregion

    private Collider2D[] hit2ds;
    private void getExp()
    {
        hit2ds = Physics2D.OverlapCircleAll(transform.position, randius, 1 << LayerMask.NameToLayer("Exp"));
        if(hit2ds!=null)
        {
            foreach(Collider2D hit in hit2ds)
            {

                hit.gameObject.transform.position = Vector2.Lerp(hit.gameObject.transform.position, 
                    this.transform.position,Time.deltaTime*10f);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.CompareTag("Exp"))
        {
            
            //����Ѿ�չʾ�����������Ҳ����
            //GamePanel gamPanel = HXW_UIManager.GetInstance().GetNewPanel(GamePanel.prefabsPath) as GamePanel;
            //����ʵ��ֱ��ʹ��������У��Ѿ����ع��Ļ����ֵ䴢�棬��������Ե���Ŀ������ShowMeEveryTime
            HXW_UIManager.GetInstance().ShowPanelAsync<GamePanel>(GamePanel.prefabsPath, PanelLayer.Max,null,(panel)=> 
            {
                panel.nowExp += collision.gameObject.GetComponent<ExpData>().expNum;
                MusicMgr.Instance.PlaySound("Music/Audio/Exp");
                ObjectPoolMgr.Instance.PutObject(collision.gameObject);
            });
        }
    }
    
    private void LoadWeapon(string prefabsPath)
    {
        weapenObj = Instantiate(Resources.Load<GameObject>(prefabsPath), weapenPos);
        hxw_weapon = weapenObj.GetComponent<HXW_BaseWeapon>();
        hxw_weapon.bulletUIText = bulletUIText;
        hxw_weapon.shooter = this;
        
    }

    
}
