using System;
using System.Collections.Generic;

// Token: 0x02000070 RID: 112
public struct PlayerWoundSelectionReport
{
	// Token: 0x060004A3 RID: 1187 RVA: 0x0002FCE4 File Offset: 0x0002DEE4
	public void FillPlayerWoundSelectionInfo(List<PlayerWound> _PlayerWounds)
	{
		if (_PlayerWounds == null || _PlayerWounds.Count == 0)
		{
			this.PlayerWounds = new PlayerWoundSelectionInfo[0];
			return;
		}
		float num = 0f;
		this.PlayerWounds = new PlayerWoundSelectionInfo[_PlayerWounds.Count];
		for (int i = 0; i < _PlayerWounds.Count; i++)
		{
			this.PlayerWounds[i] = default(PlayerWoundSelectionInfo);
			this.PlayerWounds[i].WoundName = _PlayerWounds[i].WoundLog.MainLogDefaultText;
			this.PlayerWounds[i].BaseWeight = _PlayerWounds[i].WoundSelectionWeight;
			if (this.PlayerWounds[i].BaseWeight <= 0f)
			{
				this.PlayerWounds[i].RangeUpTo = -1f;
			}
			else
			{
				num += this.PlayerWounds[i].BaseWeight;
				this.PlayerWounds[i].RangeUpTo = num;
			}
		}
		this.TotalWeight = num;
	}

	// Token: 0x040005C0 RID: 1472
	public float TotalWeight;

	// Token: 0x040005C1 RID: 1473
	public PlayerWoundSelectionInfo[] PlayerWounds;

	// Token: 0x040005C2 RID: 1474
	public float RandomValue;

	// Token: 0x040005C3 RID: 1475
	public int SelectedWound;
}
