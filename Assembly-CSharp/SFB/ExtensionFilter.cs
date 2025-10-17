using System;

namespace SFB
{
	// Token: 0x020001EC RID: 492
	public struct ExtensionFilter
	{
		// Token: 0x06000CEC RID: 3308 RVA: 0x00068D82 File Offset: 0x00066F82
		public ExtensionFilter(string filterName, params string[] filterExtensions)
		{
			this.Name = filterName;
			this.Extensions = filterExtensions;
		}

		// Token: 0x040011BD RID: 4541
		public string Name;

		// Token: 0x040011BE RID: 4542
		public string[] Extensions;
	}
}
