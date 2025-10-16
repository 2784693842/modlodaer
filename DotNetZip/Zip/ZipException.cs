using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x0200003D RID: 61
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00006")]
	[Serializable]
	public class ZipException : Exception
	{
		// Token: 0x060001CE RID: 462 RVA: 0x0000D0C1 File Offset: 0x0000B2C1
		public ZipException()
		{
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000D0C9 File Offset: 0x0000B2C9
		public ZipException(string message) : base(message)
		{
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000D0D2 File Offset: 0x0000B2D2
		public ZipException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000D0DC File Offset: 0x0000B2DC
		protected ZipException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
