using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GamePanel : HXW_MapPanel
{
    public static string prefabsPath = "GamePanel";//��������ļ�·���ı䣬��ʱ��ֻ��Ҫ�ı������·������

    private Text bloodText;
    private Text attackText;
    private Text defendText;
    private Text bulletText;
    //�ȼ�
    private Text levelText;
    //ʱ��text
    private Text timeTxt;
    //����
    private int min;
    //��
    private int s;

    private Button suspendButton;
    //������
    private Image ExpContentImg;

    //��ɫѪ��
    private float nowHp;

    //�����������
    private float maxExp = 12;

    //����ֵ
    public float nowExp;

    //�ȼ�
    private int levelNuml;
    
    public override void ShowMeFirst()
    {
        EventMgr.Instance.AddEventListener("ReSetData", ReSetData);

        levelNuml = 1;
        levelText = GetPanel("LevelText").GetUI<Text>();
        levelText.text = "�ȼ�" + levelNuml;
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
            levelText.text = "�ȼ�" + levelNuml;
            MusicMgr.Instance.PlaySound("Music/Audio/Up");
            nowExp = 0;
            maxExp =maxExp*1.065f + 5;
            EventMgr.Instance.EventTrigger("LevelUp");
            
        }

        timeTxt.text = "ʱ�䣺" + min + "��" + s + "��";

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
        levelText.text = "�ȼ�" + levelNuml;
    }
}
