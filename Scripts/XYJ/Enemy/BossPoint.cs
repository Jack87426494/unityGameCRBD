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


        //���Դ���
        //col2D = Physics2D.OverlapCircle(transform.position, 30f, 1 << LayerMask.NameToLayer("Player"));
        //if (col2D != null && col2D.CompareTag("Player") && !isSummon)
        //{
        //    //ʵ����������
        //    enemyData = BinaryDataMgr.Instance.GetTable<EnemyDataContainer>().
        //    EnemyDatadic[7];
        //    MusicMgr.Instance.PlayBkAudioSource("Music/Boss");



        //    ObjectPoolMgr.Instance.GetObject(enemyData.loadPath, (obj) =>
        //    {
        //        obj.transform.position = transform.position;

        //        //obj.GetComponent<HXW_EnemyBase>().deadEvent += BossDead;//���������¼�
        //    });
        //    isSummon = true;
        //}



        if (!GameDataMgr.Instance.CheackKeyItem("CarKey"))
            return;

        col2D = Physics2D.OverlapCircle(transform.position, 30f, 1 << LayerMask.NameToLayer("Player"));
        if (col2D != null && col2D.CompareTag("Player")&& !isSummon)
        {
            //ʵ����������
            enemyData = BinaryDataMgr.Instance.GetTable<EnemyDataContainer>().
            EnemyDatadic[7];
            MusicMgr.Instance.PlayBkAudioSource("Music/Boss");

            

            ObjectPoolMgr.Instance.GetObject(enemyData.loadPath, (obj) =>
            {
                obj.transform.position = transform.position;

                //obj.GetComponent<HXW_EnemyBase>().deadEvent += BossDead;//���������¼�
            });
            isSummon = true;
        }


    }

    //private void BossDead(GameObject obj)
    //{
    //    GameDataMgr.Instance.bossDead = true;
    //}
}
