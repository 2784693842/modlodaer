using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChatTreeLoader.Patchers
{
	// Token: 0x02000011 RID: 17
	public static class TestCardAdd
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002A88 File Offset: 0x00000C88
		public static void AddTestCard()
		{
			GameDataBase dataBase = GameLoad.Instance.DataBase;
			Encounter encounter = ScriptableObject.CreateInstance<Encounter>();
			encounter.UniqueID = "test______encounter__onlyTest__dontUseIt__{.}encounter1";
			encounter.name = "aaae1";
			dataBase.AllData.Add(encounter);
			encounter.EncounterTitle = new LocalizedString
			{
				DefaultText = "测试用"
			};
			encounter.EncounterStartingLog = new EncounterLogMessage("仅用于测试");
			CardData cardData = UnityEngine.Object.Instantiate<UniqueIDScriptable>(dataBase.AllData.First((UniqueIDScriptable scriptable) => scriptable.UniqueID == "a7384e5147b23a642809451cc4ef24fb")) as CardData;
			cardData.UniqueID = "test______encounter__onlyTest__dontUseIt__{.}card1";
			cardData.name = "aaa1";
			cardData.CardTags = new CardTag[0];
			cardData.CardInteractions = new CardOnCardAction[0];
			cardData.OnStatsChangeActions = new FromStatChangeAction[0];
			dataBase.AllData.Add(cardData);
			DurabilityStat durabilityStat = new DurabilityStat(false, 0);
			cardData.Progress = durabilityStat;
			cardData.FuelCapacity = durabilityStat;
			cardData.SpoilageTime = durabilityStat;
			cardData.UsageDurability = durabilityStat;
			cardData.SpecialDurability1 = durabilityStat;
			cardData.SpecialDurability2 = durabilityStat;
			cardData.SpecialDurability3 = durabilityStat;
			cardData.SpecialDurability4 = durabilityStat;
			cardData.CardName = new LocalizedString
			{
				DefaultText = "测试卡"
			};
			cardData.CardDescription = new LocalizedString
			{
				DefaultText = "仅测试用"
			};
			cardData.DismantleActions = new List<DismantleCardAction>
			{
				new DismantleCardAction
				{
					ActionName = new LocalizedString
					{
						DefaultText = "测试",
						LocalizationKey = ""
					},
					ProducedCards = new CardsDropCollection[]
					{
						new CardsDropCollection
						{
							DroppedEncounter = encounter
						}
					}
				}
			};
			ModEncounter modEncounter = ScriptableObject.CreateInstance<ModEncounter>();
			modEncounter.name = "mode1";
			modEncounter.ThisId = encounter.UniqueID;
			modEncounter.ModEncounterNodes = new ModEncounterNode[]
			{
				new ModEncounterNode
				{
					EndNode = true,
					Title = new LocalizedString
					{
						DefaultText = "test1"
					},
					PlayerText = new LocalizedString
					{
						LocalizationKey = "player1"
					},
					EnemyText = new LocalizedString
					{
						DefaultText = "enemy1"
					},
					HasNodeEffect = true,
					NodeEffect = new CardAction
					{
						ProducedCards = new CardsDropCollection[]
						{
							new CardsDropCollection
							{
								DroppedCards = new CardDrop[]
								{
									new CardDrop
									{
										DroppedCard = cardData,
										Quantity = Vector2Int.one
									}
								}
							}
						},
						UseMiniTicks = MiniTicksBehavior.CostsAMiniTick
					}
				},
				new ModEncounterNode
				{
					EndNode = true,
					Title = new LocalizedString
					{
						DefaultText = "test2"
					},
					PlayerText = new LocalizedString
					{
						LocalizationKey = "player2"
					},
					EnemyText = new LocalizedString
					{
						DefaultText = "enemy1"
					},
					HasNodeEffect = true,
					NodeEffect = new CardAction
					{
						DaytimeCost = 1
					}
				}
			};
		}
	}
}
