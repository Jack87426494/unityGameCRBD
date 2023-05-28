using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowMode
{
    //����׷��
    UniformLerp,
    //�ȿ����
    Lerp,
    //A��Ѱ·
    AFindWay,
    //�㶨�ٶ�׷��
    Uniform,
}

public enum RepellMode
{
    //���ٻ���
    UniformLerp,
    //�ȿ����
    Lerp,
}


public  class BaseEnemy : MonoBehaviour
{
    [Header("Ѳ�߷�Χ")]
    public float findRadius;
    [Header("��С��������")]
    public float nearDistance;

    [Header("����׷��ģʽ�ƶ��ٶ�")]
    public float uniformMoveSpeed;

    [Header("�ȿ����׷��ģʽ�ƶ��ٶ�")]
    public float lerpMoveSpeed;

    [Header("׷��ģʽ")]
    public FollowMode followMode;


    [Header("����ģʽ")]
    public RepellMode repellMode;

    [Header("���ٻ���ģʽ�ٶ�")]
    public float uniformRepellSpeed;

    [Header("�ȿ��������ģʽ�ٶ�")]
    public float lerpRepellSpeed;

    [Header("���˵ľ���")]
    public float repelledDis;

    //���˵�λ��
    private Vector3 repellVector3;
  
    //�ǹ���������Э��
    private bool isRepelledIE;
    //���˺��λ��
    private Vector3 repelledPos;

    //�Ƿ�����ɫ�仯Э��
    private bool isColorChangeIE;
    //Ŀǰ��ɫ��ֵ
    private float colorValue;

    //���ͼ��
    private LayerMask playerMask;

    //��������׷����ҵı���
    private float time;
    private Vector3 beginVector3;
    private Vector3 endVector3;

    //������������ı���
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
        //��û�б����˵������Ѳ��
        if (!isRepelledIE)
        {
            FollowPlayer();
        }
        else
        {
            RepelledUpdate();
        }
    }
    //Ѳ��
    protected virtual void FollowPlayer()
    {

        hit2D = Physics2D.OverlapCircle(transform.position, findRadius, 1 << LayerMask.NameToLayer("Player"));

        if (hit2D != null && hit2D.CompareTag("Player"))
        {
            distance = Vector3.Distance(hit2D.gameObject.transform.position,transform.position);
            switch (followMode)
            {
                case FollowMode.UniformLerp:
                    //����׷��
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
                    //��Сΰ׷��


                    break;
            }

        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet"))
            {
                //�ı���ɫ
                ColorChange();

            ////�õ����˵�λ��
            //repellVector3 = (transform.position - collision.gameObject.transform.position).normalized * repelledDis;
            ////�õ����˺��λ��
            //repelledPos = transform.position + repellVector3;
            //������
            isRepelledIE = true;

            ////������
            //Repelled(collision.gameObject.transform);

        }
        }


        /// <summary>
        /// �ı���ɫ
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
        /// �ı���ɫ��Э��
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

        
        ////ÿ֡������һ��
        //switch (repellMode)
        //{

        //    case RepellMode.Lerp:
        //        //�ȿ����ģ�»���    
        //        transform.position = Vector2.Lerp(transform.position, repelledPos, Time.deltaTime * lerpRepellSpeed);
        //        break;
        //    case RepellMode.UniformLerp:
        //        //���ٻ���
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
        //    //���˽���
        //    isRepelledIE = false;
        //}

        //���˽���
        isRepelledIE = false;
        //if (Mathf.Abs(transform.position.x - repelledPos.x) <= 0.05f ||
        //    Mathf.Abs(transform.position.y - repelledPos.y) <= 0.05f)
        //{
        //    //���˽���
        //    isRepelledIE = false;
        //}

    }

    /// <summary>
    /// ����
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
        /// ����Э��
        /// </summary>
        /// <param name="attackerTransform">�����ߵ�λ��</param>
        /// <returns></returns>
        IEnumerator RepelledIE(Transform attackerTransform)
        {
            
            //Э�̿���
            isRepelledIE = true;
            //�õ����˵�λ��
            repellVector3 = (transform.position - attackerTransform.position).normalized * repelledDis;
            //�õ����˺��λ��
            repelledPos = transform.position + repellVector3;
            //ÿ֡������һ��
            while (transform.position != repelledPos)
            {
            switch(repellMode)
            {
                case RepellMode.Lerp:
                    //�ȿ����ģ�»���
                    transform.position = Vector2.Lerp(transform.position, repelledPos, Time.deltaTime * lerpRepellSpeed);
                    break;
                case RepellMode.UniformLerp:
                    //���ٻ���
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
            //Э�̹ر�
            isRepelledIE = false;
        FollowPlayer();
    }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, findRadius);
            Gizmos.color = Color.red;
        }

        /// <summary>
        /// �ǹ��ڹ����Ƕȷ�Χ��
        /// </summary>
        /// <param name="enemyPos"></param>
        /// <returns></returns>
        private bool CheakAtkAngel(Vector3 enemyPos)
        {
            //��������Ҽн�С��75��
            if (Vector3.Angle(enemyPos, transform.position) < 75)
            {
                return true;
            }
            else
                return false;
        }
    }

