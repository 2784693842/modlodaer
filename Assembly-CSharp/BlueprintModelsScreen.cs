using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class BlueprintModelsScreen : MonoBehaviour
{
	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060002DB RID: 731 RVA: 0x0001C86E File Offset: 0x0001AA6E
	// (set) Token: 0x060002DC RID: 732 RVA: 0x0001C876 File Offset: 0x0001AA76
	public CardData CurrentResearch { get; private set; }

	// Token: 0x060002DD RID: 733 RVA: 0x0001C880 File Offset: 0x0001AA80
	private void Awake()
	{
		this.CurrentSubTab = -1;
		this.InitLockedBlueprints();
		for (int i = 0; i < this.BlueprintTabs.Length; i++)
		{
			if (this.BlueprintTabs[i])
			{
				this.BlueprintTabs[i].FillSortingList();
			}
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0001C8C9 File Offset: 0x0001AAC9
	public void Toggle()
	{
		if (!base.gameObject.activeSelf)
		{
			this.Show();
			return;
		}
		this.Hide();
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0001C8E8 File Offset: 0x0001AAE8
	public void Show()
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.LockedBlueprintsPreviews.Count == 0 && this.GM.BlueprintPurchasing)
		{
			this.UpdateLockedBlueprints();
		}
		if (this.GM.BlueprintPurchasing)
		{
			if (this.SunsCount)
			{
				this.SunsCount.text = GameLoad.Instance.SaveData.Suns.ToString();
			}
			if (this.SunsCountObject)
			{
				this.SunsCountObject.SetActive(!this.GM.PurchasingWithTime);
			}
		}
		else if (this.SunsCountObject)
		{
			this.SunsCountObject.SetActive(false);
			if (this.CurrentSubTab == -1)
			{
				this.CurrentSubTab = 0;
			}
		}
		if (this.BlueprintTabs == null)
		{
			this.ShowWithoutTabs();
			return;
		}
		if (this.BlueprintTabs.Length == 0)
		{
			this.ShowWithoutTabs();
			return;
		}
		int num = 0;
		while (num < this.BlueprintTabs.Length || num < this.TabButtons.Count)
		{
			if (num >= this.TabButtons.Count)
			{
				this.TabButtons.Add(UnityEngine.Object.Instantiate<IndexButton>(this.TabButtonPrefab, this.TabsParent));
				IndexButton indexButton = this.TabButtons[num];
				indexButton.OnClicked = (Action<int>)Delegate.Combine(indexButton.OnClicked, new Action<int>(this.ShowTab));
			}
			if (num >= this.BlueprintTabs.Length)
			{
				this.TabButtons[num].gameObject.SetActive(false);
			}
			else
			{
				this.TabButtons[num].Setup(num, this.ShowTabNames ? this.BlueprintTabs[num].TabName : "", this.ShowTabNames ? "" : this.BlueprintTabs[num].TabName, false);
				this.TabButtons[num].Sprite = this.BlueprintTabs[num].TabIcon;
				if (!this.GM.BlueprintPurchasing)
				{
					this.TabButtons[num].gameObject.SetActive(this.AnyCardUnlocked(this.BlueprintTabs[num], false));
				}
				else
				{
					this.TabButtons[num].gameObject.SetActive(true);
				}
				this.TabButtons[num].NewNotification = false;
				for (int i = 0; i < this.BlueprintTabs[num].IncludedCards.Count; i++)
				{
					if (!this.GM.CheckedBlueprints.Contains(this.BlueprintTabs[num].IncludedCards[i]) && this.GM.BlueprintModelCards.Contains(this.BlueprintTabs[num].IncludedCards[i]))
					{
						this.TabButtons[num].NewNotification = true;
						break;
					}
				}
				if (this.BlueprintTabs[num].HasSubGroups)
				{
					int num2 = 0;
					while (num2 < this.BlueprintTabs[num].SubGroups.Count && !this.TabButtons[num].NewNotification)
					{
						if (this.BlueprintTabs[num].SubGroups[num2].IncludedCards != null)
						{
							for (int j = 0; j < this.BlueprintTabs[num].SubGroups[num2].IncludedCards.Count; j++)
							{
								if (!this.GM.CheckedBlueprints.Contains(this.BlueprintTabs[num].SubGroups[num2].IncludedCards[j]) && this.GM.BlueprintModelCards.Contains(this.BlueprintTabs[num].SubGroups[num2].IncludedCards[j]))
								{
									this.TabButtons[num].NewNotification = true;
									break;
								}
							}
						}
						num2++;
					}
				}
			}
			num++;
		}
		if (this.NoBlueprintsScreen)
		{
			this.NoBlueprintsScreen.SetActive(this.GM.BlueprintModelCards.Count == 0 && !this.GM.BlueprintPurchasing);
		}
		MBSingleton<GraphicsManager>.Instance.CloseAllPopups();
		base.gameObject.SetActive(true);
		if (this.OpenOnNewestTab)
		{
			Vector2Int lhs = Vector2Int.one * -1;
			if (this.GM.BlueprintModelCards.Count != this.GM.CheckedBlueprints.Count && this.GM.BlueprintModelCards.Count > 0)
			{
				for (int k = this.GM.BlueprintModelCards.Count - 1; k >= 0; k--)
				{
					if (!this.GM.CheckedBlueprints.Contains(this.GM.BlueprintModelCards[k]))
					{
						lhs = this.FindTabFor(this.GM.BlueprintModelCards[k]);
						if (lhs.x != -1)
						{
							this.CurrentTab = lhs.x;
							this.ShowTab(lhs.x);
							if (lhs.y > -1 || (this.GM.BlueprintPurchasing && lhs.y == -1))
							{
								this.CurrentSubTab = lhs.y;
								this.ShowSubTab(lhs.y);
							}
							this.BlueprintsLine.MoveViewTo(this.BlueprintsLine.Slots[k], true, false);
							break;
						}
					}
				}
			}
			if (lhs != Vector2Int.one * -1)
			{
				return;
			}
		}
		this.ShowTab(this.CurrentTab);
		this.UpdateResearchIcon();
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0001CE90 File Offset: 0x0001B090
	private void SetupSubTabs()
	{
		CardTabGroup cardTabGroup = this.BlueprintTabs[this.CurrentTab];
		bool flag = false;
		int num = 0;
		while (num < cardTabGroup.SubGroups.Count || num < this.SubTabButtons.Count)
		{
			if (num >= this.SubTabButtons.Count)
			{
				this.SubTabButtons.Add(UnityEngine.Object.Instantiate<IndexButton>(this.SubTabButtonPrefab, this.SubTabsParent));
				IndexButton indexButton = this.SubTabButtons[num];
				indexButton.OnClicked = (Action<int>)Delegate.Combine(indexButton.OnClicked, new Action<int>(this.ShowSubTab));
			}
			if (num >= cardTabGroup.SubGroups.Count)
			{
				this.SubTabButtons[num].gameObject.SetActive(false);
			}
			else
			{
				this.SubTabButtons[num].Setup(num, this.ShowTabNames ? cardTabGroup.SubGroups[num].TabName : "", this.ShowTabNames ? "" : cardTabGroup.SubGroups[num].TabName, false);
				this.SubTabButtons[num].Sprite = cardTabGroup.SubGroups[num].TabIcon;
				this.SubTabButtons[num].gameObject.SetActive(this.AnyCardUnlocked(cardTabGroup.SubGroups[num], false));
				this.SubTabButtons[num].NewNotification = false;
				int i = 0;
				while (i < cardTabGroup.SubGroups[num].IncludedCards.Count)
				{
					if (!this.GM.CheckedBlueprints.Contains(cardTabGroup.SubGroups[num].IncludedCards[i]) && this.GM.BlueprintModelCards.Contains(cardTabGroup.SubGroups[num].IncludedCards[i]))
					{
						this.SubTabButtons[num].NewNotification = (!this.GM.BlueprintPurchasing || this.GM.BlueprintModelStates[cardTabGroup.SubGroups[num].IncludedCards[i]] == BlueprintModelState.Available);
						if (!flag)
						{
							flag = (this.GM.BlueprintPurchasing && this.GM.BlueprintModelStates[cardTabGroup.SubGroups[num].IncludedCards[i]] == BlueprintModelState.Purchasable);
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			num++;
		}
		if (!this.GM.BlueprintPurchasing)
		{
			if (this.ShopSubTabButton)
			{
				this.ShopSubTabButton.gameObject.SetActive(false);
			}
			return;
		}
		if (!this.ShopSubTabButton)
		{
			this.ShopSubTabButton = UnityEngine.Object.Instantiate<IndexButton>(this.ShopButtonPrefab ? this.ShopButtonPrefab : this.SubTabButtonPrefab, this.SubTabsParent);
			IndexButton shopSubTabButton = this.ShopSubTabButton;
			shopSubTabButton.OnClicked = (Action<int>)Delegate.Combine(shopSubTabButton.OnClicked, new Action<int>(this.ShowSubTab));
		}
		this.ShopSubTabButton.transform.SetAsLastSibling();
		if (this.GM.PurchasingWithTime)
		{
			this.ShopSubTabButton.Setup(-1, this.ShowTabNames ? LocalizedString.BlueprintResearchTab : "", this.ShowTabNames ? "" : LocalizedString.BlueprintResearchTab, false);
		}
		else
		{
			this.ShopSubTabButton.Setup(-1, this.ShowTabNames ? LocalizedString.BlueprintShop : "", this.ShowTabNames ? "" : LocalizedString.BlueprintShop, false);
		}
		this.ShopSubTabButton.Sprite = this.ShopIcon;
		this.ShopSubTabButton.NewNotification = flag;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0001D268 File Offset: 0x0001B468
	private bool AnyCardUnlocked(CardTabGroup _Group, bool _IgnorePurchasables = false)
	{
		for (int i = 0; i < _Group.IncludedCards.Count; i++)
		{
			if (_Group.IncludedCards[i] && this.GM.BlueprintModelCards.Contains(_Group.IncludedCards[i]) && (this.GM.BlueprintModelStates[_Group.IncludedCards[i]] == BlueprintModelState.Available || _IgnorePurchasables))
			{
				return true;
			}
		}
		if (_Group.HasSubGroups)
		{
			for (int j = 0; j < _Group.SubGroups.Count; j++)
			{
				if (this.AnyCardUnlocked(_Group.SubGroups[j], true))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0001D318 File Offset: 0x0001B518
	private void ShowWithoutTabs()
	{
		MBSingleton<GraphicsManager>.Instance.CloseAllPopups();
		base.gameObject.SetActive(true);
		if (this.OpenOnNewestTab && this.GM.BlueprintModelCards.Count != this.GM.CheckedBlueprints.Count && this.GM.BlueprintModelCards.Count > 0)
		{
			for (int i = this.GM.BlueprintModelCards.Count - 1; i >= 0; i--)
			{
				if (!this.GM.CheckedBlueprints.Contains(this.GM.BlueprintModelCards[i]))
				{
					this.BlueprintsLine.MoveViewTo(this.BlueprintsLine.Slots[i], true, false);
					return;
				}
			}
		}
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x0001D3DC File Offset: 0x0001B5DC
	public void ShowBlueprint(CardData _Blueprint, bool _Pulse = false)
	{
		Vector2Int vector2Int = this.FindTabFor(_Blueprint);
		if (vector2Int.x == -1)
		{
			return;
		}
		this.CurrentTab = vector2Int.x;
		this.ShowTab(vector2Int.x);
		if (vector2Int.y > -1 || (this.GM.BlueprintPurchasing && vector2Int.y == -1))
		{
			this.CurrentSubTab = vector2Int.y;
			this.DontMoveView = true;
			this.ShowSubTab(vector2Int.y);
			this.DontMoveView = false;
		}
		base.StartCoroutine(this.MoveView(_Blueprint, _Pulse));
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0001D46E File Offset: 0x0001B66E
	private IEnumerator MoveView(CardData _ToCard, bool _Pulse)
	{
		yield return null;
		if (!this.GM.BlueprintModelStates.ContainsKey(_ToCard))
		{
			yield break;
		}
		if (this.GM.BlueprintModelStates[_ToCard] != BlueprintModelState.Locked)
		{
			for (int j = 0; j < this.BlueprintsLine.Slots.Count; j++)
			{
				if (this.BlueprintsLine.Slots[j].AssignedCard && this.BlueprintsLine.Slots[j].AssignedCard.CardModel == _ToCard)
				{
					this.BlueprintsLine.MoveViewTo(this.BlueprintsLine.Slots[j], true, _Pulse);
					yield break;
				}
			}
		}
		else
		{
			int i2;
			int i;
			for (i = 0; i < this.LockedBlueprintsPreviews.Count; i = i2 + 1)
			{
				if (this.LockedBlueprintsPreviews[i].AssociatedCard && this.LockedBlueprintsPreviews[i].AssociatedCard == _ToCard)
				{
					if (_Pulse)
					{
						this.BlueprintsLine.MoveViewTo(this.LockedBlueprintsPreviews[i].transform, true, delegate()
						{
							this.LockedBlueprintsPreviews[i].Pulse();
						});
					}
					else
					{
						this.BlueprintsLine.MoveViewTo(this.LockedBlueprintsPreviews[i].transform, true, null);
					}
					yield break;
				}
				i2 = i;
			}
		}
		yield break;
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0001D48C File Offset: 0x0001B68C
	public Vector2Int FindTabFor(CardData _Card)
	{
		if (!_Card)
		{
			return Vector2Int.one * -1;
		}
		for (int i = 0; i < this.BlueprintTabs.Length; i++)
		{
			if (this.BlueprintTabs[i].IncludedCards.Contains(_Card))
			{
				return new Vector2Int(i, -1);
			}
			if (this.BlueprintTabs[i].HasSubGroups)
			{
				int j = 0;
				while (j < this.BlueprintTabs[i].SubGroups.Count)
				{
					if (this.BlueprintTabs[i].SubGroups[j].IncludedCards.Contains(_Card))
					{
						if (this.GM.BlueprintModelStates[_Card] == BlueprintModelState.Available)
						{
							return new Vector2Int(i, j);
						}
						return new Vector2Int(i, -1);
					}
					else
					{
						j++;
					}
				}
			}
		}
		return new Vector2Int(-1, -2);
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x0001D55C File Offset: 0x0001B75C
	public bool BlueprintIsVisible(InGameCardBase _Blueprint)
	{
		if (!_Blueprint)
		{
			return false;
		}
		int num = Mathf.Clamp(this.CurrentTab, 0, this.TabButtons.Count - 1);
		if (this.CurrentTab != num)
		{
			return false;
		}
		if (!this.BlueprintTabs[num].HasSubGroups)
		{
			return this.BlueprintTabs[num].IncludedCards.Contains(_Blueprint.CardModel);
		}
		num = Mathf.Clamp(this.CurrentSubTab, -1, Mathf.Min(this.SubTabButtons.Count - 1, this.BlueprintTabs[this.CurrentTab].SubGroups.Count - 1));
		if (num != this.CurrentSubTab)
		{
			return false;
		}
		if (this.CurrentSubTab == -1 && this.GM.BlueprintPurchasing)
		{
			return this.BlueprintTabs[this.CurrentTab].BlueprintSortingIndex(_Blueprint) != -1 && this.GM.BlueprintModelStates[_Blueprint.CardModel] == BlueprintModelState.Purchasable;
		}
		return this.BlueprintTabs[this.CurrentTab].SubGroups[num].IncludedCards.Contains(_Blueprint.CardModel) && this.GM.BlueprintModelStates[_Blueprint.CardModel] == BlueprintModelState.Available;
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0001D690 File Offset: 0x0001B890
	private void ShowTab(int _Index)
	{
		int clamped = Mathf.Clamp(_Index, 0, this.TabButtons.Count - 1);
		for (int i = 0; i < this.TabButtons.Count; i++)
		{
			if (i == clamped && !this.TabButtons[i].gameObject.activeSelf && clamped < this.TabButtons.Count - 1)
			{
				int clamped2 = clamped;
				clamped = clamped2 + 1;
			}
			else
			{
				this.TabButtons[i].Selected = (i == clamped);
			}
		}
		if (this.CurrentTab != clamped)
		{
			this.CheckVisibleBlueprints();
			if (!this.GM.BlueprintPurchasing)
			{
				this.CurrentSubTab = 0;
			}
			else
			{
				this.CurrentSubTab = (this.GM.BlueprintPurchasing ? -1 : 0);
			}
		}
		this.CurrentTab = clamped;
		MBSingleton<GraphicsManager>.Instance.SortBlueprints();
		this.SetupSubTabs();
		if (this.CurrentSubTab == -1)
		{
			for (int j = 0; j < this.SubTabButtons.Count; j++)
			{
				if (this.SubTabButtons[j].gameObject.activeInHierarchy)
				{
					this.CurrentSubTab = j;
					break;
				}
			}
		}
		if (!this.BlueprintTabs[clamped].HasSubGroups)
		{
			this.BlueprintsLine.SortSlots((DynamicLayoutSlot a, DynamicLayoutSlot b) => this.BlueprintTabs[clamped].BlueprintSortingIndex(a.AssignedCard).CompareTo(this.BlueprintTabs[clamped].BlueprintSortingIndex(b.AssignedCard)));
			for (int k = 0; k < this.BlueprintsLine.Slots.Count; k++)
			{
				this.BlueprintsLine.Slots[k].Index = k;
				if (this.BlueprintsLine.Slots[k].AssignedCard)
				{
					this.BlueprintsLine.Slots[k].IsActive = this.BlueprintTabs[clamped].IncludedCards.Contains(this.BlueprintsLine.Slots[k].AssignedCard.CardModel);
				}
			}
		}
		else
		{
			this.ShowSubTab(this.CurrentSubTab);
		}
		this.UpdateResearchIcon();
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0001D8C4 File Offset: 0x0001BAC4
	private void ShowSubTab(int _Index)
	{
		int clamped = Mathf.Clamp(_Index, 0, Mathf.Min(this.SubTabButtons.Count - 1, this.BlueprintTabs[this.CurrentTab].SubGroups.Count - 1));
		if (!((_Index <= 0 && this.GM.BlueprintPurchasing) ? this.ShopSubTabButton : this.SubTabButtons[clamped]).gameObject.activeSelf)
		{
			clamped = 0;
			IndexButton indexButton = this.SubTabButtons[clamped];
		}
		for (int i = 0; i < this.SubTabButtons.Count; i++)
		{
			if (i == clamped && !this.SubTabButtons[i].gameObject.activeSelf)
			{
				if (clamped < this.BlueprintTabs[this.CurrentTab].SubGroups.Count - 1)
				{
					int clamped2 = clamped;
					clamped = clamped2 + 1;
				}
			}
			else if (_Index == -1 && !this.GM.BlueprintPurchasing)
			{
				this.SubTabButtons[i].Selected = (i == clamped);
			}
			else
			{
				this.SubTabButtons[i].Selected = (i == clamped && _Index != -1);
			}
		}
		if (this.ShopSubTabButton)
		{
			this.ShopSubTabButton.Selected = (_Index == -1);
		}
		if (this.CurrentSubTab != _Index)
		{
			this.CheckVisibleBlueprints();
		}
		this.CurrentSubTab = (this.GM.BlueprintPurchasing ? _Index : clamped);
		MBSingleton<GraphicsManager>.Instance.SortBlueprints();
		if (_Index == clamped)
		{
			this.BlueprintsLine.SortSlots((DynamicLayoutSlot a, DynamicLayoutSlot b) => this.BlueprintTabs[this.CurrentTab].SubGroups[clamped].BlueprintSortingIndex(a.AssignedCard).CompareTo(this.BlueprintTabs[this.CurrentTab].SubGroups[clamped].BlueprintSortingIndex(b.AssignedCard)));
		}
		else
		{
			this.BlueprintsLine.SortSlots((DynamicLayoutSlot a, DynamicLayoutSlot b) => this.BlueprintTabs[this.CurrentTab].BlueprintSortingIndex(a.AssignedCard).CompareTo(this.BlueprintTabs[this.CurrentTab].BlueprintSortingIndex(b.AssignedCard)));
			this.LockedBlueprintsPreviews.Sort((MenuCardPreview a, MenuCardPreview b) => this.BlueprintTabs[this.CurrentTab].BlueprintSortingIndex(a.AssociatedCard, 0).CompareTo(this.BlueprintTabs[this.CurrentTab].BlueprintSortingIndex(b.AssociatedCard, 0)));
		}
		for (int j = 0; j < this.BlueprintsLine.Slots.Count; j++)
		{
			this.BlueprintsLine.Slots[j].Index = j;
			if (this.BlueprintsLine.Slots[j].AssignedCard && this.BlueprintsLine.Slots[j].AssignedCard.CardModel)
			{
				if (_Index == -1 && this.GM.BlueprintPurchasing)
				{
					this.BlueprintsLine.Slots[j].IsActive = (this.BlueprintTabs[this.CurrentTab].BlueprintSortingIndex(this.BlueprintsLine.Slots[j].AssignedCard) != -1 && this.GM.BlueprintModelStates[this.BlueprintsLine.Slots[j].AssignedCard.CardModel] == BlueprintModelState.Purchasable);
				}
				else
				{
					this.BlueprintsLine.Slots[j].IsActive = (this.BlueprintTabs[this.CurrentTab].SubGroups[clamped].IncludedCards.Contains(this.BlueprintsLine.Slots[j].AssignedCard.CardModel) && this.GM.BlueprintModelStates[this.BlueprintsLine.Slots[j].AssignedCard.CardModel] == BlueprintModelState.Available);
				}
			}
		}
		if (this.CurrentSubTab == -1)
		{
			this.UpdateLockedBlueprints();
			this.LockedBlueprintsParent.gameObject.SetActive(true);
			this.LockedBlueprintsParent.transform.SetAsLastSibling();
			if (!this.DontMoveView)
			{
				this.BlueprintsLine.MoveToPos(0f, 0f, Ease.Linear, null);
				return;
			}
		}
		else
		{
			this.LockedBlueprintsParent.gameObject.SetActive(false);
		}
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0001DCBC File Offset: 0x0001BEBC
	private void CheckVisibleBlueprints()
	{
		if (this.BlueprintTabs.Length <= this.CurrentTab)
		{
			return;
		}
		if (this.BlueprintTabs[this.CurrentTab].HasSubGroups)
		{
			if (this.CurrentSubTab == -1)
			{
				for (int i = 0; i < this.BlueprintTabs[this.CurrentTab].SubGroups.Count; i++)
				{
					MBSingleton<GameManager>.Instance.CheckBlueprints(this.BlueprintTabs[this.CurrentTab].SubGroups[i], false);
				}
				this.ShopSubTabButton.NewNotification = false;
			}
			else if (this.SubTabButtons.Count > this.CurrentSubTab && this.BlueprintTabs[this.CurrentTab].SubGroups.Count > this.CurrentSubTab)
			{
				MBSingleton<GameManager>.Instance.CheckBlueprints(this.BlueprintTabs[this.CurrentTab].SubGroups[this.CurrentSubTab], true);
				this.SubTabButtons[this.CurrentSubTab].NewNotification = false;
			}
			for (int j = 0; j < this.BlueprintTabs[this.CurrentTab].SubGroups.Count; j++)
			{
				if (this.BlueprintTabs[this.CurrentTab].SubGroups[j].IncludedCards != null)
				{
					for (int k = 0; k < this.BlueprintTabs[this.CurrentTab].SubGroups[j].IncludedCards.Count; k++)
					{
						if (this.BlueprintTabs[this.CurrentTab].SubGroups[j].IncludedCards[k] && !this.GM.CheckedBlueprints.Contains(this.BlueprintTabs[this.CurrentTab].SubGroups[j].IncludedCards[k]) && this.GM.BlueprintModelCards.Contains(this.BlueprintTabs[this.CurrentTab].SubGroups[j].IncludedCards[k]))
						{
							return;
						}
					}
				}
			}
			this.TabButtons[this.CurrentTab].NewNotification = false;
			return;
		}
		MBSingleton<GameManager>.Instance.CheckBlueprints(this.BlueprintTabs[this.CurrentTab], true);
		this.TabButtons[this.CurrentTab].NewNotification = false;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0001DF18 File Offset: 0x0001C118
	private void InitLockedBlueprints()
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (!this.GM)
		{
			return;
		}
		int num = 0;
		while (num < this.GM.AllBlueprintModels.Count || num < this.LockedBlueprintsPreviews.Count)
		{
			if (num >= this.LockedBlueprintsPreviews.Count)
			{
				this.LockedBlueprintsPreviews.Add(UnityEngine.Object.Instantiate<MenuCardPreview>(this.LockedBlueprintsPreviewPrefab, this.LockedBlueprintsParent));
				this.LockedBlueprintsPreviews[this.LockedBlueprintsPreviews.Count - 1].Setup(this.GM.AllBlueprintModels[num], true);
			}
			if (num >= this.GM.AllBlueprintModels.Count)
			{
				this.LockedBlueprintsPreviews[num].gameObject.SetActive(false);
			}
			num++;
		}
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0001E000 File Offset: 0x0001C200
	private void UpdateLockedBlueprints()
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (!this.GM)
		{
			return;
		}
		for (int i = 0; i < this.LockedBlueprintsPreviews.Count; i++)
		{
			this.LockedBlueprintsPreviews[i].gameObject.SetActive(this.GM.BlueprintModelStates[this.LockedBlueprintsPreviews[i].AssociatedCard] == BlueprintModelState.Locked && this.BlueprintTabs[this.CurrentTab].HasCard(this.LockedBlueprintsPreviews[i].AssociatedCard, true));
			if (this.LockedBlueprintsPreviews[i].gameObject.activeSelf)
			{
				this.LockedBlueprintsPreviews[i].transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0001E0E0 File Offset: 0x0001C2E0
	public bool CanAffordBlueprint(CardData _Blueprint)
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		return (this.GM && this.GM.PurchasingWithTime) || _Blueprint.BlueprintUnlockSunsCost <= (float)GameLoad.Instance.SaveData.Suns;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0001E13C File Offset: 0x0001C33C
	public void ConfirmBuyBlueprint()
	{
		this.BuyBlueprint(this.BlueprintToBuy);
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0001E14C File Offset: 0x0001C34C
	public void FinishBlueprintResearch()
	{
		if (!this.CurrentResearch)
		{
			return;
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (!this.GM)
		{
			this.CurrentResearch = null;
			return;
		}
		if (this.GM.PurchasableBlueprintCards.Contains(this.CurrentResearch))
		{
			this.GM.PurchasableBlueprintCards.Remove(this.CurrentResearch);
		}
		this.GM.BlueprintModelStates[this.CurrentResearch] = BlueprintModelState.Available;
		this.CurrentResearch = null;
		this.UpdateResearchIcon();
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0001E1E7 File Offset: 0x0001C3E7
	public void LoadResearchedBlueprint(CardData _Card)
	{
		this.CurrentResearch = _Card;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0001E1F0 File Offset: 0x0001C3F0
	private void UpdateResearchIcon()
	{
		if (!this.GM.BlueprintPurchasing)
		{
			this.ResearchingIconTabs.SetActive(false);
			this.ResearchingIconSubTabs.SetActive(false);
			return;
		}
		if (!this.GM.PurchasingWithTime)
		{
			this.ResearchingIconTabs.SetActive(false);
			this.ResearchingIconSubTabs.SetActive(false);
			return;
		}
		if (!this.CurrentResearch)
		{
			this.ResearchingIconTabs.SetActive(false);
			this.ResearchingIconSubTabs.SetActive(false);
			return;
		}
		Vector2Int vector2Int = this.FindTabFor(this.CurrentResearch);
		if (vector2Int.x != -1)
		{
			if (this.ResearchingIconTabs.transform.parent != this.TabButtons[vector2Int.x].OtherIconParent)
			{
				this.ResearchingIconTabs.transform.SetParent(this.TabButtons[vector2Int.x].OtherIconParent);
			}
			this.ResearchingIconTabs.transform.localPosition = Vector3.zero;
			this.ResearchingIconTabs.SetActive(true);
		}
		else
		{
			this.ResearchingIconTabs.SetActive(false);
		}
		if (this.CurrentTab != vector2Int.x)
		{
			this.ResearchingIconSubTabs.SetActive(false);
			return;
		}
		if (vector2Int.y > -1)
		{
			if (this.ResearchingIconSubTabs.transform.parent != this.SubTabButtons[vector2Int.y].OtherIconParent)
			{
				this.ResearchingIconSubTabs.transform.SetParent(this.SubTabButtons[vector2Int.y].OtherIconParent);
			}
			this.ResearchingIconSubTabs.transform.localPosition = Vector3.zero;
			this.ResearchingIconSubTabs.SetActive(true);
			return;
		}
		if (vector2Int.y == -1)
		{
			if (this.ResearchingIconSubTabs.transform.parent != this.ShopSubTabButton.OtherIconParent)
			{
				this.ResearchingIconSubTabs.transform.SetParent(this.ShopSubTabButton.OtherIconParent);
			}
			this.ResearchingIconSubTabs.transform.localPosition = Vector3.zero;
			this.ResearchingIconSubTabs.SetActive(true);
			return;
		}
		this.ResearchingIconSubTabs.SetActive(false);
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0001E420 File Offset: 0x0001C620
	public void BuyBlueprint(InGameCardBase _Blueprint)
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (!this.GM)
		{
			return;
		}
		GameLoad instance = GameLoad.Instance;
		if (!instance)
		{
			return;
		}
		if (!_Blueprint)
		{
			return;
		}
		if (!_Blueprint.CardModel)
		{
			return;
		}
		if (!this.CanAffordBlueprint(_Blueprint.CardModel) || !this.GM.PurchasableBlueprintCards.Contains(_Blueprint.CardModel))
		{
			return;
		}
		if (this.BlueprintPurchaseConfirmScreen && (this.CurrentResearch || !this.GM.PurchasingWithTime))
		{
			if (!this.BlueprintPurchaseConfirmScreen.activeInHierarchy)
			{
				this.BlueprintToBuy = _Blueprint;
				if (this.GM.PurchasingWithTime && this.CurrentResearch)
				{
					this.BlueprintPurchaseConfirmText.text = LocalizedString.ConfirmBlueprintResearch(this.CurrentResearch, _Blueprint.CardModel);
				}
				this.BlueprintPurchaseConfirmScreen.SetActive(true);
				return;
			}
			this.BlueprintPurchaseConfirmScreen.SetActive(false);
		}
		if (this.GM.PurchasingWithTime)
		{
			this.CurrentResearch = _Blueprint.CardModel;
			this.UpdateResearchIcon();
			if (!this.GM.BlueprintResearchTimes.ContainsKey(_Blueprint.CardModel))
			{
				this.GM.BlueprintResearchTimes.Add(_Blueprint.CardModel, 0);
			}
			return;
		}
		if (this.GM.PurchasableBlueprintCards.Contains(_Blueprint.CardModel))
		{
			this.GM.PurchasableBlueprintCards.Remove(_Blueprint.CardModel);
		}
		this.GM.BlueprintModelStates[_Blueprint.CardModel] = BlueprintModelState.Available;
		if (!this.GM.PurchasingWithTime)
		{
			instance.SaveData.Suns -= (int)_Blueprint.CardModel.BlueprintUnlockSunsCost;
		}
		this.SaveOnClose = true;
		if (this.PurchasedFeedback && _Blueprint.CardVisuals)
		{
			UIFeedbackTextAndIcon uifeedbackTextAndIcon = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.PurchasedFeedback, _Blueprint.transform);
			base.StartCoroutine(uifeedbackTextAndIcon.PlayFeedback(_Blueprint.CardVisuals.BlueprintPurchasePos, (-_Blueprint.CardModel.BlueprintUnlockSunsCost).ToString(), MBSingleton<GraphicsManager>.Instance.NegativeTextColor, this.SunsIcon, Color.white));
			uifeedbackTextAndIcon = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.PurchasedFeedback, this.SunsCountObject.transform);
			base.StartCoroutine(uifeedbackTextAndIcon.PlayFeedback(this.SunsCountObject.transform.position, (-_Blueprint.CardModel.BlueprintUnlockSunsCost).ToString(), MBSingleton<GraphicsManager>.Instance.NegativeTextColor, this.SunsIcon, Color.white));
		}
		if (!this.GM.CheckedBlueprints.Contains(_Blueprint.CardModel))
		{
			this.GM.CheckedBlueprints.Add(_Blueprint.CardModel);
		}
		if (this.SunsCount)
		{
			this.SunsCount.text = instance.SaveData.Suns.ToString();
		}
		Vector2Int vector2Int = this.FindTabFor(_Blueprint.CardModel);
		if (vector2Int.y != -1)
		{
			this.CheckVisibleBlueprints();
		}
		vector2Int.x = this.CurrentTab;
		this.CurrentTab = vector2Int.x;
		this.ShowTab(vector2Int.x);
		if (vector2Int.y != -1)
		{
			this.CurrentSubTab = vector2Int.y;
			this.ShowSubTab(vector2Int.y);
		}
		for (int i = 0; i < this.BlueprintsLine.Slots.Count; i++)
		{
			if (this.BlueprintsLine.Slots[i] && this.BlueprintsLine.Slots[i].AssignedCard && this.BlueprintsLine.Slots[i].AssignedCard == _Blueprint)
			{
				this.BlueprintsLine.MoveViewTo(this.BlueprintsLine.Slots[i], true, true);
			}
		}
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0001E818 File Offset: 0x0001CA18
	public void Hide()
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (this.BlueprintPurchaseConfirmScreen)
		{
			this.BlueprintPurchaseConfirmScreen.SetActive(false);
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.SaveOnClose)
		{
			GameLoad.Instance.SaveMainDataToFile();
			this.SaveOnClose = false;
		}
		base.gameObject.SetActive(false);
		this.CheckVisibleBlueprints();
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0001E890 File Offset: 0x0001CA90
	private void LateUpdate()
	{
		if (this.CloseOnClick && this.CloseOnClick.UpdateClickedOutside() && MBSingleton<GraphicsManager>.Instance.CurrentInspectionPopup == null)
		{
			this.Hide();
		}
	}

	// Token: 0x04000364 RID: 868
	[SerializeField]
	private CloseOnClickOutside CloseOnClick;

	// Token: 0x04000365 RID: 869
	[SerializeField]
	private GameObject NoBlueprintsScreen;

	// Token: 0x04000366 RID: 870
	[SerializeField]
	private bool OpenOnNewestTab;

	// Token: 0x04000367 RID: 871
	[SerializeField]
	private CardLine BlueprintsLine;

	// Token: 0x04000368 RID: 872
	[SerializeField]
	private RectTransform TabsParent;

	// Token: 0x04000369 RID: 873
	[SerializeField]
	private RectTransform SubTabsParent;

	// Token: 0x0400036A RID: 874
	[SerializeField]
	private IndexButton TabButtonPrefab;

	// Token: 0x0400036B RID: 875
	[SerializeField]
	private IndexButton SubTabButtonPrefab;

	// Token: 0x0400036C RID: 876
	[SerializeField]
	private IndexButton ShopButtonPrefab;

	// Token: 0x0400036D RID: 877
	[SerializeField]
	private bool ShowTabNames;

	// Token: 0x0400036E RID: 878
	[SerializeField]
	private MenuCardPreview LockedBlueprintsPreviewPrefab;

	// Token: 0x0400036F RID: 879
	[SerializeField]
	private RectTransform LockedBlueprintsParent;

	// Token: 0x04000370 RID: 880
	[SerializeField]
	private GameObject BlueprintPurchaseConfirmScreen;

	// Token: 0x04000371 RID: 881
	[SerializeField]
	private TextMeshProUGUI BlueprintPurchaseConfirmText;

	// Token: 0x04000372 RID: 882
	[SerializeField]
	private GameObject ResearchingIconTabs;

	// Token: 0x04000373 RID: 883
	[SerializeField]
	private GameObject ResearchingIconSubTabs;

	// Token: 0x04000374 RID: 884
	[Space]
	public CardTabGroup[] BlueprintTabs;

	// Token: 0x04000375 RID: 885
	[SerializeField]
	private Sprite ShopIcon;

	// Token: 0x04000376 RID: 886
	[SerializeField]
	private GameObject SunsCountObject;

	// Token: 0x04000377 RID: 887
	[SerializeField]
	private TextMeshProUGUI SunsCount;

	// Token: 0x04000378 RID: 888
	[SerializeField]
	private Sprite SunsIcon;

	// Token: 0x04000379 RID: 889
	[SerializeField]
	private UIFeedbackTextAndIcon PurchasedFeedback;

	// Token: 0x0400037A RID: 890
	private Rect WorldRect;

	// Token: 0x0400037B RID: 891
	private GameManager GM;

	// Token: 0x0400037C RID: 892
	private InGameCardBase BlueprintToBuy;

	// Token: 0x0400037D RID: 893
	private bool SaveOnClose;

	// Token: 0x0400037E RID: 894
	[InspectorReadOnly]
	public int CurrentTab;

	// Token: 0x0400037F RID: 895
	[InspectorReadOnly]
	public int CurrentSubTab = -1;

	// Token: 0x04000380 RID: 896
	private List<IndexButton> TabButtons = new List<IndexButton>();

	// Token: 0x04000381 RID: 897
	private List<IndexButton> SubTabButtons = new List<IndexButton>();

	// Token: 0x04000382 RID: 898
	private IndexButton ShopSubTabButton;

	// Token: 0x04000383 RID: 899
	private List<MenuCardPreview> LockedBlueprintsPreviews = new List<MenuCardPreview>();

	// Token: 0x04000385 RID: 901
	private bool DontMoveView;
}
