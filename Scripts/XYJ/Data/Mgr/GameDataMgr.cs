using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameDataMgr:MonoBehaviour
{
    //音乐数据
    public MusicData musicData;

    //角色数据
    private PlayerData playerData;

    //目前角色序号
    public int playerIndex=1;

    //Boss是不是死了
    public bool bossDead;

    //分数
    public int score;

    //任务道具
    private Dictionary<string, bool> keyItemDic=new Dictionary<string, bool>();
   

    private static GameDataMgr instance;

    public static GameDataMgr Instance => instance;


    private void Awake()
    {
        //加载角色数据
        BinaryDataMgr.Instance.LoadData<PlayerData, PlayerDataContainer>();
        //加载怪物数据
        BinaryDataMgr.Instance.LoadData<EnemyData, EnemyDataContainer>();
        //加载武器数据
        BinaryDataMgr.Instance.LoadData<WeaponData, WeaponDataContainer>();
        //加载技能数据
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


    //游戏打开的时候，初始化读取游戏数据
    private void InitData()
    {
        //MusicMgr.Instance.PlayBkAudioSource("Music/孙燕姿 - 擒光_hires");
        //Debug.Log(Application.persistentDataPath);
       


        musicData = BinaryMgr.Instance.LoadData<MusicData>();
        if(musicData==null)
        //重置音乐数据(发布前取消勾选)
        ReplaceMusicData();
        //同步所有音乐数据在实际音乐中
        UpdataAllMusicDataReally();

        InitKeyItem();

    }
    /// <summary>
    /// 初始化任务道具
    /// </summary>
    public void InitKeyItem()
    {
        keyItemDic.Add("CarKey", false);
        keyItemDic.Add("OpenDoorTool", false);
    }

    /// <summary>
    /// 检测任务道具是否存在
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public bool CheackKeyItem(string itemName)
    {
        return keyItemDic[itemName];
    }

    /// <summary>
    /// 得到任务道具
    /// </summary>
    /// <param name="itemName">道具名字</param>
    public void GetKeyItem(string itemName)
    {
        if(keyItemDic.ContainsKey(itemName))
        {
            keyItemDic[itemName] = true;
        }
        else
        {
            Debug.Log("任务道具不存在");
        }
    }
    /// <summary>
    /// 失去任务道具
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
            Debug.Log("任务道具不存在");
        }
    }


    /// <summary>
    /// 得到技能数据
    /// </summary>
    /// <param name="skillName">技能的名字</param>
    /// <returns></returns>
    public Skill_Data GetSkill_Data(string skillName)
    {
        return BinaryDataMgr.Instance.GetTable<Skill_DataContainer>().Skill_Datadic[skillName];
    }

    /// <summary>
    /// 得到角色数据
    /// </summary>
    /// <param name="i"></param>
    public PlayerData GetPlayerData(int i)
    {
        playerData = BinaryDataMgr.Instance.GetTable<PlayerDataContainer>().PlayerDatadic[i];
         return playerData;
    }

    /// <summary>
    /// 得到目前角色的血量
    /// </summary>
    /// <returns></returns>
    public float GetNowPlayerHp()
    {
        return BinaryDataMgr.Instance.GetTable<PlayerDataContainer>().PlayerDatadic[playerIndex].hp;
    }
    /// <summary>
    /// 重置玩家血量
    /// </summary>
    public void ReSetPlayerHp()
    {
        BinaryDataMgr.Instance.GetTable<PlayerDataContainer>().PlayerDatadic[playerIndex].hp = 100;
    }

    /// <summary>
    /// 得到怪物数据
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public EnemyData GetEnemyData(int i)
    {
        return BinaryDataMgr.Instance.GetTable<EnemyDataContainer>().EnemyDatadic[i];
    }

    /// <summary>
    /// 得到武器数据
    /// </summary>
    /// <param name="i">对应主键序号的武器</param>
    /// <returns></returns>
    public WeaponData GetWeaponData(int i)
    {
        return BinaryDataMgr.Instance.GetTable<WeaponDataContainer>().WeaponDatadic[i];
    }


    /// <summary>
    /// 重置音乐数据
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
    /// 同步背景音乐开关数据
    /// </summary>
    /// <param name="isOpen"></param>
    public void UpdateBkOpen(bool isOpen)
    {
        musicData.isOpenBk = isOpen;
        //同步实际背景音乐开关
        MusicMgr.Instance.SetBkOpen(isOpen);
       
    }

    /// <summary>
    /// 同步背景音乐大小数据
    /// </summary>
    /// <param name="voluem"></param>
    public void UpdateBkVolume(float voluem)
    {
        musicData.bKVoluem = voluem;
        //同步实际背景音乐的大小
        MusicMgr.Instance.SetBkVolume(voluem);
        
    }

    /// <summary>
    /// 同步音效开关
    /// </summary>
    /// <param name="isOpen"></param>
    public void UpdateSoundOpen(bool isOpen)
    {
        musicData.isOpenSound = isOpen;
        //同步实际背景音效的开关
        MusicMgr.Instance.SetSoundOpen(isOpen);
    }
    /// <summary>
    /// 同步背景音效大小
    /// </summary>
    /// <param name="voluem"></param>
    public void UpdateSoundVoluem(float voluem)
    {
        musicData.soundVoluem = voluem;
        //同步实际背景音效的大小
        MusicMgr.Instance.SetSoundValuem(voluem);
    }

    //同步所有音乐数据在实际音乐中
    public void UpdataAllMusicDataReally()
    {
        if (musicData == null)
            return;

        //同步实际背景音乐的大小
        MusicMgr.Instance.SetBkVolume(musicData.bKVoluem);
        //同步实际背景音乐开关
        MusicMgr.Instance.SetBkOpen(musicData.isOpenBk);
        //同步实际背景音效开关
        MusicMgr.Instance.SetSoundOpen(musicData.isOpenSound);
        //同步实际背景音效大小
        MusicMgr.Instance.SetSoundValuem(musicData.soundVoluem);
    }

    /// <summary>
    /// 保存音效数据
    /// </summary>
    public void SaveMusicData()
    {
        //JsonMgr.Instance.SaveData(musicData,"Music");
        BinaryMgr.Instance.SaveData<MusicData>(musicData);
    }
}
