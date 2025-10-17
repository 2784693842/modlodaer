using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009B RID: 155
public class ObjectiveToggle : MonoBehaviour
{
	// Token: 0x06000657 RID: 1623 RVA: 0x00042731 File Offset: 0x00040931
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00042739 File Offset: 0x00040939
	public void SetText(string _Text)
	{
		if (this.TextObject)
		{
			this.TextObject.text = _Text;
		}
		if (this.CompleteTextObject)
		{
			this.CompleteTextObject.text = _Text;
		}
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00042770 File Offset: 0x00040970
	public void SetTextObject(TextMeshProUGUI _TextObject)
	{
		this.TextObject = _TextObject;
		if (this.CompleteTextObject)
		{
			if (!_TextObject)
			{
				this.CompleteTextObject.gameObject.SetActive(false);
				return;
			}
			this.CompleteTextObject.text = _TextObject.text;
			this.CompleteTextObject.font = _TextObject.font;
			this.CompleteTextObject.fontSize = _TextObject.fontSize;
			this.CompleteTextObject.fontSizeMin = _TextObject.fontSizeMin;
			this.CompleteTextObject.fontSizeMax = _TextObject.fontSizeMax;
			this.CompleteTextObject.autoSizeTextContainer = _TextObject.autoSizeTextContainer;
			this.CompleteTextObject.transform.SetParent(_TextObject.transform.parent);
			this.CompleteTextObject.transform.SetSiblingIndex(_TextObject.transform.GetSiblingIndex());
			this.CompleteTextObject.transform.localScale = _TextObject.transform.localScale;
			RectTransform component = this.CompleteTextObject.GetComponent<RectTransform>();
			_TextObject.GetComponent<RectTransform>();
			component.anchorMin = component.anchorMin;
			component.anchorMax = component.anchorMax;
			component.anchoredPosition = component.anchoredPosition;
			component.sizeDelta = component.sizeDelta;
			if (this.AssociatedObjective)
			{
				this.CompleteTextObject.gameObject.SetActive(this.AssociatedObjective.Complete);
			}
			else
			{
				this.CompleteTextObject.gameObject.SetActive(false);
			}
			_TextObject.gameObject.SetActive(!this.CompleteTextObject.gameObject.activeInHierarchy);
		}
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00042900 File Offset: 0x00040B00
	public void Refresh()
	{
		bool flag = this.AssociatedObjective && this.AssociatedObjective.Complete;
		if (this.ToggleObject)
		{
			this.ToggleObject.isOn = flag;
		}
		if (this.TextObject)
		{
			this.TextObject.gameObject.SetActive(!flag);
		}
		if (this.CompleteTextObject)
		{
			this.CompleteTextObject.gameObject.SetActive(flag);
		}
	}

	// Token: 0x040008C3 RID: 2243
	[SerializeField]
	private Toggle ToggleObject;

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	private TextMeshProUGUI TextObject;

	// Token: 0x040008C5 RID: 2245
	[SerializeField]
	private TextMeshProUGUI CompleteTextObject;

	// Token: 0x040008C6 RID: 2246
	public Objective AssociatedObjective;
}
