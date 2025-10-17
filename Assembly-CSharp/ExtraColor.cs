using System;
using UnityEngine;

// Token: 0x020001CD RID: 461
public static class ExtraColor
{
	// Token: 0x06000C74 RID: 3188 RVA: 0x00066048 File Offset: 0x00064248
	public static Color MoveTowards(Color _From, Color _To, float _MaxDelta)
	{
		return new Color(Mathf.MoveTowards(_From.r, _To.r, _MaxDelta), Mathf.MoveTowards(_From.g, _To.g, _MaxDelta), Mathf.MoveTowards(_From.b, _To.b, _MaxDelta), Mathf.MoveTowards(_From.a, _To.a, _MaxDelta));
	}
}
