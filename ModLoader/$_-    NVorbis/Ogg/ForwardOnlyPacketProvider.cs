using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x0200008C RID: 140
	internal class ForwardOnlyPacketProvider : DataPacket, IForwardOnlyPacketProvider, IPacketProvider
	{
		// Token: 0x06000340 RID: 832 RVA: 0x0001229D File Offset: 0x0001049D
		public ForwardOnlyPacketProvider(IPageReader reader, int streamSerial)
		{
			this._pageQueue = new Queue<ValueTuple<byte[], bool>>();
			base..ctor();
			this._reader = reader;
			this.StreamSerial = streamSerial;
			this._packetIndex = int.MaxValue;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000AF37 File Offset: 0x00009137
		public bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000342 RID: 834 RVA: 0x000122C9 File Offset: 0x000104C9
		public int StreamSerial { get; }

		// Token: 0x06000343 RID: 835 RVA: 0x000122D4 File Offset: 0x000104D4
		public bool AddPage(byte[] buf, bool isResync)
		{
			if ((buf[5] & 2) != 0)
			{
				if (this._isEndOfStream)
				{
					return false;
				}
				isResync = true;
				this._lastSeqNo = BitConverter.ToInt32(buf, 18);
			}
			else
			{
				int num = BitConverter.ToInt32(buf, 18);
				isResync |= (num != this._lastSeqNo + 1);
				this._lastSeqNo = num;
			}
			int num2 = 0;
			for (int i = 0; i < (int)buf[26]; i++)
			{
				num2 += (int)buf[27 + i];
			}
			if (num2 == 0)
			{
				return false;
			}
			this._pageQueue.Enqueue(new ValueTuple<byte[], bool>(buf, isResync));
			return true;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00012359 File Offset: 0x00010559
		public void SetEndOfStream()
		{
			this._isEndOfStream = true;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00012362 File Offset: 0x00010562
		public IPacket GetNextPacket()
		{
			if (this._packetBuf.Length > 0)
			{
				if (!this._lastWasPeek)
				{
					throw new InvalidOperationException("Must call Done() on previous packet first.");
				}
				this._lastWasPeek = false;
				return this;
			}
			else
			{
				this._lastWasPeek = false;
				if (this.GetPacket())
				{
					return this;
				}
				return null;
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000123A0 File Offset: 0x000105A0
		public IPacket PeekNextPacket()
		{
			if (this._packetBuf.Length > 0)
			{
				if (!this._lastWasPeek)
				{
					throw new InvalidOperationException("Must call Done() on previous packet first.");
				}
				return this;
			}
			else
			{
				this._lastWasPeek = true;
				if (this.GetPacket())
				{
					return this;
				}
				return null;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000123D8 File Offset: 0x000105D8
		private bool GetPacket()
		{
			byte[] pageBuf;
			bool flag;
			int num;
			int packetIndex;
			bool flag2;
			bool flag3;
			if (this._pageBuf != null && this._packetIndex < (int)(27 + this._pageBuf[26]))
			{
				pageBuf = this._pageBuf;
				flag = false;
				num = this._dataStart;
				packetIndex = this._packetIndex;
				flag2 = false;
				flag3 = (pageBuf[(int)(26 + pageBuf[26])] == byte.MaxValue);
			}
			else if (!this.ReadNextPage(out pageBuf, out flag, out num, out packetIndex, out flag2, out flag3))
			{
				return false;
			}
			int num2 = num;
			bool flag4 = packetIndex == 27;
			if (flag2 && flag4)
			{
				flag = true;
				num2 += this.GetPacketLength(pageBuf, ref packetIndex);
				if (packetIndex == (int)(27 + pageBuf[26]))
				{
					return this.GetPacket();
				}
			}
			if (!flag4)
			{
				num2 = 0;
			}
			int packetLength = this.GetPacketLength(pageBuf, ref packetIndex);
			Memory<byte> memory = new Memory<byte>(pageBuf, num, packetLength);
			num += packetLength;
			bool flag5 = packetIndex == (int)(27 + pageBuf[26]);
			if (flag3)
			{
				if (flag5)
				{
					flag5 = false;
				}
				else
				{
					int num3 = packetIndex;
					this.GetPacketLength(pageBuf, ref num3);
					flag5 = (num3 == (int)(27 + pageBuf[26]));
				}
			}
			bool flag6 = false;
			long? granulePosition = null;
			if (flag5)
			{
				granulePosition = new long?(BitConverter.ToInt64(pageBuf, 6));
				if ((pageBuf[5] & 4) != 0 || (this._isEndOfStream && this._pageQueue.Count == 0))
				{
					flag6 = true;
				}
			}
			else
			{
				while (flag3 && packetIndex == (int)(27 + pageBuf[26]) && (this.ReadNextPage(out pageBuf, out flag, out num, out packetIndex, out flag2, out flag3) && !flag && flag2))
				{
					num2 += (int)(27 + pageBuf[26]);
					Memory<byte> memory2 = memory;
					int packetLength2 = this.GetPacketLength(pageBuf, ref packetIndex);
					memory = new Memory<byte>(new byte[memory2.Length + packetLength2]);
					memory2.CopyTo(memory);
					new Memory<byte>(pageBuf, num, packetLength2).CopyTo(memory.Slice(memory2.Length));
					num += packetLength2;
				}
			}
			base.IsResync = flag;
			base.GranulePosition = granulePosition;
			base.IsEndOfStream = flag6;
			base.ContainerOverheadBits = num2 * 8;
			this._pageBuf = pageBuf;
			this._dataStart = num;
			this._packetIndex = packetIndex;
			this._packetBuf = memory;
			this._isEndOfStream = (this._isEndOfStream || flag6);
			this.Reset();
			return true;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000125FC File Offset: 0x000107FC
		private bool ReadNextPage(out byte[] pageBuf, out bool isResync, out int dataStart, out int packetIndex, out bool isContinuation, out bool isContinued)
		{
			while (this._pageQueue.Count == 0)
			{
				if (this._isEndOfStream || !this._reader.ReadNextPage())
				{
					pageBuf = null;
					isResync = false;
					dataStart = 0;
					packetIndex = 0;
					isContinuation = false;
					isContinued = false;
					return false;
				}
			}
			ValueTuple<byte[], bool> valueTuple = this._pageQueue.Dequeue();
			pageBuf = valueTuple.Item1;
			isResync = valueTuple.Item2;
			dataStart = (int)(pageBuf[26] + 27);
			packetIndex = 27;
			isContinuation = ((pageBuf[5] & 1) > 0);
			isContinued = (pageBuf[(int)(26 + pageBuf[26])] == byte.MaxValue);
			return true;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00012694 File Offset: 0x00010894
		private int GetPacketLength(byte[] pageBuf, ref int packetIndex)
		{
			int num = 0;
			while (pageBuf[packetIndex] == 255 && packetIndex < (int)(pageBuf[26] + 27))
			{
				num += (int)pageBuf[packetIndex];
				packetIndex++;
			}
			if (packetIndex < (int)(pageBuf[26] + 27))
			{
				num += (int)pageBuf[packetIndex];
				packetIndex++;
			}
			return num;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600034A RID: 842 RVA: 0x000126E1 File Offset: 0x000108E1
		protected override int TotalBits
		{
			get
			{
				return this._packetBuf.Length * 8;
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000126F0 File Offset: 0x000108F0
		protected unsafe override int ReadNextByte()
		{
			if (this._dataIndex < this._packetBuf.Length)
			{
				Span<byte> span = this._packetBuf.Span;
				int dataIndex = this._dataIndex;
				this._dataIndex = dataIndex + 1;
				return (int)(*span[dataIndex]);
			}
			return -1;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00012737 File Offset: 0x00010937
		public override void Reset()
		{
			this._dataIndex = 0;
			base.Reset();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00012746 File Offset: 0x00010946
		public override void Done()
		{
			this._packetBuf = Memory<byte>.Empty;
			base.Done();
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00007CE8 File Offset: 0x00005EE8
		long IPacketProvider.GetGranuleCount()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00007CE8 File Offset: 0x00005EE8
		long IPacketProvider.SeekTo(long granulePos, int preRoll, GetPacketGranuleCount getPacketGranuleCount)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000360 RID: 864
		private int _lastSeqNo;

		// Token: 0x04000361 RID: 865
		[TupleElementNames(new string[]
		{
			"buf",
			"isResync"
		})]
		private readonly Queue<ValueTuple<byte[], bool>> _pageQueue;

		// Token: 0x04000362 RID: 866
		private readonly IPageReader _reader;

		// Token: 0x04000363 RID: 867
		private byte[] _pageBuf;

		// Token: 0x04000364 RID: 868
		private int _packetIndex;

		// Token: 0x04000365 RID: 869
		private bool _isEndOfStream;

		// Token: 0x04000366 RID: 870
		private int _dataStart;

		// Token: 0x04000367 RID: 871
		private bool _lastWasPeek;

		// Token: 0x04000368 RID: 872
		private Memory<byte> _packetBuf;

		// Token: 0x04000369 RID: 873
		private int _dataIndex;
	}
}
