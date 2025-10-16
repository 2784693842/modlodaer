using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000070 RID: 112
	internal static class Extensions
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0000DC68 File Offset: 0x0000BE68
		public static int Read(this IPacket packet, byte[] buffer, int index, int count)
		{
			if (index < 0 || index >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0 || index + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			for (int i = 0; i < count; i++)
			{
				int num;
				byte b = (byte)packet.TryPeekBits(8, out num);
				if (num == 0)
				{
					return i;
				}
				buffer[index++] = b;
				packet.SkipBits(8);
			}
			return count;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000DCD0 File Offset: 0x0000BED0
		public static byte[] ReadBytes(this IPacket packet, int count)
		{
			byte[] array = new byte[count];
			int num = packet.Read(array, 0, count);
			if (num < count)
			{
				byte[] array2 = new byte[num];
				Buffer.BlockCopy(array, 0, array2, 0, num);
				return array2;
			}
			return array;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000DD06 File Offset: 0x0000BF06
		public static bool ReadBit(this IPacket packet)
		{
			return packet.ReadBits(1) == 1UL;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000DD14 File Offset: 0x0000BF14
		public static byte PeekByte(this IPacket packet)
		{
			int num;
			return (byte)packet.TryPeekBits(8, out num);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000DD2B File Offset: 0x0000BF2B
		public static byte ReadByte(this IPacket packet)
		{
			return (byte)packet.ReadBits(8);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000DD35 File Offset: 0x0000BF35
		public static short ReadInt16(this IPacket packet)
		{
			return (short)packet.ReadBits(16);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000DD40 File Offset: 0x0000BF40
		public static int ReadInt32(this IPacket packet)
		{
			return (int)packet.ReadBits(32);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000DD4B File Offset: 0x0000BF4B
		public static long ReadInt64(this IPacket packet)
		{
			return (long)packet.ReadBits(64);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000DD55 File Offset: 0x0000BF55
		public static ushort ReadUInt16(this IPacket packet)
		{
			return (ushort)packet.ReadBits(16);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000DD60 File Offset: 0x0000BF60
		public static uint ReadUInt32(this IPacket packet)
		{
			return (uint)packet.ReadBits(32);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000DD4B File Offset: 0x0000BF4B
		public static ulong ReadUInt64(this IPacket packet)
		{
			return packet.ReadBits(64);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000DD6B File Offset: 0x0000BF6B
		public static void SkipBytes(this IPacket packet, int count)
		{
			packet.SkipBits(count * 8);
		}
	}
}
