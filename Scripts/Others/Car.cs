using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    [Header("��Ҿ����Զ���ܻ���")]
    public float getDis = 3f;
    [Header("������ʾ")]
    public GameObject tipObj;
    [Header("��־����ţ�һ�㲻�ģ�����������ڿ�����־˵��Ӧ��һ�仰")]
    public int index = 4;

    BoxCollider2D boxCollider2D;

    protected Collider2D col2D;

    private Animator animator;

    private float time;

    private bool isWin;

    private void Awake()
    {
        isWin = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        tipObj.SetActive(false);
        animator = GetComponent<Animator>();
       
    }


    protected virtual void Update()
    {
        if (isWin)
            return;
       
            col2D = Physics2D.OverlapCircle(transform.position, getDis, 1 << LayerMask.NameToLayer("Player"));
        if (col2D != null && col2D.CompareTag("Player"))
        {
            tipObj.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (GameDataMgr.Instance.CheackKeyItem("CarKey"))
                {

                    if (GameDataMgr.Instance.bossDead)
                    {
                        BasePlayerController.Instance.gameObject.transform.SetParent(this.transform);
                        BasePlayerController.Instance.gameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
                        boxCollider2D.isTrigger = true;
                        animator.SetTrigger("move");
                        BasePlayerController.Instance.isDead = true;
                        StartCoroutine(Move());
                        isWin = true;
                    }
                    else
                    {
                        tipObj.gameObject.SetActive(false);
                        TalkMgr.Instance.TalkSelf("����Ҫ�ȴ����������;");
                    }

                }
                else
                {


                    tipObj.gameObject.SetActive(false);
                    TalkMgr.Instance.TalkSelf("����Ҫ��Կ�ײ���������,Ҳ�����·�ϵĴ�����;");
                }
            }

        }
        else
        {
            tipObj.gameObject.SetActive(false);
        }
    }

    IEnumerator Move()
    {
        while (time<5f)
        {
            print(time);
            transform.Translate(transform.right*10f * Time.deltaTime);
            yield return null;
            time += Time.deltaTime;
        }
        HXW_UIManager.GetInstance().ShowPanelAsync<GameOverPanel_Win>(GameOverPanel_Win.prefabsPath);
        StopCoroutine(Move());
        //HXW_UIManager.Instance.HidePanel(GamePanel.prefabsPath);
        //while (time < 5f)
        //{
        //    transform.Translate(Vector3.right * 10 * Time.deltaTime);
        //    yield return null;
        //    time += Time.deltaTime;
        //}
        //Time.timeScale = 0f;
    }
}
