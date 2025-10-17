using System;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class FontsManager : MBSingleton<FontsManager>
{
	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0005B43F File Offset: 0x0005963F
	public static bool UseFontManager
	{
		get
		{
			return MBSingleton<FontsManager>.Instance && MBSingleton<FontsManager>.Instance.UsedInGame;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0005B459 File Offset: 0x00059659
	public int FontSetsCount
	{
		get
		{
			if (this.FontSets == null)
			{
				return 0;
			}
			return this.FontSets.Length;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0005B470 File Offset: 0x00059670
	public static FontSet SelectedFontSet
	{
		get
		{
			if (!MBSingleton<FontsManager>.Instance)
			{
				return null;
			}
			if (MBSingleton<FontsManager>.Instance.FontSets == null)
			{
				return null;
			}
			if (MBSingleton<FontsManager>.Instance.FontSets.Length == 0)
			{
				return null;
			}
			if (LocalizationManager.LanguageDoesNotSupportStylizedFont && MBSingleton<FontsManager>.Instance.FallbackSet)
			{
				return MBSingleton<FontsManager>.Instance.FallbackSet;
			}
			return MBSingleton<FontsManager>.Instance.FontSets[MBSingleton<FontsManager>.Instance.CurrentFontSet];
		}
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0005B4E0 File Offset: 0x000596E0
	public static void SelectFontSet(int _Index)
	{
		if (!MBSingleton<FontsManager>.Instance)
		{
			return;
		}
		if (MBSingleton<FontsManager>.Instance.FontSets == null)
		{
			return;
		}
		if (MBSingleton<FontsManager>.Instance.FontSets.Length == 0)
		{
			return;
		}
		MBSingleton<FontsManager>.Instance.CurrentFontSet = Mathf.Clamp(_Index, 0, MBSingleton<FontsManager>.Instance.FontSets.Length - 1);
		Action onSelectedFontSet = FontsManager.OnSelectedFontSet;
		if (onSelectedFontSet == null)
		{
			return;
		}
		onSelectedFontSet();
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x0005B544 File Offset: 0x00059744
	public static FontSettings GetSettingsForID(string _ID)
	{
		FontSet selectedFontSet = FontsManager.SelectedFontSet;
		if (!selectedFontSet)
		{
			return null;
		}
		return selectedFontSet.GetSetting(_ID);
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x0000B946 File Offset: 0x00009B46
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x04000FAE RID: 4014
	public static Action OnSelectedFontSet;

	// Token: 0x04000FAF RID: 4015
	[SerializeField]
	private bool UsedInGame;

	// Token: 0x04000FB0 RID: 4016
	[SerializeField]
	private FontSet[] FontSets;

	// Token: 0x04000FB1 RID: 4017
	[SerializeField]
	private FontSet FallbackSet;

	// Token: 0x04000FB2 RID: 4018
	private int CurrentFontSet;
}
