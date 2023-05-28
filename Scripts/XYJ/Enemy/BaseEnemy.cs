using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowMode
{
    //匀速追踪
    UniformLerp,
    //先快后慢
    Lerp,
    //A星寻路
    AFindWay,
    //恒定速度追踪
    Uniform,
}

public enum RepellMode
{
    //匀速击退
    UniformLerp,
    //先快后慢
    Lerp,
}


public  class BaseEnemy : MonoBehaviour
{
    [Header("巡逻范围")]
    public float findRadius;
    [Header("最小靠近距离")]
    public float nearDistance;

    [Header("匀速追踪模式移动速度")]
    public float uniformMoveSpeed;

    [Header("先快后慢追踪模式移动速度")]
    public float lerpMoveSpeed;

    [Header("追踪模式")]
    public FollowMode followMode;


    [Header("击退模式")]
    public RepellMode repellMode;

    [Header("匀速击退模式速度")]
    public float uniformRepellSpeed;

    [Header("先快后慢击退模式速度")]
    public float lerpRepellSpeed;

    [Header("击退的距离")]
    public float repelledDis;

    //击退的位移
    private Vector3 repellVector3;
  
    //是够开启击退协程
    private bool isRepelledIE;
    //击退后的位置
    private Vector3 repelledPos;

    //是否开启颜色变化协程
    private bool isColorChangeIE;
    //目前颜色的值
    private float colorValue;

    //玩家图层
    private LayerMask playerMask;

    //用来匀速追击玩家的变量
    private float time;
    private Vector3 beginVector3;
    private Vector3 endVector3;

    //用来击退自身的变量
    private float repealTime;
    private Vector3 repealBeginVector3;
    private Vector3 repealEndVector3;

    protected Collider2D hit2D;
    protected float distance;
    protected SpriteRenderer sr;

