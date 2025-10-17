using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000053 RID: 83
public class CharacterScreen : MonoBehaviour
{
	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x0600037D RID: 893 RVA: 0x000254FF File Offset: 0x000236FF
	// (set) Token: 0x0600037E RID: 894 RVA: 0x00025507 File Offset: 0x00023707
	public int CurrentTab { get; private set; }

	// Token: 0x0600037F RID: 895 RVA: 0x00025510 File Offset: 0x00023710
	public DynamicLayoutSlot FindSlotFor(CardData _Card, bool _Pin, int _Index)
	{
		if (!this.CanEquip(_Card, null))
		{
			return null;
		}
		for (int i = 0; i < this.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (!this.EquipmentSlotsLine.Slots[i].AssignedCard && this.EquipmentSlotsLine.Slots[i].CanReceiveCard(_Card, null, _Pin, false))
			{
				DynamicLayoutSlot result = this.EquipmentSlotsLine.Slots[i];
				if (i != _Index && _Index >= -1)
				{
					this.MoveSlot(i, Mathf.Clamp(_Index, 0, this.EquipmentSlotsLine.Slots.Count - 1));
				}
				return result;
			}
		}
		return null;
	}

	// Token: 0x06000380 RID: 896 RVA: 0x000255BB File Offset: 0x000237BB
	private void MoveSlot(int _From, int _To)
	{
		if (_To == _From)
		{
			return;
		}
		this.EquipmentSlotsLine.MoveSlot(_From, _To);
	}

	// Token: 0x06000381 RID: 897 RVA: 0x000255CF File Offset: 0x000237CF
	public bool CanEquip(CardData _Card, InGameCardBase _Instance)
	{
		return string.IsNullOrEmpty(this.ReasonForNotEquipping(_Card, _Instance));
	}

	// Token: 0x06000382 RID: 898 RVA: 0x000255E0 File Offset: 0x000237E0
	public string ReasonForNotEquipping(CardData _Card, InGameCardBase _Instance)
	{
		if (!_Card)
		{
			return "";
		}
		if (!_Card.IsEquipment)
		{
			return LocalizedString.IsNotEquipment(_Card);
		}
		if (_Instance && _Instance.IsPinned)
		{
			return LocalizedString.CannotEquipPins;
		}
		for (int i = 0; i < _Card.EquipmentTags.Length; i++)
		{
			if (this.EquippedTags.ContainsKey(_Card.EquipmentTags[i]) && this.EquippedTags[_Card.EquipmentTags[i]] >= _Card.EquipmentTags[i].MaxEquipped && _Card.EquipmentTags[i].MaxEquipped > 0)
			{
				if (!_Instance)
				{
					return LocalizedString.CannotEquipMore(_Card.EquipmentTags[i]);
				}
				if (!this.HasCardEquipped(_Instance))
				{
					return LocalizedString.CannotEquipMore(_Card.EquipmentTags[i]);
				}
			}
		}
		return "";
	}

	// Token: 0x06000383 RID: 899 RVA: 0x000256B4 File Offset: 0x000238B4
	public int SlotIndex(DynamicLayoutSlot _Slot)
	{
		if (this.EquipmentSlotsLine.Slots.Contains(_Slot))
		{
			return this.EquipmentSlotsLine.Slots.IndexOf(_Slot);
		}
		return 0;
	}

