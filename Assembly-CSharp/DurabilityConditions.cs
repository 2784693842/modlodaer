using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
[Serializable]
public struct DurabilityConditions
{
	// Token: 0x060007CA RID: 1994 RVA: 0x0004D1B0 File Offset: 0x0004B3B0
	public bool CheckDurabilities(InGameCardBase _ForCard)
	{
		if (this.RequiredLiquidQuantity > 0f)
		{
			if (!_ForCard.IsLiquid)
			{
				return false;
			}
			if (_ForCard.CurrentLiquidQuantity < this.RequiredLiquidQuantity)
			{
				return false;
			}
		}
		bool flag = true;
		bool flag2 = true;
		bool flag3 = true;
		bool flag4 = true;
		bool flag5 = true;
		bool flag6 = true;
		bool flag7 = true;
		bool flag8 = true;
		if (this.RequiredSpoilagePercent)
		{
			flag = this.RequiredSpoilagePercent.IsInRange(_ForCard.CurrentSpoilagePercent);
		}
		if (this.RequiredUsagePercent)
		{
			flag2 = this.RequiredUsagePercent.IsInRange(_ForCard.CurrentUsagePercent);
		}
		if (this.RequiredFuelPercent)
		{
			flag3 = this.RequiredFuelPercent.IsInRange(_ForCard.CurrentFuelPercent);
		}
		if (this.RequiredProgressPercent)
		{
			flag4 = this.RequiredProgressPercent.IsInRange(_ForCard.CurrentProgressPercent);
		}
		if (this.RequiredSpecial1Percent)
		{
			flag5 = this.RequiredSpecial1Percent.IsInRange(_ForCard.CurrentSpecial1Percent);
		}
		if (this.RequiredSpecial2Percent)
		{
			flag6 = this.RequiredSpecial2Percent.IsInRange(_ForCard.CurrentSpecial2Percent);
		}
		if (this.RequiredSpecial3Percent)
		{
			flag7 = this.RequiredSpecial3Percent.IsInRange(_ForCard.CurrentSpecial3Percent);
		}
		if (this.RequiredSpecial4Percent)
		{
			flag8 = this.RequiredSpecial4Percent.IsInRange(_ForCard.CurrentSpecial4Percent);
		}
		return flag && flag2 && flag3 && flag4 && flag5 && flag6 && flag7 && flag8;
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x0004D308 File Offset: 0x0004B508
	public bool IsIdentical(DurabilityConditions _To)
	{
		return this.RequiredSpoilagePercent == _To.RequiredSpoilagePercent && Mathf.Approximately(this.RequiredSpoilagePercent, _To.RequiredSpoilagePercent) && this.RequiredUsagePercent == _To.RequiredUsagePercent && Mathf.Approximately(this.RequiredUsagePercent, _To.RequiredUsagePercent) && this.RequiredFuelPercent == _To.RequiredFuelPercent && Mathf.Approximately(this.RequiredFuelPercent, _To.RequiredFuelPercent) && this.RequiredProgressPercent == _To.RequiredProgressPercent && Mathf.Approximately(this.RequiredProgressPercent, _To.RequiredProgressPercent) && this.FailMessage == _To.FailMessage;
	}

	// Token: 0x04000B82 RID: 2946
	public OptionalRangeValue RequiredSpoilagePercent;

	// Token: 0x04000B83 RID: 2947
	public OptionalRangeValue RequiredUsagePercent;

	// Token: 0x04000B84 RID: 2948
	public OptionalRangeValue RequiredFuelPercent;

	// Token: 0x04000B85 RID: 2949
	public OptionalRangeValue RequiredProgressPercent;

	// Token: 0x04000B86 RID: 2950
	public float RequiredLiquidQuantity;

	// Token: 0x04000B87 RID: 2951
	public OptionalRangeValue RequiredSpecial1Percent;

	// Token: 0x04000B88 RID: 2952
	public OptionalRangeValue RequiredSpecial2Percent;

	// Token: 0x04000B89 RID: 2953
	public OptionalRangeValue RequiredSpecial3Percent;

	// Token: 0x04000B8A RID: 2954
	public OptionalRangeValue RequiredSpecial4Percent;

	// Token: 0x04000B8B RID: 2955
	public bool HideActionIfNotMet;

	// Token: 0x04000B8C RID: 2956
	public LocalizedString FailMessage;
}
