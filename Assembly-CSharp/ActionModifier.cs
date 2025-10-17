using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020000D2 RID: 210
[Serializable]
public class ActionModifier
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x0004AEE3 File Offset: 0x000490E3
	// (set) Token: 0x06000797 RID: 1943 RVA: 0x0004AEEB File Offset: 0x000490EB
	public string Source { get; private set; }

	// Token: 0x06000798 RID: 1944 RVA: 0x0004AEF4 File Offset: 0x000490F4
	public bool AppliesToAction(CardAction _Action, bool notInBase, InGameCardBase _FromCard)
	{
		if (_Action == null)
		{
			return false;
		}
		if (this.AppliesTo == null)
		{
			return true;
		}
		if (this.AppliesTo.Count == 0)
		{
			return true;
		}
		if (_Action.ActionTags == null)
		{
			return false;
		}
		if (_Action.ActionTags.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < _Action.ActionTags.Length; i++)
		{
			if (_Action.ActionTags[i] && this.AppliesTo.Contains(_Action.ActionTags[i]))
			{
				return this.Conditions.ConditionsValid(notInBase, _FromCard);
			}
		}
		return false;
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0004AF7A File Offset: 0x0004917A
	public void SetSource(string _Source)
	{
		this.Source = _Source;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0004AF83 File Offset: 0x00049183
	public static string SourceFromPerk(CharacterPerk _Perk, int _ModifierIndex)
	{
		StringBuilder stringBuilder = new StringBuilder(_Perk.name);
		stringBuilder.Append("_");
		stringBuilder.Append(_ModifierIndex.ToString());
		return stringBuilder.ToString();
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0004AFB0 File Offset: 0x000491B0
	public static string SourceFromStatus(StatStatus _Status, int _ModifierIndex)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (_Status.ParentStat != null)
		{
			stringBuilder.Append(_Status.ParentStat.name);
			stringBuilder.Append("_");
		}
		if (!string.IsNullOrEmpty(_Status.GameName))
		{
			stringBuilder.Append(_Status.GameName.ToString());
		}
		else if (!Mathf.Approximately((float)_Status.ValueRange.x, (float)_Status.ValueRange.y))
		{
			stringBuilder.Append(((float)_Status.ValueRange.x + (float)(_Status.ValueRange.y - _Status.ValueRange.x) * 0.5f).ToString());
		}
		else
		{
			stringBuilder.Append(_Status.ValueRange.x.ToString());
		}
		stringBuilder.Append("_");
		stringBuilder.Append(_ModifierIndex.ToString());
		return stringBuilder.ToString();
	}

	// Token: 0x04000B21 RID: 2849
	public GeneralCondition Conditions;

	// Token: 0x04000B22 RID: 2850
	public List<ActionTag> AppliesTo;

	// Token: 0x04000B23 RID: 2851
	public bool BlocksAction;

	// Token: 0x04000B24 RID: 2852
	public LocalizedString ActionBlockMessage;

	// Token: 0x04000B25 RID: 2853
	public int DurationModifier;

	// Token: 0x04000B26 RID: 2854
	[StatModifierOptions(false, true)]
	public StatModifier[] AddedStatModifiers;

	// Token: 0x04000B27 RID: 2855
	public AddedDurabilityModifier ReceivingDurabilities;

	// Token: 0x04000B28 RID: 2856
	public AddedDurabilityModifier GivenDurabilities;
}
