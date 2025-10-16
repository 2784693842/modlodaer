using System;
using System.IO;

namespace ModLoader
{
	// Token: 0x02000003 RID: 3
	public static class EmbeddedResources
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000216A File Offset: 0x0000036A
		public static Stream CSTIFonts
		{
			get
			{
				return typeof(ModLoader).Assembly.GetManifestResourceStream("csti_fonts.bundle");
			}
		}
	}
}
