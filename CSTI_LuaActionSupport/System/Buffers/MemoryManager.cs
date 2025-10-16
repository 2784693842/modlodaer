using System;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
	// Token: 0x020000C4 RID: 196
	internal abstract class MemoryManager<T> : IMemoryOwner<T>, IDisposable, IPinnable
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001709C File Offset: 0x0001529C
		public virtual System.Memory<T> Memory
		{
			get
			{
				return new System.Memory<T>(this, this.GetSpan().Length);
			}
		}

		// Token: 0x06000658 RID: 1624
		public abstract System.Span<T> GetSpan();

		// Token: 0x06000659 RID: 1625
		public abstract MemoryHandle Pin(int elementIndex = 0);

		// Token: 0x0600065A RID: 1626
		public abstract void Unpin();

		// Token: 0x0600065B RID: 1627 RVA: 0x000170BD File Offset: 0x000152BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected System.Memory<T> CreateMemory(int length)
		{
			return new System.Memory<T>(this, length);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000170C6 File Offset: 0x000152C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected System.Memory<T> CreateMemory(int start, int length)
		{
			return new System.Memory<T>(this, start, length);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x000170D0 File Offset: 0x000152D0
		protected internal virtual bool TryGetArray(out ArraySegment<T> segment)
		{
			segment = default(ArraySegment<T>);
			return false;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000170DA File Offset: 0x000152DA
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600065F RID: 1631
		protected abstract void Dispose(bool disposing);
	}
}
