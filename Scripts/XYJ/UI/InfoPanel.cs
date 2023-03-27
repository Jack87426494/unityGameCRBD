using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InfoPanel : HXW_MapPanel
{
    public UnityAction LookOverAction;
    public static string prefabsPath = "InfoPanel";
    private Text fileText;
    private Button btn;
    private int fileIndex;
    public override void ShowMeFirst()
    {
        fileText = GetPanel("Button/Text").GetUI<Text>();
        btn = GetPanel("Button").GetUI<Button>();
        btn.onClick.AddListener(() =>
        {
            HXW_UIManager.Instance.HidePanel(prefabsPath);
        });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            HXW_UIManager.Instance.HidePanel(prefabsPath);
        }
    }

    public void SetInfo(string info,int fileIndex=0)
    {
        fileText.text = info;
        this.fileIndex = fileIndex;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
        BasePlayerController.Instance.isDead = false;
        
        switch (fileIndex)
        {
            case 1:
                TalkMgr.Instance.TalkSelf("难道，这里在做人体实验！我，我被接种了病毒？！;");
                break;
            case 2:
                TalkMgr.Instance.TalkSelf("日记的主人是邪教徒吗？这和人体实验有什么关系？;");
                break;
            case 3:
                TalkMgr.Instance.TalkSelf("这具尸体为什么和我长得一样？;");
                break;
            case 4:
                TalkMgr.Instance.TalkSelf("我一定要将这些信息带出去。;");
                break;
        }
        
        LookOverAction?.Invoke();
        MusicMgr.Instance.PlaySound("Music/Audio/Paper3");
    }

    private void OnEnable()
    {
        MusicMgr.Instance.PlaySound("Music/Audio/Paper1");
        //Time.timeScale = 0f;
    }
}
