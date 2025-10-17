using System;
using UnityEngine;

// Token: 0x020001A8 RID: 424
[Serializable]
public class StringSaveReplace : SaveReplaceElement
{
	// Token: 0x06000BA4 RID: 2980 RVA: 0x00061EC0 File Offset: 0x000600C0
	public override string Pattern()
	{
		return this.Replace;
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00061EC8 File Offset: 0x000600C8
	public override string Replacement()
	{
		return this.By;
	}

	// Token: 0x04001089 RID: 4233
	[SerializeField]
	private string Replace;

	// Token: 0x0400108A RID: 4234
	[SerializeField]
	private string By;
}
