using System;
using System.IO;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000A3 RID: 163
	internal class Mp3Frame
	{
		// Token: 0x060003E4 RID: 996 RVA: 0x000147C7 File Offset: 0x000129C7
		public static Mp3Frame LoadFromStream(Stream input)
		{
			return Mp3Frame.LoadFromStream(input, true);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000147D0 File Offset: 0x000129D0
		public static Mp3Frame LoadFromStream(Stream input, bool readData)
		{
			Mp3Frame mp3Frame = new Mp3Frame();
			mp3Frame.FileOffset = input.Position;
			byte[] array = new byte[4];
			if (input.Read(array, 0, array.Length) < array.Length)
			{
				return null;
			}
			while (!Mp3Frame.IsValidHeader(array, mp3Frame))
			{
				array[0] = array[1];
				array[1] = array[2];
				array[2] = array[3];
				if (input.Read(array, 3, 1) < 1)
				{
					return null;
				}
				Mp3Frame mp3Frame2 = mp3Frame;
				long fileOffset = mp3Frame2.FileOffset;
				mp3Frame2.FileOffset = fileOffset + 1L;
			}
			int num = mp3Frame.FrameLength - 4;
			if (readData)
			{
				mp3Frame.RawData = new byte[mp3Frame.FrameLength];
				Array.Copy(array, mp3Frame.RawData, 4);
				if (input.Read(mp3Frame.RawData, 4, num) < num)
				{
					throw new EndOfStreamException("Unexpected end of stream before frame complete");
				}
			}
			else
			{
				input.Position += (long)num;
			}
			return mp3Frame;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000B811 File Offset: 0x00009A11
		private Mp3Frame()
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00014898 File Offset: 0x00012A98
		private static bool IsValidHeader(byte[] headerBytes, Mp3Frame frame)
		{
			if (headerBytes[0] != 255 || (headerBytes[1] & 224) != 224)
			{
				return false;
			}
			frame.MpegVersion = (MpegVersion)((headerBytes[1] & 24) >> 3);
			if (frame.MpegVersion == MpegVersion.Reserved)
			{
				return false;
			}
			frame.MpegLayer = (MpegLayer)((headerBytes[1] & 6) >> 1);
			if (frame.MpegLayer == MpegLayer.Reserved)
			{
				return false;
			}
			int num = (frame.MpegLayer == MpegLayer.Layer1) ? 0 : ((frame.MpegLayer == MpegLayer.Layer2) ? 1 : 2);
			frame.CrcPresent = ((headerBytes[1] & 1) == 0);
			frame.BitRateIndex = (headerBytes[2] & 240) >> 4;
			if (frame.BitRateIndex == 15)
			{
				return false;
			}
			int num2 = (frame.MpegVersion == MpegVersion.Version1) ? 0 : 1;
			frame.BitRate = Mp3Frame.bitRates[num2, num, frame.BitRateIndex] * 1000;
			if (frame.BitRate == 0)
			{
				return false;
			}
			int num3 = (headerBytes[2] & 12) >> 2;
			if (num3 == 3)
			{
				return false;
			}
			if (frame.MpegVersion == MpegVersion.Version1)
			{
				frame.SampleRate = Mp3Frame.sampleRatesVersion1[num3];
			}
			else if (frame.MpegVersion == MpegVersion.Version2)
			{
				frame.SampleRate = Mp3Frame.sampleRatesVersion2[num3];
			}
			else
			{
				frame.SampleRate = Mp3Frame.sampleRatesVersion25[num3];
			}
			bool flag = (headerBytes[2] & 2) == 2;
			byte b = headerBytes[2];
			frame.ChannelMode = (ChannelMode)((headerBytes[3] & 192) >> 6);
			frame.ChannelExtension = (headerBytes[3] & 48) >> 4;
			if (frame.ChannelExtension != 0 && frame.ChannelMode != ChannelMode.JointStereo)
			{
				return false;
			}
			frame.Copyright = ((headerBytes[3] & 8) == 8);
			byte b2 = headerBytes[3];
			byte b3 = headerBytes[3];
			int num4 = flag ? 1 : 0;
			frame.SampleCount = Mp3Frame.samplesPerFrame[num2, num];
			int num5 = frame.SampleCount / 8;
			if (frame.MpegLayer == MpegLayer.Layer1)
			{
				frame.FrameLength = (num5 * frame.BitRate / frame.SampleRate + num4) * 4;
			}
			else
			{
				frame.FrameLength = num5 * frame.BitRate / frame.SampleRate + num4;
			}
			return frame.FrameLength <= 16384;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00014A87 File Offset: 0x00012C87
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x00014A8F File Offset: 0x00012C8F
		public int SampleRate { get; private set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00014A98 File Offset: 0x00012C98
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00014AA0 File Offset: 0x00012CA0
		public int FrameLength { get; private set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00014AA9 File Offset: 0x00012CA9
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x00014AB1 File Offset: 0x00012CB1
		public int BitRate { get; private set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x00014ABA File Offset: 0x00012CBA
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00014AC2 File Offset: 0x00012CC2
		public byte[] RawData { get; private set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00014ACB File Offset: 0x00012CCB
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x00014AD3 File Offset: 0x00012CD3
		public MpegVersion MpegVersion { get; private set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00014ADC File Offset: 0x00012CDC
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x00014AE4 File Offset: 0x00012CE4
		public MpegLayer MpegLayer { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00014AED File Offset: 0x00012CED
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00014AF5 File Offset: 0x00012CF5
		public ChannelMode ChannelMode { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00014AFE File Offset: 0x00012CFE
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00014B06 File Offset: 0x00012D06
		public int SampleCount { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00014B0F File Offset: 0x00012D0F
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00014B17 File Offset: 0x00012D17
		public int ChannelExtension { get; private set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00014B20 File Offset: 0x00012D20
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x00014B28 File Offset: 0x00012D28
		public int BitRateIndex { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00014B31 File Offset: 0x00012D31
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x00014B39 File Offset: 0x00012D39
		public bool Copyright { get; private set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x00014B42 File Offset: 0x00012D42
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x00014B4A File Offset: 0x00012D4A
		public bool CrcPresent { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00014B53 File Offset: 0x00012D53
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x00014B5B File Offset: 0x00012D5B
		public long FileOffset { get; private set; }

		// Token: 0x06000402 RID: 1026 RVA: 0x00014B64 File Offset: 0x00012D64
		// Note: this type is marked as 'beforefieldinit'.
		static Mp3Frame()
		{
			Mp3Frame.bitRates = new int[,,]
			{
				{
					{
						0,
						32,
						64,
						96,
						128,
						160,
						192,
						224,
						256,
						288,
						320,
						352,
						384,
						416,
						448
					},
					{
						0,
						32,
						48,
						56,
						64,
						80,
						96,
						112,
						128,
						160,
						192,
						224,
						256,
						320,
						384
					},
					{
						0,
						32,
						40,
						48,
						56,
						64,
						80,
						96,
						112,
						128,
						160,
						192,
						224,
						256,
						320
					}
				},
				{
					{
						0,
						32,
						48,
						56,
						64,
						80,
						96,
						112,
						128,
						144,
						160,
						176,
						192,
						224,
						256
					},
					{
						0,
						8,
						16,
						24,
						32,
						40,
						48,
						56,
						64,
						80,
						96,
						112,
						128,
						144,
						160
					},
					{
						0,
						8,
						16,
						24,
						32,
						40,
						48,
						56,
						64,
						80,
						96,
						112,
						128,
						144,
						160
					}
				}
			};
			Mp3Frame.samplesPerFrame = new int[,]
			{
				{
					384,
					1152,
					1152
				},
				{
					384,
					1152,
					576
				}
			};
			Mp3Frame.sampleRatesVersion1 = new int[]
			{
				44100,
				48000,
				32000
			};
			Mp3Frame.sampleRatesVersion2 = new int[]
			{
				22050,
				24000,
				16000
			};
			Mp3Frame.sampleRatesVersion25 = new int[]
			{
				11025,
				12000,
				8000
			};
		}

		// Token: 0x040003D5 RID: 981
		private static readonly int[,,] bitRates;

		// Token: 0x040003D6 RID: 982
		private static readonly int[,] samplesPerFrame;

		// Token: 0x040003D7 RID: 983
		private static readonly int[] sampleRatesVersion1;

		// Token: 0x040003D8 RID: 984
		private static readonly int[] sampleRatesVersion2;

		// Token: 0x040003D9 RID: 985
		private static readonly int[] sampleRatesVersion25;

		// Token: 0x040003DA RID: 986
		private const int MaxFrameLength = 16384;
	}
}
