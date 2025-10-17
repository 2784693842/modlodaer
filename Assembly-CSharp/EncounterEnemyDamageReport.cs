using System;
using System.Text;

// Token: 0x02000076 RID: 118
public struct EncounterEnemyDamageReport
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00031364 File Offset: 0x0002F564
	public float BaseDamage
	{
		get
		{
			return this.SizeDamage + this.ActionDamage;
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00031373 File Offset: 0x0002F573
	public float EnemyDamage
	{
		get
		{
			return this.BaseDamage + this.ValuesDamage + this.WoundsDamage + this.StatsAddedDamage + this.VsEscapeDamage;
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00031397 File Offset: 0x0002F597
	public float BaseDefense
	{
		get
		{
			return this.SizeDefense + this.BodyPartArmor;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000313A6 File Offset: 0x0002F5A6
	public float PlayerDefense
	{
		get
		{
			return this.BaseDefense + this.ArmorDefense + this.StatsDefense;
		}
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x000313BC File Offset: 0x0002F5BC
	public string ReportToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (this.DmgTypes.Length == 0)
		{
			stringBuilder.Append("Damage type: NONE\n");
		}
		else
		{
			stringBuilder.Append("Damage type: ");
			for (int i = 0; i < this.DmgTypes.Length; i++)
			{
				if (this.DmgTypes[i])
				{
					stringBuilder.Append(string.Format("{0} ", this.DmgTypes[i].Name));
				}
			}
			stringBuilder.Append("\n");
		}
		stringBuilder.Append(string.Format("<b>Enemy Attack: {0} ({1} Size + {2} Action + {3} Enemy Values + {4} Wounds + {5} Stats + {6} Vs Escape)\n", new object[]
		{
			this.EnemyDamage.ToString("0.##"),
			this.SizeDamage.ToString("0.##"),
			this.ActionDamage.ToString("0.##"),
			this.ValuesDamage.ToString("0.##"),
			this.WoundsDamage.ToString("0.##"),
			this.StatsAddedDamage.ToString("0.##"),
			this.VsEscapeDamage.ToString("0.##")
		}));
		stringBuilder.Append(string.Format("Player Defense: {0} ({1} Size + {2} {3} + {4} Armor + {5} Stats)</b>\n", new object[]
		{
			this.PlayerDefense.ToString("0.##"),
			this.SizeDefense.ToString("0.##"),
			this.BodyPartArmor.ToString("0.##"),
			this.HitBodyPart.ToString(),
			this.ArmorDefense.ToString("0.##"),
			this.StatsDefense.ToString("0.##")
		}));
		stringBuilder.Append(string.Format("Attack / Defense ratio: {0}\n", (this.EnemyDamage / this.PlayerDefense).ToString("0.##")));
		stringBuilder.Append(string.Format("Severity: {0}\n", this.AttackSeverity.ToString()));
		return stringBuilder.ToString();
	}

	// Token: 0x04000600 RID: 1536
	public float ValuesDamage;

	// Token: 0x04000601 RID: 1537
	public float WoundsDamage;

	// Token: 0x04000602 RID: 1538
	public float SizeDamage;

	// Token: 0x04000603 RID: 1539
	public float ActionDamage;

	// Token: 0x04000604 RID: 1540
	public float StatsAddedDamage;

	// Token: 0x04000605 RID: 1541
	public float VsEscapeDamage;

	// Token: 0x04000606 RID: 1542
	public DamageType[] DmgTypes;

	// Token: 0x04000607 RID: 1543
	public float SizeDefense;

	// Token: 0x04000608 RID: 1544
	public float BodyPartArmor;

	// Token: 0x04000609 RID: 1545
	public float ArmorDefense;

	// Token: 0x0400060A RID: 1546
	public float StatsDefense;

	// Token: 0x0400060B RID: 1547
	public BodyLocations HitBodyPart;

	// Token: 0x0400060C RID: 1548
	public WoundSeverity AttackSeverity;
}
