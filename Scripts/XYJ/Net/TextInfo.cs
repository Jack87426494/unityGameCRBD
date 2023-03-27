using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

public class TextInfo : BaseData
{
    public short lev=5;
    public float atk=15;
    public string textName="唐老师";
    public int hp = 100;
    public Player_Data playerData = new Player_Data();

    public override int GetBytesNum()
    {
        return 2 +
            4 +
            4 +
            Encoding.UTF8.GetBytes(textName).Length +
            4 +
            playerData.GetBytesNum();
    }

    private Byte[] bytes;
    private int index;
    public override byte[] Writing()
    {
        index = 0;
        bytes = new byte[GetBytesNum()];
        WriteShort(bytes, lev,ref index);
        WriteFloat(bytes, atk, ref index);
        WriteString(bytes, textName, ref index);
        WriteInt(bytes, hp, ref index);
        WriteData(bytes, playerData, ref index);
        return bytes;
    }

    public override int Reading(Byte[] bytes,int beginIndex=0)
    {
        
        lev=ReadShort(bytes, ref beginIndex);
        atk=ReadFloat(bytes,ref beginIndex);
        ReadString(bytes,ref textName, ref beginIndex);
        ReadInt(bytes, ref hp, ref beginIndex);
        ReadData(bytes, playerData, ref beginIndex);
        return beginIndex;
    }
}
public class Player_Data:BaseData
{
    public string playerName="大明";
    public int age = 66;

    public override int GetBytesNum()
    {
        return 4 +
            Encoding.UTF8.GetBytes(playerName).Length +
            4;
    }

    private int index;
    private Byte[] bytes;
    public override byte[] Writing()
    {
        index = 0;
        bytes = new byte[GetBytesNum()];
        WriteString(bytes, playerName, ref index);
        WriteInt(bytes, age, ref index);
        return bytes;
    }

    public override int Reading(Byte[] bytes,int beginIndex=0)
    {
        ReadString(bytes,ref playerName,ref beginIndex);
        ReadInt(bytes, ref age, ref beginIndex);
        return beginIndex;
    }
}
