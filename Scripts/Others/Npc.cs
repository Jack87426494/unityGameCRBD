using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Npc : MonoBehaviour
{
    [Header("玩家距离多远才能互动")]
    public float getDis = 5f;
    [Header("交互提示")]
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
            TalkMgr.Instance.Talk("士….咳咳….士兵。;" +
                "坚持住，我这就给你止血。;" +
                "来不及了，士兵，这封信，一定要完好无损带出去，交给国家，这是人类的希望。;" +
                "这里到底发生了什么。;" +
                "我原本秉持着我这一生的信仰，用我的智慧造福人类，后来却被人性的贪婪毁于一旦，你一定要将这些信息带出去，这是我最后的信仰。;" +
                "得到信封，获得卡车钥匙", false,canBreakOrNext:false);
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
