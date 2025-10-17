using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009C RID: 156
public class OptionsMenu : MonoBehaviour
{
	// Token: 0x0600065C RID: 1628 RVA: 0x00042984 File Offset: 0x00040B84
	private void OnEnable()
	{
		this.SetupResolutions();
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = GameLoad.GetGameOptions;
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		GraphicsManager.SetActiveGroup(this.ChangeLanguageObjects, LocalizationManager.CanChangeLanguage);
		this.CurrentLanguageIndex = this.CurrentGameOptions.CurrentLanguage;
		if (this.LanguageText && MBSingleton<LocalizationManager>.Instance)
		{
			this.LanguageText.text = MBSingleton<LocalizationManager>.Instance.Languages[this.CurrentLanguageIndex].LanguageName;
		}
		if (this.ChangeLanguageButton)
		{
			this.ChangeLanguageButton.interactable = (this.CurrentGameOptions.CurrentLanguage != this.CurrentLanguageIndex);
		}
		if (this.MusicSlider)
		{
			this.MusicSlider.value = this.CurrentGameOptions.NormalizedMusicVolume;
		}
		if (this.MusicValueText)
		{
			this.MusicValueText.text = (this.MusicSlider.value * 100f).ToString("0");
		}
		if (this.AmbienceSlider)
		{
			this.AmbienceSlider.value = this.CurrentGameOptions.NormalizedAmbienceVolume;
		}
		if (this.AmbienceValueText)
		{
			this.AmbienceValueText.text = (this.AmbienceSlider.value * 100f).ToString("0");
		}
		if (this.SfxSlider)
		{
			this.SfxSlider.value = this.CurrentGameOptions.NormalizedSFXVolume;
		}
		if (this.SfxValueText)
		{
			this.SfxValueText.text = (this.SfxSlider.value * 100f).ToString("0");
		}
		GraphicsManager.SetActiveGroup(this.MouseWheelObjects, !MobilePlatformDetection.IsMobilePlatform);
		if (this.MouseWheelSlider)
		{
			this.MouseWheelSlider.value = this.CurrentGameOptions.NormalizedMouseWheelSensitivity;
		}
		if (this.MouseWheelValueText)
		{
			this.MouseWheelValueText.text = (this.MouseWheelSlider.value * 100f).ToString("0");
		}
		if (this.HorizontalScrollDirectionToggle)
		{
			this.HorizontalScrollDirectionToggle.isOn = this.CurrentGameOptions.InvertHorizontalMouseScroll;
		}
		if (this.SpecialEffectsToggle)
		{
			this.SpecialEffectsToggle.isOn = !this.CurrentGameOptions.DisableSpecialEffects;
		}
		if (this.VSyncToggle)
		{
			this.VSyncToggle.gameObject.SetActive(!MobilePlatformDetection.IsMobilePlatform);
			this.VSyncToggle.isOn = this.CurrentGameOptions.VSync;
		}
		if (this.VSyncOptionObject)
		{
			this.VSyncOptionObject.SetActive(!MobilePlatformDetection.IsMobilePlatform);
		}
		if (this.FontSetOption)
		{
			this.FontSetOption.gameObject.SetActive(!LocalizationManager.LanguageDoesNotSupportStylizedFont);
		}
		if (this.FontSetText)
		{
			this.FontSetText.text = (FontsManager.SelectedFontSet ? FontsManager.SelectedFontSet.SetName : "");
		}
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00042CC4 File Offset: 0x00040EC4
	private void SetupResolutions()
	{
		if (MobilePlatformDetection.IsMobilePlatform)
		{
			return;
		}
		Resolution[] resolutions = Screen.resolutions;
		this.ResolutionsList.Clear();
		this.ResolutionOptions.Clear();
		for (int i = 0; i < resolutions.Length; i++)
		{
			string item = string.Format("{0}x{1}", resolutions[i].width, resolutions[i].height);
			if (!this.ResolutionOptions.Contains(item))
			{
				this.ResolutionsList.Add(resolutions[i]);
				this.ResolutionOptions.Add(item);
				if (this.ResolutionsList[this.ResolutionsList.Count - 1].width == Screen.width && this.ResolutionsList[this.ResolutionsList.Count - 1].height == Screen.height)
				{
					this.CurrentResolutionIndex = this.ResolutionsList.Count - 1;
				}
			}
		}
		if (this.ResolutionText)
		{
			this.ResolutionText.text = string.Format("{0}x{1}", Screen.width, Screen.height);
		}
		if (this.ApplyResolutionButton)
		{
			this.ApplyResolutionButton.interactable = (Screen.width != this.ResolutionsList[this.CurrentResolutionIndex].width || Screen.height != this.ResolutionsList[this.CurrentResolutionIndex].height);
		}
		if (this.Fullscreen)
		{
			this.Fullscreen.isOn = Screen.fullScreen;
		}
		if (this.ApplyResolutionButton)
		{
			this.ApplyResolutionButton.interactable = false;
		}
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00042E8D File Offset: 0x0004108D
	private void OnDisable()
	{
		if (GameLoad.Instance)
		{
			GameLoad.Instance.SaveOptions();
		}
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00042EA5 File Offset: 0x000410A5
	public void ResetOptions()
	{
		if (GameLoad.Instance)
		{
			GameLoad.Instance.LoadDefaultOptions();
			GameLoad.Instance.SaveOptions();
		}
		this.CurrentGameOptions = GameLoad.GetGameOptions;
		this.OnEnable();
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00042ED8 File Offset: 0x000410D8
	public void ResetGame()
	{
		if (GameLoad.Instance)
		{
			GameLoad.Instance.DeleteAll();
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00042EF0 File Offset: 0x000410F0
	public void SetMusicVolume(float _Value)
	{
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = GameLoad.GetGameOptions;
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		this.CurrentGameOptions.NormalizedMusicVolume = Mathf.Clamp01(_Value);
		if (this.MusicValueText)
		{
			this.MusicValueText.text = (this.MusicSlider.value * 100f).ToString("0");
		}
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00042F6C File Offset: 0x0004116C
	public void SetAmbienceVolume(float _Value)
	{
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = GameLoad.GetGameOptions;
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		this.CurrentGameOptions.NormalizedAmbienceVolume = Mathf.Clamp01(_Value);
		if (this.AmbienceValueText)
		{
			this.AmbienceValueText.text = (this.AmbienceSlider.value * 100f).ToString("0");
		}
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00042FE8 File Offset: 0x000411E8
	public void SetSFXVolume(float _Value)
	{
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = GameLoad.GetGameOptions;
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		this.CurrentGameOptions.NormalizedSFXVolume = Mathf.Clamp01(_Value);
		if (this.SfxValueText)
		{
			this.SfxValueText.text = (this.SfxSlider.value * 100f).ToString("0");
		}
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00043064 File Offset: 0x00041264
	public void SetMouseWheelSensitivity(float _Value)
	{
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = GameLoad.GetGameOptions;
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		this.CurrentGameOptions.NormalizedMouseWheelSensitivity = Mathf.Clamp01(_Value);
		if (this.MouseWheelValueText)
		{
			this.MouseWheelValueText.text = (this.MouseWheelSlider.value * 100f).ToString("0");
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x000430DE File Offset: 0x000412DE
	public void SetFullScreen(bool _FullScreen)
	{
		if (_FullScreen != Screen.fullScreen)
		{
			Screen.fullScreen = _FullScreen;
		}
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x000430EE File Offset: 0x000412EE
	public void SetVSync(bool _VSync)
	{
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = GameLoad.GetGameOptions;
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		this.CurrentGameOptions.VSync = _VSync;
		QualitySettings.vSyncCount = (_VSync ? 1 : 0);
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0004312E File Offset: 0x0004132E
	public void SetSpecialEffects(bool _Value)
	{
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = GameLoad.GetGameOptions;
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		this.CurrentGameOptions.DisableSpecialEffects = !_Value;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00043165 File Offset: 0x00041365
	public void SetInvertHorizontalMouseScroll(bool _Value)
	{
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = GameLoad.GetGameOptions;
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		this.CurrentGameOptions.InvertHorizontalMouseScroll = _Value;
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x0004319C File Offset: 0x0004139C
	public void NextResolution()
	{
		this.CurrentResolutionIndex++;
		if (this.CurrentResolutionIndex >= this.ResolutionsList.Count)
		{
			this.CurrentResolutionIndex = 0;
		}
		if (this.ResolutionText)
		{
			this.ResolutionText.text = string.Format("{0}x{1}", this.ResolutionsList[this.CurrentResolutionIndex].width, this.ResolutionsList[this.CurrentResolutionIndex].height);
		}
		if (this.ApplyResolutionButton)
		{
			this.ApplyResolutionButton.interactable = (Screen.width != this.ResolutionsList[this.CurrentResolutionIndex].width || Screen.height != this.ResolutionsList[this.CurrentResolutionIndex].height);
		}
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00043290 File Offset: 0x00041490
	public void PrevResolution()
	{
		this.CurrentResolutionIndex--;
		if (this.CurrentResolutionIndex <= -1)
		{
			this.CurrentResolutionIndex = this.ResolutionsList.Count - 1;
		}
		if (this.ResolutionText)
		{
			this.ResolutionText.text = string.Format("{0}x{1}", this.ResolutionsList[this.CurrentResolutionIndex].width, this.ResolutionsList[this.CurrentResolutionIndex].height);
		}
		if (this.ApplyResolutionButton)
		{
			this.ApplyResolutionButton.interactable = (Screen.width != this.ResolutionsList[this.CurrentResolutionIndex].width || Screen.height != this.ResolutionsList[this.CurrentResolutionIndex].height);
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00043384 File Offset: 0x00041584
	public void NextFontSet()
	{
		if (!MBSingleton<FontsManager>.Instance)
		{
			return;
		}
		int num = this.CurrentGameOptions.CurrentFontSet;
		if (num + 1 >= MBSingleton<FontsManager>.Instance.FontSetsCount)
		{
			num = 0;
		}
		else
		{
			num++;
		}
		this.CurrentGameOptions.CurrentFontSet = num;
		if (this.FontSetText)
		{
			this.FontSetText.text = (FontsManager.SelectedFontSet ? FontsManager.SelectedFontSet.SetName : "");
		}
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00043408 File Offset: 0x00041608
	public void PrevFontSet()
	{
		if (!MBSingleton<FontsManager>.Instance)
		{
			return;
		}
		int num = this.CurrentGameOptions.CurrentFontSet;
		if (num - 1 < 0)
		{
			num = MBSingleton<FontsManager>.Instance.FontSetsCount - 1;
		}
		else
		{
			num--;
		}
		this.CurrentGameOptions.CurrentFontSet = num;
		if (this.FontSetText)
		{
			this.FontSetText.text = (FontsManager.SelectedFontSet ? FontsManager.SelectedFontSet.SetName : "");
		}
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00043490 File Offset: 0x00041690
	public void NextLanguage()
	{
		if (!MBSingleton<LocalizationManager>.Instance)
		{
			return;
		}
		if (this.CurrentLanguageIndex + 1 >= MBSingleton<LocalizationManager>.Instance.Languages.Length)
		{
			this.CurrentLanguageIndex = 0;
		}
		else
		{
			this.CurrentLanguageIndex++;
		}
		while (!LocalizationManager.ValidLanguageIndex(this.CurrentLanguageIndex))
		{
			if (this.CurrentLanguageIndex + 1 >= MBSingleton<LocalizationManager>.Instance.Languages.Length)
			{
				this.CurrentLanguageIndex = 0;
			}
			else
			{
				this.CurrentLanguageIndex++;
			}
		}
		if (this.LanguageText)
		{
			this.LanguageText.text = MBSingleton<LocalizationManager>.Instance.Languages[this.CurrentLanguageIndex].LanguageName;
		}
		if (this.ChangeLanguageButton)
		{
			this.ChangeLanguageButton.interactable = (this.CurrentGameOptions.CurrentLanguage != this.CurrentLanguageIndex);
		}
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00043574 File Offset: 0x00041774
	public void PrevLanguage()
	{
		if (!MBSingleton<LocalizationManager>.Instance)
		{
			return;
		}
		if (this.CurrentLanguageIndex - 1 < 0)
		{
			this.CurrentLanguageIndex = MBSingleton<LocalizationManager>.Instance.Languages.Length - 1;
		}
		else
		{
			this.CurrentLanguageIndex--;
		}
		while (!LocalizationManager.ValidLanguageIndex(this.CurrentLanguageIndex))
		{
			if (this.CurrentLanguageIndex - 1 < 0)
			{
				this.CurrentLanguageIndex = MBSingleton<LocalizationManager>.Instance.Languages.Length - 1;
			}
			else
			{
				this.CurrentLanguageIndex--;
			}
		}
		if (this.LanguageText)
		{
			this.LanguageText.text = MBSingleton<LocalizationManager>.Instance.Languages[this.CurrentLanguageIndex].LanguageName;
		}
		if (this.ChangeLanguageButton)
		{
			this.ChangeLanguageButton.interactable = (this.CurrentGameOptions.CurrentLanguage != this.CurrentLanguageIndex);
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0004365C File Offset: 0x0004185C
	public void ApplyLanguage()
	{
		if (this.CurrentGameOptions.CurrentLanguage == this.CurrentLanguageIndex)
		{
			return;
		}
		if (this.ChangeLanguageConfirm.activeInHierarchy)
		{
			this.CurrentGameOptions.UsingCustomLanguage = true;
			this.CurrentGameOptions.CurrentLanguage = this.CurrentLanguageIndex;
			return;
		}
		this.ChangeLanguageConfirm.SetActive(true);
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x000436B4 File Offset: 0x000418B4
	public void ApplyResolution()
	{
		Screen.SetResolution(this.ResolutionsList[this.CurrentResolutionIndex].width, this.ResolutionsList[this.CurrentResolutionIndex].height, Screen.fullScreen);
		if (this.ApplyResolutionButton)
		{
			this.ApplyResolutionButton.interactable = false;
		}
	}

	// Token: 0x040008C7 RID: 2247
	private GameOptions CurrentGameOptions;

	// Token: 0x040008C8 RID: 2248
	[SerializeField]
	private Slider MusicSlider;

	// Token: 0x040008C9 RID: 2249
	[SerializeField]
	private TextMeshProUGUI MusicValueText;

	// Token: 0x040008CA RID: 2250
	[SerializeField]
	private Slider AmbienceSlider;

	// Token: 0x040008CB RID: 2251
	[SerializeField]
	private TextMeshProUGUI AmbienceValueText;

	// Token: 0x040008CC RID: 2252
	[SerializeField]
	private Slider SfxSlider;

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private TextMeshProUGUI SfxValueText;

	// Token: 0x040008CE RID: 2254
	[SerializeField]
	private GameObject[] MouseWheelObjects;

	// Token: 0x040008CF RID: 2255
	[SerializeField]
	private Slider MouseWheelSlider;

	// Token: 0x040008D0 RID: 2256
	[SerializeField]
	private TextMeshProUGUI MouseWheelValueText;

	// Token: 0x040008D1 RID: 2257
	[SerializeField]
	private Toggle HorizontalScrollDirectionToggle;

	// Token: 0x040008D2 RID: 2258
	[SerializeField]
	private TextMeshProUGUI ResolutionText;

	// Token: 0x040008D3 RID: 2259
	[SerializeField]
	private Button ApplyResolutionButton;

	// Token: 0x040008D4 RID: 2260
	[SerializeField]
	private Toggle Fullscreen;

	// Token: 0x040008D5 RID: 2261
	[SerializeField]
	private Toggle SpecialEffectsToggle;

	// Token: 0x040008D6 RID: 2262
	[SerializeField]
	private GameObject VSyncOptionObject;

	// Token: 0x040008D7 RID: 2263
	[SerializeField]
	private Toggle VSyncToggle;

	// Token: 0x040008D8 RID: 2264
	[SerializeField]
	private GameObject FontSetOption;

	// Token: 0x040008D9 RID: 2265
	[SerializeField]
	private TextMeshProUGUI FontSetText;

	// Token: 0x040008DA RID: 2266
	[SerializeField]
	private TextMeshProUGUI LanguageText;

	// Token: 0x040008DB RID: 2267
	[SerializeField]
	private GameObject[] ChangeLanguageObjects;

	// Token: 0x040008DC RID: 2268
	[SerializeField]
	private Button ChangeLanguageButton;

	// Token: 0x040008DD RID: 2269
	[SerializeField]
	private GameObject ChangeLanguageConfirm;

	// Token: 0x040008DE RID: 2270
	private List<Resolution> ResolutionsList = new List<Resolution>();

	// Token: 0x040008DF RID: 2271
	private List<string> ResolutionOptions = new List<string>();

	// Token: 0x040008E0 RID: 2272
	private int CurrentResolutionIndex;

	// Token: 0x040008E1 RID: 2273
	private int CurrentLanguageIndex;
}
