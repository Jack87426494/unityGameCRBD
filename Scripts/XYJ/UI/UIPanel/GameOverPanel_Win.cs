using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel_Win : HXW_MapPanel
{
    public static string prefabsPath = "GameOverPanel_Win";//避免后期文件路径改变，那时，只需要改变这里的路径即可

    private Button btnBack;
    private Button btnAgain;
    private Button btnQuit;
    private Text txt;
    private CanvasGroup canvasGroup;

    private CanvasGroup textCanvasGroup;
    public override void ShowMeFirst()
    {
        //btnBack = GetPanel("btnBack").GetUI<Button>();
        //btnBack.onClick.AddListener(() => { 
        //    MusicMgr.Instance.PlaySound("Music/Audio/Click1");
        //    GameManager.Instance.ChangeScene("PanelScene", () =>
        //    {
        //        HXW_UIManager.GetInstance().HidePanel(GameOverPanel_Win.prefabsPath);

        //    });
        //});

        //btnAgain = GetPanel("btnAgain").GetUI<Button>();
        //btnAgain.onClick.AddListener(() => { Debug.Log("再次挑战"); });
        
        btnQuit = GetPanel("btnQuit").GetUI<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
        txt = GetPanel("Text").GetUI<Text>();
        textCanvasGroup = txt.gameObject.GetComponent<CanvasGroup>();
        StartCoroutine("StartDisplay");
        btnQuit.onClick.AddListener(() => { Debug.Log("返回主页");
            
            GameManager.Instance.ChangeScene("PanelScene", () =>
            {
                EventMgr.Instance.EventTrigger("ReSetData");
                HXW_UIManager.Instance.HidePanel(GamePanel.prefabsPath);
                HXW_UIManager.GetInstance().HidePanel(GameOverPanel_Win.prefabsPath);
                
                MusicMgr.Instance.PlaySound("Music/Audio/Click1");
            });
        });
    }
    private void OnEnable()
    {
        MusicMgr.Instance.PlaySound("Music/Audio/Keyboard");
        
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
        Destroy(BasePlayerController.Instance.gameObject);
        MusicMgr.Instance.StopAllSounds();
    }

    IEnumerator StartDisplay()
    {
        canvasGroup.alpha = 0f;
        textCanvasGroup.alpha = 0f;
        btnQuit.gameObject.SetActive(false);
        while (canvasGroup.alpha<1)
        {
            canvasGroup.alpha += Time.deltaTime*0.2f;
            yield return null;
        }
        while(textCanvasGroup.alpha<1)
        {
            textCanvasGroup.alpha += Time.deltaTime * 0.2f;
            yield return null;
        }
        btnQuit.gameObject.SetActive(true);
        StopCoroutine(StartDisplay());
    }
}
