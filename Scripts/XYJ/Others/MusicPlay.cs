using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Muisc
{
    battle,
    Room,
    Begin
}
public class MusicPlay : MonoBehaviour
{
    [Header("���������ŵ�����")]
    public Muisc muisc;
    private void Start()
    {
        switch (muisc)
        {
            case Muisc.battle:
                MusicMgr.Instance.PlayBkAudioSource("Music/battle");
                break;
            case Muisc.Room:
                MusicMgr.Instance.PlayBkAudioSource("Music/Room");
                break;
            case Muisc.Begin:
                MusicMgr.Instance.PlayBkAudioSource("Music/begin");
                break;
        }
        
    }
}
