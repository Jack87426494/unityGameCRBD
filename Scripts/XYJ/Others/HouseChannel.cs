using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseChannel : VerticalChannel
{
    private bool isSay;
    private void Awake()
    {
        isSay = false;
    }

    protected override void Update()
    {
        base.Update();
        if (GameDataMgr.Instance.CheackKeyItem("CarKey")&&!isSay)
        {
            TalkMgr.Instance.TalkSelf("��ʲô�����ӳ����ˡ�;");
            MusicMgr.Instance.PlaySound("Music/Audio/Scare1",isLoop:false);
            isSay = true;
        }
    }
}
