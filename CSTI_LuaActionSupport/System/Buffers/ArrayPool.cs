using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Buffers
{
	// Token: 0x020000D7 RID: 215
	internal abstract class ArrayPool<T>
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0002FAC1 File Offset: 0x0002DCC1
		public static ArrayPool<T> Shared
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Volatile.Read<ArrayPool<T>>(ref ArrayPool<T>.s_sharedInstance) ?? ArrayPool<T>.EnsureSharedCreated();
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0002FAD6 File Offset: 0x0002DCD6
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static ArrayPool<T> EnsureSharedCreated()
		{
			Interlocked.CompareExchange<ArrayPool<T>>(ref ArrayPool<T>.s_sharedInstance, ArrayPool<T>.Create(), null);
			return ArrayPool<T>.s_sharedInstance;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002FAEE File Offset: 0x0002DCEE
		public static ArrayPool<T> Create()
		{
			return new DefaultArrayPool<T>();
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002FAF5 File Offset: 0x0002DCF5
		public static ArrayPool<T> Create(int maxArrayLength, int maxArraysPerBucket)
		{
			return new DefaultArrayPool<T>(maxArrayLength, maxArraysPerBucket);
		}

		// Token: 0x060007B8 RID: 1976
		public abstract T[] Rent(int minimumLength);

		// Token: 0x060007B9 RID: 1977
		public abstract void Return(T[] array, bool clearArray = false);

		// Token: 0x0400026A RID: 618
		private static ArrayPool<T> s_sharedInstance;
	}
}
