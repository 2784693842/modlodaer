using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000033 RID: 51
	[NullableContext(1)]
	[Nullable(0)]
	public static class MoniEnum
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00005608 File Offset: 0x00003808
		public static IEnumerator MoniAddCard<[Nullable(2)] TArg>(this GameManager manager, CardData _Data, [Nullable(2)] InGameCardBase _FromCard, bool _InCurrentEnv, TransferedDurabilities _TransferedDurabilites, bool _UseDefaultInventory, SpawningLiquid _WithLiquid, Vector2Int _Tick, bool _MoveView, Action<InGameCardBase, TArg> action, TArg arg)
		{
			MoniEnum.<>c__DisplayClass2_0<TArg> CS$<>8__locals1 = new MoniEnum.<>c__DisplayClass2_0<TArg>();
			CS$<>8__locals1.manager = manager;
			CS$<>8__locals1._Data = _Data;
			CS$<>8__locals1.action = action;
			CS$<>8__locals1.arg = arg;
			IEnumerator addCard = CS$<>8__locals1.manager.AddCard(CS$<>8__locals1._Data, _FromCard, _InCurrentEnv, _TransferedDurabilites, _UseDefaultInventory, _WithLiquid, _Tick, _MoveView);
			bool b = true;
			while (b)
			{
				MoniEnum.OnMoniAddCard = true;
				MoniEnum.MoniFunc = new Func<IEnumerator, IEnumerator>(CS$<>8__locals1.<MoniAddCard>g___MoniFunc|0);
				b = addCard.MoveNext();
				MoniEnum.OnMoniAddCard = false;
				yield return addCard.Current;
			}
			yield break;
		}

		// Token: 0x04000066 RID: 102
		public static bool OnMoniAddCard;

		// Token: 0x04000067 RID: 103
		public static Func<IEnumerator, IEnumerator> MoniFunc;
	}
}
