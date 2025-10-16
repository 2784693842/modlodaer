using System;

namespace Ionic.Zip
{
	// Token: 0x02000037 RID: 55
	public class ZipErrorEventArgs : ZipProgressEventArgs
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x0000CFDB File Offset: 0x0000B1DB
		private ZipErrorEventArgs()
		{
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000CFE3 File Offset: 0x0000B1E3
		internal static ZipErrorEventArgs Saving(string archiveName, ZipEntry entry, Exception exception)
		{
			return new ZipErrorEventArgs
			{
				EventType = ZipProgressEventType.Error_Saving,
				ArchiveName = archiveName,
				CurrentEntry = entry,
				_exc = exception
			};
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000D007 File Offset: 0x0000B207
		public Exception Exception
		{
			get
			{
				return this._exc;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000D00F File Offset: 0x0000B20F
		public string FileName
		{
			get
			{
				return base.CurrentEntry.LocalFileName;
			}
		}

		// Token: 0x0400019E RID: 414
		private Exception _exc;
	}
}
