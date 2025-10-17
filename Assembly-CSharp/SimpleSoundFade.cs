using System;
using UnityEngine;

// Token: 0x020001AB RID: 427
[RequireComponent(typeof(AudioSource))]
public class SimpleSoundFade : MonoBehaviour
{
	// Token: 0x06000BAB RID: 2987 RVA: 0x00062044 File Offset: 0x00060244
	private void Update()
	{
		if (!this.SourceComponent)
		{
			this.SourceComponent = base.GetComponent<AudioSource>();
			this.Volume = this.SourceComponent.volume;
		}
		this.Volume = Mathf.MoveTowards(this.Volume, this.TargetVolume, this.FadeSpeed * Time.deltaTime);
		this.SourceComponent.volume = this.Volume * this.VolumeMultiplier;
		if (this.TargetVolume * this.VolumeMultiplier > 0f && !this.SourceComponent.isPlaying)
		{
			this.SourceComponent.Play();
		}
	}

	// Token: 0x04001090 RID: 4240
	[SerializeField]
	private AudioSource SourceComponent;

	// Token: 0x04001091 RID: 4241
	[SerializeField]
	private float VolumeMultiplier = 1f;

	// Token: 0x04001092 RID: 4242
	public float TargetVolume = 1f;

	// Token: 0x04001093 RID: 4243
	public float FadeSpeed = 1f;

	// Token: 0x04001094 RID: 4244
	private float Volume;
}
