using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Boss : HXW_EnemyBase
{
    //召唤怪物cd
    protected float currentSkillCD;
    protected bool isAttacking;
    
    //进程攻击cd
    private float shortRangeAtkCd;
    //远程攻击cd
    private float remoteRangeAtkCd;

    private UnityAction<float,float> hitAction;

    private float bossMaxHp;

    protected override void Start()
    {
        base.Start();
        
        HXW_UIManager.Instance.ShowPanelAsync<BossBloodPanel>(BossBloodPanel.prefabsPath,PanelLayer.Max,firstTimeCallback:(panel) =>
        {
            hitAction += panel.RefreshBlood;
        });
        bossMaxHp = enemyData.hp;
    }


    protected override void Update()
    {
        base.Update();
        remoteRangeAtkCd += Time.deltaTime;
        currentSkillCD += Time.deltaTime;
        shortRangeAtkCd += Time.deltaTime;
    }
    public override int GetExcelIndex()
    {
        return 7;
    }
    protected override void Attack()
    {
        if(remoteRangeAtkCd>Random.Range(5,10))
        {
            RemoteAttack();
            remoteRangeAtkCd = 10;
            return;
        }
        
        if (currentSkillCD >= 15)
        {
            DoSkill();
            currentSkillCD = 0;
            return;
        }
        if(shortRangeAtkCd>=3)
        {
            atkCol = Physics2D.OverlapCircle(transform.position, enemyData.attackRadius, 1 << LayerMask.NameToLayer("Player"));
            if (atkCol != null)
            {
                animator.SetTrigger("atk");
            }
        }
    }
    protected void RemoteAttack()
    {
        if (isAttacking == true)
            return;
        ForbidMove();
        isAttacking = true;
        float delayTime = Random.Range(0.2f, 0.4f);
        DelayAction(delayTime, () => { OpenMove(); isAttacking = false; });

        //ObjectPoolMgr.Instance.GetObject(EnemyBullet.prefabsPath_1, (GameObject obj) =>
        //{
        //    MusicMgr.Instance.PlaySound("Music/Audio/Fire7", this.gameObject);
        //    EnemyBullet bullet = obj.GetComponent<EnemyBullet>();
        //    if (bullet != null)
        //        bullet.Fire(transform.position, direction);
        //});

        StartCoroutine(DelayAtk(0.2f));
    }

    private IEnumerator DelayAtk(float time)
    {
        int num = 0;
        int maxNum = Random.Range(12, 30);
        int dir = Random.Range(0, 2) == 0 ? -1 : 1;
        while (num< maxNum)
        {
            yield return new WaitForSeconds(time);
            ObjectPoolMgr.Instance.GetObject(EnemyBullet.prefabsPath_Boss, (GameObject obj) =>
            {
                MusicMgr.Instance.PlaySound("Music/Audio/Fire7", this.gameObject);
                EnemyBullet bullet = obj.GetComponent<EnemyBullet>();
                if (bullet != null)
                    bullet.FireAround(transform.position, direction, dir * 20*num);
            });
            ++num;
        }
        StopCoroutine("DelayAtk");
    }

    private Collider2D atkCol;
    protected void ShortRangeAttack()
    {
        atkCol = Physics2D.OverlapCircle(transform.position, enemyData.attackRadius - 2f, 1 << LayerMask.NameToLayer("Player"));
        if (atkCol != null)
        {
            if (atkCol.CompareTag("Player"))
            {
                //角色受伤，同步面板
                EventMgr.Instance.EventTrigger<float>("PlayerHit", enemyData.atk);
            }
        }

        
    }

    protected void DoSkill()
    {
        DelayAction(0.3f,SummonEnemy);
    }

    public void SummonEnemy()
    {
        for (int i = 0; i < 2; ++i)
        {
            ObjectPoolMgr.Instance.GetObject("Enemyprefab/WormEnemy", (GameObject obj) =>
            {
                MusicMgr.Instance.PlaySound("Music/Audio/Summon", this.gameObject);
                int edge = Random.Range(0, 2) == 0 ? -1 : 1;
                int x = edge * Random.Range(4, 8);

                edge = Random.Range(0, 2) == 0 ? -1 : 1;
                int y = edge * Random.Range(4, 8);

                Vector3 targetPos = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
                obj.transform.position = targetPos;
            });
        }
    }

    public override void GetHit(float damage)
    {
        if (isDead == true) return;

        MusicMgr.Instance.PlaySound("Music/Audio/Hit", this.gameObject);
        Debug.Log("EnemyGetHit" + damage);

        if (damage > enemyData.def)
        {
            hp -= damage;
            hitAction?.Invoke(bossMaxHp, hp);
        }
        if (!isOpenColorI)
        {
            redValue = 1f;

            //改变颜色
            StartCoroutine("ChangeColorRed");
        }

        if (hp > 0)
        {
            //animator.SetTrigger("hit");

            OnGetHit();
        }
        else
        {
            //判断玩家07_02技能是否启用
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

    protected override void OnGetHit()
    {
        animator.SetTrigger("hit");
    }

    protected override void Dead()
    {
        
        isDead = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("dead");
        rb.velocity = Vector2.zero;
        Invoke("PutPool", 2f);
       
        HXW_UIManager.Instance.HidePanel(BossBloodPanel.prefabsPath);
        
        GameDataMgr.Instance.bossDead = true;
        
    }

    int num = 0;
    Vector3 targetPos;
    int edge;
    int x;
    int y;

    private IEnumerator GenerateExp()
    {
        num = 0;
        while (num < 25)
        {
            edge = Random.Range(0, 2) == 0 ? -1 : 1;
            x = edge * Random.Range(0, 6);

            edge = Random.Range(0, 2) == 0 ? -1 : 1;
            y = edge * Random.Range(0, 6);
            targetPos = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

            ObjectPoolMgr.Instance.GetObject("Items/Exp5", (a) =>
            {
                a.transform.position = targetPos;
            });
            ++num;
            yield return null;
        }
        num = 0;
        while (num < 10)
        {
            edge = Random.Range(0, 2) == 0 ? -1 : 1;
            x = edge * Random.Range(0, 10);

            edge = Random.Range(0, 2) == 0 ? -1 : 1;
            y = edge * Random.Range(0, 10);
            targetPos = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

            ObjectPoolMgr.Instance.GetObject("Items/Exp40", (a) =>
            {
                a.transform.position = targetPos;
            });
            ++num;
            yield return null;
        }
        ObjectPoolMgr.Instance.PutObject(this.gameObject);
    }

    protected override void PutPool()
    {

        StartCoroutine(GenerateExp());
       
        MusicMgr.Instance.PlayBkAudioSource("Music/battle");
    }
}
