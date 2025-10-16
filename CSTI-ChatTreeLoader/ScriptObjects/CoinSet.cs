using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ChatTreeLoader.LocalText;
using UnityEngine;

namespace ChatTreeLoader.ScriptObjects
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class CoinSet : ScriptableObject
	{
		// Token: 0x0600001F RID: 31 RVA: 0x000023B4 File Offset: 0x000005B4
		public bool ValidCoins(int count)
		{
			return (float)(from card in MBSingleton<GameManager>.Instance.AllCards
			select new ValueTuple<InGameCardBase, int>(card, this.CoinData.FirstOrDefault((CardDrop drop) => drop.DroppedCard == card.CardModel).Quantity.x) into tuple
			where tuple.Item2 != 0
			select tuple).Sum(([TupleElementNames(new string[]
			{
				"card",
				"coin"
			})] ValueTuple<InGameCardBase, int> tuple) => tuple.Item2) + this.CoinStat.Sum((GameStat stat) => MBSingleton<GameManager>.Instance.StatsDict[stat].SimpleCurrentValue) >= (float)count;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002457 File Offset: 0x00000657
		public IEnumerator CostCoin(int count)
		{
			IEnumerable<ValueTuple<InGameCardBase, int>> source = (from card in MBSingleton<GameManager>.Instance.AllCards
			select new ValueTuple<InGameCardBase, int>(card, this.CoinData.FirstOrDefault((CardDrop drop) => drop.DroppedCard == card.CardModel).Quantity.x) into tuple
			where tuple.Item2 != 0
			orderby tuple.Item2 descending
			select tuple).ToList<ValueTuple<InGameCardBase, int>>();
			List<InGameCardBase> list = new List<InGameCardBase>();
			float hadPay = 0f;
			Func<ValueTuple<InGameCardBase, int>, bool> <>9__4;
			Func<ValueTuple<InGameCardBase, int>, bool> predicate;
			if ((predicate = <>9__4) == null)
			{
				predicate = (<>9__4 = (([TupleElementNames(new string[]
				{
					"card",
					"coin"
				})] ValueTuple<InGameCardBase, int> tuple) => hadPay + (float)tuple.Item2 <= (float)count));
			}
			foreach (ValueTuple<InGameCardBase, int> valueTuple in source.Where(predicate))
			{
				hadPay += (float)valueTuple.Item2;
				list.Add(valueTuple.Item1);
			}
			Queue<CoroutineController> queue = new Queue<CoroutineController>();
			foreach (InGameCardBase card2 in list)
			{
				CoroutineController item;
				MBSingleton<GameManager>.Instance.StartCoroutineEx(MBSingleton<GameManager>.Instance.RemoveCard(card2, false, true, GameManager.RemoveOption.Standard, false), out item);
				queue.Enqueue(item);
			}
			if (hadPay < (float)count)
			{
				foreach (InGameStat inGameStat in from stat in this.CoinStat
				select MBSingleton<GameManager>.Instance.StatsDict[stat])
				{
					if (inGameStat.SimpleCurrentValue + hadPay > (float)count)
					{
						CoroutineController item2;
						MBSingleton<GameManager>.Instance.StartCoroutineEx(MBSingleton<GameManager>.Instance.ChangeStatValue(inGameStat, -((float)count - hadPay), StatModification.Permanent), out item2);
						hadPay = (float)count;
						queue.Enqueue(item2);
						break;
					}
					CoroutineController item3;
					MBSingleton<GameManager>.Instance.StartCoroutineEx(MBSingleton<GameManager>.Instance.ChangeStatValue(inGameStat, -inGameStat.SimpleCurrentValue, StatModification.Permanent), out item3);
					hadPay += inGameStat.SimpleCurrentValue;
					queue.Enqueue(item3);
				}
			}
			foreach (CoroutineController controller in queue)
			{
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
				controller = null;
			}
			Queue<CoroutineController>.Enumerator enumerator4 = default(Queue<CoroutineController>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002470 File Offset: 0x00000670
		public string Show()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CardDrop cardDrop in this.CoinData)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string format = "一个{0}算{1}元钱,".Local();
				object arg = cardDrop.DroppedCard.CardName;
				Vector2Int quantity = cardDrop.Quantity;
				stringBuilder2.AppendFormat(format, arg, quantity.x);
			}
			if (this.CoinStat.Count > 0)
			{
				stringBuilder.Append("\n");
				stringBuilder.Append("以下状态每一点算一元钱\n".Local());
				foreach (GameStat gameStat in this.CoinStat)
				{
					stringBuilder.AppendFormat("{0} ", gameStat.GameName);
				}
			}
			stringBuilder.Append("\n按下键盘上方向键再次显示".Local());
			return stringBuilder.ToString();
		}

		// Token: 0x0400002C RID: 44
		public List<CardDrop> CoinData = new List<CardDrop>();

		// Token: 0x0400002D RID: 45
		public List<GameStat> CoinStat = new List<GameStat>();
	}
}
