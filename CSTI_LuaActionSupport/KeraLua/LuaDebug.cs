using System;
using System.Runtime.InteropServices;
using System.Text;

namespace KeraLua
{
	// Token: 0x02000064 RID: 100
	internal struct LuaDebug
	{
		// Token: 0x06000250 RID: 592 RVA: 0x0000B125 File Offset: 0x00009325
		public static LuaDebug FromIntPtr(IntPtr ar)
		{
			return Marshal.PtrToStructure<LuaDebug>(ar);
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000B12D File Offset: 0x0000932D
		public string Name
		{
			get
			{
				return Marshal.PtrToStringAnsi(this.name);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000B13A File Offset: 0x0000933A
		public string NameWhat
		{
			get
			{
				return Marshal.PtrToStringAnsi(this.what);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000B13A File Offset: 0x0000933A
		public string What
		{
			get
			{
				return Marshal.PtrToStringAnsi(this.what);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000B147 File Offset: 0x00009347
		public string Source
		{
			get
			{
				return Marshal.PtrToStringAnsi(this.source, this.SourceLength);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000B15A File Offset: 0x0000935A
		public int SourceLength
		{
			get
			{
				return this.sourceLen.ToInt32();
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000B168 File Offset: 0x00009368
		public string ShortSource
		{
			get
			{
				if (this.shortSource[0] == 0)
				{
					return string.Empty;
				}
				int num = 0;
				while (num < this.shortSource.Length && this.shortSource[num] != 0)
				{
					num++;
				}
				return Encoding.ASCII.GetString(this.shortSource, 0, num);
			}
		}

		// Token: 0x040000F0 RID: 240
		[MarshalAs(UnmanagedType.I4)]
		public LuaHookEvent Event;

		// Token: 0x040000F1 RID: 241
		private IntPtr name;

		// Token: 0x040000F2 RID: 242
		private IntPtr nameWhat;

		// Token: 0x040000F3 RID: 243
		private IntPtr what;

		// Token: 0x040000F4 RID: 244
		private IntPtr source;

		// Token: 0x040000F5 RID: 245
		private IntPtr sourceLen;

		// Token: 0x040000F6 RID: 246
		public int CurrentLine;

		// Token: 0x040000F7 RID: 247
		public int LineDefined;

		// Token: 0x040000F8 RID: 248
		public int LastLineDefined;

		// Token: 0x040000F9 RID: 249
		public byte NumberUpValues;

		// Token: 0x040000FA RID: 250
		public byte NumberParameters;

		// Token: 0x040000FB RID: 251
		[MarshalAs(UnmanagedType.I1)]
		public bool IsVarArg;

		// Token: 0x040000FC RID: 252
		[MarshalAs(UnmanagedType.I1)]
		public bool IsTailCall;

		// Token: 0x040000FD RID: 253
		public ushort IndexFirstValue;

		// Token: 0x040000FE RID: 254
		public ushort NumberTransferredValues;

		// Token: 0x040000FF RID: 255
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		private byte[] shortSource;

		// Token: 0x04000100 RID: 256
		private IntPtr i_ci;
	}
}
