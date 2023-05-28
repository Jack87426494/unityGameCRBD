using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonsterPoint : MonoBehaviour
{
    [Header("����������ſ�ʼ")]
    public int enemyTypeStart;
    [Header("����������Ž���")]
    public int enemyTypeEnd;

    [Header("��һ������Ŀ�ʼ����ʱ��")]
    public float generateFirstTime = 5f;

    [Header("ÿһ��֮�й��������֮�䴴���ļ��ʱ��")]
    public float generateMonsterGapTime = 0f;

    [Header("ÿ������֮��Ĵ����ļ��ʱ��")]
    public float generateWaveGapTime = 10f;

    [Header("ÿ�����������")]
    public float enemyNum = 2;

    

    //[Header("һ�����������������")]
    //public List<int> enemyList = new List<int>();

    [Header("һ���ᴴ����������")]
    public int waveNum = 9999;

    [Header("���ﱻ����ʱ��������������")]
    public int minDistance=20;

    [Header("���ﱻ����ʱ���������Զ����")]
    public int maxDistance=25;


    //Ŀǰ����ʣ��ֻ����Ҫ����
    protected int nowEnemyNum;
    //Ŀǰ��ʣ����������Ҫ����
    protected int nowWaveNum;
    //����ʵ��
    private GameObject enemyObj;
    //��������
    protected EnemyData enemyData;
    //���������λ��
    protected Vector3 enemyPos;

    //�����������
    [SerializeField]
    [Header("�����͹����Ŀǰ�������")]
    protected int enemyAliveNum;

    [Header("�����͹�������������")]
    public int enemyAliveMax=80;

    [Header("����ÿ��������������")]
    public float enemyWaveMax = 40;
    private void Start()
    {
        nowWaveNum = 0;
        if(BasePlayerController.Instance!=null)
        //����һ������
        Invoke("CreateWave", generateFirstTime);
    }


    /// <summary>
    /// ����һ������
    /// </summary>
    protected virtual void CreateWave()
    {
        //nowEnemyNum = enemyList.Count;
        // ����һ������(�����ж�ֻ��)
        nowEnemyNum = (int)enemyNum;
        Invoke("createEnemy", generateMonsterGapTime);
        
        
        if (nowWaveNum < waveNum)
        {
            //����һ������
            Invoke("CreateWave", generateWaveGapTime);
            ++nowWaveNum;
        }
    }

    //�����
    protected int randomInt;

    protected int x;
    protected int y;

    /// <summary>
    /// ����һֻ����
    /// </summary>
    protected virtual void createEnemy()
    {
        if (enemyAliveNum < enemyAliveMax)
        {
            //����õ������Ұ��Χ���λ��
            randomInt = Random.Range(0, 2) == 0 ? -1 : 1;
            x = randomInt * Random.Range(minDistance, maxDistance);
            randomInt = Random.Range(0, 2) == 0 ? -1 : 1;
            y = randomInt * Random.Range(minDistance, maxDistance);
            enemyPos = new Vector3(x, y) + BasePlayerController.Instance.transform.position;
            //randomInt = Random.Range(0, 4);

            #region λ��
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


            //ʵ����������
            enemyData = BinaryDataMgr.Instance.GetTable<EnemyDataContainer>().
                EnemyDatadic[Random.Range(enemyTypeStart, enemyTypeEnd)];


            ObjectPoolMgr.Instance.GetObject(enemyData.loadPath, (obj) =>
            {
                obj.transform.position = enemyPos;
                enemyAliveNum++;//������������1
                obj.GetComponent<HXW_EnemyBase>().deadEvent += ReduceEnemyAliveNum;//���������¼�
            });
        }

        #region ����
        //enemyObj= Instantiate(Resources.Load<GameObject>(enemyData.loadPath), enemyPos, Quaternion.identity);
        //enemyObj.GetComponent<HXW_EnemyBase>().enemyData = enemyData;

        //Instantiate(Resources.Load<GameObject>("Character/Monster"), enemyPos,Quaternion.identity);
        //Instantiate(Resources.Load<GameObject>("Character/SummonerEnemy"), enemyPos, Quaternion.identity);
        #endregion
        if (nowEnemyNum > 0)
        {
            nowEnemyNum--;
            // ����һֻ����
            Invoke("createEnemy", generateMonsterGapTime);
        }
    }

    /// <summary>
    /// ���ֵ�Ĺ����Ƿ�ȫ���������
    /// </summary>
    public bool PointIsOver()
    {
        return nowWaveNum <= 0 && nowEnemyNum <= 0;
    }

    protected void ReduceEnemyAliveNum(GameObject obj)
    {
        enemyAliveNum--;
        obj.GetComponent<HXW_EnemyBase>().deadEvent -= ReduceEnemyAliveNum;//ȡ�����������¼�
    }

    
}
