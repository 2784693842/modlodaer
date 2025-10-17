using System;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x020001FB RID: 507
	public static class DOTweenAnimationExtensions
	{
		// Token: 0x06000DBB RID: 3515 RVA: 0x0006CCBC File Offset: 0x0006AEBC
		public static bool IsSameOrSubclassOf<T>(this Component t)
		{
			return t is T;
		}
	}
}
