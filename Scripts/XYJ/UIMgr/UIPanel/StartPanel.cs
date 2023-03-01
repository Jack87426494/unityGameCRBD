using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanel : HXW_MapPanel
{
    //注意路径是从   Resource/UI/    后开始
    public static string prefabsPath = "StartPanel";//避免后期文件路径改变，那时，只需要改变这里的路径即可

    public override void ShowMeFirst()
    {
        //MusicMgr.Instance.PlayBkAudioSource("Music/begin");

        Button startButton = GetPanel("Main/Back/btnStart").GetUI<Button>();
        Button continueButton = GetPanel("Main/Back/btnSetting").GetUI<Button>();
        Button signOutButton = GetPanel("Main/Back/btnQuit").GetUI<Button>();


        startButton.onClick.AddListener(() => 
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Click");
            //Debug.Log("开始游戏");

            HXW_UIManager.GetInstance().HidePanel(StartPanel.prefabsPath);

            //创建加载面板
            HXW_UIManager.GetInstance().ShowPanelAsync<LoadIngPanel>(LoadIngPanel.prefabsPath, PanelLayer.Max, (LoadIngPanel) =>
             {
                 LoadIngPanel.LoadFirst();
             });

            //场景切换
            ScenesMgr.Instance.LoadSceneAsyn("StartRoom", () =>
             {

             });


            //MusicMgr.Instance.PlayBkAudioSource("Music/Room");
            HXW_UIManager.GetInstance().ShowPanelAsync<GamePanel>(GamePanel.prefabsPath, PanelLayer.Bot);

            if (SkillManager.Instance != null)
            {
                Destroy(SkillManager.Instance.gameObject);
                
            }

            if (HXW_UIManager.GetInstance().GetNewPanel("SkillPanel")!=null)
            {
                HXW_UIManager.GetInstance().DestroyPanel("SkillPanel");
                Instantiate(Resources.Load<GameObject>("Mgr/SkillManager"));
            }
            //if (GameObject.Find("SkillPanel") != null)
            //{
            //    GameObject.Find("SkillPanel").GetComponent<SkillPanel>().activatedSkills.Clear();
            //    foreach (SkillData skillData in SkillManager.Instance.initialskills)
            //    {
            //        GameObject.Find("SkillPanel").GetComponent<SkillPanel>().activatedSkills.Add(Instantiate(skillData));
            //    }
            //}
            //GameManager.Instance.ChangeScene("GameScene", () =>
            //{
            //    MusicMgr.Instance.PlayBkAudioSource("Music/battle");

            //    HXW_UIManager.GetInstance().ShowPanelAsync<GamePanel>(GamePanel.prefabsPath, PanelLayer.Bot);
            //});

        });
        continueButton.onClick.AddListener(() => 
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Click1");
            Debug.Log("设置游戏");
            HXW_UIManager.GetInstance().HidePanel(StartPanel.prefabsPath);
            HXW_UIManager.GetInstance().ShowPanelAsync<SettingPanel>(SettingPanel.prefabsPath);
        });
        signOutButton.onClick.AddListener(() => 
        {
            MusicMgr.Instance.PlaySound("Music/Audio/Click1");
            Debug.Log("退出游戏");
            HXW_UIManager.GetInstance().HidePanel(StartPanel.prefabsPath);
            Application.Quit();
        });
        
    }

    private void OnEnable()
    {
        //HXW_UIManager.GetInstance().HidePanel(LoadIngPanel.prefabsPath);
        
    }

}
