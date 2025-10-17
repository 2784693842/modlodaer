using System;
using System.Text;
using UnityEngine;

// Token: 0x02000073 RID: 115
public struct MeleeClashResultsReport
{
	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060004AE RID: 1198 RVA: 0x00030872 File Offset: 0x0002EA72
	public string PlayerSummary
	{
		get
		{
			return this.CommonClashReport.PlayerSummary(0f, true);
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060004AF RID: 1199 RVA: 0x00030885 File Offset: 0x0002EA85
	public string EnemySummary
	{
		get
		{
			return this.CommonClashReport.EnemySummary(0f, true);
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00030898 File Offset: 0x0002EA98
	private float PlayerPercentChance
	{
		get
		{
			return this.PlayerHitsRangeUpTo / this.TieRangeUpTo;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060004B1 RID: 1201 RVA: 0x000308A7 File Offset: 0x0002EAA7
	private float EnemyPercentChance
	{
		get
		{
			return (this.EnemyHitsRangeUpTo - this.PlayerHitsRangeUpTo) / this.TieRangeUpTo;
		}
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x000308C0 File Offset: 0x0002EAC0
	public string ResultsSummary()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(string.Format("Clash ratio: {0} | {1}% chance of tie\n", (Mathf.Max(this.CommonClashReport.PlayerClashValue, this.CommonClashReport.EnemyClashValue) / Mathf.Min(this.CommonClashReport.PlayerClashValue, this.CommonClashReport.EnemyClashValue)).ToString("0.##"), (this.TiePercentChance * 100f).ToString("0.#")));
		stringBuilder.Append(string.Format("Total weight: {0}, roll: {1}\n", this.TieRangeUpTo, this.RollValue.ToString("0.##")));
		if (this.RollValue < this.PlayerHitsRangeUpTo)
		{
			stringBuilder.Append(string.Format("<b>Player Hits 0_{0} ({3}%)</b>\nEnemy Hits {0}_{1} ({4}%)\nTie {1}_{2} ({5}%)", new object[]
			{
				this.PlayerHitsRangeUpTo.ToString("0.##"),
				this.EnemyHitsRangeUpTo.ToString("0.##"),
				this.TieRangeUpTo.ToString("0.##"),
				(this.PlayerPercentChance * 100f).ToString("0.##"),
				(this.EnemyPercentChance * 100f).ToString("0.##"),
				(this.TiePercentChance * 100f).ToString("0.##")
			}));
			if (this.CommonClashReport.EnemyCannotFail)
			{
				stringBuilder.Append("<color=red>But Enemy Cannot Fail so both parties will hit</color>");
			}
		}
		else if (this.RollValue < this.EnemyHitsRangeUpTo)
		{
			stringBuilder.Append(string.Format("Player Hits 0_{0} ({3}%)\n<b>Enemy Hits {0}_{1} ({4}%)</b>\nTie {1}_{2} ({5}%)", new object[]
			{
				this.PlayerHitsRangeUpTo.ToString("0.##"),
				this.EnemyHitsRangeUpTo.ToString("0.##"),
				this.TieRangeUpTo.ToString("0.##"),
				(this.PlayerPercentChance * 100f).ToString("0.##"),
				(this.EnemyPercentChance * 100f).ToString("0.##"),
				(this.TiePercentChance * 100f).ToString("0.##")
			}));
			if (this.CommonClashReport.PlayerCannotFail)
			{
				stringBuilder.Append("<color=red>But Player Cannot Fail so both parties will hit</color>");
			}
		}
		else
		{
			stringBuilder.Append(string.Format("Player Hits 0_{0} ({3}%)\nEnemy Hits {0}_{1} ({4}%)\n<b>Tie {1}_{2} ({5}%)</b>", new object[]
			{
				this.PlayerHitsRangeUpTo.ToString("0.##"),
				this.EnemyHitsRangeUpTo.ToString("0.##"),
				this.TieRangeUpTo.ToString("0.##"),
				(this.PlayerPercentChance * 100f).ToString("0.##"),
				(this.EnemyPercentChance * 100f).ToString("0.##"),
				(this.TiePercentChance * 100f).ToString("0.##")
			}));
			if (this.CommonClashReport.PlayerCannotFail)
			{
				stringBuilder.Append("<color=red>Player Cannot Fail</color>");
			}
			if (this.CommonClashReport.EnemyCannotFail)
			{
				stringBuilder.Append("<color=red>Enemy Cannot Fail</color>");
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x040005E4 RID: 1508
	public ClashResultsReport CommonClashReport;

	// Token: 0x040005E5 RID: 1509
	public float TiePercentChance;

	// Token: 0x040005E6 RID: 1510
	public float RollValue;

	// Token: 0x040005E7 RID: 1511
	public float PlayerHitsRangeUpTo;

	// Token: 0x040005E8 RID: 1512
	public float EnemyHitsRangeUpTo;

	// Token: 0x040005E9 RID: 1513
	public float TieRangeUpTo;
}
