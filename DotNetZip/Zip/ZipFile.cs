using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Ionic.Zlib;
using Microsoft.CSharp;

namespace Ionic.Zip
{
	// Token: 0x0200004B RID: 75
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00005")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ZipFile : IEnumerable, IEnumerable<ZipEntry>, IDisposable
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x00013316 File Offset: 0x00011516
		public ZipEntry AddItem(string fileOrDirectoryName)
		{
			return this.AddItem(fileOrDirectoryName, null);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00013320 File Offset: 0x00011520
		public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
		{
			if (File.Exists(fileOrDirectoryName))
			{
				return this.AddFile(fileOrDirectoryName, directoryPathInArchive);
			}
			if (Directory.Exists(fileOrDirectoryName))
			{
				return this.AddDirectory(fileOrDirectoryName, directoryPathInArchive);
			}
			throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", fileOrDirectoryName));
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00013354 File Offset: 0x00011554
		public ZipEntry AddFile(string fileName)
		{
			return this.AddFile(fileName, null);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00013360 File Offset: 0x00011560
		public ZipEntry AddFile(string fileName, string directoryPathInArchive)
		{
			string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			ZipEntry ze = ZipEntry.CreateFromFile(fileName, nameInArchive);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
			}
			return this._InternalAddEntry(ze);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000133A0 File Offset: 0x000115A0
		public void RemoveEntries(ICollection<ZipEntry> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (ZipEntry entry in entriesToRemove)
			{
				this.RemoveEntry(entry);
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000133F8 File Offset: 0x000115F8
		public void RemoveEntries(ICollection<string> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (string fileName in entriesToRemove)
			{
				this.RemoveEntry(fileName);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00013450 File Offset: 0x00011650
		public void AddFiles(IEnumerable<string> fileNames)
		{
			this.AddFiles(fileNames, null);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0001345A File Offset: 0x0001165A
		public void UpdateFiles(IEnumerable<string> fileNames)
		{
			this.UpdateFiles(fileNames, null);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00013464 File Offset: 0x00011664
		public void AddFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			this.AddFiles(fileNames, false, directoryPathInArchive);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00013470 File Offset: 0x00011670
		public void AddFiles(IEnumerable<string> fileNames, bool preserveDirHierarchy, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this._addOperationCanceled = false;
			this.OnAddStarted();
			if (preserveDirHierarchy)
			{
				using (IEnumerator<string> enumerator = fileNames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (this._addOperationCanceled)
						{
							break;
						}
						if (directoryPathInArchive != null)
						{
							string fullPath = Path.GetFullPath(Path.Combine(directoryPathInArchive, Path.GetDirectoryName(text)));
							this.AddFile(text, fullPath);
						}
						else
						{
							this.AddFile(text, null);
						}
					}
					goto IL_AC;
				}
			}
			foreach (string fileName in fileNames)
			{
				if (this._addOperationCanceled)
				{
					break;
				}
				this.AddFile(fileName, directoryPathInArchive);
			}
			IL_AC:
			if (!this._addOperationCanceled)
			{
				this.OnAddCompleted();
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00013554 File Offset: 0x00011754
		public void UpdateFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this.OnAddStarted();
			foreach (string fileName in fileNames)
			{
				this.UpdateFile(fileName, directoryPathInArchive);
			}
			this.OnAddCompleted();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000135B8 File Offset: 0x000117B8
		public ZipEntry UpdateFile(string fileName)
		{
			return this.UpdateFile(fileName, null);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000135C4 File Offset: 0x000117C4
		public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			if (this[fileName2] != null)
			{
				this.RemoveEntry(fileName2);
			}
			return this.AddFile(fileName, directoryPathInArchive);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000135F1 File Offset: 0x000117F1
		public ZipEntry UpdateDirectory(string directoryName)
		{
			return this.UpdateDirectory(directoryName, null);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000135FB File Offset: 0x000117FB
		public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00013606 File Offset: 0x00011806
		public void UpdateItem(string itemName)
		{
			this.UpdateItem(itemName, null);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00013610 File Offset: 0x00011810
		public void UpdateItem(string itemName, string directoryPathInArchive)
		{
			if (File.Exists(itemName))
			{
				this.UpdateFile(itemName, directoryPathInArchive);
				return;
			}
			if (Directory.Exists(itemName))
			{
				this.UpdateDirectory(itemName, directoryPathInArchive);
				return;
			}
			throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", itemName));
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00013646 File Offset: 0x00011846
		public ZipEntry AddEntry(string entryName, string content)
		{
			return this.AddEntry(entryName, content, Encoding.Default);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00013658 File Offset: 0x00011858
		public ZipEntry AddEntry(string entryName, string content, Encoding encoding)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, encoding);
			streamWriter.Write(content);
			streamWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return this.AddEntry(entryName, memoryStream);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00013690 File Offset: 0x00011890
		public ZipEntry AddEntry(string entryName, Stream stream)
		{
			ZipEntry zipEntry = ZipEntry.CreateForStream(entryName, stream);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000136DC File Offset: 0x000118DC
		public ZipEntry AddEntry(string entryName, WriteDelegate writer)
		{
			ZipEntry ze = ZipEntry.CreateForWriter(entryName, writer);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(ze);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00013714 File Offset: 0x00011914
		public ZipEntry AddEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			ZipEntry zipEntry = ZipEntry.CreateForJitStreamProvider(entryName, opener, closer);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00013760 File Offset: 0x00011960
		private ZipEntry _InternalAddEntry(ZipEntry ze)
		{
			ze._container = new ZipContainer(this);
			ze.CompressionMethod = this.CompressionMethod;
			ze.CompressionLevel = this.CompressionLevel;
			ze.ExtractExistingFile = this.ExtractExistingFile;
			ze.ZipErrorAction = this.ZipErrorAction;
			ze.SetCompression = this.SetCompression;
			ze.AlternateEncoding = this.AlternateEncoding;
			ze.AlternateEncodingUsage = this.AlternateEncodingUsage;
			ze.Password = this._Password;
			ze.Encryption = this.Encryption;
			ze.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			ze.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			this.InternalAddEntry(ze.FileName, ze);
			this.AfterAddEntry(ze);
			return ze;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00013812 File Offset: 0x00011A12
		public ZipEntry UpdateEntry(string entryName, string content)
		{
			return this.UpdateEntry(entryName, content, Encoding.Default);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00013821 File Offset: 0x00011A21
		public ZipEntry UpdateEntry(string entryName, string content, Encoding encoding)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, content, encoding);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00013833 File Offset: 0x00011A33
		public ZipEntry UpdateEntry(string entryName, WriteDelegate writer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, writer);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00013844 File Offset: 0x00011A44
		public ZipEntry UpdateEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, opener, closer);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00013856 File Offset: 0x00011A56
		public ZipEntry UpdateEntry(string entryName, Stream stream)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00013868 File Offset: 0x00011A68
		private void RemoveEntryForUpdate(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			string directoryPathInArchive = null;
			if (entryName.IndexOf('\\') != -1)
			{
				directoryPathInArchive = Path.GetDirectoryName(entryName);
				entryName = Path.GetFileName(entryName);
			}
			string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
			if (this[fileName] != null)
			{
				this.RemoveEntry(fileName);
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000138BC File Offset: 0x00011ABC
		public ZipEntry AddEntry(string entryName, byte[] byteContent)
		{
			if (byteContent == null)
			{
				throw new ArgumentException("bad argument", "byteContent");
			}
			MemoryStream stream = new MemoryStream(byteContent);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000138EB File Offset: 0x00011AEB
		public ZipEntry UpdateEntry(string entryName, byte[] byteContent)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, byteContent);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x000138FC File Offset: 0x00011AFC
		public ZipEntry AddDirectory(string directoryName)
		{
			return this.AddDirectory(directoryName, null);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00013906 File Offset: 0x00011B06
		public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00013914 File Offset: 0x00011B14
		public ZipEntry AddDirectoryByName(string directoryNameInArchive)
		{
			ZipEntry zipEntry = ZipEntry.CreateFromNothing(directoryNameInArchive);
			zipEntry._container = new ZipContainer(this);
			zipEntry.MarkAsDirectory();
			zipEntry.AlternateEncoding = this.AlternateEncoding;
			zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			zipEntry._Source = ZipEntrySource.Stream;
			this.InternalAddEntry(zipEntry.FileName, zipEntry);
			this.AfterAddEntry(zipEntry);
			return zipEntry;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001399B File Offset: 0x00011B9B
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action)
		{
			if (rootDirectoryPathInArchive == null)
			{
				rootDirectoryPathInArchive = "";
			}
			return this.AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, true, 0);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x000139B2 File Offset: 0x00011BB2
		internal void InternalAddEntry(string name, ZipEntry entry)
		{
			this._entries.Add(name, entry);
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000139D0 File Offset: 0x00011BD0
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action, bool recurse, int level)
		{
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("{0} {1}...", (action == AddOrUpdateAction.AddOnly) ? "adding" : "Adding or updating", directoryName);
			}
			if (level == 0)
			{
				this._addOperationCanceled = false;
				this.OnAddStarted();
			}
			if (this._addOperationCanceled)
			{
				return null;
			}
			string text = rootDirectoryPathInArchive;
			ZipEntry zipEntry = null;
			if (level > 0)
			{
				int num = directoryName.Length;
				for (int i = level; i > 0; i--)
				{
					num = directoryName.LastIndexOfAny("/\\".ToCharArray(), num - 1, num - 1);
				}
				text = directoryName.Substring(num + 1);
				text = Path.Combine(rootDirectoryPathInArchive, text);
			}
			if (level > 0 || rootDirectoryPathInArchive != "")
			{
				zipEntry = ZipEntry.CreateFromFile(directoryName, text);
				zipEntry._container = new ZipContainer(this);
				zipEntry.AlternateEncoding = this.AlternateEncoding;
				zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
				zipEntry.MarkAsDirectory();
				zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
				zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
				if (!this._entries.ContainsKey(zipEntry.FileName))
				{
					this.InternalAddEntry(zipEntry.FileName, zipEntry);
					this.AfterAddEntry(zipEntry);
				}
				text = zipEntry.FileName;
			}
			if (!this._addOperationCanceled)
			{
				string[] files = Directory.GetFiles(directoryName);
				if (recurse)
				{
					foreach (string fileName in files)
					{
						if (this._addOperationCanceled)
						{
							break;
						}
						if (action == AddOrUpdateAction.AddOnly)
						{
							this.AddFile(fileName, text);
						}
						else
						{
							this.UpdateFile(fileName, text);
						}
					}
					if (!this._addOperationCanceled)
					{
						foreach (string text2 in Directory.GetDirectories(directoryName))
						{
							FileAttributes attributes = File.GetAttributes(text2);
							if (this.AddDirectoryWillTraverseReparsePoints || (attributes & FileAttributes.ReparsePoint) == (FileAttributes)0)
							{
								this.AddOrUpdateDirectoryImpl(text2, rootDirectoryPathInArchive, action, recurse, level + 1);
							}
						}
					}
				}
			}
			if (level == 0)
			{
				this.OnAddCompleted();
			}
			return zipEntry;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00013BAA File Offset: 0x00011DAA
		public static bool CheckZip(string zipFileName)
		{
			return ZipFile.CheckZip(zipFileName, false, null);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00013BB4 File Offset: 0x00011DB4
		public static bool CheckZip(string zipFileName, bool fixIfNecessary, TextWriter writer)
		{
			ZipFile zipFile = null;
			ZipFile zipFile2 = null;
			bool flag = true;
			try
			{
				zipFile = new ZipFile();
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile2 = ZipFile.Read(zipFileName);
				foreach (ZipEntry zipEntry in zipFile)
				{
					foreach (ZipEntry zipEntry2 in zipFile2)
					{
						if (zipEntry.FileName == zipEntry2.FileName)
						{
							if (zipEntry._RelativeOffsetOfLocalHeader != zipEntry2._RelativeOffsetOfLocalHeader)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in RelativeOffsetOfLocalHeader  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._RelativeOffsetOfLocalHeader, zipEntry2._RelativeOffsetOfLocalHeader);
								}
							}
							if (zipEntry._CompressedSize != zipEntry2._CompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressedSize  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._CompressedSize, zipEntry2._CompressedSize);
								}
							}
							if (zipEntry._UncompressedSize != zipEntry2._UncompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in UncompressedSize  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._UncompressedSize, zipEntry2._UncompressedSize);
								}
							}
							if (zipEntry.CompressionMethod != zipEntry2.CompressionMethod)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressionMethod  (0x{1:X4} != 0x{2:X4})", zipEntry.FileName, zipEntry.CompressionMethod, zipEntry2.CompressionMethod);
								}
							}
							if (zipEntry.Crc == zipEntry2.Crc)
							{
								break;
							}
							flag = false;
							if (writer != null)
							{
								writer.WriteLine("{0}: mismatch in Crc32  (0x{1:X4} != 0x{2:X4})", zipEntry.FileName, zipEntry.Crc, zipEntry2.Crc);
								break;
							}
							break;
						}
					}
				}
				zipFile2.Dispose();
				zipFile2 = null;
				if (!flag && fixIfNecessary)
				{
					string text = Path.GetFileNameWithoutExtension(zipFileName);
					text = string.Format("{0}_fixed.zip", text);
					zipFile.Save(text);
				}
			}
			finally
			{
				if (zipFile != null)
				{
					zipFile.Dispose();
				}
				if (zipFile2 != null)
				{
					zipFile2.Dispose();
				}
			}
			return flag;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00013E24 File Offset: 0x00012024
		public static void FixZipDirectory(string zipFileName)
		{
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile.Save(zipFileName);
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00013E68 File Offset: 0x00012068
		public static bool CheckZipPassword(string zipFileName, string password)
		{
			bool result = false;
			try
			{
				using (ZipFile zipFile = ZipFile.Read(zipFileName))
				{
					foreach (ZipEntry zipEntry in zipFile)
					{
						if (!zipEntry.IsDirectory && zipEntry.UsesEncryption)
						{
							zipEntry.ExtractWithPassword(Stream.Null, password);
						}
					}
				}
				result = true;
			}
			catch (BadPasswordException)
			{
			}
			return result;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00013EFC File Offset: 0x000120FC
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("          ZipFile: {0}\n", this.Name));
				if (!string.IsNullOrEmpty(this._Comment))
				{
					stringBuilder.Append(string.Format("          Comment: {0}\n", this._Comment));
				}
				if (this._versionMadeBy != 0)
				{
					stringBuilder.Append(string.Format("  version made by: 0x{0:X4}\n", this._versionMadeBy));
				}
				if (this._versionNeededToExtract != 0)
				{
					stringBuilder.Append(string.Format("needed to extract: 0x{0:X4}\n", this._versionNeededToExtract));
				}
				stringBuilder.Append(string.Format("       uses ZIP64: {0}\n", this.InputUsesZip64));
				stringBuilder.Append(string.Format("     disk with CD: {0}\n", this._diskNumberWithCd));
				if (this._OffsetOfCentralDirectory == 4294967295U)
				{
					stringBuilder.Append(string.Format("      CD64 offset: 0x{0:X16}\n", this._OffsetOfCentralDirectory64));
				}
				else
				{
					stringBuilder.Append(string.Format("        CD offset: 0x{0:X8}\n", this._OffsetOfCentralDirectory));
				}
				stringBuilder.Append("\n");
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					stringBuilder.Append(zipEntry.Info);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00014070 File Offset: 0x00012270
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00014078 File Offset: 0x00012278
		public bool FullScan { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00014081 File Offset: 0x00012281
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x00014089 File Offset: 0x00012289
		public bool SortEntriesBeforeSaving { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00014092 File Offset: 0x00012292
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0001409A File Offset: 0x0001229A
		public bool AddDirectoryWillTraverseReparsePoints { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x000140A3 File Offset: 0x000122A3
		// (set) Token: 0x060002EA RID: 746 RVA: 0x000140AB File Offset: 0x000122AB
		public int BufferSize
		{
			get
			{
				return this._BufferSize;
			}
			set
			{
				this._BufferSize = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002EB RID: 747 RVA: 0x000140B4 File Offset: 0x000122B4
		// (set) Token: 0x060002EC RID: 748 RVA: 0x000140BC File Offset: 0x000122BC
		public int CodecBufferSize { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002ED RID: 749 RVA: 0x000140C5 File Offset: 0x000122C5
		// (set) Token: 0x060002EE RID: 750 RVA: 0x000140CD File Offset: 0x000122CD
		public bool FlattenFoldersOnExtract { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002EF RID: 751 RVA: 0x000140D6 File Offset: 0x000122D6
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x000140DE File Offset: 0x000122DE
		public CompressionStrategy Strategy
		{
			get
			{
				return this._Strategy;
			}
			set
			{
				this._Strategy = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x000140E7 File Offset: 0x000122E7
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x000140EF File Offset: 0x000122EF
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x000140F8 File Offset: 0x000122F8
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x00014100 File Offset: 0x00012300
		public CompressionLevel CompressionLevel { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00014109 File Offset: 0x00012309
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x00014111 File Offset: 0x00012311
		public CompressionMethod CompressionMethod
		{
			get
			{
				return this._compressionMethod;
			}
			set
			{
				this._compressionMethod = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0001411A File Offset: 0x0001231A
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x00014122 File Offset: 0x00012322
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this._contentsChanged = true;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00014132 File Offset: 0x00012332
		// (set) Token: 0x060002FA RID: 762 RVA: 0x0001413A File Offset: 0x0001233A
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return this._emitNtfsTimes;
			}
			set
			{
				this._emitNtfsTimes = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00014143 File Offset: 0x00012343
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0001414B File Offset: 0x0001234B
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return this._emitUnixTimes;
			}
			set
			{
				this._emitUnixTimes = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00014154 File Offset: 0x00012354
		internal bool Verbose
		{
			get
			{
				return this._StatusMessageTextWriter != null;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0001415F File Offset: 0x0001235F
		public bool ContainsEntry(string name)
		{
			return this._entries.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00014172 File Offset: 0x00012372
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0001417A File Offset: 0x0001237A
		public bool CaseSensitiveRetrieval
		{
			get
			{
				return this._CaseSensitiveRetrieval;
			}
			set
			{
				if (value != this._CaseSensitiveRetrieval)
				{
					this._CaseSensitiveRetrieval = value;
					this._initEntriesDictionary();
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00014192 File Offset: 0x00012392
		// (set) Token: 0x06000302 RID: 770 RVA: 0x000141B1 File Offset: 0x000123B1
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.GetEncoding("UTF-8") && this._alternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.GetEncoding("UTF-8");
					this._alternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this._alternateEncoding = ZipFile.DefaultEncoding;
				this._alternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000303 RID: 771 RVA: 0x000141E0 File Offset: 0x000123E0
		// (set) Token: 0x06000304 RID: 772 RVA: 0x000141E8 File Offset: 0x000123E8
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				return this._zip64;
			}
			set
			{
				this._zip64 = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000305 RID: 773 RVA: 0x000141F4 File Offset: 0x000123F4
		public bool? RequiresZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				if (!this._hasBeenSaved || this._contentsChanged)
				{
					bool? result = null;
					return result;
				}
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					bool? result = zipEntry.RequiresZip64;
					if (result.Value)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00014298 File Offset: 0x00012498
		public bool? OutputUsedZip64
		{
			get
			{
				return this._OutputUsesZip64;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000307 RID: 775 RVA: 0x000142A0 File Offset: 0x000124A0
		public bool? InputUsesZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				foreach (ZipEntry zipEntry in this)
				{
					if (zipEntry.Source != ZipEntrySource.ZipFile)
					{
						return null;
					}
					if (zipEntry._InputUsesZip64)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00014328 File Offset: 0x00012528
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0001433B File Offset: 0x0001253B
		[Obsolete("use AlternateEncoding instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (this._alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return this._alternateEncoding;
				}
				return null;
			}
			set
			{
				this._alternateEncoding = value;
				this._alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0001434B File Offset: 0x0001254B
		// (set) Token: 0x0600030B RID: 779 RVA: 0x00014353 File Offset: 0x00012553
		public Encoding AlternateEncoding
		{
			get
			{
				return this._alternateEncoding;
			}
			set
			{
				this._alternateEncoding = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0001435C File Offset: 0x0001255C
		// (set) Token: 0x0600030D RID: 781 RVA: 0x00014364 File Offset: 0x00012564
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return this._alternateEncodingUsage;
			}
			set
			{
				this._alternateEncodingUsage = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0001436D File Offset: 0x0001256D
		public static Encoding DefaultEncoding
		{
			get
			{
				return ZipFile._defaultEncoding;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00014374 File Offset: 0x00012574
		// (set) Token: 0x06000310 RID: 784 RVA: 0x0001437C File Offset: 0x0001257C
		public TextWriter StatusMessageTextWriter
		{
			get
			{
				return this._StatusMessageTextWriter;
			}
			set
			{
				this._StatusMessageTextWriter = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00014385 File Offset: 0x00012585
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0001438D File Offset: 0x0001258D
		public string TempFileFolder
		{
			get
			{
				return this._TempFileFolder;
			}
			set
			{
				this._TempFileFolder = value;
				if (value == null)
				{
					return;
				}
				if (!Directory.Exists(value))
				{
					throw new FileNotFoundException(string.Format("That directory ({0}) does not exist.", value));
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000314 RID: 788 RVA: 0x000143DB File Offset: 0x000125DB
		// (set) Token: 0x06000313 RID: 787 RVA: 0x000143B3 File Offset: 0x000125B3
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password == null)
				{
					this.Encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this.Encryption == EncryptionAlgorithm.None)
				{
					this.Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000315 RID: 789 RVA: 0x000143E3 File Offset: 0x000125E3
		// (set) Token: 0x06000316 RID: 790 RVA: 0x000143EB File Offset: 0x000125EB
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000317 RID: 791 RVA: 0x000143F4 File Offset: 0x000125F4
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0001440B File Offset: 0x0001260B
		public ZipErrorAction ZipErrorAction
		{
			get
			{
				if (this.ZipError != null)
				{
					this._zipErrorAction = ZipErrorAction.InvokeErrorEvent;
				}
				return this._zipErrorAction;
			}
			set
			{
				this._zipErrorAction = value;
				if (this._zipErrorAction != ZipErrorAction.InvokeErrorEvent && this.ZipError != null)
				{
					this.ZipError = null;
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0001442C File Offset: 0x0001262C
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00014434 File Offset: 0x00012634
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._Encryption;
			}
			set
			{
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._Encryption = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0001444C File Offset: 0x0001264C
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00014454 File Offset: 0x00012654
		public SetCompressionCallback SetCompression { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0001445D File Offset: 0x0001265D
		// (set) Token: 0x0600031E RID: 798 RVA: 0x00014465 File Offset: 0x00012665
		public int MaxOutputSegmentSize
		{
			get
			{
				return this._maxOutputSegmentSize;
			}
			set
			{
				if (value < 65536 && value != 0)
				{
					throw new ZipException("The minimum acceptable segment size is 65536.");
				}
				this._maxOutputSegmentSize = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00014484 File Offset: 0x00012684
		public int NumberOfSegmentsForMostRecentSave
		{
			get
			{
				return (int)(this._numberOfSegmentsForMostRecentSave + 1U);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000144B3 File Offset: 0x000126B3
		// (set) Token: 0x06000320 RID: 800 RVA: 0x0001448E File Offset: 0x0001268E
		public long ParallelDeflateThreshold
		{
			get
			{
				return this._ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1L && value < 65536L)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateThreshold should be -1, 0, or > 65536");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000322 RID: 802 RVA: 0x000144BB File Offset: 0x000126BB
		// (set) Token: 0x06000323 RID: 803 RVA: 0x000144C3 File Offset: 0x000126C3
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000144E0 File Offset: 0x000126E0
		public override string ToString()
		{
			return string.Format("ZipFile::{0}", this.Name);
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000325 RID: 805 RVA: 0x000144F2 File Offset: 0x000126F2
		public static Version LibraryVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00014503 File Offset: 0x00012703
		internal void NotifyEntryChanged()
		{
			this._contentsChanged = true;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0001450C File Offset: 0x0001270C
		internal Stream StreamForDiskNumber(uint diskNumber)
		{
			if (diskNumber + 1U == this._diskNumberWithCd || (diskNumber == 0U && this._diskNumberWithCd == 0U))
			{
				return this.ReadStream;
			}
			return ZipSegmentedStream.ForReading(this._readName ?? this._name, diskNumber, this._diskNumberWithCd);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00014548 File Offset: 0x00012748
		internal void Reset(bool whileSaving)
		{
			if (this._JustSaved)
			{
				using (ZipFile zipFile = new ZipFile())
				{
					zipFile._readName = (zipFile._name = (whileSaving ? (this._readName ?? this._name) : this._name));
					zipFile.AlternateEncoding = this.AlternateEncoding;
					zipFile.AlternateEncodingUsage = this.AlternateEncodingUsage;
					ZipFile.ReadIntoInstance(zipFile);
					foreach (ZipEntry zipEntry in zipFile)
					{
						foreach (ZipEntry zipEntry2 in this)
						{
							if (zipEntry.FileName == zipEntry2.FileName)
							{
								zipEntry2.CopyMetaData(zipEntry);
								break;
							}
						}
					}
				}
				this._JustSaved = false;
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00014654 File Offset: 0x00012854
		public ZipFile(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("Could not read {0} as a zip file", fileName), innerException);
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000146E8 File Offset: 0x000128E8
		public ZipFile(string fileName, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001478C File Offset: 0x0001298C
		public ZipFile()
		{
			this._InitInstance(null, null);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000147FC File Offset: 0x000129FC
		public ZipFile(Encoding encoding)
		{
			this.AlternateEncoding = encoding;
			this.AlternateEncodingUsage = ZipOption.Always;
			this._InitInstance(null, null);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0001487C File Offset: 0x00012A7C
		public ZipFile(string fileName, TextWriter statusMessageWriter)
		{
			try
			{
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00014910 File Offset: 0x00012B10
		public ZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000149B4 File Offset: 0x00012BB4
		public void Initialize(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000149F0 File Offset: 0x00012BF0
		private void _initEntriesDictionary()
		{
			StringComparer comparer = this.CaseSensitiveRetrieval ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
			this._entries = ((this._entries == null) ? new Dictionary<string, ZipEntry>(comparer) : new Dictionary<string, ZipEntry>(this._entries, comparer));
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00014A34 File Offset: 0x00012C34
		private void _InitInstance(string zipFileName, TextWriter statusMessageWriter)
		{
			this._name = zipFileName;
			this._StatusMessageTextWriter = statusMessageWriter;
			this._contentsChanged = true;
			this.AddDirectoryWillTraverseReparsePoints = true;
			this.CompressionLevel = CompressionLevel.Default;
			this.ParallelDeflateThreshold = 524288L;
			this._initEntriesDictionary();
			if (File.Exists(this._name))
			{
				if (this.FullScan)
				{
					ZipFile.ReadIntoInstance_Orig(this);
				}
				else
				{
					ZipFile.ReadIntoInstance(this);
				}
				this._fileAlreadyExists = true;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00014AA0 File Offset: 0x00012CA0
		private List<ZipEntry> ZipEntriesAsList
		{
			get
			{
				if (this._zipEntriesAsList == null)
				{
					this._zipEntriesAsList = new List<ZipEntry>(this._entries.Values);
				}
				return this._zipEntriesAsList;
			}
		}

		// Token: 0x170000BB RID: 187
		public ZipEntry this[int ix]
		{
			get
			{
				return this.ZipEntriesAsList[ix];
			}
		}

		// Token: 0x170000BC RID: 188
		public ZipEntry this[string fileName]
		{
			get
			{
				string text = SharedUtilities.NormalizePathForUseInZipFile(fileName);
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				text = text.Replace("/", "\\");
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				return null;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00014B30 File Offset: 0x00012D30
		public ICollection<string> EntryFileNames
		{
			get
			{
				return this._entries.Keys;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00014B3D File Offset: 0x00012D3D
		public ICollection<ZipEntry> Entries
		{
			get
			{
				return this._entries.Values;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00014B4C File Offset: 0x00012D4C
		public ICollection<ZipEntry> EntriesSorted
		{
			get
			{
				List<ZipEntry> list = new List<ZipEntry>();
				foreach (ZipEntry item in this.Entries)
				{
					list.Add(item);
				}
				StringComparison sc = this.CaseSensitiveRetrieval ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
				list.Sort((ZipEntry x, ZipEntry y) => string.Compare(x.FileName, y.FileName, sc));
				return list.AsReadOnly();
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00014BD0 File Offset: 0x00012DD0
		public int Count
		{
			get
			{
				return this._entries.Count;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00014BDD File Offset: 0x00012DDD
		public void RemoveEntry(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this._entries.Remove(SharedUtilities.NormalizePathForUseInZipFile(entry.FileName));
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00014C14 File Offset: 0x00012E14
		public void RemoveEntry(string fileName)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, null);
			ZipEntry zipEntry = this[fileName2];
			if (zipEntry == null)
			{
				throw new ArgumentException("The entry you specified was not found in the zip archive.");
			}
			this.RemoveEntry(zipEntry);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00014C46 File Offset: 0x00012E46
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00014C58 File Offset: 0x00012E58
		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (!this._disposed)
			{
				if (disposeManagedResources)
				{
					if (this._ReadStreamIsOurs && this._readstream != null)
					{
						this._readstream.Dispose();
						this._readstream = null;
					}
					if (this._temporaryFileName != null && this._name != null && this._writestream != null)
					{
						this._writestream.Dispose();
						this._writestream = null;
					}
					if (this.ParallelDeflater != null)
					{
						this.ParallelDeflater.Dispose();
						this.ParallelDeflater = null;
					}
				}
				this._disposed = true;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00014CE0 File Offset: 0x00012EE0
		internal Stream ReadStream
		{
			get
			{
				if (this._readstream == null && (this._readName != null || this._name != null))
				{
					this._readstream = File.Open(this._readName ?? this._name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					this._ReadStreamIsOurs = true;
				}
				return this._readstream;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00014D30 File Offset: 0x00012F30
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00014DAD File Offset: 0x00012FAD
		private Stream WriteStream
		{
			get
			{
				if (this._writestream != null)
				{
					return this._writestream;
				}
				if (this._name == null)
				{
					return this._writestream;
				}
				if (this._maxOutputSegmentSize != 0)
				{
					this._writestream = ZipSegmentedStream.ForWriting(this._name, this._maxOutputSegmentSize);
					return this._writestream;
				}
				SharedUtilities.CreateAndOpenUniqueTempFile(this.TempFileFolder ?? Path.GetDirectoryName(this._name), out this._writestream, out this._temporaryFileName);
				return this._writestream;
			}
			set
			{
				if (value != null)
				{
					throw new ZipException("Cannot set the stream to a non-null value.");
				}
				this._writestream = null;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00014DC4 File Offset: 0x00012FC4
		private string ArchiveNameForEvent
		{
			get
			{
				if (this._name == null)
				{
					return "(stream)";
				}
				return this._name;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000341 RID: 833 RVA: 0x00014DDC File Offset: 0x00012FDC
		// (remove) Token: 0x06000342 RID: 834 RVA: 0x00014E14 File Offset: 0x00013014
		public event EventHandler<SaveProgressEventArgs> SaveProgress;

		// Token: 0x06000343 RID: 835 RVA: 0x00014E4C File Offset: 0x0001304C
		internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00014E90 File Offset: 0x00013090
		private void OnSaveEntry(int current, ZipEntry entry, bool before)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, entry);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00014ED8 File Offset: 0x000130D8
		private void OnSaveEvent(ZipProgressEventType eventFlavor)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, eventFlavor);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00014F14 File Offset: 0x00013114
		private void OnSaveStarted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.Started(this.ArchiveNameForEvent);
				saveProgress(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00014F50 File Offset: 0x00013150
		private void OnSaveCompleted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs e = SaveProgressEventArgs.Completed(this.ArchiveNameForEvent);
				saveProgress(this, e);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000348 RID: 840 RVA: 0x00014F7C File Offset: 0x0001317C
		// (remove) Token: 0x06000349 RID: 841 RVA: 0x00014FB4 File Offset: 0x000131B4
		public event EventHandler<ReadProgressEventArgs> ReadProgress;

		// Token: 0x0600034A RID: 842 RVA: 0x00014FEC File Offset: 0x000131EC
		private void OnReadStarted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Started(this.ArchiveNameForEvent);
				readProgress(this, e);
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00015018 File Offset: 0x00013218
		private void OnReadCompleted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Completed(this.ArchiveNameForEvent);
				readProgress(this, e);
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00015044 File Offset: 0x00013244
		internal void OnReadBytes(ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, this.ReadStream.Position, this.LengthOfReadStream);
				readProgress(this, e);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00015084 File Offset: 0x00013284
		internal void OnReadEntry(bool before, ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs e = before ? ReadProgressEventArgs.Before(this.ArchiveNameForEvent, this._entries.Count) : ReadProgressEventArgs.After(this.ArchiveNameForEvent, entry, this._entries.Count);
				readProgress(this, e);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600034E RID: 846 RVA: 0x000150D6 File Offset: 0x000132D6
		private long LengthOfReadStream
		{
			get
			{
				if (this._lengthOfReadStream == -99L)
				{
					this._lengthOfReadStream = (this._ReadStreamIsOurs ? SharedUtilities.GetFileLength(this._name) : -1L);
				}
				return this._lengthOfReadStream;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600034F RID: 847 RVA: 0x00015108 File Offset: 0x00013308
		// (remove) Token: 0x06000350 RID: 848 RVA: 0x00015140 File Offset: 0x00013340
		public event EventHandler<ExtractProgressEventArgs> ExtractProgress;

		// Token: 0x06000351 RID: 849 RVA: 0x00015178 File Offset: 0x00013378
		private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = new ExtractProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, currentEntry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000151C4 File Offset: 0x000133C4
		internal bool OnExtractBlock(ZipEntry entry, long bytesWritten, long totalBytesToWrite)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00015208 File Offset: 0x00013408
		internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = before ? ExtractProgressEventArgs.BeforeExtractEntry(this.ArchiveNameForEvent, entry, path) : ExtractProgressEventArgs.AfterExtractEntry(this.ArchiveNameForEvent, entry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0001525C File Offset: 0x0001345C
		internal bool OnExtractExisting(ZipEntry entry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractExisting(this.ArchiveNameForEvent, entry, path);
				extractProgress(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000152A0 File Offset: 0x000134A0
		private void OnExtractAllCompleted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllCompleted(this.ArchiveNameForEvent, path);
				extractProgress(this, e);
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000152CC File Offset: 0x000134CC
		private void OnExtractAllStarted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllStarted(this.ArchiveNameForEvent, path);
				extractProgress(this, e);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000357 RID: 855 RVA: 0x000152F8 File Offset: 0x000134F8
		// (remove) Token: 0x06000358 RID: 856 RVA: 0x00015330 File Offset: 0x00013530
		public event EventHandler<AddProgressEventArgs> AddProgress;

		// Token: 0x06000359 RID: 857 RVA: 0x00015368 File Offset: 0x00013568
		private void OnAddStarted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.Started(this.ArchiveNameForEvent);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000153A4 File Offset: 0x000135A4
		private void OnAddCompleted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs e = AddProgressEventArgs.Completed(this.ArchiveNameForEvent);
				addProgress(this, e);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000153D0 File Offset: 0x000135D0
		internal void AfterAddEntry(ZipEntry entry)
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.AfterEntry(this.ArchiveNameForEvent, entry, this._entries.Count);
				addProgress(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600035C RID: 860 RVA: 0x00015418 File Offset: 0x00013618
		// (remove) Token: 0x0600035D RID: 861 RVA: 0x00015450 File Offset: 0x00013650
		public event EventHandler<ZipErrorEventArgs> ZipError;

		// Token: 0x0600035E RID: 862 RVA: 0x00015488 File Offset: 0x00013688
		internal bool OnZipErrorSaving(ZipEntry entry, Exception exc)
		{
			if (this.ZipError != null)
			{
				object @lock = this.LOCK;
				lock (@lock)
				{
					ZipErrorEventArgs zipErrorEventArgs = ZipErrorEventArgs.Saving(this.Name, entry, exc);
					this.ZipError(this, zipErrorEventArgs);
					if (zipErrorEventArgs.Cancel)
					{
						this._saveOperationCanceled = true;
					}
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000154FC File Offset: 0x000136FC
		public void ExtractAll(string path)
		{
			this._InternalExtractAll(path, true);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00015506 File Offset: 0x00013706
		public void ExtractAll(string path, ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this._InternalExtractAll(path, true);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00015518 File Offset: 0x00013718
		private void _InternalExtractAll(string path, bool overrideExtractExistingProperty)
		{
			bool flag = this.Verbose;
			this._inExtractAll = true;
			try
			{
				this.OnExtractAllStarted(path);
				int num = 0;
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					if (flag)
					{
						this.StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", new object[]
						{
							"Name",
							"Modified",
							"Size",
							"Ratio",
							"Packed"
						});
						this.StatusMessageTextWriter.WriteLine(new string('-', 72));
						flag = false;
					}
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", new object[]
						{
							zipEntry.FileName,
							zipEntry.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
							zipEntry.UncompressedSize,
							zipEntry.CompressionRatio,
							zipEntry.CompressedSize
						});
						if (!string.IsNullOrEmpty(zipEntry.Comment))
						{
							this.StatusMessageTextWriter.WriteLine("  Comment: {0}", zipEntry.Comment);
						}
					}
					zipEntry.Password = this._Password;
					this.OnExtractEntry(num, true, zipEntry, path);
					if (overrideExtractExistingProperty)
					{
						zipEntry.ExtractExistingFile = this.ExtractExistingFile;
					}
					zipEntry.Extract(path);
					num++;
					this.OnExtractEntry(num, false, zipEntry, path);
					if (this._extractOperationCanceled)
					{
						break;
					}
				}
				if (!this._extractOperationCanceled)
				{
					foreach (ZipEntry zipEntry2 in this._entries.Values)
					{
						if (zipEntry2.IsDirectory || zipEntry2.FileName.EndsWith("/"))
						{
							string fileOrDirectory = zipEntry2.FileName.StartsWith("/") ? Path.Combine(path, zipEntry2.FileName.Substring(1)) : Path.Combine(path, zipEntry2.FileName);
							zipEntry2._SetTimes(fileOrDirectory, false);
						}
					}
					this.OnExtractAllCompleted(path);
				}
			}
			finally
			{
				this._inExtractAll = false;
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001579C File Offset: 0x0001399C
		public static ZipFile Read(string fileName)
		{
			return ZipFile.Read(fileName, null, null, null);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000157A7 File Offset: 0x000139A7
		public static ZipFile Read(string fileName, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(fileName, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000157D0 File Offset: 0x000139D0
		private static ZipFile Read(string fileName, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			ZipFile zipFile = new ZipFile();
			zipFile.AlternateEncoding = (encoding ?? ZipFile.DefaultEncoding);
			zipFile.AlternateEncodingUsage = ZipOption.Always;
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._name = fileName;
			if (readProgress != null)
			{
				zipFile.ReadProgress = readProgress;
			}
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from {0}...", fileName);
			}
			ZipFile.ReadIntoInstance(zipFile);
			zipFile._fileAlreadyExists = true;
			return zipFile;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00015839 File Offset: 0x00013A39
		public static ZipFile Read(Stream zipStream)
		{
			return ZipFile.Read(zipStream, null, null, null);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00015844 File Offset: 0x00013A44
		public static ZipFile Read(Stream zipStream, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(zipStream, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0001586C File Offset: 0x00013A6C
		private static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			if (zipStream == null)
			{
				throw new ArgumentNullException("zipStream");
			}
			ZipFile zipFile = new ZipFile();
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._alternateEncoding = (encoding ?? ZipFile.DefaultEncoding);
			zipFile._alternateEncodingUsage = ZipOption.Always;
			if (readProgress != null)
			{
				zipFile.ReadProgress += readProgress;
			}
			zipFile._readstream = ((zipStream.Position == 0L) ? zipStream : new OffsetStream(zipStream));
			zipFile._ReadStreamIsOurs = false;
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from stream...");
			}
			ZipFile.ReadIntoInstance(zipFile);
			return zipFile;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000158F4 File Offset: 0x00013AF4
		private static void ReadIntoInstance(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			try
			{
				zf._readName = zf._name;
				if (!readStream.CanSeek)
				{
					ZipFile.ReadIntoInstance_Orig(zf);
					return;
				}
				zf.OnReadStarted();
				if (ZipFile.ReadFirstFourBytes(readStream) == 101010256U)
				{
					return;
				}
				int num = 0;
				bool flag = false;
				long num2 = readStream.Length - 64L;
				long num3 = Math.Max(readStream.Length - 16384L, 10L);
				do
				{
					if (num2 < 0L)
					{
						num2 = 0L;
					}
					readStream.Seek(num2, SeekOrigin.Begin);
					if (SharedUtilities.FindSignature(readStream, 101010256) != -1L)
					{
						flag = true;
					}
					else
					{
						if (num2 == 0L)
						{
							break;
						}
						num++;
						num2 -= (long)(32 * (num + 1) * num);
					}
				}
				while (!flag && num2 > num3);
				if (flag)
				{
					zf._locEndOfCDS = readStream.Position - 4L;
					byte[] array = new byte[16];
					readStream.Read(array, 0, array.Length);
					zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
					if (zf._diskNumberWithCd == 65535U)
					{
						throw new ZipException("Spanned archives with more than 65534 segments are not supported at this time.");
					}
					zf._diskNumberWithCd += 1U;
					int startIndex = 12;
					uint num4 = BitConverter.ToUInt32(array, startIndex);
					if (num4 == 4294967295U)
					{
						ZipFile.Zip64SeekToCentralDirectory(zf);
					}
					else
					{
						zf._OffsetOfCentralDirectory = num4;
						readStream.Seek((long)((ulong)num4), SeekOrigin.Begin);
					}
					ZipFile.ReadCentralDirectory(zf);
				}
				else
				{
					readStream.Seek(0L, SeekOrigin.Begin);
					ZipFile.ReadIntoInstance_Orig(zf);
				}
			}
			catch (Exception innerException)
			{
				if (zf._ReadStreamIsOurs && zf._readstream != null)
				{
					zf._readstream.Dispose();
					zf._readstream = null;
				}
				throw new ZipException("Cannot read that as a ZipFile", innerException);
			}
			zf._contentsChanged = false;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00015AA4 File Offset: 0x00013CA4
		private static void Zip64SeekToCentralDirectory(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			byte[] array = new byte[16];
			readStream.Seek(-40L, SeekOrigin.Current);
			readStream.Read(array, 0, 16);
			long num = BitConverter.ToInt64(array, 8);
			zf._OffsetOfCentralDirectory = uint.MaxValue;
			zf._OffsetOfCentralDirectory64 = num;
			readStream.Seek(num, SeekOrigin.Begin);
			uint num2 = (uint)SharedUtilities.ReadInt(readStream);
			if (num2 != 101075792U)
			{
				throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) looking for ZIP64 EoCD Record at position 0x{1:X8}", num2, readStream.Position));
			}
			readStream.Read(array, 0, 8);
			array = new byte[BitConverter.ToInt64(array, 0)];
			readStream.Read(array, 0, array.Length);
			num = BitConverter.ToInt64(array, 36);
			readStream.Seek(num, SeekOrigin.Begin);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00015B5B File Offset: 0x00013D5B
		private static uint ReadFirstFourBytes(Stream s)
		{
			return (uint)SharedUtilities.ReadInt(s);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00015B64 File Offset: 0x00013D64
		private static void ReadCentralDirectory(ZipFile zf)
		{
			bool flag = false;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
			{
				zipEntry.ResetDirEntry();
				zf.OnReadEntry(true, null);
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("entry {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				if (zipEntry._InputUsesZip64)
				{
					flag = true;
				}
				dictionary.Add(zipEntry.FileName, null);
			}
			if (flag)
			{
				zf.UseZip64WhenSaving = Zip64Option.Always;
			}
			if (zf._locEndOfCDS > 0L)
			{
				zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
			}
			ZipFile.ReadCentralDirectoryFooter(zf);
			if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
			{
				zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
			}
			if (zf.Verbose)
			{
				zf.StatusMessageTextWriter.WriteLine("read in {0} entries.", zf._entries.Count);
			}
			zf.OnReadCompleted();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00015C64 File Offset: 0x00013E64
		private static void ReadIntoInstance_Orig(ZipFile zf)
		{
			zf.OnReadStarted();
			zf._entries = new Dictionary<string, ZipEntry>();
			if (zf.Verbose)
			{
				if (zf.Name == null)
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
				}
				else
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", zf.Name);
				}
			}
			bool first = true;
			ZipContainer zc = new ZipContainer(zf);
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadEntry(zc, first)) != null)
			{
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("  {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				first = false;
			}
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				ZipEntry zipEntry2;
				while ((zipEntry2 = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
				{
					ZipEntry zipEntry3 = zf._entries[zipEntry2.FileName];
					if (zipEntry3 != null)
					{
						zipEntry3._Comment = zipEntry2.Comment;
						if (zipEntry2.IsDirectory)
						{
							zipEntry3.MarkAsDirectory();
						}
					}
					dictionary.Add(zipEntry2.FileName, null);
				}
				if (zf._locEndOfCDS > 0L)
				{
					zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
				}
				ZipFile.ReadCentralDirectoryFooter(zf);
				if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
				{
					zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
				}
			}
			catch (ZipException)
			{
			}
			catch (IOException)
			{
			}
			zf.OnReadCompleted();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00015DD4 File Offset: 0x00013FD4
		private static void ReadCentralDirectoryFooter(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			int num = SharedUtilities.ReadSignature(readStream);
			int num2 = 0;
			byte[] array;
			if ((long)num == 101075792L)
			{
				array = new byte[52];
				readStream.Read(array, 0, array.Length);
				long num3 = BitConverter.ToInt64(array, 0);
				if (num3 < 44L)
				{
					throw new ZipException("Bad size in the ZIP64 Central Directory.");
				}
				zf._versionMadeBy = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._versionNeededToExtract = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._diskNumberWithCd = BitConverter.ToUInt32(array, num2);
				num2 += 2;
				array = new byte[num3 - 44L];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
				if ((long)num != 117853008L)
				{
					throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
				}
				array = new byte[16];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
			}
			if ((long)num != 101010256L)
			{
				readStream.Seek(-4L, SeekOrigin.Current);
				throw new BadReadException(string.Format("Bad signature ({0:X8}) at position 0x{1:X8}", num, readStream.Position));
			}
			array = new byte[16];
			zf.ReadStream.Read(array, 0, array.Length);
			if (zf._diskNumberWithCd == 0U)
			{
				zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
			}
			ZipFile.ReadZipFileComment(zf);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00015F18 File Offset: 0x00014118
		private static void ReadZipFileComment(ZipFile zf)
		{
			byte[] array = new byte[2];
			zf.ReadStream.Read(array, 0, array.Length);
			short num = (short)((int)array[0] + (int)array[1] * 256);
			if (num > 0)
			{
				array = new byte[(int)num];
				zf.ReadStream.Read(array, 0, array.Length);
				string @string = zf.AlternateEncoding.GetString(array, 0, array.Length);
				zf.Comment = @string;
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00015F80 File Offset: 0x00014180
		public static bool IsZipFile(string fileName)
		{
			return ZipFile.IsZipFile(fileName, false);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00015F8C File Offset: 0x0001418C
		public static bool IsZipFile(string fileName, bool testExtract)
		{
			bool result = false;
			try
			{
				if (!File.Exists(fileName))
				{
					return false;
				}
				using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					result = ZipFile.IsZipFile(fileStream, testExtract);
				}
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return result;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00015FF8 File Offset: 0x000141F8
		public static bool IsZipFile(Stream stream, bool testExtract)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			bool result = false;
			try
			{
				if (!stream.CanRead)
				{
					return false;
				}
				Stream @null = Stream.Null;
				using (ZipFile zipFile = ZipFile.Read(stream, null, null, null))
				{
					if (testExtract)
					{
						foreach (ZipEntry zipEntry in zipFile)
						{
							if (!zipEntry.IsDirectory)
							{
								zipEntry.Extract(@null);
							}
						}
					}
				}
				result = true;
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return result;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x000160BC File Offset: 0x000142BC
		private void DeleteFileWithRetry(string filename)
		{
			bool flag = false;
			int num = 3;
			int num2 = 0;
			while (num2 < num && !flag)
			{
				try
				{
					File.Delete(filename);
					flag = true;
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("************************************************** Retry delete.");
					Thread.Sleep(200 + num2 * 200);
				}
				num2++;
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00016118 File Offset: 0x00014318
		public void Save()
		{
			try
			{
				bool flag = false;
				this._saveOperationCanceled = false;
				this._numberOfSegmentsForMostRecentSave = 0U;
				this.OnSaveStarted();
				if (this.WriteStream == null)
				{
					throw new BadStateException("You haven't specified where to save the zip.");
				}
				if (this._name != null && this._name.EndsWith(".exe") && !this._SavingSfx)
				{
					throw new BadStateException("You specified an EXE for a plain zip file.");
				}
				if (!this._contentsChanged)
				{
					this.OnSaveCompleted();
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("No save is necessary....");
					}
					return;
				}
				this.Reset(true);
				if (this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("saving....");
				}
				if (this._entries.Count >= 65535 && this._zip64 == Zip64Option.Default)
				{
					throw new ZipException("The number of entries is 65535 or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
				}
				int num = 0;
				ICollection<ZipEntry> collection = this.SortEntriesBeforeSaving ? this.EntriesSorted : this.Entries;
				foreach (ZipEntry zipEntry in collection)
				{
					this.OnSaveEntry(num, zipEntry, true);
					zipEntry.Write(this.WriteStream);
					if (this._saveOperationCanceled)
					{
						break;
					}
					num++;
					this.OnSaveEntry(num, zipEntry, false);
					if (this._saveOperationCanceled)
					{
						break;
					}
					if (zipEntry.IncludedInMostRecentSave)
					{
						flag |= zipEntry.OutputUsedZip64.Value;
					}
				}
				if (this._saveOperationCanceled)
				{
					return;
				}
				ZipSegmentedStream zipSegmentedStream = this.WriteStream as ZipSegmentedStream;
				this._numberOfSegmentsForMostRecentSave = ((zipSegmentedStream != null) ? zipSegmentedStream.CurrentSegment : 1U);
				bool flag2 = ZipOutput.WriteCentralDirectoryStructure(this.WriteStream, collection, this._numberOfSegmentsForMostRecentSave, this._zip64, this.Comment, new ZipContainer(this));
				this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
				this._hasBeenSaved = true;
				this._contentsChanged = false;
				flag = (flag || flag2);
				this._OutputUsesZip64 = new bool?(flag);
				if (this._name != null && (this._temporaryFileName != null || zipSegmentedStream != null))
				{
					this.WriteStream.Dispose();
					if (this._saveOperationCanceled)
					{
						return;
					}
					if (this._fileAlreadyExists && this._readstream != null)
					{
						this._readstream.Close();
						this._readstream = null;
						foreach (ZipEntry zipEntry2 in collection)
						{
							ZipSegmentedStream zipSegmentedStream2 = zipEntry2._archiveStream as ZipSegmentedStream;
							if (zipSegmentedStream2 != null)
							{
								zipSegmentedStream2.Dispose();
							}
							zipEntry2._archiveStream = null;
						}
					}
					string text = null;
					if (File.Exists(this._name))
					{
						text = this._name + "." + Path.GetRandomFileName();
						if (File.Exists(text))
						{
							this.DeleteFileWithRetry(text);
						}
						File.Move(this._name, text);
					}
					this.OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
					File.Move((zipSegmentedStream != null) ? zipSegmentedStream.CurrentTempName : this._temporaryFileName, this._name);
					this.OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
					if (text != null)
					{
						try
						{
							if (File.Exists(text))
							{
								File.Delete(text);
							}
						}
						catch
						{
						}
					}
					this._fileAlreadyExists = true;
				}
				ZipFile.NotifyEntriesSaveComplete(collection);
				this.OnSaveCompleted();
				this._JustSaved = true;
			}
			finally
			{
				this.CleanupAfterSaveOperation();
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000164A8 File Offset: 0x000146A8
		private static void NotifyEntriesSaveComplete(ICollection<ZipEntry> c)
		{
			foreach (ZipEntry zipEntry in c)
			{
				zipEntry.NotifySaveComplete();
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000164F0 File Offset: 0x000146F0
		private void RemoveTempFile()
		{
			try
			{
				if (File.Exists(this._temporaryFileName))
				{
					File.Delete(this._temporaryFileName);
				}
			}
			catch (IOException ex)
			{
				if (this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", ex.Message);
				}
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00016548 File Offset: 0x00014748
		private void CleanupAfterSaveOperation()
		{
			if (this._name != null)
			{
				if (this._writestream != null)
				{
					try
					{
						this._writestream.Dispose();
					}
					catch (IOException)
					{
					}
				}
				this._writestream = null;
				if (this._temporaryFileName != null)
				{
					this.RemoveTempFile();
					this._temporaryFileName = null;
				}
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000165A4 File Offset: 0x000147A4
		public void Save(string fileName)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			else
			{
				this._readName = this._name;
			}
			this._name = fileName;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "fileName"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this.Save();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0001661C File Offset: 0x0001481C
		public void Save(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (!outputStream.CanWrite)
			{
				throw new ArgumentException("Must be a writable stream.", "outputStream");
			}
			this._name = null;
			this._writestream = new CountingStream(outputStream);
			this._contentsChanged = true;
			this._fileAlreadyExists = false;
			this.Save();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00016678 File Offset: 0x00014878
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorFlavor flavor)
		{
			this.SaveSelfExtractor(exeToGenerate, new SelfExtractorSaveOptions
			{
				Flavor = flavor
			});
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001669C File Offset: 0x0001489C
		public void SaveSelfExtractor(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			this._SavingSfx = true;
			this._name = exeToGenerate;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "exeToGenerate"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this._SaveSfxStub(exeToGenerate, options);
			this.Save();
			this._SavingSfx = false;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001671C File Offset: 0x0001491C
		private static void ExtractResourceToFile(Assembly a, string resourceName, string filename)
		{
			byte[] array = new byte[1024];
			using (Stream manifestResourceStream = a.GetManifestResourceStream(resourceName))
			{
				if (manifestResourceStream == null)
				{
					throw new ZipException(string.Format("missing resource '{0}'", resourceName));
				}
				using (FileStream fileStream = File.OpenWrite(filename))
				{
					int num;
					do
					{
						num = manifestResourceStream.Read(array, 0, array.Length);
						fileStream.Write(array, 0, num);
					}
					while (num > 0);
				}
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000167A4 File Offset: 0x000149A4
		private void _SaveSfxStub(string exeToGenerate, SelfExtractorSaveOptions options)
		{
			string text = null;
			string text2 = null;
			string dir = null;
			try
			{
				if (File.Exists(exeToGenerate) && this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("The existing file ({0}) will be overwritten.", exeToGenerate);
				}
				if (!exeToGenerate.EndsWith(".exe") && this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("Warning: The generated self-extracting file will not have an .exe extension.");
				}
				dir = (this.TempFileFolder ?? Path.GetDirectoryName(exeToGenerate));
				text = ZipFile.GenerateTempPathname(dir, "exe");
				Assembly assembly = typeof(ZipFile).Assembly;
				using (CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider(new Dictionary<string, string>
				{
					{
						"CompilerVersion",
						"v2.0"
					}
				}))
				{
					ZipFile.ExtractorSettings extractorSettings = null;
					foreach (ZipFile.ExtractorSettings extractorSettings2 in ZipFile.SettingsList)
					{
						if (extractorSettings2.Flavor == options.Flavor)
						{
							extractorSettings = extractorSettings2;
							break;
						}
					}
					if (extractorSettings == null)
					{
						throw new BadStateException(string.Format("While saving a Self-Extracting Zip, Cannot find that flavor ({0})?", options.Flavor));
					}
					CompilerParameters compilerParameters = new CompilerParameters();
					compilerParameters.ReferencedAssemblies.Add(assembly.Location);
					if (extractorSettings.ReferencedAssemblies != null)
					{
						foreach (string value in extractorSettings.ReferencedAssemblies)
						{
							compilerParameters.ReferencedAssemblies.Add(value);
						}
					}
					compilerParameters.GenerateInMemory = false;
					compilerParameters.GenerateExecutable = true;
					compilerParameters.IncludeDebugInformation = false;
					compilerParameters.CompilerOptions = "";
					Assembly executingAssembly = Assembly.GetExecutingAssembly();
					StringBuilder stringBuilder = new StringBuilder();
					string text3 = ZipFile.GenerateTempPathname(dir, "cs");
					using (ZipFile zipFile = ZipFile.Read(executingAssembly.GetManifestResourceStream("Ionic.Zip.Resources.ZippedResources.zip")))
					{
						text2 = ZipFile.GenerateTempPathname(dir, "tmp");
						if (string.IsNullOrEmpty(options.IconFile))
						{
							Directory.CreateDirectory(text2);
							ZipEntry zipEntry = zipFile["zippedFile.ico"];
							if ((zipEntry.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
							{
								zipEntry.Attributes ^= FileAttributes.ReadOnly;
							}
							zipEntry.Extract(text2);
							string arg = Path.Combine(text2, "zippedFile.ico");
							CompilerParameters compilerParameters2 = compilerParameters;
							compilerParameters2.CompilerOptions += string.Format("/win32icon:\"{0}\"", arg);
						}
						else
						{
							CompilerParameters compilerParameters3 = compilerParameters;
							compilerParameters3.CompilerOptions += string.Format("/win32icon:\"{0}\"", options.IconFile);
						}
						compilerParameters.OutputAssembly = text;
						if (options.Flavor == SelfExtractorFlavor.WinFormsApplication)
						{
							CompilerParameters compilerParameters4 = compilerParameters;
							compilerParameters4.CompilerOptions += " /target:winexe";
						}
						if (!string.IsNullOrEmpty(options.AdditionalCompilerSwitches))
						{
							CompilerParameters compilerParameters5 = compilerParameters;
							compilerParameters5.CompilerOptions = compilerParameters5.CompilerOptions + " " + options.AdditionalCompilerSwitches;
						}
						if (string.IsNullOrEmpty(compilerParameters.CompilerOptions))
						{
							compilerParameters.CompilerOptions = null;
						}
						if (extractorSettings.CopyThroughResources != null && extractorSettings.CopyThroughResources.Count != 0)
						{
							if (!Directory.Exists(text2))
							{
								Directory.CreateDirectory(text2);
							}
							foreach (string text4 in extractorSettings.CopyThroughResources)
							{
								string text5 = Path.Combine(text2, text4);
								ZipFile.ExtractResourceToFile(executingAssembly, text4, text5);
								compilerParameters.EmbeddedResources.Add(text5);
							}
						}
						compilerParameters.EmbeddedResources.Add(assembly.Location);
						stringBuilder.Append("// " + Path.GetFileName(text3) + "\n").Append("// --------------------------------------------\n//\n").Append("// This SFX source file was generated by DotNetZip ").Append(ZipFile.LibraryVersion.ToString()).Append("\n//         at ").Append(DateTime.Now.ToString("yyyy MMMM dd  HH:mm:ss")).Append("\n//\n// --------------------------------------------\n\n\n");
						if (!string.IsNullOrEmpty(options.Description))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"" + options.Description.Replace("\"", "") + "\")]\n");
						}
						else
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"DotNetZip SFX Archive\")]\n");
						}
						if (!string.IsNullOrEmpty(options.ProductVersion))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyInformationalVersion(\"" + options.ProductVersion.Replace("\"", "") + "\")]\n");
						}
						string str = string.IsNullOrEmpty(options.Copyright) ? "Extractor: Copyright ?Dino Chiesa 2008-2011" : options.Copyright.Replace("\"", "");
						if (!string.IsNullOrEmpty(options.ProductName))
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"").Append(options.ProductName.Replace("\"", "")).Append("\")]\n");
						}
						else
						{
							stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"DotNetZip\")]\n");
						}
						stringBuilder.Append("[assembly: System.Reflection.AssemblyCopyright(\"" + str + "\")]\n").Append(string.Format("[assembly: System.Reflection.AssemblyVersion(\"{0}\")]\n", ZipFile.LibraryVersion.ToString()));
						if (options.FileVersion != null)
						{
							stringBuilder.Append(string.Format("[assembly: System.Reflection.AssemblyFileVersion(\"{0}\")]\n", options.FileVersion.ToString()));
						}
						stringBuilder.Append("\n\n\n");
						string text6 = options.DefaultExtractDirectory;
						if (text6 != null)
						{
							text6 = text6.Replace("\"", "").Replace("\\", "\\\\");
						}
						string text7 = options.PostExtractCommandLine;
						if (text7 != null)
						{
							text7 = text7.Replace("\\", "\\\\");
							text7 = text7.Replace("\"", "\\\"");
						}
						foreach (string text8 in extractorSettings.ResourcesToCompile)
						{
							using (Stream stream = zipFile[text8].OpenReader())
							{
								if (stream == null)
								{
									throw new ZipException(string.Format("missing resource '{0}'", text8));
								}
								using (StreamReader streamReader = new StreamReader(stream))
								{
									while (streamReader.Peek() >= 0)
									{
										string text9 = streamReader.ReadLine();
										if (text6 != null)
										{
											text9 = text9.Replace("@@EXTRACTLOCATION", text6);
										}
										text9 = text9.Replace("@@REMOVE_AFTER_EXECUTE", options.RemoveUnpackedFilesAfterExecute.ToString());
										text9 = text9.Replace("@@QUIET", options.Quiet.ToString());
										if (!string.IsNullOrEmpty(options.SfxExeWindowTitle))
										{
											text9 = text9.Replace("@@SFX_EXE_WINDOW_TITLE", options.SfxExeWindowTitle);
										}
										text9 = text9.Replace("@@EXTRACT_EXISTING_FILE", ((int)options.ExtractExistingFile).ToString());
										if (text7 != null)
										{
											text9 = text9.Replace("@@POST_UNPACK_CMD_LINE", text7);
										}
										stringBuilder.Append(text9).Append("\n");
									}
								}
								stringBuilder.Append("\n\n");
							}
						}
					}
					string text10 = stringBuilder.ToString();
					CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, new string[]
					{
						text10
					});
					if (compilerResults == null)
					{
						throw new SfxGenerationException("Cannot compile the extraction logic!");
					}
					if (this.Verbose)
					{
						foreach (string value2 in compilerResults.Output)
						{
							this.StatusMessageTextWriter.WriteLine(value2);
						}
					}
					if (compilerResults.Errors.Count != 0)
					{
						using (TextWriter textWriter = new StreamWriter(text3))
						{
							textWriter.Write(text10);
							textWriter.Write("\n\n\n// ------------------------------------------------------------------\n");
							textWriter.Write("// Errors during compilation: \n//\n");
							string fileName = Path.GetFileName(text3);
							foreach (object obj in compilerResults.Errors)
							{
								CompilerError compilerError = (CompilerError)obj;
								textWriter.Write(string.Format("//   {0}({1},{2}): {3} {4}: {5}\n//\n", new object[]
								{
									fileName,
									compilerError.Line,
									compilerError.Column,
									compilerError.IsWarning ? "Warning" : "error",
									compilerError.ErrorNumber,
									compilerError.ErrorText
								}));
							}
						}
						throw new SfxGenerationException(string.Format("Errors compiling the extraction logic!  {0}", text3));
					}
					this.OnSaveEvent(ZipProgressEventType.Saving_AfterCompileSelfExtractor);
					using (Stream stream2 = File.OpenRead(text))
					{
						byte[] array = new byte[4000];
						int num = 1;
						while (num != 0)
						{
							num = stream2.Read(array, 0, array.Length);
							if (num != 0)
							{
								this.WriteStream.Write(array, 0, num);
							}
						}
					}
				}
				this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
			}
			finally
			{
				try
				{
					if (Directory.Exists(text2))
					{
						try
						{
							Directory.Delete(text2, true);
						}
						catch (IOException arg2)
						{
							this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", arg2);
						}
					}
					if (File.Exists(text))
					{
						try
						{
							File.Delete(text);
						}
						catch (IOException arg3)
						{
							this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", arg3);
						}
					}
				}
				catch (IOException)
				{
				}
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00017238 File Offset: 0x00015438
		internal static string GenerateTempPathname(string dir, string extension)
		{
			string name = Assembly.GetExecutingAssembly().GetName().Name;
			string text2;
			do
			{
				string text = Guid.NewGuid().ToString();
				string path = string.Format("{0}-{1}-{2}.{3}", new object[]
				{
					name,
					DateTime.Now.ToString("yyyyMMMdd-HHmmss"),
					text,
					extension
				});
				text2 = Path.Combine(dir, path);
			}
			while (File.Exists(text2) || Directory.Exists(text2));
			return text2;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000172B8 File Offset: 0x000154B8
		public void AddSelectedFiles(string selectionCriteria)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, false);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000172C8 File Offset: 0x000154C8
		public void AddSelectedFiles(string selectionCriteria, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, recurseDirectories);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000172D8 File Offset: 0x000154D8
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, false);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000172E4 File Offset: 0x000154E4
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, recurseDirectories);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000172F0 File Offset: 0x000154F0
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, false);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000172FC File Offset: 0x000154FC
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, false);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001730A File Offset: 0x0001550A
		public void UpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, true);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00017318 File Offset: 0x00015518
		private string EnsureendInSlash(string s)
		{
			if (s.EndsWith("\\"))
			{
				return s;
			}
			return s + "\\";
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00017334 File Offset: 0x00015534
		private void _AddOrUpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories, bool wantUpdate)
		{
			if (directoryOnDisk == null && Directory.Exists(selectionCriteria))
			{
				directoryOnDisk = selectionCriteria;
				selectionCriteria = "*.*";
			}
			else if (string.IsNullOrEmpty(directoryOnDisk))
			{
				directoryOnDisk = ".";
			}
			while (directoryOnDisk.EndsWith("\\"))
			{
				directoryOnDisk = directoryOnDisk.Substring(0, directoryOnDisk.Length - 1);
			}
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding selection '{0}' from dir '{1}'...", selectionCriteria, directoryOnDisk);
			}
			ReadOnlyCollection<string> readOnlyCollection = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints).SelectFiles(directoryOnDisk, recurseDirectories);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("found {0} files...", readOnlyCollection.Count);
			}
			this.OnAddStarted();
			AddOrUpdateAction action = wantUpdate ? AddOrUpdateAction.AddOrUpdate : AddOrUpdateAction.AddOnly;
			foreach (string text in readOnlyCollection)
			{
				string text2 = (directoryPathInArchive == null) ? null : ZipFile.ReplaceLeadingDirectory(Path.GetDirectoryName(text), directoryOnDisk, directoryPathInArchive);
				if (File.Exists(text))
				{
					if (wantUpdate)
					{
						this.UpdateFile(text, text2);
					}
					else
					{
						this.AddFile(text, text2);
					}
				}
				else
				{
					this.AddOrUpdateDirectoryImpl(text, text2, action, false, 0);
				}
			}
			this.OnAddCompleted();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00017468 File Offset: 0x00015668
		private static string ReplaceLeadingDirectory(string original, string pattern, string replacement)
		{
			string text = original.ToUpper();
			string text2 = pattern.ToUpper();
			if (text.IndexOf(text2) != 0)
			{
				return original;
			}
			return replacement + original.Substring(text2.Length);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001749E File Offset: 0x0001569E
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria)
		{
			return new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints).SelectEntries(this);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000174B2 File Offset: 0x000156B2
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria, string directoryPathInArchive)
		{
			return new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints).SelectEntries(this, directoryPathInArchive);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000174C8 File Offset: 0x000156C8
		public int RemoveSelectedEntries(string selectionCriteria)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000174EC File Offset: 0x000156EC
		public int RemoveSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria, directoryPathInArchive);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00017510 File Offset: 0x00015710
		public void ExtractSelectedEntries(string selectionCriteria)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract();
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00017568 File Offset: 0x00015768
		public void ExtractSelectedEntries(string selectionCriteria, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractExistingFile);
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000175C0 File Offset: 0x000157C0
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract();
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00017618 File Offset: 0x00015818
		public void ExtractSelectedEntries(string selectionCriteria, string directoryInArchive, string extractDirectory)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractDirectory);
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00017674 File Offset: 0x00015874
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive, string extractDirectory, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractDirectory, extractExistingFile);
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x000176D0 File Offset: 0x000158D0
		public IEnumerator<ZipEntry> GetEnumerator()
		{
			foreach (ZipEntry zipEntry in this._entries.Values)
			{
				yield return zipEntry;
			}
			Dictionary<string, ZipEntry>.ValueCollection.Enumerator enumerator = default(Dictionary<string, ZipEntry>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000176DF File Offset: 0x000158DF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000176E7 File Offset: 0x000158E7
		[DispId(-4)]
		public IEnumerator GetNewEnum()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400022F RID: 559
		private TextWriter _StatusMessageTextWriter;

		// Token: 0x04000230 RID: 560
		private bool _CaseSensitiveRetrieval;

		// Token: 0x04000231 RID: 561
		private Stream _readstream;

		// Token: 0x04000232 RID: 562
		private Stream _writestream;

		// Token: 0x04000233 RID: 563
		private ushort _versionMadeBy;

		// Token: 0x04000234 RID: 564
		private ushort _versionNeededToExtract;

		// Token: 0x04000235 RID: 565
		private uint _diskNumberWithCd;

		// Token: 0x04000236 RID: 566
		private int _maxOutputSegmentSize;

		// Token: 0x04000237 RID: 567
		private uint _numberOfSegmentsForMostRecentSave;

		// Token: 0x04000238 RID: 568
		private ZipErrorAction _zipErrorAction;

		// Token: 0x04000239 RID: 569
		private bool _disposed;

		// Token: 0x0400023A RID: 570
		private Dictionary<string, ZipEntry> _entries;

		// Token: 0x0400023B RID: 571
		private List<ZipEntry> _zipEntriesAsList;

		// Token: 0x0400023C RID: 572
		private string _name;

		// Token: 0x0400023D RID: 573
		private string _readName;

		// Token: 0x0400023E RID: 574
		private string _Comment;

		// Token: 0x0400023F RID: 575
		internal string _Password;

		// Token: 0x04000240 RID: 576
		private bool _emitNtfsTimes = true;

		// Token: 0x04000241 RID: 577
		private bool _emitUnixTimes;

		// Token: 0x04000242 RID: 578
		private CompressionStrategy _Strategy;

		// Token: 0x04000243 RID: 579
		private CompressionMethod _compressionMethod = CompressionMethod.Deflate;

		// Token: 0x04000244 RID: 580
		private bool _fileAlreadyExists;

		// Token: 0x04000245 RID: 581
		private string _temporaryFileName;

		// Token: 0x04000246 RID: 582
		private bool _contentsChanged;

		// Token: 0x04000247 RID: 583
		private bool _hasBeenSaved;

		// Token: 0x04000248 RID: 584
		private string _TempFileFolder;

		// Token: 0x04000249 RID: 585
		private bool _ReadStreamIsOurs = true;

		// Token: 0x0400024A RID: 586
		private object LOCK = new object();

		// Token: 0x0400024B RID: 587
		private bool _saveOperationCanceled;

		// Token: 0x0400024C RID: 588
		private bool _extractOperationCanceled;

		// Token: 0x0400024D RID: 589
		private bool _addOperationCanceled;

		// Token: 0x0400024E RID: 590
		private EncryptionAlgorithm _Encryption;

		// Token: 0x0400024F RID: 591
		private bool _JustSaved;

		// Token: 0x04000250 RID: 592
		private long _locEndOfCDS = -1L;

		// Token: 0x04000251 RID: 593
		private uint _OffsetOfCentralDirectory;

		// Token: 0x04000252 RID: 594
		private long _OffsetOfCentralDirectory64;

		// Token: 0x04000253 RID: 595
		private bool? _OutputUsesZip64;

		// Token: 0x04000254 RID: 596
		internal bool _inExtractAll;

		// Token: 0x04000255 RID: 597
		private Encoding _alternateEncoding = Encoding.GetEncoding("UTF-8");

		// Token: 0x04000256 RID: 598
		private ZipOption _alternateEncodingUsage;

		// Token: 0x04000257 RID: 599
		private static Encoding _defaultEncoding = Encoding.GetEncoding("UTF-8");

		// Token: 0x04000258 RID: 600
		private int _BufferSize = ZipFile.BufferSizeDefault;

		// Token: 0x04000259 RID: 601
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x0400025A RID: 602
		private long _ParallelDeflateThreshold;

		// Token: 0x0400025B RID: 603
		private int _maxBufferPairs = 16;

		// Token: 0x0400025C RID: 604
		internal Zip64Option _zip64;

		// Token: 0x0400025D RID: 605
		private bool _SavingSfx;

		// Token: 0x0400025E RID: 606
		public static readonly int BufferSizeDefault = 32768;

		// Token: 0x04000261 RID: 609
		private long _lengthOfReadStream = -99L;

		// Token: 0x04000265 RID: 613
		private static ZipFile.ExtractorSettings[] SettingsList = new ZipFile.ExtractorSettings[]
		{
			new ZipFile.ExtractorSettings
			{
				Flavor = SelfExtractorFlavor.WinFormsApplication,
				ReferencedAssemblies = new List<string>
				{
					"System.dll",
					"System.Windows.Forms.dll",
					"System.Drawing.dll"
				},
				CopyThroughResources = new List<string>
				{
					"Ionic.Zip.WinFormsSelfExtractorStub.resources",
					"Ionic.Zip.Forms.PasswordDialog.resources",
					"Ionic.Zip.Forms.ZipContentsDialog.resources"
				},
				ResourcesToCompile = new List<string>
				{
					"WinFormsSelfExtractorStub.cs",
					"WinFormsSelfExtractorStub.Designer.cs",
					"PasswordDialog.cs",
					"PasswordDialog.Designer.cs",
					"ZipContentsDialog.cs",
					"ZipContentsDialog.Designer.cs",
					"FolderBrowserDialogEx.cs"
				}
			},
			new ZipFile.ExtractorSettings
			{
				Flavor = SelfExtractorFlavor.ConsoleApplication,
				ReferencedAssemblies = new List<string>
				{
					"System.dll"
				},
				CopyThroughResources = null,
				ResourcesToCompile = new List<string>
				{
					"CommandLineSelfExtractorStub.cs"
				}
			}
		};

		// Token: 0x02000064 RID: 100
		private class ExtractorSettings
		{
			// Token: 0x0400031F RID: 799
			public SelfExtractorFlavor Flavor;

			// Token: 0x04000320 RID: 800
			public List<string> ReferencedAssemblies;

			// Token: 0x04000321 RID: 801
			public List<string> CopyThroughResources;

			// Token: 0x04000322 RID: 802
			public List<string> ResourcesToCompile;
		}
	}
}
