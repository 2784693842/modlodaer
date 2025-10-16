using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000038 RID: 56
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000B")]
	[Serializable]
	public class BadPasswordException : ZipException
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0000D01C File Offset: 0x0000B21C
		public BadPasswordException()
		{
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000D024 File Offset: 0x0000B224
		public BadPasswordException(string message) : base(message)
		{
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000D02D File Offset: 0x0000B22D
		public BadPasswordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000D037 File Offset: 0x0000B237
		protected BadPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
