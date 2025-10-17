using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class UIFeedbackText : UIFeedback
{
	// Token: 0x0600071C RID: 1820 RVA: 0x00047F48 File Offset: 0x00046148
	protected override void Awake()
	{
		base.Awake();
		if (this.TextObject)
		{
			return;
		}
		if (!this.TextObject)
		{
			this.TextObject = base.GetComponent<TextMeshProUGUI>();
		}
		if (this.PlayObject && !this.TextObject)
		{
			this.TextObject = this.PlayObject.GetComponent<TextMeshProUGUI>();
		}
		if (!this.TextObject)
		{
			this.TextObject = base.GetComponentInChildren<TextMeshProUGUI>();
		}
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00047FC6 File Offset: 0x000461C6
	public void UpdateText(string _Text, Color _Color, bool _ResetTimer)
	{
		if (this.TextObject)
		{
			this.TextObject.text = _Text;
			this.TextObject.color = _Color;
		}
		if (_ResetTimer)
		{
			base.ResetPlaying();
		}
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x00047FF6 File Offset: 0x000461F6
	public IEnumerator PlayFeedback(string _Text)
	{
		yield return base.StartCoroutine(this.PlayFeedback(_Text, this.TextObject ? this.TextObject.color : Color.white));
		yield break;
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0004800C File Offset: 0x0004620C
	public IEnumerator PlayFeedback(Color _Color)
	{
		yield return base.StartCoroutine(this.PlayFeedback(this.TextObject ? this.TextObject.text : "NULL", _Color));
		yield break;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00048022 File Offset: 0x00046222
	public IEnumerator PlayFeedback(Vector3 _Pos, string _Text)
	{
		yield return base.StartCoroutine(this.PlayFeedback(_Pos, _Text, this.TextObject ? this.TextObject.color : Color.white));
		yield break;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0004803F File Offset: 0x0004623F
	public IEnumerator PlayFeedback(Vector3 _Pos, Color _Color)
	{
		yield return base.StartCoroutine(this.PlayFeedback(_Pos, this.TextObject ? this.TextObject.text : "NULL", _Color));
		yield break;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0004805C File Offset: 0x0004625C
	public IEnumerator PlayFeedback(string _Text, Color _Color)
	{
		if (this.TextObject)
		{
			this.TextObject.text = _Text;
			this.TextObject.color = _Color;
		}
		yield return base.StartCoroutine(base.PlayFeedback());
		yield break;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00048079 File Offset: 0x00046279
	public IEnumerator PlayFeedback(Vector3 _Pos, string _Text, Color _Color)
	{
		if (this.TextObject)
		{
			this.TextObject.text = _Text;
			this.TextObject.color = _Color;
		}
		yield return base.StartCoroutine(base.PlayFeedback(_Pos));
		yield break;
	}

	// Token: 0x040009DD RID: 2525
	public TextMeshProUGUI TextObject;
}
