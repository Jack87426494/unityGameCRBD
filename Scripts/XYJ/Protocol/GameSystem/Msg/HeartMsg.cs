using System;
using System.Text;
using System.Collections.Generic;
namespace GameSystem
{
	public class HeartMsg:BaseMsg
	{
		public override int GetBytesNum()
		{
			int num = 0;
			num += 8;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			int bytesNum = GetBytesNum();
			byte[] bytes = new byte[GetBytesNum()];
			WriteInt(bytes, GetId(), ref index);
			WriteInt(bytes, bytesNum - 8, ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			return index - beginIndex;
		}
		public override int GetId()
		{
			return 1002;
		}
	}
}