using System;
using UnityEngine;

// Token: 0x020001DA RID: 474
[Serializable]
public class AmtFeedbackInfo
{
	// Token: 0x06000CB5 RID: 3253 RVA: 0x00068070 File Offset: 0x00066270
	public int GetAmtFeedback(float _Value)
	{
		if (this.StepValue == 0f || _Value == 0f)
		{
			return 0;
		}
		int num = (_Value < 0f) ? Mathf.FloorToInt(_Value / this.StepValue) : Mathf.CeilToInt(_Value / this.StepValue);
		if (this.Inverted)
		{
			return -num;
		}
		return num;
	}

	// Token: 0x0400119E RID: 4510
	public Sprite Icon;

	// Token: 0x0400119F RID: 4511
	public Sprite NegativeIcon;

	// Token: 0x040011A0 RID: 4512
	public float StepValue = 1f;

	// Token: 0x040011A1 RID: 4513
	public bool Inverted;
}
