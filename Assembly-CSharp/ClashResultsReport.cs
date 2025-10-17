using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000072 RID: 114
public struct ClashResultsReport
{
	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0002FE1B File Offset: 0x0002E01B
	public float PlayerBaseClashValue
	{
		get
		{
			return this.PlayerActionClashValue + this.PlayerSizeClashValue + this.PlayerActionReachClashValue;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0002FE34 File Offset: 0x0002E034
	public float PlayerClashStatSum
	{
		get
		{
			if (this.PlayerClashStatsAddedValues == null)
			{
				return 0f;
			}
			if (this.PlayerClashStatsAddedValues.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.PlayerClashStatsAddedValues.Count; i++)
			{
				num += this.PlayerClashStatsAddedValues[i].Value;
			}
			return num;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0002FE93 File Offset: 0x0002E093
	public float PlayerClashValue
	{
		get
		{
			return Mathf.Max(this.PlayerBaseClashValue + this.PlayerClashStatSum + this.PlayerClashStealthBonus + this.PlayerClashEscapeModifier + this.PlayerClashIneffectiveRangeMalus, 0f);
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0002FEC1 File Offset: 0x0002E0C1
	public float EnemyBaseClashValue
	{
		get
		{
			return this.EnemyActionClashValue + this.EnemySizeClashValue + this.EnemyActionReachClashValue;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0002FED7 File Offset: 0x0002E0D7
	public float EnemyClashAddedFromValues
	{
		get
		{
			return this.EnemyClashAddedFromMeleeSkill + this.EnemyClashAddedFromRangedSkill + this.EnemyClashAddedFromBlood + this.EnemyClashAddedFromStamina + this.EnemyClashAddedFromMorale + this.EnemyClashAddedFromValue1 + this.EnemyClashAddedFromValue2 + this.EnemyClashAddedFromValue3 + this.EnemyClashAddedFromValue4;
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060004AA RID: 1194 RVA: 0x0002FF18 File Offset: 0x0002E118
	public float EnemyClashStatSum
	{
		get
		{
			if (this.EnemyClashStatsAddedValues == null)
			{
				return 0f;
			}
			if (this.EnemyClashStatsAddedValues.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.EnemyClashStatsAddedValues.Count; i++)
			{
				num += this.EnemyClashStatsAddedValues[i].Value;
			}
			return num;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060004AB RID: 1195 RVA: 0x0002FF77 File Offset: 0x0002E177
	public float EnemyClashValue
	{
		get
		{
			return Mathf.Max(this.EnemyBaseClashValue + this.EnemyClashAddedFromValues + this.EnemyClashAddedValueFromWounds + this.EnemyClashStealthBonus + this.EnemyClashEscapeModifier + this.EnemyClashIneffectiveRangeMalus + this.EnemyClashStatSum, 0f);
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0002FFB4 File Offset: 0x0002E1B4
	public string PlayerSummary(float _AddedClashValue, bool _Detailed)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(this.PlayerActionName))
		{
			stringBuilder.Append(string.Format("Player Action: {0}\n", this.PlayerActionName));
		}
		else
		{
			stringBuilder.Append("Player Action: UNNAMED ACTION\n");
		}
		if (!this.PlayerCannotFail)
		{
			stringBuilder.Append(string.Format("<b>Player Clash Value: {0}</b>\n", (this.PlayerClashValue + _AddedClashValue).ToString("0.##")));
		}
		else
		{
			stringBuilder.Append(string.Format("<b>Player Clash Value: {0} <color=red>(CANNOT FAIL IS CHECKED)</color></b> \n", (this.PlayerClashValue + _AddedClashValue).ToString("0.##")));
		}
		if (_Detailed && !Mathf.Approximately(this.PlayerBaseClashValue, 0f))
		{
			if (this.PlayerUsesRangedAttack)
			{
				stringBuilder.Append(string.Format("<b>Base Total: {0} (ranged)</b>\n", this.PlayerBaseClashValue.ToString("0.##")));
			}
			else
			{
				stringBuilder.Append(string.Format("<b>Base: {0}</b>\n", this.PlayerBaseClashValue.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.PlayerActionClashValue, 0f))
			{
				stringBuilder.Append(string.Format("   Action Clash Value: {0}\n", this.PlayerActionClashValue.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.PlayerSizeClashValue, 0f))
			{
				stringBuilder.Append(string.Format("   Size: {0}\n", this.PlayerSizeClashValue.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.PlayerActionReachClashValue, 0f))
			{
				stringBuilder.Append(string.Format("   Reach: {0}\n", this.PlayerActionReachClashValue.ToString("0.##")));
			}
		}
		else if (this.PlayerUsesRangedAttack)
		{
			stringBuilder.Append(string.Format("Base: {0} (ranged)\n", this.PlayerBaseClashValue.ToString("0.##")));
		}
		else
		{
			stringBuilder.Append(string.Format("Base: {0}\n", this.PlayerBaseClashValue.ToString("0.##")));
		}
		if (_Detailed)
		{
			stringBuilder.Append(string.Format("<b>Stats Total: {0}</b>\n", this.PlayerClashStatSum.ToString("0.##")));
			if (this.PlayerClashStatsAddedValues != null)
			{
				for (int i = 0; i < this.PlayerClashStatsAddedValues.Count; i++)
				{
					stringBuilder.Append(string.Format("   {0}: {1}\n", this.PlayerClashStatsAddedValues[i].Stat.name, this.PlayerClashStatsAddedValues[i].Value.ToString("0.##")));
				}
			}
		}
		else
		{
			stringBuilder.Append(string.Format("Stats: {0}\n", this.PlayerClashStatSum.ToString("0.##")));
		}
		if (this.PlayerClashStealthBonus != 0f)
		{
			stringBuilder.Append(string.Format("Stealth bonus: {0}\n", this.PlayerClashStealthBonus.ToString("0.##")));
		}
		if (this.PlayerClashIneffectiveRangeMalus != 0f)
		{
			stringBuilder.Append(string.Format("Ineffective range malus: {0}\n", this.PlayerClashIneffectiveRangeMalus.ToString("0.##")));
		}
		if (this.PlayerClashEscapeModifier != 0f)
		{
			stringBuilder.Append(string.Format("Vs Escape modifier: {0}\n", this.PlayerClashEscapeModifier.ToString("0.##")));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x000302F0 File Offset: 0x0002E4F0
	public string EnemySummary(float _AddedClashValue, bool _Detailed)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(this.EnemyActionName))
		{
			stringBuilder.Append(string.Format("Enemy Action: {0}\n", this.EnemyActionName));
		}
		else
		{
			stringBuilder.Append("Enemy Action: UNNAMED ACTION\n");
		}
		if (!this.EnemyCannotFail)
		{
			stringBuilder.Append(string.Format("<b>Enemy Clash Value: {0}</b>\n", (this.EnemyClashValue + _AddedClashValue).ToString()));
		}
		else
		{
			stringBuilder.Append(string.Format("<b>Enemy Clash Value: {0} <color=red>(CANNOT FAIL IS CHECKED)</color></b>\n", (this.EnemyClashValue + _AddedClashValue).ToString()));
		}
		if (_Detailed && !Mathf.Approximately(this.EnemyBaseClashValue, 0f))
		{
			if (this.EnemyUsesRangedAttack)
			{
				stringBuilder.Append(string.Format("<b>Base Total: {0} (ranged)</b>\n", this.EnemyBaseClashValue.ToString("0.##")));
			}
			else
			{
				stringBuilder.Append(string.Format("<b>Base Total: {0}</b>\n", this.EnemyBaseClashValue.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyActionClashValue, 0f))
			{
				stringBuilder.Append(string.Format("   Action Clash Value: {0}\n", this.EnemyActionClashValue.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemySizeClashValue, 0f))
			{
				stringBuilder.Append(string.Format("   Size: {0}\n", this.EnemySizeClashValue.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyActionReachClashValue, 0f))
			{
				stringBuilder.Append(string.Format("   Reach: {0}\n", this.EnemyActionReachClashValue.ToString("0.##")));
			}
		}
		else if (this.EnemyUsesRangedAttack)
		{
			stringBuilder.Append(string.Format("Base: {0} (ranged)\n", this.EnemyBaseClashValue.ToString("0.##")));
		}
		else
		{
			stringBuilder.Append(string.Format("Base: {0}\n", this.EnemyBaseClashValue.ToString("0.##")));
		}
		if (_Detailed && !Mathf.Approximately(this.EnemyClashAddedFromValues, 0f))
		{
			stringBuilder.Append(string.Format("<b>Values Total: {0}</b>\n", this.EnemyClashAddedFromValues.ToString("0.##")));
			if (!Mathf.Approximately(this.EnemyClashAddedFromMeleeSkill, 0f))
			{
				stringBuilder.Append(string.Format("   Melee Skill: {0}\n", this.EnemyClashAddedFromMeleeSkill.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyClashAddedFromRangedSkill, 0f))
			{
				stringBuilder.Append(string.Format("   Ranged Skill: {0}\n", this.EnemyClashAddedFromRangedSkill.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyClashAddedFromBlood, 0f))
			{
				stringBuilder.Append(string.Format("   Blood: {0}\n", this.EnemyClashAddedFromBlood.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyClashAddedFromStamina, 0f))
			{
				stringBuilder.Append(string.Format("   Stamina: {0}\n", this.EnemyClashAddedFromStamina.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyClashAddedFromMorale, 0f))
			{
				stringBuilder.Append(string.Format("   Morale: {0}\n", this.EnemyClashAddedFromMorale.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyClashAddedFromValue1, 0f))
			{
				stringBuilder.Append(string.Format("   Value 1: {0}\n", this.EnemyClashAddedFromValue1.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyClashAddedFromValue2, 0f))
			{
				stringBuilder.Append(string.Format("   Value 2: {0}\n", this.EnemyClashAddedFromValue2.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyClashAddedFromValue3, 0f))
			{
				stringBuilder.Append(string.Format("   Value 3: {0}\n", this.EnemyClashAddedFromValue3.ToString("0.##")));
			}
			if (!Mathf.Approximately(this.EnemyClashAddedFromValue4, 0f))
			{
				stringBuilder.Append(string.Format("   Value 4: {0}\n", this.EnemyClashAddedFromValue4.ToString("0.##")));
			}
		}
		else
		{
			stringBuilder.Append(string.Format("Values: {0}\n", this.EnemyClashAddedFromValues.ToString("0.##")));
		}
		stringBuilder.Append(string.Format("Wounds: {0}\n", this.EnemyClashAddedValueFromWounds.ToString("0.##")));
		if (_Detailed)
		{
			stringBuilder.Append(string.Format("<b>Stats Total: {0}</b>\n", this.EnemyClashStatSum.ToString("0.##")));
			if (this.EnemyClashStatsAddedValues != null)
			{
				for (int i = 0; i < this.EnemyClashStatsAddedValues.Count; i++)
				{
					stringBuilder.Append(string.Format("   {0}: {1}\n", this.EnemyClashStatsAddedValues[i].Stat.name, this.EnemyClashStatsAddedValues[i].Value.ToString("0.##")));
				}
			}
		}
		else
		{
			stringBuilder.Append(string.Format("Stats: {0}\n", this.EnemyClashStatSum.ToString("0.##")));
		}
		if (this.EnemyClashStealthBonus != 0f)
		{
			stringBuilder.Append(string.Format("Stealth bonus: {0}\n", this.EnemyClashStealthBonus.ToString("0.##")));
		}
		if (this.EnemyClashIneffectiveRangeMalus != 0f)
		{
			stringBuilder.Append(string.Format("Ineffective range malus: {0}\n", this.EnemyClashIneffectiveRangeMalus.ToString("0.##")));
		}
		if (this.EnemyClashEscapeModifier != 0f)
		{
			stringBuilder.Append(string.Format("Vs Escape modifier: {0}\n", this.EnemyClashEscapeModifier.ToString("0.##")));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x040005C6 RID: 1478
	public string PlayerActionName;

	// Token: 0x040005C7 RID: 1479
	public bool PlayerUsesRangedAttack;

	// Token: 0x040005C8 RID: 1480
	public bool PlayerCannotFail;

	// Token: 0x040005C9 RID: 1481
	public float PlayerActionClashValue;

	// Token: 0x040005CA RID: 1482
	public float PlayerSizeClashValue;

	// Token: 0x040005CB RID: 1483
	public float PlayerActionReachClashValue;

	// Token: 0x040005CC RID: 1484
	public List<StatAndFloatValue> PlayerClashStatsAddedValues;

	// Token: 0x040005CD RID: 1485
	public float PlayerClashStealthBonus;

	// Token: 0x040005CE RID: 1486
	public float PlayerClashIneffectiveRangeMalus;

	// Token: 0x040005CF RID: 1487
	public float PlayerClashEscapeModifier;

	// Token: 0x040005D0 RID: 1488
	public string EnemyActionName;

	// Token: 0x040005D1 RID: 1489
	public bool EnemyUsesRangedAttack;

	// Token: 0x040005D2 RID: 1490
	public bool EnemyCannotFail;

	// Token: 0x040005D3 RID: 1491
	public float EnemyActionClashValue;

	// Token: 0x040005D4 RID: 1492
	public float EnemySizeClashValue;

	// Token: 0x040005D5 RID: 1493
	public float EnemyActionReachClashValue;

	// Token: 0x040005D6 RID: 1494
	public List<StatAndFloatValue> EnemyClashStatsAddedValues;

	// Token: 0x040005D7 RID: 1495
	public float EnemyClashAddedFromMeleeSkill;

	// Token: 0x040005D8 RID: 1496
	public float EnemyClashAddedFromRangedSkill;

	// Token: 0x040005D9 RID: 1497
	public float EnemyClashAddedFromBlood;

	// Token: 0x040005DA RID: 1498
	public float EnemyClashAddedFromStamina;

	// Token: 0x040005DB RID: 1499
	public float EnemyClashAddedFromMorale;

	// Token: 0x040005DC RID: 1500
	public float EnemyClashAddedFromValue1;

	// Token: 0x040005DD RID: 1501
	public float EnemyClashAddedFromValue2;

	// Token: 0x040005DE RID: 1502
	public float EnemyClashAddedFromValue3;

	// Token: 0x040005DF RID: 1503
	public float EnemyClashAddedFromValue4;

	// Token: 0x040005E0 RID: 1504
	public float EnemyClashAddedValueFromWounds;

	// Token: 0x040005E1 RID: 1505
	public float EnemyClashStealthBonus;

	// Token: 0x040005E2 RID: 1506
	public float EnemyClashIneffectiveRangeMalus;

	// Token: 0x040005E3 RID: 1507
	public float EnemyClashEscapeModifier;
}
