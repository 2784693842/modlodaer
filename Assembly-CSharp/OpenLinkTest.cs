using System;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class OpenLinkTest : MonoBehaviour
{
	// Token: 0x06000B7A RID: 2938 RVA: 0x000614AD File Offset: 0x0005F6AD
	public void Open()
	{
		Application.OpenURL(this.Link);
	}

	// Token: 0x04001075 RID: 4213
	[SerializeField]
	private string Link;
}
