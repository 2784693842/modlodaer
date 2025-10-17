using System;
using System.Text;
using UnityEngine;

// Token: 0x0200006F RID: 111
[Serializable]
public struct PlayerWoundSelectionInfo
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0002FBA7 File Offset: 0x0002DDA7
	public float FinalWeight
	{
		get
		{
			return this.BaseWeight;
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0002FBB0 File Offset: 0x0002DDB0
	public string SimpleSummary(int _Index, float _PrevRange, float _TotalWeight, bool _Selected)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_Index.ToString());
		stringBuilder.Append("-");
		stringBuilder.Append(string.IsNullOrEmpty(this.WoundName) ? "Unnamed Wound" : this.WoundName);
		if (_Selected)
		{
			stringBuilder.Append(" SELECTED!");
		}
		stringBuilder.Append("\nRange: ");
		if (this.RangeUpTo > 0f)
		{
			stringBuilder.Append(_PrevRange.ToString());
			stringBuilder.Append("_");
			stringBuilder.Append(this.RangeUpTo.ToString());
		}
		else
		{
			stringBuilder.Append("<color=red>");
			stringBuilder.Append(this.RangeUpTo.ToString());
			stringBuilder.Append(" (OUT)</color>");
		}
		stringBuilder.Append(" | Weight: ");
		stringBuilder.Append(this.BaseWeight.ToString());
		stringBuilder.Append(" (");
		stringBuilder.Append(Mathf.Max(0f, this.FinalWeight / _TotalWeight).ToString("0.###%"));
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	// Token: 0x040005BD RID: 1469
	public string WoundName;

	// Token: 0x040005BE RID: 1470
	public float BaseWeight;

	// Token: 0x040005BF RID: 1471
	public float RangeUpTo;
}
