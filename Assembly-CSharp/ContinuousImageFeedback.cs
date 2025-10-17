using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000059 RID: 89
public class ContinuousImageFeedback : ContinuousFeedback
{
	// Token: 0x060003BC RID: 956 RVA: 0x00018E36 File Offset: 0x00017036
	protected override void PlayStart()
	{
	}

	// Token: 0x060003BD RID: 957 RVA: 0x000272B4 File Offset: 0x000254B4
	protected override void AnimateProgress(float _Progress)
	{
		if (this.SetFill)
		{
			this.Target.fillAmount = _Progress;
		}
		if (this.SetColor)
		{
			this.Target.color = Color.Lerp(this.StartColor, this.EndColor, _Progress);
		}
		if (this.AnimateSprite && this.AnimSprites != null && this.AnimSprites.Length != 0)
		{
			this.Target.overrideSprite = this.AnimSprites[Mathf.RoundToInt(Mathf.Lerp(0f, (float)(this.AnimSprites.Length - 1), _Progress))];
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00018E36 File Offset: 0x00017036
	public override void PlayStop()
	{
	}

	// Token: 0x040004CA RID: 1226
	[SerializeField]
	private Image Target;

	// Token: 0x040004CB RID: 1227
	[SerializeField]
	private bool SetFill;

	// Token: 0x040004CC RID: 1228
	[SerializeField]
	private bool SetColor;

	// Token: 0x040004CD RID: 1229
	[SerializeField]
	private Color StartColor;

	// Token: 0x040004CE RID: 1230
	[SerializeField]
	private Color EndColor;

	// Token: 0x040004CF RID: 1231
	[SerializeField]
	private bool AnimateSprite;

	// Token: 0x040004D0 RID: 1232
	[SerializeField]
	private Sprite[] AnimSprites;
}
