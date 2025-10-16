using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChatTreeLoader.Util;
using UnityEngine;

namespace ChatTreeLoader.Behaviors
{
	// Token: 0x02000013 RID: 19
	public class ChatEncounterExt : ModEncounterExtBase<ModEncounter>
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002F00 File Offset: 0x00001100
		public override void DisplayChatModEncounter(EncounterPopup __instance)
		{
			string encounterModelUniqueID = __instance.CurrentEncounter.EncounterModel.UniqueID;
			__instance.EncounterPlayerActions.Clear();
			ModEncounter modEncounter = ModEncounter.ModEncounters[encounterModelUniqueID];
			IEnumerable<string> keys = MBSingleton<GameManager>.Instance.CurrentEnvironmentCard.DroppedCollections.Keys;
			List<int> list = new List<int>();
			IEnumerable<Match> source = from key in keys
			select ChatEncounterExt.ActionRecordRegex.Match(key) into match
			where match.Success
			select match;
			Func<Match, bool> <>9__5;
			Func<Match, bool> predicate;
			if ((predicate = <>9__5) == null)
			{
				predicate = (<>9__5 = ((Match match) => match.Groups["EncounterId"].Value == encounterModelUniqueID));
			}
			using (IEnumerator<Match> enumerator = source.Where(predicate).GetEnumerator())
			{
				IL_164:
				while (enumerator.MoveNext())
				{
					Match match2 = enumerator.Current;
					int item;
					if (int.TryParse(match2.Groups["pathStartNode"].Value, out item))
					{
						list.Add(item);
						using (IEnumerator enumerator2 = match2.Groups["pathNodes"].Captures.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (!int.TryParse(((Capture)enumerator2.Current).Value, out item))
								{
									goto IL_164;
								}
								list.Add(item);
							}
							break;
						}
					}
				}
			}
			ChatEncounterExt.CurPaths[encounterModelUniqueID] = list;
			ModEncounterNode value = list.Aggregate(null, delegate(ModEncounterNode current, int i)
			{
				if (!(current == null))
				{
					return current.ChildrenEncounterNodes[i];
				}
				return modEncounter.ModEncounterNodes[i];
			});
			ChatEncounterExt.CurPathNode[encounterModelUniqueID] = value;
			ModEncounterNode[] curNodes = list.Aggregate(modEncounter.ModEncounterNodes, (ModEncounterNode[] current, int i) => current[i].ChildrenEncounterNodes);
			ChatEncounterExt.CurPathChildrenNodes[encounterModelUniqueID] = curNodes;
			__instance.CheckButtonCountAndEnable(curNodes.Length, delegate(EncounterPopup _, EncounterOptionButton button, int index)
			{
				button.Setup(index, curNodes[index].Title, null, false);
				button.Interactable = (curNodes[index].Condition.ConditionsValid(false, MBSingleton<GameManager>.Instance.CurrentEnvironmentCard) || curNodes[index].Condition.ConditionsValid(false, MBSingleton<GameManager>.Instance.CurrentWeatherCard));
				button.gameObject.SetActive(curNodes[index].ShowCondition.ConditionsValid(false, MBSingleton<GameManager>.Instance.CurrentEnvironmentCard) || curNodes[index].ShowCondition.ConditionsValid(false, MBSingleton<GameManager>.Instance.CurrentWeatherCard));
			});
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003140 File Offset: 0x00001340
		public override void DoModPlayerAction(EncounterPopup __instance, int _Action)
		{
			InGameCardBase currentEnvironmentCard = MBSingleton<GameManager>.Instance.CurrentEnvironmentCard;
			string uniqueID = __instance.CurrentEncounter.EncounterModel.UniqueID;
			ModEncounter modEncounter = ModEncounter.ModEncounters[uniqueID];
			string key2 = currentEnvironmentCard.DroppedCollections.Keys.FirstOrDefault((string key) => ChatEncounterExt.ActionRecordRegex.IsMatch(key));
			__instance.AddLogSeparator(0f);
			List<int> list = ChatEncounterExt.CurPaths[uniqueID];
			list.Add(_Action);
			ChatEncounterExt.CurPathNode[uniqueID] = ChatEncounterExt.CurPathChildrenNodes[uniqueID][_Action];
			ChatEncounterExt.CurPathChildrenNodes[uniqueID] = ChatEncounterExt.CurPathNode[uniqueID].ChildrenEncounterNodes;
			ModEncounterNode modEncounterNode = ChatEncounterExt.CurPathNode[uniqueID];
			if (modEncounterNode.BackNode && list.Count >= 2)
			{
				list.RemoveRange(list.Count - 2, 2);
				ModEncounterNode modEncounterNode2 = list.Aggregate(null, delegate(ModEncounterNode current, int i)
				{
					if (!(current == null))
					{
						return current.ChildrenEncounterNodes[i];
					}
					return modEncounter.ModEncounterNodes[i];
				});
				ChatEncounterExt.CurPathNode[uniqueID] = modEncounterNode2;
				ChatEncounterExt.CurPathChildrenNodes[uniqueID] = (modEncounterNode2 ? modEncounterNode2.ChildrenEncounterNodes : null);
			}
			if (modEncounterNode.BackNode || !modEncounterNode.EndNode)
			{
				EncounterLogMessage message = new EncounterLogMessage(modEncounterNode.PlayerText, ((double)Mathf.Abs(modEncounterNode.PlayerTextDuration) < 0.01) ? 1f : modEncounterNode.PlayerTextDuration)
				{
					SoundEffects = new AudioClip[]
					{
						modEncounterNode.PlayerAudio ? modEncounterNode.PlayerAudio : ModEncounter.ModEncounters[uniqueID].DefaultPlayerAudio
					}
				};
				__instance.AddToLog(message);
				__instance.AddLogSeparator(0f);
				message = new EncounterLogMessage(modEncounterNode.EnemyText, ((double)Mathf.Abs(modEncounterNode.EnemyTextDuration) < 0.01) ? 1f : modEncounterNode.EnemyTextDuration)
				{
					SoundEffects = new AudioClip[]
					{
						modEncounterNode.EnemyAudio ? modEncounterNode.EnemyAudio : ModEncounter.ModEncounters[uniqueID].DefaultEnemyAudio
					}
				};
				__instance.AddToLog(message);
				if (modEncounterNode.HasNodeEffect)
				{
					__instance.StartCoroutine(__instance.WaitForAction(modEncounterNode.NodeEffect, MBSingleton<GameManager>.Instance.CurrentEnvironmentCard));
				}
				currentEnvironmentCard.DroppedCollections.SafeRemove(key2);
				currentEnvironmentCard.DroppedCollections[__instance.CurrentEncounter.EncounterModel.SavePath<ModEncounter>()] = Vector2Int.one;
				__instance.CurrentEncounter.CurrentRound++;
				__instance.DisplayPlayerActions();
				return;
			}
			__instance.CurrentEncounter.EncounterResult = EncounterResult.PlayerDemoralized;
			currentEnvironmentCard.DroppedCollections.SafeRemove(key2);
			if (modEncounterNode.HasNodeEffect)
			{
				__instance.ContinueButton.interactable = false;
				MBSingleton<GameManager>.Instance.StartCoroutine(ChatEncounterExt.WaitCloseWindow(__instance).OnEnd(MBSingleton<GameManager>.Instance.ActionRoutine(modEncounterNode.NodeEffect, MBSingleton<GameManager>.Instance.CurrentEnvironmentCard, false, false)));
			}
			else
			{
				__instance.ContinueButton.interactable = false;
				MBSingleton<GameManager>.Instance.StartCoroutine(ChatEncounterExt.WaitCloseWindow(__instance));
			}
			if (modEncounterNode.NodeEffect.ProducedCards.Any(new Func<CardsDropCollection, bool>(ChatEncounterExt.DropEncounterOrEvent)) || modEncounterNode.DontShowEnd)
			{
				return;
			}
			MBSingleton<GraphicsManager>.Instance.CardsDestroyed.Setup(modEncounterNode.PlayerText + "\n\n" + modEncounterNode.EnemyText, modEncounterNode.Title);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000034DC File Offset: 0x000016DC
		public override void ModRoundStart(EncounterPopup __instance, bool _Loaded)
		{
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
			__instance.DisplayPlayerActions();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000352E File Offset: 0x0000172E
		public override void UpdateModEx(EncounterPopup __instance)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003535 File Offset: 0x00001735
		public static IEnumerator WaitCloseWindow(EncounterPopup popup)
		{
			while (popup.ActionPlaying || popup.LogIsUpdating)
			{
				yield return null;
			}
			popup.OngoingEncounter = false;
			popup.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003544 File Offset: 0x00001744
		public static bool DropEncounterOrEvent(CardsDropCollection collection)
		{
			if (!collection.DroppedEncounter)
			{
				return collection.DroppedCards.Any((CardDrop drop) => drop.DroppedCard && drop.DroppedCard.CardType == CardTypes.Event);
			}
			return true;
		}

		// Token: 0x04000032 RID: 50
		private static readonly Regex ActionRecordRegex = new Regex("__\\{(?<EncounterId>.+?)\\}ModEncounter\\.Infos__\\{EncounterPath:(?<path>(?<pathStartNode>\\d+)(\\.(?<pathNodes>\\d+))*)\\}");

		// Token: 0x04000033 RID: 51
		public static readonly Dictionary<string, List<int>> CurPaths = new Dictionary<string, List<int>>();

		// Token: 0x04000034 RID: 52
		public static readonly Dictionary<string, ModEncounterNode[]> CurPathChildrenNodes = new Dictionary<string, ModEncounterNode[]>();

		// Token: 0x04000035 RID: 53
		public static readonly Dictionary<string, ModEncounterNode> CurPathNode = new Dictionary<string, ModEncounterNode>();
	}
}
