using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class BaseData
{
    public abstract int GetBytesNum();

    public abstract byte[] Writing();

    public abstract int Reading(Byte[] bytes,int beginIndex=0);

    /// <summary>
    /// –¥»Îshort
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="value"></param>
    /// <param name="index"></param>
    protected void WriteShort(byte[] bytes, short value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += 2;
    }

    protected short ReadShort(byte[] bytes,ref int index)
    {
        short ret=BitConverter.ToInt16(bytes,index);
        index += 2;
        return ret;
    }

    protected void WriteFloat(byte[] bytes, float value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += 4;
    }

    protected float ReadFloat(byte[] bytes,ref int index)
    {
        float ret=BitConverter.ToSingle(bytes, index);
        index += 4;
        return ret;
    }

    byte[] stringBytes;
    int strLength;
    protected void WriteString(byte[] bytes, string value, ref int index)
    {
        stringBytes = Encoding.UTF8.GetBytes(value);
        strLength = stringBytes.Length;
        WriteInt(bytes, strLength,ref index);
        stringBytes.CopyTo(bytes, index);
        index += strLength;
    }

    
    protected void ReadString(byte[] bytes,ref string value,ref int index)
    {
        ReadInt(bytes,ref strLength,ref index);
        value = Encoding.UTF8.GetString(bytes, index, strLength);
        index += strLength;
    }

    protected void WriteInt(byte[] bytes, int value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += 4;
    }

    protected void ReadInt(byte[] bytes,ref int value,ref int index)
    {
        value = BitConverter.ToInt32(bytes, index);
        index += 4;
    }

    protected void WriteBool(byte[] bytes,ref bool value,ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes,index);
        index += 1;
    }

    //protected void WritePlayer(byte[] bytes,PlayerData playerData,ref int index)
    //{
    //    WriteString(bytes, playerData.name,ref index);
    //    WriteInt(bytes, playerData.age, ref index);

    //}

    protected void WriteData(byte[] bytes,BaseData baseData,ref int index)
    {
        baseData.Writing().CopyTo(bytes,index);
        index += baseData.GetBytesNum();
    }

    protected void ReadData(byte[] bytes,BaseData baseData,ref int index)
    {
        index = baseData.Reading(bytes,index);
    }
}
