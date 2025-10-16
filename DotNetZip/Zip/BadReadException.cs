using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000039 RID: 57
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000A")]
	[Serializable]
	public class BadReadException : ZipException
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x0000D041 File Offset: 0x0000B241
		public BadReadException()
		{
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000D049 File Offset: 0x0000B249
		public BadReadException(string message) : base(message)
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000D052 File Offset: 0x0000B252
		public BadReadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000D05C File Offset: 0x0000B25C
		protected BadReadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
