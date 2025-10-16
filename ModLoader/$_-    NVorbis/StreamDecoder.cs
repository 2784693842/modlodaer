using System;
using System.IO;
using System.Text;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x0200006C RID: 108
	internal sealed class StreamDecoder : IStreamDecoder, IDisposable
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000C86F File Offset: 0x0000AA6F
		// (set) Token: 0x06000229 RID: 553 RVA: 0x0000C876 File Offset: 0x0000AA76
		internal static Func<IFactory> CreateFactory { get; set; } = () => new Factory();

		// Token: 0x0600022A RID: 554 RVA: 0x0000C87E File Offset: 0x0000AA7E
		public StreamDecoder(IPacketProvider packetProvider) : this(packetProvider, new Factory())
		{
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000C88C File Offset: 0x0000AA8C
		internal StreamDecoder(IPacketProvider packetProvider, IFactory factory)
		{
			if (packetProvider == null)
			{
				throw new ArgumentNullException("packetProvider");
			}
			this._packetProvider = packetProvider;
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this._factory = factory;
			this._stats = new StreamStats();
			this._currentPosition = 0L;
			this.ClipSamples = true;
			IPacket packet = this._packetProvider.PeekNextPacket();
			if (!this.ProcessHeaderPackets(packet))
			{
				this._packetProvider = null;
				packet.Reset();
				throw StreamDecoder.GetInvalidStreamException(packet);
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C910 File Offset: 0x0000AB10
		private static Exception GetInvalidStreamException(IPacket packet)
		{
			Exception result;
			try
			{
				ulong num = packet.ReadBits(64);
				if (num == 7233173838382854223UL)
				{
					result = new ArgumentException("Found OPUS bitstream.");
				}
				else if ((num & 255UL) == 127UL)
				{
					result = new ArgumentException("Found FLAC bitstream.");
				}
				else if (num == 2314885909937746003UL)
				{
					result = new ArgumentException("Found Speex bitstream.");
				}
				else if (num == 28254585843050854UL)
				{
					result = new ArgumentException("Found Skeleton metadata bitstream.");
				}
				else if ((num & 72057594037927680UL) == 27428895509214208UL)
				{
					result = new ArgumentException("Found Theora bitsream.");
				}
				else
				{
					result = new ArgumentException("Could not find Vorbis data to decode.");
				}
			}
			finally
			{
				packet.Reset();
			}
			return result;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000C9D8 File Offset: 0x0000ABD8
		private bool ProcessHeaderPackets(IPacket packet)
		{
			if (!StreamDecoder.ProcessHeaderPacket(packet, new Func<IPacket, bool>(this.LoadStreamHeader), delegate(IPacket _)
			{
				this._packetProvider.GetNextPacket().Done();
			}))
			{
				return false;
			}
			if (!StreamDecoder.ProcessHeaderPacket(this._packetProvider.GetNextPacket(), new Func<IPacket, bool>(this.LoadComments), delegate(IPacket pkt)
			{
				pkt.Done();
			}))
			{
				return false;
			}
			if (!StreamDecoder.ProcessHeaderPacket(this._packetProvider.GetNextPacket(), new Func<IPacket, bool>(this.LoadBooks), delegate(IPacket pkt)
			{
				pkt.Done();
			}))
			{
				return false;
			}
			this._currentPosition = 0L;
			this.ResetDecoder();
			return true;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000CA94 File Offset: 0x0000AC94
		private static bool ProcessHeaderPacket(IPacket packet, Func<IPacket, bool> processAction, Action<IPacket> doneAction)
		{
			if (packet != null)
			{
				try
				{
					return processAction(packet);
				}
				finally
				{
					doneAction(packet);
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000CACC File Offset: 0x0000ACCC
		private static bool ValidateHeader(IPacket packet, byte[] expected)
		{
			for (int i = 0; i < expected.Length; i++)
			{
				if ((ulong)expected[i] != packet.ReadBits(8))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		private static string ReadString(IPacket packet)
		{
			int num = (int)packet.ReadBits(32);
			if (num == 0)
			{
				return string.Empty;
			}
			byte[] array = new byte[num];
			if (packet.Read(array, 0, num) < num)
			{
				throw new InvalidDataException("Could not read full string!");
			}
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000CB44 File Offset: 0x0000AD44
		private bool LoadStreamHeader(IPacket packet)
		{
			if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureStream))
			{
				return false;
			}
			this._channels = (byte)packet.ReadBits(8);
			this._sampleRate = (int)packet.ReadBits(32);
			this.UpperBitrate = (int)packet.ReadBits(32);
			this.NominalBitrate = (int)packet.ReadBits(32);
			this.LowerBitrate = (int)packet.ReadBits(32);
			this._block0Size = 1 << (int)packet.ReadBits(4);
			this._block1Size = 1 << (int)packet.ReadBits(4);
			if (this.NominalBitrate == 0 && this.UpperBitrate > 0 && this.LowerBitrate > 0)
			{
				this.NominalBitrate = (this.UpperBitrate + this.LowerBitrate) / 2;
			}
			this._stats.SetSampleRate(this._sampleRate);
			this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
			return true;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000CC30 File Offset: 0x0000AE30
		private bool LoadComments(IPacket packet)
		{
			if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureComments))
			{
				return false;
			}
			this._vendor = StreamDecoder.ReadString(packet);
			this._comments = new string[packet.ReadBits(32)];
			for (int i = 0; i < this._comments.Length; i++)
			{
				this._comments[i] = StreamDecoder.ReadString(packet);
			}
			this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
			return true;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000CCAC File Offset: 0x0000AEAC
		private bool LoadBooks(IPacket packet)
		{
			if (!StreamDecoder.ValidateHeader(packet, StreamDecoder.PacketSignatureBooks))
			{
				return false;
			}
			IMdct mdct = this._factory.CreateMdct();
			IHuffman huffman = this._factory.CreateHuffman();
			ICodebook[] array = new ICodebook[packet.ReadBits(8) + 1UL];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this._factory.CreateCodebook();
				array[i].Init(packet, huffman);
			}
			int num = (int)packet.ReadBits(6) + 1;
			packet.SkipBits(16 * num);
			IFloor[] array2 = new IFloor[packet.ReadBits(6) + 1UL];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = this._factory.CreateFloor(packet);
				array2[j].Init(packet, (int)this._channels, this._block0Size, this._block1Size, array);
			}
			IResidue[] array3 = new IResidue[packet.ReadBits(6) + 1UL];
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k] = this._factory.CreateResidue(packet);
				array3[k].Init(packet, (int)this._channels, array);
			}
			IMapping[] array4 = new IMapping[packet.ReadBits(6) + 1UL];
			for (int l = 0; l < array4.Length; l++)
			{
				array4[l] = this._factory.CreateMapping(packet);
				array4[l].Init(packet, (int)this._channels, array2, array3, mdct);
			}
			this._modes = new IMode[packet.ReadBits(6) + 1UL];
			for (int m = 0; m < this._modes.Length; m++)
			{
				this._modes[m] = this._factory.CreateMode();
				this._modes[m].Init(packet, (int)this._channels, this._block0Size, this._block1Size, array4);
			}
			if (!packet.ReadBit())
			{
				throw new InvalidDataException("Book packet did not end on correct bit!");
			}
			this._modeFieldBits = Utils.ilog(this._modes.Length - 1);
			this._stats.AddPacket(-1, packet.BitsRead, packet.BitsRemaining, packet.ContainerOverheadBits);
			return true;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000CEC5 File Offset: 0x0000B0C5
		private void ResetDecoder()
		{
			this._prevPacketBuf = null;
			this._prevPacketStart = 0;
			this._prevPacketEnd = 0;
			this._prevPacketStop = 0;
			this._nextPacketBuf = null;
			this._eosFound = false;
			this._hasClipped = false;
			this._hasPosition = false;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000CF00 File Offset: 0x0000B100
		public int Read(Span<float> buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count % (int)this._channels != 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must be a multiple of Channels!");
			}
			if (this._packetProvider == null)
			{
				throw new ObjectDisposedException("StreamDecoder");
			}
			if (count == 0)
			{
				return 0;
			}
			int i = offset;
			int num = offset + count;
			while (i < num)
			{
				if (this._prevPacketStart == this._prevPacketEnd)
				{
					if (this._eosFound)
					{
						this._nextPacketBuf = null;
						this._prevPacketBuf = null;
						break;
					}
					long? num2;
					if (!this.ReadNextPacket((i - offset) / (int)this._channels, out num2))
					{
						this._prevPacketEnd = this._prevPacketStop;
					}
					if (num2 != null && !this._hasPosition)
					{
						this._hasPosition = true;
						this._currentPosition = num2.Value - (long)(this._prevPacketEnd - this._prevPacketStart) - (long)((i - offset) / (int)this._channels);
					}
				}
				int num3 = Math.Min((num - i) / (int)this._channels, this._prevPacketEnd - this._prevPacketStart);
				if (num3 > 0)
				{
					if (this.ClipSamples)
					{
						i += this.ClippingCopyBuffer(buffer, i, num3);
					}
					else
					{
						i += this.CopyBuffer(buffer, i, num3);
					}
				}
			}
			count = i - offset;
			this._currentPosition += (long)(count / (int)this._channels);
			return count;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000D070 File Offset: 0x0000B270
		private unsafe int ClippingCopyBuffer(Span<float> target, int targetIndex, int count)
		{
			int num = targetIndex;
			while (count > 0)
			{
				for (int i = 0; i < (int)this._channels; i++)
				{
					*target[num++] = Utils.ClipValue(this._prevPacketBuf[i][this._prevPacketStart], ref this._hasClipped);
				}
				this._prevPacketStart++;
				count--;
			}
			return num - targetIndex;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
		private unsafe int CopyBuffer(Span<float> target, int targetIndex, int count)
		{
			int num = targetIndex;
			while (count > 0)
			{
				for (int i = 0; i < (int)this._channels; i++)
				{
					*target[num++] = this._prevPacketBuf[i][this._prevPacketStart];
				}
				this._prevPacketStart++;
				count--;
			}
			return num - targetIndex;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000D12C File Offset: 0x0000B32C
		private bool ReadNextPacket(int bufferedSamples, out long? samplePosition)
		{
			int num;
			int num2;
			int prevPacketStop;
			bool flag;
			int bits;
			int waste;
			int container;
			float[][] array = this.DecodeNextPacket(out num, out num2, out prevPacketStop, out flag, out samplePosition, out bits, out waste, out container);
			this._eosFound = (this._eosFound || flag);
			if (array == null)
			{
				this._stats.AddPacket(0, bits, waste, container);
				return false;
			}
			if (samplePosition != null && flag)
			{
				long num3 = this._currentPosition + (long)bufferedSamples + (long)num2 - (long)num;
				int num4 = (int)(samplePosition.Value - num3);
				if (num4 < 0)
				{
					num2 += num4;
				}
			}
			if (this._prevPacketEnd > 0)
			{
				StreamDecoder.OverlapBuffers(this._prevPacketBuf, array, this._prevPacketStart, this._prevPacketStop, num, (int)this._channels);
				this._prevPacketStart = num;
			}
			else if (this._prevPacketBuf == null)
			{
				this._prevPacketStart = num2;
			}
			this._stats.AddPacket(num2 - this._prevPacketStart, bits, waste, container);
			this._nextPacketBuf = this._prevPacketBuf;
			this._prevPacketEnd = num2;
			this._prevPacketStop = prevPacketStop;
			this._prevPacketBuf = array;
			return true;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000D224 File Offset: 0x0000B424
		private float[][] DecodeNextPacket(out int packetStartindex, out int packetValidLength, out int packetTotalLength, out bool isEndOfStream, out long? samplePosition, out int bitsRead, out int bitsRemaining, out int containerOverheadBits)
		{
			IPacket packet = null;
			float[][] result;
			try
			{
				if ((packet = this._packetProvider.GetNextPacket()) == null)
				{
					isEndOfStream = true;
				}
				else
				{
					isEndOfStream = packet.IsEndOfStream;
					if (packet.IsResync)
					{
						this._hasPosition = false;
					}
					containerOverheadBits = packet.ContainerOverheadBits;
					if (packet.ReadBit())
					{
						bitsRemaining = packet.BitsRemaining + 1;
					}
					else
					{
						IMode mode = this._modes[(int)packet.ReadBits(this._modeFieldBits)];
						if (this._nextPacketBuf == null)
						{
							this._nextPacketBuf = new float[(int)this._channels][];
							for (int i = 0; i < (int)this._channels; i++)
							{
								this._nextPacketBuf[i] = new float[this._block1Size];
							}
						}
						if (mode.Decode(packet, this._nextPacketBuf, out packetStartindex, out packetValidLength, out packetTotalLength))
						{
							samplePosition = packet.GranulePosition;
							bitsRead = packet.BitsRead;
							bitsRemaining = packet.BitsRemaining;
							return this._nextPacketBuf;
						}
						bitsRemaining = packet.BitsRead + packet.BitsRemaining;
					}
				}
				packetStartindex = 0;
				packetValidLength = 0;
				packetTotalLength = 0;
				samplePosition = null;
				bitsRead = 0;
				bitsRemaining = 0;
				containerOverheadBits = 0;
				result = null;
			}
			finally
			{
				if (packet != null)
				{
					packet.Done();
				}
			}
			return result;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000D368 File Offset: 0x0000B568
		private static void OverlapBuffers(float[][] previous, float[][] next, int prevStart, int prevLen, int nextStart, int channels)
		{
			while (prevStart < prevLen)
			{
				for (int i = 0; i < channels; i++)
				{
					next[i][nextStart] += previous[i][prevStart];
				}
				prevStart++;
				nextStart++;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000D3A6 File Offset: 0x0000B5A6
		public void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = SeekOrigin.Begin)
		{
			this.SeekTo((long)((double)this.SampleRate * timePosition.TotalSeconds), seekOrigin);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		public void SeekTo(long samplePosition, SeekOrigin seekOrigin = SeekOrigin.Begin)
		{
			if (this._packetProvider == null)
			{
				throw new ObjectDisposedException("StreamDecoder");
			}
			if (!this._packetProvider.CanSeek)
			{
				throw new InvalidOperationException("Seek is not supported by the Contracts.IPacketProvider instance.");
			}
			switch (seekOrigin)
			{
			case SeekOrigin.Begin:
				break;
			case SeekOrigin.Current:
				samplePosition = this.SamplePosition - samplePosition;
				break;
			case SeekOrigin.End:
				samplePosition = this.TotalSamples - samplePosition;
				break;
			default:
				throw new ArgumentOutOfRangeException("seekOrigin");
			}
			if (samplePosition < 0L)
			{
				throw new ArgumentOutOfRangeException("samplePosition");
			}
			int num;
			if (samplePosition == 0L)
			{
				this._packetProvider.SeekTo(0L, 0, new GetPacketGranuleCount(this.GetPacketGranules));
				num = 0;
			}
			else
			{
				long num2 = this._packetProvider.SeekTo(samplePosition, 1, new GetPacketGranuleCount(this.GetPacketGranules));
				num = (int)(samplePosition - num2);
			}
			this.ResetDecoder();
			this._hasPosition = true;
			long? num3;
			if (!this.ReadNextPacket(0, out num3))
			{
				this._eosFound = true;
				if (this._packetProvider.GetGranuleCount() != samplePosition)
				{
					throw new InvalidOperationException("Could not read pre-roll packet!  Try seeking again prior to reading more samples.");
				}
				this._prevPacketStart = this._prevPacketStop;
				this._currentPosition = samplePosition;
				return;
			}
			else
			{
				if (!this.ReadNextPacket(0, out num3))
				{
					this.ResetDecoder();
					this._eosFound = true;
					throw new InvalidOperationException("Could not read pre-roll packet!  Try seeking again prior to reading more samples.");
				}
				this._prevPacketStart += num;
				this._currentPosition = samplePosition;
				return;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000D504 File Offset: 0x0000B704
		private int GetPacketGranules(IPacket curPacket)
		{
			if (curPacket.IsResync)
			{
				return 0;
			}
			if (curPacket.ReadBit())
			{
				return 0;
			}
			int num = (int)curPacket.ReadBits(this._modeFieldBits);
			if (num < 0 || num >= this._modes.Length)
			{
				return 0;
			}
			return this._modes[num].GetPacketSampleCount(curPacket);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000D552 File Offset: 0x0000B752
		public void Dispose()
		{
			IDisposable disposable = this._packetProvider as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			this._packetProvider = null;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000D571 File Offset: 0x0000B771
		public int Channels
		{
			get
			{
				return (int)this._channels;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000D579 File Offset: 0x0000B779
		public int SampleRate
		{
			get
			{
				return this._sampleRate;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000D581 File Offset: 0x0000B781
		// (set) Token: 0x06000242 RID: 578 RVA: 0x0000D589 File Offset: 0x0000B789
		public int UpperBitrate { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000D592 File Offset: 0x0000B792
		// (set) Token: 0x06000244 RID: 580 RVA: 0x0000D59A File Offset: 0x0000B79A
		public int NominalBitrate { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000D5A3 File Offset: 0x0000B7A3
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000D5AB File Offset: 0x0000B7AB
		public int LowerBitrate { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000D5B4 File Offset: 0x0000B7B4
		public ITagData Tags
		{
			get
			{
				ITagData result;
				if ((result = this._tags) == null)
				{
					result = (this._tags = new TagData(this._vendor, this._comments));
				}
				return result;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000D5E5 File Offset: 0x0000B7E5
		public TimeSpan TotalTime
		{
			get
			{
				return TimeSpan.FromSeconds((double)this.TotalSamples / (double)this._sampleRate);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000D5FB File Offset: 0x0000B7FB
		public long TotalSamples
		{
			get
			{
				IPacketProvider packetProvider = this._packetProvider;
				if (packetProvider == null)
				{
					throw new ObjectDisposedException("StreamDecoder");
				}
				return packetProvider.GetGranuleCount();
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000D617 File Offset: 0x0000B817
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000D62D File Offset: 0x0000B82D
		public TimeSpan TimePosition
		{
			get
			{
				return TimeSpan.FromSeconds((double)this._currentPosition / (double)this._sampleRate);
			}
			set
			{
				this.SeekTo(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000D637 File Offset: 0x0000B837
		// (set) Token: 0x0600024D RID: 589 RVA: 0x0000D63F File Offset: 0x0000B83F
		public long SamplePosition
		{
			get
			{
				return this._currentPosition;
			}
			set
			{
				this.SeekTo(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000D649 File Offset: 0x0000B849
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0000D651 File Offset: 0x0000B851
		public bool ClipSamples { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000D65A File Offset: 0x0000B85A
		public bool HasClipped
		{
			get
			{
				return this._hasClipped;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000D662 File Offset: 0x0000B862
		public bool IsEndOfStream
		{
			get
			{
				return this._eosFound && this._prevPacketBuf == null;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000D677 File Offset: 0x0000B877
		public IStreamStats Stats
		{
			get
			{
				return this._stats;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D680 File Offset: 0x0000B880
		// Note: this type is marked as 'beforefieldinit'.
		static StreamDecoder()
		{
			StreamDecoder.PacketSignatureStream = new byte[]
			{
				1,
				118,
				111,
				114,
				98,
				105,
				115,
				0,
				0,
				0,
				0
			};
			StreamDecoder.PacketSignatureComments = new byte[]
			{
				3,
				118,
				111,
				114,
				98,
				105,
				115
			};
			StreamDecoder.PacketSignatureBooks = new byte[]
			{
				5,
				118,
				111,
				114,
				98,
				105,
				115
			};
		}

		// Token: 0x040002B2 RID: 690
		private IPacketProvider _packetProvider;

		// Token: 0x040002B3 RID: 691
		private IFactory _factory;

		// Token: 0x040002B4 RID: 692
		private StreamStats _stats;

		// Token: 0x040002B5 RID: 693
		private byte _channels;

		// Token: 0x040002B6 RID: 694
		private int _sampleRate;

		// Token: 0x040002B7 RID: 695
		private int _block0Size;

		// Token: 0x040002B8 RID: 696
		private int _block1Size;

		// Token: 0x040002B9 RID: 697
		private IMode[] _modes;

		// Token: 0x040002BA RID: 698
		private int _modeFieldBits;

		// Token: 0x040002BB RID: 699
		private string _vendor;

		// Token: 0x040002BC RID: 700
		private string[] _comments;

		// Token: 0x040002BD RID: 701
		private ITagData _tags;

		// Token: 0x040002BE RID: 702
		private long _currentPosition;

		// Token: 0x040002BF RID: 703
		private bool _hasClipped;

		// Token: 0x040002C0 RID: 704
		private bool _hasPosition;

		// Token: 0x040002C1 RID: 705
		private bool _eosFound;

		// Token: 0x040002C2 RID: 706
		private float[][] _nextPacketBuf;

		// Token: 0x040002C3 RID: 707
		private float[][] _prevPacketBuf;

		// Token: 0x040002C4 RID: 708
		private int _prevPacketStart;

		// Token: 0x040002C5 RID: 709
		private int _prevPacketEnd;

		// Token: 0x040002C6 RID: 710
		private int _prevPacketStop;

		// Token: 0x040002C7 RID: 711
		private static readonly byte[] PacketSignatureStream;

		// Token: 0x040002C8 RID: 712
		private static readonly byte[] PacketSignatureComments;

		// Token: 0x040002C9 RID: 713
		private static readonly byte[] PacketSignatureBooks;
	}
}
