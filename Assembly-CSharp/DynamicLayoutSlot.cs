using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class DynamicLayoutSlot
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060003EB RID: 1003 RVA: 0x00028489 File Offset: 0x00026689
	// (set) Token: 0x060003EC RID: 1004 RVA: 0x000284BE File Offset: 0x000266BE
	public bool IsActive
	{
		get
		{
			if (this.DynamicSlotObject != null)
			{
				return this.DynamicSlotObject.IsActive;
			}
			return this.PrivateSlotObject && this.PrivateSlotObject.gameObject.activeInHierarchy;
		}
		set
		{
			if (this.DynamicSlotObject != null)
			{
				this.DynamicSlotObject.SetActive(value, false);
				return;
			}
			if (this.PrivateSlotObject)
			{
				this.PrivateSlotObject.gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060003ED RID: 1005 RVA: 0x000284F4 File Offset: 0x000266F4
	// (set) Token: 0x060003EE RID: 1006 RVA: 0x000284FC File Offset: 0x000266FC
	public string TitleText { get; private set; }

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060003EF RID: 1007 RVA: 0x00028505 File Offset: 0x00026705
	// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0002850D File Offset: 0x0002670D
	public string SlotText { get; private set; }

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060003F1 RID: 1009 RVA: 0x00028516 File Offset: 0x00026716
	// (set) Token: 0x060003F2 RID: 1010 RVA: 0x0002851E File Offset: 0x0002671E
	public Color SlotTextColor { get; private set; }

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00028527 File Offset: 0x00026727
	// (set) Token: 0x060003F4 RID: 1012 RVA: 0x0002852F File Offset: 0x0002672F
	public bool LiquidImageActive { get; private set; }

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00028538 File Offset: 0x00026738
	// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00028540 File Offset: 0x00026740
	public string LiquidText { get; private set; }

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00028549 File Offset: 0x00026749
	// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00028551 File Offset: 0x00026751
	public Color LiquidTextColor { get; private set; }

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0002855A File Offset: 0x0002675A
	// (set) Token: 0x060003FA RID: 1018 RVA: 0x00028562 File Offset: 0x00026762
	public ContentPage HelpPage { get; private set; }

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x0002856B File Offset: 0x0002676B
	// (set) Token: 0x060003FC RID: 1020 RVA: 0x00028573 File Offset: 0x00026773
	public Sprite SlotImage { get; private set; }

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x0002857C File Offset: 0x0002677C
	// (set) Token: 0x060003FE RID: 1022 RVA: 0x00028584 File Offset: 0x00026784
	public float CookingProgressValue { get; private set; }

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x0002858D File Offset: 0x0002678D
	// (set) Token: 0x06000400 RID: 1024 RVA: 0x00028595 File Offset: 0x00026795
	public int CookingProgressRemainingTicks { get; private set; }

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000401 RID: 1025 RVA: 0x0002859E File Offset: 0x0002679E
	// (set) Token: 0x06000402 RID: 1026 RVA: 0x000285A6 File Offset: 0x000267A6
	public bool CookingProgressPaused { get; private set; }

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000403 RID: 1027 RVA: 0x000285AF File Offset: 0x000267AF
	// (set) Token: 0x06000404 RID: 1028 RVA: 0x000285B7 File Offset: 0x000267B7
	public string CookingCustomText { get; private set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x000285C0 File Offset: 0x000267C0
	// (set) Token: 0x06000406 RID: 1030 RVA: 0x000285C8 File Offset: 0x000267C8
	public bool CookingHideProgress { get; private set; }

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x000285D1 File Offset: 0x000267D1
	public Transform GetParent
	{
		get
		{
			if (this.DynamicSlotObject != null)
			{
				return this.DynamicSlotObject.PosTransform;
			}
			if (this.SlotObject)
			{
				return this.SlotObject.GetParent;
			}
			return null;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000408 RID: 1032 RVA: 0x00028601 File Offset: 0x00026801
	public bool IsVisible
	{
		get
		{
			if (this.DynamicSlotObject != null)
			{
				return this.DynamicSlotObject.IsActive && this.DynamicSlotObject.ElementObject;
			}
			return this.SlotObject;
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00028638 File Offset: 0x00026838
	public DynamicLayoutSlot(SlotSettings _Settings, DynamicElementRef _DynamicSlot, DynamicViewLayoutGroup _ParentGroup)
	{
		this.GM = MBSingleton<GameManager>.Instance;
		this.GraphicsM = MBSingleton<GraphicsManager>.Instance;
		this.Setup(_Settings);
		this.AlternatingImages = new List<Sprite>();
		this.CardPile = new List<InGameCardBase>();
		this.DynamicSlotObject = _DynamicSlot;
		this.ParentLayoutGroup = _ParentGroup;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x000286A4 File Offset: 0x000268A4
	public DynamicLayoutSlot(SlotSettings _Settings, CardSlot _SlotObject)
	{
		this.GM = MBSingleton<GameManager>.Instance;
		this.GraphicsM = MBSingleton<GraphicsManager>.Instance;
		this.Setup(_Settings);
		this.AlternatingImages = new List<Sprite>();
		this.CardPile = new List<InGameCardBase>();
		this.PrivateSlotObject = _SlotObject;
		if (this.PrivateSlotObject)
		{
			this.PrivateSlotObject.ParentSlotData = this;
			this.PrivateSlotObject.ImageListUpdated();
		}
		this.ParentLayoutGroup = null;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00028732 File Offset: 0x00026932
	private void Setup(SlotSettings _Settings)
	{
		if (!_Settings)
		{
			return;
		}
		this.SlotType = _Settings.SlotType;
		this.CompatibleCards = _Settings.CompatibleCards;
		this.CanHostPile = _Settings.CanHostPile;
		this.CanPin = _Settings.CanPin;
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0002876D File Offset: 0x0002696D
	public SlotInfo ToInfo()
	{
		return new SlotInfo(this.SlotType, this.Index);
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x00028780 File Offset: 0x00026980
	public Vector3 WorldPosition
	{
		get
		{
			if (this.PrivateSlotObject)
			{
				return this.PrivateSlotObject.transform.position;
			}
			if (this.DynamicSlotObject != null)
			{
				return this.DynamicSlotObject.WorldPos;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600040E RID: 1038 RVA: 0x000287B9 File Offset: 0x000269B9
	public Vector3 LocalPosition
	{
		get
		{
			if (this.PrivateSlotObject)
			{
				return this.PrivateSlotObject.transform.localPosition;
			}
			if (this.DynamicSlotObject != null)
			{
				return this.DynamicSlotObject.Position;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x000287F4 File Offset: 0x000269F4
	public Rect CurrentRect
	{
		get
		{
			if (this.PrivateSlotObject)
			{
				return this.PrivateSlotObject.CurrentRect;
			}
			if (this.DynamicSlotObject != null)
			{
				return this.DynamicSlotObject.Rectangle;
			}
			return default(Rect);
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000410 RID: 1040 RVA: 0x00028837 File Offset: 0x00026A37
	// (set) Token: 0x06000411 RID: 1041 RVA: 0x0002885C File Offset: 0x00026A5C
	public CardSlot SlotObject
	{
		get
		{
			if (this.PrivateSlotObject)
			{
				return this.PrivateSlotObject;
			}
			this.FindSlotObject();
			return this.PrivateSlotObject;
		}
		set
		{
			if (this.PrivateSlotObject)
			{
				this.PrivateSlotObject.ParentSlotData = null;
			}
			this.PrivateSlotObject = value;
			if (this.PrivateSlotObject)
			{
				this.PrivateSlotObject.ParentSlotData = this;
				this.PrivateSlotObject.ImageListUpdated();
			}
		}
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x000288B0 File Offset: 0x00026AB0
	private void FindSlotObject()
	{
		if (this.DynamicSlotObject != null && this.DynamicSlotObject.ElementObject)
		{
			this.PrivateSlotObject = this.DynamicSlotObject.ElementObject.GetComponent<CardSlot>();
			if (this.PrivateSlotObject)
			{
				this.PrivateSlotObject.ParentSlotData = this;
				this.PrivateSlotObject.ImageListUpdated();
			}
		}
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00028911 File Offset: 0x00026B11
	public void SetTitleText(string _Text)
	{
		this.TitleText = _Text;
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0002891A File Offset: 0x00026B1A
	public void UpdateText(string _Text)
	{
		this.SlotText = _Text;
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00028923 File Offset: 0x00026B23
	public void UpdateTextColor(Color _Col)
	{
		this.SlotTextColor = _Col;
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0002892C File Offset: 0x00026B2C
	public void SetLiquidImage(bool _Active)
	{
		this.LiquidImageActive = _Active;
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00028935 File Offset: 0x00026B35
	public void UpdateLiquidText(string _Text, Color _Color)
	{
		this.LiquidText = _Text;
		this.LiquidTextColor = _Color;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00028945 File Offset: 0x00026B45
	public void SetHelpPage(ContentPage _Page)
	{
		this.HelpPage = _Page;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0002894E File Offset: 0x00026B4E
	public void UpdateCookingProgress(float _Value, int _RemainingTicks, bool _Paused, string _CustomCookingText, bool _HideProgress)
	{
		this.CookingProgressValue = _Value;
		this.CookingProgressRemainingTicks = _RemainingTicks;
		this.CookingProgressPaused = _Paused;
		this.CookingCustomText = _CustomCookingText;
		this.CookingHideProgress = _HideProgress;
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x0600041A RID: 1050 RVA: 0x00028975 File Offset: 0x00026B75
	// (set) Token: 0x0600041B RID: 1051 RVA: 0x00028980 File Offset: 0x00026B80
	public int Index
	{
		get
		{
			return this.CurrentIndex;
		}
		set
		{
			this.CurrentIndex = value;
			if (this.AssignedCard)
			{
				for (int i = 0; i < this.CardPile.Count; i++)
				{
					if (this.CardPile[i])
					{
						if (this.CardPile[i].CurrentSlotInfo == null)
						{
							this.CardPile[i].CurrentSlotInfo = this.ToInfo();
						}
						else
						{
							this.CardPile[i].CurrentSlotInfo.SlotIndex = value;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00028A0D File Offset: 0x00026C0D
	public int CardPileCount(bool _IncludePin = true)
	{
		if (_IncludePin || !this.PinnedCard)
		{
			return this.CardPile.Count;
		}
		return this.CardPile.Count - 1;
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00028A38 File Offset: 0x00026C38
	public void SetImage(Sprite _Icon)
	{
		this.AlternatingImages.Clear();
		this.AlternatingImages.Add(_Icon);
		if (this.SlotObject)
		{
			this.SlotObject.ImageListUpdated();
		}
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00028A6C File Offset: 0x00026C6C
	public void SetImageList(List<Sprite> _List)
	{
		this.AlternatingImages.Clear();
		if (_List != null)
		{
			for (int i = 0; i < _List.Count; i++)
			{
				if (_List[i])
				{
					this.AlternatingImages.Add(_List[i]);
				}
			}
		}
		if (this.SlotObject)
		{
			this.SlotObject.ImageListUpdated();
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x0600041F RID: 1055 RVA: 0x00028AD0 File Offset: 0x00026CD0
	public InGameCardBase PinnedCard
	{
		get
		{
			if (this.CardPile.Count == 0)
			{
				return null;
			}
			for (int i = this.CardPile.Count - 1; i >= 0; i--)
			{
				if (this.CardPile[i].IsPinned)
				{
					return this.CardPile[i];
				}
			}
			return null;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000420 RID: 1056 RVA: 0x00028B25 File Offset: 0x00026D25
	public InGameCardBase AssignedCard
	{
		get
		{
			if (this.CardPile.Count == 0)
			{
				return null;
			}
			return this.CardPile[0];
		}
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00028B42 File Offset: 0x00026D42
	public InGameCardBase GetCardAtIndex(int _Index)
	{
		if (this.CardPile == null)
		{
			return null;
		}
		if (this.CardPile.Count == 0)
		{
			return null;
		}
		if (this.CardPile.Count <= _Index)
		{
			return null;
		}
		return this.CardPile[_Index];
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00028B79 File Offset: 0x00026D79
	public void OnSlotBecameVisible()
	{
		this.FindSlotObject();
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x00028B81 File Offset: 0x00026D81
	public void OnSlotBecameNotVisible()
	{
		this.PrivateSlotObject = null;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00028B8A File Offset: 0x00026D8A
	public bool PileCompatible(CardData _ForCard)
	{
		return _ForCard && (this.CanHostPile && _ForCard.CanPile) && (this.CardPileCount(true) < this.MaxPileCount || this.MaxPileCount <= 0);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00028BC8 File Offset: 0x00026DC8
	public bool CanReceiveCard(InGameCardBase _Card, bool _IgnoreAssignedCard)
	{
		if (_Card.IsPinned && !this.CanPin)
		{
			return false;
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (this.SlotType == SlotsTypes.Inventory)
		{
			if (_Card == this.GraphicsM.CurrentInspectionPopup.CurrentCard)
			{
				return false;
			}
			if (!this.GraphicsM.CurrentInspectionPopup.CurrentCard.CanReceiveInInventoryInstance(_Card))
			{
				return false;
			}
		}
		if (this.SlotType == SlotsTypes.Base && this.GM.MaxEnvWeight > 0f && this.GM.CurrentEnvWeight + _Card.CurrentWeight > this.GM.MaxEnvWeight && _Card.CurrentSlotInfo.SlotType != SlotsTypes.Base)
		{
			return false;
		}
		if (this.SlotType == SlotsTypes.Equipment)
		{
			if (!this.GraphicsM)
			{
				this.GraphicsM = MBSingleton<GraphicsManager>.Instance;
			}
			if (!this.GraphicsM.CharacterWindow.CanEquip(_Card.CardModel, _Card))
			{
				return false;
			}
		}
		return this.CanReceiveCard(_Card.CardModel, _Card.ContainedLiquidModel, _Card.IsPinned, _IgnoreAssignedCard);
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x00028CDC File Offset: 0x00026EDC
	public bool CanReceiveCard(CardData _Card, CardData _WithLiquid, bool _Pin, bool _IgnoreAssignedCard)
	{
		if (!_Card)
		{
			return false;
		}
		if (this.SlotType == SlotsTypes.Equipment)
		{
			if (!this.GraphicsM)
			{
				this.GraphicsM = MBSingleton<GraphicsManager>.Instance;
			}
			if (!this.GraphicsM.CharacterWindow.CanEquip(_Card, null))
			{
				return false;
			}
		}
		else if (_Card.IsMandatoryEquipment)
		{
			return false;
		}
		return (!this.AssignedCard || _IgnoreAssignedCard || (!(_Card != this.AssignedCard.CardModel) && this.PileCompatible(_Card))) && (this.CompatibleCards.SupportsCard(_Card, _WithLiquid) && (!_Pin || this.CanPin)) && (this.CardPileCount(true) < this.MaxPileCount || this.MaxPileCount <= 0);
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00028D9C File Offset: 0x00026F9C
	public void AssignCard(InGameCardBase _Card, bool _InstantMove = false)
	{
		if (this.AssignedCard && _Card && ((!this.PileCompatible(_Card.CardModel) && _Card != this.AssignedCard) || (this.PileCompatible(_Card.CardModel) && _Card.CardModel != this.AssignedCard.CardModel)))
		{
			Debug.LogWarning(this.Name + " already has an assigned card! " + this.AssignedCard.name, this.ParentLayoutGroup ? this.ParentLayoutGroup.gameObject : null);
		}
		if (this.SlotType == SlotsTypes.Inventory && _Card)
		{
			this.GraphicsM.InspectedCard.AddCardToInventory(_Card, MBSingleton<GraphicsManager>.Instance.CurrentInspectionPopup.GetIndex(this));
		}
		else if (this.SlotType == SlotsTypes.Exploration && _Card)
		{
			MBSingleton<ExplorationPopup>.Instance.ExplorationCard.AddCardToInventory(_Card, this.Index);
		}
		if (_Card)
		{
			DynamicLayoutSlot currentSlot = _Card.CurrentSlot;
			if (currentSlot != this && currentSlot)
			{
				if (currentSlot.SlotType != this.SlotType && this.PileCompatible(_Card.CardModel))
				{
					DynamicLayoutSlot dynamicLayoutSlot = this.GraphicsM.FindPileForCard(_Card.CardModel, _Card.ContainedLiquidModel, this.SlotType, true, this.Index, null, null, 0);
					if (dynamicLayoutSlot != this && dynamicLayoutSlot)
					{
						dynamicLayoutSlot.AssignCard(_Card, _InstantMove);
						return;
					}
				}
				this.AddCard(_Card, currentSlot, _InstantMove);
				currentSlot.RemoveSpecificCard(_Card, true, true);
				if (this.PileCompatible(_Card.CardModel) && currentSlot.AssignedCard && currentSlot.SlotType == this.SlotType && (!currentSlot.AssignedCard.IsPinned || this.CanPin))
				{
					this.AssignCard(currentSlot.AssignedCard, _InstantMove);
				}
			}
			else
			{
				this.AddCard(_Card, currentSlot, _InstantMove);
			}
		}
		else
		{
			this.RemoveCard();
		}
		if (this.PrivateSlotObject && this.PrivateSlotObject.EmptyGraphics)
		{
			this.PrivateSlotObject.EmptyGraphics.SetActive(this.CardPile.Count == 0);
		}
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00028FC8 File Offset: 0x000271C8
	private void AddCard(InGameCardBase _Card, DynamicLayoutSlot _PrevSlot, bool _InstantMove)
	{
		if (this.CardPile.Contains(_Card))
		{
			return;
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		this.CardPile.Add(_Card);
		_Card.SetSlot(this, _PrevSlot == this || _InstantMove);
		if (this.SlotType == SlotsTypes.Equipment)
		{
			MBSingleton<GraphicsManager>.Instance.CharacterWindow.EquipCard(_Card.CardModel);
		}
		if (this.SlotType == SlotsTypes.Equipment || this.SlotType == SlotsTypes.Item)
		{
			this.GM.CalculateCarryWeight();
		}
		else if (this.SlotType == SlotsTypes.Base || this.SlotType == SlotsTypes.Location)
		{
			this.GM.CalculateEnvironmentWeight(false);
		}
		this.SortCardPile();
		if (this.PrivateSlotObject)
		{
			this.PrivateSlotObject.RaycastTarget = (this.CardPile.Count > 0);
		}
		if (_Card.IsPinned)
		{
			return;
		}
		if (_PrevSlot && _PrevSlot.PinnedCard)
		{
			return;
		}
		if (this.GM.CardsStartPinned && _Card.CanBePinned)
		{
			InGamePinData pinData = this.GM.GetPinData(_Card.CardModel, _Card.ContainedLiquidModel);
			if (pinData != null)
			{
				if (!pinData.IsPinned || pinData.CorrespondingCard)
				{
					return;
				}
			}
			else if (_Card.CardModel.DoesNotPinAutomatically)
			{
				return;
			}
			this.PinCurrentCard(false);
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00029118 File Offset: 0x00027318
	private void RemoveCard()
	{
		if (this.CardPile.Count <= 0)
		{
			return;
		}
		InGameCardBase inGameCardBase = this.CardPile[0];
		if (inGameCardBase.CurrentSlot == this)
		{
			inGameCardBase.SetSlot(null, true);
		}
		this.CardPile.RemoveAt(0);
		if (this.SlotType == SlotsTypes.Equipment)
		{
			MBSingleton<GraphicsManager>.Instance.CharacterWindow.UnequipCard(inGameCardBase.CardModel, true);
		}
		this.SortCardPile();
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00029184 File Offset: 0x00027384
	public void RemoveSpecificCard(InGameCardBase _Card, bool _SortSlot, bool _RefreshEquipment = true)
	{
		if (this.CardPile.Count <= 0)
		{
			return;
		}
		if (!this.CardPile.Contains(_Card))
		{
			return;
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		this.CardPile.Remove(_Card);
		if (_SortSlot)
		{
			this.SortCardPile();
		}
		if (this.SlotType == SlotsTypes.Equipment)
		{
			MBSingleton<GraphicsManager>.Instance.CharacterWindow.UnequipCard(_Card.CardModel, _RefreshEquipment);
		}
		if (this.SlotType == SlotsTypes.Equipment || this.SlotType == SlotsTypes.Item)
		{
			this.GM.CalculateCarryWeight();
		}
		else if (this.SlotType == SlotsTypes.Base || this.SlotType == SlotsTypes.Location)
		{
			this.GM.CalculateEnvironmentWeight(false);
		}
		if (this.PrivateSlotObject && this.PrivateSlotObject.EmptyGraphics)
		{
			this.PrivateSlotObject.EmptyGraphics.SetActive(this.CardPile.Count == 0);
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00029275 File Offset: 0x00027475
	public bool ContainsCard(InGameCardBase _Card)
	{
		return this.CardPile != null && this.CardPile.Count != 0 && this.CardPile.Contains(_Card);
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0002929C File Offset: 0x0002749C
	public void LateUpdate()
	{
		if (this.SortingNeeded)
		{
			this.SortCardPile(false);
			this.SortingNeeded = false;
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x000292B4 File Offset: 0x000274B4
	public void SortCardPile()
	{
		this.SortingNeeded = true;
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x000292BD File Offset: 0x000274BD
	public void SortCardPileTransformsOnly()
	{
		this.SortCardPile(true);
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x000292C8 File Offset: 0x000274C8
	private void SortCardPile(bool _JustSortTransforms)
	{
		if (!_JustSortTransforms)
		{
			this.CardPile.Sort(new CardPileComparer(true));
		}
		this.SortCardTransforms();
		if (this.PrivateSlotObject)
		{
			if (this.PrivateSlotObject.CardSwap)
			{
				if (this.AssignedCard)
				{
					if (this.PrivateSlotObject.CardSwap.transform.parent != this.AssignedCard.transform)
					{
						this.PrivateSlotObject.CardSwap.transform.SetParent(this.AssignedCard.transform, false);
					}
					else
					{
						this.PrivateSlotObject.CardSwap.transform.SetAsLastSibling();
					}
				}
				else if (this.PrivateSlotObject.CardSwap.transform.parent != this.PrivateSlotObject.transform)
				{
					this.PrivateSlotObject.CardSwap.transform.SetParent(this.PrivateSlotObject.transform, false);
				}
				else
				{
					this.PrivateSlotObject.CardSwap.transform.SetAsLastSibling();
				}
			}
			if (this.PrivateSlotObject.ShowTextAboveCards)
			{
				this.PrivateSlotObject.SetTextsParent(this.AssignedCard ? this.GetParent : this.PrivateSlotObject.transform);
			}
		}
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0002941C File Offset: 0x0002761C
	private void SortCardTransforms()
	{
		if (this.CardPile.Count == 0 || !this.GM)
		{
			return;
		}
		int num = 0;
		for (int i = this.CardPile.Count - 1; i >= 0; i--)
		{
			if (!this.CardPile[i])
			{
				this.CardPile.RemoveAt(i);
			}
			else if (this.CardPile[i].Destroyed)
			{
				this.CardPile.RemoveAt(i);
			}
			else
			{
				num++;
				if (this.IsVisible && this.CardPile[i].transform.parent == this.GetParent && !this.GM.EnvironmentTransition)
				{
					this.CardPile[i].transform.SetAsLastSibling();
				}
				this.CardPile[i].UpdateActiveState();
				if (this.CardPile[i].CardVisuals && (this.CardPile[i].gameObject.activeSelf || this.SlotType == SlotsTypes.Event))
				{
					this.CardPile[i].CardVisuals.RefreshPileInfo(this.PinnedCard ? (num - 1) : num, this.PinnedCard ? (this.CardPile.Count - 1) : this.CardPile.Count);
				}
			}
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00029598 File Offset: 0x00027798
	public void ClearSlot(bool _ClearCardSlots)
	{
		if (_ClearCardSlots)
		{
			for (int i = this.CardPile.Count - 1; i >= 0; i--)
			{
				this.CardPile[i].SetSlot(null, true);
				this.RemoveSpecificCard(this.CardPile[i], false, true);
			}
			return;
		}
		this.CardPile.Clear();
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x000295F4 File Offset: 0x000277F4
	public List<InGameCardBase> GetCardPile(bool _GetLiquids, bool _IncludeContainers = false, bool _ForceBaseRow = false)
	{
		if (this.CardPileCount(true) == 0)
		{
			return null;
		}
		List<InGameCardBase> list = new List<InGameCardBase>();
		for (int i = 0; i < this.CardPile.Count; i++)
		{
			if ((this.SlotType != SlotsTypes.Base || !this.CardPile[i].IgnoreBaseRow || _ForceBaseRow || this.CardPile[i].CardModel.CardType == CardTypes.Base) && !this.CardPile[i].IsPinned)
			{
				if (_GetLiquids)
				{
					if (this.CardPile[i].ContainedLiquid)
					{
						list.Add(this.CardPile[i].ContainedLiquid);
					}
					if (_IncludeContainers)
					{
						list.Add(this.CardPile[i]);
					}
				}
				else
				{
					list.Add(this.CardPile[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x000296D5 File Offset: 0x000278D5
	public int CardIndexInPile(InGameCardBase _Card)
	{
		if (!this.CardPile.Contains(_Card))
		{
			return -1;
		}
		return this.CardPile.IndexOf(_Card);
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000434 RID: 1076 RVA: 0x000296F4 File Offset: 0x000278F4
	public int DraggedCardIndex
	{
		get
		{
			if (!GameManager.DraggedCard)
			{
				return -1;
			}
			if (this.CardPile == null)
			{
				return -1;
			}
			if (this.CardPile.Count == 0)
			{
				return -1;
			}
			for (int i = this.CardPile.Count - 1; i >= 0; i--)
			{
				if (GameManager.CardIsInDraggedStack(this.CardPile[i]))
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000435 RID: 1077 RVA: 0x00029758 File Offset: 0x00027958
	public InGameDraggableCard NextDraggableCard
	{
		get
		{
			if (!this.AssignedCard)
			{
				return null;
			}
			int draggedCardIndex = this.DraggedCardIndex;
			if (draggedCardIndex >= this.CardPileCount(false) - 1 || draggedCardIndex == -1)
			{
				return null;
			}
			if (this.CardPile[draggedCardIndex + 1].IsPinned)
			{
				return null;
			}
			return this.CardPile[draggedCardIndex + 1] as InGameDraggableCard;
		}
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x000297B8 File Offset: 0x000279B8
	public void PinCurrentCard(bool _Pulse)
	{
		if (this.AssignedCard == null)
		{
			return;
		}
		if (this.PinnedCard)
		{
			return;
		}
		if (!this.PileCompatible(this.AssignedCard.CardModel))
		{
			return;
		}
		if (_Pulse)
		{
			this.AssignedCard.Pulse(0f);
		}
		GameManager.PinCard(this.AssignedCard.CardModel, this.AssignedCard.ContainedLiquidModel, this);
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00029825 File Offset: 0x00027A25
	public void FreePin()
	{
		if (this.AssignedCard == null)
		{
			return;
		}
		if (!this.PinnedCard)
		{
			return;
		}
		GameManager.UnpinCard(this.PinnedCard);
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0002984F File Offset: 0x00027A4F
	public void AddFilter(CardFilter _Filter)
	{
		this.CompatibleCards.AddFilters(_Filter);
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0002985D File Offset: 0x00027A5D
	public void RemoveFilter(CardFilter _Filter)
	{
		this.CompatibleCards.RemoveFilters(_Filter);
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0002986B File Offset: 0x00027A6B
	public void ClearFilters()
	{
		this.CompatibleCards.Clear();
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00029878 File Offset: 0x00027A78
	public void PlayCardLandParticles()
	{
		if (this.SlotObject)
		{
			this.SlotObject.PlayCardLandParticles();
		}
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00029892 File Offset: 0x00027A92
	public static implicit operator bool(DynamicLayoutSlot _Slot)
	{
		return _Slot != null;
	}

	// Token: 0x04000506 RID: 1286
	public string Name;

	// Token: 0x04000507 RID: 1287
	public SlotsTypes SlotType;

	// Token: 0x04000508 RID: 1288
	public CardFilter CompatibleCards;

	// Token: 0x04000509 RID: 1289
	public bool CanHostPile;

	// Token: 0x0400050A RID: 1290
	public int MaxPileCount;

	// Token: 0x0400050B RID: 1291
	public bool CanPin;

	// Token: 0x0400050C RID: 1292
	private List<InGameCardBase> CardPile = new List<InGameCardBase>();

	// Token: 0x0400050D RID: 1293
	private GameManager GM;

	// Token: 0x0400050E RID: 1294
	private GraphicsManager GraphicsM;

	// Token: 0x04000517 RID: 1303
	public List<Sprite> AlternatingImages = new List<Sprite>();

	// Token: 0x0400051D RID: 1309
	public DynamicViewLayoutGroup ParentLayoutGroup;

	// Token: 0x0400051E RID: 1310
	private bool SortingNeeded;

	// Token: 0x0400051F RID: 1311
	public DynamicElementRef DynamicSlotObject;

	// Token: 0x04000520 RID: 1312
	protected CardSlot PrivateSlotObject;

	// Token: 0x04000521 RID: 1313
	private int CurrentIndex;
}
