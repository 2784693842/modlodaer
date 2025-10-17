using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public class UIFeedbackTextAndIcon : UIFeedback
{
	// Token: 0x06000725 RID: 1829 RVA: 0x000480A0 File Offset: 0x000462A0
	protected override void Awake()
	{
		base.Awake();
		if (this.IconAndText)
		{
			return;
		}
		if (!this.IconAndText)
		{
			this.IconAndText = base.GetComponent<IconAndTextPair>();
		}
		if (this.PlayObject && !this.IconAndText)
		{
			this.IconAndText = this.PlayObject.GetComponent<IconAndTextPair>();
		}
		if (!this.IconAndText)
		{
			this.IconAndText = base.GetComponentInChildren<IconAndTextPair>();
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0004811E File Offset: 0x0004631E
	public IEnumerator PlayFeedback(string _Text)
	{
		if (this.IconAndText)
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Text, this.IconAndText.TextColor, null, this.IconAndText.IconColor));
		}
		else
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Text, Color.white, null, Color.white));
		}
		yield break;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00048134 File Offset: 0x00046334
	public IEnumerator PlayFeedback(Sprite _Icon)
	{
		if (this.IconAndText)
		{
			yield return base.StartCoroutine(this.PlayFeedback(this.IconAndText.Text, this.IconAndText.TextColor, _Icon, this.IconAndText.IconColor));
		}
		else
		{
			yield return base.StartCoroutine(this.PlayFeedback("", Color.white, _Icon, Color.white));
		}
		yield break;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x0004814A File Offset: 0x0004634A
	public IEnumerator PlayFeedback(string _Text, Sprite _Icon)
	{
		if (this.IconAndText)
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Text, this.IconAndText.TextColor, _Icon, this.IconAndText.IconColor));
		}
		else
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Text, Color.white, null, Color.white));
		}
		yield break;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00048167 File Offset: 0x00046367
	public IEnumerator PlayFeedback(Vector3 _Pos, string _Text)
	{
		if (this.IconAndText)
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Pos, _Text, this.IconAndText.TextColor, null, this.IconAndText.IconColor));
		}
		else
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Pos, _Text, Color.white, null, Color.white));
		}
		yield break;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00048184 File Offset: 0x00046384
	public IEnumerator PlayFeedback(Vector3 _Pos, Sprite _Icon)
	{
		if (this.IconAndText)
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Pos, this.IconAndText.Text, this.IconAndText.TextColor, _Icon, this.IconAndText.IconColor));
		}
		else
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Pos, "", Color.white, _Icon, Color.white));
		}
		yield break;
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x000481A1 File Offset: 0x000463A1
	public IEnumerator PlayFeedback(Vector3 _Pos, string _Text, Sprite _Icon)
	{
		if (this.IconAndText)
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Pos, _Text, this.IconAndText.TextColor, _Icon, this.IconAndText.IconColor));
		}
		else
		{
			yield return base.StartCoroutine(this.PlayFeedback(_Pos, _Text, Color.white, null, Color.white));
		}
		yield break;
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x000481C5 File Offset: 0x000463C5
	public IEnumerator PlayFeedback(string _Text, Color _TextColor, Sprite _Icon, Color _IconColor)
	{
		if (this.IconAndText)
		{
			this.IconAndText.Text = _Text;
			this.IconAndText.TextColor = _TextColor;
			this.IconAndText.Sprite = _Icon;
			this.IconAndText.IconColor = _IconColor;
		}
		yield return base.StartCoroutine(base.PlayFeedback());
		yield break;
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x000481F1 File Offset: 0x000463F1
	public IEnumerator PlayFeedback(Vector3 _Pos, string _Text, Color _TextColor, Sprite _Icon, Color _IconColor)
	{
		if (this.IconAndText)
		{
			this.IconAndText.Text = _Text;
			this.IconAndText.TextColor = _TextColor;
			this.IconAndText.Sprite = _Icon;
			this.IconAndText.IconColor = _IconColor;
		}
		yield return base.StartCoroutine(base.PlayFeedback(_Pos));
		yield break;
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00048225 File Offset: 0x00046425
	public void UpdateText(string _Text, Color _Color, bool _ResetTimer)
	{
		if (this.IconAndText)
		{
			this.IconAndText.Text = _Text;
			this.IconAndText.TextColor = _Color;
		}
		if (_ResetTimer)
		{
			base.ResetPlaying();
		}
	}

	// Token: 0x040009DE RID: 2526
	public IconAndTextPair IconAndText;
}
