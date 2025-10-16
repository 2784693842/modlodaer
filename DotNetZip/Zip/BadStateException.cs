using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x0200003C RID: 60
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00007")]
	[Serializable]
	public class BadStateException : ZipException
	{
		// Token: 0x060001CA RID: 458 RVA: 0x0000D09C File Offset: 0x0000B29C
		public BadStateException()
		{
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000D0A4 File Offset: 0x0000B2A4
		public BadStateException(string message) : base(message)
		{
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000D0AD File Offset: 0x0000B2AD
		public BadStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000D0B7 File Offset: 0x0000B2B7
		protected BadStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
