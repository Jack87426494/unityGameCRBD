using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Npc : MonoBehaviour
{
    [Header("��Ҿ����Զ���ܻ���")]
    public float getDis = 5f;
    [Header("������ʾ")]
    public GameObject tipObj;
    private Collider2D col2D;
    private float time;
    private Animator animator;

    private bool isTalk;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("live", true);
        tipObj.SetActive(false);
        isTalk = false;
    }

    private void Update()
    {
        col2D = Physics2D.OverlapCircle(transform.position, getDis, 1 << LayerMask.NameToLayer("Player"));
        if (col2D != null && col2D.CompareTag("Player")&&!isTalk)
        {
                
            tipObj.SetActive(true);
            Camera.main.GetComponent<GameMainCamera>().ChangeLens(transform);
            TalkMgr.Instance.Talk("ʿ��.�ȿȡ�.ʿ����;" +
                "���ס������͸���ֹѪ��;" +
                "�������ˣ�ʿ��������ţ�һ��Ҫ����������ȥ���������ң����������ϣ����;" +
                "���ﵽ�׷�����ʲô��;" +
                "��ԭ������������һ�������������ҵ��ǻ��츣���࣬����ȴ�����Ե�̰������һ������һ��Ҫ����Щ��Ϣ����ȥ������������������;" +
                "�õ��ŷ⣬��ÿ���Կ��", false,canBreakOrNext:false);
            GameDataMgr.Instance.GetKeyItem("CarKey");
            isTalk = true;
            StartCoroutine(StopAnimator());

        }
        else
        {
            tipObj.SetActive(false);
        }
    }

    IEnumerator StopAnimator()
    {
        time = 30f;
        while(time>0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        animator.SetBool("live", false);
        StopCoroutine(StopAnimator());
    }
}
