using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000CB RID: 203
	[StructLayout(LayoutKind.Explicit, Pack = 2)]
	internal class WaveBuffer : IWaveBuffer
	{
		// Token: 0x060004A8 RID: 1192 RVA: 0x000162E0 File Offset: 0x000144E0
		public WaveBuffer(int sizeToAllocateInBytes)
		{
			int num = sizeToAllocateInBytes % 4;
			sizeToAllocateInBytes = ((num == 0) ? sizeToAllocateInBytes : (sizeToAllocateInBytes + 4 - num));
			this.byteBuffer = new byte[sizeToAllocateInBytes];
			this.numberOfBytes = 0;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00016317 File Offset: 0x00014517
		public WaveBuffer(byte[] bufferToBoundTo)
		{
			this.BindTo(bufferToBoundTo);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00016326 File Offset: 0x00014526
		public void BindTo(byte[] bufferToBoundTo)
		{
			this.byteBuffer = bufferToBoundTo;
			this.numberOfBytes = 0;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00016336 File Offset: 0x00014536
		public static implicit operator byte[](WaveBuffer waveBuffer)
		{
			return waveBuffer.byteBuffer;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001633E File Offset: 0x0001453E
		public static implicit operator float[](WaveBuffer waveBuffer)
		{
			return waveBuffer.floatBuffer;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00016346 File Offset: 0x00014546
		public static implicit operator int[](WaveBuffer waveBuffer)
		{
			return waveBuffer.intBuffer;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001634E File Offset: 0x0001454E
		public static implicit operator short[](WaveBuffer waveBuffer)
		{
			return waveBuffer.shortBuffer;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00016336 File Offset: 0x00014536
		public byte[] ByteBuffer
		{
			get
			{
				return this.byteBuffer;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001633E File Offset: 0x0001453E
		public float[] FloatBuffer
		{
			get
			{
				return this.floatBuffer;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001634E File Offset: 0x0001454E
		public short[] ShortBuffer
		{
			get
			{
				return this.shortBuffer;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00016346 File Offset: 0x00014546
		public int[] IntBuffer
		{
			get
			{
				return this.intBuffer;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00016356 File Offset: 0x00014556
		public int MaxSize
		{
			get
			{
				return this.byteBuffer.Length;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00016360 File Offset: 0x00014560
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x00016368 File Offset: 0x00014568
		public int ByteBufferCount
		{
			get
			{
				return this.numberOfBytes;
			}
			set
			{
				this.numberOfBytes = this.CheckValidityCount("ByteBufferCount", value, 1);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0001637D File Offset: 0x0001457D
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x00016387 File Offset: 0x00014587
		public int FloatBufferCount
		{
			get
			{
				return this.numberOfBytes / 4;
			}
			set
			{
				this.numberOfBytes = this.CheckValidityCount("FloatBufferCount", value, 4);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0001639C File Offset: 0x0001459C
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x000163A6 File Offset: 0x000145A6
		public int ShortBufferCount
		{
			get
			{
				return this.numberOfBytes / 2;
			}
			set
			{
				this.numberOfBytes = this.CheckValidityCount("ShortBufferCount", value, 2);
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0001637D File Offset: 0x0001457D
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x000163BB File Offset: 0x000145BB
		public int IntBufferCount
		{
			get
			{
				return this.numberOfBytes / 4;
			}
			set
			{
				this.numberOfBytes = this.CheckValidityCount("IntBufferCount", value, 4);
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000163D0 File Offset: 0x000145D0
		public void Clear()
		{
			Array.Clear(this.byteBuffer, 0, this.byteBuffer.Length);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000163E6 File Offset: 0x000145E6
		public void Copy(Array destinationArray)
		{
			Array.Copy(this.byteBuffer, destinationArray, this.numberOfBytes);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000163FC File Offset: 0x000145FC
		private int CheckValidityCount(string argName, int value, int sizeOfValue)
		{
			int num = value * sizeOfValue;
			if (num % 4 != 0)
			{
				throw new ArgumentOutOfRangeException(argName, string.Format("{0} cannot set a count ({1}) that is not 4 bytes aligned ", argName, num));
			}
			if (value < 0 || value > this.byteBuffer.Length / sizeOfValue)
			{
				throw new ArgumentOutOfRangeException(argName, string.Format("{0} cannot set a count that exceed max count {1}", argName, this.byteBuffer.Length / sizeOfValue));
			}
			return num;
		}

		// Token: 0x040004D4 RID: 1236
		[FieldOffset(0)]
		public int numberOfBytes;

		// Token: 0x040004D5 RID: 1237
		[FieldOffset(8)]
		private byte[] byteBuffer;

		// Token: 0x040004D6 RID: 1238
		[FieldOffset(8)]
		private float[] floatBuffer;

		// Token: 0x040004D7 RID: 1239
		[FieldOffset(8)]
		private short[] shortBuffer;

		// Token: 0x040004D8 RID: 1240
		[FieldOffset(8)]
		private int[] intBuffer;
	}
}
