using System;
using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000BF RID: 191
	[DebuggerTypeProxy(typeof(MemoryDebugView<>))]
	[DebuggerDisplay("{ToString(),raw}")]
	internal readonly struct ReadOnlyMemory<T>
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x000164FB File Offset: 0x000146FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlyMemory(T[] array)
		{
			if (array == null)
			{
				this = default(System.ReadOnlyMemory<T>);
				return;
			}
			this._object = array;
			this._index = 0;
			this._length = array.Length;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001651F File Offset: 0x0001471F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlyMemory(T[] array, int start, int length)
		{
			if (array == null)
			{
				if (start != 0 || length != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				this = default(System.ReadOnlyMemory<T>);
				return;
			}
			if (start > array.Length || length > array.Length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._object = array;
			this._index = start;
			this._length = length;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001655F File Offset: 0x0001475F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ReadOnlyMemory(object obj, int start, int length)
		{
			this._object = obj;
			this._index = start;
			this._length = length;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00016576 File Offset: 0x00014776
		public static implicit operator System.ReadOnlyMemory<T>(T[] array)
		{
			return new System.ReadOnlyMemory<T>(array);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001657E File Offset: 0x0001477E
		public static implicit operator System.ReadOnlyMemory<T>(ArraySegment<T> segment)
		{
			return new System.ReadOnlyMemory<T>(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001659C File Offset: 0x0001479C
		public static System.ReadOnlyMemory<T> Empty
		{
			get
			{
				return default(System.ReadOnlyMemory<T>);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x000165B2 File Offset: 0x000147B2
		public int Length
		{
			get
			{
				return this._length & int.MaxValue;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000165C0 File Offset: 0x000147C0
		public bool IsEmpty
		{
			get
			{
				return (this._length & int.MaxValue) == 0;
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000165D4 File Offset: 0x000147D4
		public override string ToString()
		{
			if (!(typeof(T) == typeof(char)))
			{
				return string.Format("System.ReadOnlyMemory<{0}>[{1}]", typeof(T).Name, this._length & int.MaxValue);
			}
			string text;
			if ((text = (this._object as string)) == null)
			{
				return this.Span.ToString();
			}
			return text.Substring(this._index, this._length & int.MaxValue);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00016664 File Offset: 0x00014864
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public System.ReadOnlyMemory<T> Slice(int start)
		{
			int length = this._length;
			int num = length & int.MaxValue;
			if (start > num)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.ReadOnlyMemory<T>(this._object, this._index + start, length - start);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000166A0 File Offset: 0x000148A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public System.ReadOnlyMemory<T> Slice(int start, int length)
		{
			int length2 = this._length;
			int num = this._length & int.MaxValue;
			if (start > num || length > num - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.ReadOnlyMemory<T>(this._object, this._index + start, length | (length2 & int.MinValue));
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x000166F0 File Offset: 0x000148F0
		public System.ReadOnlySpan<T> Span
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (this._index < 0)
				{
					return ((MemoryManager<T>)this._object).GetSpan().Slice(this._index & int.MaxValue, this._length);
				}
				string text;
				if (typeof(T) == typeof(char) && (text = (this._object as string)) != null)
				{
					return new System.ReadOnlySpan<T>(Unsafe.As<Pinnable<T>>(text), MemoryExtensions.StringAdjustment, text.Length).Slice(this._index, this._length);
				}
				if (this._object != null)
				{
					return new System.ReadOnlySpan<T>((T[])this._object, this._index, this._length & int.MaxValue);
				}
				return default(System.ReadOnlySpan<T>);
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000167C0 File Offset: 0x000149C0
		public void CopyTo(System.Memory<T> destination)
		{
			this.Span.CopyTo(destination.Span);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000167E4 File Offset: 0x000149E4
		public bool TryCopyTo(System.Memory<T> destination)
		{
			return this.Span.TryCopyTo(destination.Span);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00016808 File Offset: 0x00014A08
		public unsafe System.Buffers.MemoryHandle Pin()
		{
			if (this._index < 0)
			{
				return ((MemoryManager<T>)this._object).Pin(this._index & int.MaxValue);
			}
			string value;
			if (typeof(T) == typeof(char) && (value = (this._object as string)) != null)
			{
				GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
				void* pointer = Unsafe.Add<T>((void*)handle.AddrOfPinnedObject(), this._index);
				return new System.Buffers.MemoryHandle(pointer, handle, null);
			}
			T[] array;
			if ((array = (this._object as T[])) == null)
			{
				return default(System.Buffers.MemoryHandle);
			}
			if (this._length < 0)
			{
				void* pointer2 = Unsafe.Add<T>(Unsafe.AsPointer<T>(MemoryMarshal.GetReference<T>(array)), this._index);
				return new System.Buffers.MemoryHandle(pointer2, default(GCHandle), null);
			}
			GCHandle handle2 = GCHandle.Alloc(array, GCHandleType.Pinned);
			void* pointer3 = Unsafe.Add<T>((void*)handle2.AddrOfPinnedObject(), this._index);
			return new System.Buffers.MemoryHandle(pointer3, handle2, null);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00016910 File Offset: 0x00014B10
		public T[] ToArray()
		{
			return this.Span.ToArray();
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001692C File Offset: 0x00014B2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object obj)
		{
			if (obj is System.ReadOnlyMemory<T>)
			{
				System.ReadOnlyMemory<T> other = (System.ReadOnlyMemory<T>)obj;
				return this.Equals(other);
			}
			if (obj is System.Memory<T>)
			{
				System.Memory<T> memory = (System.Memory<T>)obj;
				return this.Equals(memory);
			}
			return false;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00016971 File Offset: 0x00014B71
		public bool Equals(System.ReadOnlyMemory<T> other)
		{
			return this._object == other._object && this._index == other._index && this._length == other._length;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000169A0 File Offset: 0x00014BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			if (this._object == null)
			{
				return 0;
			}
			return System.ReadOnlyMemory<T>.CombineHashCodes(this._object.GetHashCode(), this._index.GetHashCode(), this._length.GetHashCode());
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000169E3 File Offset: 0x00014BE3
		private static int CombineHashCodes(int left, int right)
		{
			return (left << 5) + left ^ right;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000169EC File Offset: 0x00014BEC
		private static int CombineHashCodes(int h1, int h2, int h3)
		{
			return System.ReadOnlyMemory<T>.CombineHashCodes(System.ReadOnlyMemory<T>.CombineHashCodes(h1, h2), h3);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000169FB File Offset: 0x00014BFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal object GetObjectStartLength(out int start, out int length)
		{
			start = this._index;
			length = this._length;
			return this._object;
		}

		// Token: 0x04000209 RID: 521
		private readonly object _object;

		// Token: 0x0400020A RID: 522
		private readonly int _index;

		// Token: 0x0400020B RID: 523
		private readonly int _length;

		// Token: 0x0400020C RID: 524
		internal const int RemoveFlagsBitMask = 2147483647;
	}
}
