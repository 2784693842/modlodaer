using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x0200005F RID: 95
internal sealed class <>z__ReadOnlyArray<T> : IEnumerable, IEnumerable<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection<T>, IList<T>
{
	// Token: 0x060001DC RID: 476 RVA: 0x0000991E File Offset: 0x00007B1E
	public <>z__ReadOnlyArray(T[] items)
	{
		this._items = items;
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000992D File Offset: 0x00007B2D
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this._items.GetEnumerator();
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000993A File Offset: 0x00007B3A
	IEnumerator<T> IEnumerable<!0>.GetEnumerator()
	{
		return this._items.GetEnumerator();
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060001DF RID: 479 RVA: 0x00009947 File Offset: 0x00007B47
	int IReadOnlyCollection<!0>.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x17000060 RID: 96
	T IReadOnlyList<!0>.this[int index]
	{
		get
		{
			return this._items[index];
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060001E1 RID: 481 RVA: 0x00009947 File Offset: 0x00007B47
	int ICollection<!0>.Count
	{
		get
		{
			return this._items.Length;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000995F File Offset: 0x00007B5F
	bool ICollection<!0>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x000046D5 File Offset: 0x000028D5
	void ICollection<!0>.Add(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x000046D5 File Offset: 0x000028D5
	void ICollection<!0>.Clear()
	{
		throw new NotSupportedException();
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x00009962 File Offset: 0x00007B62
	bool ICollection<!0>.Contains(T item)
	{
		return this._items.Contains(item);
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00009970 File Offset: 0x00007B70
	void ICollection<!0>.CopyTo(T[] array, int arrayIndex)
	{
		this._items.CopyTo(array, arrayIndex);
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x000046D5 File Offset: 0x000028D5
	bool ICollection<!0>.Remove(T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x17000063 RID: 99
	T IList<!0>.this[int index]
	{
		get
		{
			return this._items[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000997F File Offset: 0x00007B7F
	int IList<!0>.IndexOf(T item)
	{
		return this._items.IndexOf(item);
	}

	// Token: 0x060001EB RID: 491 RVA: 0x000046D5 File Offset: 0x000028D5
	void IList<!0>.Insert(int index, T item)
	{
		throw new NotSupportedException();
	}

	// Token: 0x060001EC RID: 492 RVA: 0x000046D5 File Offset: 0x000028D5
	void IList<!0>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	// Token: 0x040000DF RID: 223
	[CompilerGenerated]
	private readonly T[] _items;
}
