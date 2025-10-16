using System;

namespace Ionic.Zip
{
	// Token: 0x02000032 RID: 50
	public class ZipProgressEventArgs : EventArgs
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000CD18 File Offset: 0x0000AF18
		internal ZipProgressEventArgs()
		{
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000CD20 File Offset: 0x0000AF20
		internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
		{
			this._archiveName = archiveName;
			this._flavor = flavor;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000CD36 File Offset: 0x0000AF36
		// (set) Token: 0x0600018D RID: 397 RVA: 0x0000CD3E File Offset: 0x0000AF3E
		public int EntriesTotal
		{
			get
			{
				return this._entriesTotal;
			}
			set
			{
				this._entriesTotal = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000CD47 File Offset: 0x0000AF47
		// (set) Token: 0x0600018F RID: 399 RVA: 0x0000CD4F File Offset: 0x0000AF4F
		public ZipEntry CurrentEntry
		{
			get
			{
				return this._latestEntry;
			}
			set
			{
				this._latestEntry = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000CD58 File Offset: 0x0000AF58
		// (set) Token: 0x06000191 RID: 401 RVA: 0x0000CD60 File Offset: 0x0000AF60
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = (this._cancel || value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000CD70 File Offset: 0x0000AF70
		// (set) Token: 0x06000193 RID: 403 RVA: 0x0000CD78 File Offset: 0x0000AF78
		public ZipProgressEventType EventType
		{
			get
			{
				return this._flavor;
			}
			set
			{
				this._flavor = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000CD81 File Offset: 0x0000AF81
		// (set) Token: 0x06000195 RID: 405 RVA: 0x0000CD89 File Offset: 0x0000AF89
		public string ArchiveName
		{
			get
			{
				return this._archiveName;
			}
			set
			{
				this._archiveName = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000CD92 File Offset: 0x0000AF92
		// (set) Token: 0x06000197 RID: 407 RVA: 0x0000CD9A File Offset: 0x0000AF9A
		public long BytesTransferred
		{
			get
			{
				return this._bytesTransferred;
			}
			set
			{
				this._bytesTransferred = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000CDA3 File Offset: 0x0000AFA3
		// (set) Token: 0x06000199 RID: 409 RVA: 0x0000CDAB File Offset: 0x0000AFAB
		public long TotalBytesToTransfer
		{
			get
			{
				return this._totalBytesToTransfer;
			}
			set
			{
				this._totalBytesToTransfer = value;
			}
		}

		// Token: 0x04000194 RID: 404
		private int _entriesTotal;

		// Token: 0x04000195 RID: 405
		private bool _cancel;

		// Token: 0x04000196 RID: 406
		private ZipEntry _latestEntry;

		// Token: 0x04000197 RID: 407
		private ZipProgressEventType _flavor;

		// Token: 0x04000198 RID: 408
		private string _archiveName;

		// Token: 0x04000199 RID: 409
		private long _bytesTransferred;

		// Token: 0x0400019A RID: 410
		private long _totalBytesToTransfer;
	}
}
