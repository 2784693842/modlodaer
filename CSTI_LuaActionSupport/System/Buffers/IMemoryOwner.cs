using System;

namespace System.Buffers
{
	// Token: 0x020000C1 RID: 193
	internal interface IMemoryOwner<T> : IDisposable
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000651 RID: 1617
		System.Memory<T> Memory { get; }
	}
}
