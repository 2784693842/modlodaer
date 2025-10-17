using System;
using UnityEngine;

// Token: 0x02000135 RID: 309
[Serializable]
public struct EnemyValueToStatModifier
{
	// Token: 0x0600090D RID: 2317 RVA: 0x00055EAC File Offset: 0x000540AC
	public StatModifier ToModifier(InGameEncounter _Encounter)
	{
		StatModifier result = default(StatModifier);
		if (this.ToStat == null)
		{
			return result;
		}
		result.Stat = this.ToStat;
		switch (this.TransferType)
		{
		case EnemyValueToStatModifier.TransferOperation.AddValueToStat:
			result.ValueModifier = Vector2.one * _Encounter.GetEnemyValue(this.FromValue);
			break;
		case EnemyValueToStatModifier.TransferOperation.SubtractValueFromStat:
			result.ValueModifier = Vector2.one * -_Encounter.GetEnemyValue(this.FromValue);
			break;
		case EnemyValueToStatModifier.TransferOperation.SetStatToValue:
			if (!MBSingleton<GameManager>.Instance)
			{
				result.ValueModifier = Vector2.zero;
			}
			else if (MBSingleton<GameManager>.Instance.StatsDict.ContainsKey(this.ToStat))
			{
				result.ValueModifier = Vector2.one * (_Encounter.GetEnemyValue(this.FromValue) - MBSingleton<GameManager>.Instance.StatsDict[this.ToStat].SimpleCurrentValue);
			}
			break;
		}
		return result;
	}

	// Token: 0x04000E61 RID: 3681
	public EnemyValueNames FromValue;

	// Token: 0x04000E62 RID: 3682
	public GameStat ToStat;

	// Token: 0x04000E63 RID: 3683
	public EnemyValueToStatModifier.TransferOperation TransferType;

	// Token: 0x02000292 RID: 658
	public enum TransferOperation
	{
		// Token: 0x0400151E RID: 5406
		AddValueToStat,
		// Token: 0x0400151F RID: 5407
		SubtractValueFromStat,
		// Token: 0x04001520 RID: 5408
		SetStatToValue
	}
}
