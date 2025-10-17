using System;
using TMPro;
using UnityEngine;

// Token: 0x02000179 RID: 377
[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class DynamicFontText : MonoBehaviour
{
	// Token: 0x06000A23 RID: 2595 RVA: 0x0005AEFB File Offset: 0x000590FB
	private void Awake()
	{
		if (Application.isPlaying)
		{
			FontsManager.OnSelectedFontSet = (Action)Delegate.Combine(FontsManager.OnSelectedFontSet, new Action(this.OnFontSetChanged));
			this.OnFontSetChanged();
		}
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0005AF2C File Offset: 0x0005912C
	private void OnFontSetChanged()
	{
		FontSettings settingsForID = FontsManager.GetSettingsForID(this.FontID);
		this.ApplyFontSettings(settingsForID);
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0005AF4C File Offset: 0x0005914C
	private void ApplyFontSettings(FontSettings _Settings)
	{
		if (!FontsManager.UseFontManager)
		{
			return;
		}
		if (_Settings == null)
		{
			return;
		}
		if (!this.TargetText)
		{
			this.Setup();
		}
		if (_Settings.FontObject)
		{
			this.TargetText.font = _Settings.FontObject;
			this.TargetText.fontSharedMaterial = _Settings.FontMat;
		}
		if (!this.TargetText.enableAutoSizing)
		{
			this.TargetText.fontSize = this.FontSize * _Settings.SizeMultiplier;
		}
		this.TargetText.characterSpacing = this.CharacterSpacing + _Settings.CharacterSpacingOffset;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0005AFE4 File Offset: 0x000591E4
	private void Setup()
	{
		this.TargetText = base.GetComponent<TextMeshProUGUI>();
		this.FontSize = this.TargetText.fontSize;
		this.CharacterSpacing = this.TargetText.characterSpacing;
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0005B014 File Offset: 0x00059214
	private void Update()
	{
		if (!this.TargetText)
		{
			this.Setup();
		}
		if (!Application.isPlaying)
		{
			this.FontSize = this.TargetText.fontSize;
			this.CharacterSpacing = this.TargetText.characterSpacing;
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0005B054 File Offset: 0x00059254
	private void OnValidate()
	{
		if (!this.TargetText)
		{
			this.Setup();
		}
		if (!this.TargetText.enableAutoSizing)
		{
			this.TargetText.fontSize = this.FontSize;
		}
		this.TargetText.characterSpacing = this.CharacterSpacing;
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0005B0A3 File Offset: 0x000592A3
	private void OnDestroy()
	{
		FontsManager.OnSelectedFontSet = (Action)Delegate.Remove(FontsManager.OnSelectedFontSet, new Action(this.OnFontSetChanged));
	}

	// Token: 0x04000F9F RID: 3999
	private TextMeshProUGUI TargetText;

	// Token: 0x04000FA0 RID: 4000
	[SerializeField]
	private string FontID;

	// Token: 0x04000FA1 RID: 4001
	[SerializeField]
	private float FontSize;

	// Token: 0x04000FA2 RID: 4002
	[SerializeField]
	private float CharacterSpacing;
}
