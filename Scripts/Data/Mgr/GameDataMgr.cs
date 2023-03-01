using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameDataMgr:MonoBehaviour
{
    //��������
    public MusicData musicData;

    //��ɫ����
    private PlayerData playerData;

    //Ŀǰ��ɫ���
    public int playerIndex=1;

    //Boss�ǲ�������
    public bool bossDead;

    //����
    public int score;

    //�������
    private Dictionary<string, bool> keyItemDic=new Dictionary<string, bool>();
   

    private static GameDataMgr instance;

    public static GameDataMgr Instance => instance;


    private void Awake()
    {
        //���ؽ�ɫ����
        BinaryDataMgr.Instance.LoadData<PlayerData, PlayerDataContainer>();
        //���ع�������
        BinaryDataMgr.Instance.LoadData<EnemyData, EnemyDataContainer>();
        //������������
        BinaryDataMgr.Instance.LoadData<WeaponData, WeaponDataContainer>();
        //���ؼ�������
        BinaryDataMgr.Instance.LoadData<Skill_Data, Skill_DataContainer>();
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
            InitData();
        }
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        
    }


    //��Ϸ�򿪵�ʱ�򣬳�ʼ����ȡ��Ϸ����
    private void InitData()
    {
        //MusicMgr.Instance.PlayBkAudioSource("Music/������ - �ܹ�_hires");
        //Debug.Log(Application.persistentDataPath);
       


        musicData = BinaryMgr.Instance.LoadData<MusicData>();
        if(musicData==null)
        //������������(����ǰȡ����ѡ)
        ReplaceMusicData();
        //ͬ����������������ʵ��������
        UpdataAllMusicDataReally();

        InitKeyItem();

    }
    /// <summary>
    /// ��ʼ���������
    /// </summary>
    public void InitKeyItem()
    {
        keyItemDic.Add("CarKey", false);
        keyItemDic.Add("OpenDoorTool", false);
    }

    /// <summary>
    /// �����������Ƿ����
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public bool CheackKeyItem(string itemName)
    {
        return keyItemDic[itemName];
    }

    /// <summary>
    /// �õ��������
    /// </summary>
    /// <param name="itemName">��������</param>
    public void GetKeyItem(string itemName)
    {
        if(keyItemDic.ContainsKey(itemName))
        {
            keyItemDic[itemName] = true;
        }
        else
        {
            Debug.Log("������߲�����");
        }
    }
    /// <summary>
    /// ʧȥ�������
    /// </summary>
    /// <param name="itemName"></param>
    public void LostKeyItem(string itemName)
    {
        if (keyItemDic.ContainsKey(itemName))
        {
            keyItemDic[itemName] = false;
        }
        else
        {
            Debug.Log("������߲�����");
        }
    }


    /// <summary>
    /// �õ���������
    /// </summary>
    /// <param name="skillName">���ܵ�����</param>
    /// <returns></returns>
    public Skill_Data GetSkill_Data(string skillName)
    {
        return BinaryDataMgr.Instance.GetTable<Skill_DataContainer>().Skill_Datadic[skillName];
    }

    /// <summary>
    /// �õ���ɫ����
    /// </summary>
    /// <param name="i"></param>
    public PlayerData GetPlayerData(int i)
    {
        playerData = BinaryDataMgr.Instance.GetTable<PlayerDataContainer>().PlayerDatadic[i];
         return playerData;
    }

    /// <summary>
    /// �õ�Ŀǰ��ɫ��Ѫ��
    /// </summary>
    /// <returns></returns>
    public float GetNowPlayerHp()
    {
        return BinaryDataMgr.Instance.GetTable<PlayerDataContainer>().PlayerDatadic[playerIndex].hp;
    }
    /// <summary>
    /// �������Ѫ��
    /// </summary>
    public void ReSetPlayerHp()
    {
        BinaryDataMgr.Instance.GetTable<PlayerDataContainer>().PlayerDatadic[playerIndex].hp = 100;
    }

    /// <summary>
    /// �õ���������
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public EnemyData GetEnemyData(int i)
    {
        return BinaryDataMgr.Instance.GetTable<EnemyDataContainer>().EnemyDatadic[i];
    }

    /// <summary>
    /// �õ���������
    /// </summary>
    /// <param name="i">��Ӧ������ŵ�����</param>
    /// <returns></returns>
    public WeaponData GetWeaponData(int i)
    {
        return BinaryDataMgr.Instance.GetTable<WeaponDataContainer>().WeaponDatadic[i];
    }


    /// <summary>
    /// ������������
    /// </summary>
    public void ReplaceMusicData()
    {
        musicData = new MusicData
        {
            isOpenBk = true,
            isOpenSound = true,
            soundVoluem = 1f,
            bKVoluem = 1f
        };
        BinaryMgr.Instance.SaveData<MusicData>(musicData);
    }



    /// <summary>
    /// ͬ���������ֿ�������
    /// </summary>
    /// <param name="isOpen"></param>
    public void UpdateBkOpen(bool isOpen)
    {
        musicData.isOpenBk = isOpen;
        //ͬ��ʵ�ʱ������ֿ���
        MusicMgr.Instance.SetBkOpen(isOpen);
       
    }

    /// <summary>
    /// ͬ���������ִ�С����
    /// </summary>
    /// <param name="voluem"></param>
    public void UpdateBkVolume(float voluem)
    {
        musicData.bKVoluem = voluem;
        //ͬ��ʵ�ʱ������ֵĴ�С
        MusicMgr.Instance.SetBkVolume(voluem);
        
    }

    /// <summary>
    /// ͬ����Ч����
    /// </summary>
    /// <param name="isOpen"></param>
    public void UpdateSoundOpen(bool isOpen)
    {
        musicData.isOpenSound = isOpen;
        //ͬ��ʵ�ʱ�����Ч�Ŀ���
        MusicMgr.Instance.SetSoundOpen(isOpen);
    }
    /// <summary>
    /// ͬ��������Ч��С
    /// </summary>
    /// <param name="voluem"></param>
    public void UpdateSoundVoluem(float voluem)
    {
        musicData.soundVoluem = voluem;
        //ͬ��ʵ�ʱ�����Ч�Ĵ�С
        MusicMgr.Instance.SetSoundValuem(voluem);
    }

    //ͬ����������������ʵ��������
    public void UpdataAllMusicDataReally()
    {
        if (musicData == null)
            return;

        //ͬ��ʵ�ʱ������ֵĴ�С
        MusicMgr.Instance.SetBkVolume(musicData.bKVoluem);
        //ͬ��ʵ�ʱ������ֿ���
        MusicMgr.Instance.SetBkOpen(musicData.isOpenBk);
        //ͬ��ʵ�ʱ�����Ч����
        MusicMgr.Instance.SetSoundOpen(musicData.isOpenSound);
        //ͬ��ʵ�ʱ�����Ч��С
        MusicMgr.Instance.SetSoundValuem(musicData.soundVoluem);
    }

    /// <summary>
    /// ������Ч����
    /// </summary>
    public void SaveMusicData()
    {
        //JsonMgr.Instance.SaveData(musicData,"Music");
        BinaryMgr.Instance.SaveData<MusicData>(musicData);
    }
}
