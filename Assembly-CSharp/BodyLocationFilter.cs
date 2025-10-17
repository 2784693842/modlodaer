using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
[Serializable]
public struct BodyLocationFilter
{
	// Token: 0x060008D5 RID: 2261 RVA: 0x00054EDC File Offset: 0x000530DC
	public bool CanHit(BodyLocations _Location)
	{
		if (_Location == BodyLocations.All || (!this.Head && !this.Torso && !this.LArm && !this.RArm && !this.LLeg && !this.RLeg))
		{
			return true;
		}
		switch (_Location)
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
			return true;
		}
	}

	// Token: 0x04000DD7 RID: 3543
	[SerializeField]
	private bool Head;

	// Token: 0x04000DD8 RID: 3544
	[SerializeField]
	private bool Torso;

	// Token: 0x04000DD9 RID: 3545
	[SerializeField]
	private bool LArm;

	// Token: 0x04000DDA RID: 3546
	[SerializeField]
	private bool RArm;

	// Token: 0x04000DDB RID: 3547
	[SerializeField]
	private bool LLeg;

	// Token: 0x04000DDC RID: 3548
	[SerializeField]
	private bool RLeg;
}
