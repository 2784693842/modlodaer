using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class AudioSourcePool : MonoBehaviour
{
	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x060009CC RID: 2508 RVA: 0x00059765 File Offset: 0x00057965
	public float Volume
	{
		get
		{
			return this.SourcePrefab.volume;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x00059772 File Offset: 0x00057972
	public float Pitch
	{
		get
		{
			return this.SourcePrefab.pitch;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x0005977F File Offset: 0x0005797F
	public bool IsPlaying
	{
		get
		{
			return this.PlayingSources.Count > 0;
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00059790 File Offset: 0x00057990
	public void StopAllSounds()
	{
		if (!this.IsPlaying)
		{
			return;
		}
		for (int i = this.PlayingSources.Count - 1; i >= 0; i--)
		{
			this.PlayingSources[i].Stop();
			this.PutSourceBackIntoPool(this.PlayingSources[i]);
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x000597E1 File Offset: 0x000579E1
	public void StopSound(AudioSource _Source)
	{
		_Source.Stop();
		if (!this.PlayingSources.Contains(_Source))
		{
			return;
		}
		this.PutSourceBackIntoPool(_Source);
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00059800 File Offset: 0x00057A00
	private void Awake()
	{
		if (!this.SourcePrefab)
		{
			this.SourcePrefab = new GameObject("SourceModel").AddComponent<AudioSource>();
			this.SourcePrefab.transform.SetParent(base.transform);
		}
		this.SourcesPoolParent = new GameObject("SourcesPool").transform;
		this.PlayingSourcesParent = new GameObject("PlayingSources").transform;
		this.SourcesPoolParent.SetParent(base.transform);
		this.PlayingSourcesParent.SetParent(base.transform);
		if (this.SourcePrefab.playOnAwake)
		{
			this.SourcePrefab.playOnAwake = false;
		}
		for (int i = 0; i < 3; i++)
		{
			this.SourcesPool.Add(UnityEngine.Object.Instantiate<AudioSource>(this.SourcePrefab, this.SourcesPoolParent));
			this.SourcesPool[i].gameObject.SetActive(false);
			this.SourcesPool[i].playOnAwake = true;
		}
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x000598FC File Offset: 0x00057AFC
	private void LateUpdate()
	{
		if (this.PlayingSources.Count == 0)
		{
			return;
		}
		for (int i = this.PlayingSources.Count - 1; i >= 0; i--)
		{
			if (!this.PlayingSources[i].gameObject.activeInHierarchy)
			{
				this.PlayingSources[i].gameObject.SetActive(true);
			}
			else if (!this.PlayingSources[i].isPlaying)
			{
				this.PutSourceBackIntoPool(this.PlayingSources[i]);
			}
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00059985 File Offset: 0x00057B85
	public AudioSource PlaySound(AudioClip _SoundClip)
	{
		return this.PlaySound(_SoundClip, this.SourcePrefab.pitch, this.SourcePrefab.volume);
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x000599A4 File Offset: 0x00057BA4
	public AudioSource PlaySoundWithPitch(AudioClip _SoundClip, float _Pitch)
	{
		return this.PlaySound(_SoundClip, _Pitch, this.SourcePrefab.volume);
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x000599B9 File Offset: 0x00057BB9
	public AudioSource PlaySoundWithVolume(AudioClip _SoundClip, float _Volume)
	{
		return this.PlaySound(_SoundClip, this.SourcePrefab.pitch, _Volume);
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x000599D0 File Offset: 0x00057BD0
	public AudioSource PlaySound(AudioClip _SoundClip, float _Pitch, float _Volume)
	{
		if (!_SoundClip)
		{
			return null;
		}
		AudioSource source = this.GetSource();
		if (!GameManager.DontRenameGOs)
		{
			source.name = _SoundClip.name;
		}
		source.clip = _SoundClip;
		source.pitch = _Pitch;
		source.volume = _Volume;
		return source;
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00059A17 File Offset: 0x00057C17
	private AudioSource GetSource()
	{
		if (this.SourcesPool.Count > 0)
		{
			return this.TakeSourceFromPool();
		}
		return this.CreateNewSource();
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x00059A34 File Offset: 0x00057C34
	private AudioSource TakeSourceFromPool()
	{
		AudioSource audioSource = this.SourcesPool[0];
		this.SourcesPool.RemoveAt(0);
		this.PlayingSources.Add(audioSource);
		audioSource.transform.SetParent(this.PlayingSourcesParent);
		return audioSource;
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00059A78 File Offset: 0x00057C78
	private AudioSource CreateNewSource()
	{
		AudioSource audioSource = UnityEngine.Object.Instantiate<AudioSource>(this.SourcePrefab, this.PlayingSourcesParent);
		audioSource.gameObject.SetActive(false);
		audioSource.playOnAwake = true;
		this.PlayingSources.Add(audioSource);
		audioSource.transform.localPosition = Vector3.zero;
		return audioSource;
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x00059AC7 File Offset: 0x00057CC7
	private void PutSourceBackIntoPool(AudioSource _Source)
	{
		this.PlayingSources.Remove(_Source);
		this.SourcesPool.Add(_Source);
		_Source.gameObject.SetActive(false);
		_Source.transform.SetParent(this.SourcesPoolParent);
	}

	// Token: 0x04000F52 RID: 3922
	[SerializeField]
	private AudioSource SourcePrefab;

	// Token: 0x04000F53 RID: 3923
	private Transform SourcesPoolParent;

	// Token: 0x04000F54 RID: 3924
	private Transform PlayingSourcesParent;

	// Token: 0x04000F55 RID: 3925
	private List<AudioSource> SourcesPool = new List<AudioSource>();

	// Token: 0x04000F56 RID: 3926
	private List<AudioSource> PlayingSources = new List<AudioSource>();
}
