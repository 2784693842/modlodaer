using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000D2 RID: 210
	internal struct NUInt
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x0002D30A File Offset: 0x0002B50A
		private NUInt(uint value)
		{
			this._value = value;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002D30A File Offset: 0x0002B50A
		private NUInt(ulong value)
		{
			this._value = value;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002D314 File Offset: 0x0002B514
		public static implicit operator NUInt(uint value)
		{
			return new NUInt(value);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002D31C File Offset: 0x0002B51C
		public static implicit operator IntPtr(NUInt value)
		{
			return (IntPtr)value._value;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002D314 File Offset: 0x0002B514
		public static explicit operator NUInt(int value)
		{
			return new NUInt((uint)value);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002D329 File Offset: 0x0002B529
		public unsafe static explicit operator void*(NUInt value)
		{
			return value._value;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002D331 File Offset: 0x0002B531
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NUInt operator *(NUInt left, NUInt right)
		{
			if (sizeof(IntPtr) != 4)
			{
				return new NUInt(left._value * right._value);
			}
			return new NUInt(left._value * right._value);
		}

		// Token: 0x04000267 RID: 615
		private unsafe readonly void* _value;
	}
}
