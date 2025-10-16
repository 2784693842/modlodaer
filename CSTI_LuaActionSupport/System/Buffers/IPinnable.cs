using System;

namespace System.Buffers
{
	// Token: 0x020000C2 RID: 194
	internal interface IPinnable
	{
		// Token: 0x06000652 RID: 1618
		MemoryHandle Pin(int elementIndex);

		// Token: 0x06000653 RID: 1619
		void Unpin();
	}
}
