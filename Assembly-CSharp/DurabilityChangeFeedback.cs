using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200005F RID: 95
public class DurabilityChangeFeedback : MonoBehaviour
{
	// Token: 0x060003E7 RID: 999 RVA: 0x00028440 File Offset: 0x00026640
	public void Setup(Sprite _Icon, int _Value)
	{
		this.Icon.overrideSprite = _Icon;
		this.CurrentValue = _Value;
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00028455 File Offset: 0x00026655
	private void Update()
	{
		this.Trend.UpdateAnim((float)this.CurrentValue, new Vector2(-3f, 3f), false);
	}

	// Token: 0x04000501 RID: 1281
	[SerializeField]
	private Image Icon;

	// Token: 0x04000502 RID: 1282
	[SerializeField]
	private StatTrendIndicator Trend;

	// Token: 0x04000503 RID: 1283
	private int CurrentValue;
}
