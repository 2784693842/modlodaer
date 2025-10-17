using System;
using System.Collections.Generic;

// Token: 0x020000BF RID: 191
[Serializable]
public class EnemyBodyLocation : BodyLocation
{
	// Token: 0x0600074D RID: 1869 RVA: 0x000489C8 File Offset: 0x00046BC8
	public EnemyWound[] GetWoundsForSeverityDamageType(WoundSeverity _WoundSeverity, DamageType[] _DamageTypes)
	{
		switch (_WoundSeverity)
		{
		case WoundSeverity.NoWound:
			return this.GetWoundsForDamageType(this.UnharmedResults, _DamageTypes);
		case WoundSeverity.Minor:
			return this.GetWoundsForDamageType(this.MinorWounds, _DamageTypes);
		case WoundSeverity.Medium:
			return this.GetWoundsForDamageType(this.MediumWounds, _DamageTypes);
		case WoundSeverity.Serious:
			return this.GetWoundsForDamageType(this.SeriousWounds, _DamageTypes);
		default:
			return this.GetWoundsForDamageType(this.UnharmedResults, _DamageTypes);
		}
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00048A34 File Offset: 0x00046C34
	private EnemyWound[] GetWoundsForDamageType(EnemyWound[] _Wounds, DamageType[] _DamageTypes)
	{
		if (_Wounds == null || _Wounds.Length == 0)
		{
			return null;
		}
		List<EnemyWound> list = new List<EnemyWound>();
		for (int i = 0; i < _Wounds.Length; i++)
		{
			EnemyWound enemyWound = _Wounds[i];
			if (enemyWound.RequiredDamageTypes == null || enemyWound.RequiredDamageTypes.Length == 0)
			{
				list.Add(_Wounds[i]);
			}
			else if (_DamageTypes != null && _DamageTypes.Length != 0)
			{
				int num = 0;
				while (num < enemyWound.RequiredDamageTypes.Length && !list.Contains(_Wounds[i]))
				{
					for (int j = 0; j < _DamageTypes.Length; j++)
					{
						if (_DamageTypes[j] == enemyWound.RequiredDamageTypes[num])
						{
							list.Add(_Wounds[i]);
							break;
						}
					}
					num++;
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x04000A09 RID: 2569
	public EnemyWound[] UnharmedResults;

	// Token: 0x04000A0A RID: 2570
	public EnemyWound[] MinorWounds;

	// Token: 0x04000A0B RID: 2571
	public EnemyWound[] MediumWounds;

	// Token: 0x04000A0C RID: 2572
	public EnemyWound[] SeriousWounds;
}
