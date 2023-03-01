using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TalkMgr
{
    private static TalkMgr instance;
    public static TalkMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new TalkMgr();
            return instance;
        }
    }
    //玩家的名字
    private string playerName = "我";
    //玩家头像图片的位置
    private string playerImgPath = "Role/Me";
    //玩家对话类
    private Speaker playerSpeaker;

    //npc的名字
    private string npcName = "徐华";
    //npc头像图片的位置
    private string npcImgPath = "Role/Npc";
    //npc对话类
    private Speaker npcSpeaker;

    /// <summary>
    /// 玩家自言自语
    /// </summary>
    /// <param name="info">说话的内容</param>
    /// <param name="intervalTime">每个字出现的间隔时间</param>
    /// <param name="canBreakOrNext">是否可被打断</param>
    public void TalkSelf(string info,float intervalTime=0.1f,bool canBreakOrNext=false)
    {
        if(playerSpeaker==null)
        {
            playerSpeaker = new Speaker(playerName, HXW_ResMgr.GetInstance().Load<Sprite>(playerImgPath));
        }

        List<TalkInfoElement> talkInfoElement = new List<TalkInfoElement>();


        string sentence = "";

        foreach(char c in info)
        {
            if(c==';')
            {
                talkInfoElement.Add(new TalkInfoElement(playerSpeaker, sentence));
                sentence = "";
                break;
            }
            sentence += c;
        }
        
        TalkInfo talkInfo = new TalkInfo(talkInfoElement);

        HXW_UIManager.GetInstance().ShowPanelAsync<HXW_TalkPanel>(HXW_TalkPanel.prefabsPath, PanelLayer.Mid, null, (panel) =>
        {
            panel.StartTalk(talkInfo, intervalTime, canBreakOrNext);
        });
    }

    /// <summary>
    /// 对话
    /// </summary>
    /// <param name="info">对话的内容</param>
    /// <param name="whoTalk">谁先说话，true表示玩家先说话，false表示npc先说话</param>
    /// <param name="intervalTime">每个字出现的间隔时间</param>
    /// <param name="canBreakOrNext">是否可被打断</param>
    public void Talk(string info,bool whoTalk=false,float intervalTime = 0.1f, bool canBreakOrNext = false)
    {
        if (playerSpeaker == null)
        {
            playerSpeaker = new Speaker(playerName, HXW_ResMgr.GetInstance().Load<Sprite>(playerImgPath));
        }
        if(npcSpeaker==null)
        {
            npcSpeaker = new Speaker(npcName, HXW_ResMgr.GetInstance().Load<Sprite>(npcImgPath));
        }

        List<TalkInfoElement> talkInfoElement = new List<TalkInfoElement>();


        string sentence = "";


        foreach (char c in info)
        {
            if (c == ';')
            {
                if (whoTalk)
                {
                    talkInfoElement.Add(new TalkInfoElement(playerSpeaker, sentence));
                }
                else
                {
                    talkInfoElement.Add(new TalkInfoElement(npcSpeaker, sentence));
                }
                whoTalk = !whoTalk;
                sentence = "";
            }
            else
            {
                sentence += c;
            }
            
            
        }
            
        
        

        TalkInfo talkInfo = new TalkInfo(talkInfoElement);

        HXW_UIManager.GetInstance().ShowPanelAsync<HXW_TalkPanel>(HXW_TalkPanel.prefabsPath, PanelLayer.Mid, null, (panel) =>
        {
            panel.StartTalk(talkInfo, intervalTime, canBreakOrNext);
        });
    }
}
