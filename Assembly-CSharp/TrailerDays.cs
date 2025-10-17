using System;
using TMPro;
using UnityEngine;

// Token: 0x020001B6 RID: 438
[ExecuteInEditMode]
public class TrailerDays : MonoBehaviour
{
	// Token: 0x06000BEE RID: 3054 RVA: 0x000634BD File Offset: 0x000616BD
	private void Update()
	{
		if (this.DaysText)
		{
			this.DaysText.text = string.Format("Day <color=red>{0}</color>", this.CurrentDay.ToString());
		}
	}

	// Token: 0x040010F1 RID: 4337
	public TextMeshProUGUI DaysText;

	// Token: 0x040010F2 RID: 4338
	public int CurrentDay;
}
