using System;
using System.Collections.Generic;

// Token: 0x02000029 RID: 41
[Serializable]
public struct EnemyActionSelectionReport
{
	// Token: 0x0600023A RID: 570 RVA: 0x000166AC File Offset: 0x000148AC
	public void FillActionSelectionInfo(InGameEncounter _FromEncounter, List<EnemyAction> _ActionsList)
	{
		if (!_FromEncounter || !_FromEncounter.EncounterModel || _ActionsList == null || _ActionsList.Count == 0)
		{
			this.Actions = new EnemyActionSelectionInfo[0];
			return;
		}
		int num = 0;
		this.Actions = new EnemyActionSelectionInfo[_ActionsList.Count];
		for (int i = 0; i < _ActionsList.Count; i++)
		{
			this.Actions[i] = default(EnemyActionSelectionInfo);
			this.Actions[i].ActionName = _ActionsList[i].ActionLog;
			this.Actions[i].BaseWeight = _ActionsList[i].BaseWeight;
			this.Actions[i].DistanceWeightMod = (_FromEncounter.Distant ? _ActionsList[i].DistanceWeightModifier : 0);
			this.Actions[i].CloseWeightMod = (_FromEncounter.Distant ? 0 : _ActionsList[i].CloseRangeWeightModifier);
			this.Actions[i].EnemyHiddenWeightMod = (_FromEncounter.EnemyHidden ? _ActionsList[i].EnemyHiddenWeightModifier : 0);
			this.Actions[i].PlayerHiddenWeightMod = (_FromEncounter.PlayerHidden ? _ActionsList[i].PlayerHiddenWeightModifier : 0);
			this.Actions[i].StatWeightMods = new List<StatDropWeightModReport>();
			_ActionsList[i].GetStatWeightMods(_FromEncounter.EncounterModel, this.Actions[i].StatWeightMods, true);
			this.Actions[i].CardWeightMods = new List<CardDropWeightModReport>();
			_ActionsList[i].GetCardWeightMods(this.Actions[i].CardWeightMods, true);
			this.Actions[i].ValuesWeightMods = new EnemyValuesWeightModReport(_ActionsList[i].ValuesWeightModifiers, _FromEncounter);
			this.Actions[i].WoundsWeightMods = new EnemyWoundsWeightModReport(_ActionsList[i].WoundsWeightModifiers, _FromEncounter);
			if (this.Actions[i].FinalWeight <= 0)
			{
				this.Actions[i].RangeUpTo = -1;
			}
			else
			{
				num += this.Actions[i].FinalWeight;
				this.Actions[i].RangeUpTo = num;
			}
		}
		this.TotalWeight = num;
	}

	// Token: 0x04000245 RID: 581
	public int TotalWeight;

	// Token: 0x04000246 RID: 582
	public EnemyActionSelectionInfo[] Actions;

	// Token: 0x04000247 RID: 583
	public float RandomValue;

	// Token: 0x04000248 RID: 584
	public int SelectedAction;
}
