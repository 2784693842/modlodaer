using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02000019 RID: 25
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x00009E5E File Offset: 0x0000805E
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00009E6A File Offset: 0x0000806A
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00009E76 File Offset: 0x00008076
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00009E82 File Offset: 0x00008082
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00009E90 File Offset: 0x00008090
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00009EFF File Offset: 0x000080FF
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00009F07 File Offset: 0x00008107
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00009F10 File Offset: 0x00008110
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00009F18 File Offset: 0x00008118
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00009F35 File Offset: 0x00008135
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00009F3D File Offset: 0x0000813D
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00009F5E File Offset: 0x0000815E
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00009F66 File Offset: 0x00008166
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009F70 File Offset: 0x00008170
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000A028 File Offset: 0x00008228
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_98;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_98;
				}
				mustWait = true;
				IL_145:
				if (count <= 0)
				{
					return;
				}
				continue;
				IL_98:
				WorkItem workItem = this._pool[num];
				int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable);
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
					{
						break;
					}
					this._currentlyFilling = -1;
				}
				else
				{
					this._currentlyFilling = num;
				}
				goto IL_145;
			}
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000A184 File Offset: 0x00008384
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000A240 File Offset: 0x00008440
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem wi = this._pool[this._currentlyFilling];
				this._DeflateOne(wi);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000A2DC File Offset: 0x000084DC
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000A33F File Offset: 0x0000853F
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000A355 File Offset: 0x00008555
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000A360 File Offset: 0x00008560
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem workItem in this._pool)
			{
				this._toFill.Enqueue(workItem.index);
				workItem.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000A428 File Offset: 0x00008628
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = doAll ? 200 : (mustWait ? -1 : 0);
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								Queue<int> toWrite = this._toWrite;
								lock (toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000A5C8 File Offset: 0x000087C8
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				int index = workItem.index;
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				object obj = this._latestLock;
				lock (obj)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				Queue<int> toWrite = this._toWrite;
				lock (toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				object obj = this._eLock;
				lock (obj)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000A6F0 File Offset: 0x000088F0
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000A764 File Offset: 0x00008964
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				object outputLock = this._outputLock;
				lock (outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000A7DC File Offset: 0x000089DC
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000A7DF File Offset: 0x000089DF
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000A7E2 File Offset: 0x000089E2
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000A7EF File Offset: 0x000089EF
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000A7F6 File Offset: 0x000089F6
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x0000A803 File Offset: 0x00008A03
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000A80A File Offset: 0x00008A0A
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000A811 File Offset: 0x00008A11
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000A818 File Offset: 0x00008A18
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040000E0 RID: 224
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x040000E1 RID: 225
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x040000E2 RID: 226
		private List<WorkItem> _pool;

		// Token: 0x040000E3 RID: 227
		private bool _leaveOpen;

		// Token: 0x040000E4 RID: 228
		private bool emitting;

		// Token: 0x040000E5 RID: 229
		private Stream _outStream;

		// Token: 0x040000E6 RID: 230
		private int _maxBufferPairs;

		// Token: 0x040000E7 RID: 231
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x040000E8 RID: 232
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x040000E9 RID: 233
		private object _outputLock = new object();

		// Token: 0x040000EA RID: 234
		private bool _isClosed;

		// Token: 0x040000EB RID: 235
		private bool _firstWriteDone;

		// Token: 0x040000EC RID: 236
		private int _currentlyFilling;

		// Token: 0x040000ED RID: 237
		private int _lastFilled;

		// Token: 0x040000EE RID: 238
		private int _lastWritten;

		// Token: 0x040000EF RID: 239
		private int _latestCompressed;

		// Token: 0x040000F0 RID: 240
		private int _Crc32;

		// Token: 0x040000F1 RID: 241
		private CRC32 _runningCrc;

		// Token: 0x040000F2 RID: 242
		private object _latestLock = new object();

		// Token: 0x040000F3 RID: 243
		private Queue<int> _toWrite;

		// Token: 0x040000F4 RID: 244
		private Queue<int> _toFill;

		// Token: 0x040000F5 RID: 245
		private long _totalBytesProcessed;

		// Token: 0x040000F6 RID: 246
		private CompressionLevel _compressLevel;

		// Token: 0x040000F7 RID: 247
		private volatile Exception _pendingException;

		// Token: 0x040000F8 RID: 248
		private bool _handlingException;

		// Token: 0x040000F9 RID: 249
		private object _eLock = new object();

		// Token: 0x040000FA RID: 250
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x0200005E RID: 94
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x04000300 RID: 768
			None = 0U,
			// Token: 0x04000301 RID: 769
			NotUsed1 = 1U,
			// Token: 0x04000302 RID: 770
			EmitLock = 2U,
			// Token: 0x04000303 RID: 771
			EmitEnter = 4U,
			// Token: 0x04000304 RID: 772
			EmitBegin = 8U,
			// Token: 0x04000305 RID: 773
			EmitDone = 16U,
			// Token: 0x04000306 RID: 774
			EmitSkip = 32U,
			// Token: 0x04000307 RID: 775
			EmitAll = 58U,
			// Token: 0x04000308 RID: 776
			Flush = 64U,
			// Token: 0x04000309 RID: 777
			Lifecycle = 128U,
			// Token: 0x0400030A RID: 778
			Session = 256U,
			// Token: 0x0400030B RID: 779
			Synch = 512U,
			// Token: 0x0400030C RID: 780
			Instance = 1024U,
			// Token: 0x0400030D RID: 781
			Compress = 2048U,
			// Token: 0x0400030E RID: 782
			Write = 4096U,
			// Token: 0x0400030F RID: 783
			WriteEnter = 8192U,
			// Token: 0x04000310 RID: 784
			WriteTake = 16384U,
			// Token: 0x04000311 RID: 785
			All = 4294967295U
		}
	}
}
