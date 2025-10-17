using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class NewEquipmentContent : MonoBehaviour
{
	// Token: 0x06000642 RID: 1602 RVA: 0x00041DFB File Offset: 0x0003FFFB
	private void Update()
	{
		if (!this.EquipmentScript || !this.AlertObject)
		{
			return;
		}
		this.AlertObject.SetActive(this.EquipmentScript.HasNewCards());
	}

	// Token: 0x04000894 RID: 2196
	[SerializeField]
	private GameObject AlertObject;

	// Token: 0x04000895 RID: 2197
	[SerializeField]
	private CharacterScreen EquipmentScript;
}
