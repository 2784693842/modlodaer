using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x0200006E RID: 110
	internal class StreamStats : IStreamStats
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000D714 File Offset: 0x0000B914
		public int EffectiveBitRate
		{
			get
			{
				object @lock = this._lock;
				long totalSamples;
				long num;
				lock (@lock)
				{
					totalSamples = this._totalSamples;
					num = this._audioBits + this._headerBits + this._containerBits + this._wasteBits;
				}
				if (totalSamples > 0L)
				{
					return (int)((double)num / (double)totalSamples * (double)this._sampleRate);
				}
				return 0;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000D788 File Offset: 0x0000B988
		public int InstantBitRate
		{
			get
			{
				object @lock = this._lock;
				int num;
				int num2;
				lock (@lock)
				{
					num = this._packetBits[0] + this._packetBits[1];
					num2 = this._packetSamples[0] + this._packetSamples[1];
				}
				if (num2 > 0)
				{
					return (int)((double)num / (double)num2 * (double)this._sampleRate);
				}
				return 0;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000D7FC File Offset: 0x0000B9FC
		public long ContainerBits
		{
			get
			{
				return this._containerBits;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000D804 File Offset: 0x0000BA04
		public long OverheadBits
		{
			get
			{
				return this._headerBits;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000D80C File Offset: 0x0000BA0C
		public long AudioBits
		{
			get
			{
				return this._audioBits;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000D814 File Offset: 0x0000BA14
		public long WasteBits
		{
			get
			{
				return this._wasteBits;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000D81C File Offset: 0x0000BA1C
		public int PacketCount
		{
			get
			{
				return this._packetCount;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000D824 File Offset: 0x0000BA24
		public void ResetStats()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._packetBits[0] = (this._packetBits[1] = 0);
				this._packetSamples[0] = (this._packetSamples[1] = 0);
				this._packetIndex = 0;
				this._packetCount = 0;
				this._audioBits = 0L;
				this._totalSamples = 0L;
				this._headerBits = 0L;
				this._containerBits = 0L;
				this._wasteBits = 0L;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000D8BC File Offset: 0x0000BABC
		internal void SetSampleRate(int sampleRate)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._sampleRate = sampleRate;
				this.ResetStats();
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000D904 File Offset: 0x0000BB04
		internal void AddPacket(int samples, int bits, int waste, int container)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				if (samples >= 0)
				{
					this._audioBits += (long)bits;
					this._wasteBits += (long)waste;
					this._containerBits += (long)container;
					this._totalSamples += (long)samples;
					this._packetBits[this._packetIndex] = bits + waste;
					this._packetSamples[this._packetIndex] = samples;
					int num = this._packetIndex + 1;
					this._packetIndex = num;
					if (num == 2)
					{
						this._packetIndex = 0;
					}
				}
				else
				{
					this._headerBits += (long)bits;
					this._wasteBits += (long)waste;
					this._containerBits += (long)container;
				}
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000D9E8 File Offset: 0x0000BBE8
		public StreamStats()
		{
			this._packetBits = new int[2];
			this._packetSamples = new int[2];
			this._lock = new object();
			base..ctor();
		}

		// Token: 0x040002D1 RID: 721
		private int _sampleRate;

		// Token: 0x040002D2 RID: 722
		private readonly int[] _packetBits;

		// Token: 0x040002D3 RID: 723
		private readonly int[] _packetSamples;

		// Token: 0x040002D4 RID: 724
		private int _packetIndex;

		// Token: 0x040002D5 RID: 725
		private long _totalSamples;

		// Token: 0x040002D6 RID: 726
		private long _audioBits;

		// Token: 0x040002D7 RID: 727
		private long _headerBits;

		// Token: 0x040002D8 RID: 728
		private long _containerBits;

		// Token: 0x040002D9 RID: 729
		private long _wasteBits;

		// Token: 0x040002DA RID: 730
		private object _lock;

		// Token: 0x040002DB RID: 731
		private int _packetCount;
	}
}
