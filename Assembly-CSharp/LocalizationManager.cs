using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000191 RID: 401
public class LocalizationManager : MBSingleton<LocalizationManager>
{
	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0005E11A File Offset: 0x0005C31A
	// (set) Token: 0x06000A9A RID: 2714 RVA: 0x0005E121 File Offset: 0x0005C321
	public static int CurrentLanguage { get; private set; }

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000A9B RID: 2715 RVA: 0x0005E12C File Offset: 0x0005C32C
	public static int GetDefaultLanguage
	{
		get
		{
			if (!MBSingleton<LocalizationManager>.Instance)
			{
				return 0;
			}
			if (!LocalizationManager.CanChangeLanguage)
			{
				return 0;
			}
			if (!MBSingleton<LocalizationManager>.Instance.AutoDetectLanguage)
			{
				return MBSingleton<LocalizationManager>.Instance.DefaultLanguage;
			}
			for (int i = 0; i < MBSingleton<LocalizationManager>.Instance.Languages.Length; i++)
			{
				if ((!MBSingleton<LocalizationManager>.Instance.Languages[i].OnlyInDevBuild || Debug.isDebugBuild) && MBSingleton<LocalizationManager>.Instance.Languages[i].AssociatedLanguages.Contains(Application.systemLanguage))
				{
					return i;
				}
			}
			return MBSingleton<LocalizationManager>.Instance.DefaultLanguage;
		}
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0005E1CC File Offset: 0x0005C3CC
	public static bool ValidLanguageIndex(int _Index)
	{
		return !MBSingleton<LocalizationManager>.Instance || !LocalizationManager.CanChangeLanguage || (_Index >= 0 && _Index < MBSingleton<LocalizationManager>.Instance.Languages.Length && (!MBSingleton<LocalizationManager>.Instance.Languages[_Index].OnlyInDevBuild || Debug.isDebugBuild));
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0005E224 File Offset: 0x0005C424
	public static bool CanChangeLanguage
	{
		get
		{
			return MBSingleton<LocalizationManager>.Instance && MBSingleton<LocalizationManager>.Instance.Languages != null && MBSingleton<LocalizationManager>.Instance.Languages.Length > 1 && (Debug.isDebugBuild || MBSingleton<LocalizationManager>.Instance.ChangeLanguageOption);
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0005E274 File Offset: 0x0005C474
	public static bool LanguageDoesNotSupportStylizedFont
	{
		get
		{
			return MBSingleton<LocalizationManager>.Instance && MBSingleton<LocalizationManager>.Instance.Languages != null && LocalizationManager.CurrentLanguage >= 0 && LocalizationManager.CurrentLanguage < MBSingleton<LocalizationManager>.Instance.Languages.Length && MBSingleton<LocalizationManager>.Instance.Languages[LocalizationManager.CurrentLanguage].DoesntSupportsStylizedFont;
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0005E2D4 File Offset: 0x0005C4D4
	public static bool LanguageDoesNotSupportUnderlined
	{
		get
		{
			return MBSingleton<LocalizationManager>.Instance && MBSingleton<LocalizationManager>.Instance.Languages != null && LocalizationManager.CurrentLanguage >= 0 && LocalizationManager.CurrentLanguage < MBSingleton<LocalizationManager>.Instance.Languages.Length && MBSingleton<LocalizationManager>.Instance.Languages[LocalizationManager.CurrentLanguage].DoesntSupportUnderline;
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0005E334 File Offset: 0x0005C534
	public static bool LanguageTranslatesSpecialTexts
	{
		get
		{
			return MBSingleton<LocalizationManager>.Instance && MBSingleton<LocalizationManager>.Instance.Languages != null && LocalizationManager.CurrentLanguage >= 0 && LocalizationManager.CurrentLanguage < MBSingleton<LocalizationManager>.Instance.Languages.Length && MBSingleton<LocalizationManager>.Instance.Languages[LocalizationManager.CurrentLanguage].TranslateSpecialTexts;
		}
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0000B946 File Offset: 0x00009B46
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0005E394 File Offset: 0x0005C594
	public static void SetLanguage(int _Language, bool _DontLoadScene)
	{
		if (!MBSingleton<LocalizationManager>.Instance)
		{
			return;
		}
		LocalizationManager.CurrentLanguage = (LocalizationManager.CanChangeLanguage ? _Language : 0);
		LocalizationManager.LoadLanguage();
		if (_DontLoadScene)
		{
			return;
		}
		if (MBSingleton<GameManager>.Instance)
		{
			MBSingleton<GameManager>.Instance.QuitGame();
			GameLoad.Instance.AutoSaveGame(false);
		}
		SceneManager.LoadScene(GameLoad.Instance.MenuSceneIndex);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0005E3F8 File Offset: 0x0005C5F8
	public static void LoadLanguage()
	{
		if (!MBSingleton<LocalizationManager>.Instance)
		{
			return;
		}
		if (LocalizationManager.CurrentTexts == null)
		{
			LocalizationManager.CurrentTexts = new Dictionary<string, string>();
		}
		else
		{
			LocalizationManager.CurrentTexts.Clear();
		}
		if (LocalizationManager.CurrentLanguage < 0 || LocalizationManager.CurrentLanguage >= MBSingleton<LocalizationManager>.Instance.Languages.Length)
		{
			return;
		}
		if (MBSingleton<LocalizationManager>.Instance.Languages[LocalizationManager.CurrentLanguage].OnlyInDevBuild && !Debug.isDebugBuild)
		{
			return;
		}
		string localizationString = MBSingleton<LocalizationManager>.Instance.Languages[LocalizationManager.CurrentLanguage].GetLocalizationString();
		if (string.IsNullOrEmpty(localizationString))
		{
			return;
		}
		Dictionary<string, List<string>> dictionary = CSVParser.LoadFromString(localizationString, Delimiter.Comma);
		Regex regex = new Regex("\\\\n");
		foreach (KeyValuePair<string, List<string>> keyValuePair in dictionary)
		{
			if (!LocalizationManager.CurrentTexts.ContainsKey(keyValuePair.Key) && keyValuePair.Value.Count >= 2)
			{
				LocalizationManager.CurrentTexts.Add(keyValuePair.Key, regex.Replace(keyValuePair.Value[1], "\n"));
			}
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0005E528 File Offset: 0x0005C728
	public static bool GetText(string _Key, out string _Result)
	{
		_Result = null;
		if (LocalizationManager.CurrentTexts == null)
		{
			return false;
		}
		if (!LocalizationManager.CurrentTexts.ContainsKey(_Key))
		{
			return false;
		}
		_Result = LocalizationManager.CurrentTexts[_Key];
		return true;
	}

	// Token: 0x0400104D RID: 4173
	private static Dictionary<string, string> CurrentTexts;

	// Token: 0x0400104E RID: 4174
	public LanguageSetting[] Languages;

	// Token: 0x0400104F RID: 4175
	public bool ChangeLanguageOption;

	// Token: 0x04001050 RID: 4176
	public bool AutoDetectLanguage;

	// Token: 0x04001051 RID: 4177
	public int DefaultLanguage;
}
