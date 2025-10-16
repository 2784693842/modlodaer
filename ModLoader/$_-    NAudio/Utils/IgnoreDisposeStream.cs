using System;
using System.IO;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils
{
	// Token: 0x020000C8 RID: 200
	internal class IgnoreDisposeStream : Stream
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x000160AC File Offset: 0x000142AC
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x000160B4 File Offset: 0x000142B4
		public Stream SourceStream { get; private set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000160BD File Offset: 0x000142BD
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x000160C5 File Offset: 0x000142C5
		public bool IgnoreDispose { get; set; }

		// Token: 0x0600048D RID: 1165 RVA: 0x000160CE File Offset: 0x000142CE
		public IgnoreDisposeStream(Stream sourceStream)
		{
			this.SourceStream = sourceStream;
			this.IgnoreDispose = true;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x000160E4 File Offset: 0x000142E4
		public override bool CanRead
		{
			get
			{
				return this.SourceStream.CanRead;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x000160F1 File Offset: 0x000142F1
		public override bool CanSeek
		{
			get
			{
				return this.SourceStream.CanSeek;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x000160FE File Offset: 0x000142FE
		public override bool CanWrite
		{
			get
			{
				return this.SourceStream.CanWrite;
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001610B File Offset: 0x0001430B
		public override void Flush()
		{
			this.SourceStream.Flush();
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00016118 File Offset: 0x00014318
		public override long Length
		{
			get
			{
				return this.SourceStream.Length;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00016125 File Offset: 0x00014325
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x00016132 File Offset: 0x00014332
		public override long Position
		{
			get
			{
				return this.SourceStream.Position;
			}
			set
			{
				this.SourceStream.Position = value;
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00016140 File Offset: 0x00014340
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.SourceStream.Read(buffer, offset, count);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00016150 File Offset: 0x00014350
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.SourceStream.Seek(offset, origin);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001615F File Offset: 0x0001435F
		public override void SetLength(long value)
		{
			this.SourceStream.SetLength(value);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001616D File Offset: 0x0001436D
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.SourceStream.Write(buffer, offset, count);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001617D File Offset: 0x0001437D
		protected override void Dispose(bool disposing)
		{
			if (!this.IgnoreDispose)
			{
				this.SourceStream.Dispose();
				this.SourceStream = null;
			}
		}
	}
}
