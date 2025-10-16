using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Ionic.Zip
{
	// Token: 0x02000040 RID: 64
	internal static class SharedUtilities
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000D1AC File Offset: 0x0000B3AC
		public static long GetFileLength(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException(fileName);
			}
			long result = 0L;
			FileShare fileShare = FileShare.ReadWrite;
			fileShare |= FileShare.Delete;
			using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, fileShare))
			{
				result = fileStream.Length;
			}
			return result;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000D200 File Offset: 0x0000B400
		[Conditional("NETCF")]
		public static void Workaround_Ladybug318918(Stream s)
		{
			s.Flush();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000D208 File Offset: 0x0000B408
		private static string SimplifyFwdSlashPath(string path)
		{
			if (path.StartsWith("./"))
			{
				path = path.Substring(2);
			}
			path = path.Replace("/./", "/");
			path = SharedUtilities.doubleDotRegex1.Replace(path, "$1$3");
			return path;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000D248 File Offset: 0x0000B448
		public static string NormalizePathForUseInZipFile(string pathName)
		{
			if (string.IsNullOrEmpty(pathName))
			{
				return pathName;
			}
			if (pathName.Length >= 2 && pathName[1] == ':' && pathName[2] == '\\')
			{
				pathName = pathName.Substring(3);
			}
			pathName = pathName.Replace('\\', '/');
			while (pathName.StartsWith("/"))
			{
				pathName = pathName.Substring(1);
			}
			return SharedUtilities.SimplifyFwdSlashPath(pathName);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000D2B1 File Offset: 0x0000B4B1
		internal static byte[] StringToByteArray(string value, Encoding encoding)
		{
			return encoding.GetBytes(value);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000D2BA File Offset: 0x0000B4BA
		internal static byte[] StringToByteArray(string value)
		{
			return SharedUtilities.StringToByteArray(value, SharedUtilities.utf8);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000D2C7 File Offset: 0x0000B4C7
		internal static string Utf8StringFromBuffer(byte[] buf)
		{
			return SharedUtilities.StringFromBuffer(buf, SharedUtilities.utf8);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000D2D4 File Offset: 0x0000B4D4
		internal static string StringFromBuffer(byte[] buf, Encoding encoding)
		{
			return encoding.GetString(buf, 0, buf.Length);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000D2E4 File Offset: 0x0000B4E4
		internal static int ReadSignature(Stream s)
		{
			int result = 0;
			try
			{
				result = SharedUtilities._ReadFourBytes(s, "n/a");
			}
			catch (BadReadException)
			{
			}
			return result;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000D318 File Offset: 0x0000B518
		internal static int ReadEntrySignature(Stream s)
		{
			int num = 0;
			try
			{
				num = SharedUtilities._ReadFourBytes(s, "n/a");
				if (num == 134695760)
				{
					s.Seek(12L, SeekOrigin.Current);
					num = SharedUtilities._ReadFourBytes(s, "n/a");
					if (num != 67324752)
					{
						s.Seek(8L, SeekOrigin.Current);
						num = SharedUtilities._ReadFourBytes(s, "n/a");
						if (num != 67324752)
						{
							s.Seek(-24L, SeekOrigin.Current);
							num = SharedUtilities._ReadFourBytes(s, "n/a");
						}
					}
				}
			}
			catch (BadReadException)
			{
			}
			return num;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000D3A8 File Offset: 0x0000B5A8
		internal static int ReadInt(Stream s)
		{
			return SharedUtilities._ReadFourBytes(s, "Could not read block - no data!  (position 0x{0:X8})");
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000D3B8 File Offset: 0x0000B5B8
		private static int _ReadFourBytes(Stream s, string message)
		{
			byte[] array = new byte[4];
			if (s.Read(array, 0, array.Length) != array.Length)
			{
				throw new BadReadException(string.Format(message, s.Position));
			}
			return (((int)array[3] * 256 + (int)array[2]) * 256 + (int)array[1]) * 256 + (int)array[0];
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000D414 File Offset: 0x0000B614
		internal static long FindSignature(Stream stream, int SignatureToFind)
		{
			long position = stream.Position;
			int num = 65536;
			byte[] array = new byte[]
			{
				(byte)(SignatureToFind >> 24),
				(byte)((SignatureToFind & 16711680) >> 16),
				(byte)((SignatureToFind & 65280) >> 8),
				(byte)(SignatureToFind & 255)
			};
			byte[] array2 = new byte[num];
			bool flag = false;
			do
			{
				int num2 = stream.Read(array2, 0, array2.Length);
				if (num2 == 0)
				{
					break;
				}
				for (int i = 0; i < num2; i++)
				{
					if (array2[i] == array[3])
					{
						long position2 = stream.Position;
						stream.Seek((long)(i - num2), SeekOrigin.Current);
						flag = (SharedUtilities.ReadSignature(stream) == SignatureToFind);
						if (flag)
						{
							break;
						}
						stream.Seek(position2, SeekOrigin.Begin);
					}
				}
			}
			while (!flag);
			if (!flag)
			{
				stream.Seek(position, SeekOrigin.Begin);
				return -1L;
			}
			return stream.Position - position - 4L;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000D4E4 File Offset: 0x0000B6E4
		internal static DateTime AdjustTime_Reverse(DateTime time)
		{
			if (time.Kind == DateTimeKind.Utc)
			{
				return time;
			}
			DateTime result = time;
			if (DateTime.Now.IsDaylightSavingTime() && !time.IsDaylightSavingTime())
			{
				result = time - new TimeSpan(1, 0, 0);
			}
			else if (!DateTime.Now.IsDaylightSavingTime() && time.IsDaylightSavingTime())
			{
				result = time + new TimeSpan(1, 0, 0);
			}
			return result;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000D550 File Offset: 0x0000B750
		internal static DateTime PackedToDateTime(int packedDateTime)
		{
			if (packedDateTime == 65535 || packedDateTime == 0)
			{
				return new DateTime(1995, 1, 1, 0, 0, 0, 0);
			}
			short num = (short)(packedDateTime & 65535);
			short num2 = (short)(((long)packedDateTime & (long)((ulong)-65536)) >> 16);
			int i = 1980 + (((int)num2 & 65024) >> 9);
			int j = (num2 & 480) >> 5;
			int k = (int)(num2 & 31);
			int num3 = ((int)num & 63488) >> 11;
			int l = (num & 2016) >> 5;
			int m = (int)((num & 31) * 2);
			if (m >= 60)
			{
				l++;
				m = 0;
			}
			if (l >= 60)
			{
				num3++;
				l = 0;
			}
			if (num3 >= 24)
			{
				k++;
				num3 = 0;
			}
			DateTime dateTime = DateTime.Now;
			bool flag = false;
			try
			{
				dateTime = new DateTime(i, j, k, num3, l, m, 0);
				flag = true;
			}
			catch (ArgumentOutOfRangeException)
			{
				if (i == 1980 && (j == 0 || k == 0))
				{
					try
					{
						dateTime = new DateTime(1980, 1, 1, num3, l, m, 0);
						flag = true;
						goto IL_1A1;
					}
					catch (ArgumentOutOfRangeException)
					{
						try
						{
							dateTime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
							flag = true;
						}
						catch (ArgumentOutOfRangeException)
						{
						}
						goto IL_1A1;
					}
				}
				try
				{
					while (i < 1980)
					{
						i++;
					}
					while (i > 2030)
					{
						i--;
					}
					while (j < 1)
					{
						j++;
					}
					while (j > 12)
					{
						j--;
					}
					while (k < 1)
					{
						k++;
					}
					while (k > 28)
					{
						k--;
					}
					while (l < 0)
					{
						l++;
					}
					while (l > 59)
					{
						l--;
					}
					while (m < 0)
					{
						m++;
					}
					while (m > 59)
					{
						m--;
					}
					dateTime = new DateTime(i, j, k, num3, l, m, 0);
					flag = true;
				}
				catch (ArgumentOutOfRangeException)
				{
				}
				IL_1A1:;
			}
			if (!flag)
			{
				string arg = string.Format("y({0}) m({1}) d({2}) h({3}) m({4}) s({5})", new object[]
				{
					i,
					j,
					k,
					num3,
					l,
					m
				});
				throw new ZipException(string.Format("Bad date/time format in the zip file. ({0})", arg));
			}
			dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
			return dateTime;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
		internal static int DateTimeToPacked(DateTime time)
		{
			time = time.ToLocalTime();
			int num = (int)((ushort)((time.Day & 31) | (time.Month << 5 & 480) | (time.Year - 1980 << 9 & 65024)));
			ushort num2 = (ushort)((time.Second / 2 & 31) | (time.Minute << 5 & 2016) | (time.Hour << 11 & 63488));
			return num << 16 | (int)num2;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000D820 File Offset: 0x0000BA20
		public static void CreateAndOpenUniqueTempFile(string dir, out Stream fs, out string filename)
		{
			for (int i = 0; i < 3; i++)
			{
				try
				{
					filename = Path.Combine(dir, SharedUtilities.InternalGetTempFileName());
					fs = new FileStream(filename, FileMode.CreateNew);
					return;
				}
				catch (IOException)
				{
					if (i == 2)
					{
						throw;
					}
				}
			}
			throw new IOException();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000D874 File Offset: 0x0000BA74
		public static string InternalGetTempFileName()
		{
			return "DotNetZip-" + Path.GetRandomFileName().Substring(0, 8) + ".tmp";
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000D894 File Offset: 0x0000BA94
		internal static int ReadWithRetry(Stream s, byte[] buffer, int offset, int count, string FileName)
		{
			int result = 0;
			bool flag = false;
			int num = 0;
			do
			{
				try
				{
					result = s.Read(buffer, offset, count);
					flag = true;
				}
				catch (IOException ex)
				{
					if (!new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).IsUnrestricted())
					{
						throw;
					}
					if (SharedUtilities._HRForException(ex) != 2147942433U)
					{
						throw new IOException(string.Format("Cannot read file {0}", FileName), ex);
					}
					num++;
					if (num > 10)
					{
						throw new IOException(string.Format("Cannot read file {0}, at offset 0x{1:X8} after 10 retries", FileName, offset), ex);
					}
					Thread.Sleep(250 + num * 550);
				}
			}
			while (!flag);
			return result;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000D934 File Offset: 0x0000BB34
		private static uint _HRForException(Exception ex1)
		{
			return (uint)Marshal.GetHRForException(ex1);
		}

		// Token: 0x040001A6 RID: 422
		private static Regex doubleDotRegex1 = new Regex("^(.*/)?([^/\\\\.]+/\\\\.\\\\./)(.+)$");

		// Token: 0x040001A7 RID: 423
		private static Encoding utf8 = Encoding.GetEncoding("UTF-8");
	}
}
