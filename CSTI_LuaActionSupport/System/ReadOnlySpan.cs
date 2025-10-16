using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000A8 RID: 168
	[DebuggerTypeProxy(typeof(SpanDebugView<>))]
	[DebuggerDisplay("{ToString(),raw}")]
	[DebuggerTypeProxy(typeof(SpanDebugView<>))]
	[DebuggerDisplay("{ToString(),raw}")]
	internal readonly ref struct ReadOnlySpan<T>
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x00013D92 File Offset: 0x00011F92
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00013D9A File Offset: 0x00011F9A
		public bool IsEmpty
		{
			get
			{
				return this._length == 0;
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00013DA5 File Offset: 0x00011FA5
		public static bool operator !=(System.ReadOnlySpan<T> left, System.ReadOnlySpan<T> right)
		{
			return !(left == right);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00013DB1 File Offset: 0x00011FB1
		[Obsolete("Equals() on ReadOnlySpan will always throw an exception. Use == instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object obj)
		{
			throw new NotSupportedException(SR.NotSupported_CannotCallEqualsOnSpan);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00013DBD File Offset: 0x00011FBD
		[Obsolete("GetHashCode() on ReadOnlySpan will always throw an exception.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnSpan);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00013DC9 File Offset: 0x00011FC9
		public static implicit operator System.ReadOnlySpan<T>(T[] array)
		{
			return new System.ReadOnlySpan<T>(array);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00013DD1 File Offset: 0x00011FD1
		public static implicit operator System.ReadOnlySpan<T>(ArraySegment<T> segment)
		{
			return new System.ReadOnlySpan<T>(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00013DF0 File Offset: 0x00011FF0
		public static System.ReadOnlySpan<T> Empty
		{
			get
			{
				return default(System.ReadOnlySpan<T>);
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00013E06 File Offset: 0x00012006
		public System.ReadOnlySpan<T>.Enumerator GetEnumerator()
		{
			return new System.ReadOnlySpan<T>.Enumerator(this);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00013E13 File Offset: 0x00012013
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan(T[] array)
		{
			if (array == null)
			{
				this = default(System.ReadOnlySpan<T>);
				return;
			}
			this._length = array.Length;
			this._pinnable = Unsafe.As<Pinnable<T>>(array);
			this._byteOffset = SpanHelpers.PerTypeValues<T>.ArrayAdjustment;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00013E40 File Offset: 0x00012040
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan(T[] array, int start, int length)
		{
			if (array == null)
			{
				if (start != 0 || length != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				this = default(System.ReadOnlySpan<T>);
				return;
			}
			if (start > array.Length || length > array.Length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			this._length = length;
			this._pinnable = Unsafe.As<Pinnable<T>>(array);
			this._byteOffset = SpanHelpers.PerTypeValues<T>.ArrayAdjustment.Add(start);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00013E9C File Offset: 0x0001209C
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ReadOnlySpan(void* pointer, int length)
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<T>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
			}
			if (length < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			this._length = length;
			this._pinnable = null;
			this._byteOffset = new IntPtr(pointer);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00013ED8 File Offset: 0x000120D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlySpan(Pinnable<T> pinnable, IntPtr byteOffset, int length)
		{
			this._length = length;
			this._pinnable = pinnable;
			this._byteOffset = byteOffset;
		}

		// Token: 0x1700009B RID: 155
		public T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= this._length)
				{
					ThrowHelper.ThrowIndexOutOfRangeException();
				}
				if (this._pinnable == null)
				{
					return Unsafe.Add<T>(Unsafe.AsRef<T>(this._byteOffset.ToPointer()), index);
				}
				return Unsafe.Add<T>(Unsafe.AddByteOffset<T>(ref this._pinnable.Data, this._byteOffset), index);
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00013F4C File Offset: 0x0001214C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref readonly T GetPinnableReference()
		{
			if (this._length == 0)
			{
				return Unsafe.AsRef<T>(null);
			}
			if (this._pinnable == null)
			{
				return Unsafe.AsRef<T>(this._byteOffset.ToPointer());
			}
			return Unsafe.AddByteOffset<T>(ref this._pinnable.Data, this._byteOffset);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00013F9B File Offset: 0x0001219B
		public void CopyTo(System.Span<T> destination)
		{
			if (!this.TryCopyTo(destination))
			{
				ThrowHelper.ThrowArgumentException_DestinationTooShort();
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00013FAC File Offset: 0x000121AC
		public bool TryCopyTo(System.Span<T> destination)
		{
			int length = this._length;
			int length2 = destination.Length;
			if (length == 0)
			{
				return true;
			}
			if (length > length2)
			{
				return false;
			}
			ref T src = ref this.DangerousGetPinnableReference();
			ref T dst = ref destination.DangerousGetPinnableReference();
			SpanHelpers.CopyTo<T>(ref dst, length2, ref src, length);
			return true;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00013FEC File Offset: 0x000121EC
		public static bool operator ==(System.ReadOnlySpan<T> left, System.ReadOnlySpan<T> right)
		{
			return left._length == right._length && Unsafe.AreSame<T>(left.DangerousGetPinnableReference(), right.DangerousGetPinnableReference());
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00014014 File Offset: 0x00012214
		public unsafe override string ToString()
		{
			if (typeof(T) == typeof(char))
			{
				if (this._byteOffset == MemoryExtensions.StringAdjustment)
				{
					object obj = Unsafe.As<object>(this._pinnable);
					string text;
					if ((text = (obj as string)) != null && this._length == text.Length)
					{
						return text;
					}
				}
				fixed (char* ptr = Unsafe.As<T, char>(this.DangerousGetPinnableReference()))
				{
					char* value = ptr;
					return new string(value, 0, this._length);
				}
			}
			return string.Format("System.ReadOnlySpan<{0}>[{1}]", typeof(T).Name, this._length);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000140B8 File Offset: 0x000122B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public System.ReadOnlySpan<T> Slice(int start)
		{
			if (start > this._length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			IntPtr byteOffset = this._byteOffset.Add(start);
			int length = this._length - start;
			return new System.ReadOnlySpan<T>(this._pinnable, byteOffset, length);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000140F8 File Offset: 0x000122F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public System.ReadOnlySpan<T> Slice(int start, int length)
		{
			if (start > this._length || length > this._length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			IntPtr byteOffset = this._byteOffset.Add(start);
			return new System.ReadOnlySpan<T>(this._pinnable, byteOffset, length);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001413C File Offset: 0x0001233C
		public T[] ToArray()
		{
			if (this._length == 0)
			{
				return SpanHelpers.PerTypeValues<T>.EmptyArray;
			}
			T[] array = new T[this._length];
			this.CopyTo(array);
			return array;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00014170 File Offset: 0x00012370
		[EditorBrowsable(EditorBrowsableState.Never)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ref T DangerousGetPinnableReference()
		{
			if (this._pinnable == null)
			{
				return Unsafe.AsRef<T>(this._byteOffset.ToPointer());
			}
			return Unsafe.AddByteOffset<T>(ref this._pinnable.Data, this._byteOffset);
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x000141AF File Offset: 0x000123AF
		internal Pinnable<T> Pinnable
		{
			get
			{
				return this._pinnable;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x000141B7 File Offset: 0x000123B7
		internal IntPtr ByteOffset
		{
			get
			{
				return this._byteOffset;
			}
		}

		// Token: 0x040001D1 RID: 465
		private readonly Pinnable<T> _pinnable;

		// Token: 0x040001D2 RID: 466
		private readonly IntPtr _byteOffset;

		// Token: 0x040001D3 RID: 467
		private readonly int _length;

		// Token: 0x020000A9 RID: 169
		public ref struct Enumerator
		{
			// Token: 0x0600055E RID: 1374 RVA: 0x000141BF File Offset: 0x000123BF
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal Enumerator(System.ReadOnlySpan<T> span)
			{
				this._span = span;
				this._index = -1;
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x000141D0 File Offset: 0x000123D0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				int num = this._index + 1;
				if (num < this._span.Length)
				{
					this._index = num;
					return true;
				}
				return false;
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x06000560 RID: 1376 RVA: 0x000141FE File Offset: 0x000123FE
			public ref readonly T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this._span[this._index];
				}
			}

			// Token: 0x040001D4 RID: 468
			private readonly System.ReadOnlySpan<T> _span;

			// Token: 0x040001D5 RID: 469
			private int _index;
		}
	}
}
