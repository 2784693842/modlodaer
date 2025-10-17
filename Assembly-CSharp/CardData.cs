using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000CB RID: 203
[CreateAssetMenu(menuName = "Survival/Card")]
public class CardData : UniqueIDScriptable
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x0600075F RID: 1887 RVA: 0x00049024 File Offset: 0x00047224
	public bool HasPassiveEffects
	{
		get
		{
			bool flag = false;
			bool flag2 = false;
			if (this.PassiveStatEffects != null && this.PassiveStatEffects.Length != 0)
			{
				flag = true;
			}
			if (this.PassiveEffects != null && this.PassiveEffects.Length != 0)
			{
				flag2 = true;
			}
			return flag || flag2;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000760 RID: 1888 RVA: 0x0004905E File Offset: 0x0004725E
	public bool HasRemoteEffects
	{
		get
		{
			return this.RemotePassiveEffects != null && this.RemotePassiveEffects.Length != 0;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000761 RID: 1889 RVA: 0x00049074 File Offset: 0x00047274
	public bool HasOnCreatedSound
	{
		get
		{
			if (this.WhenCreatedSounds == null)
			{
				return false;
			}
			if (this.WhenCreatedSounds.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.WhenCreatedSounds.Length; i++)
			{
				if (this.WhenCreatedSounds[i])
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000762 RID: 1890 RVA: 0x000490BB File Offset: 0x000472BB
	public bool HasOnDestroyDrops
	{
		get
		{
			return this.DroppedOnDestroy != null && this.DroppedOnDestroy.Length != 0;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000763 RID: 1891 RVA: 0x000490D3 File Offset: 0x000472D3
	public bool CanContainLiquid
	{
		get
		{
			return this.MaxLiquidCapacity > 0f && this.CardType != CardTypes.Liquid;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000764 RID: 1892 RVA: 0x000490F1 File Offset: 0x000472F1
	public bool NeedsAnyAmmo
	{
		get
		{
			return this.AmmoNeeded != null && this.AmmoNeeded.Length != 0;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000765 RID: 1893 RVA: 0x00049108 File Offset: 0x00047308
	public CardUnlockConditions GetUnlockConditions
	{
		get
		{
			if (this.CardsOnBoard != null && this.CardsOnBoard.Count > 0)
			{
				return new CardUnlockConditions(this, false);
			}
			if (this.TagsOnBoard != null && this.TagsOnBoard.Count > 0)
			{
				return new CardUnlockConditions(this, false);
			}
			if (this.StatValues != null && this.StatValues.Count > 0)
			{
				return new CardUnlockConditions(this, false);
			}
			if (this.TimeValues != null && this.TimeValues.Count > 0)
			{
				return new CardUnlockConditions(this, false);
			}
			if (this.CompletedObjectives != null && this.CompletedObjectives.Count > 0)
			{
				return new CardUnlockConditions(this, false);
			}
			return null;
		}
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x000491AC File Offset: 0x000473AC
	public Sprite GetImageForLiquid(CardData _Liquid)
	{
		if (_Liquid == null)
		{
			return null;
		}
		if (this.LiquidImages != null)
		{
			for (int i = 0; i < this.LiquidImages.Length; i++)
			{
				if (this.LiquidImages[i] != null && this.LiquidImages[i].UseSprite(_Liquid))
				{
					return this.LiquidImages[i].LiquidImage;
				}
			}
		}
		return this.DefaultLiquidImage;
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00049210 File Offset: 0x00047410
	public bool HasTag(CardTag _Tag)
	{
		if (!_Tag)
		{
			return false;
		}
		if (this.CardTags == null)
		{
			return false;
		}
		if (this.CardTags.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.CardTags.Length; i++)
		{
			if (this.CardTags[i] == _Tag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x00049264 File Offset: 0x00047464
	public bool HasAnyTag(CardTag[] _Tags)
	{
		if (_Tags == null)
		{
			return false;
		}
		if (_Tags.Length == 0)
		{
			return false;
		}
		if (this.CardTags == null)
		{
			return false;
		}
		if (this.CardTags.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < _Tags.Length; i++)
		{
			if (this.HasTag(_Tags[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000769 RID: 1897 RVA: 0x000492B0 File Offset: 0x000474B0
	public bool IsEquipment
	{
		get
		{
			if (this.EquipmentTags == null)
			{
				return false;
			}
			if (this.EquipmentTags.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.EquipmentTags.Length; i++)
			{
				if (this.EquipmentTags[i])
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x0600076A RID: 1898 RVA: 0x000492F8 File Offset: 0x000474F8
	public bool IsMandatoryEquipment
	{
		get
		{
			if (this.EquipmentTags == null)
			{
				return false;
			}
			if (this.EquipmentTags.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.EquipmentTags.Length; i++)
			{
				if (this.EquipmentTags[i] && this.EquipmentTags[i].MandatoryEquip)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600076B RID: 1899 RVA: 0x00049350 File Offset: 0x00047550
	public bool IsImportantEquipment
	{
		get
		{
			if (this.EquipmentTags == null)
			{
				return false;
			}
			if (this.EquipmentTags.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.EquipmentTags.Length; i++)
			{
				if (this.EquipmentTags[i].NotifyWhenNew)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00049398 File Offset: 0x00047598
	public bool HasEquipmentTag(EquipmentTag _Tag)
	{
		if (!_Tag)
		{
			return false;
		}
		if (this.EquipmentTags == null)
		{
			return false;
		}
		if (this.EquipmentTags.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.EquipmentTags.Length; i++)
		{
			if (this.EquipmentTags[i] == _Tag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x000493EC File Offset: 0x000475EC
	public bool HasAnyEquipmentTag(EquipmentTag[] _Tags)
	{
		if (_Tags == null)
		{
			return false;
		}
		if (_Tags.Length == 0)
		{
			return false;
		}
		if (this.EquipmentTags == null)
		{
			return false;
		}
		if (this.EquipmentTags.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < _Tags.Length; i++)
		{
			if (this.HasEquipmentTag(_Tags[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x0600076E RID: 1902 RVA: 0x00049438 File Offset: 0x00047638
	public bool IsTravellingCard
	{
		get
		{
			if (this.IsTravelCard)
			{
				return true;
			}
			if (this.CardInteractions != null && this.CardInteractions.Length != 0)
			{
				for (int i = 0; i < this.CardInteractions.Length; i++)
				{
					if (this.CardInteractions[i].IsTravellingAction)
					{
						return true;
					}
				}
			}
			if (this.DismantleActions != null && this.DismantleActions.Count > 0)
			{
				for (int j = 0; j < this.DismantleActions.Count; j++)
				{
					if (this.DismantleActions[j].IsTravellingAction)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x000494C8 File Offset: 0x000476C8
	public bool HasImprovement(CardData _Improvement)
	{
		if (!_Improvement)
		{
			return false;
		}
		if (this.EnvironmentImprovements == null)
		{
			return false;
		}
		if (this.EnvironmentImprovements.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.EnvironmentImprovements.Length; i++)
		{
			if (this.EnvironmentImprovements[i] != null && this.EnvironmentImprovements[i].Card == _Improvement)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0004952C File Offset: 0x0004772C
	public bool HasDamage(CardData _Damage)
	{
		if (!_Damage)
		{
			return false;
		}
		if (this.EnvironmentDamages == null)
		{
			return false;
		}
		if (this.EnvironmentDamages.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.EnvironmentDamages.Length; i++)
		{
			if (this.EnvironmentDamages[i] != null && this.EnvironmentDamages[i].Card == _Damage)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000771 RID: 1905 RVA: 0x00049590 File Offset: 0x00047790
	public bool HasInventory
	{
		get
		{
			if (this.CardType == CardTypes.Explorable || this.CardType == CardTypes.Blueprint || this.CardType == CardTypes.EnvImprovement || this.CardType == CardTypes.EnvDamage)
			{
				return true;
			}
			if (this.MaxWeightCapacity <= 0f)
			{
				return this.InventorySlots != null && this.InventorySlots.Length != 0;
			}
			return this.MaxWeightCapacity > 0f;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000772 RID: 1906 RVA: 0x000495F4 File Offset: 0x000477F4
	public float BlueprintResultWeight
	{
		get
		{
			if (this.CardType != CardTypes.Blueprint)
			{
				return 0f;
			}
			if (this.BlueprintResult == null)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.BlueprintResult.Length; i++)
			{
				if (this.BlueprintResult[i].DroppedCard)
				{
					num += this.BlueprintResult[i].DroppedCard.ObjectWeight * (float)Mathf.Max(this.BlueprintResult[i].Quantity.x, this.BlueprintResult[i].Quantity.y);
				}
			}
			return num;
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0004969C File Offset: 0x0004789C
	public bool CanContainThisLiquid(CardData _Liquid)
	{
		if (!this.CanContainLiquid)
		{
			return false;
		}
		if (this.ExclusivelyAcceptedLiquids != null && this.ExclusivelyAcceptedLiquids.Length != 0)
		{
			for (int i = 0; i < this.ExclusivelyAcceptedLiquids.Length; i++)
			{
				if (this.ExclusivelyAcceptedLiquids[i].CheckCard(_Liquid))
				{
					return true;
				}
			}
			return false;
		}
		if (this.NOTAcceptedLiquids != null && this.NOTAcceptedLiquids.Length != 0)
		{
			for (int j = 0; j < this.NOTAcceptedLiquids.Length; j++)
			{
				if (this.NOTAcceptedLiquids[j].CheckCard(_Liquid))
				{
					return false;
				}
			}
		}
		if (_Liquid && _Liquid.LiquidValidContainers != null && _Liquid.LiquidValidContainers.Length != 0)
		{
			for (int k = 0; k < _Liquid.LiquidValidContainers.Length; k++)
			{
				if (_Liquid.LiquidValidContainers[k].CheckCard(this))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0004976E File Offset: 0x0004796E
	public float GetWeightCapacity(float _BonusWeight)
	{
		if (this.MaxWeightCapacity <= 0f)
		{
			return 0f;
		}
		return Mathf.Max(this.MaxWeightCapacity + _BonusWeight, 0.1f);
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000775 RID: 1909 RVA: 0x00049798 File Offset: 0x00047998
	public bool LegacyInventory
	{
		get
		{
			return this.CardType == CardTypes.Blueprint || this.CardType == CardTypes.EnvImprovement || this.CardType == CardTypes.EnvDamage || this.CardType == CardTypes.Explorable || this.CanCook || this.MaxWeightCapacity <= 0f;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000776 RID: 1910 RVA: 0x000497E8 File Offset: 0x000479E8
	public CardData GetTravelDestination
	{
		get
		{
			if (this.CardInteractions != null && this.CardInteractions.Length != 0)
			{
				for (int i = 0; i < this.CardInteractions.Length; i++)
				{
					CardData travelDestination = this.CardInteractions[i].TravelDestination;
					if (travelDestination)
					{
						return travelDestination;
					}
				}
			}
			if (this.DismantleActions != null && this.DismantleActions.Count > 0)
			{
				for (int j = 0; j < this.DismantleActions.Count; j++)
				{
					CardData travelDestination2 = this.DismantleActions[j].TravelDestination;
					if (travelDestination2)
					{
						return travelDestination2;
					}
				}
			}
			return null;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000777 RID: 1911 RVA: 0x00049880 File Offset: 0x00047A80
	public bool IsAutoSolvableEvent
	{
		get
		{
			return this.CardType == CardTypes.Event && this.DismantleActions != null && (this.CardInteractions == null || this.CardInteractions.Length == 0) && (this.OnStatsChangeActions == null || this.OnStatsChangeActions.Length == 0) && this.DismantleActions.Count == 1;
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000778 RID: 1912 RVA: 0x000498D8 File Offset: 0x00047AD8
	public int ExplorationActionIndex
	{
		get
		{
			if (this.CardType != CardTypes.Explorable)
			{
				return -1;
			}
			if (this.DismantleActions == null)
			{
				return -1;
			}
			if (this.DismantleActions.Count == 0)
			{
				return -1;
			}
			for (int i = 0; i < this.DismantleActions.Count; i++)
			{
				if (this.DismantleActions[i] != null && this.DismantleActions[i].ExplorationValue > 0f)
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000779 RID: 1913 RVA: 0x00049948 File Offset: 0x00047B48
	public bool HasNormalDismantleActions
	{
		get
		{
			if (this.CardType != CardTypes.Explorable)
			{
				return false;
			}
			if (this.DismantleActions == null)
			{
				return false;
			}
			if (this.DismantleActions.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.DismantleActions.Count; i++)
			{
				if (this.DismantleActions[i] != null && this.DismantleActions[i].ExplorationValue <= 0f)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x0600077A RID: 1914 RVA: 0x000499B8 File Offset: 0x00047BB8
	public bool HasImprovements
	{
		get
		{
			return this.CardType == CardTypes.Explorable && this.EnvironmentImprovements != null && this.EnvironmentImprovements.Length != 0;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x0600077B RID: 1915 RVA: 0x000499D9 File Offset: 0x00047BD9
	public bool HasDamages
	{
		get
		{
			return this.CardType == CardTypes.Explorable && this.EnvironmentDamages != null && this.EnvironmentDamages.Length != 0;
		}
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x000499FC File Offset: 0x00047BFC
	public string EnvironmentDictionaryKey(CardData _FromEnvironment, int _ID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(this.UniqueID);
		if (this.InstancedEnvironment && _FromEnvironment)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(_FromEnvironment.UniqueID);
			if (_ID > 0)
			{
				stringBuilder.Append("=");
				stringBuilder.Append(_ID.ToString());
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00049A6C File Offset: 0x00047C6C
	public bool CanSpawnOnBoard()
	{
		if (this.CardType == CardTypes.Event && MBSingleton<GameManager>.Instance.ExplorationDroppedEvents.Contains(this))
		{
			return false;
		}
		if (this.IsMandatoryEquipment && MBSingleton<GraphicsManager>.Instance && MBSingleton<GraphicsManager>.Instance.CharacterWindow && !MBSingleton<GraphicsManager>.Instance.CharacterWindow.CanEquip(this, null))
		{
			return false;
		}
		if (this.UniqueOnBoard && MBSingleton<GameManager>.Instance.CardIsOnBoard(this, false, true, false, false, null, Array.Empty<InGameCardBase>()))
		{
			return false;
		}
		if (this.CardTags != null && this.CardTags.Length != 0)
		{
			for (int i = 0; i < this.CardTags.Length; i++)
			{
				if (this.CardTags[i] && this.CardTags[i].UniqueOnBoard && MBSingleton<GameManager>.Instance.TagIsOnBoard(this.CardTags[i], false, true, false, false, null))
				{
					return false;
				}
			}
		}
		if (this.SpawningBlockedBy != null && this.SpawningBlockedBy.Length != 0)
		{
			for (int j = 0; j < this.SpawningBlockedBy.Length; j++)
			{
				if (this.SpawningBlockedBy[j] && MBSingleton<GameManager>.Instance.CardIsOnBoard(this.SpawningBlockedBy[j], true, true, false, false, null, Array.Empty<InGameCardBase>()))
				{
					return false;
				}
			}
		}
		return (!this.CannotSpawnDuringTickCatchup || !MBSingleton<GameManager>.Instance.IsCatchingUp) && MBSingleton<GameManager>.Instance.CheckExclusiveGroups(this);
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x00049BC4 File Offset: 0x00047DC4
	public bool CanPile
	{
		get
		{
			return (this.InventorySlots == null || this.InventorySlots.Length == 0) && this.CardType != CardTypes.Blueprint && this.CardType != CardTypes.EnvImprovement && this.CardType != CardTypes.EnvDamage && !this.IsTravellingCard && !this.DoesNotPile;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x0600077F RID: 1919 RVA: 0x00049C16 File Offset: 0x00047E16
	public bool CanCook
	{
		get
		{
			return this.CookingRecipes != null && this.CookingRecipes.Length != 0;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000780 RID: 1920 RVA: 0x00049C2C File Offset: 0x00047E2C
	public bool CanBookmark
	{
		get
		{
			return this.CardType == CardTypes.Item || this.CardType == CardTypes.Location || this.CardType == CardTypes.Base || this.CardType == CardTypes.Blueprint;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000781 RID: 1921 RVA: 0x00049C54 File Offset: 0x00047E54
	public bool IndependentFromEnv
	{
		get
		{
			return this.AlwaysUpdate || (this.CardType != CardTypes.Base && this.CardType != CardTypes.Item && this.CardType != CardTypes.Location && this.CardType != CardTypes.Explorable && this.CardType != CardTypes.Blueprint && this.CardType != CardTypes.Liquid && this.CardType != CardTypes.EnvImprovement && this.CardType != CardTypes.EnvDamage);
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000782 RID: 1922 RVA: 0x00049CBC File Offset: 0x00047EBC
	public CardAction DefaultDiscardAction
	{
		get
		{
			return new CardAction
			{
				ActionName = LocalizedString.DiscardTitle,
				ActionDescription = LocalizedString.DiscardDesc,
				ReceivingCardChanges = new CardStateChange
				{
					ModType = CardModifications.Destroy,
					DropOnDestroyList = this.DropFromTrashButton
				}
			};
		}
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00049D08 File Offset: 0x00047F08
	public ExplorationResult GetExplorationResult(string _Name)
	{
		for (int i = 0; i < this.ExplorationResults.Length; i++)
		{
			if (this.ExplorationResults[i].IsIdentical(_Name))
			{
				return this.ExplorationResults[i];
			}
		}
		return null;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00049D44 File Offset: 0x00047F44
	private void FillDropsList()
	{
		this.AllDrops = new List<CardsDropCollection>();
		if (this.SpoilageTime)
		{
			if (this.SpoilageTime.HasActionOnZero)
			{
				this.AddDropsFromAction(this.SpoilageTime.OnZero);
			}
			if (this.SpoilageTime.HasActionOnFull)
			{
				this.AddDropsFromAction(this.SpoilageTime.OnFull);
			}
		}
		if (this.UsageDurability)
		{
			if (this.UsageDurability.HasActionOnZero)
			{
				this.AddDropsFromAction(this.UsageDurability.OnZero);
			}
			if (this.UsageDurability.HasActionOnFull)
			{
				this.AddDropsFromAction(this.UsageDurability.OnFull);
			}
		}
		if (this.FuelCapacity)
		{
			if (this.FuelCapacity.HasActionOnZero)
			{
				this.AddDropsFromAction(this.FuelCapacity.OnZero);
			}
			if (this.FuelCapacity.HasActionOnFull)
			{
				this.AddDropsFromAction(this.FuelCapacity.OnFull);
			}
		}
		if (this.Progress)
		{
			if (this.Progress.HasActionOnZero)
			{
				this.AddDropsFromAction(this.Progress.OnZero);
			}
			if (this.Progress.HasActionOnFull)
			{
				this.AddDropsFromAction(this.Progress.OnFull);
			}
		}
		if (this.SpecialDurability1)
		{
			if (this.SpecialDurability1.HasActionOnZero)
			{
				this.AddDropsFromAction(this.SpecialDurability1.OnZero);
			}
			if (this.SpecialDurability1.HasActionOnFull)
			{
				this.AddDropsFromAction(this.SpecialDurability1.OnFull);
			}
		}
		if (this.SpecialDurability2)
		{
			if (this.SpecialDurability2.HasActionOnZero)
			{
				this.AddDropsFromAction(this.SpecialDurability2.OnZero);
			}
			if (this.SpecialDurability2.HasActionOnFull)
			{
				this.AddDropsFromAction(this.SpecialDurability2.OnFull);
			}
		}
		if (this.SpecialDurability3)
		{
			if (this.SpecialDurability3.HasActionOnZero)
			{
				this.AddDropsFromAction(this.SpecialDurability3.OnZero);
			}
			if (this.SpecialDurability3.HasActionOnFull)
			{
				this.AddDropsFromAction(this.SpecialDurability3.OnFull);
			}
		}
		if (this.SpecialDurability4)
		{
			if (this.SpecialDurability4.HasActionOnZero)
			{
				this.AddDropsFromAction(this.SpecialDurability4.OnZero);
			}
			if (this.SpecialDurability4.HasActionOnFull)
			{
				this.AddDropsFromAction(this.SpecialDurability4.OnFull);
			}
		}
		for (int i = 0; i < this.DismantleActions.Count; i++)
		{
			this.AddDropsFromAction(this.DismantleActions[i]);
		}
		for (int j = 0; j < this.OnStatsChangeActions.Length; j++)
		{
			this.AddDropsFromAction(this.OnStatsChangeActions[j]);
		}
		for (int k = 0; k < this.CardInteractions.Length; k++)
		{
			this.AddDropsFromAction(this.CardInteractions[k]);
		}
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x0004A010 File Offset: 0x00048210
	private void AddDropsFromAction(CardAction _Action)
	{
		if (_Action.ProducedCards == null)
		{
			return;
		}
		if (_Action.ProducedCards.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _Action.ProducedCards.Length; i++)
		{
			if (!this.AllDrops.Contains(_Action.ProducedCards[i]))
			{
				this.AllDrops.Add(_Action.ProducedCards[i]);
			}
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000786 RID: 1926 RVA: 0x0004A06C File Offset: 0x0004826C
	public string GetInventorySlotsText
	{
		get
		{
			if (!string.IsNullOrEmpty(this.InventorySlotsText))
			{
				return this.InventorySlotsText;
			}
			if (!this.CanCook)
			{
				return LocalizedString.DefaultInventorySlot;
			}
			bool flag = false;
			if (this.CookingRecipes[0].CompatibleCards == null)
			{
				flag = true;
			}
			else if (this.CookingRecipes[0].CompatibleCards.Length == 0)
			{
				flag = true;
			}
			else if (this.CookingRecipes[0].CompatibleCards.Length == 1)
			{
				flag = (this.CookingRecipes[0].CompatibleCards[0] == null);
			}
			bool flag2 = false;
			if (this.CookingRecipes[0].CompatibleTags == null)
			{
				flag2 = true;
			}
			else if (this.CookingRecipes[0].CompatibleTags.Length == 0)
			{
				flag2 = true;
			}
			else if (this.CookingRecipes[0].CompatibleTags.Length == 1)
			{
				flag2 = (this.CookingRecipes[0].CompatibleTags[0] == null);
			}
			if (flag && flag2)
			{
				return LocalizedString.DefaultEmptyRecipeSlot;
			}
			return LocalizedString.DefaultCookingSlot;
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000787 RID: 1927 RVA: 0x0004A16C File Offset: 0x0004836C
	public CardFilter CompleteInventoryFilter
	{
		get
		{
			if (this.CachedFilter.IsEmpty)
			{
				this.CachedFilter = this.InventoryFilter;
				List<CardTypes> list = new List<CardTypes>();
				if (!this.CachedFilter.TypesContains(CardTypes.Blueprint))
				{
					list.Add(CardTypes.Blueprint);
				}
				if (!this.CachedFilter.TypesContains(CardTypes.EnvImprovement))
				{
					list.Add(CardTypes.EnvImprovement);
				}
				if (!this.CachedFilter.TypesContains(CardTypes.EnvDamage))
				{
					list.Add(CardTypes.EnvDamage);
				}
				if (!this.CachedFilter.TypesContains(CardTypes.Environment))
				{
					list.Add(CardTypes.Environment);
				}
				if (!this.CachedFilter.TypesContains(CardTypes.Event))
				{
					list.Add(CardTypes.Event);
				}
				if (!this.CachedFilter.TypesContains(CardTypes.Explorable))
				{
					list.Add(CardTypes.Explorable);
				}
				if (!this.CachedFilter.TypesContains(CardTypes.Hand))
				{
					list.Add(CardTypes.Hand);
				}
				if (!this.CachedFilter.TypesContains(CardTypes.Location))
				{
					list.Add(CardTypes.Location);
				}
				if (!this.CachedFilter.TypesContains(CardTypes.Weather))
				{
					list.Add(CardTypes.Weather);
				}
				this.CachedFilter.AddFilters(new CardFilter
				{
					NOTAcceptedTypes = list.ToArray()
				});
				this.CachedFilter.AddFilters(this.CookingFilter);
			}
			return this.CachedFilter;
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000788 RID: 1928 RVA: 0x0004A294 File Offset: 0x00048494
	public CardFilter CookingFilter
	{
		get
		{
			CardFilter result = default(CardFilter);
			if (this.CookingRecipes == null)
			{
				return result;
			}
			if (this.CookingRecipes.Length == 0)
			{
				return result;
			}
			List<CardData> list = new List<CardData>();
			List<CardTag> list2 = new List<CardTag>();
			for (int i = 0; i < this.CookingRecipes.Length; i++)
			{
				if (this.CookingRecipes[i].CompatibleCards != null && this.CookingRecipes[i].CompatibleCards.Length != 0)
				{
					for (int j = 0; j < this.CookingRecipes[i].CompatibleCards.Length; j++)
					{
						if (!list.Contains(this.CookingRecipes[i].CompatibleCards[j]) && this.CookingRecipes[i].CompatibleCards[j] != null)
						{
							list.Add(this.CookingRecipes[i].CompatibleCards[j]);
						}
					}
				}
				if (this.CookingRecipes[i].CompatibleTags != null && this.CookingRecipes[i].CompatibleTags.Length != 0)
				{
					for (int k = 0; k < this.CookingRecipes[i].CompatibleTags.Length; k++)
					{
						if (!list2.Contains(this.CookingRecipes[i].CompatibleTags[k]) && this.CookingRecipes[i].CompatibleTags[k] != null)
						{
							list2.Add(this.CookingRecipes[i].CompatibleTags[k]);
						}
					}
				}
				bool flag = false;
				if (this.CookingRecipes[i].DropsAsCollection != null && this.CookingRecipes[i].DropsAsCollection.Length != 0)
				{
					flag = true;
				}
				if (flag)
				{
					for (int l = 0; l < this.CookingRecipes[i].DropsAsCollection.Length; l++)
					{
						this.CookingRecipes[i].DropsAsCollection[l].GetAllPossibleCards(list);
					}
				}
				else if (this.CookingRecipes[i].Drops != null && this.CookingRecipes[i].Drops.Length != 0)
				{
					for (int m = 0; m < this.CookingRecipes[i].Drops.Length; m++)
					{
						if (!list.Contains(this.CookingRecipes[i].Drops[m].DroppedCard) && this.CookingRecipes[i].Drops[m].DroppedCard != null)
						{
							list.Add(this.CookingRecipes[i].Drops[m].DroppedCard);
						}
					}
				}
				if (this.CookingRecipes[i].IngredientChanges.ModType == CardModifications.Transform && this.CookingRecipes[i].IngredientChanges.TransformInto && !list.Contains(this.CookingRecipes[i].IngredientChanges.TransformInto))
				{
					list.Add(this.CookingRecipes[i].IngredientChanges.TransformInto);
				}
			}
			result.AcceptedCards = list.ToArray();
			result.AcceptedTags = list2.ToArray();
			return result;
		}
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0004A568 File Offset: 0x00048768
	public CookingRecipe GetRecipeForCard(CardData _Card, InGameCardBase _Ingredient, InGameCardBase _Cooker)
	{
		if (this.CookingRecipes == null)
		{
			return null;
		}
		if (this.CookingRecipes.Length == 0)
		{
			return null;
		}
		if (this.InventorySlots == null)
		{
			return null;
		}
		if (this.InventorySlots.Length == 0)
		{
			return null;
		}
		int num = -1;
		for (int i = 0; i < this.CookingRecipes.Length; i++)
		{
			if (_Card)
			{
				if (this.CookingRecipes[i].ValidForCard(_Card))
				{
					if (this.CookingRecipes[i].Conditions.ConditionsValid(false, (this.CookingRecipes[i].ConditionsCard == CookingConditionsCard.Cooker) ? _Cooker : _Ingredient))
					{
						return this.CookingRecipes[i];
					}
					if (num == -1)
					{
						num = i;
					}
				}
			}
			else
			{
				bool flag = false;
				if (this.CookingRecipes[i].CompatibleCards != null && this.CookingRecipes[i].CompatibleCards.Length != 0)
				{
					for (int j = 0; j < this.CookingRecipes[i].CompatibleCards.Length; j++)
					{
						if (this.CookingRecipes[i].CompatibleCards[j])
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						goto IL_1A2;
					}
				}
				if (this.CookingRecipes[i].CompatibleTags != null && this.CookingRecipes[i].CompatibleTags.Length != 0)
				{
					for (int k = 0; k < this.CookingRecipes[i].CompatibleTags.Length; k++)
					{
						if (this.CookingRecipes[i].CompatibleTags[k])
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						goto IL_1A2;
					}
				}
				for (int l = 0; l < this.InventorySlots.Length; l++)
				{
					if (this.InventorySlots[l] == null)
					{
						if (this.CookingRecipes[i].Conditions.ConditionsValid(false, (this.CookingRecipes[i].ConditionsCard == CookingConditionsCard.Cooker) ? _Cooker : _Ingredient))
						{
							return this.CookingRecipes[i];
						}
						if (num == -1)
						{
							num = i;
						}
					}
				}
			}
			IL_1A2:;
		}
		if (num != -1)
		{
			return this.CookingRecipes[num];
		}
		return null;
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x0600078A RID: 1930 RVA: 0x0004A738 File Offset: 0x00048938
	public DismantleCardAction BlueprintCreationAction
	{
		get
		{
			DismantleCardAction dismantleCardAction = new CardAction(LocalizedString.Build, new LocalizedString
			{
				DefaultText = this.BlueprintBuildingDesc
			}, 0, new List<CardData>
			{
				this
			}, null, this.BuildSounds).ToDismantleAction();
			dismantleCardAction.UseMiniTicks = MiniTicksBehavior.KeepsZeroCost;
			dismantleCardAction.RequiredStatValues = this.BlueprintStatConditions;
			dismantleCardAction.RequiredCardsOnBoard = this.BlueprintCardConditions;
			dismantleCardAction.RequiredTagsOnBoard = this.BlueprintTagConditions;
			return dismantleCardAction;
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x0600078B RID: 1931 RVA: 0x0004A7A9 File Offset: 0x000489A9
	private string BlueprintBuildingDesc
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(LocalizedString.StartBuilding);
			stringBuilder.Append(this.CardName);
			return stringBuilder.ToString();
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x0600078C RID: 1932 RVA: 0x0004A7D8 File Offset: 0x000489D8
	public static CardAction OnEvaporatedAction
	{
		get
		{
			if (CardData.OnEvaporatedActionInstance == null)
			{
				CardData.OnEvaporatedActionInstance = new CardAction();
			}
			CardData.OnEvaporatedActionInstance.UseMiniTicks = MiniTicksBehavior.KeepsZeroCost;
			CardData.OnEvaporatedActionInstance.ReceivingCardChanges.ModType = CardModifications.Destroy;
			return CardData.OnEvaporatedActionInstance;
		}
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0004A80B File Offset: 0x00048A0B
	public static DismantleCardAction EmptyLiquidAction(CardData _Card)
	{
		DismantleCardAction dismantleCardAction = new DismantleCardAction();
		dismantleCardAction.UseMiniTicks = MiniTicksBehavior.KeepsZeroCost;
		dismantleCardAction.ReceivingCardChanges.ModType = CardModifications.Destroy;
		dismantleCardAction.ActionName = LocalizedString.Empty;
		dismantleCardAction.StackCompatible = true;
		return dismantleCardAction;
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0004A838 File Offset: 0x00048A38
	public static TransferedDurabilities CalculateLiquidDurabilities(TransferedDurabilities _LiquidA, TransferedDurabilities _LiquidB)
	{
		TransferedDurabilities transferedDurabilities = new TransferedDurabilities();
		float num = _LiquidA.Liquid + _LiquidB.Liquid;
		transferedDurabilities.Spoilage.FloatValue = (_LiquidA.Spoilage * _LiquidA.Liquid + _LiquidB.Spoilage * _LiquidB.Liquid) / num;
		transferedDurabilities.Spoilage.FloatValue -= _LiquidA.Spoilage.FloatValue;
		transferedDurabilities.Usage.FloatValue = (_LiquidA.Usage * _LiquidA.Liquid + _LiquidB.Usage * _LiquidB.Liquid) / num;
		transferedDurabilities.Usage.FloatValue -= _LiquidA.Usage.FloatValue;
		transferedDurabilities.Fuel.FloatValue = (_LiquidA.Fuel * _LiquidA.Liquid + _LiquidB.Fuel * _LiquidB.Liquid) / num;
		transferedDurabilities.Fuel.FloatValue -= _LiquidA.Fuel.FloatValue;
		transferedDurabilities.ConsumableCharges.FloatValue = (_LiquidA.ConsumableCharges * _LiquidA.Liquid + _LiquidB.ConsumableCharges * _LiquidB.Liquid) / num;
		transferedDurabilities.ConsumableCharges.FloatValue -= _LiquidA.ConsumableCharges.FloatValue;
		transferedDurabilities.Special1.FloatValue = (_LiquidA.Special1 * _LiquidA.Liquid + _LiquidB.Special1 * _LiquidB.Liquid) / num;
		transferedDurabilities.Special1.FloatValue -= _LiquidA.Special1.FloatValue;
		transferedDurabilities.Special2.FloatValue = (_LiquidA.Special2 * _LiquidA.Liquid + _LiquidB.Special2 * _LiquidB.Liquid) / num;
		transferedDurabilities.Special2.FloatValue -= _LiquidA.Special2.FloatValue;
		transferedDurabilities.Special3.FloatValue = (_LiquidA.Special3 * _LiquidA.Liquid + _LiquidB.Special3 * _LiquidB.Liquid) / num;
		transferedDurabilities.Special3.FloatValue -= _LiquidA.Special3.FloatValue;
		transferedDurabilities.Special4.FloatValue = (_LiquidA.Special4 * _LiquidA.Liquid + _LiquidB.Special4 * _LiquidB.Liquid) / num;
		transferedDurabilities.Special4.FloatValue -= _LiquidA.Special4.FloatValue;
		transferedDurabilities.Liquid = _LiquidB.Liquid;
		return transferedDurabilities;
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0004AADC File Offset: 0x00048CDC
	public static CardOnCardAction GenerateLiquidTransferAction(InGameCardBase _ContainerA, InGameCardBase _ContainerB, out bool _ActionReversed)
	{
		_ActionReversed = false;
		CardOnCardAction cardOnCardAction = new CardOnCardAction();
		cardOnCardAction.UseMiniTicks = MiniTicksBehavior.KeepsZeroCost;
		cardOnCardAction.InstantStatModifications = true;
		cardOnCardAction.ReceivingCardChanges.ModType = CardModifications.DurabilityChanges;
		cardOnCardAction.ReceivingCardChanges.ModifyLiquid = true;
		cardOnCardAction.GivenCardChanges.ModType = CardModifications.None;
		cardOnCardAction.GivenCardChanges.ModifyLiquid = true;
		switch (LiquidTransferRules.TransferResult(_ContainerB.ContainedLiquidModel, _ContainerA.ContainedLiquidModel))
		{
		case LiquidTransferInteraction.TransferToGiven:
			_ActionReversed = true;
			break;
		case LiquidTransferInteraction.UseDefaultTransferRules:
			if (_ContainerA.ContainedLiquid)
			{
				if (_ContainerA.ContainedLiquid.CurrentLiquidQuantity >= _ContainerA.CurrentMaxLiquidQuantity)
				{
					_ActionReversed = true;
				}
			}
			else
			{
				_ActionReversed = false;
			}
			if (_ContainerB.ContainedLiquid)
			{
				if (_ContainerB.ContainedLiquid.CurrentLiquidQuantity >= _ContainerB.CurrentMaxLiquidQuantity)
				{
					_ActionReversed = false;
				}
			}
			else
			{
				_ActionReversed = true;
			}
			break;
		case LiquidTransferInteraction.DontTransfer:
			Debug.LogError("This transfer action should not happen, something went wrong");
			break;
		}
		InGameCardBase inGameCardBase = _ActionReversed ? _ContainerB : _ContainerA;
		InGameCardBase inGameCardBase2 = _ActionReversed ? _ContainerA : _ContainerB;
		TransferedDurabilities liquidA = inGameCardBase.ContainedLiquid ? inGameCardBase.ContainedLiquid.GetDurabilities() : new TransferedDurabilities();
		TransferedDurabilities durabilities = inGameCardBase2.ContainedLiquid.GetDurabilities();
		if (!inGameCardBase.ContainedLiquid)
		{
			durabilities.Liquid = Mathf.Min(durabilities.Liquid, inGameCardBase.CardModel.MaxLiquidCapacity);
		}
		else
		{
			durabilities.Liquid = Mathf.Min(durabilities.Liquid, inGameCardBase.CardModel.MaxLiquidCapacity - inGameCardBase.ContainedLiquid.CurrentLiquidQuantity);
		}
		TransferedDurabilities transferedDurabilities = CardData.CalculateLiquidDurabilities(liquidA, durabilities);
		TransferedDurabilities transferedDurabilities2 = new TransferedDurabilities();
		transferedDurabilities2.Liquid = -transferedDurabilities.Liquid;
		if (!inGameCardBase.ContainedLiquid)
		{
			CardData cardData = inGameCardBase.CardModel.ContainedLiquidTransform ? inGameCardBase.CardModel.ContainedLiquidTransform : inGameCardBase2.ContainedLiquidModel;
			cardOnCardAction.ProducedCards = new CardsDropCollection[1];
			cardOnCardAction.ProducedCards[0] = new CardsDropCollection
			{
				CollectionName = "Creating liquid " + cardData,
				CollectionWeight = 1
			};
			cardOnCardAction.ProducedCards[0].SetLiquidDrop(new LiquidDrop(cardData, Vector2.one * transferedDurabilities.Liquid, transferedDurabilities));
		}
		else
		{
			cardOnCardAction.ReceivingCardChanges.ApplyDurabilityChanges(transferedDurabilities);
		}
		cardOnCardAction.GivenCardChanges.ApplyDurabilityChanges(transferedDurabilities2);
		cardOnCardAction.ActionName = LocalizedString.LiquidTransfer;
		cardOnCardAction.ActionDescription = new LocalizedString
		{
			LocalizationKey = "IGNOREKEY",
			DefaultText = LocalizedString.FillWithLiquid(inGameCardBase, inGameCardBase2.ContainedLiquidModel)
		};
		return cardOnCardAction;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0004AD54 File Offset: 0x00048F54
	public static CardOnCardAction GenerateMoveToLocationAction(InGameCardBase _ReceivingCard, InGameCardBase _GivenCard, string _AddedTooltip)
	{
		CardOnCardAction cardOnCardAction = new CardOnCardAction(LocalizedString.TransferToLocation, new LocalizedString
		{
			LocalizationKey = "IGNOREKEY",
			DefaultText = LocalizedString.MoveItemToLocation(_GivenCard, _ReceivingCard.CardModel.GetTravelDestination) + _AddedTooltip
		}, 0);
		cardOnCardAction.StackCompatible = true;
		cardOnCardAction.StackStopConditions = default(StackActionStopConditions);
		cardOnCardAction.StackStopConditions.LocationWeightIsFull = true;
		cardOnCardAction.DontShowDestroyMessage = true;
		cardOnCardAction.ExtraDurabilityModifications = new ExtraDurabilityChange[1];
		ExtraDurabilityChange extraDurabilityChange = new ExtraDurabilityChange();
		extraDurabilityChange.AppliesTo = RemoteDurabilityChanges.GivenCard;
		extraDurabilityChange.CardChanges = RemoteCardStateChanges.Destroy;
		extraDurabilityChange.SendToEnvironment = new EnvironmentCardDataRef[]
		{
			new EnvironmentCardDataRef
			{
				Card = _ReceivingCard.CardModel.GetTravelDestination,
				PrevEnv = _ReceivingCard.Environment,
				TravelIndex = _ReceivingCard.TravelCardIndex
			}
		};
		cardOnCardAction.ExtraDurabilityModifications[0] = extraDurabilityChange;
		return cardOnCardAction;
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0004AE2D File Offset: 0x0004902D
	public override void Init()
	{
		base.Init();
		this.FillDropsList();
		this.CachedFilter.Clear();
	}

	// Token: 0x04000A69 RID: 2665
	public LocalizedString CardName;

	// Token: 0x04000A6A RID: 2666
	public LocalizedString CardDescription;

	// Token: 0x04000A6B RID: 2667
	public bool CanBeRenamed;

	// Token: 0x04000A6C RID: 2668
	public Sprite CardImage;

	// Token: 0x04000A6D RID: 2669
	public Sprite CardBackground;

	// Token: 0x04000A6E RID: 2670
	public CardTypes CardType;

	// Token: 0x04000A6F RID: 2671
	public float ObjectWeight;

	// Token: 0x04000A70 RID: 2672
	public float WeightReductionWhenEquipped;

	// Token: 0x04000A71 RID: 2673
	public CardTag[] CardTags;

	// Token: 0x04000A72 RID: 2674
	public EquipmentTag[] EquipmentTags;

	// Token: 0x04000A73 RID: 2675
	public CardOrTagRef[] LiquidValidContainers;

	// Token: 0x04000A74 RID: 2676
	[Tooltip("Also makes the inventory of that card count for Action Requirements and Card Drop Chance Modifiers")]
	public bool InHandWhenEquipped;

	// Token: 0x04000A75 RID: 2677
	public bool InstancedEnvironment;

	// Token: 0x04000A76 RID: 2678
	public bool UniqueOnBoard;

	// Token: 0x04000A77 RID: 2679
	public CardData[] SpawningBlockedBy;

	// Token: 0x04000A78 RID: 2680
	public bool CannotSpawnDuringTickCatchup;

	// Token: 0x04000A79 RID: 2681
	public bool AlwaysUpdate;

	// Token: 0x04000A7A RID: 2682
	public bool CannotBeTrashed;

	// Token: 0x04000A7B RID: 2683
	public List<CardData> CarriesOverTo = new List<CardData>();

	// Token: 0x04000A7C RID: 2684
	public bool CanDragItemsToTravel;

	// Token: 0x04000A7D RID: 2685
	public bool DoesNotPile;

	// Token: 0x04000A7E RID: 2686
	[FormerlySerializedAs("DoesNotPin")]
	public bool DoesNotPinAutomatically;

	// Token: 0x04000A7F RID: 2687
	public AudioClip[] WhenCreatedSounds;

	// Token: 0x04000A80 RID: 2688
	public AmbienceSettings Ambience;

	// Token: 0x04000A81 RID: 2689
	public WeatherSpecialEffect[] VisualEffects;

	// Token: 0x04000A82 RID: 2690
	public Sprite LocationsBackground;

	// Token: 0x04000A83 RID: 2691
	public Sprite BaseBackground;

	// Token: 0x04000A84 RID: 2692
	public WeatherSet WeatherEffects;

	// Token: 0x04000A85 RID: 2693
	public LightSourceSettings LightSource;

	// Token: 0x04000A86 RID: 2694
	public bool IsDarkPlace;

	// Token: 0x04000A87 RID: 2695
	public bool IsHotPlace;

	// Token: 0x04000A88 RID: 2696
	public bool IsTravelCard;

	// Token: 0x04000A89 RID: 2697
	public DurabilityStat SpoilageTime;

	// Token: 0x04000A8A RID: 2698
	public DurabilityStat UsageDurability;

	// Token: 0x04000A8B RID: 2699
	public DurabilityStat FuelCapacity;

	// Token: 0x04000A8C RID: 2700
	[FormerlySerializedAs("ConsumableCharges")]
	public DurabilityStat Progress;

	// Token: 0x04000A8D RID: 2701
	public float LiquidEvaporationRate;

	// Token: 0x04000A8E RID: 2702
	public DurabilityStat SpecialDurability1;

	// Token: 0x04000A8F RID: 2703
	public DurabilityStat SpecialDurability2;

	// Token: 0x04000A90 RID: 2704
	public DurabilityStat SpecialDurability3;

	// Token: 0x04000A91 RID: 2705
	public DurabilityStat SpecialDurability4;

	// Token: 0x04000A92 RID: 2706
	[SpecialHeader("Actions", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public CardOnCardAction[] CardInteractions;

	// Token: 0x04000A93 RID: 2707
	[SpecialHeader("", HeaderSizes.Small, HeaderStyles.Underlined, -12f)]
	public FromStatChangeAction[] OnStatsChangeActions;

	// Token: 0x04000A94 RID: 2708
	[SpecialHeader("", HeaderSizes.Small, HeaderStyles.Underlined, -12f)]
	public List<DismantleCardAction> DismantleActions;

	// Token: 0x04000A95 RID: 2709
	[SpecialHeader("Passive Effects", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public bool AffectStatsOnlyWhenEquipped;

	// Token: 0x04000A96 RID: 2710
	public bool AffectStatsOnlyWhenNotInBackground;

	// Token: 0x04000A97 RID: 2711
	public bool AffectStatsOnlyOnBase;

	// Token: 0x04000A98 RID: 2712
	[StatModifierOptions(true, false)]
	public StatModifier[] PassiveStatEffects;

	// Token: 0x04000A99 RID: 2713
	[SpecialHeader("", HeaderSizes.Small, HeaderStyles.Underlined, -12f)]
	public PassiveEffect[] PassiveEffects;

	// Token: 0x04000A9A RID: 2714
	[SpecialHeader("", HeaderSizes.Small, HeaderStyles.Underlined, -12f)]
	public RemotePassiveEffect[] RemotePassiveEffects;

	// Token: 0x04000A9B RID: 2715
	[SpecialHeader("", HeaderSizes.Small, HeaderStyles.Underlined, -12f)]
	public LocalTickCounterRef[] ActiveCounters;

	// Token: 0x04000A9C RID: 2716
	[SpecialHeader("", HeaderSizes.Small, HeaderStyles.Underlined, -12f)]
	public LocalCounterEffect[] LocalCounterEffects;

	// Token: 0x04000A9D RID: 2717
	public CardsDropCollection[] DroppedOnDestroy;

	// Token: 0x04000A9E RID: 2718
	public bool DropFromTrashButton;

	// Token: 0x04000A9F RID: 2719
	[SpecialHeader("Liquid Container", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public float MaxLiquidCapacity;

	// Token: 0x04000AA0 RID: 2720
	public bool CannotEmpty;

	// Token: 0x04000AA1 RID: 2721
	public bool KeepName;

	// Token: 0x04000AA2 RID: 2722
	public LiquidDrop DefaultLiquidContained;

	// Token: 0x04000AA3 RID: 2723
	public Sprite DefaultLiquidImage;

	// Token: 0x04000AA4 RID: 2724
	public LiquidVisuals[] LiquidImages;

	// Token: 0x04000AA5 RID: 2725
	public CardOrTagRef[] ExclusivelyAcceptedLiquids;

	// Token: 0x04000AA6 RID: 2726
	public CardOrTagRef[] NOTAcceptedLiquids;

	// Token: 0x04000AA7 RID: 2727
	public CardData ContainedLiquidTransform;

	// Token: 0x04000AA8 RID: 2728
	[SpecialHeader("Inventory", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public bool InventoryIsHidden;

	// Token: 0x04000AA9 RID: 2729
	[SerializeField]
	private float MaxWeightCapacity;

	// Token: 0x04000AAA RID: 2730
	public float ContentWeightReduction;

	// Token: 0x04000AAB RID: 2731
	public CardData[] InventorySlots;

	// Token: 0x04000AAC RID: 2732
	public CardFilter InventoryFilter;

	// Token: 0x04000AAD RID: 2733
	public bool SpillsInventoryOnDestroy;

	// Token: 0x04000AAE RID: 2734
	public LocalizedString InventorySlotsText;

	// Token: 0x04000AAF RID: 2735
	public CookingConditions CookingConditions;

	// Token: 0x04000AB0 RID: 2736
	public bool TransferCookingResultsOnTransform;

	// Token: 0x04000AB1 RID: 2737
	public CookingRecipe[] CookingRecipes;

	// Token: 0x04000AB2 RID: 2738
	public Sprite CookingSprite;

	// Token: 0x04000AB3 RID: 2739
	public PassiveEffect[] EffectsToInventoryContent;

	// Token: 0x04000AB4 RID: 2740
	[SpecialHeader("Auto Unlock", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public List<CardOnBoardSubObjective> CardsOnBoard;

	// Token: 0x04000AB5 RID: 2741
	public List<TagOnBoardSubObjective> TagsOnBoard;

	// Token: 0x04000AB6 RID: 2742
	public List<StatSubObjective> StatValues;

	// Token: 0x04000AB7 RID: 2743
	public List<TimeObjective> TimeValues;

	// Token: 0x04000AB8 RID: 2744
	public List<ObjectiveSubObjective> CompletedObjectives;

	// Token: 0x04000AB9 RID: 2745
	public EndgameLog OnUnlockedLog;

	// Token: 0x04000ABA RID: 2746
	public LocalizedString UnlockConditionsDesc;

	// Token: 0x04000ABB RID: 2747
	public CardData ExplicitBlueprintNeeded;

	// Token: 0x04000ABC RID: 2748
	[SpecialHeader("Blueprint Construction", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public CardOnBoardCondition[] BlueprintCardConditions;

	// Token: 0x04000ABD RID: 2749
	public TagOnBoardCondition[] BlueprintTagConditions;

	// Token: 0x04000ABE RID: 2750
	[StatCondition]
	public StatValueTrigger[] BlueprintStatConditions;

	// Token: 0x04000ABF RID: 2751
	[Space]
	public CardOnBoardCondition[] BuildingCardConditions;

	// Token: 0x04000AC0 RID: 2752
	public TagOnBoardCondition[] BuildingTagConditions;

	// Token: 0x04000AC1 RID: 2753
	[StatCondition]
	public StatValueTrigger[] BuildingStatConditions;

	// Token: 0x04000AC2 RID: 2754
	[Space]
	public BlueprintStage[] BlueprintStages;

	// Token: 0x04000AC3 RID: 2755
	public AudioClip[] BuildSounds;

	// Token: 0x04000AC4 RID: 2756
	public AudioClip[] DeconstructSounds;

	// Token: 0x04000AC5 RID: 2757
	public int BuildingDaytimeCost;

	// Token: 0x04000AC6 RID: 2758
	public int DeconstructDaytimeCost;

	// Token: 0x04000AC7 RID: 2759
	public ActionTag[] BlueprintActionTags;

	// Token: 0x04000AC8 RID: 2760
	public bool BPRandomSingleDrop;

	// Token: 0x04000AC9 RID: 2761
	public CardDrop[] BlueprintResult;

	// Token: 0x04000ACA RID: 2762
	[StatModifierOptions(false, false)]
	public StatModifier[] BlueprintStatModifications;

	// Token: 0x04000ACB RID: 2763
	public ExtraDurabilityChange[] BlueprintCardModifications;

	// Token: 0x04000ACC RID: 2764
	public EndgameLog BlueprintFinishedLog;

	// Token: 0x04000ACD RID: 2765
	public float BlueprintUnlockSunsCost;

	// Token: 0x04000ACE RID: 2766
	[SpecialHeader("Exploration", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public ExplorationResult[] ExplorationResults;

	// Token: 0x04000ACF RID: 2767
	public CardDataRef[] EnvironmentImprovements;

	// Token: 0x04000AD0 RID: 2768
	public CardDataRef[] EnvironmentDamages;

	// Token: 0x04000AD1 RID: 2769
	[SpecialHeader("Environment", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public CardData[] DefaultEnvCards;

	// Token: 0x04000AD2 RID: 2770
	[SpecialHeader("Combat", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public bool IsWeapon;

	// Token: 0x04000AD3 RID: 2771
	public bool IsAmmo;

	// Token: 0x04000AD4 RID: 2772
	public EncounterDistanceCondition RequiredDistance;

	// Token: 0x04000AD5 RID: 2773
	public ActionRange ActionRange;

	// Token: 0x04000AD6 RID: 2774
	public CardOrTagRef[] AmmoNeeded;

	// Token: 0x04000AD7 RID: 2775
	public float AmmoUsageCost;

	// Token: 0x04000AD8 RID: 2776
	[Space]
	public Vector2 BaseClashValue;

	// Token: 0x04000AD9 RID: 2777
	public Vector2 ClashRangedInaccuracy;

	// Token: 0x04000ADA RID: 2778
	public PlayerEncounterVariable[] WeaponClashStatInfluences;

	// Token: 0x04000ADB RID: 2779
	public PlayerEncounterVariable[] WeaponDamageStatInfluences;

	// Token: 0x04000ADC RID: 2780
	public float WeaponReach;

	// Token: 0x04000ADD RID: 2781
	public Vector2 WeaponDamage;

	// Token: 0x04000ADE RID: 2782
	public Vector2 ClashStealthBonus;

	// Token: 0x04000ADF RID: 2783
	public Vector2 ClashIneffectiveRangeMalus;

	// Token: 0x04000AE0 RID: 2784
	public Vector2 ClashVsEscapeBonus;

	// Token: 0x04000AE1 RID: 2785
	public Vector2 DmgVsEscapeBonus;

	// Token: 0x04000AE2 RID: 2786
	public DamageType[] DamageTypes;

	// Token: 0x04000AE3 RID: 2787
	[StatModifierOptions(true, false)]
	public StatModifier[] WeaponActionStatChanges;

	// Token: 0x04000AE4 RID: 2788
	public EncounterLogMessage BrokenDuringCombatLog;

	// Token: 0x04000AE5 RID: 2789
	public EncounterLogMessage WeaponIneffectiveCombatLog;

	// Token: 0x04000AE6 RID: 2790
	[Header("Armor")]
	public bool IsArmor;

	// Token: 0x04000AE7 RID: 2791
	public ArmorValues ArmorValues;

	// Token: 0x04000AE8 RID: 2792
	[Header("Cover")]
	public bool IsCover;

	// Token: 0x04000AE9 RID: 2793
	public bool AppliesCoverWhenEquipped;

	// Token: 0x04000AEA RID: 2794
	public float PlayerAddedCover;

	// Token: 0x04000AEB RID: 2795
	public float EnemyAddedCover;

	// Token: 0x04000AEC RID: 2796
	public float PlayerAddedStealth;

	// Token: 0x04000AED RID: 2797
	public float EnemyAddedStealth;

	// Token: 0x04000AEE RID: 2798
	public SimpleHitProbabilityModifier PlayerCoverHitProbabilityModifiers;

	// Token: 0x04000AEF RID: 2799
	public SimpleHitProbabilityModifier EnemyCoverHitProbabilityModifiers;

	// Token: 0x04000AF0 RID: 2800
	[NonSerialized]
	public List<CardsDropCollection> AllDrops;

	// Token: 0x04000AF1 RID: 2801
	private CardFilter CachedFilter;

	// Token: 0x04000AF2 RID: 2802
	private static CardAction OnEvaporatedActionInstance;
}
