using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPoint:MonoBehaviour
{
    private EnemyData enemyData;

    private Collider2D col2D;

    bool isSummon;
    private void Awake()
    {
        isSummon = false;

        

    }


    private void Update()
    {


        //测试代码
        //col2D = Physics2D.OverlapCircle(transform.position, 30f, 1 << LayerMask.NameToLayer("Player"));
        //if (col2D != null && col2D.CompareTag("Player") && !isSummon)
        //{
        //    //实际生产怪物
        //    enemyData = BinaryDataMgr.Instance.GetTable<EnemyDataContainer>().
        //    EnemyDatadic[7];
        //    MusicMgr.Instance.PlayBkAudioSource("Music/Boss");



        //    ObjectPoolMgr.Instance.GetObject(enemyData.loadPath, (obj) =>
        //    {
        //        obj.transform.position = transform.position;

        //        //obj.GetComponent<HXW_EnemyBase>().deadEvent += BossDead;//订阅死亡事件
        //    });
        //    isSummon = true;
        //}



        if (!GameDataMgr.Instance.CheackKeyItem("CarKey"))
            return;

        col2D = Physics2D.OverlapCircle(transform.position, 30f, 1 << LayerMask.NameToLayer("Player"));
        if (col2D != null && col2D.CompareTag("Player")&& !isSummon)
        {
            //实际生产怪物
            enemyData = BinaryDataMgr.Instance.GetTable<EnemyDataContainer>().
            EnemyDatadic[7];
            MusicMgr.Instance.PlayBkAudioSource("Music/Boss");

            

            ObjectPoolMgr.Instance.GetObject(enemyData.loadPath, (obj) =>
            {
                obj.transform.position = transform.position;

                //obj.GetComponent<HXW_EnemyBase>().deadEvent += BossDead;//订阅死亡事件
            });
            isSummon = true;
        }


    }

    //private void BossDead(GameObject obj)
    //{
    //    GameDataMgr.Instance.bossDead = true;
    //}
}
