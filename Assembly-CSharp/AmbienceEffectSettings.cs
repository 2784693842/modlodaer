using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
[Serializable]
public struct AmbienceEffectSettings
{
	// Token: 0x0600075A RID: 1882 RVA: 0x00048ED8 File Offset: 0x000470D8
	public void Update(float _Time)
	{
		this.Grayscale.Update(_Time);
		this.Saturation.Update(_Time);
		this.Value.Update(_Time);
		this.Contrast.Update(_Time);
		this.Aberration.Update(_Time);
		this.AudioLowPass.Update(_Time);
		this.AudioHighPass.Update(_Time);
		this.AudioTrippyEcho.Update(_Time);
		this.AmbientSoundVolume.Update(_Time);
	}

	// Token: 0x04000A58 RID: 2648
	public AnimatedValue Grayscale;

	// Token: 0x04000A59 RID: 2649
	public AnimatedValue Saturation;

	// Token: 0x04000A5A RID: 2650
	public AnimatedValue Value;

	// Token: 0x04000A5B RID: 2651
	public AnimatedValue Contrast;

	// Token: 0x04000A5C RID: 2652
	public AnimatedValue Aberration;

	// Token: 0x04000A5D RID: 2653
	public AnimatedValue AudioLowPass;

	// Token: 0x04000A5E RID: 2654
	public AnimatedValue AudioHighPass;

	// Token: 0x04000A5F RID: 2655
	public AnimatedValue AudioTrippyEcho;

	// Token: 0x04000A60 RID: 2656
	public AudioClip AmbientSound;

	// Token: 0x04000A61 RID: 2657
	public AnimatedValue AmbientSoundVolume;
}
