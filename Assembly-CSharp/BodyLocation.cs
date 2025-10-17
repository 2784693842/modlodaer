using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000BE RID: 190
[Serializable]
public class BodyLocation
{
	// Token: 0x0600074B RID: 1867 RVA: 0x000489B1 File Offset: 0x00046BB1
	public float GetArmor(DamageType[] _Types)
	{
		return BodyTemplate.CalculateArmor(this.Armor, _Types, this.ArmorModifiers);
	}

	// Token: 0x04000A05 RID: 2565
	public float MeleeHitChanceWeight;

	// Token: 0x04000A06 RID: 2566
	public float RangedHitChanceWeight;

	// Token: 0x04000A07 RID: 2567
	[FormerlySerializedAs("BaseDefense")]
	[SerializeField]
	private float Armor;

	// Token: 0x04000A08 RID: 2568
	[SerializeField]
	private DamageTypeArmorModifier[] ArmorModifiers;
}
