using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterPoint : MonoBehaviour
{
    [Header("怪物类型序号开始")]
    public int enemyTypeStart;
    [Header("怪物类型序号结束")]
    public int enemyTypeEnd;

    [Header("第一波怪物的开始创建时间")]
    public float generateFirstTime = 5f;

    [Header("每一波之中怪物与怪物之间创建的间隔时间")]
    public float generateMonsterGapTime = 0f;

    [Header("每波怪物之间的创建的间隔时间")]
    public float generateWaveGapTime = 10f;

    [Header("每波怪物的数量")]
    public float enemyNum = 2;

    

    //[Header("一波怪物的数量和种类")]
    //public List<int> enemyList = new List<int>();

    [Header("一共会创建几波怪物")]
    public int waveNum = 9999;

    [Header("怪物被创建时距离玩家最近距离")]
    public int minDistance=20;

    [Header("怪物被创建时距离玩家最远距离")]
    public int maxDistance=25;


    //目前波还剩几只怪需要创建
    protected int nowEnemyNum;
    //目前还剩几波怪物需要创建
    protected int nowWaveNum;
    //怪物实例
    private GameObject enemyObj;
    //怪物数据
    protected EnemyData enemyData;
    //创建怪物的位置
    protected Vector3 enemyPos;

    //怪物存活的数量
    [SerializeField]
    [Header("该类型怪物的目前存活数量")]
    protected int enemyAliveNum;

    [Header("该类型怪物存活的最大数量")]
    public int enemyAliveMax=80;

    [Header("限制每波怪物的最大数量")]
    public float enemyWaveMax = 40;
    private void Start()
    {
        nowWaveNum = 0;
        if(BasePlayerController.Instance!=null)
        //创建一波怪物
        Invoke("CreateWave", generateFirstTime);
    }


    /// <summary>
    /// 创建一波怪物
    /// </summary>
    protected virtual void CreateWave()
    {
        //nowEnemyNum = enemyList.Count;
        // 创建一波怪物(可以有多只怪)
        nowEnemyNum = (int)enemyNum;
        Invoke("createEnemy", generateMonsterGapTime);
        
        
        if (nowWaveNum < waveNum)
        {
            //创建一波怪物
            Invoke("CreateWave", generateWaveGapTime);
            ++nowWaveNum;
        }
    }

    //随机数
    protected int randomInt;

    protected int x;
    protected int y;

    /// <summary>
    /// 创建一只怪物
    /// </summary>
    protected virtual void createEnemy()
    {
        if (enemyAliveNum < enemyAliveMax)
        {
            //随机得到玩家视野范围外的位置
            randomInt = Random.Range(0, 2) == 0 ? -1 : 1;
            x = randomInt * Random.Range(minDistance, maxDistance);
            randomInt = Random.Range(0, 2) == 0 ? -1 : 1;
            y = randomInt * Random.Range(minDistance, maxDistance);
            enemyPos = new Vector3(x, y) + BasePlayerController.Instance.transform.position;
            //randomInt = Random.Range(0, 4);

            #region 位置
            //switch (randomInt)
            //{
            //    case 0:
            //        enemyPos = BasePlayerController.Instance.transform.position +
            //  Vector3.right * Random.Range(minDistance, maxDistance) +
            //  Vector3.up * Random.Range(-maxDistance, maxDistance);
            //        break;
            //    case 1:
            //        enemyPos = BasePlayerController.Instance.transform.position +
            //  Vector3.right * Random.Range(-maxDistance, maxDistance) +
            //  Vector3.up * Random.Range(minDistance, maxDistance);
            //        break;
            //    case 2:
            //        enemyPos = BasePlayerController.Instance.transform.position +
            // Vector3.right * Random.Range(-minDistance, -maxDistance) +
            // Vector3.up * Random.Range(-maxDistance, maxDistance);
            //        break;
            //    case 3:
            //        enemyPos = BasePlayerController.Instance.transform.position +
            // Vector3.right * Random.Range(-maxDistance, maxDistance) +
            // Vector3.up * Random.Range(-minDistance, -maxDistance);
            //        break;
            //};
            #endregion


            //实际生产怪物
            enemyData = BinaryDataMgr.Instance.GetTable<EnemyDataContainer>().
                EnemyDatadic[Random.Range(enemyTypeStart, enemyTypeEnd)];


            ObjectPoolMgr.Instance.GetObject(enemyData.loadPath, (obj) =>
            {
                obj.transform.position = enemyPos;
                enemyAliveNum++;//存活怪物数量加1
                obj.GetComponent<HXW_EnemyBase>().deadEvent += ReduceEnemyAliveNum;//订阅死亡事件
            });
        }

        #region 生产
        //enemyObj= Instantiate(Resources.Load<GameObject>(enemyData.loadPath), enemyPos, Quaternion.identity);
        //enemyObj.GetComponent<HXW_EnemyBase>().enemyData = enemyData;

        //Instantiate(Resources.Load<GameObject>("Character/Monster"), enemyPos,Quaternion.identity);
        //Instantiate(Resources.Load<GameObject>("Character/SummonerEnemy"), enemyPos, Quaternion.identity);
        #endregion
        if (nowEnemyNum > 0)
        {
            nowEnemyNum--;
            // 创建一只怪物
            Invoke("createEnemy", generateMonsterGapTime);
        }
    }

    /// <summary>
    /// 出怪点的怪物是否全部创建完毕
    /// </summary>
    public bool PointIsOver()
    {
        return nowWaveNum <= 0 && nowEnemyNum <= 0;
    }

    protected void ReduceEnemyAliveNum(GameObject obj)
    {
        enemyAliveNum--;
        obj.GetComponent<HXW_EnemyBase>().deadEvent -= ReduceEnemyAliveNum;//取消订阅死亡事件
    }

    
}
