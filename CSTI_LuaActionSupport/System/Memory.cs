using System;
using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C0 RID: 192
	[DebuggerTypeProxy(typeof(MemoryDebugView<>))]
	[DebuggerDisplay("{ToString(),raw}")]
	internal readonly struct Memory<T>
	{
		// Token: 0x06000638 RID: 1592 RVA: 0x00016A14 File Offset: 0x00014C14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Memory(T[] array)
		{
			if (array == null)
			{
				this = default(System.Memory<T>);
				return;
			}
			if (default(T) == null && array.GetType() != typeof(T[]))
			{
				ThrowHelper.ThrowArrayTypeMismatchException();
			}
			this._object = array;
			this._index = 0;
			this._length = array.Length;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00016A70 File Offset: 0x00014C70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Memory(T[] array, int start)
		{
			if (array == null)
			{
				if (start != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				this = default(System.Memory<T>);
				return;
			}
			if (default(T) == null && array.GetType() != typeof(T[]))
			{
				ThrowHelper.ThrowArrayTypeMismatchException();
			}
			if (start > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._object = array;
			this._index = start;
			this._length = array.Length - start;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00016AE0 File Offset: 0x00014CE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Memory(T[] array, int start, int length)
		{
			if (array == null)
			{
				if (start != 0 || length != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				this = default(System.Memory<T>);
				return;
			}
			if (default(T) == null && array.GetType() != typeof(T[]))
			{
				ThrowHelper.ThrowArrayTypeMismatchException();
			}
			if (start > array.Length || length > array.Length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._object = array;
			this._index = start;
			this._length = length;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00016B57 File Offset: 0x00014D57
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Memory(MemoryManager<T> manager, int length)
		{
			if (length < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._object = manager;
			this._index = int.MinValue;
			this._length = length;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00016B7B File Offset: 0x00014D7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Memory(MemoryManager<T> manager, int start, int length)
		{
			if (length < 0 || start < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._object = manager;
			this._index = (start | int.MinValue);
			this._length = length;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00016BA5 File Offset: 0x00014DA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Memory(object obj, int start, int length)
		{
			this._object = obj;
			this._index = start;
			this._length = length;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00016BBC File Offset: 0x00014DBC
		public static implicit operator System.Memory<T>(T[] array)
		{
			return new System.Memory<T>(array);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00016BC4 File Offset: 0x00014DC4
		public static implicit operator System.Memory<T>(ArraySegment<T> segment)
		{
			return new System.Memory<T>(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00016BE0 File Offset: 0x00014DE0
		public unsafe static implicit operator System.ReadOnlyMemory<T>(System.Memory<T> memory)
		{
			return *Unsafe.As<System.Memory<T>, System.ReadOnlyMemory<T>>(ref memory);
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00016BF0 File Offset: 0x00014DF0
		public static System.Memory<T> Empty
		{
			get
			{
				return default(System.Memory<T>);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x00016C06 File Offset: 0x00014E06
		public int Length
		{
			get
			{
				return this._length & int.MaxValue;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00016C14 File Offset: 0x00014E14
		public bool IsEmpty
		{
			get
			{
				return (this._length & int.MaxValue) == 0;
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00016C28 File Offset: 0x00014E28
		public override string ToString()
		{
			if (!(typeof(T) == typeof(char)))
			{
				return string.Format("System.Memory<{0}>[{1}]", typeof(T).Name, this._length & int.MaxValue);
			}
			string text;
			if ((text = (this._object as string)) == null)
			{
				return this.Span.ToString();
			}
			return text.Substring(this._index, this._length & int.MaxValue);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00016CB8 File Offset: 0x00014EB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public System.Memory<T> Slice(int start)
		{
			int length = this._length;
			int num = length & int.MaxValue;
			if (start > num)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.Memory<T>(this._object, this._index + start, length - start);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00016CF4 File Offset: 0x00014EF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public System.Memory<T> Slice(int start, int length)
		{
			int length2 = this._length;
			int num = length2 & int.MaxValue;
			if (start > num || length > num - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			return new System.Memory<T>(this._object, this._index + start, length | (length2 & int.MinValue));
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00016D3C File Offset: 0x00014F3C
		public System.Span<T> Span
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
					return new System.Span<T>(Unsafe.As<Pinnable<T>>(text), MemoryExtensions.StringAdjustment, text.Length).Slice(this._index, this._length);
				}
				if (this._object != null)
				{
					return new System.Span<T>((T[])this._object, this._index, this._length & int.MaxValue);
				}
				return default(System.Span<T>);
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00016E08 File Offset: 0x00015008
		public void CopyTo(System.Memory<T> destination)
		{
			this.Span.CopyTo(destination.Span);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00016E2C File Offset: 0x0001502C
		public bool TryCopyTo(System.Memory<T> destination)
		{
			return this.Span.TryCopyTo(destination.Span);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00016E50 File Offset: 0x00015050
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

		// Token: 0x0600064B RID: 1611 RVA: 0x00016F58 File Offset: 0x00015158
		public T[] ToArray()
		{
			return this.Span.ToArray();
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00016F74 File Offset: 0x00015174
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Equals(object obj)
		{
			if (obj is System.ReadOnlyMemory<T>)
			{
				return ((System.ReadOnlyMemory<T>)obj).Equals(this);
			}
			if (obj is System.Memory<T>)
			{
				System.Memory<T> other = (System.Memory<T>)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00016FBD File Offset: 0x000151BD
		public bool Equals(System.Memory<T> other)
		{
			return this._object == other._object && this._index == other._index && this._length == other._length;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00016FEC File Offset: 0x000151EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetHashCode()
		{
			if (this._object == null)
			{
				return 0;
			}
			return System.Memory<T>.CombineHashCodes(this._object.GetHashCode(), this._index.GetHashCode(), this._length.GetHashCode());
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x000169E3 File Offset: 0x00014BE3
		private static int CombineHashCodes(int left, int right)
		{
			return (left << 5) + left ^ right;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001702F File Offset: 0x0001522F
		private static int CombineHashCodes(int h1, int h2, int h3)
		{
			return System.Memory<T>.CombineHashCodes(System.Memory<T>.CombineHashCodes(h1, h2), h3);
		}

		// Token: 0x0400020D RID: 525
		private readonly object _object;

		// Token: 0x0400020E RID: 526
		private readonly int _index;

		// Token: 0x0400020F RID: 527
		private readonly int _length;

		// Token: 0x04000210 RID: 528
		private const int RemoveFlagsBitMask = 2147483647;
	}
}
