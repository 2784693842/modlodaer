using System;
using System.Collections.Generic;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x02000084 RID: 132
	internal abstract class PageReaderBase : IPageReader, IDisposable
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0001187E File Offset: 0x0000FA7E
		// (set) Token: 0x060002FD RID: 765 RVA: 0x00011885 File Offset: 0x0000FA85
		internal static Func<ICrc> CreateCrc { get; set; } = () => new Crc();

		// Token: 0x060002FE RID: 766 RVA: 0x00011890 File Offset: 0x0000FA90
		protected PageReaderBase(Stream stream, bool closeOnDispose)
		{
			this._crc = PageReaderBase.CreateCrc();
			this._ignoredSerials = new HashSet<int>();
			this._headerBuf = new byte[305];
			base..ctor();
			this._stream = stream;
			this._closeOnDispose = closeOnDispose;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002FF RID: 767 RVA: 0x000118DC File Offset: 0x0000FADC
		protected long StreamPosition
		{
			get
			{
				Stream stream = this._stream;
				if (stream == null)
				{
					throw new ObjectDisposedException("PageReaderBase");
				}
				return stream.Position;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000300 RID: 768 RVA: 0x000118F8 File Offset: 0x0000FAF8
		// (set) Token: 0x06000301 RID: 769 RVA: 0x00011900 File Offset: 0x0000FB00
		public long ContainerBits { get; private set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00011909 File Offset: 0x0000FB09
		// (set) Token: 0x06000303 RID: 771 RVA: 0x00011911 File Offset: 0x0000FB11
		public long WasteBits { get; private set; }

		// Token: 0x06000304 RID: 772 RVA: 0x0001191C File Offset: 0x0000FB1C
		private bool VerifyPage(byte[] headerBuf, int index, int cnt, out byte[] pageBuf, out int bytesRead)
		{
			byte b = headerBuf[index + 26];
			if (cnt - index < index + 27 + (int)b)
			{
				pageBuf = null;
				bytesRead = 0;
				return false;
			}
			int num = 0;
			int i;
			for (i = 0; i < (int)b; i++)
			{
				num += (int)headerBuf[index + i + 27];
			}
			pageBuf = new byte[num + (int)b + 27];
			Buffer.BlockCopy(headerBuf, index, pageBuf, 0, (int)(b + 27));
			bytesRead = this.EnsureRead(pageBuf, (int)(b + 27), num, 10);
			if (bytesRead != num)
			{
				return false;
			}
			num = pageBuf.Length;
			this._crc.Reset();
			for (i = 0; i < 22; i++)
			{
				this._crc.Update((int)pageBuf[i]);
			}
			this._crc.Update(0);
			this._crc.Update(0);
			this._crc.Update(0);
			this._crc.Update(0);
			for (i += 4; i < num; i++)
			{
				this._crc.Update((int)pageBuf[i]);
			}
			return this._crc.Test(BitConverter.ToUInt32(pageBuf, 22));
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00011A28 File Offset: 0x0000FC28
		private bool AddPage(byte[] pageBuf, bool isResync)
		{
			int num = BitConverter.ToInt32(pageBuf, 14);
			if (!this._ignoredSerials.Contains(num))
			{
				if (this.AddPage(num, pageBuf, isResync))
				{
					this.ContainerBits += (long)(8 * (27 + pageBuf[26]));
					return true;
				}
				this._ignoredSerials.Add(num);
			}
			return false;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00011A80 File Offset: 0x0000FC80
		private void EnqueueData(byte[] buf, int count)
		{
			if (this._overflowBuf != null)
			{
				byte[] array = new byte[this._overflowBuf.Length - this._overflowBufIndex + count];
				Buffer.BlockCopy(this._overflowBuf, this._overflowBufIndex, array, 0, array.Length - count);
				int srcOffset = buf.Length - count;
				Buffer.BlockCopy(buf, srcOffset, array, array.Length - count, count);
				this._overflowBufIndex = 0;
				return;
			}
			this._overflowBuf = buf;
			this._overflowBufIndex = buf.Length - count;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00011AF4 File Offset: 0x0000FCF4
		private void ClearEnqueuedData(int count)
		{
			if (this._overflowBuf != null && (this._overflowBufIndex += count) >= this._overflowBuf.Length)
			{
				this._overflowBuf = null;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00011B2C File Offset: 0x0000FD2C
		private int FillHeader(byte[] buf, int index, int count, int maxTries = 10)
		{
			int num = 0;
			if (this._overflowBuf != null)
			{
				num = Math.Min(this._overflowBuf.Length - this._overflowBufIndex, count);
				Buffer.BlockCopy(this._overflowBuf, this._overflowBufIndex, buf, index, num);
				index += num;
				count -= num;
				if ((this._overflowBufIndex += num) == this._overflowBuf.Length)
				{
					this._overflowBuf = null;
				}
			}
			if (count > 0)
			{
				num += this.EnsureRead(buf, index, count, maxTries);
			}
			return num;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00011BAC File Offset: 0x0000FDAC
		private bool VerifyHeader(byte[] buffer, int index, ref int cnt, bool isFromReadNextPage)
		{
			if (buffer[index] == 79 && buffer[index + 1] == 103 && buffer[index + 2] == 103 && buffer[index + 3] == 83)
			{
				if (cnt < 27)
				{
					if (isFromReadNextPage)
					{
						cnt += this.FillHeader(buffer, 27 - cnt + index, 27 - cnt, 10);
					}
					else
					{
						cnt += this.EnsureRead(buffer, 27 - cnt + index, 27 - cnt, 10);
					}
				}
				if (cnt >= 27)
				{
					byte b = buffer[index + 26];
					if (isFromReadNextPage)
					{
						cnt += this.FillHeader(buffer, index + 27, (int)b, 10);
					}
					else
					{
						cnt += this.EnsureRead(buffer, index + 27, (int)b, 10);
					}
					if (cnt == index + 27 + (int)b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00011C70 File Offset: 0x0000FE70
		protected int EnsureRead(byte[] buf, int index, int count, int maxTries = 10)
		{
			int num = 0;
			int num2 = 0;
			do
			{
				int num3 = this._stream.Read(buf, index + num, count - num);
				if (num3 == 0 && ++num2 == maxTries)
				{
					break;
				}
				num += num3;
			}
			while (num < count);
			return num;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00011CA9 File Offset: 0x0000FEA9
		protected bool VerifyHeader(byte[] buffer, int index, ref int cnt)
		{
			return this.VerifyHeader(buffer, index, ref cnt, false);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00011CB5 File Offset: 0x0000FEB5
		protected long SeekStream(long offset)
		{
			if (!this.CheckLock())
			{
				throw new InvalidOperationException("Must be locked prior to reading!");
			}
			return this._stream.Seek(offset, SeekOrigin.Begin);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00007C03 File Offset: 0x00005E03
		protected virtual void PrepareStreamForNextPage()
		{
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00007C03 File Offset: 0x00005E03
		protected virtual void SaveNextPageSearch()
		{
		}

		// Token: 0x0600030F RID: 783
		protected abstract bool AddPage(int streamSerial, byte[] pageBuf, bool isResync);

		// Token: 0x06000310 RID: 784
		protected abstract void SetEndOfStreams();

		// Token: 0x06000311 RID: 785 RVA: 0x00007C03 File Offset: 0x00005E03
		public virtual void Lock()
		{
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000AF34 File Offset: 0x00009134
		protected virtual bool CheckLock()
		{
			return true;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000AF37 File Offset: 0x00009137
		public virtual bool Release()
		{
			return false;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00011CD8 File Offset: 0x0000FED8
		public bool ReadNextPage()
		{
			if (!this.CheckLock())
			{
				throw new InvalidOperationException("Must be locked prior to reading!");
			}
			bool isResync = false;
			int num = 0;
			this.PrepareStreamForNextPage();
			int num2;
			while ((num2 = this.FillHeader(this._headerBuf, num, 27 - num, 10)) > 0)
			{
				num2 += num;
				for (int i = 0; i < num2 - 4; i++)
				{
					if (this.VerifyHeader(this._headerBuf, i, ref num2, true))
					{
						byte[] array;
						int count;
						if (this.VerifyPage(this._headerBuf, i, num2, out array, out count))
						{
							this.ClearEnqueuedData(count);
							this.SaveNextPageSearch();
							if (this.AddPage(array, isResync))
							{
								return true;
							}
							this.WasteBits += (long)(array.Length * 8);
							num = 0;
							num2 = 0;
							break;
						}
						else if (array != null)
						{
							this.EnqueueData(array, count);
						}
					}
					this.WasteBits += 8L;
					isResync = true;
				}
				if (num2 >= 3)
				{
					this._headerBuf[0] = this._headerBuf[num2 - 3];
					this._headerBuf[1] = this._headerBuf[num2 - 2];
					this._headerBuf[2] = this._headerBuf[num2 - 1];
					num = 3;
				}
			}
			if (num2 == 0)
			{
				this.SetEndOfStreams();
			}
			return false;
		}

		// Token: 0x06000315 RID: 789
		public abstract bool ReadPageAt(long offset);

		// Token: 0x06000316 RID: 790 RVA: 0x00011DF7 File Offset: 0x0000FFF7
		public void Dispose()
		{
			this.SetEndOfStreams();
			if (this._closeOnDispose)
			{
				Stream stream = this._stream;
				if (stream != null)
				{
					stream.Dispose();
				}
			}
			this._stream = null;
		}

		// Token: 0x04000342 RID: 834
		private readonly ICrc _crc;

		// Token: 0x04000343 RID: 835
		private readonly HashSet<int> _ignoredSerials;

		// Token: 0x04000344 RID: 836
		private readonly byte[] _headerBuf;

		// Token: 0x04000345 RID: 837
		private byte[] _overflowBuf;

		// Token: 0x04000346 RID: 838
		private int _overflowBufIndex;

		// Token: 0x04000347 RID: 839
		private Stream _stream;

		// Token: 0x04000348 RID: 840
		private bool _closeOnDispose;
	}
}
