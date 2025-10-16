using System;
using System.Diagnostics;

namespace System
{
	// Token: 0x020000C5 RID: 197
	internal sealed class MemoryDebugView<T>
	{
		// Token: 0x06000661 RID: 1633 RVA: 0x000170E9 File Offset: 0x000152E9
		public MemoryDebugView(System.Memory<T> memory)
		{
			this._memory = memory;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000170FD File Offset: 0x000152FD
		public MemoryDebugView(System.ReadOnlyMemory<T> memory)
		{
			this._memory = memory;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001710C File Offset: 0x0001530C
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this._memory.ToArray();
			}
		}

		// Token: 0x04000214 RID: 532
		private readonly System.ReadOnlyMemory<T> _memory;
	}
}
