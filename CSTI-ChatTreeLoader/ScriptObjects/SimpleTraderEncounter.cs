using System;
using System.Collections.Generic;
using BepInEx;

namespace ChatTreeLoader.ScriptObjects
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class SimpleTraderEncounter : ModEncounterTypedBase<SimpleTraderEncounter>
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002206 File Offset: 0x00000406
		public override Dictionary<string, SimpleTraderEncounter> GetValidEncounterTable()
		{
			return SimpleTraderEncounter.SimpleTraderEncounters;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002210 File Offset: 0x00000410
		public override void OnEnable()
		{
			List<ModEncounterBase> list;
			if (ModEncounterBase.AllModEncounters.TryGetValue(typeof(SimpleTraderEncounter), out list))
			{
				list.Add(this);
				return;
			}
			ModEncounterBase.AllModEncounters[typeof(SimpleTraderEncounter)] = new List<ModEncounterBase>
			{
				this
			};
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000225D File Offset: 0x0000045D
		public override void Init()
		{
			if (this.HadInit)
			{
				return;
			}
			if (Utility.IsNullOrWhiteSpace(this.ThisId))
			{
				return;
			}
			this.HadInit = true;
			SimpleTraderEncounter.SimpleTraderEncounters[this.ThisId] = this;
		}

		// Token: 0x0400001B RID: 27
		public static readonly Dictionary<string, SimpleTraderEncounter> SimpleTraderEncounters = new Dictionary<string, SimpleTraderEncounter>();

		// Token: 0x0400001C RID: 28
		public string ThisId;

		// Token: 0x0400001D RID: 29
		public CoinSet CoinSet;

		// Token: 0x0400001E RID: 30
		public float CoinCostBase = 1f;

		// Token: 0x0400001F RID: 31
		public GameStat CoinCostBaseByStat;

		// Token: 0x04000020 RID: 32
		public List<BuySet> BuySets;

		// Token: 0x04000021 RID: 33
		public LocalizedString TradeEndMessage;
	}
}
