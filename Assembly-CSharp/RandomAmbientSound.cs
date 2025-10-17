using System;
using UnityEngine;

// Token: 0x02000161 RID: 353
[Serializable]
public class RandomAmbientSound
{
	// Token: 0x060009C5 RID: 2501 RVA: 0x000595F0 File Offset: 0x000577F0
	public RandomAmbientSound(AudioSource _AudioSource)
	{
		if (!_AudioSource)
		{
			return;
		}
		this.Source = _AudioSource;
		this.GetNewTime();
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00059610 File Offset: 0x00057810
	public void Update()
	{
		if (!this.Source)
		{
			return;
		}
		this.Timer += Time.unscaledDeltaTime;
		if (this.Timer >= this.NextPlayTime)
		{
			this.Source.Play();
			this.GetNewTime();
			this.Timer = 0f;
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00059668 File Offset: 0x00057868
	private void GetNewTime()
	{
		this.Source.pitch = UnityEngine.Random.Range(0.7f, 1.3f);
		this.NextPlayTime = UnityEngine.Random.Range(this.Source.clip.length + 5f, this.Source.clip.length + 15f);
	}

	// Token: 0x04000F47 RID: 3911
	private AudioSource Source;

	// Token: 0x04000F48 RID: 3912
	private float Timer;

	// Token: 0x04000F49 RID: 3913
	private float NextPlayTime;
}
