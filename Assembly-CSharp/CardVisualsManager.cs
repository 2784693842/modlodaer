using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class CardVisualsManager : MBSingleton<CardVisualsManager>
{
	// Token: 0x06000374 RID: 884 RVA: 0x00024EE0 File Offset: 0x000230E0
	private static CardGraphics GetPrefab(CardTypes _ForType)
	{
		if (!MBSingleton<CardVisualsManager>.Instance)
		{
			return null;
		}
		switch (_ForType)
		{
		case CardTypes.Item:
			return MBSingleton<CardVisualsManager>.Instance.ItemCardVisualsPrefab;
		case CardTypes.Base:
			return MBSingleton<CardVisualsManager>.Instance.BaseCardVisualsPrefab;
		case CardTypes.Location:
			return MBSingleton<CardVisualsManager>.Instance.LocationCardVisualsPrefab;
		case CardTypes.Event:
			return MBSingleton<CardVisualsManager>.Instance.EventCardVisualsPrefab;
		case CardTypes.Environment:
			return MBSingleton<CardVisualsManager>.Instance.EnvironmentCardVisualsPrefab;
		case CardTypes.Weather:
			return MBSingleton<CardVisualsManager>.Instance.WeatherCardVisualsPrefab;
		case CardTypes.Hand:
			return MBSingleton<CardVisualsManager>.Instance.ItemCardVisualsPrefab;
		case CardTypes.Blueprint:
			return MBSingleton<CardVisualsManager>.Instance.BlueprintCardVisualsPrefab;
		case CardTypes.Explorable:
			return MBSingleton<CardVisualsManager>.Instance.ExplorableCardVisualsPrefab;
		case CardTypes.Liquid:
			return MBSingleton<CardVisualsManager>.Instance.LiquidCardVisualsPrefab;
		case CardTypes.EnvImprovement:
			return MBSingleton<CardVisualsManager>.Instance.ImprovementCardVisualsPrefab;
		case CardTypes.EnvDamage:
			return MBSingleton<CardVisualsManager>.Instance.DamageCardVisualsPrefab;
		default:
			return null;
		}
	}

	// Token: 0x06000375 RID: 885 RVA: 0x00024FBC File Offset: 0x000231BC
	private static Queue<CardGraphics> GetQueue(CardTypes _ForType)
	{
		if (!MBSingleton<CardVisualsManager>.Instance)
		{
			return null;
		}
		switch (_ForType)
		{
		case CardTypes.Item:
			return MBSingleton<CardVisualsManager>.Instance.ItemCardsQueue;
		case CardTypes.Base:
			return MBSingleton<CardVisualsManager>.Instance.BaseCardsQueue;
		case CardTypes.Location:
			return MBSingleton<CardVisualsManager>.Instance.LocationCardsQueue;
		case CardTypes.Event:
			return MBSingleton<CardVisualsManager>.Instance.EventCardsQueue;
		case CardTypes.Environment:
			return MBSingleton<CardVisualsManager>.Instance.EnvironmentCardsQueue;
		case CardTypes.Weather:
			return MBSingleton<CardVisualsManager>.Instance.WeatherCardsQueue;
		case CardTypes.Hand:
			return MBSingleton<CardVisualsManager>.Instance.ItemCardsQueue;
		case CardTypes.Blueprint:
			return MBSingleton<CardVisualsManager>.Instance.BlueprintModelCardsQueue;
		case CardTypes.Explorable:
			return MBSingleton<CardVisualsManager>.Instance.ExplorableCardsQueue;
		case CardTypes.Liquid:
			return MBSingleton<CardVisualsManager>.Instance.LiquidCardsQueue;
		case CardTypes.EnvImprovement:
			return MBSingleton<CardVisualsManager>.Instance.ImprovementsCardsQueue;
		case CardTypes.EnvDamage:
			return MBSingleton<CardVisualsManager>.Instance.EnvDamageCardsQueue;
		default:
			return null;
		}
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00025098 File Offset: 0x00023298
	public static CardGraphics NextCard(Transform _Parent, Vector3 _Pos, CardData _Card, bool _BlueprintInstance)
	{
		if (!_Card || !MBSingleton<CardVisualsManager>.Instance)
		{
			return null;
		}
		Queue<CardGraphics> queue = _BlueprintInstance ? MBSingleton<CardVisualsManager>.Instance.BlueprintInstanceCardsQueue : CardVisualsManager.GetQueue(_Card.CardType);
		if (queue.Count != 0)
		{
			CardGraphics cardGraphics = queue.Dequeue();
			cardGraphics.transform.SetParent(_Parent, true);
			cardGraphics.transform.position = _Pos;
			cardGraphics.transform.localScale = Vector3.one;
			cardGraphics.gameObject.SetActive(true);
			return cardGraphics;
		}
		CardGraphics cardGraphics2 = _BlueprintInstance ? MBSingleton<CardVisualsManager>.Instance.BlueprintInstanceCardVisualsPrefab : CardVisualsManager.GetPrefab(_Card.CardType);
		if (cardGraphics2)
		{
			CardGraphics cardGraphics3 = UnityEngine.Object.Instantiate<CardGraphics>(cardGraphics2, _Pos, Quaternion.identity, _Parent);
			cardGraphics3.PoolIndex = MBSingleton<CardVisualsManager>.Instance.CreatedCardsIndex;
			MBSingleton<CardVisualsManager>.Instance.CreatedCardsIndex++;
			return cardGraphics3;
		}
		return null;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00025170 File Offset: 0x00023370
	public static void FreeCard(CardGraphics _Card)
	{
		if (!_Card)
		{
			return;
		}
		Queue<CardGraphics> queue = _Card.IsBlueprintInstance ? MBSingleton<CardVisualsManager>.Instance.BlueprintInstanceCardsQueue : CardVisualsManager.GetQueue(_Card.CardType);
		if (!queue.Contains(_Card))
		{
			queue.Enqueue(_Card);
			_Card.gameObject.SetActive(false);
			_Card.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x000251D4 File Offset: 0x000233D4
	private static string DebugQueueName(Queue<CardGraphics> _Queue)
	{
		if (!MBSingleton<CardVisualsManager>.Instance)
		{
			return "NONE";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.ItemCardsQueue)
		{
			return "Items";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.BaseCardsQueue)
		{
			return "Base";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.LocationCardsQueue)
		{
			return "Locations";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.ExplorableCardsQueue)
		{
			return "Explorables";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.EventCardsQueue)
		{
			return "Events";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.EnvironmentCardsQueue)
		{
			return "Environments";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.WeatherCardsQueue)
		{
			return "Weather";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.BlueprintModelCardsQueue)
		{
			return "Blueprint Models";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.BlueprintInstanceCardsQueue)
		{
			return "Blueprint Instances";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.LiquidCardsQueue)
		{
			return "Liquid";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.ImprovementsCardsQueue)
		{
			return "Improvements";
		}
		if (_Queue == MBSingleton<CardVisualsManager>.Instance.EnvDamageCardsQueue)
		{
			return "Damages";
		}
		return "Not found";
	}

	// Token: 0x04000466 RID: 1126
	[SerializeField]
	private CardGraphics ItemCardVisualsPrefab;

	// Token: 0x04000467 RID: 1127
	[SerializeField]
	private CardGraphics BaseCardVisualsPrefab;

	// Token: 0x04000468 RID: 1128
	[SerializeField]
	private CardGraphics LocationCardVisualsPrefab;

	// Token: 0x04000469 RID: 1129
	[SerializeField]
	private CardGraphics ExplorableCardVisualsPrefab;

	// Token: 0x0400046A RID: 1130
	[SerializeField]
	private CardGraphics EventCardVisualsPrefab;

	// Token: 0x0400046B RID: 1131
	[SerializeField]
	private CardGraphics EnvironmentCardVisualsPrefab;

	// Token: 0x0400046C RID: 1132
	[SerializeField]
	private CardGraphics WeatherCardVisualsPrefab;

	// Token: 0x0400046D RID: 1133
	[SerializeField]
	private CardGraphics BlueprintCardVisualsPrefab;

	// Token: 0x0400046E RID: 1134
	[SerializeField]
	private CardGraphics BlueprintInstanceCardVisualsPrefab;

	// Token: 0x0400046F RID: 1135
	[SerializeField]
	private CardGraphics LiquidCardVisualsPrefab;

	// Token: 0x04000470 RID: 1136
	[SerializeField]
	private CardGraphics ImprovementCardVisualsPrefab;

	// Token: 0x04000471 RID: 1137
	[SerializeField]
	private CardGraphics DamageCardVisualsPrefab;

	// Token: 0x04000472 RID: 1138
	private Queue<CardGraphics> EnvironmentCardsQueue = new Queue<CardGraphics>();

	// Token: 0x04000473 RID: 1139
	private Queue<CardGraphics> LocationCardsQueue = new Queue<CardGraphics>();

	// Token: 0x04000474 RID: 1140
	private Queue<CardGraphics> ItemCardsQueue = new Queue<CardGraphics>();

	// Token: 0x04000475 RID: 1141
	private Queue<CardGraphics> BaseCardsQueue = new Queue<CardGraphics>();

	// Token: 0x04000476 RID: 1142
	private Queue<CardGraphics> WeatherCardsQueue = new Queue<CardGraphics>();

	// Token: 0x04000477 RID: 1143
	private Queue<CardGraphics> EventCardsQueue = new Queue<CardGraphics>();

	// Token: 0x04000478 RID: 1144
	private Queue<CardGraphics> ExplorableCardsQueue = new Queue<CardGraphics>();

	// Token: 0x04000479 RID: 1145
	private Queue<CardGraphics> BlueprintModelCardsQueue = new Queue<CardGraphics>();

	// Token: 0x0400047A RID: 1146
	private Queue<CardGraphics> BlueprintInstanceCardsQueue = new Queue<CardGraphics>();

	// Token: 0x0400047B RID: 1147
	private Queue<CardGraphics> LiquidCardsQueue = new Queue<CardGraphics>();

	// Token: 0x0400047C RID: 1148
	private Queue<CardGraphics> ImprovementsCardsQueue = new Queue<CardGraphics>();

	// Token: 0x0400047D RID: 1149
	private Queue<CardGraphics> EnvDamageCardsQueue = new Queue<CardGraphics>();

	// Token: 0x0400047E RID: 1150
	private int CreatedCardsIndex;
}
