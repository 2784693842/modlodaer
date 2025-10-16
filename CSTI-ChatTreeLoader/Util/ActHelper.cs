using System;
using System.Collections;
using System.Collections.Generic;
using ChatTreeLoader.Patchers;

namespace ChatTreeLoader.Util
{
	// Token: 0x02000004 RID: 4
	public static class ActHelper
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000211D File Offset: 0x0000031D
		public static void WaitAll(this GameManager __instance, Queue<CoroutineController> queue)
		{
			__instance.StartCoroutine(ActHelper._inner_WaitAll(queue));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000212C File Offset: 0x0000032C
		private static IEnumerator _inner_WaitAll(Queue<CoroutineController> queue)
		{
			NormalPatcher.ShouldWaitExtra = true;
			while (queue.Count > 0)
			{
				CoroutineController coroutineController = queue.Dequeue();
				while (coroutineController.state == CoroutineState.Running)
				{
					yield return null;
				}
				coroutineController = null;
			}
			NormalPatcher.ShouldWaitExtra = false;
			yield break;
		}
	}
}
