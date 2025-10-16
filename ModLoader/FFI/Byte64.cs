using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ModLoader.FFI
{
	// Token: 0x0200002B RID: 43
	[StructLayout(LayoutKind.Explicit)]
	public struct Byte64
	{
		// Token: 0x040000B2 RID: 178
		[FixedBuffer(typeof(byte), 64)]
		[FieldOffset(0)]
		public Byte64.<Data>e__FixedBuffer Data;

		// Token: 0x0200002C RID: 44
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 64)]
		public struct <Data>e__FixedBuffer
		{
			// Token: 0x040000B3 RID: 179
			public byte FixedElementField;
		}
	}
}
