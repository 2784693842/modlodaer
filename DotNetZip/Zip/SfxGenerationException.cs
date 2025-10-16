using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x0200003B RID: 59
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00008")]
	[Serializable]
	public class SfxGenerationException : ZipException
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x0000D081 File Offset: 0x0000B281
		public SfxGenerationException()
		{
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000D089 File Offset: 0x0000B289
		public SfxGenerationException(string message) : base(message)
		{
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000D092 File Offset: 0x0000B292
		protected SfxGenerationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
