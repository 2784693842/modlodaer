using System;

// Token: 0x02000071 RID: 113
public struct EnemyWoundSelectionReport
{
	// Token: 0x060004A4 RID: 1188 RVA: 0x0002FDE5 File Offset: 0x0002DFE5
	public EnemyWoundSelectionReport(EnemyWound[] _Wounds, int _Selected)
	{
		if (_Wounds == null)
		{
			this.AllWounds = null;
			this.SelectedWound = -1;
			return;
		}
		this.AllWounds = new EnemyWound[_Wounds.Length];
		_Wounds.CopyTo(this.AllWounds, 0);
		this.SelectedWound = _Selected;
	}

	// Token: 0x040005C4 RID: 1476
	public EnemyWound[] AllWounds;

	// Token: 0x040005C5 RID: 1477
	public int SelectedWound;
}
