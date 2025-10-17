using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class InGameStatInfo
{
	// Token: 0x06000CA7 RID: 3239 RVA: 0x0006785B File Offset: 0x00065A5B
	public InGameStatInfo(InGameStat _Stat)
	{
		this.Stat = _Stat;
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0006788C File Offset: 0x00065A8C
	public void ApplyModifier(StatModifierReport _Report)
	{
		bool flag = false;
		if (this.Stat && this.Stat.StatModel)
		{
			flag = this.Stat.StatModel.StatusDebugMode;
		}
		if (flag)
		{
			Debug.Log(this.Stat.name + " - Trying to add report: " + _Report.ModifierSource);
		}
		if (_Report.ModificationType == StatModification.Permanent)
		{
			if (flag)
			{
				Debug.Log("Adding to permanent modifiers");
			}
			this.PermanentModifiers.Add(_Report);
			return;
		}
		if (!_Report.IsInverse)
		{
			if (flag)
			{
				Debug.Log("Adding to current modifiers");
			}
			this.CurrentModifiers.Add(_Report);
			return;
		}
		if (this.CurrentModifiers.Count > 0)
		{
			for (int i = 0; i < this.CurrentModifiers.Count; i++)
			{
				if (this.GetEffectName(this.CurrentModifiers[i].EffectName) == this.GetEffectName(_Report.EffectName) && Mathf.Approximately(_Report.Rate + this.CurrentModifiers[i].Rate, 0f) && Mathf.Approximately(_Report.Value + this.CurrentModifiers[i].Value, 0f))
				{
					if (flag)
					{
						Debug.Log("Report is inverse, removing: " + this.CurrentModifiers[i].ModifierSource);
					}
					this.CurrentModifiers.RemoveAt(i);
					return;
				}
			}
		}
		if (flag)
		{
			Debug.Log("Couldn't find a report to remove, storing in unmet cancels");
		}
		this.UnmetCancels.Add(_Report);
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x00067A20 File Offset: 0x00065C20
	private string GetEffectName(string _Name)
	{
		string text = _Name;
		while (text.Length > 0 && !text.StartsWith("| "))
		{
			text = text.Remove(0, 1);
		}
		if (text.Length == 0)
		{
			return _Name;
		}
		return text.Remove(0, 2);
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00067A64 File Offset: 0x00065C64
	public string Title
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT :(";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Stat.name);
			if (this.Expanded)
			{
				stringBuilder.Append(" <size=8>(<i>Click to collapse</i>)</size>");
			}
			else
			{
				stringBuilder.Append(" <size=8>(<i>Click to expand</i>)</size>");
			}
			stringBuilder.Append("\nValue: ");
			stringBuilder.Append(this.Stat.CurrentValue(MBSingleton<GameManager>.Instance.NotInBase).ToString("0.##"));
			stringBuilder.Append(" | Rate: ");
			stringBuilder.Append(this.Stat.CurrentRatePerTick(MBSingleton<GameManager>.Instance.NotInBase).ToString("0.##"));
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00067B30 File Offset: 0x00065D30
	public string MinMaxValues
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			if (!this.Stat.StatModel)
			{
				return "NO STAT MODEL!";
			}
			StringBuilder stringBuilder = new StringBuilder("<color=grey>");
			stringBuilder.Append("Min/Max Value: ");
			stringBuilder.Append(this.Stat.StatModel.MinMaxValue.x.ToString("0.##"));
			stringBuilder.Append("|");
			stringBuilder.Append(this.Stat.StatModel.MinMaxValue.y.ToString("0.##"));
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00067BE8 File Offset: 0x00065DE8
	public string MinMaxRates
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			if (!this.Stat.StatModel)
			{
				return "NO STAT MODEL!";
			}
			StringBuilder stringBuilder = new StringBuilder("<color=grey>");
			stringBuilder.Append("Min/Max Rate: ");
			stringBuilder.Append(this.Stat.StatModel.MinMaxRate.x.ToString("0.##"));
			stringBuilder.Append("|");
			stringBuilder.Append(this.Stat.StatModel.MinMaxRate.y.ToString("0.##"));
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00067CA0 File Offset: 0x00065EA0
	public string BaseValue
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			StringBuilder stringBuilder = new StringBuilder("<color=grey>");
			stringBuilder.Append("Base Value: ");
			stringBuilder.Append(this.Stat.CurrentBaseValue.ToString("0.##"));
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00067D04 File Offset: 0x00065F04
	public string GlobalModifiedValue
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			StringBuilder stringBuilder = new StringBuilder("<color=grey>");
			stringBuilder.Append("Global Modified Value: ");
			stringBuilder.Append(this.Stat.GlobalModifiedValue.ToString("0.##"));
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00067D68 File Offset: 0x00065F68
	public string AtBaseModifiedValue
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			StringBuilder stringBuilder = new StringBuilder("<color=grey>");
			stringBuilder.Append("At Base Modified Value: ");
			stringBuilder.Append(this.Stat.AtBaseModifiedValue.ToString("0.##"));
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00067DCC File Offset: 0x00065FCC
	public string BaseRate
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			StringBuilder stringBuilder = new StringBuilder("<color=grey>");
			stringBuilder.Append("Base Rate: ");
			stringBuilder.Append(this.Stat.CurrentBaseRate.ToString("0.##"));
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00067E30 File Offset: 0x00066030
	public string GlobalModifiedRate
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			StringBuilder stringBuilder = new StringBuilder("<color=grey>");
			stringBuilder.Append("Global Modified Rate: ");
			stringBuilder.Append(this.Stat.GlobalModifiedRate.ToString("0.##"));
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00067E94 File Offset: 0x00066094
	public string AtBaseModifiedRate
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			StringBuilder stringBuilder = new StringBuilder("<color=grey>");
			stringBuilder.Append("At Base Modified Rate: ");
			stringBuilder.Append(this.Stat.AtBaseModifiedRate.ToString("0.##"));
			stringBuilder.Append("</color>");
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00067EF8 File Offset: 0x000660F8
	public string StatusList
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.Stat.CurrentStatuses.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("\n");
				}
				stringBuilder.Append((!string.IsNullOrEmpty(this.Stat.CurrentStatuses[i].GameName)) ? this.Stat.CurrentStatuses[i].GameName : "Unnamed status");
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x00067F9C File Offset: 0x0006619C
	public string ModifiersList
	{
		get
		{
			if (!this.Stat)
			{
				return "NO STAT";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.CurrentModifiers.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("\n----\n");
				}
				stringBuilder.Append(this.CurrentModifiers[i].Summary);
			}
			if (this.UnmetCancels.Count > 0)
			{
				if (this.CurrentModifiers.Count > 0)
				{
					stringBuilder.Append("\n----\n");
				}
				stringBuilder.Append("<b>Unmatched cancelled effects</b>\n");
				for (int j = 0; j < this.UnmetCancels.Count; j++)
				{
					stringBuilder.Append(this.UnmetCancels[j].Summary);
				}
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x04001199 RID: 4505
	public InGameStat Stat;

	// Token: 0x0400119A RID: 4506
	public bool Expanded;

	// Token: 0x0400119B RID: 4507
	public List<StatModifierReport> CurrentModifiers = new List<StatModifierReport>();

	// Token: 0x0400119C RID: 4508
	public List<StatModifierReport> PermanentModifiers = new List<StatModifierReport>();

	// Token: 0x0400119D RID: 4509
	public List<StatModifierReport> UnmetCancels = new List<StatModifierReport>();
}
