using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChatTreeLoader.LocalText;
using ChatTreeLoader.ScriptObjects;
using ChatTreeLoader.Util;
using UnityEngine;

namespace ChatTreeLoader.Behaviors
{
	// Token: 0x02000019 RID: 25
	public class TraderEncounterExt : ModEncounterExtBase<SimpleTraderEncounter>
	{
		// Token: 0x06000051 RID: 81 RVA: 0x0000391C File Offset: 0x00001B1C
		public override void DisplayChatModEncounter(EncounterPopup __instance)
		{
			SimpleTraderEncounter traderEncounter = SimpleTraderEncounter.SimpleTraderEncounters[__instance.CurrentEncounter.EncounterModel.UniqueID];
			InGameCardBase currentEnvironmentCard = MBSingleton<GameManager>.Instance.CurrentEnvironmentCard;
			TraderEncounterExt.TraderStage traderStage = TraderEncounterExt.TraderStage.empty;
			int stageInfo = 0;
			string key = null;
			foreach (KeyValuePair<string, Vector2Int> pair in currentEnvironmentCard.DroppedCollections)
			{
				string text;
				Vector2Int lhs;
				pair.Deconstruct(out text, out lhs);
				string text2 = text;
				if (!(lhs != Vector2Int.zero))
				{
					Match match = TraderEncounterExt.ActionRecordRegex.Match(text2);
					if (match != null && match.Success)
					{
						Enum.TryParse<TraderEncounterExt.TraderStage>(match.Groups["stage"].Value, true, out traderStage);
						int.TryParse(match.Groups["stageInfo"].Value, out stageInfo);
						key = text2;
						break;
					}
				}
			}
			switch (traderStage)
			{
			case TraderEncounterExt.TraderStage.ShowItem:
			case TraderEncounterExt.TraderStage.empty:
				__instance.CheckButtonCountAndEnable(Math.Min(5, 1 + traderEncounter.BuySets.Count - stageInfo * 4), delegate(EncounterPopup _, EncounterOptionButton button, int index)
				{
					if (index > 0)
					{
						BuySet buySet = traderEncounter.BuySets[stageInfo * 4 + index - 1];
						button.Setup(index, buySet.GetShowName(), null, false);
						button.Interactable = buySet.BuyCondition.ConditionsValid(false, MBSingleton<GameManager>.Instance.CurrentEnvironmentCard);
						button.gameObject.SetActive(buySet.BuyShowCondition.ConditionsValid(false, MBSingleton<GameManager>.Instance.CurrentEnvironmentCard));
						return;
					}
					button.Setup(index, "我该走了".Local(), null, false);
					button.Interactable = true;
					button.gameObject.SetActive(true);
				});
				break;
			case TraderEncounterExt.TraderStage.ShowCost:
				__instance.CheckButtonCountAndEnable(4, delegate(EncounterPopup _, EncounterOptionButton button, int index)
				{
					string text3;
					switch (index)
					{
					case 0:
						text3 = "确认购买".Local();
						break;
					case 1:
						text3 = "购买十份".Local();
						break;
					case 2:
						text3 = "再看看别的".Local();
						break;
					default:
						text3 = "我该走了".Local();
						break;
					}
					button.Setup(index, text3, null, false);
					bool interactable;
					if (index != 0)
					{
						interactable = (index != 1 || traderEncounter.CoinSet.ValidCoins(traderEncounter.BuySets[stageInfo].CalCost(traderEncounter) * 10));
					}
					else
					{
						interactable = traderEncounter.CoinSet.ValidCoins(traderEncounter.BuySets[stageInfo].CalCost(traderEncounter));
					}
					button.Interactable = interactable;
					button.gameObject.SetActive(true);
				});
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (traderStage == TraderEncounterExt.TraderStage.empty)
			{
				traderStage = TraderEncounterExt.TraderStage.ShowItem;
				currentEnvironmentCard.DroppedCollections.SafeRemove(key);
				currentEnvironmentCard.DroppedCollections[string.Format("__{{{0}}}TraderEncounter.Infos__{{Stage:{1},StageInfo:{2}}}", traderEncounter.ThisId, traderStage, stageInfo)] = Vector2Int.zero;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003AD4 File Offset: 0x00001CD4
		public override void DoModPlayerAction(EncounterPopup __instance, int _Action)
		{
			InGameCardBase instanceCurrentEnvironmentCard = MBSingleton<GameManager>.Instance.CurrentEnvironmentCard;
			string uniqueID = __instance.CurrentEncounter.EncounterModel.UniqueID;
			SimpleTraderEncounter simpleTraderEncounter = SimpleTraderEncounter.SimpleTraderEncounters[uniqueID];
			TraderEncounterExt.TraderStage traderStage = TraderEncounterExt.TraderStage.empty;
			int num = 0;
			string key = null;
			foreach (KeyValuePair<string, Vector2Int> pair in instanceCurrentEnvironmentCard.DroppedCollections)
			{
				string text;
				Vector2Int lhs;
				pair.Deconstruct(out text, out lhs);
				string text2 = text;
				if (!(lhs != Vector2Int.zero))
				{
					Match match = TraderEncounterExt.ActionRecordRegex.Match(text2);
					if (match != null && match.Success)
					{
						Enum.TryParse<TraderEncounterExt.TraderStage>(match.Groups["stage"].Value, true, out traderStage);
						int.TryParse(match.Groups["stageInfo"].Value, out num);
						key = text2;
						break;
					}
				}
			}
			switch (traderStage)
			{
			case TraderEncounterExt.TraderStage.ShowItem:
			{
				if (_Action == 0)
				{
					instanceCurrentEnvironmentCard.DroppedCollections.SafeRemove(key);
					__instance.ContinueButton.interactable = false;
					MBSingleton<GameManager>.Instance.StartCoroutine(ChatEncounterExt.WaitCloseWindow(__instance));
					MBSingleton<GraphicsManager>.Instance.CardsDestroyed.Setup(simpleTraderEncounter.TradeEndMessage, "你离开了".Local());
					instanceCurrentEnvironmentCard.DroppedCollections.SafeRemove(key);
					this.IsRunning = false;
					return;
				}
				traderStage = TraderEncounterExt.TraderStage.ShowCost;
				num = num * 4 + _Action - 1;
				BuySet buySet = simpleTraderEncounter.BuySets[num];
				__instance.AddLogSeparator(0f);
				if (buySet.UseBuySetMessage)
				{
					__instance.AddToLog(buySet.BuySetMessage);
					goto IL_42E;
				}
				__instance.AddToLog(new EncounterLogMessage(string.Format("要买这个，需要{0}元钱。".Local(), buySet.CalCost(simpleTraderEncounter))));
				goto IL_42E;
			}
			case TraderEncounterExt.TraderStage.ShowCost:
				switch (_Action)
				{
				case 0:
				{
					BuySet traderEncounterBuySet = simpleTraderEncounter.BuySets[num];
					traderEncounterBuySet.BuyResult.FillDropList(false, 1);
					traderEncounterBuySet.BuyResult.FillStatModsList();
					MBSingleton<GameManager>.Instance.StartCoroutine(simpleTraderEncounter.CoinSet.CostCoin(traderEncounterBuySet.CalCost(simpleTraderEncounter)).OnEnd(MBSingleton<GameManager>.Instance.ProduceCards(traderEncounterBuySet.BuyResult, null, false, false, false)).OnEnd(delegate()
					{
						if (traderEncounterBuySet.UseBuyEffect)
						{
							MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.ActionRoutine(traderEncounterBuySet.BuyEffect, instanceCurrentEnvironmentCard, false, false));
						}
					}));
					__instance.AddLogSeparator(0f);
					__instance.AddToLog(new EncounterLogMessage(string.Format("你购买了{0}, 花费{1}元钱".Local(), traderEncounterBuySet.GetShowName(), traderEncounterBuySet.CalCost(simpleTraderEncounter))));
					break;
				}
				case 1:
				{
					BuySet _buySet = simpleTraderEncounter.BuySets[num];
					_buySet.BuyResult.FillDropList(false, 10);
					_buySet.BuyResult.FillStatModsList();
					MBSingleton<GameManager>.Instance.StartCoroutine(simpleTraderEncounter.CoinSet.CostCoin(_buySet.CalCost(simpleTraderEncounter)).OnEnd(MBSingleton<GameManager>.Instance.ProduceCards(_buySet.BuyResult, null, false, false, false)).OnEnd(delegate()
					{
						if (_buySet.UseBuyEffect)
						{
							MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.ActionRoutine(_buySet.BuyEffect, instanceCurrentEnvironmentCard, false, false));
						}
					}));
					__instance.AddLogSeparator(0f);
					__instance.AddToLog(new EncounterLogMessage(string.Format("你购买了{0}, 花费{1}元钱".Local(), _buySet.GetShowName(), _buySet.CalCost(simpleTraderEncounter) * 10)));
					break;
				}
				case 3:
					instanceCurrentEnvironmentCard.DroppedCollections.SafeRemove(key);
					__instance.ContinueButton.interactable = false;
					MBSingleton<GameManager>.Instance.StartCoroutine(ChatEncounterExt.WaitCloseWindow(__instance));
					MBSingleton<GraphicsManager>.Instance.CardsDestroyed.Setup(simpleTraderEncounter.TradeEndMessage, "你离开了".Local());
					this.IsRunning = false;
					instanceCurrentEnvironmentCard.DroppedCollections.SafeRemove(key);
					return;
				}
				__instance.AddLogSeparator(0f);
				__instance.AddToLog(new EncounterLogMessage("按下键盘左右方向键换页(AD不行)".Local()));
				traderStage = TraderEncounterExt.TraderStage.ShowItem;
				num = 0;
				goto IL_42E;
			}
			throw new ArgumentOutOfRangeException();
			IL_42E:
			instanceCurrentEnvironmentCard.DroppedCollections.SafeRemove(key);
			instanceCurrentEnvironmentCard.DroppedCollections[string.Format("__{{{0}}}TraderEncounter.Infos__{{Stage:{1},StageInfo:{2}}}", simpleTraderEncounter.ThisId, traderStage, num)] = Vector2Int.zero;
			__instance.DisplayPlayerActions();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003F6C File Offset: 0x0000216C
		public override void ModRoundStart(EncounterPopup __instance, bool _Loaded)
		{
			string uniqueID = __instance.CurrentEncounter.EncounterModel.UniqueID;
			SimpleTraderEncounter simpleTraderEncounter = SimpleTraderEncounter.SimpleTraderEncounters[uniqueID];
			this.IsRunning = true;
			__instance.ActionsPlaying.Clear();
			Action<int> onNextRound = EncounterPopup.OnNextRound;
			if (onNextRound != null)
			{
				onNextRound(__instance.CurrentEncounter.CurrentRound);
			}
			if (__instance.ContinueButtonObject.activeSelf)
			{
				__instance.ContinueButtonObject.SetActive(false);
			}
			__instance.AddLogSeparator(0f);
			__instance.AddToLog(new EncounterLogMessage(simpleTraderEncounter.CoinSet.Show()));
			__instance.AddLogSeparator(0f);
			__instance.AddToLog(new EncounterLogMessage("按下键盘左右方向键换页(AD不行)".Local()));
			__instance.DisplayPlayerActions();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004024 File Offset: 0x00002224
		public override void UpdateModEx(EncounterPopup __instance)
		{
			SimpleTraderEncounter simpleTraderEncounter = SimpleTraderEncounter.SimpleTraderEncounters[__instance.CurrentEncounter.EncounterModel.UniqueID];
			InGameCardBase currentEnvironmentCard = MBSingleton<GameManager>.Instance.CurrentEnvironmentCard;
			TraderEncounterExt.TraderStage traderStage = TraderEncounterExt.TraderStage.empty;
			int num = 0;
			string key = null;
			bool flag = false;
			foreach (KeyValuePair<string, Vector2Int> pair in currentEnvironmentCard.DroppedCollections)
			{
				string text;
				Vector2Int lhs;
				pair.Deconstruct(out text, out lhs);
				string text2 = text;
				if (lhs == Vector2Int.zero)
				{
					Match match = TraderEncounterExt.ActionRecordRegex.Match(text2);
					if (match != null && match.Success)
					{
						Enum.TryParse<TraderEncounterExt.TraderStage>(match.Groups["stage"].Value, true, out traderStage);
						int.TryParse(match.Groups["stageInfo"].Value, out num);
						key = text2;
						break;
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				if (traderStage == TraderEncounterExt.TraderStage.ShowItem && num > 0)
				{
					flag = true;
					num--;
				}
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				if (traderStage == TraderEncounterExt.TraderStage.ShowItem && (num + 1) * 4 < simpleTraderEncounter.BuySets.Count)
				{
					flag = true;
					num++;
				}
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				__instance.AddLogSeparator(0f);
				__instance.AddToLog(new EncounterLogMessage(simpleTraderEncounter.CoinSet.Show()));
			}
			if (!flag)
			{
				return;
			}
			currentEnvironmentCard.DroppedCollections.SafeRemove(key);
			currentEnvironmentCard.DroppedCollections[string.Format("__{{{0}}}TraderEncounter.Infos__{{Stage:({1}),StageInfo:({2})}}", simpleTraderEncounter.ThisId, traderStage, num)] = Vector2Int.zero;
			__instance.DisplayPlayerActions();
		}

		// Token: 0x04000038 RID: 56
		private static readonly Regex ActionRecordRegex = new Regex("__\\{(?<EncounterId>.+?)\\}TraderEncounter\\.Infos__\\{Stage:(?<stage>.+?),StageInfo:(?<stageInfo>\\d+?)\\}");

		// Token: 0x0200002E RID: 46
		public enum TraderStage
		{
			// Token: 0x0400007D RID: 125
			ShowItem,
			// Token: 0x0400007E RID: 126
			ShowCost,
			// Token: 0x0400007F RID: 127
			empty
		}
	}
}
