using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//按键管理
public class HXW_InputMgr : HXW_BaseManager<HXW_InputMgr>
{
    //是否开启按键检测
    private bool isStart = false;

    //构造方法中，添加Update监听
    public HXW_InputMgr() 
    {
        //开启按键检测
        StartOrEndCheck(true);
        //添加按键检测事件
        HXW_MonoMgr.GetInstance().AddUpdateListener(MyUpdate);
    }
    //是否需要开启输入检测
    public void StartOrEndCheck(bool isOpen) 
    {
        isStart = isOpen;
    } 
    //按键检测
    private void MyUpdate() 
    {
        //没有开启输入检测，就不去检测
        if (!isStart)
        {
            return;
        }
        //Test();
    }
    public void Test()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("按键E");
            HXW_UIManager.GetInstance().ShowPanelAsync<LoadIngPanel>(LoadIngPanel.prefabsPath, PanelLayer.Top);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("按键P");
            HXW_UIManager.GetInstance().ShowPanelAsync<StartPanel>(StartPanel.prefabsPath, PanelLayer.Bot);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("按键O");
            HXW_UIManager.GetInstance().ShowPanelAsync<SettingPanel>(SettingPanel.prefabsPath, PanelLayer.Bot);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("按键I");
            HXW_UIManager.GetInstance().ShowPanelAsync<SuspendPanel>(SuspendPanel.prefabsPath, PanelLayer.Bot);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("按键U");
            HXW_UIManager.GetInstance().ShowPanelAsync<GameOverPanel_Win>(GameOverPanel_Win.prefabsPath, PanelLayer.Bot);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Debug.Log("按键U");
            HXW_UIManager.GetInstance().ShowPanelAsync<GameOverPanel_Fail>(GameOverPanel_Fail.prefabsPath, PanelLayer.Bot);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Debug.Log("按键T");
            HXW_UIManager.GetInstance().ShowPanelAsync<ChoosePanel>(ChoosePanel.prefabsPath, PanelLayer.Bot);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            //Debug.Log("按键T");

            Sprite SpriteA = HXW_ResMgr.GetInstance().Load<Sprite>("Character/西装暴徒");
            Sprite SpriteB = HXW_ResMgr.GetInstance().Load<Sprite>("Character/怪物");
            

            Speaker speakerA = new Speaker("西装暴徒", SpriteA);
            Speaker speakerB = new Speaker("怪物", SpriteB);

            List<TalkInfoElement> talkInfoElement = new List<TalkInfoElement>();

            TalkInfoElement talkInfoElement_1 = new TalkInfoElement(speakerA, "今天是个好日子~");
            TalkInfoElement talkInfoElement_2 = new TalkInfoElement(speakerB, "你在说什么");
            TalkInfoElement talkInfoElement_3 = new TalkInfoElement(speakerA, "听不懂就算了");
            TalkInfoElement talkInfoElement_4 = new TalkInfoElement(speakerB, "???");

            talkInfoElement.Add(talkInfoElement_1);
            talkInfoElement.Add(talkInfoElement_2);
            talkInfoElement.Add(talkInfoElement_3);
            talkInfoElement.Add(talkInfoElement_4);

            TalkInfo talkInfo = new TalkInfo(talkInfoElement);
            //HXW_UIManager.GetInstance().ShowPanelAsync<HXW_TalkPanel>(HXW_TalkPanel.prefabsPath, PanelLayer.Mid,null,(panel)=>
            //{
            //    panel.StartTalk(talkInfo);
            //});
        }
    }
}
