using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GamePanel : HXW_MapPanel
{
    public static string prefabsPath = "GamePanel";//避免后期文件路径改变，那时，只需要改变这里的路径即可

    private Text bloodText;
    private Text attackText;
    private Text defendText;
    private Text bulletText;
    //等级
    private Text levelText;
    //时间text
    private Text timeTxt;
    //分钟
    private int min;
    //秒
    private int s;

    private Button suspendButton;
    //经验条
    private Image ExpContentImg;

    //角色血量
    private float nowHp;

    //本级别最大经验
    private float maxExp = 12;

    //经验值
    public float nowExp;

    //等级
    private int levelNuml;
    
    public override void ShowMeFirst()
    {
        EventMgr.Instance.AddEventListener("ReSetData", ReSetData);

        levelNuml = 1;
        levelText = GetPanel("LevelText").GetUI<Text>();
        levelText.text = "等级" + levelNuml;
        bloodText = GetPanel("imgBlood/Text").GetUI<Text>();

        timeTxt = GetPanel("TimeTxt").GetUI<Text>();
        s = 0;
        min = 0;

        ThreadPool.QueueUserWorkItem((o) =>
        {

            while (min < 10)
            {
                Thread.Sleep(1000);
                s += 1;
                if (s >= 60)
                {
                    s = 0;
                    min += 1;
                }
            }

        });


        ExpContentImg = GetPanel("ExpFrameImg/ExpContentImg").GetUI<Image>();


        nowHp = GameDataMgr.Instance.GetNowPlayerHp();
        
        
        

        attackText = GetPanel("imgAtk/Text").GetUI<Text>();
        defendText = GetPanel("imgDef/Text").GetUI<Text>();
        bulletText = GetPanel("imgBullet/Text").GetUI<Text>();

        suspendButton = GetPanel("btnSuspend").GetUI<Button>();
        suspendButton.onClick.AddListener(() =>
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Click");
            Time.timeScale = 0f;
            HXW_UIManager.GetInstance().ShowPanelAsync<SuspendPanel>(SuspendPanel.prefabsPath,PanelLayer.Mid);
        });
    }

    private float targetExp=0;

 
    private void Update()
    {
       
        targetExp = Mathf.Lerp(targetExp, nowExp, Time.unscaledDeltaTime * 5f);
        ExpContentImg.fillAmount = targetExp / maxExp;
        if (targetExp >= maxExp)
        {
            levelNuml++;
            levelText.text = "等级" + levelNuml;
            MusicMgr.Instance.PlaySound("Music/Audio/Up");
            nowExp = 0;
            maxExp =maxExp*1.065f + 5;
            EventMgr.Instance.EventTrigger("LevelUp");
            
        }

        timeTxt.text = "时间：" + min + "分" + s + "秒";

        if(min>=10)
        {
           
        }
    }

    private void ReSetData()
    {
        s = 0;
        min = 0;
        levelNuml = 1;
        nowExp = 0;
        maxExp = 30;
        GameDataMgr.Instance.ReSetPlayerHp();
    }

    private void OnDisable()
    {
        //ReSetData();
        //EventMgr.Instance.CanselEventListener<float>("PlayerHit", GetHit);
        EventMgr.Instance.CanselEventListener<float>("UpdateHp", UpdateHp);
    }

    private void GetHit(float damage)
    {
        if (nowHp <= 0)
            return;
        MusicMgr.Instance.PlaySound("Music/Audio/Hit");
        nowHp -= damage;
        bloodText.text = nowHp.ToString();
    }

    private void UpdateHp(float damage)
    {
        if (nowHp <= 0)
            return;
        MusicMgr.Instance.PlaySound("Music/Audio/Hit");
        nowHp -= damage;
        bloodText.text = nowHp.ToString();
    }
    private void OnEnable()
    {

        //GameDataMgr.Instance.ReSetPlayerHp();
        //HXW_UIManager.GetInstance().HidePanel(LoadIngPanel.prefabsPath);

        //EventMgr.Instance.AddEventListener<float>("PlayerHit", GetHit);
        EventMgr.Instance.AddEventListener<float>("UpdateHp", UpdateHp);
        nowHp = GameDataMgr.Instance.GetNowPlayerHp();
        bloodText = GetPanel("imgBlood/Text").GetUI<Text>();
        bloodText.text = nowHp.ToString();
        levelNuml = 1;
        levelText = GetPanel("LevelText").GetUI<Text>();
        levelText.text = "等级" + levelNuml;
    }
}
