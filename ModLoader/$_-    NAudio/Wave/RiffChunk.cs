using System;
using System.Text;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x0200003F RID: 63
	internal class RiffChunk
	{
		// Token: 0x06000121 RID: 289 RVA: 0x0000B4A7 File Offset: 0x000096A7
		public RiffChunk(int identifier, int length, long streamPosition)
		{
			this.Identifier = identifier;
			this.Length = length;
			this.StreamPosition = streamPosition;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000B4C4 File Offset: 0x000096C4
		public int Identifier { get; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000B4CC File Offset: 0x000096CC
		public string IdentifierAsString
		{
			get
			{
				return Encoding.UTF8.GetString(BitConverter.GetBytes(this.Identifier));
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000B4E3 File Offset: 0x000096E3
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000B4EB File Offset: 0x000096EB
		public int Length { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000B4F4 File Offset: 0x000096F4
		// (set) Token: 0x06000127 RID: 295 RVA: 0x0000B4FC File Offset: 0x000096FC
		public long StreamPosition { get; private set; }
	}
}
