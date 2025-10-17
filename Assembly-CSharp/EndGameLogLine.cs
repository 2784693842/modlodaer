using System;
using TMPro;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class EndGameLogLine : MonoBehaviour
{
	// Token: 0x060004D5 RID: 1237 RVA: 0x00031960 File Offset: 0x0002FB60
	public void SetLogText(LogSaveData _Log)
	{
		EndgameLogCategory fromID = UniqueIDScriptable.GetFromID<EndgameLogCategory>(_Log.CategoryID);
		if (!fromID)
		{
			this.LogText.color = this.DefaultColor;
			this.LogText.fontStyle = this.DefaultFontStyle;
		}
		else
		{
			this.LogText.color = fromID.TextColor;
			this.LogText.fontStyle = (fromID.Bold ? FontStyles.Bold : this.DefaultFontStyle);
		}
		this.LogText.text = _Log.GetText;
	}

	// Token: 0x0400061B RID: 1563
	[SerializeField]
	private TextMeshProUGUI LogText;

	// Token: 0x0400061C RID: 1564
	[SerializeField]
	private Color DefaultColor;

	// Token: 0x0400061D RID: 1565
	[SerializeField]
	private FontStyles DefaultFontStyle;
}
