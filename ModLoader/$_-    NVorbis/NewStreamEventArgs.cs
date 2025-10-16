using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000055 RID: 85
	[Serializable]
	internal class NewStreamEventArgs : EventArgs
	{
		// Token: 0x0600016B RID: 363 RVA: 0x0000C218 File Offset: 0x0000A418
		public NewStreamEventArgs(IStreamDecoder streamDecoder)
		{
			if (streamDecoder == null)
			{
				throw new ArgumentNullException("streamDecoder");
			}
			this.StreamDecoder = streamDecoder;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000C236 File Offset: 0x0000A436
		public IStreamDecoder StreamDecoder { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000C23E File Offset: 0x0000A43E
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000C246 File Offset: 0x0000A446
		public bool IgnoreStream { get; set; }
	}
}
