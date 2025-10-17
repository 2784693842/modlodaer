using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class CardPooling : MBSingleton<CardPooling>
{
	// Token: 0x06000A05 RID: 2565 RVA: 0x0005A23C File Offset: 0x0005843C
	private static Queue<InGameCardBase> GetQueue(CardTypes _ForType)
	{
		if (!MBSingleton<CardPooling>.Instance)
		{
			return null;
		}
		switch (_ForType)
		{
		case CardTypes.Item:
			return MBSingleton<CardPooling>.Instance.ItemCardsQueue;
		case CardTypes.Base:
			return MBSingleton<CardPooling>.Instance.BaseCardsQueue;
		case CardTypes.Location:
			return MBSingleton<CardPooling>.Instance.LocationCardsQueue;
		case CardTypes.Event:
			return MBSingleton<CardPooling>.Instance.EventCardsQueue;
		case CardTypes.Environment:
			return MBSingleton<CardPooling>.Instance.EnvironmentCardsQueue;
		case CardTypes.Weather:
			return MBSingleton<CardPooling>.Instance.WeatherCardsQueue;
		case CardTypes.Hand:
			return MBSingleton<CardPooling>.Instance.HandCardsQueue;
		case CardTypes.Blueprint:
			return MBSingleton<CardPooling>.Instance.BlueprintModelCardsQueue;
		case CardTypes.Explorable:
			return MBSingleton<CardPooling>.Instance.ExplorableCardsQueue;
		case CardTypes.Liquid:
			return MBSingleton<CardPooling>.Instance.LiquidCardsQueue;
		case CardTypes.EnvImprovement:
			return MBSingleton<CardPooling>.Instance.ImprovementsCardsQueue;
		case CardTypes.EnvDamage:
			return MBSingleton<CardPooling>.Instance.EnvDamageCardsQueue;
		default:
			return null;
		}
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x0005A318 File Offset: 0x00058518
	private static InGameCardBase GetPrefab(CardTypes _ForType)
	{
		if (!CardPooling.GM)
		{
			CardPooling.GM = MBSingleton<GameManager>.Instance;
		}
		if (!CardPooling.GM || !MBSingleton<CardPooling>.Instance)
		{
			return null;
		}
		switch (_ForType)
		{
		case CardTypes.Item:
			return CardPooling.GM.ItemCardPrefab;
		case CardTypes.Base:
			return CardPooling.GM.BaseCardPrefab;
		case CardTypes.Location:
			return CardPooling.GM.LocationCardPrefab;
		case CardTypes.Event:
			return CardPooling.GM.EventCardPrefab;
		case CardTypes.Environment:
			return CardPooling.GM.EnvironmentCardPrefab;
		case CardTypes.Weather:
			return CardPooling.GM.WeatherCardPrefab;
		case CardTypes.Hand:
			return CardPooling.GM.HandCardPrefab;
		case CardTypes.Blueprint:
			return CardPooling.GM.BlueprintModelCardPrefab;
		case CardTypes.Explorable:
			return CardPooling.GM.ExplorableCardPrefab;
		case CardTypes.Liquid:
			return CardPooling.GM.LiquidCardPrefab;
		case CardTypes.EnvImprovement:
			return CardPooling.GM.ImprovementCardPrefab;
		case CardTypes.EnvDamage:
			return CardPooling.GM.EnvDamageCardPrefab;
		default:
			return null;
		}
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x0005A418 File Offset: 0x00058618
	public static InGameCardBase NextCard(Transform _Parent, Vector3 _Pos, CardData _Card, bool _BlueprintInstance)
	{
		if (!_Card || !MBSingleton<CardPooling>.Instance)
		{
			return null;
		}
		if (!CardPooling.GM)
		{
			CardPooling.GM = MBSingleton<GameManager>.Instance;
		}
		if (!CardPooling.GM)
		{
			return null;
		}
		Queue<InGameCardBase> queue = _BlueprintInstance ? MBSingleton<CardPooling>.Instance.BlueprintInstanceCardsQueue : CardPooling.GetQueue(_Card.CardType);
		if (queue.Count == 0)
		{
			return UnityEngine.Object.Instantiate<InGameCardBase>(_BlueprintInstance ? CardPooling.GM.BlueprintInstanceCardPrefab : CardPooling.GetPrefab(_Card.CardType), _Pos, Quaternion.identity, _Parent);
		}
		InGameCardBase inGameCardBase = queue.Dequeue();
		inGameCardBase.transform.position = _Pos;
		return inGameCardBase;
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0005A4BC File Offset: 0x000586BC
	public static void FreeCard(InGameCardBase _Card)
	{
		if (!_Card)
		{
			return;
		}
		if (!CardPooling.GM)
		{
			CardPooling.GM = MBSingleton<GameManager>.Instance;
		}
		if (!CardPooling.GM)
		{
			return;
		}
		if (!_Card.CardModel)
		{
			UnityEngine.Object.Destroy(_Card.gameObject);
			return;
		}
		Queue<InGameCardBase> queue = _Card.IsBlueprintInstance ? MBSingleton<CardPooling>.Instance.BlueprintInstanceCardsQueue : CardPooling.GetQueue(_Card.CardModel.CardType);
		if (!queue.Contains(_Card))
		{
			queue.Enqueue(_Card);
		}
	}

	// Token: 0x04000F76 RID: 3958
	public RectTransform PoolParentTr;

	// Token: 0x04000F77 RID: 3959
	private static GameManager GM;

	// Token: 0x04000F78 RID: 3960
	private Queue<InGameCardBase> HandCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F79 RID: 3961
	private Queue<InGameCardBase> EnvironmentCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F7A RID: 3962
	private Queue<InGameCardBase> LocationCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F7B RID: 3963
	private Queue<InGameCardBase> ItemCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F7C RID: 3964
	private Queue<InGameCardBase> BaseCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F7D RID: 3965
	private Queue<InGameCardBase> WeatherCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F7E RID: 3966
	private Queue<InGameCardBase> EventCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F7F RID: 3967
	private Queue<InGameCardBase> ExplorableCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F80 RID: 3968
	private Queue<InGameCardBase> BlueprintModelCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F81 RID: 3969
	private Queue<InGameCardBase> BlueprintInstanceCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F82 RID: 3970
	private Queue<InGameCardBase> LiquidCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F83 RID: 3971
	private Queue<InGameCardBase> ImprovementsCardsQueue = new Queue<InGameCardBase>();

	// Token: 0x04000F84 RID: 3972
	private Queue<InGameCardBase> EnvDamageCardsQueue = new Queue<InGameCardBase>();
}
