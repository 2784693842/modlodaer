using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
[Serializable]
public struct LightSourceSettings
{
	// Token: 0x060002BF RID: 703 RVA: 0x0001A96C File Offset: 0x00018B6C
	public void ApplyLight(Material _Mat)
	{
		_Mat.SetVector("_LightPointAndSize", new Vector4(this.LightPos.x, this.LightPos.y, this.LightSize.x, this.LightSize.y));
		_Mat.SetColor("_LightSourceColor", this.LightColor);
		_Mat.SetFloat("_LightFlickerFreq", this.LightFlickerFreq);
		_Mat.SetFloat("_LightFlickerStrength", this.LightFlickerStrength);
	}

	// Token: 0x04000334 RID: 820
	public bool IsSourceOfLight;

	// Token: 0x04000335 RID: 821
	public float Priority;

	// Token: 0x04000336 RID: 822
	[SerializeField]
	private Vector2 LightPos;

	// Token: 0x04000337 RID: 823
	[SerializeField]
	private Vector2 LightSize;

	// Token: 0x04000338 RID: 824
	[SerializeField]
	private Color LightColor;

	// Token: 0x04000339 RID: 825
	[SerializeField]
	private float LightFlickerFreq;

	// Token: 0x0400033A RID: 826
	[SerializeField]
	private float LightFlickerStrength;

	// Token: 0x0400033B RID: 827
	private const string PointAndSize = "_LightPointAndSize";

	// Token: 0x0400033C RID: 828
	private const string SourceColor = "_LightSourceColor";

	// Token: 0x0400033D RID: 829
	private const string FlickerFreq = "_LightFlickerFreq";

	// Token: 0x0400033E RID: 830
	private const string FlickerStrength = "_LightFlickerStrength";
}
