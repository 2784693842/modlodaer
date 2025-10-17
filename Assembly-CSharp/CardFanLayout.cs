using System;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class CardFanLayout : MonoBehaviour
{
	// Token: 0x06000A03 RID: 2563 RVA: 0x0005A150 File Offset: 0x00058350
	private void Update()
	{
		float num = (float)(base.transform.childCount - 1) * 0.5f * this.AngleDistanceBetweenChildren;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			float z = num - this.AngleDistanceBetweenChildren * (float)i;
			base.transform.GetChild(i).localPosition = new Vector3(0f, -this.Radius * this.EllipsisFactor, 0f) + Vector3.Scale(new Vector3(1f, this.EllipsisFactor, 1f), Quaternion.Euler(0f, 0f, z) * Vector3.up * this.Radius);
			base.transform.GetChild(i).localRotation = Quaternion.Euler(0f, 0f, z);
		}
	}

	// Token: 0x04000F73 RID: 3955
	public float Radius;

	// Token: 0x04000F74 RID: 3956
	public float EllipsisFactor;

	// Token: 0x04000F75 RID: 3957
	public float AngleDistanceBetweenChildren;
}
