using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
[CreateAssetMenu(menuName = "Survival/GameOptions")]
public class GameOptions : ScriptableObject
{
	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00055A6C File Offset: 0x00053C6C
	// (set) Token: 0x060008FA RID: 2298 RVA: 0x00055AA0 File Offset: 0x00053CA0
	public int CurrentFontSet
	{
		get
		{
			if (!MBSingleton<FontsManager>.Instance)
			{
				return 0;
			}
			this.SelectedFontSet = Mathf.Clamp(this.SelectedFontSet, 0, MBSingleton<FontsManager>.Instance.FontSetsCount - 1);
			return this.SelectedFontSet;
		}
		set
		{
			if (!MBSingleton<FontsManager>.Instance)
			{
				return;
			}
			this.SelectedFontSet = Mathf.Clamp(value, 0, MBSingleton<FontsManager>.Instance.FontSetsCount - 1);
			FontsManager.SelectFontSet(value);
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x00055ACE File Offset: 0x00053CCE
	public void SetLanguageToDefault()
	{
		this.SelectedLanguage = LocalizationManager.GetDefaultLanguage;
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060008FC RID: 2300 RVA: 0x00055ADB File Offset: 0x00053CDB
	// (set) Token: 0x060008FD RID: 2301 RVA: 0x00055B11 File Offset: 0x00053D11
	public int CurrentLanguage
	{
		get
		{
			if (!MBSingleton<LocalizationManager>.Instance)
			{
				return 0;
			}
			this.SelectedLanguage = Mathf.Clamp(this.SelectedLanguage, 0, MBSingleton<LocalizationManager>.Instance.Languages.Length - 1);
			return this.SelectedLanguage;
		}
		set
		{
			if (!MBSingleton<LocalizationManager>.Instance)
			{
				return;
			}
			this.SelectedLanguage = Mathf.Clamp(value, 0, MBSingleton<LocalizationManager>.Instance.Languages.Length - 1);
			LocalizationManager.SetLanguage(value, false);
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060008FE RID: 2302 RVA: 0x00055B42 File Offset: 0x00053D42
	public float MusicVolume
	{
		get
		{
			return Mathf.Lerp(-25f, 0f, this.NormalizedMusicVolume);
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00055B59 File Offset: 0x00053D59
	public float AmbienceVolume(float _WithMultiplier)
	{
		return Mathf.Lerp(-25f, 0f, this.NormalizedAmbienceVolume * _WithMultiplier);
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06000900 RID: 2304 RVA: 0x00055B72 File Offset: 0x00053D72
	public float SFXVolume
	{
		get
		{
			return Mathf.Lerp(-25f, 0f, this.NormalizedSFXVolume);
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x00055B89 File Offset: 0x00053D89
	public float MouseWheelSensitivity
	{
		get
		{
			return Mathf.Lerp(10f, 300f, this.NormalizedMouseWheelSensitivity);
		}
	}

	// Token: 0x04000E2D RID: 3629
	private const float MinMusicVolume = -25f;

	// Token: 0x04000E2E RID: 3630
	private const float MinAmbienceVolume = -25f;

	// Token: 0x04000E2F RID: 3631
	private const float MinSFXVolume = -25f;

	// Token: 0x04000E30 RID: 3632
	private const float MinMouseWheelSensitivity = 10f;

	// Token: 0x04000E31 RID: 3633
	private const float MaxMouseWheelSensitivity = 300f;

	// Token: 0x04000E32 RID: 3634
	[Range(0f, 1f)]
	public float NormalizedMusicVolume;

	// Token: 0x04000E33 RID: 3635
	[Range(0f, 1f)]
	public float NormalizedAmbienceVolume;

	// Token: 0x04000E34 RID: 3636
	[Range(0f, 1f)]
	public float NormalizedSFXVolume;

	// Token: 0x04000E35 RID: 3637
	[Range(0f, 1f)]
	public float NormalizedMouseWheelSensitivity;

	// Token: 0x04000E36 RID: 3638
	public bool InvertHorizontalMouseScroll;

	// Token: 0x04000E37 RID: 3639
	public bool DisableSpecialEffects;

	// Token: 0x04000E38 RID: 3640
	public bool VSync;

	// Token: 0x04000E39 RID: 3641
	[SerializeField]
	private int SelectedFontSet;

	// Token: 0x04000E3A RID: 3642
	[SerializeField]
	private int SelectedLanguage;

	// Token: 0x04000E3B RID: 3643
	public bool UsingCustomLanguage;
}
