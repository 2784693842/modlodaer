using System;
using System.IO;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x0200003D RID: 61
	internal abstract class WaveStream : Stream, IWaveProvider
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000104 RID: 260
		public abstract WaveFormat WaveFormat { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000AF34 File Offset: 0x00009134
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000AF34 File Offset: 0x00009134
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000AF37 File Offset: 0x00009137
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007C03 File Offset: 0x00005E03
		public override void Flush()
		{
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000AF3A File Offset: 0x0000913A
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin == SeekOrigin.Begin)
			{
				this.Position = offset;
			}
			else if (origin == SeekOrigin.Current)
			{
				this.Position += offset;
			}
			else
			{
				this.Position = this.Length + offset;
			}
			return this.Position;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000AF70 File Offset: 0x00009170
		public override void SetLength(long length)
		{
			throw new NotSupportedException("Can't set length of a WaveFormatString");
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000AF7C File Offset: 0x0000917C
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Can't write to a WaveFormatString");
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000AF88 File Offset: 0x00009188
		public virtual int BlockAlign
		{
			get
			{
				return this.WaveFormat.BlockAlign;
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000AF98 File Offset: 0x00009198
		public void Skip(int seconds)
		{
			long num = this.Position + (long)(this.WaveFormat.AverageBytesPerSecond * seconds);
			if (num > this.Length)
			{
				this.Position = this.Length;
				return;
			}
			if (num < 0L)
			{
				this.Position = 0L;
				return;
			}
			this.Position = num;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000AFE6 File Offset: 0x000091E6
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000B001 File Offset: 0x00009201
		public virtual TimeSpan CurrentTime
		{
			get
			{
				return TimeSpan.FromSeconds((double)this.Position / (double)this.WaveFormat.AverageBytesPerSecond);
			}
			set
			{
				this.Position = (long)(value.TotalSeconds * (double)this.WaveFormat.AverageBytesPerSecond);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000B01E File Offset: 0x0000921E
		public virtual TimeSpan TotalTime
		{
			get
			{
				return TimeSpan.FromSeconds((double)this.Length / (double)this.WaveFormat.AverageBytesPerSecond);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000B039 File Offset: 0x00009239
		public virtual bool HasData(int count)
		{
			return this.Position < this.Length;
		}
	}
}
