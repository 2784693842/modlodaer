using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class AmbientSounds : MonoBehaviour
{
	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x00059166 File Offset: 0x00057366
	// (set) Token: 0x060009B9 RID: 2489 RVA: 0x0005916E File Offset: 0x0005736E
	public bool IsPlaying { get; private set; }

	// Token: 0x060009BA RID: 2490 RVA: 0x00059178 File Offset: 0x00057378
	private void Start()
	{
		if (this.MyAudio)
		{
			return;
		}
		this.MyAudio = base.GetComponent<AudioSource>();
		this.MyAudio.volume = this.DefaultVolume;
		this.NoisesParent = new GameObject("RandomNoises").transform;
		this.NoisesParent.SetParent(base.transform);
		this.NoisePlayers = new List<AudioSource>();
		for (int i = 0; i < 3; i++)
		{
			this.NoisePlayers.Add(UnityEngine.Object.Instantiate<AudioSource>(this.NoisePlayerPrefab, this.NoisesParent));
			this.NoisePlayers[i].volume = this.RandomNoisesVolume;
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00059220 File Offset: 0x00057420
	public void Init(AudioClip _MainBackgroundNoise, AudioClip[] _RandomBackgroundNoises, float _VolumeScale, bool _Play = true)
	{
		if (!this.MyAudio)
		{
			this.Start();
		}
		this.ConditionsCards.Clear();
		this.CurrentConditions.Clear();
		this.SetVolume(_VolumeScale);
		this.MyAudio.clip = _MainBackgroundNoise;
		this.RandomClipsCount = 0;
		if (_RandomBackgroundNoises != null)
		{
			this.RandomClipsCount = _RandomBackgroundNoises.Length;
			if (_RandomBackgroundNoises.Length != 0)
			{
				this.RandomSounds = new RandomAmbientSound[_RandomBackgroundNoises.Length];
			}
			else
			{
				this.RandomSounds = null;
			}
			while (this.NoisePlayers.Count < _RandomBackgroundNoises.Length)
			{
				this.NoisePlayers.Add(UnityEngine.Object.Instantiate<AudioSource>(this.NoisePlayerPrefab, this.NoisesParent));
			}
			for (int i = 0; i < this.NoisePlayers.Count; i++)
			{
				if (i >= _RandomBackgroundNoises.Length)
				{
					this.NoisePlayers[i].gameObject.SetActive(false);
				}
				else if (_RandomBackgroundNoises[i])
				{
					this.NoisePlayers[i].gameObject.SetActive(true);
					this.NoisePlayers[i].clip = _RandomBackgroundNoises[i];
					this.RandomSounds[i] = new RandomAmbientSound(this.NoisePlayers[i]);
				}
				else
				{
					this.NoisePlayers[i].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			for (int j = 0; j < this.NoisePlayers.Count; j++)
			{
				this.NoisePlayers[j].gameObject.SetActive(false);
			}
			this.RandomSounds = null;
		}
		if (_Play)
		{
			this.FadeIn();
		}
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x000593AA File Offset: 0x000575AA
	public void SetConditions(DurabilitiesConditions _Conditions)
	{
		this.CurrentConditions = _Conditions;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x000593B3 File Offset: 0x000575B3
	public void AddToConditionCards(InGameCardBase _Card)
	{
		if (!this.ConditionsCards.Contains(_Card))
		{
			this.ConditionsCards.Add(_Card);
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x000593D0 File Offset: 0x000575D0
	private void Update()
	{
		if (this.MyAudio.clip != null && this.ConditionsCards.Count > 0 && !this.CurrentConditions.IsEmpty)
		{
			bool isPlaying = this.IsPlaying;
			bool flag = false;
			for (int i = 0; i < this.ConditionsCards.Count; i++)
			{
				if (this.ConditionsCards[i] && !this.ConditionsCards[i].Destroyed && this.CurrentConditions.ValidConditions(this.ConditionsCards[i]))
				{
					flag = true;
					break;
				}
			}
			if (isPlaying != flag)
			{
				if (flag)
				{
					this.FadeIn();
				}
				else
				{
					this.FadeOut();
				}
			}
		}
		if (!this.MyAudio.isPlaying && this.MyAudio.clip != null)
		{
			return;
		}
		this.MyAudio.volume = this.CurrentMainVolume;
		if (this.RandomSounds != null && this.RandomSounds.Length != 0)
		{
			for (int j = 0; j < this.RandomSounds.Length; j++)
			{
				if (this.RandomSounds[j] != null)
				{
					this.RandomSounds[j].Update();
				}
			}
		}
		if (this.NoisePlayers != null && this.NoisePlayers.Count > 0)
		{
			for (int k = 0; k < this.NoisePlayers.Count; k++)
			{
				if (this.NoisePlayers[k])
				{
					this.NoisePlayers[k].volume = this.CurrentNoisesVolume;
				}
			}
		}
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00059551 File Offset: 0x00057751
	public void SetVolume(float _VolumeScale)
	{
		this.VolumeScale = _VolumeScale;
		this.CurrentMainVolume = this.DefaultVolume * this.VolumeScale;
		this.CurrentNoisesVolume = this.RandomNoisesVolume * this.VolumeScale;
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00059580 File Offset: 0x00057780
	public void FadeIn()
	{
		base.StartCoroutine(this.fadeIn());
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0005958F File Offset: 0x0005778F
	private IEnumerator fadeIn()
	{
		this.IsPlaying = true;
		this.MyAudio.Play();
		this.Fading = true;
		if (this.RandomClipsCount > 0)
		{
			this.RandomSounds = new RandomAmbientSound[this.RandomClipsCount];
			int num = 0;
			for (int i = 0; i < this.NoisePlayers.Count; i++)
			{
				if (this.NoisePlayers[i].gameObject.activeInHierarchy && this.NoisePlayers[i].clip)
				{
					this.RandomSounds[num] = new RandomAmbientSound(this.NoisePlayers[i]);
					num++;
					if (num >= this.RandomClipsCount)
					{
						break;
					}
				}
			}
		}
		float timer = 0f;
		while (timer < 0.8f)
		{
			timer += Time.deltaTime;
			this.CurrentMainVolume = Mathf.Lerp(0f, this.DefaultVolume * this.VolumeScale, timer / 0.8f);
			this.CurrentNoisesVolume = Mathf.Lerp(0f, this.RandomNoisesVolume * this.VolumeScale, timer / 0.8f);
			yield return null;
		}
		this.CurrentMainVolume = this.DefaultVolume * this.VolumeScale;
		this.CurrentNoisesVolume = this.RandomNoisesVolume * this.VolumeScale;
		this.Fading = false;
		yield break;
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0005959E File Offset: 0x0005779E
	public void FadeOut()
	{
		base.StartCoroutine(this.fadeOut());
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x000595AD File Offset: 0x000577AD
	private IEnumerator fadeOut()
	{
		this.IsPlaying = false;
		this.Fading = true;
		float timer = 0f;
		this.RandomSounds = null;
		while (timer < 0.8f)
		{
			timer += Time.deltaTime;
			this.MyAudio.volume = Mathf.Lerp(this.DefaultVolume, 0f, timer / 0.8f);
			for (int i = 0; i < this.NoisePlayers.Count; i++)
			{
				this.NoisePlayers[i].volume = Mathf.Lerp(this.RandomNoisesVolume, 0f, timer / 0.8f);
			}
			yield return null;
		}
		this.MyAudio.volume = 0f;
		for (int j = 0; j < this.NoisePlayers.Count; j++)
		{
			this.NoisePlayers[j].volume = 0f;
		}
		this.MyAudio.Stop();
		for (int k = 0; k < this.NoisePlayers.Count; k++)
		{
			this.NoisePlayers[k].Stop();
		}
		this.Fading = false;
		yield break;
	}

	// Token: 0x04000F38 RID: 3896
	[SerializeField]
	private AudioSource NoisePlayerPrefab;

	// Token: 0x04000F39 RID: 3897
	[SerializeField]
	private float RandomNoisesVolume = 1f;

	// Token: 0x04000F3A RID: 3898
	[SerializeField]
	private float DefaultVolume = 1f;

	// Token: 0x04000F3B RID: 3899
	private List<InGameCardBase> ConditionsCards = new List<InGameCardBase>();

	// Token: 0x04000F3C RID: 3900
	private DurabilitiesConditions CurrentConditions;

	// Token: 0x04000F3D RID: 3901
	private Transform NoisesParent;

	// Token: 0x04000F3E RID: 3902
	private RandomAmbientSound[] RandomSounds;

	// Token: 0x04000F3F RID: 3903
	private AudioSource MyAudio;

	// Token: 0x04000F40 RID: 3904
	private float CurrentMainVolume;

	// Token: 0x04000F41 RID: 3905
	private float CurrentNoisesVolume;

	// Token: 0x04000F42 RID: 3906
	private List<AudioSource> NoisePlayers;

	// Token: 0x04000F43 RID: 3907
	private int RandomClipsCount;

	// Token: 0x04000F44 RID: 3908
	private float VolumeScale = 1f;

	// Token: 0x04000F45 RID: 3909
	private bool Fading;
}
