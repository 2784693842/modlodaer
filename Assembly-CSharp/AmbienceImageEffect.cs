using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x0200003F RID: 63
[ExecuteInEditMode]
public class AmbienceImageEffect : MBSingleton<AmbienceImageEffect>
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060002A8 RID: 680 RVA: 0x000198DF File Offset: 0x00017ADF
	// (set) Token: 0x060002A9 RID: 681 RVA: 0x000198E7 File Offset: 0x00017AE7
	public WeatherSet CurrentWeather { get; private set; }

	// Token: 0x060002AA RID: 682 RVA: 0x000198F0 File Offset: 0x00017AF0
	private void Awake()
	{
		this.PrevColor = WeatherColors.Default;
		this.CurrentColor = WeatherColors.Default;
		this.GM = MBSingleton<GameManager>.Instance;
		GameManager.OnCardSpawned = (Action<InGameCardBase>)Delegate.Combine(GameManager.OnCardSpawned, new Action<InGameCardBase>(this.OnCardSpawned));
		GameManager.OnCardLoaded = (Action<InGameCardBase>)Delegate.Combine(GameManager.OnCardLoaded, new Action<InGameCardBase>(this.OnCardLoaded));
		GameManager.OnCardDestroyed = (Action<InGameCardBase>)Delegate.Combine(GameManager.OnCardDestroyed, new Action<InGameCardBase>(this.OnCardDestroyed));
		this.EffectMat.SetFloat("_LightSource", this.LightSourceValue);
		this.FXM = MBSingleton<SpecialEffectsManager>.Instance;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x000199A0 File Offset: 0x00017BA0
	private void OnDestroy()
	{
		GameManager.OnCardSpawned = (Action<InGameCardBase>)Delegate.Remove(GameManager.OnCardSpawned, new Action<InGameCardBase>(this.OnCardSpawned));
		GameManager.OnCardLoaded = (Action<InGameCardBase>)Delegate.Remove(GameManager.OnCardLoaded, new Action<InGameCardBase>(this.OnCardLoaded));
		GameManager.OnCardDestroyed = (Action<InGameCardBase>)Delegate.Remove(GameManager.OnCardDestroyed, new Action<InGameCardBase>(this.OnCardDestroyed));
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00019A10 File Offset: 0x00017C10
	private void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (!this.FXM)
		{
			this.FXM = MBSingleton<SpecialEffectsManager>.Instance;
		}
		if (!this.FXM)
		{
			return;
		}
		this.CurrentTimeValue += Time.deltaTime;
		if (this.CurrentTimeValue >= 1000f)
		{
			this.CurrentTimeValue -= 1000f;
		}
		this.EffectMat.SetFloat("_CriticalEffect", this.FXM.TerminalValue * (1f - Mathf.Ceil(this.FXM.SpecialEndingValue)));
		if (this.FXM.TerminalEffect)
		{
			this.EffectMat.SetFloat("_CriticalFrameThickness", Mathf.Lerp(this.CriticalEffectPulseStrength.x, this.CriticalEffectPulseStrength.y, Mathf.Sin(6.2831855f * this.CriticalEffectPulseFreq * this.CurrentTimeValue) * 0.5f + 0.5f));
		}
		this.FXM.EffectSettings.DerealizationEffect.Update(this.CurrentTimeValue);
		this.FXM.EffectSettings.GoodAlteredStateEffect.Update(this.CurrentTimeValue);
		this.FXM.EffectSettings.BadAlteredStateEffect.Update(this.CurrentTimeValue);
		this.FXM.EffectSettings.SpecialEndingEffect.Update(this.CurrentTimeValue);
		this.AlteredStateGrayscale = Mathf.Lerp(this.FXM.EffectSettings.BadAlteredStateEffect.Grayscale, this.FXM.EffectSettings.GoodAlteredStateEffect.Grayscale, this.FXM.GoodOrBadStateValue);
		this.AlteredStateSaturation = Mathf.Lerp(this.FXM.EffectSettings.BadAlteredStateEffect.Saturation, this.FXM.EffectSettings.GoodAlteredStateEffect.Saturation, this.FXM.GoodOrBadStateValue);
		this.AlteredStateColorValue = Mathf.Lerp(this.FXM.EffectSettings.BadAlteredStateEffect.Value, this.FXM.EffectSettings.GoodAlteredStateEffect.Value, this.FXM.GoodOrBadStateValue);
		this.AlteredStateContrast = Mathf.Lerp(this.FXM.EffectSettings.BadAlteredStateEffect.Contrast, this.FXM.EffectSettings.GoodAlteredStateEffect.Contrast, this.FXM.GoodOrBadStateValue);
		this.AlteredStateAberration = Mathf.Lerp(this.FXM.EffectSettings.BadAlteredStateEffect.Aberration, this.FXM.EffectSettings.GoodAlteredStateEffect.Aberration, this.FXM.GoodOrBadStateValue);
		float num = 1f - Mathf.Max(this.FXM.DerealizationValue, this.FXM.AlteredStateValue);
		float num2 = num + this.FXM.DerealizationValue + this.FXM.AlteredStateValue;
		if (this.FXM.SpecialEndingValue <= 0f)
		{
			this.CurrentSaturation = (num + this.FXM.DerealizationValue * this.FXM.EffectSettings.DerealizationEffect.Saturation + this.FXM.AlteredStateValue * this.AlteredStateSaturation) / num2;
			this.CurrentColorValue = (num + this.FXM.DerealizationValue * this.FXM.EffectSettings.DerealizationEffect.Value + this.FXM.AlteredStateValue * this.AlteredStateColorValue) / num2;
			this.CurrentContrast = (num + this.FXM.DerealizationValue * this.FXM.EffectSettings.DerealizationEffect.Contrast + this.FXM.AlteredStateValue * this.AlteredStateContrast) / num2;
			this.CurrentAberration = (this.FXM.DerealizationValue * this.FXM.EffectSettings.DerealizationEffect.Aberration + this.FXM.AlteredStateValue * this.AlteredStateAberration) / num2;
		}
		else
		{
			num = 1f - this.FXM.SpecialEndingValue;
			num2 = num + this.FXM.SpecialEndingValue;
			this.CurrentSaturation = (this.FXM.SpecialEndingValue * this.FXM.EffectSettings.SpecialEndingEffect.Saturation + num) / num2;
			this.CurrentColorValue = (this.FXM.SpecialEndingValue * this.FXM.EffectSettings.SpecialEndingEffect.Value + num) / num2;
			this.CurrentContrast = (this.FXM.SpecialEndingValue * this.FXM.EffectSettings.SpecialEndingEffect.Contrast + num) / num2;
			this.CurrentAberration = this.FXM.SpecialEndingValue * this.FXM.EffectSettings.SpecialEndingEffect.Aberration;
		}
		if (!this.DontUseNewEffects)
		{
			float num3 = 1f - Mathf.Max(new float[]
			{
				this.FXM.TerminalValue,
				this.FXM.DerealizationValue,
				this.FXM.AlteredStateValue
			}) + this.FXM.DerealizationValue + this.FXM.TerminalValue + this.FXM.AlteredStateValue;
			if (this.FXM.SpecialEndingValue <= 0f)
			{
				this.CurrentGrayscale = (this.FXM.TerminalValue * this.CriticalEffectGrayscale + this.FXM.DerealizationValue * this.FXM.EffectSettings.DerealizationEffect.Grayscale + this.FXM.AlteredStateValue * this.AlteredStateGrayscale) / num3;
			}
			else
			{
				this.CurrentGrayscale = this.FXM.SpecialEndingValue * this.FXM.EffectSettings.SpecialEndingEffect.Grayscale;
			}
			this.EffectMat.SetFloat("_SaturationMult", this.CurrentSaturation);
			this.EffectMat.SetFloat("_ValueMult", this.CurrentColorValue);
			this.EffectMat.SetFloat("_Contrast", this.CurrentContrast);
			this.EffectMat.SetFloat("_Aberration", this.CurrentAberration);
			this.EffectMat.SetFloat("_HeatEffect", this.FXM.HeatValue);
			if (this.DerealizationImageEffect)
			{
				this.DerealizationImageEffect.color = new Color(this.DerealizationImageEffect.color.r, this.DerealizationImageEffect.color.g, this.DerealizationImageEffect.color.b, this.FXM.DerealizationValue);
			}
		}
		else if (this.FXM.SpecialEndingValue <= 0f)
		{
			this.CurrentGrayscale = Mathf.Lerp(0f, this.CriticalEffectGrayscale, this.FXM.TerminalValue);
		}
		else
		{
			this.CurrentGrayscale = this.FXM.SpecialEndingValue * this.FXM.EffectSettings.SpecialEndingEffect.Grayscale;
		}
		this.EffectMat.SetFloat("_Grayscale", this.CurrentGrayscale);
		this.CurrentColor = (this.CurrentWeather ? this.CurrentWeather.CurrentColors : WeatherColors.Default);
		if (!this.CurrentColor.IsSameAs(this.PrevColor))
		{
			if (this.Transition != null)
			{
				base.StopCoroutine(this.Transition);
			}
			this.Transition = base.StartCoroutine(this.DiscreteTransition());
		}
		else if (this.Transition == null)
		{
			this.ApplyColors(this.CurrentColor);
		}
		if (this.CurrentLightSources.Count == 0 || !this.CurrentColor.LightSourceAllowed)
		{
			this.CurrentLightSource.IsSourceOfLight = false;
		}
		else if (this.CurrentLightSources.Count > 0)
		{
			this.CurrentLightSource.IsSourceOfLight = false;
			for (int i = this.CurrentLightSources.Count - 1; i >= 0; i--)
			{
				if (this.GM.CardIsOnBoard(this.CurrentLightSources[i], false, true, false, false, null, Array.Empty<InGameCardBase>()))
				{
					this.CurrentLightSource = this.CurrentLightSources[i].LightSource;
					break;
				}
			}
		}
		if (!this.CurrentLightSource.IsSourceOfLight)
		{
			if (this.LightSourceValue > 0f)
			{
				this.LightSourceValue = Mathf.MoveTowards(this.LightSourceValue, 0f, 1f / this.TransitionFadeTime * Time.deltaTime);
				this.EffectMat.SetFloat("_LightSource", this.LightSourceValue);
			}
		}
		else
		{
			this.LightSourceValue = Mathf.MoveTowards(this.LightSourceValue, 1f, 1f / this.TransitionFadeTime * Time.deltaTime);
			this.EffectMat.SetFloat("_LightSource", this.LightSourceValue);
			this.CurrentLightSource.ApplyLight(this.EffectMat);
		}
		this.DarkValue = Mathf.MoveTowards(this.DarkValue, this.FXM.DarkValue, Time.deltaTime * (1f / this.TransitionFadeTime));
		if (this.DarkEffect)
		{
			this.DarkEffect.color = new Color(this.DarkEffect.color.r, this.DarkEffect.color.g, this.DarkEffect.color.b, this.DarkValue);
		}
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0001A3A1 File Offset: 0x000185A1
	private void OnCardSpawned(InGameCardBase _Card)
	{
		this.ApplyLightSource(_Card);
	}

	// Token: 0x060002AE RID: 686 RVA: 0x0001A3A1 File Offset: 0x000185A1
	private void OnCardLoaded(InGameCardBase _Card)
	{
		this.ApplyLightSource(_Card);
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0001A3AA File Offset: 0x000185AA
	private void OnCardDestroyed(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return;
		}
		this.StopLightSource(_Card);
		AmbienceImageEffect.RemoveCardVisualEffects(_Card);
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x0001A3C8 File Offset: 0x000185C8
	public void ApplyLightSource(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return;
		}
		if (!_Card.CardModel.LightSource.IsSourceOfLight)
		{
			return;
		}
		if (!this.CurrentLightSources.Contains(_Card.CardModel))
		{
			this.CurrentLightSources.Add(_Card.CardModel);
			this.CurrentLightSources.Sort((CardData a, CardData b) => a.LightSource.Priority.CompareTo(b.LightSource.Priority));
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x0001A444 File Offset: 0x00018644
	public void SetConfetti(bool _Active)
	{
		if (!this.ConfettiEffect)
		{
			return;
		}
		if (_Active)
		{
			this.ConfettiEffect.Play(true);
			return;
		}
		this.ConfettiEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0001A474 File Offset: 0x00018674
	public void StopLightSource(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return;
		}
		if (!_Card.CardModel.LightSource.IsSourceOfLight)
		{
			return;
		}
		if (this.CurrentLightSources.Contains(_Card.CardModel) && !this.GM.CardIsOnBoard(_Card.CardModel, true, true, false, false, null, Array.Empty<InGameCardBase>()))
		{
			this.CurrentLightSources.Remove(_Card.CardModel);
			if (this.CurrentLightSources.Count > 0)
			{
				this.CurrentLightSources.Sort((CardData a, CardData b) => a.LightSource.Priority.CompareTo(b.LightSource.Priority));
			}
		}
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0001A51C File Offset: 0x0001871C
	private void ApplyColors(WeatherColors _Colors)
	{
		this.EffectMat.color = _Colors.MainColor;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0001A52F File Offset: 0x0001872F
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.EffectMat)
		{
			Graphics.Blit(source, destination, this.EffectMat);
		}
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0001A54C File Offset: 0x0001874C
	[ContextMenu("Shake!")]
	public void ShakeScreen()
	{
		if (this.UIShakeParent)
		{
			this.UIShakeParent.DOKill(true);
			this.UIShakeParent.DOShakePosition(this.ShakeDuration, this.ShakeStrength, this.ShakeVibrato, this.ShakeRandomness, false, true);
		}
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0001A59C File Offset: 0x0001879C
	public static void SpawnCardVisualEffect(WeatherSpecialEffect _Effect, InGameCardBase _FromCard)
	{
		if (!MBSingleton<AmbienceImageEffect>.Instance || !_FromCard)
		{
			return;
		}
		if (MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects == null)
		{
			return;
		}
		MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects.Add(UnityEngine.Object.Instantiate<WeatherSpecialEffect>(_Effect, (_Effect is WeatherUI) ? MBSingleton<AmbienceImageEffect>.Instance.WeatherUIEffectsParent : MBSingleton<AmbienceImageEffect>.Instance.WeatherEffectsParent));
		MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects[MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects.Count - 1].AssociatedCard = _FromCard;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0001A624 File Offset: 0x00018824
	public static void RemoveCardVisualEffects(InGameCardBase _FromCard)
	{
		if (!MBSingleton<AmbienceImageEffect>.Instance || !_FromCard)
		{
			return;
		}
		if (MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects == null)
		{
			return;
		}
		for (int i = MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects.Count - 1; i >= 0; i--)
		{
			if (MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects[i].AssociatedCard == _FromCard)
			{
				MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects[i].Remove();
				MBSingleton<AmbienceImageEffect>.Instance.CurrentCardVisualEffects.RemoveAt(i);
			}
		}
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0001A6B4 File Offset: 0x000188B4
	public static void SetWeather(WeatherSet _Weather)
	{
		if (!MBSingleton<AmbienceImageEffect>.Instance)
		{
			return;
		}
		if (MBSingleton<AmbienceImageEffect>.Instance.CurrentWeather == _Weather)
		{
			return;
		}
		MBSingleton<AmbienceImageEffect>.Instance.PrevColor = MBSingleton<AmbienceImageEffect>.Instance.CurrentColor;
		MBSingleton<AmbienceImageEffect>.Instance.RemoveWeatherEffects();
		if (_Weather.EffectsToSpawn != null)
		{
			for (int i = 0; i < _Weather.EffectsToSpawn.Length; i++)
			{
				MBSingleton<AmbienceImageEffect>.Instance.CurrentWeatherEffects.Add(UnityEngine.Object.Instantiate<WeatherSpecialEffect>(_Weather.EffectsToSpawn[i], (_Weather.EffectsToSpawn[i] is WeatherUI) ? MBSingleton<AmbienceImageEffect>.Instance.WeatherUIEffectsParent : MBSingleton<AmbienceImageEffect>.Instance.WeatherEffectsParent));
			}
		}
		MBSingleton<AmbienceImageEffect>.Instance.CurrentWeather = _Weather;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0001A768 File Offset: 0x00018968
	private void RemoveWeatherEffects()
	{
		for (int i = 0; i < this.CurrentWeatherEffects.Count; i++)
		{
			this.CurrentWeatherEffects[i].Remove();
		}
		this.CurrentWeatherEffects.Clear();
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0001A7A8 File Offset: 0x000189A8
	public static void HideWeatherEffects()
	{
		if (MBSingleton<AmbienceImageEffect>.Instance.WeatherEffectsParent.gameObject.activeInHierarchy)
		{
			MBSingleton<AmbienceImageEffect>.Instance.WeatherEffectsParent.gameObject.SetActive(false);
		}
		if (MBSingleton<AmbienceImageEffect>.Instance.WeatherUIEffectsParent.gameObject.activeInHierarchy)
		{
			MBSingleton<AmbienceImageEffect>.Instance.WeatherUIEffectsParent.gameObject.SetActive(false);
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0001A80C File Offset: 0x00018A0C
	public static void ShowWeatherEffects()
	{
		if (!MBSingleton<AmbienceImageEffect>.Instance.WeatherEffectsParent.gameObject.activeInHierarchy)
		{
			MBSingleton<AmbienceImageEffect>.Instance.WeatherEffectsParent.gameObject.SetActive(true);
		}
		if (!MBSingleton<AmbienceImageEffect>.Instance.WeatherUIEffectsParent.gameObject.activeInHierarchy)
		{
			MBSingleton<AmbienceImageEffect>.Instance.WeatherUIEffectsParent.gameObject.SetActive(true);
		}
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0001A86F File Offset: 0x00018A6F
	private IEnumerator DiscreteTransition()
	{
		if (!this.EffectMat)
		{
			yield break;
		}
		WeatherColors from = new WeatherColors
		{
			MainColor = this.EffectMat.color
		};
		WeatherColors colors = default(WeatherColors);
		this.PrevColor = this.CurrentColor;
		float timer = 0f;
		while (timer < this.TransitionFadeTime)
		{
			timer += Time.deltaTime;
			colors = WeatherColors.Lerp(from, this.CurrentColor, timer / this.TransitionFadeTime);
			this.ApplyColors(colors);
			yield return null;
		}
		this.ApplyColors(this.CurrentColor);
		this.Transition = null;
		yield break;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0001A880 File Offset: 0x00018A80
	public Vector3 ScaleForMask(Vector2 _RectSize)
	{
		if (!this.MaskPrefab)
		{
			return Vector3.zero;
		}
		if (!this.MaskPrefab.sprite)
		{
			return Vector3.zero;
		}
		float pixelsPerUnit = this.MaskPrefab.sprite.pixelsPerUnit;
		Vector2 size = this.MaskPrefab.sprite.rect.size;
		return new Vector3(_RectSize.x * pixelsPerUnit / size.x, _RectSize.y * pixelsPerUnit / size.y, 1f);
	}

	// Token: 0x040002FF RID: 767
	public Material EffectMat;

	// Token: 0x04000301 RID: 769
	public Camera EffectsCam;

	// Token: 0x04000302 RID: 770
	public float TransitionFadeTime;

	// Token: 0x04000303 RID: 771
	[FormerlySerializedAs("UIEffectsParent")]
	public RectTransform WeatherUIEffectsParent;

	// Token: 0x04000304 RID: 772
	[FormerlySerializedAs("OtherEffectsParent")]
	public Transform WeatherEffectsParent;

	// Token: 0x04000305 RID: 773
	public SpriteMask MaskPrefab;

	// Token: 0x04000306 RID: 774
	public Image DarkEffect;

	// Token: 0x04000307 RID: 775
	public GameStat LightStat;

	// Token: 0x04000308 RID: 776
	public Vector2 DarkEffectRange;

	// Token: 0x04000309 RID: 777
	[SerializeField]
	private ParticleSystem ConfettiEffect;

	// Token: 0x0400030A RID: 778
	[Space]
	[FormerlySerializedAs("CriticalEffectSaturation")]
	public float CriticalEffectGrayscale;

	// Token: 0x0400030B RID: 779
	public float CriticalEffectPulseFreq;

	// Token: 0x0400030C RID: 780
	public Vector2 CriticalEffectPulseStrength;

	// Token: 0x0400030D RID: 781
	[Space]
	public bool DontUseNewEffects;

	// Token: 0x0400030E RID: 782
	public Image DerealizationImageEffect;

	// Token: 0x0400030F RID: 783
	[Space]
	[SerializeField]
	private RectTransform UIShakeParent;

	// Token: 0x04000310 RID: 784
	[SerializeField]
	private float ShakeDuration = 0.5f;

	// Token: 0x04000311 RID: 785
	[SerializeField]
	private float ShakeStrength = 30f;

	// Token: 0x04000312 RID: 786
	[SerializeField]
	private int ShakeVibrato = 15;

	// Token: 0x04000313 RID: 787
	[SerializeField]
	private float ShakeRandomness = 90f;

	// Token: 0x04000314 RID: 788
	private GameManager GM;

	// Token: 0x04000315 RID: 789
	private WeatherColors PrevColor;

	// Token: 0x04000316 RID: 790
	private WeatherColors CurrentColor;

	// Token: 0x04000317 RID: 791
	private List<WeatherSpecialEffect> CurrentWeatherEffects = new List<WeatherSpecialEffect>();

	// Token: 0x04000318 RID: 792
	private List<WeatherSpecialEffect> CurrentCardVisualEffects = new List<WeatherSpecialEffect>();

	// Token: 0x04000319 RID: 793
	private Coroutine Transition;

	// Token: 0x0400031A RID: 794
	private float LightSourceValue;

	// Token: 0x0400031B RID: 795
	[NonSerialized]
	public float DarkValue;

	// Token: 0x0400031C RID: 796
	private float AlteredStateGrayscale;

	// Token: 0x0400031D RID: 797
	private float AlteredStateSaturation;

	// Token: 0x0400031E RID: 798
	private float AlteredStateColorValue;

	// Token: 0x0400031F RID: 799
	private float AlteredStateContrast;

	// Token: 0x04000320 RID: 800
	private float AlteredStateAberration;

	// Token: 0x04000321 RID: 801
	private float CurrentGrayscale;

	// Token: 0x04000322 RID: 802
	private float CurrentSaturation;

	// Token: 0x04000323 RID: 803
	private float CurrentColorValue;

	// Token: 0x04000324 RID: 804
	private float CurrentContrast;

	// Token: 0x04000325 RID: 805
	private float CurrentAberration;

	// Token: 0x04000326 RID: 806
	private LightSourceSettings CurrentLightSource;

	// Token: 0x04000327 RID: 807
	private List<CardData> CurrentLightSources = new List<CardData>();

	// Token: 0x04000328 RID: 808
	private float CurrentTimeValue;

	// Token: 0x04000329 RID: 809
	private const string LightSource = "_LightSource";

	// Token: 0x0400032A RID: 810
	private const string Grayscale = "_Grayscale";

	// Token: 0x0400032B RID: 811
	private const string CriticalToggle = "_CriticalEffect";

	// Token: 0x0400032C RID: 812
	private const string CriticalThickness = "_CriticalFrameThickness";

	// Token: 0x0400032D RID: 813
	private const string Saturation = "_SaturationMult";

	// Token: 0x0400032E RID: 814
	private const string ColorValue = "_ValueMult";

	// Token: 0x0400032F RID: 815
	private const string Contrast = "_Contrast";

	// Token: 0x04000330 RID: 816
	private const string Aberration = "_Aberration";

	// Token: 0x04000331 RID: 817
	private const string HeatHaze = "_HeatEffect";

	// Token: 0x04000332 RID: 818
	private RenderTexture MyRenderTexture;

	// Token: 0x04000333 RID: 819
	private SpecialEffectsManager FXM;
}
