using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HXW_TalkPanel : HXW_MapPanel
{
    public static string prefabsPath = "TalkPanel";

    private Button clickButton;
    private Image speakerImage;
    private Text speakerName;
    private Text contentText;

    private int talkInfoElemtIndex = 0;
    private bool isSpeaking = false;
    private bool isBreakSpeak = false;
    private TalkInfo talkInfo;
    private float intervalTime;

    private bool canBreakOrNext;

    private bool isAutoNext;
    private bool isStartWait;
    private float autoNextWaitTime = 1;
    private float currentAutoNextWaitTime = 0;
    public override void ShowMeFirst()
    {
        clickButton = ThisPanel.GetUI<Button>();
        clickButton.onClick.AddListener(() =>
        {
            BreakOrNext();
        });

        speakerImage = GetPanel("Content/SpeakerImage").GetUI<Image>();
        speakerName = GetPanel("Content/SpeakerName").GetUI<Text>();
        contentText = GetPanel("Content/Text").GetUI<Text>();
    }
    public void OpenNextWords()
    {
        isAutoNext = true;
    }
    public void CloseNextWords()
    {
        isAutoNext = false;
    }
    private void Update()
    {
        if (isAutoNext == false)
            return;
        if (CheckEnd() == true)
            return;
        if (isSpeaking == true)
        {
            currentAutoNextWaitTime = 0;
            return;
        }
        currentAutoNextWaitTime += Time.deltaTime;
        if (currentAutoNextWaitTime >= autoNextWaitTime)
        {
            BreakOrNext();
            currentAutoNextWaitTime = 0;
        }
    }

    public void StartTalk(TalkInfo talkInfo, float intervalTime = 0.1f,bool canBreakOrNext=true)
    {
        this.talkInfoElemtIndex = 0;
        this.isSpeaking = false;
        this.isBreakSpeak = false;
        this.talkInfo = talkInfo;
        this.SpeakerHandler();
        this.intervalTime = intervalTime;
        this.canBreakOrNext = canBreakOrNext;
    }

    private bool CheckTalkInfo()
    {
        if (talkInfo == null || talkInfo.GetTalkInfos() == null || talkInfo.GetTalkInfos().Count <= 0)
        {
            Debug.Log("信息有误");
            return false;
        }
        return true;
    }

    private bool CheckEnd()
    {
        if (CheckTalkInfo() == false)
            return true;

        if (this.talkInfoElemtIndex >= talkInfo.GetTalkInfos().Count - 1)//对话结束
        {
            HXW_UIManager.GetInstance().HidePanel(HXW_TalkPanel.prefabsPath);
            return true;
        }
        return false;
    }

    private void BreakOrNext()
    {
        
        if (isSpeaking == true)
        {
            if (!canBreakOrNext)
                return;
            isBreakSpeak = true;
            return;
        }
        if (CheckEnd() == true)
            return;

        this.talkInfoElemtIndex++;
        this.SpeakerHandler();
    }

    public void SpeakerHandler()
    {
        if (CheckTalkInfo() == false)
            return;

        TalkInfoElement talkInfoElement = talkInfo.GetTalkInfos()[talkInfoElemtIndex];
        Speak(talkInfoElement.GetSpeaker(), talkInfoElement.GetContent());
    }

    public void Speak(Speaker speaker, string content)
    {
        this.speakerImage.sprite = speaker.GetSpeakerSprite();
        this.speakerName.text = speaker.GetSpeakerName();
        this.StartCoroutine(DoSpeak(content));
    }

    IEnumerator DoSpeak(string content)
    {
        contentText.text = "";
        isSpeaking = true;
        for (int i = 0; i < content.Length; i++)
        {
            contentText.text += content[i];
            float time = intervalTime;
            yield return new WaitUntil(() =>
            {
                time -= Time.deltaTime;

                if (time <= 0 || isBreakSpeak == true)
                    return true;
                return false;
            });
            if (isBreakSpeak == true)
            {
                contentText.text = content;
                isBreakSpeak = false;
                break;
            }
        }
        isSpeaking = false;
       
    }
}

public class Speaker
{
    private string speakerName;
    private Sprite speakerSprite;

    public Speaker(string speakerName, Sprite speakerSprite)
    {
        this.speakerName = speakerName;
        this.speakerSprite = speakerSprite;
    }
    public string GetSpeakerName()
    {
        return speakerName;
    }
    public Sprite GetSpeakerSprite()
    {
        return speakerSprite;
    }
}

public class TalkInfoElement
{
    public TalkInfoElement(Speaker speaker, string content)
    {
        this.speaker = speaker;
        this.content = content;
    }

    public Speaker speaker;
    public string content;

    public Speaker GetSpeaker()
    {
        return speaker;
    }
    public string GetContent()
    {
        return content;
    }
}

public class TalkInfo
{
    public TalkInfo(List<TalkInfoElement> talkInfos)
    {
        this.talkInfos = talkInfos;
    }

    public List<TalkInfoElement> GetTalkInfos()
    {
        return talkInfos;
    }
    private List<TalkInfoElement> talkInfos = new List<TalkInfoElement>();
}

