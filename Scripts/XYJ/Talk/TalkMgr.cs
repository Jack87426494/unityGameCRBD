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
    //��ҵ�����
    private string playerName = "��";
    //���ͷ��ͼƬ��λ��
    private string playerImgPath = "Role/Me";
    //��ҶԻ���
    private Speaker playerSpeaker;

    //npc������
    private string npcName = "�컪";
    //npcͷ��ͼƬ��λ��
    private string npcImgPath = "Role/Npc";
    //npc�Ի���
    private Speaker npcSpeaker;

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="info">˵��������</param>
    /// <param name="intervalTime">ÿ���ֳ��ֵļ��ʱ��</param>
    /// <param name="canBreakOrNext">�Ƿ�ɱ����</param>
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
    /// �Ի�
    /// </summary>
    /// <param name="info">�Ի�������</param>
    /// <param name="whoTalk">˭��˵����true��ʾ�����˵����false��ʾnpc��˵��</param>
    /// <param name="intervalTime">ÿ���ֳ��ֵļ��ʱ��</param>
    /// <param name="canBreakOrNext">�Ƿ�ɱ����</param>
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
