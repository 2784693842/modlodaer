using System;
using System.Collections.Generic;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x02000093 RID: 147
	internal class StreamPageReader : IStreamPageReader
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00012D25 File Offset: 0x00010F25
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00012D2C File Offset: 0x00010F2C
		internal static Func<IStreamPageReader, int, IPacketProvider> CreatePacketProvider { get; set; } = (IStreamPageReader pr, int ss) => new PacketProvider(pr, ss);

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00012D34 File Offset: 0x00010F34
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00012D3C File Offset: 0x00010F3C
		public IPacketProvider PacketProvider { get; private set; }

		// Token: 0x06000391 RID: 913 RVA: 0x00012D45 File Offset: 0x00010F45
		public StreamPageReader(IPageData pageReader, int streamSerial)
		{
			this._pageOffsets = new List<long>();
			this._lastPageIndex = -1;
			base..ctor();
			this._reader = pageReader;
			this.PacketProvider = StreamPageReader.CreatePacketProvider(this, streamSerial);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00012D78 File Offset: 0x00010F78
		public void AddPage()
		{
			if (!this.HasAllPages)
			{
				if (this._reader.GranulePosition != -1L)
				{
					if (this._firstDataPageIndex == null && this._reader.GranulePosition > 0L)
					{
						this._firstDataPageIndex = new int?(this._pageOffsets.Count);
					}
					else if (this._maxGranulePos > this._reader.GranulePosition)
					{
						throw new InvalidDataException("Granule Position regressed?!");
					}
					this._maxGranulePos = this._reader.GranulePosition;
				}
				else if (this._firstDataPageIndex != null && (!this._reader.IsContinued || this._reader.PacketCount != 1))
				{
					throw new InvalidDataException("Granule Position was -1 but page does not have exactly 1 continued packet.");
				}
				if ((this._reader.PageFlags & PageFlags.EndOfStream) != PageFlags.None)
				{
					this.HasAllPages = true;
				}
				if (this._reader.IsResync.Value || (this._lastSeqNbr != 0 && this._lastSeqNbr + 1 != this._reader.SequenceNumber))
				{
					this._pageOffsets.Add(-this._reader.PageOffset);
				}
				else
				{
					this._pageOffsets.Add(this._reader.PageOffset);
				}
				this._lastSeqNbr = this._reader.SequenceNumber;
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00012EC0 File Offset: 0x000110C0
		public Memory<byte>[] GetPagePackets(int pageIndex)
		{
			if (this._cachedPagePackets != null && this._lastPageIndex == pageIndex)
			{
				return this._cachedPagePackets;
			}
			long num = this._pageOffsets[pageIndex];
			if (num < 0L)
			{
				num = -num;
			}
			this._reader.Lock();
			Memory<byte>[] result;
			try
			{
				this._reader.ReadPageAt(num);
				Memory<byte>[] packets = this._reader.GetPackets();
				if (pageIndex == this._lastPageIndex)
				{
					this._cachedPagePackets = packets;
				}
				result = packets;
			}
			finally
			{
				this._reader.Release();
			}
			return result;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00012F50 File Offset: 0x00011150
		public int FindPage(long granulePos)
		{
			int num = -1;
			if (granulePos == 0L)
			{
				num = this.FindFirstDataPage();
			}
			else
			{
				int num2 = this._pageOffsets.Count - 1;
				long num3;
				if (this.GetPageRaw(num2, out num3))
				{
					if (granulePos < num3)
					{
						num = this.FindPageBisection(granulePos, this.FindFirstDataPage(), num2, num3);
					}
					else if (granulePos > num3)
					{
						num = this.FindPageForward(num2, num3, granulePos);
					}
					else
					{
						num = num2 + 1;
					}
				}
			}
			if (num == -1)
			{
				throw new ArgumentOutOfRangeException("granulePos");
			}
			return num;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00012FC0 File Offset: 0x000111C0
		private int FindFirstDataPage()
		{
			while (this._firstDataPageIndex == null)
			{
				long num;
				if (!this.GetPageRaw(this._pageOffsets.Count, out num))
				{
					return -1;
				}
			}
			return this._firstDataPageIndex.Value;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00013000 File Offset: 0x00011200
		private int FindPageForward(int pageIndex, long pageGranulePos, long granulePos)
		{
			while (pageGranulePos <= granulePos)
			{
				if (++pageIndex == this._pageOffsets.Count)
				{
					if (!this.GetNextPageGranulePos(out pageGranulePos))
					{
						long? maxGranulePosition = this.MaxGranulePosition;
						if (maxGranulePosition.GetValueOrDefault() < granulePos & maxGranulePosition != null)
						{
							pageIndex = -1;
							break;
						}
						break;
					}
				}
				else if (!this.GetPageRaw(pageIndex, out pageGranulePos))
				{
					pageIndex = -1;
					break;
				}
			}
			return pageIndex;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00013064 File Offset: 0x00011264
		private bool GetNextPageGranulePos(out long granulePos)
		{
			int count = this._pageOffsets.Count;
			while (count == this._pageOffsets.Count && !this.HasAllPages)
			{
				this._reader.Lock();
				try
				{
					if (!this._reader.ReadNextPage())
					{
						this.HasAllPages = true;
					}
					else if (count < this._pageOffsets.Count)
					{
						granulePos = this._reader.GranulePosition;
						return true;
					}
				}
				finally
				{
					this._reader.Release();
				}
			}
			granulePos = 0L;
			return false;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000130FC File Offset: 0x000112FC
		private int FindPageBisection(long granulePos, int low, int high, long highGranulePos)
		{
			long num = 0L;
			int num2;
			while ((num2 = high - low) > 0)
			{
				int num3 = low + (int)((double)num2 * ((double)(granulePos - num) / (double)(highGranulePos - num)));
				long num4;
				if (!this.GetPageRaw(num3, out num4))
				{
					return -1;
				}
				if (num4 > granulePos)
				{
					high = num3;
					highGranulePos = num4;
				}
				else
				{
					if (num4 >= granulePos)
					{
						return num3 + 1;
					}
					low = num3 + 1;
					num = num4 + 1L;
				}
			}
			return low;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00013158 File Offset: 0x00011358
		private bool GetPageRaw(int pageIndex, out long pageGranulePos)
		{
			long num = this._pageOffsets[pageIndex];
			if (num < 0L)
			{
				num = -num;
			}
			this._reader.Lock();
			bool result;
			try
			{
				if (this._reader.ReadPageAt(num))
				{
					pageGranulePos = this._reader.GranulePosition;
					result = true;
				}
				else
				{
					pageGranulePos = 0L;
					result = false;
				}
			}
			finally
			{
				this._reader.Release();
			}
			return result;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000131CC File Offset: 0x000113CC
		public bool GetPage(int pageIndex, out long granulePos, out bool isResync, out bool isContinuation, out bool isContinued, out int packetCount, out int pageOverhead)
		{
			if (this._lastPageIndex == pageIndex)
			{
				granulePos = this._lastPageGranulePos;
				isResync = this._lastPageIsResync;
				isContinuation = this._lastPageIsContinuation;
				isContinued = this._lastPageIsContinued;
				packetCount = this._lastPagePacketCount;
				pageOverhead = this._lastPageOverhead;
				return true;
			}
			this._reader.Lock();
			try
			{
				while (pageIndex >= this._pageOffsets.Count && !this.HasAllPages && this._reader.ReadNextPage())
				{
					if (pageIndex < this._pageOffsets.Count)
					{
						isResync = this._reader.IsResync.Value;
						this.ReadPageData(pageIndex, out granulePos, out isContinuation, out isContinued, out packetCount, out pageOverhead);
						return true;
					}
				}
			}
			finally
			{
				this._reader.Release();
			}
			if (pageIndex < this._pageOffsets.Count)
			{
				long num = this._pageOffsets[pageIndex];
				if (num < 0L)
				{
					isResync = true;
					num = -num;
				}
				else
				{
					isResync = false;
				}
				this._reader.Lock();
				try
				{
					if (this._reader.ReadPageAt(num))
					{
						this._lastPageIsResync = isResync;
						this.ReadPageData(pageIndex, out granulePos, out isContinuation, out isContinued, out packetCount, out pageOverhead);
						return true;
					}
				}
				finally
				{
					this._reader.Release();
				}
			}
			granulePos = 0L;
			isResync = false;
			isContinuation = false;
			isContinued = false;
			packetCount = 0;
			pageOverhead = 0;
			return false;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00013338 File Offset: 0x00011538
		private void ReadPageData(int pageIndex, out long granulePos, out bool isContinuation, out bool isContinued, out int packetCount, out int pageOverhead)
		{
			this._cachedPagePackets = null;
			this._lastPageGranulePos = (granulePos = this._reader.GranulePosition);
			this._lastPageIsContinuation = (isContinuation = ((this._reader.PageFlags & PageFlags.ContinuesPacket) > PageFlags.None));
			this._lastPageIsContinued = (isContinued = this._reader.IsContinued);
			this._lastPagePacketCount = (packetCount = (int)this._reader.PacketCount);
			this._lastPageOverhead = (pageOverhead = this._reader.PageOverhead);
			this._lastPageIndex = pageIndex;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000133C9 File Offset: 0x000115C9
		public void SetEndOfStream()
		{
			this.HasAllPages = true;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600039D RID: 925 RVA: 0x000133D2 File Offset: 0x000115D2
		public int PageCount
		{
			get
			{
				return this._pageOffsets.Count;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600039E RID: 926 RVA: 0x000133DF File Offset: 0x000115DF
		// (set) Token: 0x0600039F RID: 927 RVA: 0x000133E7 File Offset: 0x000115E7
		public bool HasAllPages { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x000133F0 File Offset: 0x000115F0
		public long? MaxGranulePosition
		{
			get
			{
				if (!this.HasAllPages)
				{
					return null;
				}
				return new long?(this._maxGranulePos);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0001341A File Offset: 0x0001161A
		public int FirstDataPageIndex
		{
			get
			{
				return this.FindFirstDataPage();
			}
		}

		// Token: 0x04000385 RID: 901
		private readonly IPageData _reader;

		// Token: 0x04000386 RID: 902
		private readonly List<long> _pageOffsets;

		// Token: 0x04000387 RID: 903
		private int _lastSeqNbr;

		// Token: 0x04000388 RID: 904
		private int? _firstDataPageIndex;

		// Token: 0x04000389 RID: 905
		private long _maxGranulePos;

		// Token: 0x0400038A RID: 906
		private int _lastPageIndex;

		// Token: 0x0400038B RID: 907
		private long _lastPageGranulePos;

		// Token: 0x0400038C RID: 908
		private bool _lastPageIsResync;

		// Token: 0x0400038D RID: 909
		private bool _lastPageIsContinuation;

		// Token: 0x0400038E RID: 910
		private bool _lastPageIsContinued;

		// Token: 0x0400038F RID: 911
		private int _lastPagePacketCount;

		// Token: 0x04000390 RID: 912
		private int _lastPageOverhead;

		// Token: 0x04000391 RID: 913
		private Memory<byte>[] _cachedPagePackets;
	}
}
