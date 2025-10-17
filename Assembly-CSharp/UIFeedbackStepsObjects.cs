using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class UIFeedbackStepsObjects : UIFeedbackStepsBase
{
	// Token: 0x06000715 RID: 1813 RVA: 0x00047E87 File Offset: 0x00046087
	protected override IEnumerator CustomStepsProcessing(Sprite _Icon, int _Steps, bool _Negative)
	{
		if (!this.PositiveStepFeedback && !this.NegativeStepFeedback)
		{
			yield break;
		}
		GameObject gameObject = _Negative ? this.NegativeStepFeedback : this.PositiveStepFeedback;
		if (this.StepsParent && gameObject)
		{
			for (int i = 0; i < this.StepsParent.childCount; i++)
			{
				UnityEngine.Object.DestroyImmediate(this.StepsParent.GetChild(0).gameObject);
			}
			for (int j = 0; j < Mathf.Abs(_Steps); j++)
			{
				this.CurrentStepObjects.Add(UnityEngine.Object.Instantiate<GameObject>(gameObject, this.StepsParent));
			}
		}
		yield break;
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00047EA4 File Offset: 0x000460A4
	protected override void OnStop()
	{
		for (int i = this.CurrentStepObjects.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.CurrentStepObjects[i].gameObject);
			this.CurrentStepObjects.RemoveAt(i);
		}
	}

	// Token: 0x040009D5 RID: 2517
	[SerializeField]
	private GameObject PositiveStepFeedback;

	// Token: 0x040009D6 RID: 2518
	[SerializeField]
	private GameObject NegativeStepFeedback;

	// Token: 0x040009D7 RID: 2519
	[SerializeField]
	private RectTransform StepsParent;

	// Token: 0x040009D8 RID: 2520
	private List<GameObject> CurrentStepObjects = new List<GameObject>();
}
