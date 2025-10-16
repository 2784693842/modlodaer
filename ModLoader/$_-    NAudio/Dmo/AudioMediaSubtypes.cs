using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Dmo
{
	// Token: 0x02000047 RID: 71
	internal class AudioMediaSubtypes
	{
		// Token: 0x0600014B RID: 331 RVA: 0x0000BC78 File Offset: 0x00009E78
		public static string GetAudioSubtypeName(Guid subType)
		{
			for (int i = 0; i < AudioMediaSubtypes.AudioSubTypes.Length; i++)
			{
				if (subType == AudioMediaSubtypes.AudioSubTypes[i])
				{
					return AudioMediaSubtypes.AudioSubTypeNames[i];
				}
			}
			return subType.ToString();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000BCC0 File Offset: 0x00009EC0
		// Note: this type is marked as 'beforefieldinit'.
		static AudioMediaSubtypes()
		{
			AudioMediaSubtypes.MEDIASUBTYPE_PCM = new Guid("00000001-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.MEDIASUBTYPE_PCMAudioObsolete = new Guid("e436eb8a-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_MPEG1Packet = new Guid("e436eb80-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_MPEG1Payload = new Guid("e436eb81-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_MPEG2_AUDIO = new Guid("e06d802b-db46-11cf-b4d1-00805f6cbbea");
			AudioMediaSubtypes.MEDIASUBTYPE_DVD_LPCM_AUDIO = new Guid("e06d8032-db46-11cf-b4d1-00805f6cbbea");
			AudioMediaSubtypes.MEDIASUBTYPE_DRM_Audio = new Guid("00000009-0000-0010-8000-00aa00389b71");
			AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT = new Guid("00000003-0000-0010-8000-00aa00389b71");
			AudioMediaSubtypes.MEDIASUBTYPE_DOLBY_AC3 = new Guid("e06d802c-db46-11cf-b4d1-00805f6cbbea");
			AudioMediaSubtypes.MEDIASUBTYPE_DOLBY_AC3_SPDIF = new Guid("00000092-0000-0010-8000-00aa00389b71");
			AudioMediaSubtypes.MEDIASUBTYPE_RAW_SPORT = new Guid("00000240-0000-0010-8000-00aa00389b71");
			AudioMediaSubtypes.MEDIASUBTYPE_SPDIF_TAG_241h = new Guid("00000241-0000-0010-8000-00aa00389b71");
			AudioMediaSubtypes.MEDIASUBTYPE_I420 = new Guid("30323449-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.MEDIASUBTYPE_IYUV = new Guid("56555949-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.MEDIASUBTYPE_RGB1 = new Guid("e436eb78-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_RGB24 = new Guid("e436eb7d-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_RGB32 = new Guid("e436eb7e-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_RGB4 = new Guid("e436eb79-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_RGB555 = new Guid("e436eb7c-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_RGB565 = new Guid("e436eb7b-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_RGB8 = new Guid("e436eb7a-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_UYVY = new Guid("59565955-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.MEDIASUBTYPE_VIDEOIMAGE = new Guid("1d4a45f2-e5f6-4b44-8388-f0ae5c0e0c37");
			AudioMediaSubtypes.MEDIASUBTYPE_YUY2 = new Guid("32595559-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.MEDIASUBTYPE_YV12 = new Guid("31313259-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.MEDIASUBTYPE_YVU9 = new Guid("39555659-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.MEDIASUBTYPE_YVYU = new Guid("55595659-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMFORMAT_MPEG2Video = new Guid("e06d80e3-db46-11cf-b4d1-00805f6cbbea");
			AudioMediaSubtypes.WMFORMAT_Script = new Guid("5C8510F2-DEBE-4ca7-BBA5-F07A104F8DFF");
			AudioMediaSubtypes.WMFORMAT_VideoInfo = new Guid("05589f80-c356-11ce-bf01-00aa0055595a");
			AudioMediaSubtypes.WMFORMAT_WaveFormatEx = new Guid("05589f81-c356-11ce-bf01-00aa0055595a");
			AudioMediaSubtypes.WMFORMAT_WebStream = new Guid("da1e6b13-8359-4050-b398-388e965bf00c");
			AudioMediaSubtypes.WMMEDIASUBTYPE_ACELPnet = new Guid("00000130-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_Base = new Guid("00000000-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_DRM = new Guid("00000009-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_MP3 = new Guid("00000055-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_MP43 = new Guid("3334504D-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_MP4S = new Guid("5334504D-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_M4S2 = new Guid("3253344D-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_P422 = new Guid("32323450-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_MPEG2_VIDEO = new Guid("e06d8026-db46-11cf-b4d1-00805f6cbbea");
			AudioMediaSubtypes.WMMEDIASUBTYPE_MSS1 = new Guid("3153534D-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_MSS2 = new Guid("3253534D-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_PCM = new Guid("00000001-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WebStream = new Guid("776257d4-c627-41cb-8f81-7ac7ff1c40cc");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMAudio_Lossless = new Guid("00000163-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMAudioV2 = new Guid("00000161-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMAudioV7 = new Guid("00000161-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMAudioV8 = new Guid("00000161-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMAudioV9 = new Guid("00000162-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMSP1 = new Guid("0000000A-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMV1 = new Guid("31564D57-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMV2 = new Guid("32564D57-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMV3 = new Guid("33564D57-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMVA = new Guid("41564D57-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WMVP = new Guid("50564D57-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIASUBTYPE_WVP2 = new Guid("32505657-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIATYPE_Audio = new Guid("73647561-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIATYPE_FileTransfer = new Guid("D9E47579-930E-4427-ADFC-AD80F290E470");
			AudioMediaSubtypes.WMMEDIATYPE_Image = new Guid("34A50FD8-8AA5-4386-81FE-A0EFE0488E31");
			AudioMediaSubtypes.WMMEDIATYPE_Script = new Guid("73636d64-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMMEDIATYPE_Text = new Guid("9BBA1EA7-5AB2-4829-BA57-0940209BCF3E");
			AudioMediaSubtypes.WMMEDIATYPE_Video = new Guid("73646976-0000-0010-8000-00AA00389B71");
			AudioMediaSubtypes.WMSCRIPTTYPE_TwoStrings = new Guid("82f38a70-c29f-11d1-97ad-00a0c95ea850");
			AudioMediaSubtypes.MEDIASUBTYPE_WAVE = new Guid("e436eb8b-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_AU = new Guid("e436eb8c-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.MEDIASUBTYPE_AIFF = new Guid("e436eb8d-524f-11ce-9f53-0020af0ba770");
			AudioMediaSubtypes.AudioSubTypes = new Guid[]
			{
				AudioMediaSubtypes.MEDIASUBTYPE_PCM,
				AudioMediaSubtypes.MEDIASUBTYPE_PCMAudioObsolete,
				AudioMediaSubtypes.MEDIASUBTYPE_MPEG1Packet,
				AudioMediaSubtypes.MEDIASUBTYPE_MPEG1Payload,
				AudioMediaSubtypes.MEDIASUBTYPE_MPEG2_AUDIO,
				AudioMediaSubtypes.MEDIASUBTYPE_DVD_LPCM_AUDIO,
				AudioMediaSubtypes.MEDIASUBTYPE_DRM_Audio,
				AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT,
				AudioMediaSubtypes.MEDIASUBTYPE_DOLBY_AC3,
				AudioMediaSubtypes.MEDIASUBTYPE_DOLBY_AC3_SPDIF,
				AudioMediaSubtypes.MEDIASUBTYPE_RAW_SPORT,
				AudioMediaSubtypes.MEDIASUBTYPE_SPDIF_TAG_241h,
				AudioMediaSubtypes.WMMEDIASUBTYPE_MP3
			};
			AudioMediaSubtypes.AudioSubTypeNames = new string[]
			{
				"PCM",
				"PCM Obsolete",
				"MPEG1Packet",
				"MPEG1Payload",
				"MPEG2_AUDIO",
				"DVD_LPCM_AUDIO",
				"DRM_Audio",
				"IEEE_FLOAT",
				"DOLBY_AC3",
				"DOLBY_AC3_SPDIF",
				"RAW_SPORT",
				"SPDIF_TAG_241h",
				"MP3"
			};
		}

		// Token: 0x04000245 RID: 581
		public static readonly Guid MEDIASUBTYPE_PCM;

		// Token: 0x04000246 RID: 582
		public static readonly Guid MEDIASUBTYPE_PCMAudioObsolete;

		// Token: 0x04000247 RID: 583
		public static readonly Guid MEDIASUBTYPE_MPEG1Packet;

		// Token: 0x04000248 RID: 584
		public static readonly Guid MEDIASUBTYPE_MPEG1Payload;

		// Token: 0x04000249 RID: 585
		public static readonly Guid MEDIASUBTYPE_MPEG2_AUDIO;

		// Token: 0x0400024A RID: 586
		public static readonly Guid MEDIASUBTYPE_DVD_LPCM_AUDIO;

		// Token: 0x0400024B RID: 587
		public static readonly Guid MEDIASUBTYPE_DRM_Audio;

		// Token: 0x0400024C RID: 588
		public static readonly Guid MEDIASUBTYPE_IEEE_FLOAT;

		// Token: 0x0400024D RID: 589
		public static readonly Guid MEDIASUBTYPE_DOLBY_AC3;

		// Token: 0x0400024E RID: 590
		public static readonly Guid MEDIASUBTYPE_DOLBY_AC3_SPDIF;

		// Token: 0x0400024F RID: 591
		public static readonly Guid MEDIASUBTYPE_RAW_SPORT;

		// Token: 0x04000250 RID: 592
		public static readonly Guid MEDIASUBTYPE_SPDIF_TAG_241h;

		// Token: 0x04000251 RID: 593
		public static readonly Guid MEDIASUBTYPE_I420;

		// Token: 0x04000252 RID: 594
		public static readonly Guid MEDIASUBTYPE_IYUV;

		// Token: 0x04000253 RID: 595
		public static readonly Guid MEDIASUBTYPE_RGB1;

		// Token: 0x04000254 RID: 596
		public static readonly Guid MEDIASUBTYPE_RGB24;

		// Token: 0x04000255 RID: 597
		public static readonly Guid MEDIASUBTYPE_RGB32;

		// Token: 0x04000256 RID: 598
		public static readonly Guid MEDIASUBTYPE_RGB4;

		// Token: 0x04000257 RID: 599
		public static readonly Guid MEDIASUBTYPE_RGB555;

		// Token: 0x04000258 RID: 600
		public static readonly Guid MEDIASUBTYPE_RGB565;

		// Token: 0x04000259 RID: 601
		public static readonly Guid MEDIASUBTYPE_RGB8;

		// Token: 0x0400025A RID: 602
		public static readonly Guid MEDIASUBTYPE_UYVY;

		// Token: 0x0400025B RID: 603
		public static readonly Guid MEDIASUBTYPE_VIDEOIMAGE;

		// Token: 0x0400025C RID: 604
		public static readonly Guid MEDIASUBTYPE_YUY2;

		// Token: 0x0400025D RID: 605
		public static readonly Guid MEDIASUBTYPE_YV12;

		// Token: 0x0400025E RID: 606
		public static readonly Guid MEDIASUBTYPE_YVU9;

		// Token: 0x0400025F RID: 607
		public static readonly Guid MEDIASUBTYPE_YVYU;

		// Token: 0x04000260 RID: 608
		public static readonly Guid WMFORMAT_MPEG2Video;

		// Token: 0x04000261 RID: 609
		public static readonly Guid WMFORMAT_Script;

		// Token: 0x04000262 RID: 610
		public static readonly Guid WMFORMAT_VideoInfo;

		// Token: 0x04000263 RID: 611
		public static readonly Guid WMFORMAT_WaveFormatEx;

		// Token: 0x04000264 RID: 612
		public static readonly Guid WMFORMAT_WebStream;

		// Token: 0x04000265 RID: 613
		public static readonly Guid WMMEDIASUBTYPE_ACELPnet;

		// Token: 0x04000266 RID: 614
		public static readonly Guid WMMEDIASUBTYPE_Base;

		// Token: 0x04000267 RID: 615
		public static readonly Guid WMMEDIASUBTYPE_DRM;

		// Token: 0x04000268 RID: 616
		public static readonly Guid WMMEDIASUBTYPE_MP3;

		// Token: 0x04000269 RID: 617
		public static readonly Guid WMMEDIASUBTYPE_MP43;

		// Token: 0x0400026A RID: 618
		public static readonly Guid WMMEDIASUBTYPE_MP4S;

		// Token: 0x0400026B RID: 619
		public static readonly Guid WMMEDIASUBTYPE_M4S2;

		// Token: 0x0400026C RID: 620
		public static readonly Guid WMMEDIASUBTYPE_P422;

		// Token: 0x0400026D RID: 621
		public static readonly Guid WMMEDIASUBTYPE_MPEG2_VIDEO;

		// Token: 0x0400026E RID: 622
		public static readonly Guid WMMEDIASUBTYPE_MSS1;

		// Token: 0x0400026F RID: 623
		public static readonly Guid WMMEDIASUBTYPE_MSS2;

		// Token: 0x04000270 RID: 624
		public static readonly Guid WMMEDIASUBTYPE_PCM;

		// Token: 0x04000271 RID: 625
		public static readonly Guid WMMEDIASUBTYPE_WebStream;

		// Token: 0x04000272 RID: 626
		public static readonly Guid WMMEDIASUBTYPE_WMAudio_Lossless;

		// Token: 0x04000273 RID: 627
		public static readonly Guid WMMEDIASUBTYPE_WMAudioV2;

		// Token: 0x04000274 RID: 628
		public static readonly Guid WMMEDIASUBTYPE_WMAudioV7;

		// Token: 0x04000275 RID: 629
		public static readonly Guid WMMEDIASUBTYPE_WMAudioV8;

		// Token: 0x04000276 RID: 630
		public static readonly Guid WMMEDIASUBTYPE_WMAudioV9;

		// Token: 0x04000277 RID: 631
		public static readonly Guid WMMEDIASUBTYPE_WMSP1;

		// Token: 0x04000278 RID: 632
		public static readonly Guid WMMEDIASUBTYPE_WMV1;

		// Token: 0x04000279 RID: 633
		public static readonly Guid WMMEDIASUBTYPE_WMV2;

		// Token: 0x0400027A RID: 634
		public static readonly Guid WMMEDIASUBTYPE_WMV3;

		// Token: 0x0400027B RID: 635
		public static readonly Guid WMMEDIASUBTYPE_WMVA;

		// Token: 0x0400027C RID: 636
		public static readonly Guid WMMEDIASUBTYPE_WMVP;

		// Token: 0x0400027D RID: 637
		public static readonly Guid WMMEDIASUBTYPE_WVP2;

		// Token: 0x0400027E RID: 638
		public static readonly Guid WMMEDIATYPE_Audio;

		// Token: 0x0400027F RID: 639
		public static readonly Guid WMMEDIATYPE_FileTransfer;

		// Token: 0x04000280 RID: 640
		public static readonly Guid WMMEDIATYPE_Image;

		// Token: 0x04000281 RID: 641
		public static readonly Guid WMMEDIATYPE_Script;

		// Token: 0x04000282 RID: 642
		public static readonly Guid WMMEDIATYPE_Text;

		// Token: 0x04000283 RID: 643
		public static readonly Guid WMMEDIATYPE_Video;

		// Token: 0x04000284 RID: 644
		public static readonly Guid WMSCRIPTTYPE_TwoStrings;

		// Token: 0x04000285 RID: 645
		public static readonly Guid MEDIASUBTYPE_WAVE;

		// Token: 0x04000286 RID: 646
		public static readonly Guid MEDIASUBTYPE_AU;

		// Token: 0x04000287 RID: 647
		public static readonly Guid MEDIASUBTYPE_AIFF;

		// Token: 0x04000288 RID: 648
		public static readonly Guid[] AudioSubTypes;

		// Token: 0x04000289 RID: 649
		public static readonly string[] AudioSubTypeNames;
	}
}
