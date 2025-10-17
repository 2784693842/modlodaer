using System;
using UnityEngine;

// Token: 0x020000C5 RID: 197
[Serializable]
public class ArmorValues
{
	// Token: 0x06000752 RID: 1874 RVA: 0x00048BD8 File Offset: 0x00046DD8
	public float CalculateArmorForLocation(DamageType[] _DamageTypes, BodyLocations _BodyLocation)
	{
		switch (_BodyLocation)
		{
		case BodyLocations.Head:
			return BodyTemplate.CalculateArmor(this.HeadArmor, _DamageTypes, this.HeadArmorModifiers);
		case BodyLocations.Torso:
			return BodyTemplate.CalculateArmor(this.TorsoArmor, _DamageTypes, this.TorsoArmorModifiers);
		case BodyLocations.LArm:
			return BodyTemplate.CalculateArmor(this.LArmArmor, _DamageTypes, this.LArmArmorModifiers);
		case BodyLocations.RArm:
			return BodyTemplate.CalculateArmor(this.RArmArmor, _DamageTypes, this.RArmArmorModifiers);
		case BodyLocations.LLeg:
			return BodyTemplate.CalculateArmor(this.LLegArmor, _DamageTypes, this.LLegArmorModifiers);
		case BodyLocations.RLeg:
			return BodyTemplate.CalculateArmor(this.RLegArmor, _DamageTypes, this.RLegArmorModifiers);
		default:
			return 0f;
		}
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00048C7C File Offset: 0x00046E7C
	public bool ProtectsLocation(BodyLocations _BodyLocation)
	{
		switch (_BodyLocation)
		{
		case BodyLocations.Head:
			return !Mathf.Approximately(this.HeadArmor, 0f) || (this.HeadArmorModifiers != null && this.HeadArmorModifiers.Length != 0);
		case BodyLocations.Torso:
			return !Mathf.Approximately(this.TorsoArmor, 0f) || (this.TorsoArmorModifiers != null && this.TorsoArmorModifiers.Length != 0);
		case BodyLocations.LArm:
			return !Mathf.Approximately(this.LArmArmor, 0f) || (this.LArmArmorModifiers != null && this.LArmArmorModifiers.Length != 0);
		case BodyLocations.RArm:
			return !Mathf.Approximately(this.RArmArmor, 0f) || (this.RArmArmorModifiers != null && this.RArmArmorModifiers.Length != 0);
		case BodyLocations.LLeg:
			return !Mathf.Approximately(this.LLegArmor, 0f) || (this.LLegArmorModifiers != null && this.LLegArmorModifiers.Length != 0);
		case BodyLocations.RLeg:
			return !Mathf.Approximately(this.RLegArmor, 0f) || (this.RLegArmorModifiers != null && this.RLegArmorModifiers.Length != 0);
		default:
			return this.ProtectsLocation(BodyLocations.Head) || this.ProtectsLocation(BodyLocations.Torso) || this.ProtectsLocation(BodyLocations.LArm) || this.ProtectsLocation(BodyLocations.RArm) || this.ProtectsLocation(BodyLocations.LLeg) || this.ProtectsLocation(BodyLocations.RLeg);
		}
	}

	// Token: 0x04000A3E RID: 2622
	[SerializeField]
	private float HeadArmor;

	// Token: 0x04000A3F RID: 2623
	[SerializeField]
	private DamageTypeArmorModifier[] HeadArmorModifiers;

	// Token: 0x04000A40 RID: 2624
	public float HeadHitProbabilityModifier;

	// Token: 0x04000A41 RID: 2625
	[Space]
	[SerializeField]
	private float TorsoArmor;

	// Token: 0x04000A42 RID: 2626
	[SerializeField]
	private DamageTypeArmorModifier[] TorsoArmorModifiers;

	// Token: 0x04000A43 RID: 2627
	public float TorsoHitProbabilityModifier;

	// Token: 0x04000A44 RID: 2628
	[Space]
	[SerializeField]
	private float LArmArmor;

	// Token: 0x04000A45 RID: 2629
	[SerializeField]
	private DamageTypeArmorModifier[] LArmArmorModifiers;

	// Token: 0x04000A46 RID: 2630
	public float LArmHitProbabilityModifier;

	// Token: 0x04000A47 RID: 2631
	[Space]
	[SerializeField]
	private float RArmArmor;

	// Token: 0x04000A48 RID: 2632
	[SerializeField]
	private DamageTypeArmorModifier[] RArmArmorModifiers;

	// Token: 0x04000A49 RID: 2633
	public float RArmHitProbabilityModifier;

	// Token: 0x04000A4A RID: 2634
	[Space]
	[SerializeField]
	private float LLegArmor;

	// Token: 0x04000A4B RID: 2635
	[SerializeField]
	private DamageTypeArmorModifier[] LLegArmorModifiers;

	// Token: 0x04000A4C RID: 2636
	public float LLegHitProbabilityModifier;

	// Token: 0x04000A4D RID: 2637
	[Space]
	[SerializeField]
	private float RLegArmor;

	// Token: 0x04000A4E RID: 2638
	[SerializeField]
	private DamageTypeArmorModifier[] RLegArmorModifiers;

	// Token: 0x04000A4F RID: 2639
	public float RLegHitProbabilityModifier;
}
