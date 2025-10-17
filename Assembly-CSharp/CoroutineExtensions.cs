using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000174 RID: 372
public static class CoroutineExtensions
{
	// Token: 0x06000A17 RID: 2583 RVA: 0x0005AA13 File Offset: 0x00058C13
	public static Coroutine StartCoroutineEx(this MonoBehaviour monoBehaviour, IEnumerator routine, out CoroutineController coroutineController)
	{
		if (routine == null)
		{
			throw new ArgumentNullException("routine");
		}
		coroutineController = new CoroutineController(routine);
		return monoBehaviour.StartCoroutine(coroutineController.Start());
	}
}
