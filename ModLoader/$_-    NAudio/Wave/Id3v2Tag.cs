using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000A9 RID: 169
	internal class Id3v2Tag
	{
		// Token: 0x0600040F RID: 1039 RVA: 0x00014C28 File Offset: 0x00012E28
		public static Id3v2Tag ReadTag(Stream input)
		{
			Id3v2Tag result;
			try
			{
				result = new Id3v2Tag(input);
			}
			catch (FormatException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00014C54 File Offset: 0x00012E54
		public static Id3v2Tag Create(IEnumerable<KeyValuePair<string, string>> tags)
		{
			return Id3v2Tag.ReadTag(Id3v2Tag.CreateId3v2TagStream(tags));
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00014C61 File Offset: 0x00012E61
		private static byte[] FrameSizeToBytes(int n)
		{
			byte[] bytes = BitConverter.GetBytes(n);
			Array.Reverse(bytes);
			return bytes;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00014C70 File Offset: 0x00012E70
		private static byte[] CreateId3v2Frame(string key, string value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value");
			}
			if (key.Length != 4)
			{
				throw new ArgumentOutOfRangeException("key", "key " + key + " must be 4 characters long");
			}
			byte[] array = new byte[]
			{
				byte.MaxValue,
				254
			};
			byte[] array2 = new byte[3];
			byte[] array3 = new byte[2];
			byte[] array4;
			if (key == "COMM")
			{
				array4 = ByteArrayExtensions.Concat(new byte[][]
				{
					new byte[]
					{
						1
					},
					array2,
					array3,
					array,
					Encoding.Unicode.GetBytes(value)
				});
			}
			else
			{
				array4 = ByteArrayExtensions.Concat(new byte[][]
				{
					new byte[]
					{
						1
					},
					array,
					Encoding.Unicode.GetBytes(value)
				});
			}
			return ByteArrayExtensions.Concat(new byte[][]
			{
				Encoding.UTF8.GetBytes(key),
				Id3v2Tag.FrameSizeToBytes(array4.Length),
				new byte[2],
				array4
			});
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00014D8C File Offset: 0x00012F8C
		private static byte[] GetId3TagHeaderSize(int size)
		{
			byte[] array = new byte[4];
			for (int i = array.Length - 1; i >= 0; i--)
			{
				array[i] = (byte)(size % 128);
				size /= 128;
			}
			return array;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00014DC8 File Offset: 0x00012FC8
		private static byte[] CreateId3v2TagHeader(IEnumerable<byte[]> frames)
		{
			int num = 0;
			foreach (byte[] array in frames)
			{
				num += array.Length;
			}
			byte[][] array2 = new byte[4][];
			array2[0] = Encoding.UTF8.GetBytes("ID3");
			int num2 = 1;
			byte[] array3 = new byte[2];
			array3[0] = 3;
			array2[num2] = array3;
			array2[2] = new byte[1];
			array2[3] = Id3v2Tag.GetId3TagHeaderSize(num);
			return ByteArrayExtensions.Concat(array2);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00014E50 File Offset: 0x00013050
		private static Stream CreateId3v2TagStream(IEnumerable<KeyValuePair<string, string>> tags)
		{
			List<byte[]> list = new List<byte[]>();
			foreach (KeyValuePair<string, string> keyValuePair in tags)
			{
				list.Add(Id3v2Tag.CreateId3v2Frame(keyValuePair.Key, keyValuePair.Value));
			}
			byte[] array = Id3v2Tag.CreateId3v2TagHeader(list);
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(array, 0, array.Length);
			foreach (byte[] array2 in list)
			{
				memoryStream.Write(array2, 0, array2.Length);
			}
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00014F18 File Offset: 0x00013118
		private Id3v2Tag(Stream input)
		{
			this.tagStartPosition = input.Position;
			BinaryReader binaryReader = new BinaryReader(input);
			byte[] array = binaryReader.ReadBytes(10);
			if (array.Length >= 3 && array[0] == 73 && array[1] == 68 && array[2] == 51)
			{
				if ((array[5] & 64) == 64)
				{
					byte[] array2 = binaryReader.ReadBytes(4);
					int num = (int)array2[0] * 2097152 + (int)array2[1] * 16384 + (int)(array2[2] * 128);
					byte b = array2[3];
				}
				int num2 = (int)array[6] * 2097152;
				num2 += (int)array[7] * 16384;
				num2 += (int)(array[8] * 128);
				num2 += (int)array[9];
				binaryReader.ReadBytes(num2);
				if ((array[5] & 16) == 16)
				{
					binaryReader.ReadBytes(10);
				}
				this.tagEndPosition = input.Position;
				input.Position = this.tagStartPosition;
				this.rawData = binaryReader.ReadBytes((int)(this.tagEndPosition - this.tagStartPosition));
				return;
			}
			input.Position = this.tagStartPosition;
			throw new FormatException("Not an ID3v2 tag");
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0001502E File Offset: 0x0001322E
		public byte[] RawData
		{
			get
			{
				return this.rawData;
			}
		}

		// Token: 0x040003FB RID: 1019
		private long tagStartPosition;

		// Token: 0x040003FC RID: 1020
		private long tagEndPosition;

		// Token: 0x040003FD RID: 1021
		private byte[] rawData;
	}
}