	// Token: 0x06000384 RID: 900 RVA: 0x000256DC File Offset: 0x000238DC
	public DynamicLayoutSlot GetEquipmentForIndex(int _Index)
	{
		if (_Index < 0 || _Index >= this.EquipmentSlotsLine.Slots.Count)
		{
			return null;
		}
		return this.EquipmentSlotsLine.Slots[_Index];
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00025708 File Offset: 0x00023908
	public bool HasCardEquipped(CardData _Card, List<InGameCardBase> _Results = null)
	{
		bool result = false;
		for (int i = 0; i < this.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (this.EquipmentSlotsLine.Slots[i].AssignedCard && this.EquipmentSlotsLine.Slots[i].AssignedCard.CardModel == _Card)
			{
				if (_Results == null)
				{
					return true;
				}
				result = true;
				if (!_Results.Contains(this.EquipmentSlotsLine.Slots[i].AssignedCard))
				{
					_Results.Add(this.EquipmentSlotsLine.Slots[i].AssignedCard);
				}
			}
		}
		return result;
	}

	// Token: 0x06000386 RID: 902 RVA: 0x000257BC File Offset: 0x000239BC
	public bool HasCardEquipped(InGameCardBase _Card)
	{
		if (_Card == null)
		{
			return false;
		}
		for (int i = 0; i < this.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (this.EquipmentSlotsLine.Slots[i].AssignedCard && this.EquipmentSlotsLine.Slots[i].AssignedCard == _Card)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00025830 File Offset: 0x00023A30
	public bool IsNewlyEquipped(InGameCardBase _Card)
	{
		return !(_Card == null) && !(_Card.CardModel == null) && _Card.CardModel.IsImportantEquipment && this.HasCardEquipped(_Card) && !this.CheckedCards.Contains(_Card);
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00025884 File Offset: 0x00023A84
	public DynamicLayoutSlot HasNewCards()
	{
		if (this.EquipmentSlotsLine == null)
		{
			return null;
		}
		if (this.EquipmentSlotsLine.Slots == null)
		{
			return null;
		}
		for (int i = 0; i < this.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (this.EquipmentSlotsLine.Slots[i].AssignedCard && this.EquipmentSlotsLine.Slots[i].AssignedCard.CardModel && this.EquipmentSlotsLine.Slots[i].AssignedCard.CardModel.IsImportantEquipment && !this.CheckedCards.Contains(this.EquipmentSlotsLine.Slots[i].AssignedCard))
			{
				return this.EquipmentSlotsLine.Slots[i];
			}
		}
		return null;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00025968 File Offset: 0x00023B68
	public void CheckCards()
	{
		for (int i = 0; i < this.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (this.EquipmentSlotsLine.Slots[i].IsActive && this.EquipmentSlotsLine.Slots[i].AssignedCard && !this.CheckedCards.Contains(this.EquipmentSlotsLine.Slots[i].AssignedCard))
			{
				this.CheckedCards.Add(this.EquipmentSlotsLine.Slots[i].AssignedCard);
			}
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00025A0C File Offset: 0x00023C0C
	public bool HasTagsEquipped(CardTag[] _Tags, List<InGameCardBase> _Results = null)
	{
		bool result = false;
		for (int i = 0; i < this.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (this.EquipmentSlotsLine.Slots[i].AssignedCard && this.EquipmentSlotsLine.Slots[i].AssignedCard.CardModel.HasAnyTag(_Tags))
			{
				if (_Results == null)
				{
					return true;
				}
				result = true;
				if (!_Results.Contains(this.EquipmentSlotsLine.Slots[i].AssignedCard))
				{
					_Results.Add(this.EquipmentSlotsLine.Slots[i].AssignedCard);
				}
			}
		}
		return result;
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00025AC0 File Offset: 0x00023CC0
	public bool HasTagEquipped(CardTag _Tag, List<InGameCardBase> _Results = null)
	{
		bool result = false;
		for (int i = 0; i < this.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (this.EquipmentSlotsLine.Slots[i].AssignedCard && this.EquipmentSlotsLine.Slots[i].AssignedCard.CardModel.HasTag(_Tag))
			{
				if (_Results == null)
				{
					return true;
				}
				result = true;
				if (!_Results.Contains(this.EquipmentSlotsLine.Slots[i].AssignedCard))
				{
					_Results.Add(this.EquipmentSlotsLine.Slots[i].AssignedCard);
				}
			}
		}
		return result;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00025B74 File Offset: 0x00023D74
	public void EquipCard(CardData _Card)
	{
		bool flag = false;
		for (int i = 0; i < _Card.EquipmentTags.Length; i++)
		{
			if (_Card.EquipmentTags[i])
			{
				flag = true;
				if (this.EquippedTags.ContainsKey(_Card.EquipmentTags[i]))
				{
					Dictionary<EquipmentTag, int> equippedTags = this.EquippedTags;
					EquipmentTag key = _Card.EquipmentTags[i];
					int num = equippedTags[key];
					equippedTags[key] = num + 1;
				}
				else
				{
					this.EquippedTags.Add(_Card.EquipmentTags[i], 1);
				}
			}
		}
		if (flag)
		{
			this.RefreshSlots();
		}
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00025BFC File Offset: 0x00023DFC
	public void UnequipCard(CardData _Card, bool _UpdateSlots = true)
	{
		bool flag = false;
		for (int i = 0; i < _Card.EquipmentTags.Length; i++)
		{
			if (_Card.EquipmentTags[i])
			{
				flag = true;
				if (this.EquippedTags.ContainsKey(_Card.EquipmentTags[i]))
				{
					this.EquippedTags[_Card.EquipmentTags[i]] = Mathf.Max(0, this.EquippedTags[_Card.EquipmentTags[i]] - 1);
				}
				else
				{
					this.EquippedTags.Add(_Card.EquipmentTags[i], 0);
				}
			}
		}
		if (flag && _UpdateSlots)
		{
			this.RefreshSlots();
		}
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00025C94 File Offset: 0x00023E94
	public void ChangeTab(int _NewTab)
	{
		this.CurrentTab = _NewTab;
		this.EquipmentTabButton.Selected = (this.CurrentTab == 0);
		this.WoundsTabButton.Selected = (this.CurrentTab == 1);
		this.CharacterTabButton.Selected = (this.CurrentTab == 2);
		string text;
		switch (this.CurrentTab)
		{
		default:
			text = LocalizedString.Equipment;
			break;
		case 1:
			text = LocalizedString.Wounds;
			break;
		case 2:
			text = LocalizedString.Character;
			break;
		}
		text = text.ToUpper();
		this.TitleText.text = text;
		if (this.CurrentTab == 2)
		{
			this.EquipmentAndWoundsGroup.SetActive(false);
			this.CharacterSheetGroup.SetActive(true);
			return;
		}
		for (int i = 0; i < this.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (this.EquipmentSlotsLine.Slots[i].AssignedCard)
			{
				if (this.WoundsList.IsWound(this.EquipmentSlotsLine.Slots[i].AssignedCard.CardModel))
				{
					this.EquipmentSlotsLine.Slots[i].IsActive = (this.CurrentTab == 1);
				}
				else
				{
					this.EquipmentSlotsLine.Slots[i].IsActive = (this.CurrentTab == 0);
				}
				if (this.CurrentTab == 1)
				{
					this.EquipmentSlotsLine.MoveToPos(0f);
				}
			}
		}
		this.EquipmentAndWoundsGroup.SetActive(true);
		this.CharacterSheetGroup.SetActive(false);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00025E34 File Offset: 0x00024034
	public void RefreshSlots()
	{
		if (this.EquipmentSlotsLine.Slots.Count > 0)
		{
			for (int i = this.EquipmentSlotsLine.Slots.Count - 1; i >= 0; i--)
			{
				if (this.EquipmentSlotsLine.Slots[i].AssignedCard == null)
				{
					this.EquipmentSlotsLine.RemoveSlot(i);
				}
				else if (this.EquipmentSlotsLine.Slots[i].AssignedCard.Destroyed)
				{
					this.EquipmentSlotsLine.RemoveSlot(i);
				}
			}
		}
		for (int j = 0; j < this.MinEquipmentSlots; j++)
		{
			if (j >= this.EquipmentSlotsLine.Slots.Count)
			{
				this.EquipmentSlotsLine.AddSlot(this.EquipmentSlotsLine.Slots.Count);
			}
		}
		bool flag = true;
		for (int k = 0; k < this.EquipmentSlotsLine.Slots.Count; k++)
		{
			if (!this.EquipmentSlotsLine.Slots[k].AssignedCard)
			{
				flag = false;
				break;
			}
			if (this.EquipmentSlotsLine.Slots[k].AssignedCard.Destroyed)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			this.EquipmentSlotsLine.AddSlot(this.EquipmentSlotsLine.Slots.Count);
		}
		this.ChangeTab(this.CurrentTab);
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00025F90 File Offset: 0x00024190
	public void Init()
	{
		this.EquipmentSlotModel = new DynamicLayoutSlot(this.EquipmentSlotSettings, null);
		this.UnusedSlotsParent = new GameObject("UnusedSlots", new Type[]
		{
			typeof(RectTransform)
		}).transform;
		this.UnusedSlotsParent.transform.SetParent(this.EquipmentsParent.parent);
		this.UnusedSlotsParent.gameObject.SetActive(false);
		this.EquipmentSlotsLine.Init(this.EquipmentsParent, this.EquipmentSlotSettings, this.EquipmentScrollRect);
		this.EquipmentTabButton.Setup(0, "", LocalizedString.Equipment, false);
		this.EquipmentTabButton.NewNotification = false;
		this.EquipmentTabButton.Selected = true;
		this.WoundsTabButton.Setup(1, "", LocalizedString.Wounds, false);
		this.WoundsTabButton.NewNotification = false;
		this.WoundsTabButton.Selected = false;
		this.CharacterTabButton.Setup(2, "", LocalizedString.Character, false);
		this.CharacterTabButton.NewNotification = false;
		this.CharacterTabButton.Selected = false;
		IndexButton equipmentTabButton = this.EquipmentTabButton;
		equipmentTabButton.OnClicked = (Action<int>)Delegate.Combine(equipmentTabButton.OnClicked, new Action<int>(this.ChangeTab));
		IndexButton woundsTabButton = this.WoundsTabButton;
		woundsTabButton.OnClicked = (Action<int>)Delegate.Combine(woundsTabButton.OnClicked, new Action<int>(this.ChangeTab));
		IndexButton characterTabButton = this.CharacterTabButton;
		characterTabButton.OnClicked = (Action<int>)Delegate.Combine(characterTabButton.OnClicked, new Action<int>(this.ChangeTab));
		this.CurrentTab = 0;
		this.CharacterTabIcon.overrideSprite = GameManager.CurrentPlayerCharacter.CharacterPortrait;
		this.CharacterPortrait.overrideSprite = GameManager.CurrentPlayerCharacter.CharacterPortrait;
		this.CharacterName.text = GameManager.CurrentPlayerCharacter.CharacterName;
		if (string.IsNullOrEmpty(GameManager.CurrentPlayerCharacter.CharacterDescription))
		{
			this.CharacterBio.text = LocalizedString.EmptyCharacterBio;
		}
		else
		{
			this.CharacterBio.text = GameManager.CurrentPlayerCharacter.CharacterDescription;
		}
		for (int i = 0; i < GameManager.CurrentPlayerCharacter.CharacterPerks.Count; i++)
		{
			MenuPerkPreview menuPerkPreview = UnityEngine.Object.Instantiate<MenuPerkPreview>(this.PerkPreviewPrefab, this.PerksParent);
			menuPerkPreview.DontClearTooltipOnDisable = true;
			menuPerkPreview.Setup(GameManager.CurrentPlayerCharacter.CharacterPerks[i]);
		}
		this.RefreshSlots();
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00026210 File Offset: 0x00024410
	public void Toggle()
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.Close();
			return;
		}
		this.Open();
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0002622C File Offset: 0x0002442C
	public bool CardIsWound(CardData _Card)
	{
		return this.WoundsList && _Card.IsMandatoryEquipment && this.WoundsList.IsWound(_Card);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00026254 File Offset: 0x00024454
	public void Open()
	{
		MBSingleton<GraphicsManager>.Instance.CloseAllPopups();
		base.gameObject.SetActive(true);
		if (this.EquipmentSlotsLine)
		{
			DynamicLayoutSlot dynamicLayoutSlot = this.HasNewCards();
			if (dynamicLayoutSlot)
			{
				if (this.WoundsList.IsWound(dynamicLayoutSlot.AssignedCard.CardModel))
				{
					this.ChangeTab(1);
				}
				else
				{
					this.ChangeTab(0);
				}
				this.EquipmentSlotsLine.MoveViewTo(dynamicLayoutSlot, true, true);
				return;
			}
			if (this.CurrentTab == 2 && this.DontStayOnCharacterSheet)
			{
				this.ChangeTab(0);
				return;
			}
			this.ChangeTab(this.CurrentTab);
		}
	}

	// Token: 0x06000394 RID: 916 RVA: 0x000262EF File Offset: 0x000244EF
	public void Close()
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.CheckCards();
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00026310 File Offset: 0x00024510
	public void OpenLogJournal()
	{
		this.LogJournal.SetupWhilePlaying();
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0002631D File Offset: 0x0002451D
	private void LateUpdate()
	{
		if (this.CloseOnClick && this.CloseOnClick.UpdateClickedOutside())
		{
			this.Close();
		}
	}

	// Token: 0x04000483 RID: 1155
	public SlotSettings EquipmentSlotSettings;

	// Token: 0x04000484 RID: 1156
	public DynamicLayoutSlot EquipmentSlotModel;

	// Token: 0x04000485 RID: 1157
	public RectTransform EquipmentsParent;

	// Token: 0x04000486 RID: 1158
	public RectTransform EquipmentCardsParent;

	// Token: 0x04000487 RID: 1159
	public int MinEquipmentSlots;

	// Token: 0x04000488 RID: 1160
	public CardLine EquipmentSlotsLine;

	// Token: 0x04000489 RID: 1161
	public ScrollRect EquipmentScrollRect;

	// Token: 0x0400048A RID: 1162
	[SerializeField]
	private CloseOnClickOutside CloseOnClick;

	// Token: 0x0400048B RID: 1163
	[SerializeField]
	private TextMeshProUGUI TitleText;

	// Token: 0x0400048C RID: 1164
	[SerializeField]
	private WoundAlert WoundsList;

	// Token: 0x0400048D RID: 1165
	[SerializeField]
	private IndexButton EquipmentTabButton;

	// Token: 0x0400048E RID: 1166
	[SerializeField]
	private IndexButton WoundsTabButton;

	// Token: 0x0400048F RID: 1167
	[SerializeField]
	private IndexButton CharacterTabButton;

	// Token: 0x04000490 RID: 1168
	[SerializeField]
	private Image CharacterTabIcon;

	// Token: 0x04000491 RID: 1169
	[SerializeField]
	private GameObject EquipmentAndWoundsGroup;

	// Token: 0x04000492 RID: 1170
	[SerializeField]
	private GameObject CharacterSheetGroup;

	// Token: 0x04000493 RID: 1171
	[SerializeField]
	private bool DontStayOnCharacterSheet;

	// Token: 0x04000494 RID: 1172
	[SerializeField]
	private Image CharacterPortrait;

	// Token: 0x04000495 RID: 1173
	[SerializeField]
	private TextMeshProUGUI CharacterName;

	// Token: 0x04000496 RID: 1174
	[SerializeField]
	private TextMeshProUGUI CharacterBio;

	// Token: 0x04000497 RID: 1175
	[SerializeField]
	private RectTransform PerksParent;

	// Token: 0x04000498 RID: 1176
	[SerializeField]
	private MenuPerkPreview PerkPreviewPrefab;

	// Token: 0x04000499 RID: 1177
	[SerializeField]
	private EndgameMenu LogJournal;

	// Token: 0x0400049A RID: 1178
	private List<InGameCardBase> CheckedCards = new List<InGameCardBase>();

	// Token: 0x0400049B RID: 1179
	public Dictionary<EquipmentTag, int> EquippedTags = new Dictionary<EquipmentTag, int>();

	// Token: 0x0400049C RID: 1180
	private Transform UnusedSlotsParent;

	// Token: 0x0400049D RID: 1181
	private Rect WorldRect;
}
