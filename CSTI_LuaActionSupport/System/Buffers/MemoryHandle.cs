using System;
using System.Runtime.InteropServices;

namespace System.Buffers
{
	// Token: 0x020000C3 RID: 195
	internal struct MemoryHandle : IDisposable
	{
		// Token: 0x06000654 RID: 1620 RVA: 0x0001703E File Offset: 0x0001523E
		[CLSCompliant(false)]
		public unsafe MemoryHandle(void* pointer, GCHandle handle = default(GCHandle), IPinnable pinnable = null)
		{
			this._pointer = pointer;
			this._handle = handle;
			this._pinnable = pinnable;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00017055 File Offset: 0x00015255
		[CLSCompliant(false)]
		public unsafe void* Pointer
		{
			get
			{
				return this._pointer;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001705D File Offset: 0x0001525D
		public void Dispose()
		{
			if (this._handle.IsAllocated)
			{
				this._handle.Free();
			}
			if (this._pinnable != null)
			{
				this._pinnable.Unpin();
				this._pinnable = null;
			}
			this._pointer = null;
		}

		// Token: 0x04000211 RID: 529
		private unsafe void* _pointer;

		// Token: 0x04000212 RID: 530
		private GCHandle _handle;

		// Token: 0x04000213 RID: 531
		private IPinnable _pinnable;
	}
}
