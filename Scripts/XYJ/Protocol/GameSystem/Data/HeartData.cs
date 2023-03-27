using System;
using System.Text;
using System.Collections.Generic;
namespace GameSystem
{
	public class HeartData:BaseData
	{
		public long time;
		public override int GetBytesNum()
		{
			int num = 0;
			num += 8;
			return num;
		}
		public override byte[] Writing()
		{
			int index = 0;
			byte[] bytes = new byte[GetBytesNum()];
			WriteLong(bytes, time, ref index);
			return bytes;
		}
		public override int Reading(byte[] bytes, int beginIndex = 0)
		{
			int index = beginIndex;
			time = ReadLong(bytes, ref index);
			return index - beginIndex;
		}
	}
}