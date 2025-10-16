using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x0200008A RID: 138
	internal abstract class DataPacket : IPacket
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00011F8C File Offset: 0x0001018C
		// (set) Token: 0x0600032B RID: 811 RVA: 0x00011F94 File Offset: 0x00010194
		public int ContainerOverheadBits { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00011F9D File Offset: 0x0001019D
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00011FA5 File Offset: 0x000101A5
		public long? GranulePosition { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00011FAE File Offset: 0x000101AE
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00011FB7 File Offset: 0x000101B7
		public bool IsResync
		{
			get
			{
				return this.GetFlag(DataPacket.PacketFlags.IsResync);
			}
			set
			{
				this.SetFlag(DataPacket.PacketFlags.IsResync, value);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00011FC1 File Offset: 0x000101C1
		// (set) Token: 0x06000331 RID: 817 RVA: 0x00011FCA File Offset: 0x000101CA
		public bool IsShort
		{
			get
			{
				return this.GetFlag(DataPacket.PacketFlags.IsShort);
			}
			private set
			{
				this.SetFlag(DataPacket.PacketFlags.IsShort, value);
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00011FD4 File Offset: 0x000101D4
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00011FDD File Offset: 0x000101DD
		public bool IsEndOfStream
		{
			get
			{
				return this.GetFlag(DataPacket.PacketFlags.IsEndOfStream);
			}
			set
			{
				this.SetFlag(DataPacket.PacketFlags.IsEndOfStream, value);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00011FE7 File Offset: 0x000101E7
		public int BitsRead
		{
			get
			{
				return this._readBits;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00011FEF File Offset: 0x000101EF
		public int BitsRemaining
		{
			get
			{
				return this.TotalBits - this._readBits;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000336 RID: 822
		protected abstract int TotalBits { get; }

		// Token: 0x06000337 RID: 823 RVA: 0x00011FFE File Offset: 0x000101FE
		private bool GetFlag(DataPacket.PacketFlags flag)
		{
			return this._packetFlags.HasFlag(flag);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00012016 File Offset: 0x00010216
		private void SetFlag(DataPacket.PacketFlags flag, bool value)
		{
			if (value)
			{
				this._packetFlags |= flag;
				return;
			}
			this._packetFlags &= ~flag;
		}

		// Token: 0x06000339 RID: 825
		protected abstract int ReadNextByte();

		// Token: 0x0600033A RID: 826 RVA: 0x00007C03 File Offset: 0x00005E03
		public virtual void Done()
		{
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0001203A File Offset: 0x0001023A
		public virtual void Reset()
		{
			this._bitBucket = 0UL;
			this._bitCount = 0;
			this._overflowBits = 0;
			this._readBits = 0;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0001205C File Offset: 0x0001025C
		ulong IPacket.ReadBits(int count)
		{
			if (count == 0)
			{
				return 0UL;
			}
			int num;
			ulong result = this.TryPeekBits(count, out num);
			this.SkipBits(count);
			return result;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00012080 File Offset: 0x00010280
		public ulong TryPeekBits(int count, out int bitsRead)
		{
			if (count < 0 || count > 64)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count == 0)
			{
				bitsRead = 0;
				return 0UL;
			}
			while (this._bitCount < count)
			{
				int num = this.ReadNextByte();
				if (num == -1)
				{
					bitsRead = this._bitCount;
					return this._bitBucket;
				}
				this._bitBucket = (ulong)((long)(num & 255) << this._bitCount | (long)this._bitBucket);
				this._bitCount += 8;
				if (this._bitCount > 64)
				{
					this._overflowBits = (byte)(num >> 72 - this._bitCount);
				}
			}
			ulong num2 = this._bitBucket;
			if (count < 64)
			{
				num2 &= (1UL << count) - 1UL;
			}
			bitsRead = count;
			return num2;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00012138 File Offset: 0x00010338
		public void SkipBits(int count)
		{
			if (count > 0)
			{
				if (this._bitCount > count)
				{
					if (count > 63)
					{
						this._bitBucket = 0UL;
					}
					else
					{
						this._bitBucket >>= count;
					}
					if (this._bitCount > 64)
					{
						int num = this._bitCount - 64;
						this._bitBucket |= (ulong)this._overflowBits << this._bitCount - count - num;
						if (num > count)
						{
							this._overflowBits = (byte)(this._overflowBits >> count);
						}
					}
					this._bitCount -= count;
					this._readBits += count;
					return;
				}
				if (this._bitCount == count)
				{
					this._bitBucket = 0UL;
					this._bitCount = 0;
					this._readBits += count;
					return;
				}
				count -= this._bitCount;
				this._readBits += this._bitCount;
				this._bitCount = 0;
				this._bitBucket = 0UL;
				while (count > 8)
				{
					if (this.ReadNextByte() == -1)
					{
						count = 0;
						this.IsShort = true;
						break;
					}
					count -= 8;
					this._readBits += 8;
				}
				if (count > 0)
				{
					int num2 = this.ReadNextByte();
					if (num2 == -1)
					{
						this.IsShort = true;
						return;
					}
					this._bitBucket = (ulong)((long)(num2 >> count));
					this._bitCount = 8 - count;
					this._readBits += count;
				}
			}
		}

		// Token: 0x04000350 RID: 848
		private ulong _bitBucket;

		// Token: 0x04000351 RID: 849
		private int _bitCount;

		// Token: 0x04000352 RID: 850
		private byte _overflowBits;

		// Token: 0x04000353 RID: 851
		private DataPacket.PacketFlags _packetFlags;

		// Token: 0x04000354 RID: 852
		private int _readBits;

		// Token: 0x0200008B RID: 139
		[Flags]
		protected enum PacketFlags : byte
		{
			// Token: 0x04000358 RID: 856
			IsResync = 1,
			// Token: 0x04000359 RID: 857
			IsEndOfStream = 2,
			// Token: 0x0400035A RID: 858
			IsShort = 4,
			// Token: 0x0400035B RID: 859
			User0 = 8,
			// Token: 0x0400035C RID: 860
			User1 = 16,
			// Token: 0x0400035D RID: 861
			User2 = 32,
			// Token: 0x0400035E RID: 862
			User3 = 64,
			// Token: 0x0400035F RID: 863
			User4 = 128
		}
	}
}
