using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A5 RID: 421
[RequireComponent(typeof(AudioSourcePool))]
public class RandomSoundPlay : MonoBehaviour
{
	// Token: 0x06000B95 RID: 2965 RVA: 0x00061A9B File Offset: 0x0005FC9B
	private void Awake()
	{
		this.SourcePool = base.GetComponent<AudioSourcePool>();
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x00061AA9 File Offset: 0x0005FCA9
	private void Update()
	{
		if (this.SoundQueue.Count > 0)
		{
			if (this.SourcePool.IsPlaying)
			{
				return;
			}
			this.SourcePool.PlaySound(this.SoundQueue[0]);
		}
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00061AE0 File Offset: 0x0005FCE0
	public void PlaySound()
	{
		int num = UnityEngine.Random.Range(0, this.Sounds.Length);
		if (this.Sounds[num])
		{
			this.PlaySound(this.Sounds[num], true, true);
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00061B1C File Offset: 0x0005FD1C
	public void PlaySound(AudioClip _Sound, bool _WithRandomPitch, bool _WithRandomVolume)
	{
		if (_WithRandomPitch && _WithRandomVolume)
		{
			this.SourcePool.PlaySound(_Sound, this.SourcePool.Pitch + UnityEngine.Random.Range(-this.RandomPitchOffset, this.RandomPitchOffset), this.SourcePool.Volume + UnityEngine.Random.Range(-this.RandomVolumeOffset, this.RandomVolumeOffset));
			return;
		}
		if (_WithRandomPitch)
		{
			this.SourcePool.PlaySoundWithPitch(_Sound, this.SourcePool.Pitch + UnityEngine.Random.Range(-this.RandomPitchOffset, this.RandomPitchOffset));
			return;
		}
		if (_WithRandomVolume)
		{
			this.SourcePool.PlaySoundWithVolume(_Sound, this.SourcePool.Volume + UnityEngine.Random.Range(-this.RandomVolumeOffset, this.RandomVolumeOffset));
			return;
		}
		this.SourcePool.PlaySound(_Sound);
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x00061BE3 File Offset: 0x0005FDE3
	public void PlaySoundSequence(AudioClip[] _SoundQueue)
	{
		this.SoundQueue.AddRange(_SoundQueue);
	}

	// Token: 0x04001080 RID: 4224
	[SerializeField]
	private float RandomPitchOffset;

	// Token: 0x04001081 RID: 4225
	[SerializeField]
	private float RandomVolumeOffset;

	// Token: 0x04001082 RID: 4226
	[SerializeField]
	private AudioClip[] Sounds;

	// Token: 0x04001083 RID: 4227
	private AudioSourcePool SourcePool;

	// Token: 0x04001084 RID: 4228
	private List<AudioClip> SoundQueue = new List<AudioClip>();

	// Token: 0x04001085 RID: 4229
	private List<AudioClip> JustPlayedSounds = new List<AudioClip>();
}
