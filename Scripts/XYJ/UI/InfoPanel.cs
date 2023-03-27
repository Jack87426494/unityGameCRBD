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
                TalkMgr.Instance.TalkSelf("�ѵ���������������ʵ�飡�ң��ұ������˲�������;");
                break;
            case 2:
                TalkMgr.Instance.TalkSelf("�ռǵ�������а��ͽ���������ʵ����ʲô��ϵ��;");
                break;
            case 3:
                TalkMgr.Instance.TalkSelf("���ʬ��Ϊʲô���ҳ���һ����;");
                break;
            case 4:
                TalkMgr.Instance.TalkSelf("��һ��Ҫ����Щ��Ϣ����ȥ��;");
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
