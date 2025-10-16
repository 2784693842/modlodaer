using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020JeremyAnsel.Media.Dds
{
	// Token: 0x02000039 RID: 57
	internal static class DdsHelpers
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000A32C File Offset: 0x0000852C
		public static int GetBitsPerPixel(DdsFormat format)
		{
			switch (format)
			{
			case DdsFormat.R32G32B32A32Typeless:
			case DdsFormat.R32G32B32A32Float:
			case DdsFormat.R32G32B32A32UInt:
			case DdsFormat.R32G32B32A32SInt:
				return 128;
			case DdsFormat.R32G32B32Typeless:
			case DdsFormat.R32G32B32Float:
			case DdsFormat.R32G32B32UInt:
			case DdsFormat.R32G32B32SInt:
				return 96;
			case DdsFormat.R16G16B16A16Typeless:
			case DdsFormat.R16G16B16A16Float:
			case DdsFormat.R16G16B16A16UNorm:
			case DdsFormat.R16G16B16A16UInt:
			case DdsFormat.R16G16B16A16SNorm:
			case DdsFormat.R16G16B16A16SInt:
			case DdsFormat.R32G32Typeless:
			case DdsFormat.R32G32Float:
			case DdsFormat.R32G32UInt:
			case DdsFormat.R32G32SInt:
			case DdsFormat.R32G8X24Typeless:
			case DdsFormat.D32FloatS8X24UInt:
			case DdsFormat.R32FloatX8X24Typeless:
			case DdsFormat.X32TypelessG8X24UInt:
			case DdsFormat.Y416:
			case DdsFormat.Y210:
			case DdsFormat.Y216:
				return 64;
			case DdsFormat.R10G10B10A2Typeless:
			case DdsFormat.R10G10B10A2UNorm:
			case DdsFormat.R10G10B10A2UInt:
			case DdsFormat.R11G11B10Float:
			case DdsFormat.R8G8B8A8Typeless:
			case DdsFormat.R8G8B8A8UNorm:
			case DdsFormat.R8G8B8A8UNormSrgb:
			case DdsFormat.R8G8B8A8UInt:
			case DdsFormat.R8G8B8A8SNorm:
			case DdsFormat.R8G8B8A8SInt:
			case DdsFormat.R16G16Typeless:
			case DdsFormat.R16G16Float:
			case DdsFormat.R16G16UNorm:
			case DdsFormat.R16G16UInt:
			case DdsFormat.R16G16SNorm:
			case DdsFormat.R16G16SInt:
			case DdsFormat.R32Typeless:
			case DdsFormat.D32Float:
			case DdsFormat.R32Float:
			case DdsFormat.R32UInt:
			case DdsFormat.R32SInt:
			case DdsFormat.R24G8Typeless:
			case DdsFormat.D24UNormS8UInt:
			case DdsFormat.R24UNormX8Typeless:
			case DdsFormat.X24TypelessG8UInt:
			case DdsFormat.R9G9B9E5SharedExp:
			case DdsFormat.R8G8B8G8UNorm:
			case DdsFormat.G8R8G8B8UNorm:
			case DdsFormat.B8G8R8A8UNorm:
			case DdsFormat.B8G8R8X8UNorm:
			case DdsFormat.R10G10B10XRBiasA2UNorm:
			case DdsFormat.B8G8R8A8Typeless:
			case DdsFormat.B8G8R8A8UNormSrgb:
			case DdsFormat.B8G8R8X8Typeless:
			case DdsFormat.B8G8R8X8UNormSrgb:
			case DdsFormat.AYuv:
			case DdsFormat.Y410:
			case DdsFormat.Yuy2:
				return 32;
			case DdsFormat.R8G8Typeless:
			case DdsFormat.R8G8UNorm:
			case DdsFormat.R8G8UInt:
			case DdsFormat.R8G8SNorm:
			case DdsFormat.R8G8SInt:
			case DdsFormat.R16Typeless:
			case DdsFormat.R16Float:
			case DdsFormat.D16UNorm:
			case DdsFormat.R16UNorm:
			case DdsFormat.R16UInt:
			case DdsFormat.R16SNorm:
			case DdsFormat.R16SInt:
			case DdsFormat.B5G6R5UNorm:
			case DdsFormat.B5G5R5A1UNorm:
			case DdsFormat.A8P8:
			case DdsFormat.B4G4R4A4UNorm:
				return 16;
			case DdsFormat.R8Typeless:
			case DdsFormat.R8UNorm:
			case DdsFormat.R8UInt:
			case DdsFormat.R8SNorm:
			case DdsFormat.R8SInt:
			case DdsFormat.A8UNorm:
			case DdsFormat.AI44:
			case DdsFormat.IA44:
			case DdsFormat.P8:
				return 8;
			case DdsFormat.R1UNorm:
				return 1;
			case DdsFormat.BC1Typeless:
			case DdsFormat.BC1UNorm:
			case DdsFormat.BC1UNormSrgb:
			case DdsFormat.BC4Typeless:
			case DdsFormat.BC4UNorm:
			case DdsFormat.BC4SNorm:
				return 4;
			case DdsFormat.BC2Typeless:
			case DdsFormat.BC2UNorm:
			case DdsFormat.BC2UNormSrgb:
			case DdsFormat.BC3Typeless:
			case DdsFormat.BC3UNorm:
			case DdsFormat.BC3UNormSrgb:
			case DdsFormat.BC5Typeless:
			case DdsFormat.BC5UNorm:
			case DdsFormat.BC5SNorm:
			case DdsFormat.BC6HalfTypeless:
			case DdsFormat.BC6HalfUF16:
			case DdsFormat.BC6HalfSF16:
			case DdsFormat.BC7Typeless:
			case DdsFormat.BC7UNorm:
			case DdsFormat.BC7UNormSrgb:
				return 8;
			case DdsFormat.NV12:
			case DdsFormat.P420Opaque:
			case DdsFormat.NV11:
				return 12;
			case DdsFormat.P010:
			case DdsFormat.P016:
				return 24;
			default:
				return 0;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000A530 File Offset: 0x00008730
		public static void GetSurfaceInfo(int width, int height, DdsFormat format, out int outNumBytes, out int outRowBytes, out int outNumRows)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			int num = 0;
			switch (format)
			{
			case DdsFormat.R8G8B8G8UNorm:
			case DdsFormat.G8R8G8B8UNorm:
			case DdsFormat.Yuy2:
				flag2 = true;
				num = 4;
				break;
			case DdsFormat.BC1Typeless:
			case DdsFormat.BC1UNorm:
			case DdsFormat.BC1UNormSrgb:
			case DdsFormat.BC4Typeless:
			case DdsFormat.BC4UNorm:
			case DdsFormat.BC4SNorm:
				flag = true;
				num = 8;
				break;
			case DdsFormat.BC2Typeless:
			case DdsFormat.BC2UNorm:
			case DdsFormat.BC2UNormSrgb:
			case DdsFormat.BC3Typeless:
			case DdsFormat.BC3UNorm:
			case DdsFormat.BC3UNormSrgb:
			case DdsFormat.BC5Typeless:
			case DdsFormat.BC5UNorm:
			case DdsFormat.BC5SNorm:
			case DdsFormat.BC6HalfTypeless:
			case DdsFormat.BC6HalfUF16:
			case DdsFormat.BC6HalfSF16:
			case DdsFormat.BC7Typeless:
			case DdsFormat.BC7UNorm:
			case DdsFormat.BC7UNormSrgb:
				flag = true;
				num = 16;
				break;
			case DdsFormat.NV12:
			case DdsFormat.P420Opaque:
				flag3 = true;
				num = 2;
				break;
			case DdsFormat.P010:
			case DdsFormat.P016:
				flag3 = true;
				num = 4;
				break;
			case DdsFormat.Y210:
			case DdsFormat.Y216:
				flag2 = true;
				num = 8;
				break;
			}
			int num4;
			int num5;
			int num6;
			if (flag)
			{
				int num2 = 0;
				if (width > 0)
				{
					num2 = Math.Max(1, (width + 3) / 4);
				}
				int num3 = 0;
				if (height > 0)
				{
					num3 = Math.Max(1, (height + 3) / 4);
				}
				num4 = num2 * num;
				num5 = num3;
				num6 = num4 * num3;
			}
			else if (flag2)
			{
				num4 = (width + 1 >> 1) * num;
				num5 = height;
				num6 = num4 * height;
			}
			else if (format == DdsFormat.NV11)
			{
				num4 = (width + 3 >> 2) * 4;
				num5 = height * 2;
				num6 = num4 * num5;
			}
			else if (flag3)
			{
				num4 = (width + 1 >> 1) * num;
				num6 = num4 * height + (num4 * height + 1 >> 1);
				num5 = height + (height + 1 >> 1);
			}
			else
			{
				int bitsPerPixel = DdsHelpers.GetBitsPerPixel(format);
				num4 = (width * bitsPerPixel + 7) / 8;
				num5 = height;
				num6 = num4 * height;
			}
			outNumBytes = num6;
			outRowBytes = num4;
			outNumRows = num5;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000A6DC File Offset: 0x000088DC
		public static DdsFormat MakeSrgb(DdsFormat format)
		{
			if (format <= DdsFormat.BC2UNorm)
			{
				if (format == DdsFormat.R8G8B8A8UNorm)
				{
					return DdsFormat.R8G8B8A8UNormSrgb;
				}
				if (format == DdsFormat.BC1UNorm)
				{
					return DdsFormat.BC1UNormSrgb;
				}
				if (format == DdsFormat.BC2UNorm)
				{
					return DdsFormat.BC2UNormSrgb;
				}
			}
			else if (format <= DdsFormat.B8G8R8A8UNorm)
			{
				if (format == DdsFormat.BC3UNorm)
				{
					return DdsFormat.BC3UNormSrgb;
				}
				if (format == DdsFormat.B8G8R8A8UNorm)
				{
					return DdsFormat.B8G8R8A8UNormSrgb;
				}
			}
			else
			{
				if (format == DdsFormat.B8G8R8X8UNorm)
				{
					return DdsFormat.B8G8R8X8UNormSrgb;
				}
				if (format == DdsFormat.BC7UNorm)
				{
					return DdsFormat.BC7UNormSrgb;
				}
			}
			return format;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000A732 File Offset: 0x00008932
		private static bool IsBitMask(DdsPixelFormat ddpf, uint r, uint g, uint b, uint a)
		{
			return ddpf.RedBitMask == r && ddpf.GreenBitMask == g && ddpf.BlueBitMask == b && ddpf.AlphaBitMask == a;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000A75C File Offset: 0x0000895C
		internal static DdsFormat GetDdsFormat(DdsPixelFormat ddpf)
		{
			if ((ddpf.Options & DdsPixelFormatOptions.Rgb) != (DdsPixelFormatOptions)0)
			{
				int rgbBitCount = ddpf.RgbBitCount;
				if (rgbBitCount != 16)
				{
					if (rgbBitCount != 24 && rgbBitCount == 32)
					{
						if (DdsHelpers.IsBitMask(ddpf, 255U, 65280U, 16711680U, 4278190080U))
						{
							return DdsFormat.R8G8B8A8UNorm;
						}
						if (DdsHelpers.IsBitMask(ddpf, 16711680U, 65280U, 255U, 4278190080U))
						{
							return DdsFormat.B8G8R8A8UNorm;
						}
						if (DdsHelpers.IsBitMask(ddpf, 16711680U, 65280U, 255U, 0U))
						{
							return DdsFormat.B8G8R8X8UNorm;
						}
						if (DdsHelpers.IsBitMask(ddpf, 1072693248U, 1047552U, 1023U, 3221225472U))
						{
							return DdsFormat.R10G10B10A2UNorm;
						}
						if (DdsHelpers.IsBitMask(ddpf, 65535U, 4294901760U, 0U, 0U))
						{
							return DdsFormat.R16G16UNorm;
						}
						if (DdsHelpers.IsBitMask(ddpf, 4294967295U, 0U, 0U, 0U))
						{
							return DdsFormat.R32Float;
						}
					}
				}
				else
				{
					if (DdsHelpers.IsBitMask(ddpf, 31744U, 992U, 31U, 32768U))
					{
						return DdsFormat.B5G5R5A1UNorm;
					}
					if (DdsHelpers.IsBitMask(ddpf, 63488U, 2016U, 31U, 0U))
					{
						return DdsFormat.B5G6R5UNorm;
					}
					if (DdsHelpers.IsBitMask(ddpf, 3840U, 240U, 15U, 61440U))
					{
						return DdsFormat.B4G4R4A4UNorm;
					}
				}
			}
			else if ((ddpf.Options & DdsPixelFormatOptions.Luminance) != (DdsPixelFormatOptions)0)
			{
				int rgbBitCount = ddpf.RgbBitCount;
				if (rgbBitCount != 8)
				{
					if (rgbBitCount == 16)
					{
						if (DdsHelpers.IsBitMask(ddpf, 65535U, 0U, 0U, 0U))
						{
							return DdsFormat.R16UNorm;
						}
						if (DdsHelpers.IsBitMask(ddpf, 255U, 0U, 0U, 65280U))
						{
							return DdsFormat.R8G8UNorm;
						}
					}
				}
				else
				{
					if (DdsHelpers.IsBitMask(ddpf, 255U, 0U, 0U, 0U))
					{
						return DdsFormat.R8UNorm;
					}
					if (DdsHelpers.IsBitMask(ddpf, 255U, 0U, 0U, 65280U))
					{
						return DdsFormat.R8G8UNorm;
					}
				}
			}
			else if ((ddpf.Options & DdsPixelFormatOptions.Alpha) != (DdsPixelFormatOptions)0)
			{
				if (ddpf.RgbBitCount == 8)
				{
					return DdsFormat.A8UNorm;
				}
			}
			else if ((ddpf.Options & DdsPixelFormatOptions.BumpDuDv) != (DdsPixelFormatOptions)0)
			{
				int rgbBitCount = ddpf.RgbBitCount;
				if (rgbBitCount != 16)
				{
					if (rgbBitCount == 32)
					{
						if (DdsHelpers.IsBitMask(ddpf, 255U, 65280U, 16711680U, 4278190080U))
						{
							return DdsFormat.R8G8B8A8SNorm;
						}
						if (DdsHelpers.IsBitMask(ddpf, 65535U, 4294901760U, 0U, 0U))
						{
							return DdsFormat.R16G16SNorm;
						}
					}
				}
				else if (DdsHelpers.IsBitMask(ddpf, 255U, 65280U, 0U, 0U))
				{
					return DdsFormat.R8G8SNorm;
				}
			}
			else if ((ddpf.Options & DdsPixelFormatOptions.FourCC) != (DdsPixelFormatOptions)0)
			{
				DdsFourCC fourCC = ddpf.FourCC;
				if (fourCC <= DdsFourCC.DXT4)
				{
					if (fourCC <= DdsFourCC.DXT1)
					{
						if (fourCC == DdsFourCC.D3DFMT_A16B16G16R16)
						{
							return DdsFormat.R16G16B16A16UNorm;
						}
						switch (fourCC)
						{
						case DdsFourCC.D3DFMT_Q16W16V16U16:
							return DdsFormat.R16G16B16A16SNorm;
						case DdsFourCC.D3DFMT_R16F:
							return DdsFormat.R16Float;
						case DdsFourCC.D3DFMT_G16R16F:
							return DdsFormat.R16G16Float;
						case DdsFourCC.D3DFMT_A16B16G16R16F:
							return DdsFormat.R16G16B16A16Float;
						case DdsFourCC.D3DFMT_R32F:
							return DdsFormat.R32Float;
						case DdsFourCC.D3DFMT_G32R32F:
							return DdsFormat.R32G32Float;
						case DdsFourCC.D3DFMT_A32B32G32R32F:
							return DdsFormat.R32G32B32A32Float;
						default:
							if (fourCC == DdsFourCC.DXT1)
							{
								return DdsFormat.BC1UNorm;
							}
							break;
						}
					}
					else if (fourCC <= DdsFourCC.YUY2)
					{
						if (fourCC == DdsFourCC.DXT2)
						{
							return DdsFormat.BC2UNorm;
						}
						if (fourCC == DdsFourCC.YUY2)
						{
							return DdsFormat.Yuy2;
						}
					}
					else
					{
						if (fourCC == DdsFourCC.DXT3)
						{
							return DdsFormat.BC2UNorm;
						}
						if (fourCC == DdsFourCC.DXT4)
						{
							return DdsFormat.BC3UNorm;
						}
					}
				}
				else if (fourCC <= DdsFourCC.RGBG)
				{
					if (fourCC == DdsFourCC.DXT5)
					{
						return DdsFormat.BC3UNorm;
					}
					if (fourCC == DdsFourCC.GRGB)
					{
						return DdsFormat.G8R8G8B8UNorm;
					}
					if (fourCC == DdsFourCC.RGBG)
					{
						return DdsFormat.R8G8B8G8UNorm;
					}
				}
				else if (fourCC <= DdsFourCC.BC5S)
				{
					if (fourCC == DdsFourCC.BC4S)
					{
						return DdsFormat.BC4SNorm;
					}
					if (fourCC == DdsFourCC.BC5S)
					{
						return DdsFormat.BC5SNorm;
					}
				}
				else
				{
					if (fourCC == DdsFourCC.BC4U)
					{
						return DdsFormat.BC4UNorm;
					}
					if (fourCC == DdsFourCC.BC5U)
					{
						return DdsFormat.BC5UNorm;
					}
				}
			}
			return DdsFormat.Unknown;
		}
	}
}
