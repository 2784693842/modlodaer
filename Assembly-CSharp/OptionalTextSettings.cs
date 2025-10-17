using System;

// Token: 0x020001C8 RID: 456
[Serializable]
public class OptionalTextSettings : OptionalValue
{
	// Token: 0x06000C60 RID: 3168 RVA: 0x00065B45 File Offset: 0x00063D45
	public OptionalTextSettings(bool _Active, bool _Bold, bool _Italics, bool _Underlined) : base(_Active)
	{
		this.Bold = _Bold;
		this.Italics = _Italics;
		this.Underlined = _Underlined;
	}

	// Token: 0x0400113F RID: 4415
	public bool Bold;

	// Token: 0x04001140 RID: 4416
	public bool Italics;

	// Token: 0x04001141 RID: 4417
	public bool Underlined;
}
