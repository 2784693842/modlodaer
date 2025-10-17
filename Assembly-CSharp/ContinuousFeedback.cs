using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public abstract class ContinuousFeedback : MonoBehaviour
{
	// Token: 0x060003B7 RID: 951 RVA: 0x00027288 File Offset: 0x00025488
	public void SetProgress(float _Progress, bool _PlayStartAndStop = false)
	{
		if (_PlayStartAndStop)
		{
			if (_Progress <= 0f)
			{
				this.PlayStart();
			}
			else if (_Progress >= 1f)
			{
				this.PlayStop();
			}
		}
		this.AnimateProgress(_Progress);
	}

	// Token: 0x060003B8 RID: 952
	protected abstract void PlayStart();

	// Token: 0x060003B9 RID: 953
	protected abstract void AnimateProgress(float _Progress);

	// Token: 0x060003BA RID: 954
	public abstract void PlayStop();
}
