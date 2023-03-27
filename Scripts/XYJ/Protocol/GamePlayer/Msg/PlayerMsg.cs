using System;
using System.Text;
using System.Collections.Generic;
namespace GamePlayer
{
	public class PlayerMsg:BaseMsg
	{
		public int playerID;
		public PlayerData data;
		public override int GetBytesNum()
		{
			int num = 0;
			num += 8;
			num += 4;
			num += data.GetBytesNum();
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			int bytesNum = GetBytesNum();
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, GetId(), ref index);
			WriteInt(bytes, bytesNum - 8, ref index);
			WriteInt(bytes, playerID, ref index);
			WriteData(bytes, data, ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			playerID = ReadInt(bytes, ref index);
			data = ReadData<PlayerData>(bytes, ref index);
			return index - beginIndex;
		}
		public override int GetId()
		{
			return 1001;
		}
	}
}