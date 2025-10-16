using System;
using System.Text;
using BepInEx;
using UnityEngine;

namespace ChatTreeLoader.ScriptObjects
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class BuySet : ScriptableObject
	{
		// Token: 0x0600001C RID: 28 RVA: 0x000022B0 File Offset: 0x000004B0
		public string GetShowName()
		{
			if (!Utility.IsNullOrWhiteSpace(this.NameLocal.ToString()))
			{
				return this.NameLocal.ToString();
			}
			if (!Utility.IsNullOrWhiteSpace(this.Name))
			{
				return this.Name;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CardDrop cardDrop in this.BuyResult.DroppedCards)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string format = "{0}*{1};";
				object arg = cardDrop.DroppedCard.CardName;
				Vector2Int quantity = cardDrop.Quantity;
				stringBuilder2.Append(string.Format(format, arg, quantity.x));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000235E File Offset: 0x0000055E
		public int CalCost(SimpleTraderEncounter traderEncounter)
		{
			return (int)(traderEncounter.CoinCostBaseByStat ? (MBSingleton<GameManager>.Instance.StatsDict[traderEncounter.CoinCostBaseByStat].SimpleCurrentValue * this.CoinCostFactor) : (traderEncounter.CoinCostBase * this.CoinCostFactor));
		}

		// Token: 0x04000022 RID: 34
		public string Name;

		// Token: 0x04000023 RID: 35
		public LocalizedString NameLocal;

		// Token: 0x04000024 RID: 36
		public float CoinCostFactor = 1f;

		// Token: 0x04000025 RID: 37
		public CardsDropCollection BuyResult;

		// Token: 0x04000026 RID: 38
		public CardAction BuyEffect;

		// Token: 0x04000027 RID: 39
		public bool UseBuyEffect;

		// Token: 0x04000028 RID: 40
		public GeneralCondition BuyCondition;

		// Token: 0x04000029 RID: 41
		public GeneralCondition BuyShowCondition;

		// Token: 0x0400002A RID: 42
		public EncounterLogMessage BuySetMessage;

		// Token: 0x0400002B RID: 43
		public bool UseBuySetMessage;
	}
}
