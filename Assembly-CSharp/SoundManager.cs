using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

// Token: 0x020001AF RID: 431
public class SoundManager : MBSingleton<SoundManager>
{
	// Token: 0x06000BB1 RID: 2993 RVA: 0x00062130 File Offset: 0x00060330
	private void OnDestroy()
	{
		GameManager.OnCardSpawned = (Action<InGameCardBase>)Delegate.Remove(GameManager.OnCardSpawned, new Action<InGameCardBase>(this.OnCardSpawned));
		GameManager.OnCardLoaded = (Action<InGameCardBase>)Delegate.Remove(GameManager.OnCardLoaded, new Action<InGameCardBase>(this.OnCardLoaded));
		GameManager.OnCardDestroyed = (Action<InGameCardBase>)Delegate.Remove(GameManager.OnCardDestroyed, new Action<InGameCardBase>(this.OnCardDestroyed));
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x000621A0 File Offset: 0x000603A0
	public void Init()
	{
		this.GM = MBSingleton<GameManager>.Instance;
		this.GameData = GameLoad.Instance;
		this.FXM = MBSingleton<SpecialEffectsManager>.Instance;
		GameManager.OnCardSpawned = (Action<InGameCardBase>)Delegate.Combine(GameManager.OnCardSpawned, new Action<InGameCardBase>(this.OnCardSpawned));
		GameManager.OnCardLoaded = (Action<InGameCardBase>)Delegate.Combine(GameManager.OnCardLoaded, new Action<InGameCardBase>(this.OnCardLoaded));
		GameManager.OnCardDestroyed = (Action<InGameCardBase>)Delegate.Combine(GameManager.OnCardDestroyed, new Action<InGameCardBase>(this.OnCardDestroyed));
		Transform transform = new GameObject("Ambience").transform;
		transform.SetParent(base.transform);
		transform.localPosition = Vector3.zero;
		new GameObject("Environment").transform.SetParent(transform);
		this.EnvironmentAmbience = new AmbientSounds[]
		{
			UnityEngine.Object.Instantiate<AmbientSounds>(this.EnvironmentAmbiencePrefab, transform.GetChild(0)),
			UnityEngine.Object.Instantiate<AmbientSounds>(this.EnvironmentAmbiencePrefab, transform.GetChild(0))
		};
		new GameObject("Weather").transform.SetParent(transform);
		this.WeatherAmbience = new AmbientSounds[]
		{
			UnityEngine.Object.Instantiate<AmbientSounds>(this.WeatherAmbiencePrefab, transform.GetChild(1)),
			UnityEngine.Object.Instantiate<AmbientSounds>(this.WeatherAmbiencePrefab, transform.GetChild(1))
		};
		this.OtherAmbienceParent = new GameObject("Other").transform;
		this.OtherAmbienceParent.SetParent(transform);
		this.OtherAmbiences = new Dictionary<CardData, AmbientSounds>();
		this.CriticalStateAmbience = UnityEngine.Object.Instantiate<AmbientSounds>(this.CriticalStateAmbiencePrefab, this.OtherAmbienceParent);
		this.CriticalStateAmbience.Init(this.CriticalAmbienceSound, null, 1f, false);
		this.LowpassTargetValue = this.DefaultLowPassValue;
		this.LowpassCurrentValue = this.LowpassTargetValue;
		this.AmbienceVolumeMultiplier = 1f;
		this.AmbienceVolumeMultiplierTarget = 1f;
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00062374 File Offset: 0x00060574
	public void SetCriticalState(bool _State)
	{
		if (_State == this.CriticalEffect)
		{
			return;
		}
		this.CriticalEffect = _State;
		if (_State)
		{
			this.CriticalStateAmbience.FadeIn();
		}
		else
		{
			this.CriticalStateAmbience.FadeOut();
		}
		this.LowpassTargetValue = (_State ? this.TerminalLowPassValue : this.DefaultLowPassValue);
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x000623C4 File Offset: 0x000605C4
	private void LateUpdate()
	{
		if (!this.GameData || !this.GameMixer)
		{
			return;
		}
		this.SetCriticalState(this.FXM.TerminalEffect && this.FXM.SpecialEndingValue <= 0f);
		if (this.GameData.CurrentGameOptions.NormalizedMusicVolume > 0f)
		{
			this.GameMixer.SetFloat("MusicVol", this.GameData.CurrentGameOptions.MusicVolume);
			this.GameMixer.SetFloat("MusicUnfilteredVol", this.GameData.CurrentGameOptions.MusicVolume);
		}
		else
		{
			this.GameMixer.SetFloat("MusicVol", -80f);
			this.GameMixer.SetFloat("MusicUnfilteredVol", -80f);
		}
		this.AmbienceVolumeMultiplier = Mathf.MoveTowards(this.AmbienceVolumeMultiplier, this.AmbienceVolumeMultiplierTarget, Time.deltaTime * 1f);
		if (this.GameData.CurrentGameOptions.NormalizedAmbienceVolume > 0f && this.AmbienceVolumeMultiplier > 0f)
		{
			this.GameMixer.SetFloat("AmbienceVol", this.GameData.CurrentGameOptions.AmbienceVolume(this.AmbienceVolumeMultiplier));
			this.GameMixer.SetFloat("AmbienceUnfilteredVol", this.GameData.CurrentGameOptions.AmbienceVolume(this.AmbienceVolumeMultiplier));
		}
		else
		{
			this.GameMixer.SetFloat("AmbienceVol", -80f);
			this.GameMixer.SetFloat("AmbienceUnfilteredVol", -80f);
		}
		if (this.GameData.CurrentGameOptions.NormalizedSFXVolume > 0f)
		{
			this.GameMixer.SetFloat("SFXVol", this.GameData.CurrentGameOptions.SFXVolume);
			this.GameMixer.SetFloat("SFXUnfilteredVol", this.GameData.CurrentGameOptions.SFXVolume);
		}
		else
		{
			this.GameMixer.SetFloat("SFXVol", -80f);
			this.GameMixer.SetFloat("SFXUnfilteredVol", -80f);
		}
		this.AlteredStateLowPass = Mathf.Lerp(this.FXM.EffectSettings.BadAlteredStateEffect.AudioLowPass, this.FXM.EffectSettings.GoodAlteredStateEffect.AudioLowPass, this.FXM.GoodOrBadStateValue);
		this.AlteredStateHighPass = Mathf.Lerp(this.FXM.EffectSettings.BadAlteredStateEffect.AudioHighPass, this.FXM.EffectSettings.GoodAlteredStateEffect.AudioHighPass, this.FXM.GoodOrBadStateValue);
		this.AlteredStateEcho = Mathf.Lerp(this.FXM.EffectSettings.BadAlteredStateEffect.AudioTrippyEcho, this.FXM.EffectSettings.GoodAlteredStateEffect.AudioTrippyEcho, this.FXM.GoodOrBadStateValue);
		float num = 1f - Mathf.Max(this.FXM.DerealizationValue, this.FXM.AlteredStateValue);
		float num2 = num + this.FXM.DerealizationValue + this.FXM.AlteredStateValue;
		if (this.FXM.SpecialEndingValue > 0f)
		{
			this.EffectsLowPass = this.FXM.EffectSettings.SpecialEndingEffect.AudioLowPass;
			this.EffectsHighPass = this.FXM.EffectSettings.SpecialEndingEffect.AudioHighPass;
			this.EffectsEcho = this.FXM.EffectSettings.SpecialEndingEffect.AudioTrippyEcho;
		}
		else
		{
			this.EffectsLowPass = (num * this.DefaultLowPassValue + this.FXM.AlteredStateValue * this.AlteredStateLowPass + this.FXM.DerealizationValue * this.FXM.EffectSettings.DerealizationEffect.AudioLowPass) / num2;
			this.EffectsHighPass = (num * this.DefaultHighPassValue + this.FXM.AlteredStateValue * this.AlteredStateHighPass + this.FXM.DerealizationValue * this.FXM.EffectSettings.DerealizationEffect.AudioHighPass) / num2;
			this.EffectsEcho = (num * this.DefaultEchoValue + this.FXM.AlteredStateValue * this.AlteredStateEcho + this.FXM.DerealizationValue * this.FXM.EffectSettings.DerealizationEffect.AudioTrippyEcho) / num2;
		}
		this.LowpassTargetValue = ((this.CriticalEffect && this.FXM.SpecialEndingValue <= 0f) ? this.TerminalLowPassValue : this.EffectsLowPass);
		this.LowpassCurrentValue = Mathf.MoveTowards(this.LowpassCurrentValue, this.LowpassTargetValue, Mathf.Abs(this.TerminalLowPassValue - this.DefaultLowPassValue) / 0.8f * Time.deltaTime);
		this.GameMixer.SetFloat("LowPassFilterEffect", Mathf.Lerp(this.MinMaxLowPassValue.x, this.MinMaxLowPassValue.y, this.LowPassValueMapping.Evaluate(this.LowpassCurrentValue)));
		this.GameMixer.SetFloat("HighPassFilterEffect", Mathf.Lerp(this.MinMaxHighPassValue.x, this.MinMaxHighPassValue.y, this.HighPassValueMapping.Evaluate(this.EffectsHighPass)));
		this.GameMixer.SetFloat("TrippyEchoEffect", Mathf.Lerp(this.MinMaxEchoValue.x, this.MinMaxEchoValue.y, this.EchoValueMapping.Evaluate(this.EffectsEcho)));
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00062970 File Offset: 0x00060B70
	public void PerformActionSound(AudioClip[] _Sounds, bool _NoPitchVariation)
	{
		this.PlaySoundWith(_Sounds, this.PerformActionSoundObject, !_NoPitchVariation);
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00062983 File Offset: 0x00060B83
	public void PerformStatusAlertSound(AudioClip[] _Sounds)
	{
		this.PlaySoundWith(_Sounds, this.OnStatusAlertSoundObject, true);
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00062993 File Offset: 0x00060B93
	public void PerformCardAppearanceSound(AudioClip[] _Sounds)
	{
		this.PlaySoundWith(_Sounds, this.OnCardAppearSoundObject, true);
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x000629A3 File Offset: 0x00060BA3
	public void PerformSingleSound(AudioClip _Sound, bool _WithRandomPitch, bool _WithRandomVolume)
	{
		if (_Sound)
		{
			this.OtherSingleSoundObject.PlaySound(_Sound, _WithRandomPitch, _WithRandomVolume);
		}
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x000629BC File Offset: 0x00060BBC
	private void PlaySoundWith(AudioClip[] _Sounds, RandomSoundPlay _Source, bool _PitchVariation = true)
	{
		if (_Sounds == null || !_Source)
		{
			return;
		}
		if (_Sounds.Length == 0)
		{
			return;
		}
		AudioClip audioClip = _Sounds[UnityEngine.Random.Range(0, _Sounds.Length)];
		if (audioClip)
		{
			_Source.PlaySound(audioClip, _PitchVariation, true);
		}
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x000629F7 File Offset: 0x00060BF7
	private void OnCardLoaded(InGameCardBase _Card)
	{
		this.PlayNewCardSounds(_Card, true);
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x00062A01 File Offset: 0x00060C01
	private void OnCardSpawned(InGameCardBase _Card)
	{
		this.PlayNewCardSounds(_Card, false);
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00062A0C File Offset: 0x00060C0C
	private void PlayNewCardSounds(InGameCardBase _Card, bool _Loaded)
	{
		if (!_Card.CardModel || _Card.InBackground || _Card.HiddenInInventory || _Card.IsPinned)
		{
			return;
		}
		if (!_Loaded)
		{
			this.PerformCardAppearanceSound(_Card.CardModel.WhenCreatedSounds);
		}
		CardTypes cardType = _Card.CardModel.CardType;
		if (cardType != CardTypes.Environment)
		{
			if (cardType != CardTypes.Weather)
			{
				this.PlayOtherAmbience(_Card);
				return;
			}
			this.SetWeatherAmbience(_Card.CardModel);
			return;
		}
		else
		{
			if (!_Card.CardModel.InstancedEnvironment)
			{
				this.SetEnvironmentAmbience(_Card.CardModel, 1f);
				return;
			}
			if (_Card.CardModel.Ambience.BackgroundSound)
			{
				this.SetEnvironmentAmbience(_Card.CardModel, 1f);
				return;
			}
			if (this.GM.PrevEnvironment)
			{
				this.SetEnvironmentAmbience(this.GM.PrevEnvironment, this.InstancedEnvironmentVolume);
				return;
			}
			this.SetEnvironmentAmbience(_Card.CardModel, 1f);
			return;
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x00062B08 File Offset: 0x00060D08
	private void OnCardDestroyed(InGameCardBase _Card)
	{
		if (!_Card.CardModel || _Card.InBackground || _Card.HiddenInInventory || _Card.IsPinned)
		{
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Environment || _Card.CardModel.CardType == CardTypes.Weather)
		{
			return;
		}
		this.StopOtherAmbience(_Card);
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x00062B5F File Offset: 0x00060D5F
	private void SetEnvironmentAmbience(CardData _Environment, float _Volume)
	{
		this.SetAlternatingAmbience(_Environment.Ambience, this.EnvironmentAmbience, true, _Volume);
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x00062B75 File Offset: 0x00060D75
	private void SetWeatherAmbience(CardData _Weather)
	{
		this.SetAlternatingAmbience(_Weather.Ambience, this.WeatherAmbience, true, this.FXM.IsInside ? 0.25f : 1f);
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00062BA3 File Offset: 0x00060DA3
	public static void SetWeatherVolume(float _Volume)
	{
		if (!MBSingleton<SoundManager>.Instance)
		{
			return;
		}
		if (MBSingleton<SoundManager>.Instance.WeatherAmbience == null)
		{
			return;
		}
		MBSingleton<SoundManager>.Instance.WeatherAmbience[0].SetVolume(_Volume);
		MBSingleton<SoundManager>.Instance.WeatherAmbience[1].SetVolume(_Volume);
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00062BE4 File Offset: 0x00060DE4
	public static void PauseWeatherAmbience()
	{
		if (!MBSingleton<SoundManager>.Instance)
		{
			return;
		}
		if (MBSingleton<SoundManager>.Instance.WeatherAmbience == null)
		{
			return;
		}
		if (MBSingleton<SoundManager>.Instance.WeatherAmbience[0].IsPlaying)
		{
			MBSingleton<SoundManager>.Instance.WeatherAmbience[0].FadeOut();
		}
		if (MBSingleton<SoundManager>.Instance.WeatherAmbience[1].IsPlaying)
		{
			MBSingleton<SoundManager>.Instance.WeatherAmbience[1].FadeOut();
		}
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00062C54 File Offset: 0x00060E54
	public static void ResumeWeatherAmbience()
	{
		if (!MBSingleton<SoundManager>.Instance)
		{
			return;
		}
		if (!MBSingleton<SoundManager>.Instance.GM)
		{
			MBSingleton<SoundManager>.Instance.GM = MBSingleton<GameManager>.Instance;
		}
		if (MBSingleton<SoundManager>.Instance.GM.CurrentWeatherCard)
		{
			MBSingleton<SoundManager>.Instance.SetWeatherAmbience(MBSingleton<SoundManager>.Instance.GM.CurrentWeatherCard.CardModel);
		}
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x00062CC4 File Offset: 0x00060EC4
	private void SetAlternatingAmbience(AmbienceSettings _Ambience, AmbientSounds[] _AmbiencePair, bool _Play, float _NewVolume = 1f)
	{
		if (_Ambience == null)
		{
			if (_AmbiencePair[0].IsPlaying)
			{
				_AmbiencePair[0].FadeOut();
			}
			if (_AmbiencePair[1].IsPlaying)
			{
				_AmbiencePair[1].FadeOut();
			}
			return;
		}
		if (_Ambience.IsEmpty)
		{
			if (_AmbiencePair[0].IsPlaying)
			{
				_AmbiencePair[0].FadeOut();
			}
			if (_AmbiencePair[1].IsPlaying)
			{
				_AmbiencePair[1].FadeOut();
			}
			return;
		}
		if (_AmbiencePair[0].IsPlaying)
		{
			_AmbiencePair[0].FadeOut();
			_AmbiencePair[1].Init(_Ambience.BackgroundSound, _Ambience.RandomNoises, _NewVolume, _Play);
			return;
		}
		if (_AmbiencePair[1].IsPlaying)
		{
			_AmbiencePair[1].FadeOut();
			_AmbiencePair[0].Init(_Ambience.BackgroundSound, _Ambience.RandomNoises, _NewVolume, _Play);
			return;
		}
		_AmbiencePair[0].Init(_Ambience.BackgroundSound, _Ambience.RandomNoises, _NewVolume, _Play);
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00062D94 File Offset: 0x00060F94
	public void PlayOtherAmbience(InGameCardBase _OtherCard)
	{
		if (!_OtherCard)
		{
			return;
		}
		if (!_OtherCard.CardModel)
		{
			return;
		}
		if (_OtherCard.CardModel.Ambience == null)
		{
			return;
		}
		if (this.OtherAmbiences.ContainsKey(_OtherCard.CardModel))
		{
			if (!_OtherCard.CardModel.Ambience.AmbienceConditions.IsEmpty)
			{
				this.OtherAmbiences[_OtherCard.CardModel].AddToConditionCards(_OtherCard);
			}
			return;
		}
		if (_OtherCard.CardModel.Ambience.IsEmpty)
		{
			return;
		}
		if (this.FreeAmbientSounds.Count == 0)
		{
			this.OtherAmbiences.Add(_OtherCard.CardModel, UnityEngine.Object.Instantiate<AmbientSounds>(this.OtherCardAmbiencePrefab, this.OtherAmbienceParent));
		}
		else
		{
			this.OtherAmbiences.Add(_OtherCard.CardModel, this.FreeAmbientSounds[0]);
			this.FreeAmbientSounds.RemoveAt(0);
		}
		if (!GameManager.DontRenameGOs)
		{
			this.OtherAmbiences[_OtherCard.CardModel].name = "Ambience_" + _OtherCard.name;
		}
		this.OtherAmbiences[_OtherCard.CardModel].Init(_OtherCard.CardModel.Ambience.BackgroundSound, _OtherCard.CardModel.Ambience.RandomNoises, 1f, true);
		if (!_OtherCard.CardModel.Ambience.AmbienceConditions.IsEmpty)
		{
			this.OtherAmbiences[_OtherCard.CardModel].SetConditions(_OtherCard.CardModel.Ambience.AmbienceConditions);
			this.OtherAmbiences[_OtherCard.CardModel].AddToConditionCards(_OtherCard);
		}
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00062F34 File Offset: 0x00061134
	public void StopOtherAmbience(InGameCardBase _FromCard)
	{
		if (!_FromCard)
		{
			return;
		}
		if (!_FromCard.CardModel)
		{
			return;
		}
		if (!this.OtherAmbiences.ContainsKey(_FromCard.CardModel))
		{
			return;
		}
		if (this.GM.CardIsOnBoard(_FromCard.CardModel, true, true, false, false, null, new InGameCardBase[]
		{
			_FromCard
		}))
		{
			return;
		}
		this.OtherAmbiences[_FromCard.CardModel].FadeOut();
		this.FreeAmbientSounds.Add(this.OtherAmbiences[_FromCard.CardModel]);
		if (!GameManager.DontRenameGOs)
		{
			this.OtherAmbiences[_FromCard.CardModel].name = "FreeAmbientSound";
		}
		this.OtherAmbiences.Remove(_FromCard.CardModel);
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00062FF5 File Offset: 0x000611F5
	public void PauseCardAmbience(InGameCardBase _FromCard)
	{
		if (this.OtherAmbiences.ContainsKey(_FromCard.CardModel))
		{
			this.OtherAmbiences[_FromCard.CardModel].FadeOut();
		}
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00063020 File Offset: 0x00061220
	public void ResumeCardAmbience(InGameCardBase _FromCard)
	{
		if (this.OtherAmbiences.ContainsKey(_FromCard.CardModel))
		{
			this.OtherAmbiences[_FromCard.CardModel].FadeIn();
		}
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x0006304C File Offset: 0x0006124C
	public void StopAllOtherAmbiences()
	{
		if (this.OtherAmbiences.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<CardData, AmbientSounds> keyValuePair in this.OtherAmbiences)
		{
			keyValuePair.Value.FadeOut();
			this.FreeAmbientSounds.Add(keyValuePair.Value);
			if (!GameManager.DontRenameGOs)
			{
				keyValuePair.Value.name = "FreeAmbientSound";
			}
		}
		this.OtherAmbiences.Clear();
	}

	// Token: 0x04001098 RID: 4248
	[SerializeField]
	private AudioMixer GameMixer;

	// Token: 0x04001099 RID: 4249
	[SerializeField]
	private RandomSoundPlay PerformActionSoundObject;

	// Token: 0x0400109A RID: 4250
	[SerializeField]
	private RandomSoundPlay OnCardAppearSoundObject;

	// Token: 0x0400109B RID: 4251
	[SerializeField]
	private RandomSoundPlay OnStatusAlertSoundObject;

	// Token: 0x0400109C RID: 4252
	[SerializeField]
	private RandomSoundPlay OtherSingleSoundObject;

	// Token: 0x0400109D RID: 4253
	[SerializeField]
	private AmbientSounds EnvironmentAmbiencePrefab;

	// Token: 0x0400109E RID: 4254
	[SerializeField]
	private AmbientSounds WeatherAmbiencePrefab;

	// Token: 0x0400109F RID: 4255
	[SerializeField]
	private AmbientSounds OtherCardAmbiencePrefab;

	// Token: 0x040010A0 RID: 4256
	[SerializeField]
	private AmbientSounds CriticalStateAmbiencePrefab;

	// Token: 0x040010A1 RID: 4257
	[SerializeField]
	private AmbientSounds SpecialEffectAmbiencePrefab;

	// Token: 0x040010A2 RID: 4258
	[Header("Default Sounds")]
	public AudioClip DefaultSpoilageFull;

	// Token: 0x040010A3 RID: 4259
	public AudioClip DefaultSpoilageOnZero;

	// Token: 0x040010A4 RID: 4260
	[Space]
	public AudioClip DefaultFuelFull;

	// Token: 0x040010A5 RID: 4261
	public AudioClip DefaultFuelOnZero;

	// Token: 0x040010A6 RID: 4262
	[Space]
	public AudioClip DefaultUsageFull;

	// Token: 0x040010A7 RID: 4263
	public AudioClip DefaultUsageOnZero;

	// Token: 0x040010A8 RID: 4264
	[Space]
	public AudioClip DefaultProgressFull;

	// Token: 0x040010A9 RID: 4265
	public AudioClip DefaultProgressOnZero;

	// Token: 0x040010AA RID: 4266
	[Space]
	public AudioClip DefaultSpecialDurabilityFull;

	// Token: 0x040010AB RID: 4267
	public AudioClip DefaultSpecialDurabilityOnZero;

	// Token: 0x040010AC RID: 4268
	[Space]
	public AudioClip DefaultCookingComplete;

	// Token: 0x040010AD RID: 4269
	[Space]
	public AudioClip CriticalAmbienceSound;

	// Token: 0x040010AE RID: 4270
	[Header("Effects")]
	public float InstancedEnvironmentVolume;

	// Token: 0x040010AF RID: 4271
	public Vector2 MinMaxLowPassValue;

	// Token: 0x040010B0 RID: 4272
	public AnimationCurve LowPassValueMapping;

	// Token: 0x040010B1 RID: 4273
	public Vector2 MinMaxHighPassValue;

	// Token: 0x040010B2 RID: 4274
	public AnimationCurve HighPassValueMapping;

	// Token: 0x040010B3 RID: 4275
	public Vector2 MinMaxEchoValue;

	// Token: 0x040010B4 RID: 4276
	public AnimationCurve EchoValueMapping;

	// Token: 0x040010B5 RID: 4277
	[Space]
	[FormerlySerializedAs("CriticalOffValue")]
	public float DefaultLowPassValue;

	// Token: 0x040010B6 RID: 4278
	[FormerlySerializedAs("CriticalOnValue")]
	public float TerminalLowPassValue;

	// Token: 0x040010B7 RID: 4279
	public float DefaultHighPassValue;

	// Token: 0x040010B8 RID: 4280
	public float DefaultEchoValue;

	// Token: 0x040010B9 RID: 4281
	private AmbientSounds[] EnvironmentAmbience;

	// Token: 0x040010BA RID: 4282
	private AmbientSounds[] WeatherAmbience;

	// Token: 0x040010BB RID: 4283
	private Dictionary<CardData, AmbientSounds> OtherAmbiences;

	// Token: 0x040010BC RID: 4284
	private List<AmbientSounds> FreeAmbientSounds = new List<AmbientSounds>();

	// Token: 0x040010BD RID: 4285
	private AmbientSounds CriticalStateAmbience;

	// Token: 0x040010BE RID: 4286
	private AmbientSounds SpecialEffectAmbience;

	// Token: 0x040010BF RID: 4287
	private GameManager GM;

	// Token: 0x040010C0 RID: 4288
	private GameLoad GameData;

	// Token: 0x040010C1 RID: 4289
	private Transform OtherAmbienceParent;

	// Token: 0x040010C2 RID: 4290
	private bool CriticalEffect;

	// Token: 0x040010C3 RID: 4291
	private float LowpassCurrentValue;

	// Token: 0x040010C4 RID: 4292
	private float LowpassTargetValue;

	// Token: 0x040010C5 RID: 4293
	private float AlteredStateLowPass;

	// Token: 0x040010C6 RID: 4294
	private float AlteredStateHighPass;

	// Token: 0x040010C7 RID: 4295
	private float AlteredStateEcho;

	// Token: 0x040010C8 RID: 4296
	private float AlteredStateReverb;

	// Token: 0x040010C9 RID: 4297
	private float EffectsLowPass;

	// Token: 0x040010CA RID: 4298
	private float EffectsHighPass;

	// Token: 0x040010CB RID: 4299
	private float EffectsEcho;

	// Token: 0x040010CC RID: 4300
	private const string AmbienceName = "AmbienceVol";

	// Token: 0x040010CD RID: 4301
	private const string AmbienceUnfilteredName = "AmbienceUnfilteredVol";

	// Token: 0x040010CE RID: 4302
	private const string MusicName = "MusicVol";

	// Token: 0x040010CF RID: 4303
	private const string MusicUnfilteredName = "MusicUnfilteredVol";

	// Token: 0x040010D0 RID: 4304
	private const string SFXName = "SFXVol";

	// Token: 0x040010D1 RID: 4305
	private const string SFXUnfilteredName = "SFXUnfilteredVol";

	// Token: 0x040010D2 RID: 4306
	private const string LowPassName = "LowPassFilterEffect";

	// Token: 0x040010D3 RID: 4307
	private const string HighPassName = "HighPassFilterEffect";

	// Token: 0x040010D4 RID: 4308
	private const string EchoName = "TrippyEchoEffect";

	// Token: 0x040010D5 RID: 4309
	private const float VolumeMultiplierFadeSpeed = 1f;

	// Token: 0x040010D6 RID: 4310
	private SpecialEffectsManager FXM;

	// Token: 0x040010D7 RID: 4311
	[NonSerialized]
	public float AmbienceVolumeMultiplierTarget;

	// Token: 0x040010D8 RID: 4312
	private float AmbienceVolumeMultiplier;
}
