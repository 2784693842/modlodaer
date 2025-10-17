using System;
using TMPro;
using UnityEngine;

// Token: 0x02000192 RID: 402
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedStaticText : MonoBehaviour
{
	// Token: 0x06000AA6 RID: 2726 RVA: 0x0005E55B File Offset: 0x0005C75B
	private void Awake()
	{
		this.LocalizedText = this.GetLocalizedText(false);
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0005E56A File Offset: 0x0005C76A
	private void OnEnable()
	{
		if (this.ChangeOnUpdate)
		{
			this.TargetText.text = this.LocalizedText;
		}
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0005E56A File Offset: 0x0005C76A
	private void Update()
	{
		if (this.ChangeOnUpdate)
		{
			this.TargetText.text = this.LocalizedText;
		}
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0005E58A File Offset: 0x0005C78A
	private void Start()
	{
		this.TargetText.text = this.LocalizedText;
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0005E5A4 File Offset: 0x0005C7A4
	public LocalizedString GetLocalizedText(bool _ForceKey)
	{
		LocalizedString result = default(LocalizedString);
		this.TargetText = base.GetComponent<TextMeshProUGUI>();
		if (this.IsSpecialText && !LocalizationManager.LanguageTranslatesSpecialTexts && !_ForceKey)
		{
			return new LocalizedString
			{
				LocalizationKey = "IGNOREKEY",
				DefaultText = this.TargetText.text
			};
		}
		if (string.IsNullOrEmpty(this.LocalizedStringKey))
		{
			result = new LocalizedString
			{
				LocalizationKey = this.TargetText.text
			};
		}
		else
		{
			result = new LocalizedString
			{
				LocalizationKey = this.LocalizedStringKey
			};
		}
		result.DefaultText = this.TargetText.text;
		return result;
	}

	// Token: 0x04001053 RID: 4179
	public string LocalizedStringKey;

	// Token: 0x04001054 RID: 4180
	[SerializeField]
	private bool IsSpecialText;

	// Token: 0x04001055 RID: 4181
	[SerializeField]
	private bool ChangeOnUpdate;

	// Token: 0x04001056 RID: 4182
	private TextMeshProUGUI TargetText;

	// Token: 0x04001057 RID: 4183
	private LocalizedString LocalizedText;
}
