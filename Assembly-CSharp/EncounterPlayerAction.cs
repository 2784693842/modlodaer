using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000128 RID: 296
[Serializable]
public class EncounterPlayerAction
{
	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00054FE6 File Offset: 0x000531E6
	// (set) Token: 0x060008DA RID: 2266 RVA: 0x00054FEE File Offset: 0x000531EE
	public InGameCardBase AssociatedCard { get; private set; }

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060008DB RID: 2267 RVA: 0x00054FF7 File Offset: 0x000531F7
	// (set) Token: 0x060008DC RID: 2268 RVA: 0x00054FFF File Offset: 0x000531FF
	public InGameCardBase AmmoCard { get; private set; }

	// Token: 0x060008DD RID: 2269 RVA: 0x00055008 File Offset: 0x00053208
	private float GetValueFromVector(Vector2 _Vector, bool _WithRandomness)
	{
		if (Mathf.Approximately(_Vector.x, _Vector.y))
		{
			return _Vector.x;
		}
		if (_WithRandomness)
		{
			return UnityEngine.Random.Range(_Vector.x, _Vector.y);
		}
		return Mathf.Lerp(_Vector.x, _Vector.y, 0.5f);
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0005505A File Offset: 0x0005325A
	public float GetClash(bool _WithRandomness)
	{
		return this.GetValueFromVector(this.Clash, _WithRandomness);
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00055069 File Offset: 0x00053269
	public float GetClashInaccuracy(bool _WithRandomness)
	{
		return this.GetValueFromVector(this.ClashInaccuracy, _WithRandomness);
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00055078 File Offset: 0x00053278
	public float GetDamage(bool _WithRandomness)
	{
		return this.GetValueFromVector(this.Damage, _WithRandomness);
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00055087 File Offset: 0x00053287
	public float GetDamageStatSum(bool _WithRandomness)
	{
		return this.GetValueFromVector(this.DamageStatSum, _WithRandomness);
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x00055096 File Offset: 0x00053296
	public float GetClashStealthBonus(bool _WithRandomness)
	{
		return this.GetValueFromVector(this.ClashStealthBonus, _WithRandomness);
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x000550A5 File Offset: 0x000532A5
	public float GetClashIneffectiveRangeMalus(bool _WithRandomness)
	{
		return this.GetValueFromVector(this.ClashIneffectiveRangeMalus, _WithRandomness);
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x000550B4 File Offset: 0x000532B4
	public float GetClashEscapeModifier(bool _WithRandomness)
	{
		return this.GetValueFromVector(this.ClashEscapeModifier, _WithRandomness);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x000550C3 File Offset: 0x000532C3
	public float GetDmgVsEscapeModifier(bool _WithRandomness)
	{
		return this.GetValueFromVector(this.DmgVsEscapeModifier, _WithRandomness);
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x000550D4 File Offset: 0x000532D4
	public List<StatAndFloatValue> GetClashStatsAddedValues(bool _WithRandomness)
	{
		if (this.ClashStatsAddedValues == null)
		{
			return null;
		}
		if (this.ClashStatsAddedValues.Count == 0)
		{
			return null;
		}
		List<StatAndFloatValue> list = new List<StatAndFloatValue>();
		for (int i = 0; i < this.ClashStatsAddedValues.Count; i++)
		{
			list.Add(new StatAndFloatValue(this.ClashStatsAddedValues[i].Stat, this.GetValueFromVector(this.ClashStatsAddedValues[i].Value, _WithRandomness)));
		}
		return list;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0005514B File Offset: 0x0005334B
	public bool TryReplaceCard(InGameCardBase _WithCard)
	{
		if (!this.AssociatedCard)
		{
			this.AssociatedCard = _WithCard;
			return true;
		}
		if (this.AssociatedCard.DurabilityScore() > _WithCard.DurabilityScore())
		{
			this.AssociatedCard = _WithCard;
			return true;
		}
		return false;
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00055180 File Offset: 0x00053380
	public bool TryReplaceAmmo(InGameCardBase _WithCard)
	{
		if (!this.AmmoCard)
		{
			this.AmmoCard = _WithCard;
			return true;
		}
		if (this.AmmoCard.DurabilityScore() > _WithCard.DurabilityScore())
		{
			this.AmmoCard = _WithCard;
			return true;
		}
		return false;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x000551B8 File Offset: 0x000533B8
	public EncounterPlayerAction(InGameCardBase _Card, EncounterLogMessage _SuccessLog, EncounterLogMessage _FailureLog, string _EnemyName, InGameCardBase _WithAmmo = null)
	{
		if (!_Card || _Card.CardModel == null)
		{
			return;
		}
		this.AssociatedCard = _Card;
		if (!_WithAmmo)
		{
			if (!_Card.CardModel.NeedsAnyAmmo)
			{
				this.ActionName = _Card.CardName(false);
			}
			else
			{
				this.ActionName = string.Format("{0}\n{1}", _Card.CardName(false), LocalizedString.MissingCard(_Card.CardModel.AmmoNeeded[0].TargetName, false));
				this.NoAmmo = true;
			}
		}
		else
		{
			this.ActionName = LocalizedString.WeaponAndProjectileName(_Card.CardName(false), _WithAmmo.CardName(false));
			this.CurrentAmmoCount = 1;
		}
		this.ActionSuccessLog = EncounterLogMessage.ApplyFormatArguments(_SuccessLog, new object[]
		{
			_EnemyName,
			_Card.CardName(true)
		});
		this.ActionFailureLog = EncounterLogMessage.ApplyFormatArguments(_FailureLog, new object[]
		{
			_EnemyName,
			_Card.CardName(true)
		});
		this.DistanceChange = EncounterDistanceChange.DontChangeDistance;
		this.RequiredDistance = _Card.CardModel.RequiredDistance;
		this.ActionRange = _Card.CardModel.ActionRange;
		this.Reach = _Card.CardModel.WeaponReach;
		this.Clash = Vector2.zero;
		this.Damage = Vector2.zero;
		this.DamageStatSum = Vector2.zero;
		this.StatModifiers = _Card.CardModel.WeaponActionStatChanges;
		if (this.ClashStatsAddedValues == null)
		{
			this.ClashStatsAddedValues = new List<StatAndVector2Value>();
		}
		else
		{
			this.ClashStatsAddedValues.Clear();
		}
		this.ClashStealthBonus = Vector2.zero;
		this.ClashIneffectiveRangeMalus = Vector2.zero;
		this.ClashEscapeModifier = Vector2.zero;
		this.DmgVsEscapeModifier = Vector2.zero;
		this.ApplyClashAndDamageFromCard(_Card.CardModel, _Card.CardModel.ActionRange == ActionRange.Ranged);
		if (_WithAmmo)
		{
			int num = (_Card.CardModel.DamageTypes == null) ? 0 : _Card.CardModel.DamageTypes.Length;
			int num2 = (_WithAmmo.CardModel.DamageTypes == null) ? 0 : _WithAmmo.CardModel.DamageTypes.Length;
			this.DamageTypes = new DamageType[num + num2];
			for (int i = 0; i < this.DamageTypes.Length; i++)
			{
				if (i < num)
				{
					this.DamageTypes[i] = _Card.CardModel.DamageTypes[i];
				}
			}
		}
		else
		{
			this.DamageTypes = _Card.CardModel.DamageTypes;
		}
		if (!_WithAmmo)
		{
			return;
		}
		if (!_WithAmmo.CardModel)
		{
			return;
		}
		this.AmmoCard = _WithAmmo;
		this.ApplyClashAndDamageFromCard(_WithAmmo.CardModel, _Card.CardModel.ActionRange == ActionRange.Ranged);
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00055450 File Offset: 0x00053650
	private void ApplyClashAndDamageFromCard(CardData _Card, bool _Ranged)
	{
		this.Clash += _Card.BaseClashValue;
		if (_Ranged)
		{
			this.ClashInaccuracy += _Card.ClashRangedInaccuracy;
		}
		this.Damage += _Card.WeaponDamage;
		if (_Card.WeaponDamageStatInfluences != null)
		{
			for (int i = 0; i < _Card.WeaponDamageStatInfluences.Length; i++)
			{
				this.DamageStatSum += _Card.WeaponDamageStatInfluences[i].GenerateRandomRange();
			}
		}
		if (_Card.WeaponClashStatInfluences != null)
		{
			for (int j = 0; j < _Card.WeaponClashStatInfluences.Length; j++)
			{
				bool flag = false;
				for (int k = 0; k < this.ClashStatsAddedValues.Count; k++)
				{
					if (this.ClashStatsAddedValues[k].Stat == _Card.WeaponClashStatInfluences[j].Stat)
					{
						StatAndVector2Value value = this.ClashStatsAddedValues[k];
						value.Value += _Card.WeaponClashStatInfluences[j].GenerateRandomRange();
						this.ClashStatsAddedValues[k] = value;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.ClashStatsAddedValues.Add(new StatAndVector2Value(_Card.WeaponClashStatInfluences[j].Stat, _Card.WeaponClashStatInfluences[j].GenerateRandomRange()));
				}
			}
		}
		this.ClashStealthBonus += _Card.ClashStealthBonus;
		this.ClashIneffectiveRangeMalus += _Card.ClashIneffectiveRangeMalus;
		this.ClashEscapeModifier += _Card.ClashVsEscapeBonus;
		this.DmgVsEscapeModifier += _Card.DmgVsEscapeBonus;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00055628 File Offset: 0x00053828
	public EncounterPlayerAction(GenericEncounterPlayerAction _From, string _EnemyName)
	{
		if (_From == null)
		{
			return;
		}
		this.ActionName = _From.ActionName;
		this.ActionSuccessLog = EncounterLogMessage.ApplyFormatArguments(_From.ActionSuccessLog, new object[]
		{
			_EnemyName
		});
		this.ActionFailureLog = EncounterLogMessage.ApplyFormatArguments(_From.ActionFailureLog, new object[]
		{
			_EnemyName
		});
		this.RequiredDistance = _From.RequiredDistance;
		this.DistanceChange = _From.PreClashDistanceChange;
		this.ActionRange = _From.ActionRange;
		this.Reach = _From.Reach;
		this.CannotFailClash = _From.CannotFailClash;
		this.DoesNotAttack = _From.DoesNotAttack;
		this.DontShowSuccessChance = _From.DontShowSuccessChance;
		this.IsEscapeAction = _From.IsEscapeAction;
		this.Clash = _From.InitialClashValue;
		this.ClashInaccuracy = _From.ClashRangedInaccuracy;
		this.Damage = _From.InitialDamage;
		this.DamageStatSum = Vector2.zero;
		if (_From.DamageStatInfluences != null)
		{
			for (int i = 0; i < _From.DamageStatInfluences.Length; i++)
			{
				this.DamageStatSum += _From.DamageStatInfluences[i].GenerateRandomRange();
			}
		}
		if (this.ClashStatsAddedValues == null)
		{
			this.ClashStatsAddedValues = new List<StatAndVector2Value>();
		}
		else
		{
			this.ClashStatsAddedValues.Clear();
		}
		if (_From.ClashStatInfluences != null)
		{
			for (int j = 0; j < _From.ClashStatInfluences.Length; j++)
			{
				this.ClashStatsAddedValues.Add(new StatAndVector2Value(_From.ClashStatInfluences[j].Stat, _From.ClashStatInfluences[j].GenerateRandomRange()));
			}
		}
		this.StatModifiers = _From.ActionStatChanges;
		this.ClashStealthBonus = _From.ClashStealthBonus;
		this.ClashIneffectiveRangeMalus = _From.ClashIneffectiveRangeMalus;
		this.ClashEscapeModifier = _From.ClashVsEscapeModifier;
		this.DmgVsEscapeModifier = _From.DmgVsEscapeModifier;
		this.EncounterResult = _From.EncounterResult;
		this.DamageTypes = _From.DamageTypes;
	}

	// Token: 0x04000DF8 RID: 3576
	public string ActionName;

	// Token: 0x04000DF9 RID: 3577
	public EncounterLogMessage ActionSuccessLog;

	// Token: 0x04000DFA RID: 3578
	public EncounterLogMessage ActionFailureLog;

	// Token: 0x04000DFB RID: 3579
	public EncounterLogMessage WeaponIneffectiveLog;

	// Token: 0x04000DFC RID: 3580
	public EncounterDistanceCondition RequiredDistance;

	// Token: 0x04000DFD RID: 3581
	public EncounterDistanceChange DistanceChange;

	// Token: 0x04000DFE RID: 3582
	public bool CannotFailClash;

	// Token: 0x04000DFF RID: 3583
	public bool DoesNotAttack;

	// Token: 0x04000E00 RID: 3584
	public bool DontShowSuccessChance;

	// Token: 0x04000E01 RID: 3585
	public bool IsEscapeAction;

	// Token: 0x04000E02 RID: 3586
	public ActionRange ActionRange;

	// Token: 0x04000E03 RID: 3587
	public float Reach;

	// Token: 0x04000E04 RID: 3588
	public Vector2 Clash;

	// Token: 0x04000E05 RID: 3589
	private Vector2 ClashInaccuracy;

	// Token: 0x04000E06 RID: 3590
	private Vector2 Damage;

	// Token: 0x04000E07 RID: 3591
	public StatModifier[] StatModifiers;

	// Token: 0x04000E08 RID: 3592
	public List<StatAndVector2Value> ClashStatsAddedValues;

	// Token: 0x04000E09 RID: 3593
	private Vector2 DamageStatSum;

	// Token: 0x04000E0A RID: 3594
	private Vector2 ClashStealthBonus;

	// Token: 0x04000E0B RID: 3595
	private Vector2 ClashIneffectiveRangeMalus;

	// Token: 0x04000E0C RID: 3596
	private Vector2 ClashEscapeModifier;

	// Token: 0x04000E0D RID: 3597
	private Vector2 DmgVsEscapeModifier;

	// Token: 0x04000E0E RID: 3598
	public DamageType[] DamageTypes;

	// Token: 0x04000E0F RID: 3599
	public bool NoAmmo;

	// Token: 0x04000E10 RID: 3600
	public EncounterResult EncounterResult;

	// Token: 0x04000E13 RID: 3603
	public int CurrentAmmoCount;
}
