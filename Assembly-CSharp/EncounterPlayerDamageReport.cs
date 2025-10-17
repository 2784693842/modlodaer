using System;
using System.Text;

// Token: 0x02000075 RID: 117
public struct EncounterPlayerDamageReport
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060004BC RID: 1212 RVA: 0x00031139 File Offset: 0x0002F339
	public float BaseDamage
	{
		get
		{
			return this.SizeDamage + this.ActionDamage;
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060004BD RID: 1213 RVA: 0x00031148 File Offset: 0x0002F348
	public float PlayerDamage
	{
		get
		{
			return this.BaseDamage + this.StatsAddedDamage + this.VsEscapeDamage;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060004BE RID: 1214 RVA: 0x0003115E File Offset: 0x0002F35E
	public float BaseDefense
	{
		get
		{
			return this.SizeDefense + this.BodyPartArmor;
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060004BF RID: 1215 RVA: 0x0003116D File Offset: 0x0002F36D
	public float EnemyDefense
	{
		get
		{
			return this.BaseDefense + this.ArmorDefense + this.TrackingDefense;
		}
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00031184 File Offset: 0x0002F384
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
		stringBuilder.Append(string.Format("<b>Player Attack: {0} ({1} Size + {2} Action + {3} Stats + {4} Vs Escape)\n", new object[]
		{
			this.PlayerDamage.ToString("0.##"),
			this.SizeDamage.ToString("0.##"),
			this.ActionDamage.ToString("0.##"),
			this.StatsAddedDamage.ToString("0.##"),
			this.VsEscapeDamage.ToString("0.##")
		}));
		stringBuilder.Append(string.Format("Enemy Defense: {0} ({1} Size + {2} {3} + {4} Armor + {5} Enemy State)</b>\n", new object[]
		{
			this.EnemyDefense.ToString("0.##"),
			this.SizeDefense.ToString("0.##"),
			this.BodyPartArmor.ToString("0.##"),
			this.HitBodyPart.ToString(),
			this.ArmorDefense.ToString("0.##"),
			this.TrackingDefense.ToString("0.##")
		}));
		stringBuilder.Append(string.Format("Attack / Defense ratio: {0}\n", (this.PlayerDamage / this.EnemyDefense).ToString("0.##")));
		stringBuilder.Append(string.Format("Severity: {0}", this.AttackSeverity.ToString()));
		return stringBuilder.ToString();
	}

	// Token: 0x040005F5 RID: 1525
	public float SizeDamage;

	// Token: 0x040005F6 RID: 1526
	public float ActionDamage;

	// Token: 0x040005F7 RID: 1527
	public float VsEscapeDamage;

	// Token: 0x040005F8 RID: 1528
	public float StatsAddedDamage;

	// Token: 0x040005F9 RID: 1529
	public DamageType[] DmgTypes;

	// Token: 0x040005FA RID: 1530
	public float SizeDefense;

	// Token: 0x040005FB RID: 1531
	public float BodyPartArmor;

	// Token: 0x040005FC RID: 1532
	public float ArmorDefense;

	// Token: 0x040005FD RID: 1533
	public float TrackingDefense;

	// Token: 0x040005FE RID: 1534
	public BodyLocations HitBodyPart;

	// Token: 0x040005FF RID: 1535
	public WoundSeverity AttackSeverity;
}
