using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class WeatherSpecialEffect : MonoBehaviour
{
	// Token: 0x06000738 RID: 1848 RVA: 0x0004840B File Offset: 0x0004660B
	public virtual void Remove()
	{
		UnityEngine.Object.Destroy(base.gameObject, this.DestroyDelay);
	}

	// Token: 0x040009E5 RID: 2533
	public float DestroyDelay;

	// Token: 0x040009E6 RID: 2534
	[NonSerialized]
	public InGameCardBase AssociatedCard;
}
