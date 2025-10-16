using System;
using System.IO;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020JeremyAnsel.Media.Dds
{
	// Token: 0x0200002E RID: 46
	internal sealed class DdsFile
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00009F20 File Offset: 0x00008120
		internal DdsFile()
		{
			this.PixelFormat = new DdsPixelFormat();
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00009F33 File Offset: 0x00008133
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00009F3B File Offset: 0x0000813B
		public DdsOptions Options { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00009F44 File Offset: 0x00008144
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00009F4C File Offset: 0x0000814C
		public int Height { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00009F55 File Offset: 0x00008155
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00009F5D File Offset: 0x0000815D
		public int Width { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00009F66 File Offset: 0x00008166
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00009F6E File Offset: 0x0000816E
		public int LinearSize { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00009F77 File Offset: 0x00008177
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00009F7F File Offset: 0x0000817F
		public int Depth { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00009F88 File Offset: 0x00008188
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00009F90 File Offset: 0x00008190
		public int MipmapCount { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00009F99 File Offset: 0x00008199
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00009FA1 File Offset: 0x000081A1
		public DdsPixelFormat PixelFormat { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00009FAA File Offset: 0x000081AA
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00009FB2 File Offset: 0x000081B2
		public DdsCaps Caps { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00009FBB File Offset: 0x000081BB
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00009FC3 File Offset: 0x000081C3
		public DdsAdditionalCaps Caps2 { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00009FCC File Offset: 0x000081CC
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00009FD4 File Offset: 0x000081D4
		public DdsFormat Format { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00009FDD File Offset: 0x000081DD
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00009FE5 File Offset: 0x000081E5
		public DdsResourceDimension ResourceDimension { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00009FEE File Offset: 0x000081EE
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00009FF6 File Offset: 0x000081F6
		public DdsResourceMiscOptions ResourceMiscOptions { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00009FFF File Offset: 0x000081FF
		// (set) Token: 0x060000CD RID: 205 RVA: 0x0000A007 File Offset: 0x00008207
		public int ArraySize { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000A010 File Offset: 0x00008210
		// (set) Token: 0x060000CF RID: 207 RVA: 0x0000A018 File Offset: 0x00008218
		public DdsAlphaMode AlphaMode { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000A021 File Offset: 0x00008221
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x0000A029 File Offset: 0x00008229
		public int DataOffset { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000A032 File Offset: 0x00008232
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x0000A03A File Offset: 0x0000823A
		public byte[] Data { get; private set; }

		// Token: 0x060000D4 RID: 212 RVA: 0x0000A044 File Offset: 0x00008244
		public static DdsFile FromFile(string fileName)
		{
			DdsFile result;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				result = DdsFile.FromStream(fileStream);
			}
			return result;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000A080 File Offset: 0x00008280
		public static DdsFile FromStream(Stream stream)
		{
			DdsFile ddsFile = new DdsFile();
			BinaryReader binaryReader = new BinaryReader(stream);
			if (binaryReader.ReadInt32() != 542327876)
			{
				throw new InvalidDataException();
			}
			if (binaryReader.ReadInt32() != 124)
			{
				throw new InvalidDataException();
			}
			ddsFile.Options = (DdsOptions)binaryReader.ReadInt32();
			ddsFile.Height = binaryReader.ReadInt32();
			ddsFile.Width = binaryReader.ReadInt32();
			ddsFile.LinearSize = binaryReader.ReadInt32();
			ddsFile.Depth = binaryReader.ReadInt32();
			ddsFile.MipmapCount = binaryReader.ReadInt32();
			binaryReader.BaseStream.Position += 44L;
			if (binaryReader.ReadInt32() != 32)
			{
				throw new InvalidDataException();
			}
			ddsFile.PixelFormat.Options = (DdsPixelFormatOptions)binaryReader.ReadInt32();
			ddsFile.PixelFormat.FourCC = (DdsFourCC)binaryReader.ReadInt32();
			ddsFile.PixelFormat.RgbBitCount = binaryReader.ReadInt32();
			ddsFile.PixelFormat.RedBitMask = binaryReader.ReadUInt32();
			ddsFile.PixelFormat.GreenBitMask = binaryReader.ReadUInt32();
			ddsFile.PixelFormat.BlueBitMask = binaryReader.ReadUInt32();
			ddsFile.PixelFormat.AlphaBitMask = binaryReader.ReadUInt32();
			ddsFile.Caps = (DdsCaps)binaryReader.ReadInt32();
			ddsFile.Caps2 = (DdsAdditionalCaps)binaryReader.ReadInt32();
			binaryReader.BaseStream.Position += 12L;
			if ((ddsFile.PixelFormat.Options & DdsPixelFormatOptions.FourCC) != (DdsPixelFormatOptions)0 && ddsFile.PixelFormat.FourCC == DdsFourCC.DX10)
			{
				ddsFile.Format = (DdsFormat)binaryReader.ReadInt32();
				ddsFile.ResourceDimension = (DdsResourceDimension)binaryReader.ReadInt32();
				ddsFile.ResourceMiscOptions = (DdsResourceMiscOptions)binaryReader.ReadInt32();
				ddsFile.ArraySize = binaryReader.ReadInt32();
				DdsAlphaMode ddsAlphaMode = (DdsAlphaMode)(binaryReader.ReadInt32() & 7);
				if (ddsAlphaMode - DdsAlphaMode.Straight <= 3)
				{
					ddsFile.AlphaMode = ddsAlphaMode;
				}
			}
			else
			{
				ddsFile.Format = DdsHelpers.GetDdsFormat(ddsFile.PixelFormat);
				if ((ddsFile.PixelFormat.Options & DdsPixelFormatOptions.FourCC) != (DdsPixelFormatOptions)0)
				{
					DdsFourCC fourCC = ddsFile.PixelFormat.FourCC;
					if (fourCC == DdsFourCC.DXT2 || fourCC == DdsFourCC.DXT4)
					{
						ddsFile.AlphaMode = DdsAlphaMode.Premultiplied;
					}
				}
			}
			ddsFile.DataOffset = (int)binaryReader.BaseStream.Position;
			ddsFile.Data = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length - (int)binaryReader.BaseStream.Position);
			return ddsFile;
		}

		// Token: 0x040000B4 RID: 180
		private const int DdsMagic = 542327876;
	}
}
