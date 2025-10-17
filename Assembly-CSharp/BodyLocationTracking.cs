using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[Serializable]
public struct BodyLocationTracking
{
	// Token: 0x06000242 RID: 578 RVA: 0x00017044 File Offset: 0x00015244
	public float GetDefenseModifierForLocation(BodyLocations _BodyLocation)
	{
		switch (_BodyLocation)
		{
		case BodyLocations.Head:
			return this.CurrentHeadDefenseModifier;
		case BodyLocations.Torso:
			return this.CurrentTorsoDefenseModifier;
		case BodyLocations.LArm:
			return this.CurrentLArmDefenseModifier;
		case BodyLocations.RArm:
			return this.CurrentRArmDefenseModifier;
		case BodyLocations.LLeg:
			return this.CurrentLLegDefenseModifier;
		case BodyLocations.RLeg:
			return this.CurrentRLegDefenseModifier;
		default:
			return 0f;
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x000170A0 File Offset: 0x000152A0
	public void ApplyChanges(BodyLocationModifiers _Mods)
	{
		this.CurrentHeadProbModifier += (float)UnityEngine.Random.Range(_Mods.HeadProbModifier.x, _Mods.HeadProbModifier.y);
		this.CurrentHeadDefenseModifier += UnityEngine.Random.Range(_Mods.HeadDefenseModifier.x, _Mods.HeadDefenseModifier.y);
		this.CurrentTorsoProbModifier += (float)UnityEngine.Random.Range(_Mods.TorsoProbModifier.x, _Mods.TorsoProbModifier.y);
		this.CurrentTorsoDefenseModifier += UnityEngine.Random.Range(_Mods.TorsoDefenseModifier.x, _Mods.TorsoDefenseModifier.y);
		this.CurrentLArmProbModifier += (float)UnityEngine.Random.Range(_Mods.LArmProbModifier.x, _Mods.LArmProbModifier.y);
		this.CurrentLArmDefenseModifier += UnityEngine.Random.Range(_Mods.LArmDefenseModifier.x, _Mods.LArmDefenseModifier.y);
		this.CurrentRArmProbModifier += (float)UnityEngine.Random.Range(_Mods.RArmProbModifier.x, _Mods.RArmProbModifier.y);
		this.CurrentRArmDefenseModifier += UnityEngine.Random.Range(_Mods.RArmDefenseModifier.x, _Mods.RArmDefenseModifier.y);
		this.CurrentLLegProbModifier += (float)UnityEngine.Random.Range(_Mods.LLegProbModifier.x, _Mods.LLegProbModifier.y);
		this.CurrentLLegDefenseModifier += UnityEngine.Random.Range(_Mods.LLegDefenseModifier.x, _Mods.LLegDefenseModifier.y);
		this.CurrentRLegProbModifier += (float)UnityEngine.Random.Range(_Mods.RLegProbModifier.x, _Mods.RLegProbModifier.y);
		this.CurrentRLegDefenseModifier += UnityEngine.Random.Range(_Mods.RLegDefenseModifier.x, _Mods.RLegDefenseModifier.y);
	}

	// Token: 0x04000264 RID: 612
	public float CurrentHeadProbModifier;

	// Token: 0x04000265 RID: 613
	public float CurrentHeadDefenseModifier;

	// Token: 0x04000266 RID: 614
	public float CurrentTorsoProbModifier;

	// Token: 0x04000267 RID: 615
	public float CurrentTorsoDefenseModifier;

	// Token: 0x04000268 RID: 616
	public float CurrentLArmProbModifier;

	// Token: 0x04000269 RID: 617
	public float CurrentLArmDefenseModifier;

	// Token: 0x0400026A RID: 618
	public float CurrentRArmProbModifier;

	// Token: 0x0400026B RID: 619
	public float CurrentRArmDefenseModifier;

	// Token: 0x0400026C RID: 620
	public float CurrentLLegProbModifier;

	// Token: 0x0400026D RID: 621
	public float CurrentLLegDefenseModifier;

	// Token: 0x0400026E RID: 622
	public float CurrentRLegProbModifier;

	// Token: 0x0400026F RID: 623
	public float CurrentRLegDefenseModifier;
}
