using System;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class TransferedDurabilities
{
	// Token: 0x060007E1 RID: 2017 RVA: 0x0004DF80 File Offset: 0x0004C180
	public TransferedDurabilities()
	{
		this.Spoilage = new OptionalFloatValue(true, 0f);
		this.Usage = new OptionalFloatValue(true, 0f);
		this.Fuel = new OptionalFloatValue(true, 0f);
		this.ConsumableCharges = new OptionalFloatValue(true, 0f);
		this.Special1 = new OptionalFloatValue(true, 0f);
		this.Special2 = new OptionalFloatValue(true, 0f);
		this.Special3 = new OptionalFloatValue(true, 0f);
		this.Special4 = new OptionalFloatValue(true, 0f);
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0004E01C File Offset: 0x0004C21C
	public bool IsEmpty
	{
		get
		{
			return this.Spoilage == 0f && this.Usage == 0f && this.Fuel == 0f && this.ConsumableCharges == 0f && this.Special1 == 0f && this.Special2 == 0f && this.Special3 == 0f && this.Special4 == 0f;
		}
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0004E0BC File Offset: 0x0004C2BC
	public void Add(TransferedDurabilities _From)
	{
		this.Spoilage.Add(_From.Spoilage);
		this.Usage.Add(_From.Usage);
		this.Fuel.Add(_From.Fuel);
		this.ConsumableCharges.Add(_From.ConsumableCharges);
		this.Liquid += _From.Liquid;
		this.Special1.Add(_From.Special1);
		this.Special2.Add(_From.Special2);
		this.Special3.Add(_From.Special3);
		this.Special4.Add(_From.Special4);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0004E164 File Offset: 0x0004C364
	public void Multiply(float _Mult, bool _IgnoreLiquid)
	{
		this.Spoilage.Multiply(_Mult);
		this.Usage.Multiply(_Mult);
		this.Fuel.Multiply(_Mult);
		this.ConsumableCharges.Multiply(_Mult);
		if (!_IgnoreLiquid)
		{
			this.Liquid *= _Mult;
		}
		this.Special1.Multiply(_Mult);
		this.Special2.Multiply(_Mult);
		this.Special3.Multiply(_Mult);
		this.Special4.Multiply(_Mult);
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0004E1E4 File Offset: 0x0004C3E4
	public void Add(AddedDurabilityModifier _From)
	{
		if (_From.SpoilageChange != Vector2.zero)
		{
			this.Spoilage.Add(new OptionalFloatValue(true, UnityEngine.Random.Range(_From.SpoilageChange.x, _From.SpoilageChange.y)));
		}
		if (_From.UsageChange != Vector2.zero)
		{
			this.Usage.Add(new OptionalFloatValue(true, UnityEngine.Random.Range(_From.UsageChange.x, _From.UsageChange.y)));
		}
		if (_From.FuelChange != Vector2.zero)
		{
			this.Fuel.Add(new OptionalFloatValue(true, UnityEngine.Random.Range(_From.FuelChange.x, _From.FuelChange.y)));
		}
		if (_From.ChargesChange != Vector2.zero)
		{
			this.ConsumableCharges.Add(new OptionalFloatValue(true, UnityEngine.Random.Range(_From.ChargesChange.x, _From.ChargesChange.y)));
		}
		if (_From.Special1Change != Vector2.zero)
		{
			this.Special1.Add(new OptionalFloatValue(true, UnityEngine.Random.Range(_From.Special1Change.x, _From.Special1Change.y)));
		}
		if (_From.Special2Change != Vector2.zero)
		{
			this.Special2.Add(new OptionalFloatValue(true, UnityEngine.Random.Range(_From.Special2Change.x, _From.Special2Change.y)));
		}
		if (_From.Special3Change != Vector2.zero)
		{
			this.Special3.Add(new OptionalFloatValue(true, UnityEngine.Random.Range(_From.Special3Change.x, _From.Special3Change.y)));
		}
		if (_From.Special4Change != Vector2.zero)
		{
			this.Special4.Add(new OptionalFloatValue(true, UnityEngine.Random.Range(_From.Special4Change.x, _From.Special4Change.y)));
		}
	}

	// Token: 0x04000BE0 RID: 3040
	public OptionalFloatValue Spoilage;

	// Token: 0x04000BE1 RID: 3041
	public OptionalFloatValue Usage;

	// Token: 0x04000BE2 RID: 3042
	public OptionalFloatValue Fuel;

	// Token: 0x04000BE3 RID: 3043
	public OptionalFloatValue ConsumableCharges;

	// Token: 0x04000BE4 RID: 3044
	public float Liquid;

	// Token: 0x04000BE5 RID: 3045
	public OptionalFloatValue Special1;

	// Token: 0x04000BE6 RID: 3046
	public OptionalFloatValue Special2;

	// Token: 0x04000BE7 RID: 3047
	public OptionalFloatValue Special3;

	// Token: 0x04000BE8 RID: 3048
	public OptionalFloatValue Special4;
}
