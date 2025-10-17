using System;
using System.Text;
using UnityEngine;

// Token: 0x0200006E RID: 110
public struct EnemyBodyLocationSelectionReport
{
	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000496 RID: 1174 RVA: 0x0002F881 File Offset: 0x0002DA81
	public float HeadHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.Head);
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000497 RID: 1175 RVA: 0x0002F88A File Offset: 0x0002DA8A
	public float TorsoHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.Torso);
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000498 RID: 1176 RVA: 0x0002F893 File Offset: 0x0002DA93
	public float LArmHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.LArm);
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000499 RID: 1177 RVA: 0x0002F89C File Offset: 0x0002DA9C
	public float RArmHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.RArm);
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x0600049A RID: 1178 RVA: 0x0002F8A5 File Offset: 0x0002DAA5
	public float LLegHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.LLeg);
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x0600049B RID: 1179 RVA: 0x0002F8AE File Offset: 0x0002DAAE
	public float RLegHitWeight
	{
		get
		{
			return this.GetWeight(BodyLocations.RLeg);
		}
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0002F8B7 File Offset: 0x0002DAB7
	private float GetWeight(BodyLocations _ForLocation)
	{
		return Mathf.Max(this.BaseWeights.GetValue(_ForLocation) + this.ArmorWeights.GetValue(_ForLocation) + this.TrackingWeights.GetValue(_ForLocation), 0f);
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x0600049D RID: 1181 RVA: 0x0002F8E9 File Offset: 0x0002DAE9
	public float TotalWeight
	{
		get
		{
			return this.HeadHitWeight + this.TorsoHitWeight + this.LArmHitWeight + this.RArmHitWeight + this.LLegHitWeight + this.RLegHitWeight;
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0002F914 File Offset: 0x0002DB14
	public void FillRanges()
	{
		this.Ranges.Head = this.HeadHitWeight;
		this.Ranges.Torso = this.Ranges.Head + this.TorsoHitWeight;
		this.Ranges.LArm = this.Ranges.Torso + this.LArmHitWeight;
		this.Ranges.RArm = this.Ranges.LArm + this.RArmHitWeight;
		this.Ranges.LLeg = this.Ranges.RArm + this.LLegHitWeight;
		this.Ranges.RLeg = this.Ranges.LLeg + this.RLegHitWeight;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0002F9C4 File Offset: 0x0002DBC4
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

	// Token: 0x060004A0 RID: 1184 RVA: 0x0002FA8C File Offset: 0x0002DC8C
	private string BodyLocationWeightReport(BodyLocations _Location)
	{
		float weight = this.GetWeight(_Location);
		float value = this.BaseWeights.GetValue(_Location);
		float value2 = this.ArmorWeights.GetValue(_Location);
		float value3 = this.TrackingWeights.GetValue(_Location);
		float num = (_Location == BodyLocations.Head) ? 0f : this.Ranges.GetValue(_Location - 1);
		float value4 = this.Ranges.GetValue(_Location);
		string text = string.Format("{0}_{1} - {2}: {3} + {4} Armor + {5} Enemy State = {6} ({7}%)", new object[]
		{
			num.ToString("0.##"),
			value4.ToString("0.##"),
			BodyTemplate.LocationName(_Location),
			value.ToString("0.##"),
			value2.ToString("0.##"),
			value3.ToString("0.##"),
			weight.ToString("0.##"),
			(weight / this.TotalWeight * 100f).ToString("0.##")
		});
		if (this.RandomRoll > num && this.RandomRoll < value4)
		{
			return string.Format("<b>SELECTED {0}</b>", text);
		}
		return text;
	}

	// Token: 0x040005B7 RID: 1463
	public bool Ranged;

	// Token: 0x040005B8 RID: 1464
	public BodyLocationReportWeights BaseWeights;

	// Token: 0x040005B9 RID: 1465
	public BodyLocationReportWeights ArmorWeights;

	// Token: 0x040005BA RID: 1466
	public BodyLocationReportWeights TrackingWeights;

	// Token: 0x040005BB RID: 1467
	public BodyLocationReportWeights Ranges;

	// Token: 0x040005BC RID: 1468
	public float RandomRoll;
}
