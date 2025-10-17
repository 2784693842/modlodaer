using System;
using System.Text;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public struct StatModifierReport
{
	// Token: 0x06000C84 RID: 3204 RVA: 0x00066CC4 File Offset: 0x00064EC4
	public static string SourceFromStatStatus(string _StatusName, InGameStat _FromStat, bool _Entering)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_Entering ? "<b>ENTERING STATUS</b> | " : "<b>LEAVING STATUS</b> | ");
		if (string.IsNullOrEmpty(_StatusName))
		{
			stringBuilder.Append("Unnamed status");
		}
		else
		{
			stringBuilder.Append(_StatusName);
		}
		stringBuilder.Append(" on ");
		if (_FromStat)
		{
			stringBuilder.Append(_FromStat.name);
		}
		else
		{
			stringBuilder.Append("Unkown Stat");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x00066D40 File Offset: 0x00064F40
	public static string SourceFromPassiveEffect(string _EffectName, InGameCardBase _FromCard)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<b>PASSIVE EFFECT</b> | ");
		if (string.IsNullOrEmpty(_EffectName))
		{
			stringBuilder.Append("Unnamed effect");
		}
		else
		{
			stringBuilder.Append(_EffectName);
		}
		stringBuilder.Append(" on ");
		if (_FromCard)
		{
			stringBuilder.Append(_FromCard.name);
		}
		else
		{
			stringBuilder.Append("Unkown Source");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x00066DB4 File Offset: 0x00064FB4
	public static string SourceFromAction(CardAction _Action, InGameCardBase _FromCard)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<b>ACTION</b> | ");
		if (string.IsNullOrEmpty(_Action.ActionName))
		{
			stringBuilder.Append("Unnamed action");
		}
		else
		{
			stringBuilder.Append(_Action.ActionName);
		}
		stringBuilder.Append(" on ");
		if (_FromCard)
		{
			stringBuilder.Append(_FromCard.name);
		}
		else
		{
			stringBuilder.Append("Self Triggered Action");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00066E3C File Offset: 0x0006503C
	public static string SourceFromGamemode(Gamemode _Mode)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<b>GAMEMODE</b> | ");
		if (string.IsNullOrEmpty(_Mode.ModeName))
		{
			stringBuilder.Append("Unnamed gamemode");
		}
		else
		{
			stringBuilder.Append(_Mode.ModeName);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x00066E94 File Offset: 0x00065094
	public static string SourceFromPlayerCharacter(PlayerCharacter _Character)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<b>CHARACTER</b> | ");
		if (string.IsNullOrEmpty(_Character.CharacterName))
		{
			stringBuilder.Append("Unnamed character");
		}
		else
		{
			stringBuilder.Append(_Character.CharacterName);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00066EEC File Offset: 0x000650EC
	public static string SourceFromPerk(CharacterPerk _Perk)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<b>PERK</b> | ");
		if (string.IsNullOrEmpty(_Perk.PerkName))
		{
			stringBuilder.Append("Unnamed perk");
		}
		else
		{
			stringBuilder.Append(_Perk.PerkName);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00066F44 File Offset: 0x00065144
	public string EffectName
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (string.IsNullOrEmpty(this.ModifierSource))
			{
				stringBuilder.Append("UNKOWN EFFECT");
			}
			else
			{
				stringBuilder.Append(this.ModifierSource);
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00066F88 File Offset: 0x00065188
	public string StatName
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Value != 0f || this.Rate != 0f)
			{
				switch (this.ModificationType)
				{
				case StatModification.Permanent:
					stringBuilder.Append(this.IsInverse ? "<color=blue>Permanently modified (as inverse)</color> <b>" : "<color=blue>Permanently modified </color> <b>");
					break;
				case StatModification.GlobalModifier:
					stringBuilder.Append(this.IsInverse ? "<color=red>Removed global effect from </color> <b>" : "<color=green>Added global effect to </color> <b>");
					break;
				case StatModification.AtBaseModifier:
					stringBuilder.Append(this.IsInverse ? "<color=red>Removed at base effect from </color> <b>" : "<color=green>Added at base effect to </color> <b>");
					break;
				}
			}
			else
			{
				stringBuilder.Append("<color=grey>Did nothing on <b>");
			}
			stringBuilder.Append(this.Stat.StatModel.name);
			if (this.Value != 0f || this.Rate != 0f)
			{
				switch (this.ModificationType)
				{
				case StatModification.Permanent:
					stringBuilder.Append("</b> (BASE)");
					break;
				case StatModification.GlobalModifier:
					stringBuilder.Append("</b> (GLOBAL MODIFIED)");
					break;
				case StatModification.AtBaseModifier:
					stringBuilder.Append("</b> (AT BASE MODIFIED)");
					break;
				}
			}
			else
			{
				stringBuilder.Append("</b></color>");
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06000C8C RID: 3212 RVA: 0x000670BC File Offset: 0x000652BC
	public string ReportEffects
	{
		get
		{
			if (this.Value == 0f && this.Rate == 0f)
			{
				return "<color=grey>NO EFFECTS</color>";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Value != 0f)
			{
				switch (this.ModificationType)
				{
				case StatModification.Permanent:
					stringBuilder.Append("Base Value: ");
					break;
				case StatModification.GlobalModifier:
					stringBuilder.Append("Global Modified Value: ");
					break;
				case StatModification.AtBaseModifier:
					stringBuilder.Append("At Base Modified Value: ");
					break;
				}
				if (this.Value > 0f)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.Value.ToString());
			}
			else
			{
				switch (this.ModificationType)
				{
				case StatModification.Permanent:
					stringBuilder.Append("<color=grey>Base Value: 0</color>");
					break;
				case StatModification.GlobalModifier:
					stringBuilder.Append("<color=grey>Global Modified Value: 0</color>");
					break;
				case StatModification.AtBaseModifier:
					stringBuilder.Append("<color=grey>At Base Modified Value: 0</color>");
					break;
				}
			}
			if (this.Rate != 0f)
			{
				switch (this.ModificationType)
				{
				case StatModification.Permanent:
					stringBuilder.Append(" | Base Rate: ");
					break;
				case StatModification.GlobalModifier:
					stringBuilder.Append(" | Global Modified Rate: ");
					break;
				case StatModification.AtBaseModifier:
					stringBuilder.Append(" | At Base Modified Rate: ");
					break;
				}
				if (this.Rate > 0f)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.Rate.ToString());
			}
			else
			{
				switch (this.ModificationType)
				{
				case StatModification.Permanent:
					stringBuilder.Append(" | <color=grey>Base Rate: 0</color>");
					break;
				case StatModification.GlobalModifier:
					stringBuilder.Append(" | <color=grey>Global Modified Rate: 0</color>");
					break;
				case StatModification.AtBaseModifier:
					stringBuilder.Append(" | <color=grey>At Base Modified Rate: 0</color>");
					break;
				}
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06000C8D RID: 3213 RVA: 0x00067275 File Offset: 0x00065475
	public string Summary
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.EffectName);
			stringBuilder.Append("\n");
			stringBuilder.Append(this.ReportEffects);
			return stringBuilder.ToString();
		}
	}

	// Token: 0x04001170 RID: 4464
	public Vector3Int TickInfo;

	// Token: 0x04001171 RID: 4465
	public string ModifierSource;

	// Token: 0x04001172 RID: 4466
	public InGameStat Stat;

	// Token: 0x04001173 RID: 4467
	public float Value;

	// Token: 0x04001174 RID: 4468
	public float Rate;

	// Token: 0x04001175 RID: 4469
	public bool IsInverse;

	// Token: 0x04001176 RID: 4470
	public StatModification ModificationType;
}
