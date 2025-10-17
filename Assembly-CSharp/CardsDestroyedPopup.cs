using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class CardsDestroyedPopup : MonoBehaviour
{
	// Token: 0x0600037A RID: 890 RVA: 0x00025374 File Offset: 0x00023574
	public void Setup(Dictionary<string, int> _CardsDestroyed, string _Title, string _Message)
	{
		this.TitleText.text = _Title;
		if (this.ItemsLostText)
		{
			this.ItemsLostText.gameObject.SetActive(true);
			if (string.IsNullOrEmpty(_Message))
			{
				this.ItemsLostText.text = this.DefaultLostMessage;
			}
			else
			{
				this.ItemsLostText.text = _Message;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		foreach (KeyValuePair<string, int> keyValuePair in _CardsDestroyed)
		{
			stringBuilder.Append(keyValuePair.Key);
			stringBuilder.Append(" x");
			stringBuilder.Append(keyValuePair.Value.ToString());
			if (num < 2)
			{
				stringBuilder.Append("   ");
				num++;
			}
			else
			{
				stringBuilder.Append("\n");
				num = 0;
			}
		}
		if (num == 0)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		else
		{
			stringBuilder.Remove(stringBuilder.Length - 3, 3);
		}
		this.DestroyedItemsList.text = stringBuilder.ToString();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x000254B0 File Offset: 0x000236B0
	public void Setup(string _Message, string _Title)
	{
		this.TitleText.text = _Title;
		this.DestroyedItemsList.text = _Message;
		if (this.ItemsLostText)
		{
			this.ItemsLostText.gameObject.SetActive(false);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x0400047F RID: 1151
	[SerializeField]
	private TextMeshProUGUI TitleText;

	// Token: 0x04000480 RID: 1152
	[SerializeField]
	private TextMeshProUGUI ItemsLostText;

	// Token: 0x04000481 RID: 1153
	[SerializeField]
	private TextMeshProUGUI DestroyedItemsList;

	// Token: 0x04000482 RID: 1154
	public LocalizedString DefaultLostMessage;
}
