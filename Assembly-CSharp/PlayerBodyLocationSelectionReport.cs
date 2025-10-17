using System;
using System.Text;
using UnityEngine;

// Token: 0x0200006D RID: 109
public struct PlayerBodyLocationSelectionReport
{
	// Token: 0x170000DC RID: 220
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x0002F52E File Offset: 0x0002D72E
	public float HeadHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.Head);
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x0600048C RID: 1164 RVA: 0x0002F537 File Offset: 0x0002D737
	public float TorsoHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.Torso);
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x0002F540 File Offset: 0x0002D740
	public float LArmHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.LArm);
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600048E RID: 1166 RVA: 0x0002F549 File Offset: 0x0002D749
	public float RArmHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.RArm);
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x0002F552 File Offset: 0x0002D752
	public float LLegHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.LLeg);
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x0002F55B File Offset: 0x0002D75B
	public float RLegHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.RLeg);
		}
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0002F564 File Offset: 0x0002D764
	private float GetWeight(BodyLocations _ForLocation)
	{
		return Mathf.Max(this.BaseWeights.GetValue(_ForLocation) + this.ArmorWeights.GetValue(_ForLocation) + this.CoverWeights.GetValue(_ForLocation) + this.EnemyActionWeights.GetValue(_ForLocation), 0f);
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000492 RID: 1170 RVA: 0x0002F5A3 File Offset: 0x0002D7A3
	public float TotalWeight
	{
		get
		{
			return this.HeadHitWeight + this.TorsoHitWeight + this.LArmHitWeight + this.RArmHitWeight + this.LLegHitWeight + this.RLegHitWeight;
		}
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0002F5D0 File Offset: 0x0002D7D0
	public void FillRanges()
	{
		this.Ranges.Head = this.HeadHitWeight;
		this.Ranges.Torso = this.Ranges.Head + this.TorsoHitWeight;
		this.Ranges.LArm = this.Ranges.Torso + this.LArmHitWeight;
		this.Ranges.RArm = this.Ranges.LArm + this.RArmHitWeight;
		this.Ranges.LLeg = this.Ranges.RArm + this.LLegHitWeight;
		this.Ranges.RLeg = this.Ranges.LLeg + this.RLegHitWeight;
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0002F680 File Offset: 0x0002D880
	public string ReportToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(string.Format("Rolled: {0}\n", this.RandomRoll.ToString("0.##")));
		stringBuilder.Append(string.Format("{0}\n", this.BodyLocationWeightReport(BodyLocations.Head)));
		stringBuilder.Append(string.Format("{0}\n", this.BodyLocationWeightReport(BodyLocations.Torso)));
		stringBuilder.Append(string.Format("{0}\n", this.BodyLocationWeightReport(BodyLocations.LArm)));
		stringBuilder.Append(string.Format("{0}\n", this.BodyLocationWeightReport(BodyLocations.RArm)));
		stringBuilder.Append(string.Format("{0}\n", this.BodyLocationWeightReport(BodyLocations.LLeg)));
		stringBuilder.Append(string.Format("{0}\n", this.BodyLocationWeightReport(BodyLocations.RLeg)));
		return stringBuilder.ToString();
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0002F748 File Offset: 0x0002D948
	private string BodyLocationWeightReport(BodyLocations _Location)
	{
		float weight = this.GetWeight(_Location);
		float value = this.BaseWeights.GetValue(_Location);
		float value2 = this.ArmorWeights.GetValue(_Location);
		float value3 = this.CoverWeights.GetValue(_Location);
		float value4 = this.EnemyActionWeights.GetValue(_Location);
		float num = (_Location == BodyLocations.Head) ? 0f : this.Ranges.GetValue(_Location - 1);
		float value5 = this.Ranges.GetValue(_Location);
		string text = string.Format("{0}_{1} - {2}: {3} + {4} Armor + {5} Cover + {6} Enemy Action = {7} ({8}%)", new object[]
		{
			num.ToString("0.##"),
			value5.ToString("0.##"),
			BodyTemplate.LocationName(_Location),
			value.ToString("0.##"),
			value2.ToString("0.##"),
			value3.ToString("0.##"),
			value4.ToString("0.##"),
			weight.ToString("0.##"),
			(weight / this.TotalWeight * 100f).ToString("0.##")
		});
		if (this.RandomRoll > num && this.RandomRoll < value5)
		{
			return string.Format("<b>SELECTED {0}</b>", text);
		}
		return text;
	}

	// Token: 0x040005B0 RID: 1456
	public bool Ranged;

	// Token: 0x040005B1 RID: 1457
	public BodyLocationReportWeights BaseWeights;

	// Token: 0x040005B2 RID: 1458
	public BodyLocationReportWeights ArmorWeights;

	// Token: 0x040005B3 RID: 1459
	public BodyLocationReportWeights CoverWeights;

	// Token: 0x040005B4 RID: 1460
	public BodyLocationReportWeights EnemyActionWeights;

	// Token: 0x040005B5 RID: 1461
	public BodyLocationReportWeights Ranges;

	// Token: 0x040005B6 RID: 1462
	public float RandomRoll;
}
