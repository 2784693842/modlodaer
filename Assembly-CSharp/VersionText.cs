using System;
using TMPro;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class VersionText : MonoBehaviour
{
	// Token: 0x06000CC4 RID: 3268 RVA: 0x000685B6 File Offset: 0x000667B6
	private void Start()
	{
		if (this.TextObject)
		{
			this.TextObject.text = "Version v1.05s";
		}
	}

	// Token: 0x040011B4 RID: 4532
	[SerializeField]
	private TextMeshProUGUI TextObject;
}
