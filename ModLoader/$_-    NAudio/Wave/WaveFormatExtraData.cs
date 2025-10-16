using System;
using System.IO;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x02000042 RID: 66
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal class WaveFormatExtraData : WaveFormat
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000B819 File Offset: 0x00009A19
		public byte[] ExtraData
		{
			get
			{
				return this.extraData;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000B821 File Offset: 0x00009A21
		internal WaveFormatExtraData()
		{
			this.extraData = new byte[100];
			base..ctor();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000B836 File Offset: 0x00009A36
		public WaveFormatExtraData(BinaryReader reader)
		{
			this.extraData = new byte[100];
			base..ctor(reader);
			this.ReadExtraData(reader);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000B853 File Offset: 0x00009A53
		internal void ReadExtraData(BinaryReader reader)
		{
			if (this.extraSize > 0)
			{
				reader.Read(this.extraData, 0, (int)this.extraSize);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000B872 File Offset: 0x00009A72
		public override void Serialize(BinaryWriter writer)
		{
			base.Serialize(writer);
			if (this.extraSize > 0)
			{
				writer.Write(this.extraData, 0, (int)this.extraSize);
			}
		}

		// Token: 0x0400023D RID: 573
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
		private byte[] extraData;
	}
}
