using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x02000090 RID: 144
	internal class PageReader : PageReaderBase, IPageData, IPageReader, IDisposable
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600035F RID: 863 RVA: 0x000127FD File Offset: 0x000109FD
		// (set) Token: 0x06000360 RID: 864 RVA: 0x00012804 File Offset: 0x00010A04
		internal static Func<IPageData, int, IStreamPageReader> CreateStreamPageReader { get; set; } = (IPageData pr, int ss) => new StreamPageReader(pr, ss);

		// Token: 0x06000361 RID: 865 RVA: 0x0001280C File Offset: 0x00010A0C
		public PageReader(Stream stream, bool closeOnDispose, Func<IPacketProvider, bool> newStreamCallback)
		{
			this._streamReaders = new Dictionary<int, IStreamPageReader>();
			this._readLock = new object();
			base..ctor(stream, closeOnDispose);
			this._newStreamCallback = newStreamCallback;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00012834 File Offset: 0x00010A34
		private ushort ParsePageHeader(byte[] pageBuf, int? streamSerial, bool? isResync)
		{
			byte b = pageBuf[26];
			int num = 0;
			int num2 = 0;
			bool isContinued = false;
			int num3 = 0;
			int i = 0;
			int num4 = 27;
			while (i < (int)b)
			{
				byte b2 = pageBuf[num4];
				num3 += (int)b2;
				num += (int)b2;
				if (b2 < 255)
				{
					if (num3 > 0)
					{
						num2++;
					}
					num3 = 0;
				}
				i++;
				num4++;
			}
			if (num3 > 0)
			{
				isContinued = (pageBuf[(int)(b + 26)] == byte.MaxValue);
				num2++;
			}
			this.StreamSerial = (streamSerial ?? BitConverter.ToInt32(pageBuf, 14));
			this.SequenceNumber = BitConverter.ToInt32(pageBuf, 18);
			this.PageFlags = (PageFlags)pageBuf[5];
			this.GranulePosition = BitConverter.ToInt64(pageBuf, 6);
			this.PacketCount = (short)num2;
			this.IsResync = isResync;
			this.IsContinued = isContinued;
			this.PageOverhead = (int)(27 + b);
			return (ushort)(this.PageOverhead + num);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00012918 File Offset: 0x00010B18
		private unsafe static Memory<byte>[] ReadPackets(int packetCount, Span<byte> segments, Memory<byte> dataBuffer)
		{
			Memory<byte>[] array = new Memory<byte>[packetCount];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < segments.Length; i++)
			{
				byte b = *segments[i];
				num3 += (int)b;
				if (b < 255)
				{
					if (num3 > 0)
					{
						array[num++] = dataBuffer.Slice(num2, num3);
						num2 += num3;
					}
					num3 = 0;
				}
			}
			if (num3 > 0)
			{
				array[num] = dataBuffer.Slice(num2, num3);
			}
			return array;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00012995 File Offset: 0x00010B95
		public override void Lock()
		{
			Monitor.Enter(this._readLock);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000129A2 File Offset: 0x00010BA2
		protected override bool CheckLock()
		{
			return Monitor.IsEntered(this._readLock);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x000129AF File Offset: 0x00010BAF
		public override bool Release()
		{
			if (Monitor.IsEntered(this._readLock))
			{
				Monitor.Exit(this._readLock);
				return true;
			}
			return false;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x000129CC File Offset: 0x00010BCC
		protected override void SaveNextPageSearch()
		{
			this._nextPageOffset = base.StreamPosition;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000129DA File Offset: 0x00010BDA
		protected override void PrepareStreamForNextPage()
		{
			base.SeekStream(this._nextPageOffset);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000129EC File Offset: 0x00010BEC
		protected override bool AddPage(int streamSerial, byte[] pageBuf, bool isResync)
		{
			this.PageOffset = base.StreamPosition - (long)pageBuf.Length;
			this.ParsePageHeader(pageBuf, new int?(streamSerial), new bool?(isResync));
			if (this.PacketCount == 0)
			{
				return false;
			}
			this._packets = PageReader.ReadPackets((int)this.PacketCount, new Span<byte>(pageBuf, 27, (int)pageBuf[26]), new Memory<byte>(pageBuf, (int)(27 + pageBuf[26]), pageBuf.Length - 27 - (int)pageBuf[26]));
			IStreamPageReader streamPageReader;
			if (this._streamReaders.TryGetValue(streamSerial, out streamPageReader))
			{
				streamPageReader.AddPage();
				if ((this.PageFlags & PageFlags.EndOfStream) == PageFlags.EndOfStream)
				{
					this._streamReaders.Remove(this.StreamSerial);
				}
			}
			else
			{
				IStreamPageReader streamPageReader2 = PageReader.CreateStreamPageReader(this, this.StreamSerial);
				streamPageReader2.AddPage();
				this._streamReaders.Add(this.StreamSerial, streamPageReader2);
				if (!this._newStreamCallback(streamPageReader2.PacketProvider))
				{
					this._streamReaders.Remove(this.StreamSerial);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00012AE4 File Offset: 0x00010CE4
		public override bool ReadPageAt(long offset)
		{
			if (!this.CheckLock())
			{
				throw new InvalidOperationException("Must be locked prior to reading!");
			}
			if (offset == this.PageOffset)
			{
				return true;
			}
			byte[] array = new byte[282];
			base.SeekStream(offset);
			int num = base.EnsureRead(array, 0, 27, 10);
			this.PageOffset = offset;
			if (base.VerifyHeader(array, 0, ref num))
			{
				this._packets = null;
				this._pageSize = this.ParsePageHeader(array, null, null);
				return true;
			}
			return false;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00012B6C File Offset: 0x00010D6C
		protected override void SetEndOfStreams()
		{
			foreach (KeyValuePair<int, IStreamPageReader> keyValuePair in this._streamReaders)
			{
				keyValuePair.Value.SetEndOfStream();
			}
			this._streamReaders.Clear();
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00012BD0 File Offset: 0x00010DD0
		// (set) Token: 0x0600036D RID: 877 RVA: 0x00012BD8 File Offset: 0x00010DD8
		public long PageOffset { get; private set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00012BE1 File Offset: 0x00010DE1
		// (set) Token: 0x0600036F RID: 879 RVA: 0x00012BE9 File Offset: 0x00010DE9
		public int StreamSerial { get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00012BF2 File Offset: 0x00010DF2
		// (set) Token: 0x06000371 RID: 881 RVA: 0x00012BFA File Offset: 0x00010DFA
		public int SequenceNumber { get; private set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00012C03 File Offset: 0x00010E03
		// (set) Token: 0x06000373 RID: 883 RVA: 0x00012C0B File Offset: 0x00010E0B
		public PageFlags PageFlags { get; private set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00012C14 File Offset: 0x00010E14
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00012C1C File Offset: 0x00010E1C
		public long GranulePosition { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00012C25 File Offset: 0x00010E25
		// (set) Token: 0x06000377 RID: 887 RVA: 0x00012C2D File Offset: 0x00010E2D
		public short PacketCount { get; private set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00012C36 File Offset: 0x00010E36
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00012C3E File Offset: 0x00010E3E
		public bool? IsResync { get; private set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00012C47 File Offset: 0x00010E47
		// (set) Token: 0x0600037B RID: 891 RVA: 0x00012C4F File Offset: 0x00010E4F
		public bool IsContinued { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00012C58 File Offset: 0x00010E58
		// (set) Token: 0x0600037D RID: 893 RVA: 0x00012C60 File Offset: 0x00010E60
		public int PageOverhead { get; private set; }

		// Token: 0x0600037E RID: 894 RVA: 0x00012C6C File Offset: 0x00010E6C
		public Memory<byte>[] GetPackets()
		{
			if (!this.CheckLock())
			{
				throw new InvalidOperationException("Must be locked!");
			}
			if (this._packets == null)
			{
				byte[] array = new byte[(int)this._pageSize];
				base.SeekStream(this.PageOffset);
				base.EnsureRead(array, 0, (int)this._pageSize, 10);
				this._packets = PageReader.ReadPackets((int)this.PacketCount, new Span<byte>(array, 27, (int)array[26]), new Memory<byte>(array, (int)(27 + array[26]), array.Length - 27 - (int)array[26]));
			}
			return this._packets;
		}

		// Token: 0x04000374 RID: 884
		private readonly Dictionary<int, IStreamPageReader> _streamReaders;

		// Token: 0x04000375 RID: 885
		private readonly Func<IPacketProvider, bool> _newStreamCallback;

		// Token: 0x04000376 RID: 886
		private readonly object _readLock;

		// Token: 0x04000377 RID: 887
		private long _nextPageOffset;

		// Token: 0x04000378 RID: 888
		private ushort _pageSize;

		// Token: 0x04000379 RID: 889
		private Memory<byte>[] _packets;
	}
}
