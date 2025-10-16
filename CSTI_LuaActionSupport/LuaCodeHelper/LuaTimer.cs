using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.Helper;
using HarmonyLib;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200002D RID: 45
	[NullableContext(1)]
	[Nullable(0)]
	public static class LuaTimer
	{
		// Token: 0x060000EA RID: 234 RVA: 0x000052C4 File Offset: 0x000034C4
		[NullableContext(2)]
		public static Coroutine Wait4CA()
		{
			if (!MBSingleton<GameManager>.Instance)
			{
				return null;
			}
			Queue<CoroutineController> controllers = MBSingleton<GameManager>.Instance.ProcessCache();
			return LuaSupportRuntime.Runtime.StartCoroutine(LuaTimer.<Wait4CA>g__waitAll|1_0(controllers));
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000052FA File Offset: 0x000034FA
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "PerformingAction", MethodType.Getter)]
		public static void SetOnWaitCA(ref bool __result)
		{
			__result |= LuaTimer.OnWaitCA;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005306 File Offset: 0x00003506
		[LuaFunc]
		public static void ProcessCacheEnum()
		{
			MBSingleton<GameManager>.Instance.ProcessCache();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005313 File Offset: 0x00003513
		[LuaFunc]
		public static void Frame(LuaFunction function)
		{
			LuaTimer.FrameFunctions.Add(function);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005320 File Offset: 0x00003520
		[LuaFunc]
		public static void FixFrame(LuaFunction function)
		{
			LuaTimer.FixFrameFunctions.Add(function);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000532D File Offset: 0x0000352D
		[LuaFunc]
		public static void EveryTime(LuaFunction function, float time = 0.1f)
		{
			LuaTimer.EveryTimeFunctions.Add(function, new LuaTimer.SimpleTimer(time, 0f));
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005345 File Offset: 0x00003545
		[LuaFunc]
		public static float FrameTime()
		{
			return Time.deltaTime;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000534C File Offset: 0x0000354C
		[LuaFunc]
		public static float FixFrameTime()
		{
			return Time.fixedDeltaTime;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005354 File Offset: 0x00003554
		[LuaFunc]
		public static void StartCoroutine(LuaFunction function)
		{
			LuaTimer.<>c__DisplayClass13_0 CS$<>8__locals1 = new LuaTimer.<>c__DisplayClass13_0();
			CS$<>8__locals1.function = function;
			LuaSupportRuntime.Runtime.StartCoroutine(CS$<>8__locals1.<StartCoroutine>g__Coroutine|0());
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000539F File Offset: 0x0000359F
		[CompilerGenerated]
		internal static IEnumerator <Wait4CA>g__waitAll|1_0(Queue<CoroutineController> controllers)
		{
			while (LuaTimer.OnWaitCA)
			{
				yield return null;
			}
			LuaTimer.OnWaitCA = true;
			while (controllers.Count > 0)
			{
				CoroutineController coroutineController = controllers.Dequeue();
				while (coroutineController.state != CoroutineState.Finished)
				{
					yield return null;
				}
				coroutineController = null;
			}
			LuaTimer.OnWaitCA = false;
			yield break;
		}

		// Token: 0x04000058 RID: 88
		private static bool OnWaitCA;

		// Token: 0x04000059 RID: 89
		public static readonly List<LuaFunction> FrameFunctions = new List<LuaFunction>();

		// Token: 0x0400005A RID: 90
		public static readonly List<LuaFunction> FixFrameFunctions = new List<LuaFunction>();

		// Token: 0x0400005B RID: 91
		public static readonly Dictionary<LuaFunction, LuaTimer.SimpleTimer> EveryTimeFunctions = new Dictionary<LuaFunction, LuaTimer.SimpleTimer>();

		// Token: 0x0200002E RID: 46
		[NullableContext(0)]
		public class SimpleTimer
		{
			// Token: 0x060000F5 RID: 245 RVA: 0x000053AE File Offset: 0x000035AE
			public SimpleTimer(float time, float curTime)
			{
			}

			// Token: 0x0400005C RID: 92
			public readonly float Time = time;

			// Token: 0x0400005D RID: 93
			public float CurTime = curTime;
		}
	}
}
