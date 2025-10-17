using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class OverTimeDurabilityChanges
{
	// Token: 0x060007E6 RID: 2022 RVA: 0x0004E3E4 File Offset: 0x0004C5E4
	public void Add(TransferedDurabilities _Added)
	{
		this.Ticks = Mathf.Max(this.Ticks, 1);
		float num = this.SpoilagePerTick * (float)this.Ticks + _Added.Spoilage;
		float num2 = this.UsagePerTick * (float)this.Ticks + _Added.Usage;
		float num3 = this.FuelPerTick * (float)this.Ticks + _Added.Fuel;
		float num4 = this.ProgressPerTick * (float)this.Ticks + _Added.ConsumableCharges;
		float num5 = this.LiquidPerTick * (float)this.Ticks + _Added.Liquid;
		float num6 = this.Special1PerTick * (float)this.Ticks + _Added.Special1;
		float num7 = this.Special1PerTick * (float)this.Ticks + _Added.Special2;
		float num8 = this.Special1PerTick * (float)this.Ticks + _Added.Special3;
		float num9 = this.Special1PerTick * (float)this.Ticks + _Added.Special4;
		this.SpoilagePerTick = num / (float)this.Ticks;
		this.UsagePerTick = num2 / (float)this.Ticks;
		this.FuelPerTick = num3 / (float)this.Ticks;
		this.ProgressPerTick = num4 / (float)this.Ticks;
		this.LiquidPerTick = num5 / (float)this.Ticks;
		this.Special1PerTick = num6 / (float)this.Ticks;
		this.Special2PerTick = num7 / (float)this.Ticks;
		this.Special3PerTick = num8 / (float)this.Ticks;
		this.Special4PerTick = num9 / (float)this.Ticks;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0004E584 File Offset: 0x0004C784
	public OverTimeDurabilityChanges(TransferedDurabilities _From, int _Ticks)
	{
		this.Ticks = Mathf.Max(_Ticks, 1);
		float num = _From.Spoilage;
		float num2 = _From.Usage;
		float num3 = _From.Fuel;
		float num4 = _From.ConsumableCharges;
		float liquid = _From.Liquid;
		float num5 = _From.Special1;
		float num6 = _From.Special2;
		float num7 = _From.Special3;
		float num8 = _From.Special4;
		this.SpoilagePerTick = num / (float)this.Ticks;
		this.UsagePerTick = num2 / (float)this.Ticks;
		this.FuelPerTick = num3 / (float)this.Ticks;
		this.ProgressPerTick = num4 / (float)this.Ticks;
		this.LiquidPerTick = liquid / (float)this.Ticks;
		this.Special1PerTick = num5 / (float)this.Ticks;
		this.Special2PerTick = num6 / (float)this.Ticks;
		this.Special3PerTick = num7 / (float)this.Ticks;
		this.Special4PerTick = num8 / (float)this.Ticks;
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0004E69C File Offset: 0x0004C89C
	public OverTimeDurabilityChanges(CardStateChange _Modification, int _Ticks, float _LiquidScale)
	{
		this.Ticks = Mathf.Max(_Ticks, 1);
		float num = UnityEngine.Random.Range(_Modification.SpoilageChange.x, _Modification.SpoilageChange.y);
		float num2 = UnityEngine.Random.Range(_Modification.UsageChange.x, _Modification.UsageChange.y);
		float num3 = UnityEngine.Random.Range(_Modification.FuelChange.x, _Modification.FuelChange.y);
		float num4 = UnityEngine.Random.Range(_Modification.ChargesChange.x, _Modification.ChargesChange.y);
		float num5 = UnityEngine.Random.Range(_Modification.LiquidQuantityChange.x, _Modification.LiquidQuantityChange.y);
		float num6 = UnityEngine.Random.Range(_Modification.Special1Change.x, _Modification.Special1Change.y);
		float num7 = UnityEngine.Random.Range(_Modification.Special2Change.x, _Modification.Special2Change.y);
		float num8 = UnityEngine.Random.Range(_Modification.Special3Change.x, _Modification.Special3Change.y);
		float num9 = UnityEngine.Random.Range(_Modification.Special4Change.x, _Modification.Special4Change.y);
		if (_LiquidScale >= 0f)
		{
			num *= _LiquidScale;
			num2 *= _LiquidScale;
			num3 *= _LiquidScale;
			num4 *= _LiquidScale;
			num6 *= _LiquidScale;
			num7 *= _LiquidScale;
			num8 *= _LiquidScale;
			num9 *= _LiquidScale;
		}
		this.SpoilagePerTick = num / (float)this.Ticks;
		this.UsagePerTick = num2 / (float)this.Ticks;
		this.FuelPerTick = num3 / (float)this.Ticks;
		this.ProgressPerTick = num4 / (float)this.Ticks;
		this.LiquidPerTick = num5 / (float)this.Ticks;
		this.Special1PerTick = num6 / (float)this.Ticks;
		this.Special2PerTick = num7 / (float)this.Ticks;
		this.Special3PerTick = num8 / (float)this.Ticks;
		this.Special4PerTick = num9 / (float)this.Ticks;
	}

	// Token: 0x04000BE9 RID: 3049
	public float SpoilagePerTick;

	// Token: 0x04000BEA RID: 3050
	public float UsagePerTick;

	// Token: 0x04000BEB RID: 3051
	public float FuelPerTick;

	// Token: 0x04000BEC RID: 3052
	public float ProgressPerTick;

	// Token: 0x04000BED RID: 3053
	public float LiquidPerTick;

	// Token: 0x04000BEE RID: 3054
	public float Special1PerTick;

	// Token: 0x04000BEF RID: 3055
	public float Special2PerTick;

	// Token: 0x04000BF0 RID: 3056
	public float Special3PerTick;

	// Token: 0x04000BF1 RID: 3057
	public float Special4PerTick;

	// Token: 0x04000BF2 RID: 3058
	private int Ticks;
}
