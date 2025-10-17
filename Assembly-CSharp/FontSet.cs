using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
[CreateAssetMenu(menuName = "Survival/Font Set")]
public class FontSet : ScriptableObject
{
	// Token: 0x060008F5 RID: 2293 RVA: 0x00055A18 File Offset: 0x00053C18
	public FontSettings GetSetting(string _Name)
	{
		if (this.Settings == null)
		{
			return null;
		}
		if (this.Settings.Length == 0)
		{
			return null;
		}
		for (int i = 0; i < this.Settings.Length; i++)
		{
			if (this.Settings[i].SettingsName == _Name)
			{
				return this.Settings[i];
			}
		}
		return null;
	}

	// Token: 0x04000E22 RID: 3618
	public LocalizedString SetName;

	// Token: 0x04000E23 RID: 3619
	public FontSettings[] Settings;
}
