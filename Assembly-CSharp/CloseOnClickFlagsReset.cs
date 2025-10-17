using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
[DefaultExecutionOrder(10000)]
public class CloseOnClickFlagsReset : MonoBehaviour
{
	// Token: 0x06000398 RID: 920 RVA: 0x0002635D File Offset: 0x0002455D
	private void LateUpdate()
	{
		CloseOnClickOutside.CancelFlag = 0;
	}
}
