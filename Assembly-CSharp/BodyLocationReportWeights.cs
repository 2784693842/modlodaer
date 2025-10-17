using System;

// Token: 0x0200006C RID: 108
public struct BodyLocationReportWeights
{
	// Token: 0x0600048A RID: 1162 RVA: 0x0002F4A4 File Offset: 0x0002D6A4
	public float GetValue(BodyLocations _ForLocation)
	{
		switch (_ForLocation)
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
		case BodyLocations.All:
			return this.Head + this.Torso + this.LArm + this.RArm + this.LLeg + this.RLeg;
		default:
			return 0f;
		}
	}

	// Token: 0x040005AA RID: 1450
	public float Head;

	// Token: 0x040005AB RID: 1451
	public float Torso;

	// Token: 0x040005AC RID: 1452
	public float LArm;

	// Token: 0x040005AD RID: 1453
	public float RArm;

	// Token: 0x040005AE RID: 1454
	public float LLeg;

	// Token: 0x040005AF RID: 1455
	public float RLeg;
}
