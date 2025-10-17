using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000190 RID: 400
[Serializable]
public struct LanguageSetting
{
	// Token: 0x06000A97 RID: 2711 RVA: 0x0005E084 File Offset: 0x0005C284
	public string GetLocalizationString()
	{
		string text = null;
		try
		{
			text = this.FindTextAsset();
		}
		catch
		{
			return null;
		}
		if (string.IsNullOrEmpty(text) && this.LanguageFile != null)
		{
			return this.LanguageFile.text;
		}
		return text;
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0005E0D8 File Offset: 0x0005C2D8
	private string FindTextAsset()
	{
		if (string.IsNullOrEmpty(this.FilePath))
		{
			return null;
		}
		string path = string.Format("{0}/{1}", Application.streamingAssetsPath, this.FilePath);
		if (!File.Exists(path))
		{
			return null;
		}
		return File.ReadAllText(path);
	}

	// Token: 0x04001045 RID: 4165
	public string LanguageName;

	// Token: 0x04001046 RID: 4166
	[SerializeField]
	private TextAsset LanguageFile;

	// Token: 0x04001047 RID: 4167
	[SerializeField]
	private string FilePath;

	// Token: 0x04001048 RID: 4168
	public bool DoesntSupportsStylizedFont;

	// Token: 0x04001049 RID: 4169
	public bool DoesntSupportUnderline;

	// Token: 0x0400104A RID: 4170
	public bool TranslateSpecialTexts;

	// Token: 0x0400104B RID: 4171
	public bool OnlyInDevBuild;

	// Token: 0x0400104C RID: 4172
	public List<SystemLanguage> AssociatedLanguages;
}
