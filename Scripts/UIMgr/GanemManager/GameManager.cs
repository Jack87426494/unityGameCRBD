using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //HXW_InputMgr.GetInstance();//º§ªÓ∞¥º¸ ‰»Î
        //HXW_UIManager.GetInstance().ShowPanelAsync<GamePanel>(GamePanel.prefabsPath, PanelLayer.Bot);
        HXW_UIManager.GetInstance().ShowPanelAsync<StartPanel>(StartPanel.prefabsPath, PanelLayer.Bot);
        //MusicMgr.Instance.PlayBkAudioSource("Music/begin");
        DontDestroyOnLoad(this);
    }

    public void ChangeScene(string sceneName,UnityAction action = null)
    {

        
        //EventMgr.Instance.ClearEventDic();
        ScenesMgr.Instance.LoadScene(sceneName);
       
        //ScenesMgr.Instance.LoadSceneAsyn(sceneName, action);
            action?.Invoke();
        ObjectPoolMgr.Instance.ClearObjectPool();
    }
}
