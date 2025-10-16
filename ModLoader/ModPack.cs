using System;
using BepInEx.Configuration;

namespace ModLoader
{
	// Token: 0x0200001A RID: 26
	public class ModPack
	{
		// Token: 0x0600004F RID: 79 RVA: 0x000043DD File Offset: 0x000025DD
		public ModPack(ModInfo modInfo, string fileName, ConfigEntry<bool> enableEntry)
		{
			this.ModInfo = modInfo;
			this.FileName = fileName;
			this.EnableEntry = enableEntry;
		}

		// Token: 0x04000049 RID: 73
		public readonly ModInfo ModInfo;

		// Token: 0x0400004A RID: 74
		public readonly string FileName;

		// Token: 0x0400004B RID: 75
		public readonly ConfigEntry<bool> EnableEntry;
	}
}
