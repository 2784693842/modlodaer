using System;
using System.Text;
using UnityEngine;

// Token: 0x02000074 RID: 116
public struct RangedClashResultReport
{
	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00030BF5 File Offset: 0x0002EDF5
	public float PlayerDefense
	{
		get
		{
			return Mathf.Max(0f, this.PlayerDefenseCover + this.PlayerDefenseInaccuracy + this.PlayerDefenseClashValue + this.PlayerDefenseSize);
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00030C1C File Offset: 0x0002EE1C
	public float PlayerClashValue
	{
		get
		{
			return this.CommonClashReport.PlayerClashValue;
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00030C29 File Offset: 0x0002EE29
	public float PlayerSuccessChance
	{
		get
		{
			return this.PlayerClashValue / Mathf.Max(0.0001f, this.PlayerClashValue + this.EnemyDefense);
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00030C49 File Offset: 0x0002EE49
	public float EnemyDefense
	{
		get
		{
			return Mathf.Max(0f, this.EnemyDefenseCover + this.EnemyDefenseInaccuracy + this.EnemyClashValue + this.EnemyDefenseSize);
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00030C70 File Offset: 0x0002EE70
	public float EnemyClashValue
	{
		get
		{
			return this.CommonClashReport.EnemyClashValue;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00030C7D File Offset: 0x0002EE7D
	public float EnemySuccessChance
	{
		get
		{
			return this.EnemyClashValue / Mathf.Max(0.0001f, this.EnemyClashValue + this.PlayerDefense);
		}
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00030C9D File Offset: 0x0002EE9D
	public string PlayerSummary()
	{
		return this.CommonClashReport.PlayerSummary(0f, true);
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00030CB0 File Offset: 0x0002EEB0
	public string EnemySummary()
	{
		return this.CommonClashReport.EnemySummary(0f, true);
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x00030CC4 File Offset: 0x0002EEC4
	public string ResultsSummary()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (Mathf.Approximately(this.PlayerDefenseClashValue, 0f))
		{
			stringBuilder.Append(string.Format("Player Defense: {0} Cover + {1} Enemy Inaccuracy - {2} Size = {3} TOTAL\n", new object[]
			{
				this.PlayerDefenseCover.ToString("0.##"),
				this.PlayerDefenseInaccuracy.ToString("0.##"),
				(-this.PlayerDefenseSize).ToString("0.##"),
				this.PlayerDefense.ToString("0.##")
			}));
		}
		else
		{
			stringBuilder.Append(string.Format("Player Defense: {0} Clash Value (because Enemy uses Melee) = {1} TOTAL\n", this.PlayerDefenseClashValue.ToString("0.##"), this.PlayerDefense.ToString("0.##")));
		}
		if (Mathf.Approximately(this.EnemyDefenseClashValue, 0f))
		{
			stringBuilder.Append(string.Format("Enemy Defense: {0} Cover + {1} Player Inaccuracy - {2} Size = {3} TOTAL\n", new object[]
			{
				this.EnemyDefenseCover.ToString("0.##"),
				this.EnemyDefenseInaccuracy.ToString("0.##"),
				(-this.EnemyDefenseSize).ToString("0.##"),
				this.EnemyDefense.ToString("0.##")
			}));
		}
		else
		{
			stringBuilder.Append(string.Format("Enemy Defense: {0} Clash Value (because Player uses Melee) = {1} TOTAL\n", this.EnemyDefenseClashValue.ToString("0.##"), this.EnemyDefense.ToString("0.##")));
		}
		stringBuilder.Append(string.Format("Player hit roll: {0}\n", this.PlayerRandomRoll.ToString("0.##")));
		if (this.PlayerRandomRoll < this.PlayerClashValue)
		{
			stringBuilder.Append(string.Format("Player hit: <b>0_{0} SUCCESS ({2}%)</b> | {0}_{1} failure ({3}%)\n", new object[]
			{
				this.PlayerClashValue.ToString("0.##"),
				(this.PlayerClashValue + this.EnemyDefense).ToString("0.##"),
				(this.PlayerSuccessChance * 100f).ToString("0.##"),
				((1f - this.PlayerSuccessChance) * 100f).ToString("0.##")
			}));
			if (this.CommonClashReport.PlayerCannotFail)
			{
				stringBuilder.Append("<color=red>Player Cannot Fail</color>");
			}
		}
		else
		{
			stringBuilder.Append(string.Format("Player hit: 0_{0} success ({2}%) | <b>{0}_{1} FAILURE ({3}%)</b>\n", new object[]
			{
				this.PlayerClashValue.ToString("0.##"),
				(this.PlayerClashValue + this.EnemyDefense).ToString("0.##"),
				(this.PlayerSuccessChance * 100f).ToString("0.##"),
				((1f - this.PlayerSuccessChance) * 100f).ToString("0.##")
			}));
			if (this.CommonClashReport.PlayerCannotFail)
			{
				stringBuilder.Append("<color=red>But Player Cannot Fail</color>");
			}
		}
		stringBuilder.Append(string.Format("Enemy hit roll: {0}\n", this.EnemyRandomRoll.ToString("0.##")));
		if (this.EnemyRandomRoll < this.EnemyClashValue)
		{
			stringBuilder.Append(string.Format("Enemy hit: <b>0_{0} SUCCESS ({2}%)</b> | {0}_{1} failure ({3}%)\n", new object[]
			{
				this.EnemyClashValue.ToString("0.##"),
				(this.EnemyClashValue + this.PlayerDefense).ToString("0.##"),
				(this.EnemySuccessChance * 100f).ToString("0.##"),
				((1f - this.EnemySuccessChance) * 100f).ToString("0.##")
			}));
			if (this.CommonClashReport.EnemyCannotFail)
			{
				stringBuilder.Append("<color=red>Enemy Cannot Fail</color>");
			}
		}
		else
		{
			stringBuilder.Append(string.Format("Enemy hit: 0_{0} success ({2}%) | <b>{0}_{1} FAILURE ({3}%)</b>\n", new object[]
			{
				this.EnemyClashValue.ToString("0.##"),
				(this.EnemyClashValue + this.PlayerDefense).ToString("0.##"),
				(this.EnemySuccessChance * 100f).ToString("0.##"),
				((1f - this.EnemySuccessChance) * 100f).ToString("0.##")
			}));
			if (this.CommonClashReport.EnemyCannotFail)
			{
				stringBuilder.Append("<color=red>But Enemy Cannot Fail</color>");
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x040005EA RID: 1514
	public ClashResultsReport CommonClashReport;

	// Token: 0x040005EB RID: 1515
	public float PlayerDefenseSize;

	// Token: 0x040005EC RID: 1516
	public float PlayerDefenseCover;

	// Token: 0x040005ED RID: 1517
	public float PlayerDefenseInaccuracy;

	// Token: 0x040005EE RID: 1518
	public float PlayerDefenseClashValue;

	// Token: 0x040005EF RID: 1519
	public float EnemyDefenseSize;

	// Token: 0x040005F0 RID: 1520
	public float EnemyDefenseCover;

	// Token: 0x040005F1 RID: 1521
	public float EnemyDefenseInaccuracy;

	// Token: 0x040005F2 RID: 1522
	public float EnemyDefenseClashValue;

	// Token: 0x040005F3 RID: 1523
	public float PlayerRandomRoll;

	// Token: 0x040005F4 RID: 1524
	public float EnemyRandomRoll;
}
