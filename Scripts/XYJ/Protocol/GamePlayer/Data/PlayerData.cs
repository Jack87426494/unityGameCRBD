using System;
using System.Text;
using System.Collections.Generic;
namespace GamePlayer
{
	public class PlayerData:BaseData
	{
		public int id;
		public float atk;
		public bool sex;
		public long lev;
		public int[] arrays;
		public List<int> list;
		public Dictionary<int,string> dic;
		public E_HERO_TYPE heroType;
		public override int GetBytesNum()
		{
			int num = 0;
			num += 4;
			num += 4;
			num += 1;
			num += 8;
			num += 2;
			for (int i = 0; i < arrays.Length; ++i)
				num += 4;
			num += 2;
			for (int i = 0; i < list.Count; i++)
				num += 4;
			num += 2;
			foreach (int key in dic.Keys)
			{
				num += 4;
				num += 4 + Encoding.UTF8.GetByteCount(dic[key]);
			}
			num += 4;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, id, ref index);
			WriteFloat(bytes, atk, ref index);
			WriteBool(bytes, sex, ref index);
			WriteLong(bytes, lev, ref index);
			WriteShort(bytes, (short)arrays.Length, ref index);
			for (int i = 0; i < arrays.Length; ++i)
				WriteInt(bytes, arrays[i], ref index);
			WriteShort(bytes, (short)list.Count, ref index);
			for (int i = 0; i < list.Count; ++i)
				WriteInt(bytes, list[i], ref index);
			WriteShort(bytes, (short)dic.Count, ref index);
			foreach (int key in dic.Keys)
			{
				WriteInt(bytes, key, ref index);
				WriteString(bytes, dic[key], ref index);
			}
			WriteInt(bytes, Convert.ToInt32(heroType), ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			id = ReadInt(bytes, ref index);
			atk = ReadFloat(bytes, ref index);
			sex = ReadBool(bytes, ref index);
			lev = ReadLong(bytes, ref index);
			short arraysLength = ReadShort(bytes, ref index);
			arrays = new int[arraysLength];
			for (int i = 0; i < arraysLength; ++i)
				arrays[i] = ReadInt(bytes, ref index);
			list = new List<int>();
			short listCount = ReadShort(bytes, ref index);
			for (int i = 0; i < listCount; ++i)
				list.Add(ReadInt(bytes, ref index));
			dic = new Dictionary<int, string>();
			short dicCount = ReadShort(bytes, ref index);
			for (int i = 0; i < dicCount; ++i)
				dic.Add(ReadInt(bytes, ref index), ReadString(bytes, ref index));
			heroType = (E_HERO_TYPE)ReadInt(bytes, ref index);
			return index - beginIndex;
		}
	}
}