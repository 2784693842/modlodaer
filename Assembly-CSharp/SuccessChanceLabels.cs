using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000157 RID: 343
[CreateAssetMenu(menuName = "Survival/Success Chance Labels")]
public class SuccessChanceLabels : ScriptableObject
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x0600098C RID: 2444 RVA: 0x0005895C File Offset: 0x00056B5C
	public float ConfidenceModifier
	{
		get
		{
			if (this.AllConfidenceMods == null)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.AllConfidenceMods.Count; i++)
			{
				num += this.AllConfidenceMods[i].AddedValue;
			}
			return num;
		}
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x000589A8 File Offset: 0x00056BA8
	public void Init()
	{
		this.AllConfidenceMods = new List<SuccessChanceLabels.StatConfidenceModifier>();
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x000589B8 File Offset: 0x00056BB8
	public void AddConfidenceMod(GameStat _Stat, float _Value)
	{
		if (this.AllConfidenceMods == null)
		{
			this.AllConfidenceMods = new List<SuccessChanceLabels.StatConfidenceModifier>();
		}
		if (this.AllConfidenceMods.Count == 0)
		{
			this.AllConfidenceMods.Add(new SuccessChanceLabels.StatConfidenceModifier
			{
				FromStat = _Stat,
				AddedValue = _Value
			});
			return;
		}
		for (int i = 0; i < this.AllConfidenceMods.Count; i++)
		{
			if (this.AllConfidenceMods[i].FromStat == _Stat)
			{
				SuccessChanceLabels.StatConfidenceModifier value = this.AllConfidenceMods[i];
				value.AddedValue += _Value;
				this.AllConfidenceMods[i] = value;
				return;
			}
		}
		this.AllConfidenceMods.Add(new SuccessChanceLabels.StatConfidenceModifier
		{
			FromStat = _Stat,
			AddedValue = _Value
		});
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00058A84 File Offset: 0x00056C84
	public void RemoveConfidenceMod(GameStat _Stat, float _Value)
	{
		if (this.AllConfidenceMods == null)
		{
			this.AllConfidenceMods = new List<SuccessChanceLabels.StatConfidenceModifier>();
		}
		if (this.AllConfidenceMods.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.AllConfidenceMods.Count; i++)
		{
			if (this.AllConfidenceMods[i].FromStat == _Stat)
			{
				SuccessChanceLabels.StatConfidenceModifier value = this.AllConfidenceMods[i];
				value.AddedValue -= _Value;
				this.AllConfidenceMods[i] = value;
				return;
			}
		}
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00058B08 File Offset: 0x00056D08
	public string GetSuccessLabel(float _ChanceValue)
	{
		if (Mathf.Abs(this.ConfidenceModifier) < 0.001f)
		{
			return this.GetLabel(_ChanceValue, this.SuccessLabels);
		}
		string label = this.GetLabel(_ChanceValue + this.ConfidenceModifier, this.ModifiedSuccessLabels);
		if (!string.IsNullOrEmpty(label))
		{
			return label;
		}
		return this.GetLabel(_ChanceValue, this.SuccessLabels);
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00058B61 File Offset: 0x00056D61
	public string GetContributionLabel(float _ChanceValue)
	{
		return this.GetLabel(_ChanceValue, this.ContributionLabels);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x00058B70 File Offset: 0x00056D70
	public string GetLabel(float _ChanceValue, SuccessChanceLabels.ChanceLabel[] _Labels)
	{
		if (_Labels == null)
		{
			return "";
		}
		if (_Labels.Length == 0)
		{
			return "";
		}
		for (int i = 0; i < _Labels.Length; i++)
		{
			if (_Labels[i].MaxChance >= 1f || _Labels[i].MaxChance <= 0f || _ChanceValue < _Labels[i].MaxChance)
			{
				return _Labels[i].LabelText;
			}
		}
		return "";
	}

	// Token: 0x04000F1D RID: 3869
	[FormerlySerializedAs("AllLabels")]
	public SuccessChanceLabels.ChanceLabel[] SuccessLabels;

	// Token: 0x04000F1E RID: 3870
	public SuccessChanceLabels.ChanceLabel[] ModifiedSuccessLabels;

	// Token: 0x04000F1F RID: 3871
	public SuccessChanceLabels.ChanceLabel[] ContributionLabels;

	// Token: 0x04000F20 RID: 3872
	private List<SuccessChanceLabels.StatConfidenceModifier> AllConfidenceMods;

	// Token: 0x02000294 RID: 660
	[Serializable]
	public struct ChanceLabel
	{
		// Token: 0x04001528 RID: 5416
		public float MaxChance;

		// Token: 0x04001529 RID: 5417
		public LocalizedString LabelText;
	}

	// Token: 0x02000295 RID: 661
	private struct StatConfidenceModifier
	{
		// Token: 0x0400152A RID: 5418
		public GameStat FromStat;

		// Token: 0x0400152B RID: 5419
		public float AddedValue;
	}
}
