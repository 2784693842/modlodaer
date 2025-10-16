using System;

namespace ChatTreeLoader.Attr
{
	// Token: 0x0200001B RID: 27
	[AttributeUsage(AttributeTargets.Field)]
	public class NoteEn : Attribute
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00004200 File Offset: 0x00002400
		public NoteEn(string note)
		{
			this.note = note;
		}

		// Token: 0x0400003A RID: 58
		public string note;
	}
}
