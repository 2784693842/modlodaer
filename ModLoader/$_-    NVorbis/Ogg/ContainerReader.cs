using System;
using System.Collections.Generic;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x02000081 RID: 129
	internal sealed class ContainerReader : IContainerReader, IDisposable
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0001161D File Offset: 0x0000F81D
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00011624 File Offset: 0x0000F824
		internal static Func<Stream, bool, Func<IPacketProvider, bool>, IPageReader> CreatePageReader { get; set; } = (Stream s, bool cod, Func<IPacketProvider, bool> cb) => new PageReader(s, cod, cb);

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0001162C File Offset: 0x0000F82C
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00011633 File Offset: 0x0000F833
		internal static Func<Stream, bool, Func<IPacketProvider, bool>, IPageReader> CreateForwardOnlyPageReader { get; set; } = (Stream s, bool cod, Func<IPacketProvider, bool> cb) => new ForwardOnlyPageReader(s, cod, cb);

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0001163B File Offset: 0x0000F83B
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00011643 File Offset: 0x0000F843
		public NewStreamHandler NewStreamCallback { get; set; }

		// Token: 0x060002E8 RID: 744 RVA: 0x0001164C File Offset: 0x0000F84C
		public IReadOnlyList<IPacketProvider> GetStreams()
		{
			List<IPacketProvider> list = new List<IPacketProvider>(this._packetProviders.Count);
			for (int i = 0; i < this._packetProviders.Count; i++)
			{
				IPacketProvider item;
				if (this._packetProviders[i].TryGetTarget(out item))
				{
					list.Add(item);
				}
				else
				{
					list.RemoveAt(i);
					i--;
				}
			}
			return list;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x000116AA File Offset: 0x0000F8AA
		public bool CanSeek { get; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000116B2 File Offset: 0x0000F8B2
		public long WasteBits
		{
			get
			{
				return this._reader.WasteBits;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002EB RID: 747 RVA: 0x000116BF File Offset: 0x0000F8BF
		public long ContainerBits
		{
			get
			{
				return this._reader.ContainerBits;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000116CC File Offset: 0x0000F8CC
		public ContainerReader(Stream stream, bool closeOnDispose)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._packetProviders = new List<WeakReference<IPacketProvider>>();
			if (stream.CanSeek)
			{
				this._reader = ContainerReader.CreatePageReader(stream, closeOnDispose, new Func<IPacketProvider, bool>(this.ProcessNewStream));
				this.CanSeek = 1;
				return;
			}
			this._reader = ContainerReader.CreateForwardOnlyPageReader(stream, closeOnDispose, new Func<IPacketProvider, bool>(this.ProcessNewStream));
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00011744 File Offset: 0x0000F944
		public bool TryInit()
		{
			return this.FindNextStream();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0001174C File Offset: 0x0000F94C
		public bool FindNextStream()
		{
			this._reader.Lock();
			bool result;
			try
			{
				this._foundStream = false;
				while (this._reader.ReadNextPage())
				{
					if (this._foundStream)
					{
						return true;
					}
				}
				result = false;
			}
			finally
			{
				this._reader.Release();
			}
			return result;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000117A8 File Offset: 0x0000F9A8
		private bool ProcessNewStream(IPacketProvider packetProvider)
		{
			bool flag = this._reader.Release();
			bool result;
			try
			{
				NewStreamHandler newStreamCallback = this.NewStreamCallback;
				if (newStreamCallback == null || newStreamCallback(packetProvider))
				{
					this._packetProviders.Add(new WeakReference<IPacketProvider>(packetProvider));
					this._foundStream = true;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			finally
			{
				if (flag)
				{
					this._reader.Lock();
				}
			}
			return result;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00011818 File Offset: 0x0000FA18
		public void Dispose()
		{
			IPageReader reader = this._reader;
			if (reader != null)
			{
				reader.Dispose();
			}
			this._reader = null;
		}

		// Token: 0x0400033B RID: 827
		private IPageReader _reader;

		// Token: 0x0400033C RID: 828
		private List<WeakReference<IPacketProvider>> _packetProviders;

		// Token: 0x0400033D RID: 829
		private bool _foundStream;
	}
}
