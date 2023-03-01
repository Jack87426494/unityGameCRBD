using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : HXW_MapPanel
{
    public static string prefabsPath = "SettingPanel";

    private Button btnQuit;

    private Slider sliderMusic;
    private Slider sliderSound;
    private Toggle togMuisc;
    private Toggle togSound;
    private Button btnSure;

    public override void ShowMeFirst()
    {
        btnQuit = GetPanel("btnQuit").GetUI<Button>();
        btnQuit.onClick.AddListener(() => {
            MusicMgr.Instance.PlaySound("Music/Audio/Click3");
            HXW_UIManager.GetInstance().HidePanel(SettingPanel.prefabsPath);
            GameDataMgr.Instance.SaveMusicData();
            HXW_UIManager.GetInstance().ShowPanelAsync<StartPanel>(StartPanel.prefabsPath);
        });

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
            HXW_UIManager.GetInstance().HidePanel(SettingPanel.prefabsPath);
            GameDataMgr.Instance.SaveMusicData();
            HXW_UIManager.GetInstance().ShowPanelAsync<StartPanel>(StartPanel.prefabsPath);
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
}
