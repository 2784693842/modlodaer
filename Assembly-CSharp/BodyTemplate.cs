using System;
using UnityEngine;

// Token: 0x020000BC RID: 188
[CreateAssetMenu(menuName = "Survival/Combat/Body Template")]
public class BodyTemplate : ScriptableObject
{
	// Token: 0x06000747 RID: 1863 RVA: 0x0004885C File Offset: 0x00046A5C
	public EnemyBodyLocation GetBodyLocation(BodyLocations _BodyLocation)
	{
		switch (_BodyLocation)
		{
		case BodyLocations.Head:
			return this.Head;
		case BodyLocations.Torso:
			return this.Torso;
		case BodyLocations.LArm:
			return this.LArm;
		case BodyLocations.RArm:
			return this.RArm;
		case BodyLocations.LLeg:
			return this.LLeg;
		case BodyLocations.RLeg:
			return this.RLeg;
		default:
			return this.Torso;
		}
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x000488BC File Offset: 0x00046ABC
	public static string LocationName(BodyLocations _Location)
	{
		switch (_Location)
		{
		case BodyLocations.Head:
			return "Head";
		case BodyLocations.Torso:
			return "Torso";
		case BodyLocations.LArm:
			return "Left Arm";
		case BodyLocations.RArm:
			return "Right Arm";
		case BodyLocations.LLeg:
			return "Left Leg";
		case BodyLocations.RLeg:
			return "Right Leg";
		default:
			return "NONE";
		}
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00048914 File Offset: 0x00046B14
	public static float CalculateArmor(float _Armor, DamageType[] _DamageTypes, DamageTypeArmorModifier[] _ArmorModifiers)
	{
		float num = float.MaxValue;
		if (_DamageTypes != null && _DamageTypes.Length != 0 && _ArmorModifiers != null && _ArmorModifiers.Length != 0)
		{
			for (int i = 0; i < _DamageTypes.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < _ArmorModifiers.Length; j++)
				{
					if (_DamageTypes[i] == _ArmorModifiers[j].DmgType)
					{
						flag = true;
						if (_ArmorModifiers[j].Modifier < num)
						{
							num = _ArmorModifiers[j].Modifier;
						}
					}
				}
				if (!flag && num > 0f)
				{
					num = 0f;
				}
			}
		}
		else
		{
			num = 0f;
		}
		if (num > 1000000f)
		{
			num = 0f;
		}
		return _Armor + num;
	}

	// Token: 0x040009F6 RID: 2550
	public EnemyBodyLocation Head;

	// Token: 0x040009F7 RID: 2551
	public EnemyBodyLocation Torso;

	// Token: 0x040009F8 RID: 2552
	public EnemyBodyLocation LArm;

	// Token: 0x040009F9 RID: 2553
	public EnemyBodyLocation RArm;

	// Token: 0x040009FA RID: 2554
	public EnemyBodyLocation LLeg;

	// Token: 0x040009FB RID: 2555
	public EnemyBodyLocation RLeg;

	// Token: 0x040009FC RID: 2556
	public EnemyWound DefaultWound;
}
