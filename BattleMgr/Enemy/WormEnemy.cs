using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnemy : HXW_EnemyBase
{
    private bool isAttacking;
    private bool isHide;
    private float awayTime;
    private float hideTime;
    public override int GetExcelIndex()
    {
        return 5;
    }
    protected override void MoveController()
    {
        rb.velocity = Vector3.zero;
        if (hit2D == null || !hit2D.CompareTag("Player") || distance >= enemyData.attackRadius)
            awayTime += Time.deltaTime;
        else
            awayTime = 0;

        if (isHide == true)
            hideTime += Time.deltaTime;
        else
            hideTime = 0;
        if (awayTime >= 0.5f && isHide == false)
        {
            Hide();
        }
        if (hideTime >= 1f && isHide == true)
        {
            Appear();
        }
    }
    protected override void ControlDir()
    {
        if (hit2D == null || !hit2D.CompareTag("Player"))
            return;

        if (hit2D.gameObject.transform.position.x > transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    private void Hide()
    {
        isHide = true;
        animator.SetBool("DisAppear", true);
        coll.enabled = false;
    }
    private void Appear()
    {
        isHide = false;
        animator.SetBool("DisAppear", false);
        gameObject.transform.position = BasePlayerController.Instance.transform.position + new Vector3(Random.Range(2, 5), Random.Range(2, 5), 0);
        coll.enabled = true;
    }
    protected override void OnGetHit()
    {
        Hide();
    }
    protected override void Attack()
    {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != normalClipName)
            return;
        if (isAttacking == true)
            return;
        isAttacking = true;
        animator.SetTrigger("Attack");
        DelayAction(0.35f, () =>
        {
            ObjectPoolMgr.Instance.GetObject(EnemyBullet.prefabsPath, (GameObject obj) =>
            {
                MusicMgr.Instance.PlaySound("Music/Audio/Fire7", this.gameObject);
                EnemyBullet bullet = obj.GetComponent<EnemyBullet>();
                if (bullet != null)
                    bullet.Fire(transform.position, direction);
            });
            isAttacking = false; 
        });
    }
}
