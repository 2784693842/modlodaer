using System;

namespace ChatTreeLoader.Attr
{
	// Token: 0x0200001A RID: 26
	[AttributeUsage(AttributeTargets.Field)]
	public class Note : Attribute
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000041F1 File Offset: 0x000023F1
		public Note(string note)
		{
			this.note = note;
		}

		// Token: 0x04000039 RID: 57
		public string note;
	}
}
