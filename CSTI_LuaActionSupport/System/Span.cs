using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000AD RID: 173
	[DebuggerTypeProxy(typeof(SpanDebugView<>))]
	[DebuggerDisplay("{ToString(),raw}")]
	[DebuggerTypeProxy(typeof(SpanDebugView<>))]
	[DebuggerDisplay("{ToString(),raw}")]
	internal readonly ref struct Span<T>
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00014211 File Offset: 0x00012411
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00014219 File Offset: 0x00012419
		public bool IsEmpty
		{
			get
			{
				return this._length == 0;
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00014224 File Offset: 0x00012424
		public static bool operator !=(System.Span<T> left, System.Span<T> right)
		{
			return !(left == right);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00013DB1 File Offset: 0x00011FB1
		[Obsolete("Equals() on Span will always throw an exception. Use == instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object obj)
		{
			throw new NotSupportedException(SR.NotSupported_CannotCallEqualsOnSpan);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00013DBD File Offset: 0x00011FBD
		[Obsolete("GetHashCode() on Span will always throw an exception.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnSpan);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00014230 File Offset: 0x00012430
		public static implicit operator System.Span<T>(T[] array)
		{
			return new System.Span<T>(array);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00014238 File Offset: 0x00012438
		public static implicit operator System.Span<T>(ArraySegment<T> segment)
		{
			return new System.Span<T>(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00014254 File Offset: 0x00012454
		public static System.Span<T> Empty
		{
			get
			{
				return default(System.Span<T>);
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001426A File Offset: 0x0001246A
		public System.Span<T>.Enumerator GetEnumerator()
		{
			return new System.Span<T>.Enumerator(this);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00014278 File Offset: 0x00012478
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span(T[] array)
		{
			if (array == null)
			{
				this = default(System.Span<T>);
				return;
			}
			if (default(T) == null && array.GetType() != typeof(T[]))
			{
				ThrowHelper.ThrowArrayTypeMismatchException();
			}
			this._length = array.Length;
			this._pinnable = Unsafe.As<Pinnable<T>>(array);
			this._byteOffset = SpanHelpers.PerTypeValues<T>.ArrayAdjustment;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000142DC File Offset: 0x000124DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static System.Span<T> Create(T[] array, int start)
		{
			if (array == null)
			{
				if (start != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				return default(System.Span<T>);
			}
			if (default(T) == null && array.GetType() != typeof(T[]))
			{
				ThrowHelper.ThrowArrayTypeMismatchException();
			}
			if (start > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			IntPtr byteOffset = SpanHelpers.PerTypeValues<T>.ArrayAdjustment.Add(start);
			int length = array.Length - start;
			return new System.Span<T>(Unsafe.As<Pinnable<T>>(array), byteOffset, length);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00014358 File Offset: 0x00012558
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span(T[] array, int start, int length)
		{
			if (array == null)
			{
				if (start != 0 || length != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				this = default(System.Span<T>);
				return;
			}
			if (default(T) == null && array.GetType() != typeof(T[]))
			{
				ThrowHelper.ThrowArrayTypeMismatchException();
			}
			if (start > array.Length || length > array.Length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			this._length = length;
			this._pinnable = Unsafe.As<Pinnable<T>>(array);
			this._byteOffset = SpanHelpers.PerTypeValues<T>.ArrayAdjustment.Add(start);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x000143E0 File Offset: 0x000125E0
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe Span(void* pointer, int length)
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

		// Token: 0x06000571 RID: 1393 RVA: 0x0001441C File Offset: 0x0001261C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Span(Pinnable<T> pinnable, IntPtr byteOffset, int length)
		{
			this._length = length;
			this._pinnable = pinnable;
			this._byteOffset = byteOffset;
		}

		// Token: 0x170000A2 RID: 162
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

		// Token: 0x06000573 RID: 1395 RVA: 0x00014490 File Offset: 0x00012690
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
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

		// Token: 0x06000574 RID: 1396 RVA: 0x000144E0 File Offset: 0x000126E0
		public unsafe void Clear()
		{
			int length = this._length;
			if (length == 0)
			{
				return;
			}
			UIntPtr byteLength = (UIntPtr)((ulong)length * (ulong)((long)Unsafe.SizeOf<T>()));
			if ((Unsafe.SizeOf<T>() & sizeof(IntPtr) - 1) != 0)
			{
				if (this._pinnable == null)
				{
					byte* ptr = (byte*)this._byteOffset.ToPointer();
					SpanHelpers.ClearLessThanPointerSized(ptr, byteLength);
					return;
				}
				ref byte b = ref Unsafe.As<T, byte>(Unsafe.AddByteOffset<T>(ref this._pinnable.Data, this._byteOffset));
				SpanHelpers.ClearLessThanPointerSized(ref b, byteLength);
				return;
			}
			else
			{
				if (SpanHelpers.IsReferenceOrContainsReferences<T>())
				{
					UIntPtr pointerSizeLength = (UIntPtr)((ulong)((long)(length * Unsafe.SizeOf<T>() / sizeof(IntPtr))));
					ref IntPtr ip = ref Unsafe.As<T, IntPtr>(this.DangerousGetPinnableReference());
					SpanHelpers.ClearPointerSizedWithReferences(ref ip, pointerSizeLength);
					return;
				}
				ref byte b2 = ref Unsafe.As<T, byte>(this.DangerousGetPinnableReference());
				SpanHelpers.ClearPointerSizedWithoutReferences(ref b2, byteLength);
				return;
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000145A8 File Offset: 0x000127A8
		public unsafe void Fill(T value)
		{
			int length = this._length;
			if (length == 0)
			{
				return;
			}
			if (Unsafe.SizeOf<T>() != 1)
			{
				ref T source = ref this.DangerousGetPinnableReference();
				int i;
				for (i = 0; i < (length & -8); i += 8)
				{
					*Unsafe.Add<T>(ref source, i) = value;
					*Unsafe.Add<T>(ref source, i + 1) = value;
					*Unsafe.Add<T>(ref source, i + 2) = value;
					*Unsafe.Add<T>(ref source, i + 3) = value;
					*Unsafe.Add<T>(ref source, i + 4) = value;
					*Unsafe.Add<T>(ref source, i + 5) = value;
					*Unsafe.Add<T>(ref source, i + 6) = value;
					*Unsafe.Add<T>(ref source, i + 7) = value;
				}
				if (i < (length & -4))
				{
					*Unsafe.Add<T>(ref source, i) = value;
					*Unsafe.Add<T>(ref source, i + 1) = value;
					*Unsafe.Add<T>(ref source, i + 2) = value;
					*Unsafe.Add<T>(ref source, i + 3) = value;
					i += 4;
				}
				while (i < length)
				{
					*Unsafe.Add<T>(ref source, i) = value;
					i++;
				}
				return;
			}
			byte value2 = *Unsafe.As<T, byte>(ref value);
			if (this._pinnable == null)
			{
				Unsafe.InitBlockUnaligned(this._byteOffset.ToPointer(), value2, (uint)length);
				return;
			}
			ref byte startAddress = ref Unsafe.As<T, byte>(Unsafe.AddByteOffset<T>(ref this._pinnable.Data, this._byteOffset));
			Unsafe.InitBlockUnaligned(ref startAddress, value2, (uint)length);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00014727 File Offset: 0x00012927
		public void CopyTo(System.Span<T> destination)
		{
			if (!this.TryCopyTo(destination))
			{
				ThrowHelper.ThrowArgumentException_DestinationTooShort();
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00014738 File Offset: 0x00012938
		public bool TryCopyTo(System.Span<T> destination)
		{
			int length = this._length;
			int length2 = destination._length;
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

		// Token: 0x06000578 RID: 1400 RVA: 0x00014777 File Offset: 0x00012977
		public static bool operator ==(System.Span<T> left, System.Span<T> right)
		{
			return left._length == right._length && Unsafe.AreSame<T>(left.DangerousGetPinnableReference(), right.DangerousGetPinnableReference());
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001479C File Offset: 0x0001299C
		public static implicit operator System.ReadOnlySpan<T>(System.Span<T> span)
		{
			return new System.ReadOnlySpan<T>(span._pinnable, span._byteOffset, span._length);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x000147B8 File Offset: 0x000129B8
		public unsafe override string ToString()
		{
			if (typeof(T) == typeof(char))
			{
				fixed (char* ptr = Unsafe.As<T, char>(this.DangerousGetPinnableReference()))
				{
					char* value = ptr;
					return new string(value, 0, this._length);
				}
			}
			return string.Format("System.Span<{0}>[{1}]", typeof(T).Name, this._length);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00014824 File Offset: 0x00012A24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public System.Span<T> Slice(int start)
		{
			if (start > this._length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			IntPtr byteOffset = this._byteOffset.Add(start);
			int length = this._length - start;
			return new System.Span<T>(this._pinnable, byteOffset, length);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00014864 File Offset: 0x00012A64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public System.Span<T> Slice(int start, int length)
		{
			if (start > this._length || length > this._length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			IntPtr byteOffset = this._byteOffset.Add(start);
			return new System.Span<T>(this._pinnable, byteOffset, length);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000148A8 File Offset: 0x00012AA8
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

		// Token: 0x0600057E RID: 1406 RVA: 0x000148DC File Offset: 0x00012ADC
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001491B File Offset: 0x00012B1B
		internal Pinnable<T> Pinnable
		{
			get
			{
				return this._pinnable;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00014923 File Offset: 0x00012B23
		internal IntPtr ByteOffset
		{
			get
			{
				return this._byteOffset;
			}
		}

		// Token: 0x040001D7 RID: 471
		private readonly Pinnable<T> _pinnable;

		// Token: 0x040001D8 RID: 472
		private readonly IntPtr _byteOffset;

		// Token: 0x040001D9 RID: 473
		private readonly int _length;

		// Token: 0x020000AE RID: 174
		public ref struct Enumerator
		{
			// Token: 0x06000581 RID: 1409 RVA: 0x0001492B File Offset: 0x00012B2B
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal Enumerator(System.Span<T> span)
			{
				this._span = span;
				this._index = -1;
			}

			// Token: 0x06000582 RID: 1410 RVA: 0x0001493C File Offset: 0x00012B3C
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

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001496A File Offset: 0x00012B6A
			public ref T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this._span[this._index];
				}
			}

			// Token: 0x040001DA RID: 474
			private readonly System.Span<T> _span;

			// Token: 0x040001DB RID: 475
			private int _index;
		}
	}
}
