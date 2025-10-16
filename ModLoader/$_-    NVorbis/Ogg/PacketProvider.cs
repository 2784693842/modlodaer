using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x02000096 RID: 150
	internal class PacketProvider : IPacketProvider, IPacketReader
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000AF34 File Offset: 0x00009134
		public bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0001344E File Offset: 0x0001164E
		public int StreamSerial { get; }

		// Token: 0x060003AA RID: 938 RVA: 0x00013456 File Offset: 0x00011656
		internal PacketProvider(IStreamPageReader reader, int streamSerial)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._reader = reader;
			this.StreamSerial = streamSerial;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001347C File Offset: 0x0001167C
		public long GetGranuleCount()
		{
			if (this._reader == null)
			{
				throw new ObjectDisposedException("PacketProvider");
			}
			if (!this._reader.HasAllPages)
			{
				long num;
				bool flag;
				bool flag2;
				bool flag3;
				int num2;
				int num3;
				this._reader.GetPage(int.MaxValue, out num, out flag, out flag2, out flag3, out num2, out num3);
			}
			return this._reader.MaxGranulePosition.Value;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000134DA File Offset: 0x000116DA
		public IPacket GetNextPacket()
		{
			return this.GetNextPacket(ref this._pageIndex, ref this._packetIndex);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000134F0 File Offset: 0x000116F0
		public IPacket PeekNextPacket()
		{
			int pageIndex = this._pageIndex;
			int packetIndex = this._packetIndex;
			return this.GetNextPacket(ref pageIndex, ref packetIndex);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00013518 File Offset: 0x00011718
		public long SeekTo(long granulePos, int preRoll, GetPacketGranuleCount getPacketGranuleCount)
		{
			if (this._reader == null)
			{
				throw new ObjectDisposedException("PacketProvider");
			}
			int pageIndex = this._reader.FindPage(granulePos);
			int packetIndex = this.FindPacket(pageIndex, preRoll, ref granulePos, getPacketGranuleCount);
			if (!this.NormalizePacketIndex(ref pageIndex, ref packetIndex))
			{
				throw new ArgumentOutOfRangeException("granulePos");
			}
			this._lastPacket = null;
			this._pageIndex = pageIndex;
			this._packetIndex = packetIndex;
			return granulePos;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00013580 File Offset: 0x00011780
		[return: TupleElementNames(new string[]
		{
			"lastPageGranulePos",
			"lastPagePacketLength",
			"firstRealPacket"
		})]
		private ValueTuple<long, int, int> GetPreviousPageInfo(int pageIndex, GetPacketGranuleCount getPacketGranuleCount)
		{
			if (pageIndex <= 0)
			{
				return new ValueTuple<long, int, int>(0L, 0, 0);
			}
			long num;
			bool flag;
			bool flag2;
			bool flag3;
			int num2;
			int num3;
			if (this._reader.GetPage(pageIndex - 1, out num, out flag, out flag2, out flag3, out num2, out num3))
			{
				int num5;
				if (pageIndex > this._reader.FirstDataPageIndex)
				{
					pageIndex--;
					int num4 = num2 - 1;
					Packet packet = this.CreatePacket(ref pageIndex, ref num4, false, 0L, false, flag3, num2, 0);
					if (packet == null)
					{
						throw new InvalidDataException("Could not find end of continuation!");
					}
					num5 = getPacketGranuleCount(packet);
				}
				else
				{
					num5 = 0;
				}
				return new ValueTuple<long, int, int>(num, num5, flag3 ? 1 : 0);
			}
			throw new InvalidDataException("Could not get preceding page?!");
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00013618 File Offset: 0x00011818
		[return: TupleElementNames(new string[]
		{
			"gps",
			"endGP"
		})]
		private ValueTuple<long[], long> GetTargetPageInfo(int pageIndex, int firstRealPacket, int lastPagePacketLength, GetPacketGranuleCount getPacketGranuleCount)
		{
			long num;
			bool flag;
			bool flag2;
			bool flag3;
			int num2;
			int num3;
			if (!this._reader.GetPage(pageIndex, out num, out flag, out flag2, out flag3, out num2, out num3))
			{
				throw new InvalidDataException("Could not get found page?!");
			}
			if (flag3)
			{
				num2--;
			}
			long[] array = new long[num2];
			long num4 = num;
			for (int i = num2 - 1; i >= firstRealPacket; i--)
			{
				array[i] = num4;
				Packet packet = this.CreatePacket(ref pageIndex, ref i, false, num, i == 0 && flag, flag3, num2, 0);
				if (packet == null)
				{
					throw new InvalidDataException("Could not find end of continuation!");
				}
				num4 -= (long)getPacketGranuleCount(packet);
			}
			if (firstRealPacket == 1)
			{
				array[0] = num4;
				num4 -= (long)lastPagePacketLength;
			}
			return new ValueTuple<long[], long>(array, num4);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000136CC File Offset: 0x000118CC
		private int FindPacket(int pageIndex, long[] gps, long endGP, long lastPageGranulePos, int lastPagePacketLength, ref long granulePos)
		{
			if (endGP != lastPageGranulePos)
			{
				long num = endGP - lastPageGranulePos;
				if (this.GetIsVorbisBugDiff(num))
				{
					if (num > 0L)
					{
						if (granulePos <= endGP)
						{
							granulePos = endGP - (long)lastPagePacketLength;
							return -1;
						}
					}
					else
					{
						for (int i = 0; i < gps.Length; i++)
						{
							gps[i] -= num;
						}
					}
				}
				else if (pageIndex > this._reader.FirstDataPageIndex)
				{
					throw new InvalidDataException(string.Format("GranulePos mismatch: Page {0}, expected {1}, calculated {2}", pageIndex, lastPageGranulePos, endGP));
				}
			}
			for (int j = 0; j < gps.Length; j++)
			{
				if (gps[j] >= granulePos)
				{
					if (j == 0)
					{
						granulePos = endGP;
					}
					else
					{
						granulePos = gps[j - 1];
					}
					return j;
				}
			}
			throw new InvalidDataException("Could not find seek packet?!");
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00013784 File Offset: 0x00011984
		private int FindPacket(int pageIndex, int preRoll, ref long granulePos, GetPacketGranuleCount getPacketGranuleCount)
		{
			ValueTuple<long, int, int> previousPageInfo = this.GetPreviousPageInfo(pageIndex, getPacketGranuleCount);
			long item = previousPageInfo.Item1;
			int item2 = previousPageInfo.Item2;
			int item3 = previousPageInfo.Item3;
			ValueTuple<long[], long> targetPageInfo = this.GetTargetPageInfo(pageIndex, item3, item2, getPacketGranuleCount);
			long[] item4 = targetPageInfo.Item1;
			long item5 = targetPageInfo.Item2;
			int num = this.FindPacket(pageIndex, item4, item5, item, item2, ref granulePos);
			if (item5 > 0L || num > 1)
			{
				num -= preRoll;
			}
			return num;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x000137EC File Offset: 0x000119EC
		private bool GetIsVorbisBugDiff(long diff)
		{
			diff = Math.Abs(diff);
			long num = diff;
			int num2 = 0;
			while (num > 0L && (num & 1L) == 0L)
			{
				num2++;
				num >>= 1;
			}
			int num3 = num2;
			while ((num & 1L) == 1L)
			{
				num3++;
				num >>= 1;
			}
			return num == 0L && diff == (long)((1 << num3) - (1 << num2));
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00013844 File Offset: 0x00011A44
		private bool NormalizePacketIndex(ref int pageIndex, ref int packetIndex)
		{
			long num;
			bool flag;
			bool flag2;
			bool flag3;
			int num2;
			int num3;
			if (!this._reader.GetPage(pageIndex, out num, out flag, out flag2, out flag3, out num2, out num3))
			{
				return false;
			}
			int num4 = pageIndex;
			int i;
			bool flag4;
			int num5;
			for (i = packetIndex; i < (flag2 ? 1 : 0); i += num5 - (flag4 ? 1 : 0))
			{
				if (flag2 && flag)
				{
					return false;
				}
				flag4 = flag2;
				bool flag5;
				if (!this._reader.GetPage(--num4, out num, out flag, out flag2, out flag5, out num5, out num3))
				{
					return false;
				}
				if (flag4 && !flag5)
				{
					return false;
				}
			}
			pageIndex = num4;
			packetIndex = i;
			return true;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x000138CC File Offset: 0x00011ACC
		private Packet GetNextPacket(ref int pageIndex, ref int packetIndex)
		{
			if (this._reader == null)
			{
				throw new ObjectDisposedException("PacketProvider");
			}
			if (this._lastPacketPacketIndex != packetIndex || this._lastPacketPageIndex != pageIndex || this._lastPacket == null)
			{
				this._lastPacket = null;
				long granulePos;
				bool isResync;
				bool flag;
				bool isContinued;
				int packetCount;
				int pageOverhead;
				if (this._reader.GetPage(pageIndex, out granulePos, out isResync, out flag, out isContinued, out packetCount, out pageOverhead))
				{
					this._lastPacketPageIndex = pageIndex;
					this._lastPacketPacketIndex = packetIndex;
					this._lastPacket = this.CreatePacket(ref pageIndex, ref packetIndex, true, granulePos, isResync, isContinued, packetCount, pageOverhead);
					this._nextPacketPageIndex = pageIndex;
					this._nextPacketPacketIndex = packetIndex;
				}
			}
			else
			{
				pageIndex = this._nextPacketPageIndex;
				packetIndex = this._nextPacketPacketIndex;
			}
			return this._lastPacket;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00013978 File Offset: 0x00011B78
		private Packet CreatePacket(ref int pageIndex, ref int packetIndex, bool advance, long granulePos, bool isResync, bool isContinued, int packetCount, int pageOverhead)
		{
			Memory<byte> initialData = this._reader.GetPagePackets(pageIndex)[packetIndex];
			List<int> list = new List<int>(2)
			{
				pageIndex << 8 | packetIndex
			};
			int num = pageIndex;
			bool flag;
			bool flag3;
			if (isContinued && packetIndex == packetCount - 1)
			{
				flag = true;
				if (packetIndex > 0)
				{
					pageOverhead = 0;
				}
				int num2 = pageIndex;
				while (isContinued)
				{
					bool flag2;
					int num3;
					if (!this._reader.GetPage(++num2, out granulePos, out isResync, out flag2, out isContinued, out packetCount, out num3))
					{
						return null;
					}
					pageOverhead += num3;
					if (!flag2 || isResync)
					{
						break;
					}
					if (isContinued && packetCount > 1)
					{
						isContinued = false;
					}
					list.Add(num2 << 8);
				}
				flag3 = (packetCount == 1);
				num = num2;
			}
			else
			{
				flag = (packetIndex == 0);
				flag3 = (packetIndex == packetCount - 1);
			}
			Packet packet = new Packet(list, this, initialData)
			{
				IsResync = isResync
			};
			if (flag)
			{
				packet.ContainerOverheadBits = pageOverhead * 8;
			}
			if (flag3)
			{
				packet.GranulePosition = new long?(granulePos);
				if (this._reader.HasAllPages && num == this._reader.PageCount - 1)
				{
					packet.IsEndOfStream = true;
				}
			}
			if (advance)
			{
				if (num != pageIndex)
				{
					pageIndex = num;
					packetIndex = 0;
				}
				if (packetIndex == packetCount - 1)
				{
					pageIndex++;
					packetIndex = 0;
				}
				else
				{
					packetIndex++;
				}
			}
			return packet;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00013AB8 File Offset: 0x00011CB8
		Memory<byte> IPacketReader.GetPacketData(int pagePacketIndex)
		{
			int pageIndex = pagePacketIndex >> 8 & 16777215;
			int num = pagePacketIndex & 255;
			Memory<byte>[] pagePackets = this._reader.GetPagePackets(pageIndex);
			if (num < pagePackets.Length)
			{
				return pagePackets[num];
			}
			return Memory<byte>.Empty;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00013AF7 File Offset: 0x00011CF7
		void IPacketReader.InvalidatePacketCache(IPacket packet)
		{
			if (this._lastPacket == packet)
			{
				this._lastPacket = null;
			}
		}

		// Token: 0x04000395 RID: 917
		private IStreamPageReader _reader;

		// Token: 0x04000396 RID: 918
		private int _pageIndex;

		// Token: 0x04000397 RID: 919
		private int _packetIndex;

		// Token: 0x04000398 RID: 920
		private int _lastPacketPageIndex;

		// Token: 0x04000399 RID: 921
		private int _lastPacketPacketIndex;

		// Token: 0x0400039A RID: 922
		private Packet _lastPacket;

		// Token: 0x0400039B RID: 923
		private int _nextPacketPageIndex;

		// Token: 0x0400039C RID: 924
		private int _nextPacketPacketIndex;
	}
}
