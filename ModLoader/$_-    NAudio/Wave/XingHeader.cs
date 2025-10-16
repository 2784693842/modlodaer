using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000A2 RID: 162
	internal class XingHeader
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x0001454E File Offset: 0x0001274E
		private static int ReadBigEndian(byte[] buffer, int offset)
		{
			return (((int)buffer[offset] << 8 | (int)buffer[offset + 1]) << 8 | (int)buffer[offset + 2]) << 8 | (int)buffer[offset + 3];
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001456C File Offset: 0x0001276C
		private void WriteBigEndian(byte[] buffer, int offset, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			for (int i = 0; i < 4; i++)
			{
				buffer[offset + 3 - i] = bytes[i];
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00014598 File Offset: 0x00012798
		public static XingHeader LoadXingHeader(Mp3Frame frame)
		{
			XingHeader xingHeader = new XingHeader();
			xingHeader.frame = frame;
			int num;
			if (frame.MpegVersion == MpegVersion.Version1)
			{
				if (frame.ChannelMode != ChannelMode.Mono)
				{
					num = 36;
				}
				else
				{
					num = 21;
				}
			}
			else
			{
				if (frame.MpegVersion != MpegVersion.Version2)
				{
					return null;
				}
				if (frame.ChannelMode != ChannelMode.Mono)
				{
					num = 21;
				}
				else
				{
					num = 13;
				}
			}
			if (frame.RawData[num] == 88 && frame.RawData[num + 1] == 105 && frame.RawData[num + 2] == 110 && frame.RawData[num + 3] == 103)
			{
				xingHeader.startOffset = num;
				num += 4;
			}
			else
			{
				if (frame.RawData[num] != 73 || frame.RawData[num + 1] != 110 || frame.RawData[num + 2] != 102 || frame.RawData[num + 3] != 111)
				{
					return null;
				}
				xingHeader.startOffset = num;
				num += 4;
			}
			int num2 = XingHeader.ReadBigEndian(frame.RawData, num);
			num += 4;
			if ((num2 & 1) != 0)
			{
				xingHeader.framesOffset = num;
				num += 4;
			}
			if ((num2 & 2) != 0)
			{
				xingHeader.bytesOffset = num;
				num += 4;
			}
			if ((num2 & 4) != 0)
			{
				xingHeader.tocOffset = num;
				num += 100;
			}
			if ((num2 & 8) != 0)
			{
				xingHeader.vbrScale = XingHeader.ReadBigEndian(frame.RawData, num);
				num += 4;
			}
			xingHeader.endOffset = num;
			return xingHeader;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000146D9 File Offset: 0x000128D9
		private XingHeader()
		{
			this.vbrScale = -1;
			this.tocOffset = -1;
			this.framesOffset = -1;
			this.bytesOffset = -1;
			base..ctor();
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003DD RID: 989 RVA: 0x000146FD File Offset: 0x000128FD
		// (set) Token: 0x060003DE RID: 990 RVA: 0x00014720 File Offset: 0x00012920
		public int Frames
		{
			get
			{
				if (this.framesOffset == -1)
				{
					return -1;
				}
				return XingHeader.ReadBigEndian(this.frame.RawData, this.framesOffset);
			}
			set
			{
				if (this.framesOffset == -1)
				{
					throw new InvalidOperationException("Frames flag is not set");
				}
				this.WriteBigEndian(this.frame.RawData, this.framesOffset, value);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0001474E File Offset: 0x0001294E
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x00014771 File Offset: 0x00012971
		public int Bytes
		{
			get
			{
				if (this.bytesOffset == -1)
				{
					return -1;
				}
				return XingHeader.ReadBigEndian(this.frame.RawData, this.bytesOffset);
			}
			set
			{
				if (this.framesOffset == -1)
				{
					throw new InvalidOperationException("Bytes flag is not set");
				}
				this.WriteBigEndian(this.frame.RawData, this.bytesOffset, value);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001479F File Offset: 0x0001299F
		public int VbrScale
		{
			get
			{
				return this.vbrScale;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x000147A7 File Offset: 0x000129A7
		public Mp3Frame Mp3Frame
		{
			get
			{
				return this.frame;
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000147AF File Offset: 0x000129AF
		// Note: this type is marked as 'beforefieldinit'.
		static XingHeader()
		{
			XingHeader.sr_table = new int[]
			{
				44100,
				48000,
				32000,
				99999
			};
		}

		// Token: 0x040003CD RID: 973
		private static int[] sr_table;

		// Token: 0x040003CE RID: 974
		private int vbrScale;

		// Token: 0x040003CF RID: 975
		private int startOffset;

		// Token: 0x040003D0 RID: 976
		private int endOffset;

		// Token: 0x040003D1 RID: 977
		private int tocOffset;

		// Token: 0x040003D2 RID: 978
		private int framesOffset;

		// Token: 0x040003D3 RID: 979
		private int bytesOffset;

		// Token: 0x040003D4 RID: 980
		private Mp3Frame frame;
	}
}
