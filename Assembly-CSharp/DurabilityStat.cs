using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
[Serializable]
public class DurabilityStat : OptionalFloatValue
{
	// Token: 0x06000793 RID: 1939 RVA: 0x0004AE59 File Offset: 0x00049059
	public DurabilityStat(bool _Active, int _Value) : base(_Active, (float)_Value)
	{
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000794 RID: 1940 RVA: 0x0004AE6F File Offset: 0x0004906F
	public float Max
	{
		get
		{
			if (this.MaxValue > 0f)
			{
				return this.MaxValue;
			}
			return this.FloatValue;
		}
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0004AE8C File Offset: 0x0004908C
	public bool Show(bool _HasLiquid, float _Value)
	{
		if (!this.Active)
		{
			return false;
		}
		switch (this.HidingOptions)
		{
		case DurabilityHidingOptions.HideWhenLiquid:
			return !_HasLiquid;
		case DurabilityHidingOptions.AlwaysShow:
			return true;
		case DurabilityHidingOptions.AlwaysHide:
			return false;
		case DurabilityHidingOptions.HideWhenZero:
			return _Value > 0f;
		case DurabilityHidingOptions.HideWhenFull:
			return _Value < this.Max;
		default:
			return true;
		}
	}

	// Token: 0x04000B0F RID: 2831
	public LocalizedString CardStatName;

	// Token: 0x04000B10 RID: 2832
	public float MaxValue = 100f;

	// Token: 0x04000B11 RID: 2833
	public float RatePerDaytimePoint;

	// Token: 0x04000B12 RID: 2834
	public float ExtraRateWhenEquipped;

	// Token: 0x04000B13 RID: 2835
	public DurabilityHidingOptions HidingOptions;

	// Token: 0x04000B14 RID: 2836
	public DurabilityTextDisplay TextDisplay;

	// Token: 0x04000B15 RID: 2837
	public Sprite OverrideIcon;

	// Token: 0x04000B16 RID: 2838
	public bool HasActionOnZero;

	// Token: 0x04000B17 RID: 2839
	public bool ShowPopupOnZero;

	// Token: 0x04000B18 RID: 2840
	public CardNotifications OnZeroNotification;

	// Token: 0x04000B19 RID: 2841
	public CardAction OnZero;

	// Token: 0x04000B1A RID: 2842
	public bool HasActionOnFull;

	// Token: 0x04000B1B RID: 2843
	public bool ShowPopupOnFull;

	// Token: 0x04000B1C RID: 2844
	public CardNotifications OnFullNotification;

	// Token: 0x04000B1D RID: 2845
	public CardAction OnFull;
}
