using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class SkippableCoroutines : MonoBehaviour
{
	// Token: 0x06000BAE RID: 2990 RVA: 0x0006210B File Offset: 0x0006030B
	public static IEnumerator SkippableWait(float _Duration)
	{
		float timer = 0f;
		while (timer < _Duration)
		{
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				yield break;
			}
			timer += Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x0006211A File Offset: 0x0006031A
	public static IEnumerator SkippableWait(float _Duration, float _MinDuration)
	{
		float timer = 0f;
		while (timer < _Duration)
		{
			if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && timer >= _MinDuration)
			{
				yield break;
			}
			timer += Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001097 RID: 4247
	public const float BaseMinDuration = 0.3f;
}
