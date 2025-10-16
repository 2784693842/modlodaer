using System;
using System.Collections.Generic;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x02000087 RID: 135
	internal class ForwardOnlyPageReader : PageReaderBase
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00011E49 File Offset: 0x00010049
		// (set) Token: 0x0600031F RID: 799 RVA: 0x00011E50 File Offset: 0x00010050
		internal static Func<IPageReader, int, IForwardOnlyPacketProvider> CreatePacketProvider { get; set; } = (IPageReader pr, int ss) => new ForwardOnlyPacketProvider(pr, ss);

		// Token: 0x06000320 RID: 800 RVA: 0x00011E58 File Offset: 0x00010058
		public ForwardOnlyPageReader(Stream stream, bool closeOnDispose, Func<IPacketProvider, bool> newStreamCallback)
		{
			this._packetProviders = new Dictionary<int, IForwardOnlyPacketProvider>();
			base..ctor(stream, closeOnDispose);
			this._newStreamCallback = newStreamCallback;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00011E74 File Offset: 0x00010074
		protected override bool AddPage(int streamSerial, byte[] pageBuf, bool isResync)
		{
			IForwardOnlyPacketProvider forwardOnlyPacketProvider;
			if (!this._packetProviders.TryGetValue(streamSerial, out forwardOnlyPacketProvider))
			{
				forwardOnlyPacketProvider = ForwardOnlyPageReader.CreatePacketProvider(this, streamSerial);
				if (forwardOnlyPacketProvider.AddPage(pageBuf, isResync))
				{
					this._packetProviders.Add(streamSerial, forwardOnlyPacketProvider);
					if (this._newStreamCallback(forwardOnlyPacketProvider))
					{
						return true;
					}
					this._packetProviders.Remove(streamSerial);
				}
				return false;
			}
			if (forwardOnlyPacketProvider.AddPage(pageBuf, isResync))
			{
				if ((pageBuf[5] & 4) != 0)
				{
					forwardOnlyPacketProvider.SetEndOfStream();
					this._packetProviders.Remove(streamSerial);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00011EFC File Offset: 0x000100FC
		protected override void SetEndOfStreams()
		{
			foreach (KeyValuePair<int, IForwardOnlyPacketProvider> keyValuePair in this._packetProviders)
			{
				keyValuePair.Value.SetEndOfStream();
			}
			this._packetProviders.Clear();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00007CE8 File Offset: 0x00005EE8
		public override bool ReadPageAt(long offset)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400034D RID: 845
		private readonly Dictionary<int, IForwardOnlyPacketProvider> _packetProviders;

		// Token: 0x0400034E RID: 846
		private readonly Func<IPacketProvider, bool> _newStreamCallback;
	}
}
