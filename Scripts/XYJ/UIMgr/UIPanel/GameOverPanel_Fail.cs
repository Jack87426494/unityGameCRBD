using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel_Fail : HXW_MapPanel
{
    public static string prefabsPath = "GameOverPanel_Fail";//��������ļ�·���ı䣬��ʱ��ֻ��Ҫ�ı������·������

    private Button btnBack;
    private Button btnAgain;
    private Button btnQuit;

    public override void ShowMeFirst()
    {

        
        btnBack = GetPanel("btnBack").GetUI<Button>();
        btnBack.onClick.AddListener(() => {
            MusicMgr.Instance.PlaySound("Music/Audio/Click1");
            
            GameManager.Instance.ChangeScene("PanelScene", () =>
            {
                HXW_UIManager.GetInstance().HidePanel(BossBloodPanel.prefabsPath);
                HXW_UIManager.GetInstance().HidePanel(GameOverPanel_Fail.prefabsPath);
                //MusicMgr.Instance.PlayBkAudioSource("Music/begin");
            });
        });

        btnAgain = GetPanel("btnAgain").GetUI<Button>();
        btnAgain.onClick.AddListener(() => { Debug.Log("�ٴ���ս"); });

        btnQuit = GetPanel("btnQuit").GetUI<Button>();
        btnQuit.onClick.AddListener(() => { Debug.Log("������ҳ");
            MusicMgr.Instance.PlaySound("Music/Audio/Click1");
            
            GameManager.Instance.ChangeScene("PanelScene", () =>
            {
                HXW_UIManager.GetInstance().HidePanel(BossBloodPanel.prefabsPath);
                HXW_UIManager.GetInstance().HidePanel(GameOverPanel_Fail.prefabsPath);
                //MusicMgr.Instance.PlayBkAudioSource("Music/begin");
            });
            
        });
    }

    private void OnEnable()
    {
        
        
        Cursor.visible = true;
    }
    private void OnDisable()
    {
        Destroy(BasePlayerController.Instance.gameObject);
        Time.timeScale = 1f;
        
    }
}
