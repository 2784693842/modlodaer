using System;
using System.Collections.Generic;
using ChatTreeLoader.ScriptObjects;
using UnityEngine;

namespace ChatTreeLoader
{
	// Token: 0x02000002 RID: 2
	[Serializable]
	public class ModEncounter : ModEncounterTypedBase<ModEncounter>
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override Dictionary<string, ModEncounter> GetValidEncounterTable()
		{
			return ModEncounter.ModEncounters;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public override void Init()
		{
			if (this.HadInit)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.ThisId) && !ModEncounter.ModEncounters.ContainsKey(this.ThisId))
			{
				this.HadInit = true;
				ModEncounter.ModEncounters[this.ThisId] = this;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020A8 File Offset: 0x000002A8
		public override void OnEnable()
		{
			List<ModEncounterBase> list;
			if (ModEncounterBase.AllModEncounters.TryGetValue(typeof(ModEncounter), out list))
			{
				list.Add(this);
				return;
			}
			ModEncounterBase.AllModEncounters[typeof(ModEncounter)] = new List<ModEncounterBase>
			{
				this
			};
		}

		// Token: 0x04000001 RID: 1
		public static readonly Dictionary<string, ModEncounter> ModEncounters = new Dictionary<string, ModEncounter>();

		// Token: 0x04000002 RID: 2
		public string ThisId;

		// Token: 0x04000003 RID: 3
		public ModEncounterNode[] ModEncounterNodes = new ModEncounterNode[0];

		// Token: 0x04000004 RID: 4
		public AudioClip DefaultPlayerAudio;

		// Token: 0x04000005 RID: 5
		public AudioClip DefaultEnemyAudio;
	}
}
