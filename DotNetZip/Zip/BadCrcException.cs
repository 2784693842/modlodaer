using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x0200003A RID: 58
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00009")]
	[Serializable]
	public class BadCrcException : ZipException
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x0000D066 File Offset: 0x0000B266
		public BadCrcException()
		{
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000D06E File Offset: 0x0000B26E
		public BadCrcException(string message) : base(message)
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000D077 File Offset: 0x0000B277
		protected BadCrcException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
