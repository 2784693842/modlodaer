using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.AllPatcher;
using UnityEngine;

namespace CSTI_LuaActionSupport.Helper
{
	// Token: 0x02000044 RID: 68
	[NullableContext(1)]
	[Nullable(0)]
	public static class CoroutineHelper
	{
		// Token: 0x06000150 RID: 336 RVA: 0x0000687C File Offset: 0x00004A7C
		public static Queue<CoroutineController> ProcessCache(this GameManager manager)
		{
			Queue<CoroutineController> queue = new Queue<CoroutineController>();
			foreach (IEnumerator routine in CardActionPatcher.Enumerators)
			{
				CoroutineController item;
				manager.StartCoroutineEx(routine, out item);
				queue.Enqueue(item);
			}
			CardActionPatcher.Enumerators.Clear();
			InspectionPopup[] array = UnityEngine.Object.FindObjectsOfType<InspectionPopup>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ReCommonSetup();
			}
			return queue;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006910 File Offset: 0x00004B10
		private static void ReCommonSetup(this InspectionPopup popup)
		{
			if (popup.isActiveAndEnabled && popup.CurrentCard)
			{
				popup.CommonSetup(popup.CurrentCard.CardName(false), popup.CurrentCard.CardDescription(false));
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006948 File Offset: 0x00004B48
		public static IEnumerator SpendDaytimePoints(this GameManager manager, int tp, InGameCardBase _ReceivingCard)
		{
			return manager.SpendDaytimePoints(tp, true, true, false, _ReceivingCard, FadeToBlackTypes.None, "", false, false, null, null, null);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000696C File Offset: 0x00004B6C
		public static CoroutineController Start(this IEnumerator enumerator, MonoBehaviour monoBehaviour)
		{
			CoroutineController result;
			monoBehaviour.StartCoroutineEx(enumerator, out result);
			return result;
		}
	}
}
