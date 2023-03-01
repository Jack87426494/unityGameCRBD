using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BasePlayerController : MonoBehaviour,IShootAble
{
    [Header("玩家捡拾经验的范围")]
    public float randius;

    [Header("玩家的移动速度")]
    public float moveSpeed;

    //[Header("人物朝向右边时枪口开火位置")]
    //public Transform rightFirePos;
    //[Header("人物朝向左边时枪口的开火位置")]
    //public Transform leftFirePos;
    [Header("武器位置")]
    public Transform weapenPos;

    public Transform bulletUIText;

    public HXW_BaseWeapon hxw_weapon;
    //[Header("手电筒位置")]
    //public Transform flashLightTransform;

    //控制手电筒旋转的字段
    //鼠标与手电筒之间的向量
    private Vector2 vector2;
    private float angle;
    private Vector3 mousePos;

    private SpriteRenderer spriteRenderer;
    ////子弹预设体
    //private GameObject bulletObj;

    //键盘的左右上下键的值
    private float x;
    private float y;

    //枪械实例
    public GameObject weapenObj;
    //保护罩
    public Transform ProtectiveCover;
    ////武器脚本
    //private BaseWeapen baseWeapen;

    //动画
    private Animator animator;

    //角色数据
    public PlayerData playerData;

    //游戏面板
    private GamePanel gamePanel;

    //是否死亡
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
        TalkMgr.Instance.TalkSelf("这里好像经过了一场厮杀，我为什么在这里，嘶，头好痛，什么都想不起来了。;");
        
        isDead = true;
    }

    private void OnEnable()
    {
        //受伤
        EventMgr.Instance.AddEventListener<float>("PlayerHit", GetHit);
    }

    private void FixedUpdate()
    {
        #region 八方位移动
        //如果只按住上下键
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
        ////击退时间
        //if (beatTime > 0)
        //{
        //    beatTime -= Time.deltaTime;
        //    if (beatTime <= 0)
        //        rigidbody2D.velocity = Vector2.zero;
        //}
        #region 动画
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            animator.SetBool("isMove", true);
        else
            animator.SetBool("isMove", false);
        #endregion

        if (isDead)
            return;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        //翻转
        Filp();
        
        #region 八方位移动
        //如果只按住上下键
        if (x == 0 && y != 0)
            transform.Translate(transform.up * y * Time.deltaTime * moveSpeed);

        //如果只按住左右键
        if (x != 0 && y == 0)
            transform.Translate(transform.right * x * Time.deltaTime * moveSpeed);

        //如果都不为0
        if (x != 0 && y != 0)
        {
            transform.Translate(((transform.right * x + transform.up * y) / Mathf.Sqrt((Mathf.Pow(x, 2) +
                Mathf.Pow(y, 2))))
                * moveSpeed * Time.deltaTime);
        }
        #endregion

        #region 射击
        //if (Input.GetMouseButton(0))
        //{
        //    baseWeapen.Attack();
        //}
        #endregion

        #region 手电筒和枪械的旋转
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

        //获得经验
        getExp();
    }


    protected void StandUp()
    {
        isDead = false;
    }

    private void Filp()//翻转
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


    //颜色值
    private float redValue = 1;
    //是不是开启了变色协程
    private bool isOpenColorI;

    #region 受伤
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="enemyData">怪物数据</param>
    private void GetHit(float damage)
    {
        //检测保护罩
        ProtectiveCover=transform.Find("Protective Cover(Clone)");
        if (ProtectiveCover != null && ProtectiveCover.gameObject.activeInHierarchy)
        {
            ProtectionObj protectionScript = ProtectiveCover.GetComponent<ProtectionObj>();
            protectionScript.CloseProtect();
            print("保护罩破裂");
            return;
        }

        //print("受伤aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

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
            //改变颜色
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
    /// 改变为红色的协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeColorRed()
    {
        isOpenColorI = true;
        //print("协程开始");
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
        //print("协程结束");
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

    //击退时间
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
            
            //如果已经展示过，就用这个也可以
            //GamePanel gamPanel = HXW_UIManager.GetInstance().GetNewPanel(GamePanel.prefabsPath) as GamePanel;
            //但其实都直接使用这个就行，已经加载过的会有字典储存，这个还可以调用目标面板的ShowMeEveryTime
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
