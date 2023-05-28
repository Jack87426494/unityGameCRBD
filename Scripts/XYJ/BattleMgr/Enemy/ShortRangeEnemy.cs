using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeEnemy : HXW_EnemyBase
{
    private bool isMove;


    protected override void OnEnable()
    {
        isMove = false;
        base.OnEnable();
    }

    protected override void MoveController()
    {
        if (!isMove)
            return;
        base.MoveController();
    }

    public override int GetExcelIndex()
    {
        return 1;
    }

    private bool isAttack;
    private Vector3 attackDirection;
    protected override void Attack()
    {

        animator.SetTrigger("attack");
        attackDirection = direction;
        StartCoroutine("AttackPlayer");
    }

    private IEnumerator AttackPlayer()
    {
        isAttack = true;

        while (isAttack)
        {
            if (hit2D == null) break;
            transform.position = Vector3.Lerp(transform.position, hit2D.transform.position,Time.deltaTime*3.6f);
            yield return null;
        }
        StopCoroutine("AttackPlayer");
    }

    protected override void ControlDir()
    {
        if (isAttack)
            return;
        base.ControlDir();
    }

    protected void Damage()
    {
        isAttack = false;
        hit2D = Physics2D.OverlapCircle(transform.position + direction/2, 0.3f, 1 << LayerMask.NameToLayer("Player"));
        if (hit2D != null && hit2D.CompareTag("Player"))
        {

            //角色受伤，同步面板
            EventMgr.Instance.EventTrigger<float>("PlayerHit", enemyData.atk);

        }
    }



    protected void MoveBegin()
    {
        isMove = true;
    }

}
