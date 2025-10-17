using System;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class WoundAlert : MonoBehaviour
{
	// Token: 0x06000743 RID: 1859 RVA: 0x00048764 File Offset: 0x00046964
	public bool IsWound(CardData _Card)
	{
		if (!_Card)
		{
			return false;
		}
		if (this.WoundTags == null)
		{
			return false;
		}
		if (this.WoundTags.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.WoundTags.Length; i++)
		{
			if (_Card.HasEquipmentTag(this.WoundTags[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x000487B8 File Offset: 0x000469B8
	private void Update()
	{
		if (!this.EquipmentScript || !this.AlertObject)
		{
			return;
		}
		bool flag = false;
		if (this.WoundTags != null)
		{
			for (int i = 0; i < this.WoundTags.Length; i++)
			{
				if (this.WoundTags[i] && this.EquipmentScript.EquippedTags.ContainsKey(this.WoundTags[i]))
				{
					flag = (this.EquipmentScript.EquippedTags[this.WoundTags[i]] > 0);
					if (flag)
					{
						break;
					}
				}
			}
		}
		this.AlertObject.SetActive(flag);
	}

	// Token: 0x040009F3 RID: 2547
	[SerializeField]
	private EquipmentTag[] WoundTags;

	// Token: 0x040009F4 RID: 2548
	[SerializeField]
	private GameObject AlertObject;

	// Token: 0x040009F5 RID: 2549
	[SerializeField]
	private CharacterScreen EquipmentScript;
}