    //private Rigidbody2D rigidbody2D;
   
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        //    rigidbody2D = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {
        //在没有被击退的情况下巡逻
        if (!isRepelledIE)
        {
            FollowPlayer();
        }
        else
        {
            RepelledUpdate();
        }
    }
    //巡逻
    protected virtual void FollowPlayer()
    {

        hit2D = Physics2D.OverlapCircle(transform.position, findRadius, 1 << LayerMask.NameToLayer("Player"));

        if (hit2D != null && hit2D.CompareTag("Player"))
        {
            distance = Vector3.Distance(hit2D.gameObject.transform.position,transform.position);
            switch (followMode)
            {
                case FollowMode.UniformLerp:
                    //匀速追踪
                    if (endVector3 != hit2D.transform.position)
                    {
                        beginVector3 = transform.position;
                        time = 0;
                        endVector3 = hit2D.transform.position;
                    }
                    time += Time.deltaTime;
                    transform.position = Vector3.Lerp(beginVector3, endVector3, time * uniformMoveSpeed);
                    break;
                case FollowMode.Lerp:
                    transform.position = Vector2.Lerp(transform.position, hit2D.transform.position,
                        Time.deltaTime * lerpMoveSpeed);
                    break;
                case FollowMode.AFindWay:
                   
                    break;
                case FollowMode.Uniform:
                    //胡小伟追踪


                    break;
            }

        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet"))
            {
                //改变颜色
                ColorChange();

            ////得到击退的位移
            //repellVector3 = (transform.position - collision.gameObject.transform.position).normalized * repelledDis;
            ////得到击退后的位置
            //repelledPos = transform.position + repellVector3;
            //被击退
            isRepelledIE = true;

            ////被击退
            //Repelled(collision.gameObject.transform);

        }
        }


        /// <summary>
        /// 改变颜色
        /// </summary>
        public void ColorChange()
        {
            if (!isColorChangeIE)
            {
                StartCoroutine("ColorChangeIE");
            }
            else
            {
                StopCoroutine("ColorChangeIE");
                StartCoroutine("ColorChangeIE");
            }
        }
        /// <summary>
        /// 改变颜色的协程
        /// </summary>
        /// <returns></returns>
        private IEnumerator ColorChangeIE()
        {
            isColorChangeIE = true;
            colorValue = 1;
            sr.material.SetFloat("_FlashAmount", 1);
            while (sr.material.GetFloat("_FlashAmount") > 0.1f)
            {
                colorValue -= Time.deltaTime * 3;
                sr.material.SetFloat("_FlashAmount", colorValue);
                yield return null;
            }
            colorValue = 1;
            isColorChangeIE = false;
        }

    private void RepelledUpdate()
    {

        
        ////每帧被击退一点
        //switch (repellMode)
        //{

        //    case RepellMode.Lerp:
        //        //先快后慢模仿击退    
        //        transform.position = Vector2.Lerp(transform.position, repelledPos, Time.deltaTime * lerpRepellSpeed);
        //        break;
        //    case RepellMode.UniformLerp:
        //        //匀速击退
        //        if (repelledPos != repealEndVector3)
        //        {
        //            repealBeginVector3 = transform.position;
        //            repealTime = 0;
        //            repealEndVector3 = repelledPos;

        //        }
        //        repealTime += Time.deltaTime;
        //        transform.position = Vector3.Slerp(repealBeginVector3, repealEndVector3, repealTime * uniformRepellSpeed);
        //        break;
        //}
        //if (transform.position == repelledPos)
        //{
        //    //击退结束
        //    isRepelledIE = false;
        //}

        //击退结束
        isRepelledIE = false;
        //if (Mathf.Abs(transform.position.x - repelledPos.x) <= 0.05f ||
        //    Mathf.Abs(transform.position.y - repelledPos.y) <= 0.05f)
        //{
        //    //击退结束
        //    isRepelledIE = false;
        //}

    }

    /// <summary>
    /// 击退
    /// </summary>
    /// <param name="targetTr"></param>
    public void Repelled(Transform targetTr)
        {
            if (!isRepelledIE)
            {
                StartCoroutine("RepelledIE", targetTr);
            }
            else
            {
                StopCoroutine("RepelledIE");
            StartCoroutine("RepelledIE", targetTr);
            StopCoroutine("RepelledIE");
        }
        
    }

        /// <summary>
        /// 击退协程
        /// </summary>
        /// <param name="attackerTransform">攻击者的位置</param>
        /// <returns></returns>
        IEnumerator RepelledIE(Transform attackerTransform)
        {
            
            //协程开启
            isRepelledIE = true;
            //得到击退的位移
            repellVector3 = (transform.position - attackerTransform.position).normalized * repelledDis;
            //得到击退后的位置
            repelledPos = transform.position + repellVector3;
            //每帧被击退一点
            while (transform.position != repelledPos)
            {
            switch(repellMode)
            {
                case RepellMode.Lerp:
                    //先快后慢模仿击退
                    transform.position = Vector2.Lerp(transform.position, repelledPos, Time.deltaTime * lerpRepellSpeed);
                    break;
                case RepellMode.UniformLerp:
                    //匀速击退
                    if(repelledPos!=repealEndVector3)
                    {
                        repealBeginVector3 = transform.position;
                        repealTime = 0;
                        repealEndVector3 = repelledPos;
                    }
                    repealTime += Time.deltaTime;
                    transform.position = Vector3.Slerp(repealBeginVector3, repealEndVector3, repealTime * uniformRepellSpeed);
                    break;
            }
            
            yield return null;
            
        }
            //协程关闭
            isRepelledIE = false;
        FollowPlayer();
    }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, findRadius);
            Gizmos.color = Color.red;
        }

        /// <summary>
        /// 是够在攻击角度范围内
        /// </summary>
        /// <param name="enemyPos"></param>
        /// <returns></returns>
        private bool CheakAtkAngel(Vector3 enemyPos)
        {
            //敌人与玩家夹角小于75度
            if (Vector3.Angle(enemyPos, transform.position) < 75)
            {
                return true;
            }
            else
                return false;
        }
    }

