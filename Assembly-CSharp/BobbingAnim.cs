using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class BobbingAnim : MonoBehaviour
{
	// Token: 0x060002FD RID: 765 RVA: 0x0001EC28 File Offset: 0x0001CE28
	private void Update()
	{
		this.FreqTime += Time.deltaTime;
		if (this.FreqTime >= 1000f)
		{
			this.FreqTime -= 1000f;
		}
		this.AnimPos = this.AnimCurve.Evaluate(Mathf.Sin(6.2831855f * this.AnimFreq * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		if (!this.AnimParent)
		{
			this.AnimParent = this.AnimatedTr.parent.GetComponent<RectTransform>();
		}
		this.yPos = this.AnimParent.rect.center.y + Mathf.Lerp(-this.AnimYOffset, this.AnimYOffset, this.AnimPos);
		this.AnimatedTr.localPosition = new Vector3(this.AnimatedTr.localPosition.x, this.yPos, this.AnimatedTr.localPosition.z);
	}

	// Token: 0x04000397 RID: 919
	[SerializeField]
	private Transform AnimatedTr;

	// Token: 0x04000398 RID: 920
	[SerializeField]
	private float AnimYOffset;

	// Token: 0x04000399 RID: 921
	[SerializeField]
	private float AnimFreq;

	// Token: 0x0400039A RID: 922
	[SerializeField]
	private AnimationCurve AnimCurve;

	// Token: 0x0400039B RID: 923
	private RectTransform AnimParent;

	// Token: 0x0400039C RID: 924
	private float AnimPos;

	// Token: 0x0400039D RID: 925
	private float FreqTime;

	// Token: 0x0400039E RID: 926
	private float yPos;
}
