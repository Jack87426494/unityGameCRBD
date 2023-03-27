using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuspendPanel : HXW_MapPanel
{
    public static string prefabsPath = "SuspendPanel";

    private Button btnBack;

    private Slider sliderMusic;
    private Slider sliderSound;
    private Toggle togMuisc;
    private Toggle togSound;
    private Button btnSure;
    private Button btnQuit;


    public override void ShowMeFirst()
    {
        

        btnBack = GetPanel("btnBack").GetUI<Button>();
        btnBack.onClick.AddListener(() => {
            MusicMgr.Instance.PlaySound("Music/Audio/Click3");
            HXW_UIManager.GetInstance().HidePanel(SuspendPanel.prefabsPath); });

        sliderMusic = GetPanel("sliderMusic").GetUI<Slider>();
        sliderMusic.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.UpdateBkVolume(value);
        });
        sliderSound = GetPanel("sliderSound").GetUI<Slider>();
        sliderSound.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.UpdateSoundVoluem(value);
        });

        togMuisc = GetPanel("togMuisc").GetUI<Toggle>();
        togMuisc.onValueChanged.AddListener((isOpen) =>
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Click3");
            GameDataMgr.Instance.UpdateBkOpen(isOpen);
        });
        togSound = GetPanel("togSound").GetUI<Toggle>();
        togSound.onValueChanged.AddListener((isOpen) =>
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Click3");
            GameDataMgr.Instance.UpdateSoundOpen(isOpen);
        });

        btnSure = GetPanel("btnSure").GetUI<Button>();
        btnSure.onClick.AddListener(() => {
            MusicMgr.Instance.PlaySound("Music/Audio/Click4");
            GameDataMgr.Instance.SaveMusicData();
            HXW_UIManager.GetInstance().HidePanel(SuspendPanel.prefabsPath); });

        btnQuit = GetPanel("btnQuit").GetUI<Button>();
        btnQuit.onClick.AddListener(() => { Debug.Log("返回主页");
            MusicMgr.Instance.PlaySound("Music/Audio/Click3");
            GameDataMgr.Instance.SaveMusicData();
            
            //SceneManager.LoadScene("PanelScene");
            GameManager.Instance.ChangeScene("PanelScene", () =>
            {
                EventMgr.Instance.EventTrigger("ReSetData");
                HXW_UIManager.GetInstance().ShowPanelAsync<StartPanel>(StartPanel.prefabsPath, PanelLayer.Bot);
                HXW_UIManager.GetInstance().HidePanel(SuspendPanel.prefabsPath);
                HXW_UIManager.GetInstance().HidePanel(GamePanel.prefabsPath);
                Destroy(BasePlayerController.Instance.gameObject);
                //MusicMgr.Instance.PlayBkAudioSource("Music/begin");
            });
            
        });
        initValue();
    }
    private MusicData musciData;

    //初始化面板数据
    private void initValue()
    {
        musciData = GameDataMgr.Instance.musicData;
        sliderMusic.value = musciData.bKVoluem;
        sliderSound.value = musciData.soundVoluem;
        togMuisc.isOn = musciData.isOpenBk;
        togSound.isOn = musciData.isOpenSound;
    }

    private void OnEnable()
    {
        HXW_UIManager.Instance.HidePanel(UITipPanel.prefabsPath);
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
