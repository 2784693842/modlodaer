using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
	// Token: 0x02000050 RID: 80
	internal static class ZipOutput
	{
		// Token: 0x0600039C RID: 924 RVA: 0x00017860 File Offset: 0x00015A60
		public static bool WriteCentralDirectoryStructure(Stream s, ICollection<ZipEntry> entries, uint numSegments, Zip64Option zip64, string comment, ZipContainer container)
		{
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = true;
			}
			long num = 0L;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				foreach (ZipEntry zipEntry in entries)
				{
					if (zipEntry.IncludedInMostRecentSave)
					{
						zipEntry.WriteCentralDirectoryEntry(memoryStream);
					}
				}
				byte[] array = memoryStream.ToArray();
				s.Write(array, 0, array.Length);
				num = (long)array.Length;
			}
			CountingStream countingStream = s as CountingStream;
			long num2 = (countingStream != null) ? countingStream.ComputedPosition : s.Position;
			long num3 = num2 - num;
			uint num4 = (zipSegmentedStream != null) ? zipSegmentedStream.CurrentSegment : 0U;
			long num5 = num2 - num3;
			int num6 = ZipOutput.CountEntries(entries);
			bool flag = zip64 == Zip64Option.Always || num6 >= 65535 || num5 > (long)((ulong)-1) || num3 > (long)((ulong)-1);
			byte[] array3;
			if (flag)
			{
				if (zip64 == Zip64Option.Default)
				{
					if (new StackFrame(1).GetMethod().DeclaringType == typeof(ZipFile))
					{
						throw new ZipException("The archive requires a ZIP64 Central Directory. Consider setting the ZipFile.UseZip64WhenSaving property.");
					}
					throw new ZipException("The archive requires a ZIP64 Central Directory. Consider setting the ZipOutputStream.EnableZip64 property.");
				}
				else
				{
					byte[] array2 = ZipOutput.GenZip64EndOfCentralDirectory(num3, num2, num6, numSegments);
					array3 = ZipOutput.GenCentralDirectoryFooter(num3, num2, zip64, num6, comment, container);
					if (num4 != 0U)
					{
						uint value = zipSegmentedStream.ComputeSegment(array2.Length + array3.Length);
						int num7 = 16;
						Array.Copy(BitConverter.GetBytes(value), 0, array2, num7, 4);
						num7 += 4;
						Array.Copy(BitConverter.GetBytes(value), 0, array2, num7, 4);
						num7 = 60;
						Array.Copy(BitConverter.GetBytes(value), 0, array2, num7, 4);
						num7 += 4;
						num7 += 8;
						Array.Copy(BitConverter.GetBytes(value), 0, array2, num7, 4);
					}
					s.Write(array2, 0, array2.Length);
				}
			}
			else
			{
				array3 = ZipOutput.GenCentralDirectoryFooter(num3, num2, zip64, num6, comment, container);
			}
			if (num4 != 0U)
			{
				ushort value2 = (ushort)zipSegmentedStream.ComputeSegment(array3.Length);
				int num8 = 4;
				Array.Copy(BitConverter.GetBytes(value2), 0, array3, num8, 2);
				num8 += 2;
				Array.Copy(BitConverter.GetBytes(value2), 0, array3, num8, 2);
				num8 += 2;
			}
			s.Write(array3, 0, array3.Length);
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = false;
			}
			return flag;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00017AA8 File Offset: 0x00015CA8
		private static Encoding GetEncoding(ZipContainer container, string t)
		{
			ZipOption alternateEncodingUsage = container.AlternateEncodingUsage;
			if (alternateEncodingUsage == ZipOption.Default)
			{
				return container.DefaultEncoding;
			}
			if (alternateEncodingUsage == ZipOption.Always)
			{
				return container.AlternateEncoding;
			}
			Encoding defaultEncoding = container.DefaultEncoding;
			if (t == null)
			{
				return defaultEncoding;
			}
			byte[] bytes = defaultEncoding.GetBytes(t);
			if (defaultEncoding.GetString(bytes, 0, bytes.Length).Equals(t))
			{
				return defaultEncoding;
			}
			return container.AlternateEncoding;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00017B00 File Offset: 0x00015D00
		private static byte[] GenCentralDirectoryFooter(long StartOfCentralDirectory, long EndOfCentralDirectory, Zip64Option zip64, int entryCount, string comment, ZipContainer container)
		{
			Encoding encoding = ZipOutput.GetEncoding(container, comment);
			short num = 22;
			byte[] array = null;
			short num2 = 0;
			if (comment != null && comment.Length != 0)
			{
				array = encoding.GetBytes(comment);
				num2 = (short)array.Length;
			}
			byte[] array2 = new byte[(int)(num + num2)];
			int num3 = 0;
			Array.Copy(BitConverter.GetBytes(101010256U), 0, array2, num3, 4);
			num3 += 4;
			array2[num3++] = 0;
			array2[num3++] = 0;
			array2[num3++] = 0;
			array2[num3++] = 0;
			if (entryCount >= 65535 || zip64 == Zip64Option.Always)
			{
				for (int i = 0; i < 4; i++)
				{
					array2[num3++] = byte.MaxValue;
				}
			}
			else
			{
				array2[num3++] = (byte)(entryCount & 255);
				array2[num3++] = (byte)((entryCount & 65280) >> 8);
				array2[num3++] = (byte)(entryCount & 255);
				array2[num3++] = (byte)((entryCount & 65280) >> 8);
			}
			long num4 = EndOfCentralDirectory - StartOfCentralDirectory;
			if (num4 >= (long)((ulong)-1) || StartOfCentralDirectory >= (long)((ulong)-1))
			{
				for (int i = 0; i < 8; i++)
				{
					array2[num3++] = byte.MaxValue;
				}
			}
			else
			{
				array2[num3++] = (byte)(num4 & 255L);
				array2[num3++] = (byte)((num4 & 65280L) >> 8);
				array2[num3++] = (byte)((num4 & 16711680L) >> 16);
				array2[num3++] = (byte)((num4 & (long)((ulong)-16777216)) >> 24);
				array2[num3++] = (byte)(StartOfCentralDirectory & 255L);
				array2[num3++] = (byte)((StartOfCentralDirectory & 65280L) >> 8);
				array2[num3++] = (byte)((StartOfCentralDirectory & 16711680L) >> 16);
				array2[num3++] = (byte)((StartOfCentralDirectory & (long)((ulong)-16777216)) >> 24);
			}
			if (comment == null || comment.Length == 0)
			{
				array2[num3++] = 0;
				array2[num3++] = 0;
			}
			else
			{
				if ((int)num2 + num3 + 2 > array2.Length)
				{
					num2 = (short)(array2.Length - num3 - 2);
				}
				array2[num3++] = (byte)(num2 & 255);
				array2[num3++] = (byte)(((int)num2 & 65280) >> 8);
				if (num2 != 0)
				{
					int i = 0;
					while (i < (int)num2 && num3 + i < array2.Length)
					{
						array2[num3 + i] = array[i];
						i++;
					}
					num3 += i;
				}
			}
			return array2;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00017D6C File Offset: 0x00015F6C
		private static byte[] GenZip64EndOfCentralDirectory(long StartOfCentralDirectory, long EndOfCentralDirectory, int entryCount, uint numSegments)
		{
			byte[] array = new byte[76];
			int num = 0;
			Array.Copy(BitConverter.GetBytes(101075792U), 0, array, num, 4);
			num += 4;
			Array.Copy(BitConverter.GetBytes(44L), 0, array, num, 8);
			num += 8;
			array[num++] = 45;
			array[num++] = 0;
			array[num++] = 45;
			array[num++] = 0;
			for (int i = 0; i < 8; i++)
			{
				array[num++] = 0;
			}
			long value = (long)entryCount;
			Array.Copy(BitConverter.GetBytes(value), 0, array, num, 8);
			num += 8;
			Array.Copy(BitConverter.GetBytes(value), 0, array, num, 8);
			num += 8;
			Array.Copy(BitConverter.GetBytes(EndOfCentralDirectory - StartOfCentralDirectory), 0, array, num, 8);
			num += 8;
			Array.Copy(BitConverter.GetBytes(StartOfCentralDirectory), 0, array, num, 8);
			num += 8;
			Array.Copy(BitConverter.GetBytes(117853008U), 0, array, num, 4);
			num += 4;
			Array.Copy(BitConverter.GetBytes((numSegments == 0U) ? 0U : (numSegments - 1U)), 0, array, num, 4);
			num += 4;
			Array.Copy(BitConverter.GetBytes(EndOfCentralDirectory), 0, array, num, 8);
			num += 8;
			Array.Copy(BitConverter.GetBytes(numSegments), 0, array, num, 4);
			num += 4;
			return array;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00017E90 File Offset: 0x00016090
		private static int CountEntries(ICollection<ZipEntry> _entries)
		{
			int num = 0;
			using (IEnumerator<ZipEntry> enumerator = _entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IncludedInMostRecentSave)
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
