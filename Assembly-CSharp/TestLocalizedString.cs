using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class TestLocalizedString : MonoBehaviour
{
	// Token: 0x06000BCE RID: 3022 RVA: 0x00063120 File Offset: 0x00061320
	private void Start()
	{
		if (this.LocalizationFile)
		{
			this.LocalizationValues = CSVParser.LoadFromString(this.LocalizationFile.text, Delimiter.Tab);
		}
		if (this.Test1)
		{
			this.Test1.text = this.LocalizationValues[LocalizedString.ActionHappening.LocalizationKey][1];
		}
		if (this.Test2)
		{
			this.Test2.text = this.LocalizationValues[LocalizedString.BlueprintResearchTab.LocalizationKey][1];
		}
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x000631B8 File Offset: 0x000613B8
	private void Update()
	{
		if (this.TweenTest)
		{
			this.TweenTest.text = this.From;
			Debug.Log(this.TweenTest.maxVisibleCharacters);
			this.TweenTest.maxVisibleCharacters = this.MaxVisibleCharacters;
			Debug.Log(this.TweenTest.textInfo.characterCount);
		}
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00063223 File Offset: 0x00061423
	[ContextMenu("Test tweening")]
	public void TestTween()
	{
		this.TweenTest.DOText(this.To, 1f, true, ScrambleMode.None, null);
	}

	// Token: 0x040010DC RID: 4316
	public TextAsset LocalizationFile;

	// Token: 0x040010DD RID: 4317
	public TextMeshProUGUI Test1;

	// Token: 0x040010DE RID: 4318
	public TextMeshProUGUI Test2;

	// Token: 0x040010DF RID: 4319
	public TextMeshProUGUI TweenTest;

	// Token: 0x040010E0 RID: 4320
	public string From;

	// Token: 0x040010E1 RID: 4321
	public string To;

	// Token: 0x040010E2 RID: 4322
	public int MaxVisibleCharacters;

	// Token: 0x040010E3 RID: 4323
	public Dictionary<string, List<string>> LocalizationValues = new Dictionary<string, List<string>>();
}
