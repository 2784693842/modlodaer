using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class SpecialEffectsManager : MBSingleton<SpecialEffectsManager>
{
	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600027D RID: 637 RVA: 0x0001889E File Offset: 0x00016A9E
	// (set) Token: 0x0600027E RID: 638 RVA: 0x000188A6 File Offset: 0x00016AA6
	public float SpecialEndingValue { get; private set; }

	// Token: 0x0600027F RID: 639 RVA: 0x000188AF File Offset: 0x00016AAF
	private void Awake()
	{
		this.GM = MBSingleton<GameManager>.Instance;
	}

	// Token: 0x06000280 RID: 640 RVA: 0x000188BC File Offset: 0x00016ABC
	public void SetSpecialEndingValue(float _Target)
	{
		this.SpecialEndingTargetValue = _Target;
	}

	// Token: 0x06000281 RID: 641 RVA: 0x000188C8 File Offset: 0x00016AC8
	private void Update()
	{
		this.DisableAllEffects = false;
		if (GameLoad.Instance && GameLoad.Instance.CurrentGameOptions)
		{
			this.DisableAllEffects = GameLoad.Instance.CurrentGameOptions.DisableSpecialEffects;
		}
		if (this.DisableAllEffects)
		{
			this.DarkValue = 0f;
			this.DerealizationValue = 0f;
			this.AlteredStateValue = 0f;
			this.GoodOrBadStateValue = 1f;
			return;
		}
		if (this.GM)
		{
			if (!this.LightStatValue && this.LightStat)
			{
				this.GM.StatsDict.TryGetValue(this.LightStat, out this.LightStatValue);
			}
			if (!this.DerealizationStatValue && this.DerealizationStat)
			{
				this.GM.StatsDict.TryGetValue(this.DerealizationStat, out this.DerealizationStatValue);
			}
			if (!this.AlteredStateStatValue && this.AlteredStateStat)
			{
				this.GM.StatsDict.TryGetValue(this.AlteredStateStat, out this.AlteredStateStatValue);
			}
			if (!this.GoodOrBadStateStatValue && this.GoodOrBadStateStat)
			{
				this.GM.StatsDict.TryGetValue(this.GoodOrBadStateStat, out this.GoodOrBadStateStatValue);
			}
			if (!this.InsideStatValue && this.InsideStat)
			{
				this.GM.StatsDict.TryGetValue(this.InsideStat, out this.InsideStatValue);
			}
		}
		if (this.TerminalEffect)
		{
			this.TerminalValue = Mathf.MoveTowards(this.TerminalValue, 1f, this.TerminalTransitionSpeed * Time.deltaTime);
		}
		else
		{
			this.TerminalValue = Mathf.MoveTowards(this.TerminalValue, 0f, this.TerminalTransitionSpeed * Time.deltaTime);
		}
		this.SpecialEndingValue = Mathf.MoveTowards(this.SpecialEndingValue, this.SpecialEndingTargetValue, this.SpecialEndingTransitionSpeed * Time.deltaTime);
		if (this.DerealizationStatValue)
		{
			this.DerealizationValue = this.DerealizationProgressCurve.Evaluate(Mathf.InverseLerp(this.DerealizationEffectRange.x, this.DerealizationEffectRange.y, this.DerealizationStatValue.SimpleCurrentValue));
		}
		else
		{
			this.DerealizationValue = 0f;
		}
		if (this.AlteredStateStatValue)
		{
			this.AlteredStateValue = this.AlteredStateProgressCurve.Evaluate(Mathf.InverseLerp(this.AlteredStateRange.x, this.AlteredStateRange.y, this.AlteredStateStatValue.SimpleCurrentValue));
		}
		else
		{
			this.AlteredStateValue = 0f;
		}
		if (this.GoodOrBadStateStatValue)
		{
			this.GoodOrBadStateValue = this.GoodOrBadStateCurve.Evaluate(Mathf.InverseLerp(this.GoodOrBadStateRange.x, this.GoodOrBadStateRange.y, this.GoodOrBadStateStatValue.SimpleCurrentValue));
		}
		else
		{
			this.GoodOrBadStateValue = 1f;
		}
		if (this.InsideStatValue)
		{
			this.IsInside = (this.InsideStatValue.SimpleCurrentValue > 0f);
		}
		else
		{
			this.IsInside = false;
		}
		if (this.IsInside)
		{
			SoundManager.SetWeatherVolume(0.4f);
			AmbienceImageEffect.HideWeatherEffects();
		}
		else
		{
			SoundManager.SetWeatherVolume(1f);
			AmbienceImageEffect.ShowWeatherEffects();
		}
		if (!this.GM.CurrentEnvironment)
		{
			this.DarkValue = 0f;
			this.HeatValue = 0f;
			return;
		}
		if (this.GM.CurrentEnvironment.IsHotPlace)
		{
			this.HeatValue = Mathf.MoveTowards(this.HeatValue, 1f, 3f * Time.deltaTime);
		}
		else
		{
			this.HeatValue = Mathf.MoveTowards(this.HeatValue, 0f, 3f * Time.deltaTime);
		}
		if (!this.GM.CurrentEnvironment.IsDarkPlace && !this.UseDarkEffectEverywhere)
		{
			this.DarkValue = 0f;
			return;
		}
		if (this.LightStatValue)
		{
			this.DarkValue = Mathf.InverseLerp(this.DarkEffectRange.x, this.DarkEffectRange.y, this.LightStatValue.SimpleCurrentValue);
			return;
		}
		this.DarkValue = 1f;
	}

	// Token: 0x040002AD RID: 685
	public CameraEffectSettings EffectSettings;

	// Token: 0x040002AE RID: 686
	[SerializeField]
	private float TerminalTransitionSpeed;

	// Token: 0x040002AF RID: 687
	[SerializeField]
	private float SpecialEndingTransitionSpeed;

	// Token: 0x040002B0 RID: 688
	[Space]
	[SerializeField]
	private GameStat LightStat;

	// Token: 0x040002B1 RID: 689
	[SerializeField]
	private Vector2 DarkEffectRange;

	// Token: 0x040002B2 RID: 690
	[SerializeField]
	private bool UseDarkEffectEverywhere;

	// Token: 0x040002B3 RID: 691
	[Space]
	[SerializeField]
	private GameStat InsideStat;

	// Token: 0x040002B4 RID: 692
	[Space]
	[SerializeField]
	private GameStat DerealizationStat;

	// Token: 0x040002B5 RID: 693
	[SerializeField]
	private Vector2 DerealizationEffectRange;

	// Token: 0x040002B6 RID: 694
	[SerializeField]
	private AnimationCurve DerealizationProgressCurve;

	// Token: 0x040002B7 RID: 695
	[Space]
	[SerializeField]
	private GameStat AlteredStateStat;

	// Token: 0x040002B8 RID: 696
	[SerializeField]
	private Vector2 AlteredStateRange;

	// Token: 0x040002B9 RID: 697
	[SerializeField]
	private AnimationCurve AlteredStateProgressCurve;

	// Token: 0x040002BA RID: 698
	[SerializeField]
	private GameStat GoodOrBadStateStat;

	// Token: 0x040002BB RID: 699
	[SerializeField]
	private Vector2 GoodOrBadStateRange;

	// Token: 0x040002BC RID: 700
	[SerializeField]
	private AnimationCurve GoodOrBadStateCurve;

	// Token: 0x040002BD RID: 701
	private bool DisableAllEffects;

	// Token: 0x040002BE RID: 702
	private InGameStat LightStatValue;

	// Token: 0x040002BF RID: 703
	private InGameStat DerealizationStatValue;

	// Token: 0x040002C0 RID: 704
	private InGameStat AlteredStateStatValue;

	// Token: 0x040002C1 RID: 705
	private InGameStat GoodOrBadStateStatValue;

	// Token: 0x040002C2 RID: 706
	private InGameStat InsideStatValue;

	// Token: 0x040002C3 RID: 707
	[NonSerialized]
	public bool TerminalEffect;

	// Token: 0x040002C4 RID: 708
	[NonSerialized]
	public float DerealizationValue;

	// Token: 0x040002C5 RID: 709
	[NonSerialized]
	public float AlteredStateValue;

	// Token: 0x040002C6 RID: 710
	[NonSerialized]
	public float GoodOrBadStateValue;

	// Token: 0x040002C7 RID: 711
	[NonSerialized]
	public float DarkValue;

	// Token: 0x040002C8 RID: 712
	[NonSerialized]
	public float TerminalValue;

	// Token: 0x040002C9 RID: 713
	[NonSerialized]
	public bool IsInside;

	// Token: 0x040002CA RID: 714
	[NonSerialized]
	public float HeatValue;

	// Token: 0x040002CC RID: 716
	private float SpecialEndingTargetValue;

	// Token: 0x040002CD RID: 717
	private GameManager GM;
}
