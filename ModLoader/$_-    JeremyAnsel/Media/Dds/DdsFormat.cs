using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020JeremyAnsel.Media.Dds
{
	// Token: 0x02000035 RID: 53
	internal enum DdsFormat
	{
		// Token: 0x04000101 RID: 257
		Unknown,
		// Token: 0x04000102 RID: 258
		R32G32B32A32Typeless,
		// Token: 0x04000103 RID: 259
		R32G32B32A32Float,
		// Token: 0x04000104 RID: 260
		R32G32B32A32UInt,
		// Token: 0x04000105 RID: 261
		R32G32B32A32SInt,
		// Token: 0x04000106 RID: 262
		R32G32B32Typeless,
		// Token: 0x04000107 RID: 263
		R32G32B32Float,
		// Token: 0x04000108 RID: 264
		R32G32B32UInt,
		// Token: 0x04000109 RID: 265
		R32G32B32SInt,
		// Token: 0x0400010A RID: 266
		R16G16B16A16Typeless,
		// Token: 0x0400010B RID: 267
		R16G16B16A16Float,
		// Token: 0x0400010C RID: 268
		R16G16B16A16UNorm,
		// Token: 0x0400010D RID: 269
		R16G16B16A16UInt,
		// Token: 0x0400010E RID: 270
		R16G16B16A16SNorm,
		// Token: 0x0400010F RID: 271
		R16G16B16A16SInt,
		// Token: 0x04000110 RID: 272
		R32G32Typeless,
		// Token: 0x04000111 RID: 273
		R32G32Float,
		// Token: 0x04000112 RID: 274
		R32G32UInt,
		// Token: 0x04000113 RID: 275
		R32G32SInt,
		// Token: 0x04000114 RID: 276
		R32G8X24Typeless,
		// Token: 0x04000115 RID: 277
		D32FloatS8X24UInt,
		// Token: 0x04000116 RID: 278
		R32FloatX8X24Typeless,
		// Token: 0x04000117 RID: 279
		X32TypelessG8X24UInt,
		// Token: 0x04000118 RID: 280
		R10G10B10A2Typeless,
		// Token: 0x04000119 RID: 281
		R10G10B10A2UNorm,
		// Token: 0x0400011A RID: 282
		R10G10B10A2UInt,
		// Token: 0x0400011B RID: 283
		R11G11B10Float,
		// Token: 0x0400011C RID: 284
		R8G8B8A8Typeless,
		// Token: 0x0400011D RID: 285
		R8G8B8A8UNorm,
		// Token: 0x0400011E RID: 286
		R8G8B8A8UNormSrgb,
		// Token: 0x0400011F RID: 287
		R8G8B8A8UInt,
		// Token: 0x04000120 RID: 288
		R8G8B8A8SNorm,
		// Token: 0x04000121 RID: 289
		R8G8B8A8SInt,
		// Token: 0x04000122 RID: 290
		R16G16Typeless,
		// Token: 0x04000123 RID: 291
		R16G16Float,
		// Token: 0x04000124 RID: 292
		R16G16UNorm,
		// Token: 0x04000125 RID: 293
		R16G16UInt,
		// Token: 0x04000126 RID: 294
		R16G16SNorm,
		// Token: 0x04000127 RID: 295
		R16G16SInt,
		// Token: 0x04000128 RID: 296
		R32Typeless,
		// Token: 0x04000129 RID: 297
		D32Float,
		// Token: 0x0400012A RID: 298
		R32Float,
		// Token: 0x0400012B RID: 299
		R32UInt,
		// Token: 0x0400012C RID: 300
		R32SInt,
		// Token: 0x0400012D RID: 301
		R24G8Typeless,
		// Token: 0x0400012E RID: 302
		D24UNormS8UInt,
		// Token: 0x0400012F RID: 303
		R24UNormX8Typeless,
		// Token: 0x04000130 RID: 304
		X24TypelessG8UInt,
		// Token: 0x04000131 RID: 305
		R8G8Typeless,
		// Token: 0x04000132 RID: 306
		R8G8UNorm,
		// Token: 0x04000133 RID: 307
		R8G8UInt,
		// Token: 0x04000134 RID: 308
		R8G8SNorm,
		// Token: 0x04000135 RID: 309
		R8G8SInt,
		// Token: 0x04000136 RID: 310
		R16Typeless,
		// Token: 0x04000137 RID: 311
		R16Float,
		// Token: 0x04000138 RID: 312
		D16UNorm,
		// Token: 0x04000139 RID: 313
		R16UNorm,
		// Token: 0x0400013A RID: 314
		R16UInt,
		// Token: 0x0400013B RID: 315
		R16SNorm,
		// Token: 0x0400013C RID: 316
		R16SInt,
		// Token: 0x0400013D RID: 317
		R8Typeless,
		// Token: 0x0400013E RID: 318
		R8UNorm,
		// Token: 0x0400013F RID: 319
		R8UInt,
		// Token: 0x04000140 RID: 320
		R8SNorm,
		// Token: 0x04000141 RID: 321
		R8SInt,
		// Token: 0x04000142 RID: 322
		A8UNorm,
		// Token: 0x04000143 RID: 323
		R1UNorm,
		// Token: 0x04000144 RID: 324
		R9G9B9E5SharedExp,
		// Token: 0x04000145 RID: 325
		R8G8B8G8UNorm,
		// Token: 0x04000146 RID: 326
		G8R8G8B8UNorm,
		// Token: 0x04000147 RID: 327
		BC1Typeless,
		// Token: 0x04000148 RID: 328
		BC1UNorm,
		// Token: 0x04000149 RID: 329
		BC1UNormSrgb,
		// Token: 0x0400014A RID: 330
		BC2Typeless,
		// Token: 0x0400014B RID: 331
		BC2UNorm,
		// Token: 0x0400014C RID: 332
		BC2UNormSrgb,
		// Token: 0x0400014D RID: 333
		BC3Typeless,
		// Token: 0x0400014E RID: 334
		BC3UNorm,
		// Token: 0x0400014F RID: 335
		BC3UNormSrgb,
		// Token: 0x04000150 RID: 336
		BC4Typeless,
		// Token: 0x04000151 RID: 337
		BC4UNorm,
		// Token: 0x04000152 RID: 338
		BC4SNorm,
		// Token: 0x04000153 RID: 339
		BC5Typeless,
		// Token: 0x04000154 RID: 340
		BC5UNorm,
		// Token: 0x04000155 RID: 341
		BC5SNorm,
		// Token: 0x04000156 RID: 342
		B5G6R5UNorm,
		// Token: 0x04000157 RID: 343
		B5G5R5A1UNorm,
		// Token: 0x04000158 RID: 344
		B8G8R8A8UNorm,
		// Token: 0x04000159 RID: 345
		B8G8R8X8UNorm,
		// Token: 0x0400015A RID: 346
		R10G10B10XRBiasA2UNorm,
		// Token: 0x0400015B RID: 347
		B8G8R8A8Typeless,
		// Token: 0x0400015C RID: 348
		B8G8R8A8UNormSrgb,
		// Token: 0x0400015D RID: 349
		B8G8R8X8Typeless,
		// Token: 0x0400015E RID: 350
		B8G8R8X8UNormSrgb,
		// Token: 0x0400015F RID: 351
		BC6HalfTypeless,
		// Token: 0x04000160 RID: 352
		BC6HalfUF16,
		// Token: 0x04000161 RID: 353
		BC6HalfSF16,
		// Token: 0x04000162 RID: 354
		BC7Typeless,
		// Token: 0x04000163 RID: 355
		BC7UNorm,
		// Token: 0x04000164 RID: 356
		BC7UNormSrgb,
		// Token: 0x04000165 RID: 357
		AYuv,
		// Token: 0x04000166 RID: 358
		Y410,
		// Token: 0x04000167 RID: 359
		Y416,
		// Token: 0x04000168 RID: 360
		NV12,
		// Token: 0x04000169 RID: 361
		P010,
		// Token: 0x0400016A RID: 362
		P016,
		// Token: 0x0400016B RID: 363
		P420Opaque,
		// Token: 0x0400016C RID: 364
		Yuy2,
		// Token: 0x0400016D RID: 365
		Y210,
		// Token: 0x0400016E RID: 366
		Y216,
		// Token: 0x0400016F RID: 367
		NV11,
		// Token: 0x04000170 RID: 368
		AI44,
		// Token: 0x04000171 RID: 369
		IA44,
		// Token: 0x04000172 RID: 370
		P8,
		// Token: 0x04000173 RID: 371
		A8P8,
		// Token: 0x04000174 RID: 372
		B4G4R4A4UNorm
	}
}
