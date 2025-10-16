using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000059 RID: 89
	internal sealed class VorbisReader : IVorbisReader, IDisposable
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000C24F File Offset: 0x0000A44F
		// (set) Token: 0x0600019F RID: 415 RVA: 0x0000C256 File Offset: 0x0000A456
		internal static Func<Stream, bool, IContainerReader> CreateContainerReader { get; set; } = (Stream s, bool cod) => new ContainerReader(s, cod);

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000C25E File Offset: 0x0000A45E
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000C265 File Offset: 0x0000A465
		internal static Func<IPacketProvider, IStreamDecoder> CreateStreamDecoder { get; set; } = (IPacketProvider pp) => new StreamDecoder(pp, new Factory());

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001A2 RID: 418 RVA: 0x0000C270 File Offset: 0x0000A470
		// (remove) Token: 0x060001A3 RID: 419 RVA: 0x0000C2A8 File Offset: 0x0000A4A8
		public event EventHandler<NewStreamEventArgs> NewStream;

		// Token: 0x060001A4 RID: 420 RVA: 0x0000C2DD File Offset: 0x0000A4DD
		public VorbisReader(string fileName) : this(File.OpenRead(fileName), true)
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000C2EC File Offset: 0x0000A4EC
		public VorbisReader(Stream stream, bool closeOnDispose = true)
		{
			this._decoders = new List<IStreamDecoder>();
			IContainerReader containerReader = VorbisReader.CreateContainerReader(stream, closeOnDispose);
			containerReader.NewStreamCallback = new NewStreamHandler(this.ProcessNewStream);
			if (!containerReader.TryInit() || this._decoders.Count == 0)
			{
				containerReader.NewStreamCallback = null;
				containerReader.Dispose();
				if (closeOnDispose)
				{
					stream.Dispose();
				}
				throw new ArgumentException("Could not load the specified container!", "containerReader");
			}
			this._closeOnDispose = closeOnDispose;
			this._containerReader = containerReader;
			this._streamDecoder = this._decoders[0];
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000C384 File Offset: 0x0000A584
		[Obsolete("Use \"new StreamDecoder(Contracts.IPacketProvider)\" and the container's NewStreamCallback or Streams property instead.", true)]
		public VorbisReader(IContainerReader containerReader)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000C384 File Offset: 0x0000A584
		[Obsolete("Use \"new StreamDecoder(Contracts.IPacketProvider)\" instead.", true)]
		public VorbisReader(IPacketProvider packetProvider)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000C394 File Offset: 0x0000A594
		private bool ProcessNewStream(IPacketProvider packetProvider)
		{
			IStreamDecoder streamDecoder = VorbisReader.CreateStreamDecoder(packetProvider);
			streamDecoder.ClipSamples = true;
			NewStreamEventArgs newStreamEventArgs = new NewStreamEventArgs(streamDecoder);
			EventHandler<NewStreamEventArgs> newStream = this.NewStream;
			if (newStream != null)
			{
				newStream(this, newStreamEventArgs);
			}
			if (!newStreamEventArgs.IgnoreStream)
			{
				this._decoders.Add(streamDecoder);
				return true;
			}
			return false;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000C3E8 File Offset: 0x0000A5E8
		public void Dispose()
		{
			if (this._decoders != null)
			{
				foreach (IStreamDecoder streamDecoder in this._decoders)
				{
					streamDecoder.Dispose();
				}
				this._decoders.Clear();
			}
			if (this._containerReader != null)
			{
				this._containerReader.NewStreamCallback = null;
				if (this._closeOnDispose)
				{
					this._containerReader.Dispose();
				}
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000C474 File Offset: 0x0000A674
		public IReadOnlyList<IStreamDecoder> Streams
		{
			get
			{
				return this._decoders;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000C47C File Offset: 0x0000A67C
		public int Channels
		{
			get
			{
				return this._streamDecoder.Channels;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000C489 File Offset: 0x0000A689
		public int SampleRate
		{
			get
			{
				return this._streamDecoder.SampleRate;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000C496 File Offset: 0x0000A696
		public int UpperBitrate
		{
			get
			{
				return this._streamDecoder.UpperBitrate;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000C4A3 File Offset: 0x0000A6A3
		public int NominalBitrate
		{
			get
			{
				return this._streamDecoder.NominalBitrate;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		public int LowerBitrate
		{
			get
			{
				return this._streamDecoder.LowerBitrate;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000C4BD File Offset: 0x0000A6BD
		public ITagData Tags
		{
			get
			{
				return this._streamDecoder.Tags;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000C4CA File Offset: 0x0000A6CA
		[Obsolete("Use .Tags.EncoderVendor instead.")]
		public string Vendor
		{
			get
			{
				return this._streamDecoder.Tags.EncoderVendor;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000C4DC File Offset: 0x0000A6DC
		[Obsolete("Use .Tags.All instead.")]
		public string[] Comments
		{
			get
			{
				return this._streamDecoder.Tags.All.SelectMany((KeyValuePair<string, IReadOnlyList<string>> k) => k.Value, (KeyValuePair<string, IReadOnlyList<string>> kvp, string Item) => kvp.Key + "=" + Item).ToArray<string>();
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00007CE8 File Offset: 0x00005EE8
		[Obsolete("No longer supported.  Will receive a new stream when parameters change.", true)]
		public bool IsParameterChange
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000C541 File Offset: 0x0000A741
		public long ContainerOverheadBits
		{
			get
			{
				IContainerReader containerReader = this._containerReader;
				if (containerReader == null)
				{
					return 0L;
				}
				return containerReader.ContainerBits;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000C555 File Offset: 0x0000A755
		public long ContainerWasteBits
		{
			get
			{
				IContainerReader containerReader = this._containerReader;
				if (containerReader == null)
				{
					return 0L;
				}
				return containerReader.WasteBits;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000C569 File Offset: 0x0000A769
		public int StreamIndex
		{
			get
			{
				return this._decoders.IndexOf(this._streamDecoder);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000C57C File Offset: 0x0000A77C
		[Obsolete("Use .Streams.Count instead.")]
		public int StreamCount
		{
			get
			{
				return this._decoders.Count;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000C589 File Offset: 0x0000A789
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000C596 File Offset: 0x0000A796
		[Obsolete("Use VorbisReader.TimePosition instead.")]
		public TimeSpan DecodedTime
		{
			get
			{
				return this._streamDecoder.TimePosition;
			}
			set
			{
				this.TimePosition = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000C59F File Offset: 0x0000A79F
		// (set) Token: 0x060001BB RID: 443 RVA: 0x0000C5AC File Offset: 0x0000A7AC
		[Obsolete("Use VorbisReader.SamplePosition instead.")]
		public long DecodedPosition
		{
			get
			{
				return this._streamDecoder.SamplePosition;
			}
			set
			{
				this.SamplePosition = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000C5B5 File Offset: 0x0000A7B5
		public TimeSpan TotalTime
		{
			get
			{
				return this._streamDecoder.TotalTime;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000C5C2 File Offset: 0x0000A7C2
		public long TotalSamples
		{
			get
			{
				return this._streamDecoder.TotalSamples;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000C589 File Offset: 0x0000A789
		// (set) Token: 0x060001BF RID: 447 RVA: 0x0000C5CF File Offset: 0x0000A7CF
		public TimeSpan TimePosition
		{
			get
			{
				return this._streamDecoder.TimePosition;
			}
			set
			{
				this._streamDecoder.TimePosition = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000C59F File Offset: 0x0000A79F
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000C5DD File Offset: 0x0000A7DD
		public long SamplePosition
		{
			get
			{
				return this._streamDecoder.SamplePosition;
			}
			set
			{
				this._streamDecoder.SamplePosition = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000C5EB File Offset: 0x0000A7EB
		public bool IsEndOfStream
		{
			get
			{
				return this._streamDecoder.IsEndOfStream;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000C5F8 File Offset: 0x0000A7F8
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0000C605 File Offset: 0x0000A805
		public bool ClipSamples
		{
			get
			{
				return this._streamDecoder.ClipSamples;
			}
			set
			{
				this._streamDecoder.ClipSamples = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000C613 File Offset: 0x0000A813
		public bool HasClipped
		{
			get
			{
				return this._streamDecoder.HasClipped;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000C620 File Offset: 0x0000A820
		public IStreamStats StreamStats
		{
			get
			{
				return this._streamDecoder.Stats;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00007CE8 File Offset: 0x00005EE8
		[Obsolete("Use Streams[*].Stats instead.", true)]
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000C62D File Offset: 0x0000A82D
		public bool FindNextStream()
		{
			return this._containerReader != null && this._containerReader.FindNextStream();
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000C644 File Offset: 0x0000A844
		public bool SwitchStreams(int index)
		{
			if (index < 0 || index >= this._decoders.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			IStreamDecoder streamDecoder = this._decoders[index];
			IStreamDecoder streamDecoder2 = this._streamDecoder;
			if (streamDecoder == streamDecoder2)
			{
				return false;
			}
			streamDecoder.ClipSamples = streamDecoder2.ClipSamples;
			this._streamDecoder = streamDecoder;
			return streamDecoder.Channels != streamDecoder2.Channels || streamDecoder.SampleRate != streamDecoder2.SampleRate;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		public void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = SeekOrigin.Begin)
		{
			this._streamDecoder.SeekTo(timePosition, seekOrigin);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000C6CB File Offset: 0x0000A8CB
		public void SeekTo(long samplePosition, SeekOrigin seekOrigin = SeekOrigin.Begin)
		{
			this._streamDecoder.SeekTo(samplePosition, seekOrigin);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000C6DA File Offset: 0x0000A8DA
		public int ReadSamples(float[] buffer, int offset, int count)
		{
			count -= count % this._streamDecoder.Channels;
			if (count > 0)
			{
				return this._streamDecoder.Read(buffer, offset, count);
			}
			return 0;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000C708 File Offset: 0x0000A908
		public int ReadSamples(Span<float> buffer)
		{
			int count = buffer.Length - buffer.Length % this._streamDecoder.Channels;
			if (!buffer.IsEmpty)
			{
				return this._streamDecoder.Read(buffer, 0, count);
			}
			return 0;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00007CE8 File Offset: 0x00005EE8
		[Obsolete("No longer needed.", true)]
		public void ClearParameterChange()
		{
			throw new NotSupportedException();
		}

		// Token: 0x040002A5 RID: 677
		private readonly List<IStreamDecoder> _decoders;

		// Token: 0x040002A6 RID: 678
		private readonly IContainerReader _containerReader;

		// Token: 0x040002A7 RID: 679
		private readonly bool _closeOnDispose;

		// Token: 0x040002A8 RID: 680
		private IStreamDecoder _streamDecoder;
	}
}
