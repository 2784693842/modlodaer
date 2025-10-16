using System;
using System.Collections.Generic;
using HarmonyLib;

namespace ChatTreeLoader.Behaviors
{
	// Token: 0x02000014 RID: 20
	public static class SavePathExt
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000035B6 File Offset: 0x000017B6
		public static string SavePath<T>(this Encounter encounter)
		{
			if (typeof(T) == typeof(ModEncounter))
			{
				return encounter.SaveChatPath();
			}
			return "";
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000035E0 File Offset: 0x000017E0
		public static string SaveChatPath(this Encounter encounter)
		{
			string uniqueID = encounter.UniqueID;
			if (!ModEncounter.ModEncounters.ContainsKey(uniqueID))
			{
				return null;
			}
			List<int> enumeration = ChatEncounterExt.CurPaths[uniqueID];
			return string.Concat(new string[]
			{
				"__{",
				uniqueID,
				"}ModEncounter.Infos__{EncounterPath:",
				enumeration.Join(null, "."),
				"}"
			});
		}
	}
}
