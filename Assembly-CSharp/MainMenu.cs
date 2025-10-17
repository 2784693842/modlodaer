using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008E RID: 142
public class MainMenu : MonoBehaviour
{
	// Token: 0x060005EE RID: 1518 RVA: 0x0003E2FC File Offset: 0x0003C4FC
	private void Awake()
	{
		if (!GameLoad.Instance || !this.MenuNavigation)
		{
			return;
		}
		this.SaveData = GameLoad.Instance;
		this.SaveData.CheckSteamAchievements();
		GraphicsManager.SetActiveGroup(this.DemoObjects, false);
		this.RefreshCreatedCharacters();
		GameLoad instance = GameLoad.Instance;
		instance.OnSaveFail = (Action)Delegate.Combine(instance.OnSaveFail, new Action(this.ShowSaveErrorPopup));
		this.NameInputSounds = this.CharacterNameField.GetComponent<ButtonSounds>();
		this.BioInputSounds = this.CharacterBioField.GetComponent<ButtonSounds>();
		if (this.ImportPortraitButton)
		{
			this.ImportPortraitButtonTooltip = this.ImportPortraitButton.GetComponent<TooltipProvider>();
		}
		if (this.CharacterInfos)
		{
			this.LoadingPortraits = true;
			this.CharacterInfos.LoadCustomPortraits(this, delegate(bool b)
			{
				this.LoadingPortraits = false;
			});
		}
		this.DemoSaveWarningPopup.SetActive(GameLoad.Instance.SaveData.SavedFromFullGame && false);
		this.AllCharacterPerks = new List<CharacterPerk>();
		for (int i = 0; i < this.SaveData.DataBase.AllData.Count; i++)
		{
			if (this.SaveData.DataBase.AllData[i])
			{
				if (this.SaveData.DataBase.AllData[i].GetType() == typeof(CharacterPerk))
				{
					this.AllCharacterPerks.Add(this.SaveData.DataBase.AllData[i] as CharacterPerk);
					if (this.AllCharacterPerks[this.AllCharacterPerks.Count - 1].StartUnlocked)
					{
						this.AllCharacterPerks[this.AllCharacterPerks.Count - 1].ForceComplete(false);
						this.UnlockedPerks.Add(this.AllCharacterPerks[this.AllCharacterPerks.Count - 1]);
					}
					if (UniqueIDScriptable.ListContains(this.SaveData.SaveData.UnlockedPerks, this.AllCharacterPerks[this.AllCharacterPerks.Count - 1]))
					{
						this.UnlockedPerks.Add(this.AllCharacterPerks[this.AllCharacterPerks.Count - 1]);
					}
				}
				else if (this.SaveData.DataBase.AllData[i].GetType() == typeof(PerkGroup))
				{
					this.AllPerkGroups.Add(this.SaveData.DataBase.AllData[i] as PerkGroup);
				}
				else if (this.SaveData.DataBase.AllData[i].GetType() == typeof(PerkTabGroup))
				{
					this.AllPerkTabs.Add(this.SaveData.DataBase.AllData[i] as PerkTabGroup);
				}
				else if (this.SaveData.DataBase.AllData[i].GetType() == typeof(PlayerCharacter))
				{
					PlayerCharacter playerCharacter = this.SaveData.DataBase.AllData[i] as PlayerCharacter;
					if (playerCharacter.StartUnlocked || UniqueIDScriptable.ListContains(this.SaveData.SaveData.UnlockedCharacters, playerCharacter))
					{
						this.UnlockedCharacters.Add(playerCharacter);
					}
				}
			}
		}
		if (this.AllPerkTabs.Count > 0)
		{
			for (int j = 0; j < this.AllPerkTabs.Count; j++)
			{
				this.PerkTabs.Add(UnityEngine.Object.Instantiate<IndexButton>(this.PerkTabPrefab, this.PerkTabsParent));
				this.PerkTabs[j].Setup(j, "", this.AllPerkTabs[j].TabName, false);
				this.PerkTabs[j].Sprite = this.AllPerkTabs[j].Icon;
				IndexButton indexButton = this.PerkTabs[j];
				indexButton.OnClicked = (Action<int>)Delegate.Combine(indexButton.OnClicked, new Action<int>(this.SelectPerkTab));
			}
			this.SelectedPerkTab = 0;
			this.PerkTabs[0].Selected = true;
		}
		this.AllCharacterPerks.Sort(new Comparison<CharacterPerk>(this.ComparePerks));
		MenuingHelper menuNavigation = this.MenuNavigation;
		menuNavigation.OnScreenChanged = (Action<int>)Delegate.Combine(menuNavigation.OnScreenChanged, new Action<int>(this.OnScreenChanged));
		this.MenuNavigation.SetGroupActive(0);
		if (this.SelectCharacterCreationButton)
		{
			IndexButton selectCharacterCreationButton = this.SelectCharacterCreationButton;
			selectCharacterCreationButton.OnClicked = (Action<int>)Delegate.Combine(selectCharacterCreationButton.OnClicked, new Action<int>(this.SelectCharacter));
		}
		if (this.OfficialCharactersTab)
		{
			this.OfficialCharactersTab.Setup(0, "", LocalizedString.OfficialCharacters, false);
			IndexButton officialCharactersTab = this.OfficialCharactersTab;
			officialCharactersTab.OnClicked = (Action<int>)Delegate.Combine(officialCharactersTab.OnClicked, new Action<int>(this.SelectCharacterTab));
			this.CustomCharactersTab.Setup(1, "", LocalizedString.CustomCharacters, false);
			IndexButton customCharactersTab = this.CustomCharactersTab;
			customCharactersTab.OnClicked = (Action<int>)Delegate.Combine(customCharactersTab.OnClicked, new Action<int>(this.SelectCharacterTab));
		}
		this.SelectedCharacter = 0;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0003E878 File Offset: 0x0003CA78
	private int ComparePerks(CharacterPerk _A, CharacterPerk _B)
	{
		if (this.PerkIsEquipped(_A) && !this.PerkIsEquipped(_B))
		{
			return 1;
		}
		if (this.PerkIsEquipped(_B) && !this.PerkIsEquipped(_A))
		{
			return -1;
		}
		if (this.UnlockedPerks.Contains(_A) && !this.UnlockedPerks.Contains(_B))
		{
			return -1;
		}
		if (!this.UnlockedPerks.Contains(_A) && this.UnlockedPerks.Contains(_B))
		{
			return 1;
		}
		if (_B.DisplayPriority == _A.DisplayPriority)
		{
			return _B.name.CompareTo(_A.name);
		}
		return _B.DisplayPriority.CompareTo(_A.DisplayPriority);
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0003E91C File Offset: 0x0003CB1C
	private int ComparePerksTab(CharacterPerk _A, CharacterPerk _B)
	{
		if (this.PerkIsEquipped(_A) && this.PerkIsEquipped(_B))
		{
			return this.ComparePerks(_A, _B);
		}
		if (this.PerkIsEquipped(_A) && !this.PerkIsEquipped(_B))
		{
			return 1;
		}
		if (this.PerkIsEquipped(_B) && !this.PerkIsEquipped(_A))
		{
			return -1;
		}
		if (this.UnlockedPerks.Contains(_A) && !this.UnlockedPerks.Contains(_B))
		{
			return -1;
		}
		if (!this.UnlockedPerks.Contains(_A) && this.UnlockedPerks.Contains(_B))
		{
			return 1;
		}
		PerkTabGroup perkTabGroup = this.AllPerkTabs[this.SelectedPerkTab];
		if (perkTabGroup.ContainsPerk(_A) && !perkTabGroup.ContainsPerk(_B))
		{
			return -1;
		}
		if (!perkTabGroup.ContainsPerk(_A) && perkTabGroup.ContainsPerk(_B))
		{
			return 1;
		}
		return perkTabGroup.ContainedPerks.IndexOf(_A).CompareTo(perkTabGroup.ContainedPerks.IndexOf(_B));
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0003EA04 File Offset: 0x0003CC04
	private void OnScreenChanged(int _Screen)
	{
		switch (_Screen)
		{
		case 0:
			if (this.SimpleDiscordButton)
			{
				this.SimpleDiscordButton.SetActive(false);
			}
			if (this.NewsButton)
			{
				this.NewsButton.SetActive(true);
				return;
			}
			break;
		case 1:
			this.SetupSlotsScreen();
			return;
		case 2:
			this.SetupGamemodeScreen();
			return;
		case 3:
			this.SetupCharacterSelectionScreen();
			return;
		case 4:
			this.SetupCharacterCreationScreen();
			break;
		default:
			return;
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0003EA7A File Offset: 0x0003CC7A
	public void QuitGame()
	{
		if (this.QuitPopup.activeInHierarchy)
		{
			Application.Quit();
			this.QuitPopup.SetActive(false);
			return;
		}
		this.QuitPopup.SetActive(true);
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0003EAA7 File Offset: 0x0003CCA7
	private void ShowSaveErrorPopup()
	{
		if (this.SaveErrorPopup)
		{
			this.SaveErrorPopup.SetActive(true);
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0003EAC4 File Offset: 0x0003CCC4
	private void SetupSlotsScreen()
	{
		for (int i = 0; i < this.GameSlots.Length; i++)
		{
			this.GameSlots[i].Setup(this.SaveData.Games[i].MainData, i, this);
		}
		if (this.GameSlots.Length != 4)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"There's ",
				this.GameSlots.Length,
				" game slots set up in the menu for ",
				4,
				" official save slots."
			}));
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0003EB58 File Offset: 0x0003CD58
	public void ClickOnSlot(int _Index)
	{
		if (!this.SaveData)
		{
			return;
		}
		if (this.SaveData.Games[_Index].MainData.HasCardsData)
		{
			this.SaveData.LoadGame(_Index);
			return;
		}
		this.SelectedSlotIndex = _Index;
		this.SelectedGamemode = 0;
		this.MenuNavigation.SetGroupActive(3);
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0003EBB8 File Offset: 0x0003CDB8
	public void DeleteSlot(int _Index)
	{
		if (!this.SaveData)
		{
			return;
		}
		this.SaveData.DeleteGameData(_Index);
		this.GameSlots[_Index].Setup(this.SaveData.Games[_Index].MainData, _Index, this);
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0003EC04 File Offset: 0x0003CE04
	private void SetupGamemodeScreen()
	{
		this.SelectedGamemode = Mathf.Clamp(this.SelectedGamemode, 0, this.AvailableGamemodes.Length - 1);
		if (this.GamemodeSelectButtonPrefab && this.GamemodeListParent)
		{
			int num = 0;
			while (num < this.GamemodeListButtons.Count || num < this.AvailableGamemodes.Length)
			{
				if (num >= this.GamemodeListButtons.Count)
				{
					this.GamemodeListButtons.Add(UnityEngine.Object.Instantiate<IndexButton>(this.GamemodeSelectButtonPrefab, this.GamemodeListParent));
					IndexButton indexButton = this.GamemodeListButtons[num];
					indexButton.OnClicked = (Action<int>)Delegate.Combine(indexButton.OnClicked, new Action<int>(this.SelectGamemode));
				}
				if (num >= this.AvailableGamemodes.Length)
				{
					this.GamemodeListButtons[num].gameObject.SetActive(false);
				}
				else
				{
					this.GamemodeListButtons[num].Selected = (num == this.SelectedGamemode);
					this.GamemodeListButtons[num].Setup(num, this.AvailableGamemodes[num].ModeName, null, false);
					this.GamemodeListButtons[num].Sprite = this.AvailableGamemodes[num].ModeIllustration;
					this.GamemodeListButtons[num].gameObject.SetActive(true);
				}
				num++;
			}
		}
		this.SelectGamemode(this.SelectedGamemode);
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0003ED74 File Offset: 0x0003CF74
	private void SelectGamemode(int _Index)
	{
		this.SelectedGamemode = _Index;
		Gamemode gamemode = this.AvailableGamemodes[this.SelectedGamemode];
		for (int i = 0; i < this.GamemodeListButtons.Count; i++)
		{
			this.GamemodeListButtons[i].Selected = (i == this.SelectedGamemode);
		}
		this.GamemodeName.text = gamemode.ModeName;
		this.GamemodeDesc.text = gamemode.ModeDescription;
		this.GamemodeIllustration.overrideSprite = gamemode.ModeIllustration;
		int num = 0;
		while (num < gamemode.PlayableCharacters.Length || num < this.GamemodeCharacters.Count)
		{
			if (num >= this.GamemodeCharacters.Count)
			{
				this.GamemodeCharacters.Add(UnityEngine.Object.Instantiate<IndexButton>(this.CharacterPreviewPrefab, this.GamemodeCharactersParent));
			}
			if (num >= gamemode.PlayableCharacters.Length)
			{
				this.GamemodeCharacters[num].gameObject.SetActive(false);
			}
			else
			{
				this.GamemodeCharacters[num].Setup(num, "", gamemode.PlayableCharacters[num].CharacterName, false);
				this.GamemodeCharacters[num].Sprite = gamemode.PlayableCharacters[num].CharacterPortrait;
				this.GamemodeCharacters[num].gameObject.SetActive(true);
			}
			num++;
		}
		if (this.GamemodeEnvPreview)
		{
			this.GamemodeEnvPreview.Setup(gamemode.Environment, false);
		}
		if (this.GamemodeWeatherPreview)
		{
			this.GamemodeWeatherPreview.Setup(gamemode.Weather, false);
		}
		List<CardData> startingCards = gamemode.GetStartingCards();
		int num2 = 0;
		while (num2 < startingCards.Count || num2 < this.GamemodeStartingCards.Count)
		{
			if (num2 >= this.GamemodeStartingCards.Count)
			{
				this.GamemodeStartingCards.Add(UnityEngine.Object.Instantiate<MenuCardPreview>(this.CardPreviewPrefab, this.GamemodeCardsParent));
			}
			if (num2 >= startingCards.Count)
			{
				this.GamemodeStartingCards[num2].gameObject.SetActive(false);
			}
			else
			{
				this.GamemodeStartingCards[num2].Setup(startingCards[num2], false);
				this.GamemodeStartingCards[num2].gameObject.SetActive(true);
			}
			num2++;
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0003EFD0 File Offset: 0x0003D1D0
	private void RefreshCreatedCharacters()
	{
		for (int i = 0; i < this.CreatedCharacters.Count; i++)
		{
			if (this.AvailableGamemodes[this.SelectedGamemode].PremadeCustomCharacters == null || i >= this.AvailableGamemodes[this.SelectedGamemode].PremadeCustomCharacters.Length)
			{
				UnityEngine.Object.Destroy(this.CreatedCharacters[i]);
			}
		}
		this.CreatedCharacters.Clear();
		if (this.AvailableGamemodes[this.SelectedGamemode].PremadeCustomCharacters != null && this.AvailableGamemodes[this.SelectedGamemode].PremadeCustomCharacters.Length != 0)
		{
			for (int j = 0; j < this.AvailableGamemodes[this.SelectedGamemode].PremadeCustomCharacters.Length; j++)
			{
				this.CreatedCharacters.Add(this.AvailableGamemodes[this.SelectedGamemode].PremadeCustomCharacters[j]);
			}
		}
		if (!this.SaveData)
		{
			return;
		}
		if (this.SaveData.SaveData.CreatedCharacters != null)
		{
			if (this.CustomCharacterAchievement && this.SaveData.SaveData.CreatedCharacters.Count > 0)
			{
				this.CustomCharacterAchievement.ForceComplete(false);
			}
			for (int k = 0; k < this.SaveData.SaveData.CreatedCharacters.Count; k++)
			{
				this.CreatedCharacters.Add(this.LoadCustomCharacter(this.SaveData.SaveData.CreatedCharacters[k]));
			}
		}
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0003F139 File Offset: 0x0003D339
	public PlayerCharacter LoadCustomCharacter(PlayerCharacterSaveData _FromData)
	{
		PlayerCharacter playerCharacter = ScriptableObject.CreateInstance<PlayerCharacter>();
		playerCharacter.CopyCards(this.CharacterBaseModel);
		playerCharacter.LoadCustomCharacter(_FromData, this.CharacterInfos.GetCharacterPortrait(_FromData.PortraitID));
		return playerCharacter;
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0003F164 File Offset: 0x0003D364
	private PlayerCharacter GetSelectedCharacter()
	{
		return this.GetCharacter(this.SelectedCharacter);
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0003F174 File Offset: 0x0003D374
	private PlayerCharacter GetCharacter(int _Index)
	{
		if (_Index < this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length)
		{
			return this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters[_Index];
		}
		if (_Index - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length < this.CreatedCharacters.Count)
		{
			return this.CreatedCharacters[_Index - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length];
		}
		return null;
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0003F1F4 File Offset: 0x0003D3F4
	private void SetupCharacterSelectionScreen()
	{
		float verticalNormalizedPosition = this.CharacterListScrollRect.verticalNormalizedPosition;
		this.RefreshCreatedCharacters();
		this.SelectedCharacter = Mathf.Clamp(this.SelectedCharacter, 0, this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length + this.CreatedCharacters.Count);
		PlayerCharacter selectedCharacter = this.GetSelectedCharacter();
		if (selectedCharacter == null)
		{
			this.SelectedCharacter = this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length + this.CreatedCharacters.Count;
		}
		else if (selectedCharacter.EditorOnly && !Application.isEditor && !Debug.isDebugBuild)
		{
			this.SelectedCharacter = this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length + this.CreatedCharacters.Count;
		}
		this.CharacterScreenTitle.text = LocalizedString.SelectCharacter;
		int num = this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length;
		if (this.CharacterSelectButtonPrefab && this.CharacterListParent)
		{
			int num2 = 0;
			while (num2 < this.CharacterListButtons.Count || num2 < num)
			{
				if (num2 >= this.CharacterListButtons.Count)
				{
					this.CharacterListButtons.Add(UnityEngine.Object.Instantiate<MenuCharacterButton>(this.CharacterSelectButtonPrefab, this.CharacterListParent));
					MenuCharacterButton menuCharacterButton = this.CharacterListButtons[num2];
					menuCharacterButton.OnClicked = (Action<int>)Delegate.Combine(menuCharacterButton.OnClicked, new Action<int>(this.SelectCharacter));
				}
				if (num2 >= num)
				{
					this.CharacterListButtons[num2].gameObject.SetActive(false);
				}
				else if (this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters[num2].EditorOnly && !Application.isEditor && !Debug.isDebugBuild)
				{
					this.CharacterListButtons[num2].gameObject.SetActive(false);
				}
				else
				{
					this.CharacterListButtons[num2].Selected = (num2 == this.SelectedCharacter);
					this.CharacterListButtons[num2].Setup(num2, this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters[num2], this.CharacterInfos, this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters[num2].IsUnlocked);
					this.CharacterListButtons[num2].gameObject.SetActive(true);
				}
				num2++;
			}
			int num3 = 0;
			while (num3 + num < this.CharacterListButtons.Count || num3 < this.CreatedCharacters.Count)
			{
				int num4 = num3 + num;
				if (num4 >= this.CharacterListButtons.Count)
				{
					this.CharacterListButtons.Add(UnityEngine.Object.Instantiate<MenuCharacterButton>(this.CharacterSelectButtonPrefab, this.CharacterListParent));
					MenuCharacterButton menuCharacterButton2 = this.CharacterListButtons[num4];
					menuCharacterButton2.OnClicked = (Action<int>)Delegate.Combine(menuCharacterButton2.OnClicked, new Action<int>(this.SelectCharacter));
				}
				if (num3 >= this.CreatedCharacters.Count)
				{
					this.CharacterListButtons[num4].gameObject.SetActive(false);
				}
				else
				{
					this.CharacterListButtons[num4].Selected = (num4 == this.SelectedCharacter);
					this.CharacterListButtons[num4].Setup(num4, this.CreatedCharacters[num3], this.CharacterInfos, true);
					this.CharacterListButtons[num4].gameObject.SetActive(true);
				}
				num3++;
			}
		}
		if (this.SelectCharacterCreationButton)
		{
			this.SelectCharacterCreationButton.Setup(this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length + this.CreatedCharacters.Count, LocalizedString.CharacterCreator, null, false);
			this.SelectCharacterCreationButton.transform.SetAsFirstSibling();
			this.SelectCharacterCreationButton.Selected = (this.SelectedCharacter == this.SelectCharacterCreationButton.Index);
		}
		this.SelectCharacterTab(this.SelectedCharacterTab);
		this.RefreshSunsAndMoonsCounts();
		Canvas.ForceUpdateCanvases();
		this.CharacterListScrollRect.verticalNormalizedPosition = verticalNormalizedPosition;
		this.SelectCharacter(this.SelectedCharacter);
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0003F618 File Offset: 0x0003D818
	private void SelectCharacterTab(int _Tab)
	{
		this.SelectedCharacterTab = _Tab;
		int num = this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length;
		this.OfficialCharactersTab.Selected = (this.SelectedCharacterTab == 0);
		this.CustomCharactersTab.Selected = (this.SelectedCharacterTab == 1);
		this.SelectCharacterCreationButton.gameObject.SetActive(this.SelectedCharacterTab == 1);
		for (int i = 0; i < this.CharacterListButtons.Count; i++)
		{
			if (this.SelectedCharacterTab == 0)
			{
				if (i >= num)
				{
					this.CharacterListButtons[i].gameObject.SetActive(false);
				}
				else
				{
					this.CharacterListButtons[i].gameObject.SetActive(!this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters[i].EditorOnly || Application.isEditor || Debug.isDebugBuild);
				}
			}
			else
			{
				this.CharacterListButtons[i].gameObject.SetActive(i >= num && i - num < this.CreatedCharacters.Count);
			}
		}
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0003F734 File Offset: 0x0003D934
	private void SelectCharacter(int _Index)
	{
		this.SelectedCharacter = _Index;
		for (int i = 0; i < this.CharacterListButtons.Count; i++)
		{
			this.CharacterListButtons[i].Selected = (i == this.SelectedCharacter);
		}
		if (this.SelectCharacterCreationButton)
		{
			this.SelectCharacterCreationButton.Selected = (_Index == this.SelectCharacterCreationButton.Index);
		}
		PlayerCharacter selectedCharacter = this.GetSelectedCharacter();
		if (this.CreateNewCharacterButton)
		{
			this.CreateNewCharacterButton.interactable = true;
		}
		if (selectedCharacter == null)
		{
			this.CreateNewCharacterScreen.SetActive(true);
			this.StartGameScreen.SetActive(false);
			this.ScrollToCharacter(-1);
			return;
		}
		this.CreateNewCharacterScreen.SetActive(false);
		this.StartGameScreen.SetActive(true);
		this.StartGameButton.interactable = true;
		this.CharacterUnavailableInDemoText.SetActive(!this.StartGameButton.interactable);
		GraphicsManager.SetActiveGroup(this.EditAndDeleteCharacterButtons, this.StartGameButton.interactable);
		if (this.SelectedPackages == null)
		{
			this.SelectedPackages = new List<GameModifierPackage>();
		}
		else
		{
			this.SelectedPackages.Clear();
		}
		if (this.EasyPackage && selectedCharacter && selectedCharacter.EasyPackage)
		{
			this.SelectedPackages.Add(selectedCharacter.EasyPackage);
		}
		if (selectedCharacter != null)
		{
			this.CharacterName.text = selectedCharacter.CharacterName;
			this.CharacterDesc.text = selectedCharacter.CharacterDescription;
			this.CharacterDifficulty.text = this.CharacterInfos.GetRating(selectedCharacter.CharacterScore);
			this.CharacterPortrait.overrideSprite = selectedCharacter.CharacterPortrait;
			this.EasyPackageToggle.interactable = selectedCharacter.EasyPackage;
			this.EasyPackageToggle.isOn = (selectedCharacter.EasyPackage && this.SelectedPackages.Contains(selectedCharacter.EasyPackage));
		}
		if (this.SafeModeToggle)
		{
			this.SafeModeToggle.isOn = GameLoad.Instance.SaveData.SafeModeState;
		}
		this.StartGameGroup.SetActive(selectedCharacter.IsUnlocked);
		this.BuyCharacterGroup.SetActive(!selectedCharacter.IsUnlocked);
		this.CharacterSunsCost.text = selectedCharacter.SunsCost.ToString();
		this.CharacterMoonsCost.text = selectedCharacter.MoonsCost.ToString();
		this.BuyCharacterButton.interactable = this.CanAffordCharacter(selectedCharacter);
		if (this.EditCharacterButton)
		{
			this.EditCharacterButton.interactable = selectedCharacter.CustomCharacter;
		}
		if (this.DeleteCharacterButton)
		{
			this.DeleteCharacterButton.interactable = selectedCharacter.CustomCharacter;
		}
		this.RefreshCharacterCards(selectedCharacter);
		this.ScrollToCharacter(this.SelectedCharacter);
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0003FA08 File Offset: 0x0003DC08
	private void ScrollToCharacter(int _Index)
	{
		RectTransform viewport = this.CharacterListScrollRect.viewport;
		Rect rect = new Rect(viewport.transform.TransformPoint(viewport.rect.position), viewport.transform.TransformVector(viewport.rect.size));
		Transform transform = (_Index == -1) ? this.SelectCharacterCreationButton.transform : this.CharacterListButtons[_Index].transform;
		if (rect.Contains(transform.position))
		{
			this.CharacterListScrollRect.DOKill(false);
			return;
		}
		float num = viewport.rect.height / 2f;
		float verticalNormalizedPosition = Mathf.InverseLerp(this.CharacterListScrollRect.content.rect.height - num, num, Mathf.Abs(this.CharacterListScrollRect.content.InverseTransformPoint(transform.position).y));
		this.CharacterListScrollRect.verticalNormalizedPosition = verticalNormalizedPosition;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0003FB18 File Offset: 0x0003DD18
	public void OnEasyPackageValueChanged(bool _Value)
	{
		PlayerCharacter selectedCharacter = this.GetSelectedCharacter();
		if (!selectedCharacter)
		{
			return;
		}
		if (!selectedCharacter.EasyPackage)
		{
			return;
		}
		this.EasyPackage = _Value;
		if (!this.EasyPackage)
		{
			if (this.SelectedPackages.Contains(selectedCharacter.EasyPackage))
			{
				this.SelectedPackages.Remove(selectedCharacter.EasyPackage);
			}
		}
		else if (!this.SelectedPackages.Contains(selectedCharacter.EasyPackage))
		{
			this.SelectedPackages.Add(selectedCharacter.EasyPackage);
		}
		this.RefreshCharacterCards(selectedCharacter);
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0003FBA4 File Offset: 0x0003DDA4
	public void OnSafeModeValueChanged(bool _Value)
	{
		GameLoad.Instance.SaveData.SafeModeState = _Value;
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0003FBB8 File Offset: 0x0003DDB8
	private void RefreshCharacterCards(PlayerCharacter _Character)
	{
		if (!_Character)
		{
			for (int i = 0; i < this.CharacterStartingCards.Count; i++)
			{
				this.CharacterStartingCards[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < this.CharacterPerksPreview.Count; j++)
			{
				this.CharacterPerksPreview[j].gameObject.SetActive(false);
			}
			return;
		}
		int num = 0;
		while (num < _Character.CharacterPerks.Count || num < this.CharacterPerksPreview.Count)
		{
			if (num >= this.CharacterPerksPreview.Count)
			{
				this.CharacterPerksPreview.Add(UnityEngine.Object.Instantiate<MenuPerkPreview>(this.PerkPreviewPrefab, this.CharacterCardsParent));
			}
			if (num >= _Character.CharacterPerks.Count)
			{
				this.CharacterPerksPreview[num].gameObject.SetActive(false);
			}
			else
			{
				this.CharacterPerksPreview[num].Setup(_Character.CharacterPerks[num]);
				this.CharacterPerksPreview[num].gameObject.SetActive(true);
			}
			num++;
		}
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0003FCDA File Offset: 0x0003DEDA
	public void BuyCharacter()
	{
		this.BuyCharacter(this.SelectedCharacter);
		this.SelectCharacter(this.SelectedCharacter);
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0003FCF4 File Offset: 0x0003DEF4
	private void BuyCharacter(int _Index)
	{
		PlayerCharacter character = this.GetCharacter(_Index);
		if (!this.CanAffordCharacter(character) || this.UnlockedCharacters.Contains(character))
		{
			return;
		}
		this.UnlockedCharacters.Add(character);
		this.SaveData.SaveData.UnlockedCharacters.Add(UniqueIDScriptable.SaveID(character));
		this.SaveData.SaveData.Moons -= character.MoonsCost;
		this.SaveData.SaveData.Suns -= character.SunsCost;
		this.SaveData.SaveMainDataToFile();
		this.CharacterListButtons[_Index].SetUnlocked(true);
		this.RefreshSunsAndMoonsCounts();
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0003FDA6 File Offset: 0x0003DFA6
	private bool CanAffordCharacter(PlayerCharacter _Char)
	{
		return _Char.SunsCost <= this.SaveData.SaveData.Suns && _Char.MoonsCost <= this.SaveData.SaveData.Moons;
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0003FDE0 File Offset: 0x0003DFE0
	public void StartGame()
	{
		if (this.EasyPackagePopup && this.EasyPackage && this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters[this.SelectedCharacter].EasyPackage && !this.SaveData.SaveData.DontShowEasyPopup && !this.EasyPackagePopup.activeInHierarchy)
		{
			this.EasyPackagePopup.SetActive(true);
			return;
		}
		GameManager.CurrentGamemode = this.AvailableGamemodes[this.SelectedGamemode];
		if (this.SelectedCharacter < this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length)
		{
			GameManager.CurrentPlayerCharacter = this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters[this.SelectedCharacter];
		}
		else
		{
			GameManager.CurrentPlayerCharacter = this.CreatedCharacters[this.SelectedCharacter - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length];
		}
		GameManager.CurrentModifierPackages = new List<GameModifierPackage>();
		GameManager.CurrentModifierPackages.AddRange(this.SelectedPackages);
		this.SaveData.StartNewGame(this.SelectedSlotIndex, GameManager.CurrentPlayerCharacter.EasyPackage && this.EasyPackage, GameLoad.Instance.SaveData.SafeModeState);
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0003FF20 File Offset: 0x0003E120
	public void HideEasyPackagePopup()
	{
		this.SaveData.SaveData.DontShowEasyPopup = true;
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0003FF34 File Offset: 0x0003E134
	private void Update()
	{
		if (this.LoadingPortraitsPopup)
		{
			this.LoadingPortraitsPopup.SetActive(this.LoadingPortraits);
		}
		if (this.ImportingPortraitPopup)
		{
			this.ImportingPortraitPopup.SetActive(this.ImportingPortrait);
		}
		if (this.DeletingPortraitPopup)
		{
			this.DeletingPortraitPopup.SetActive(this.DeletingPortrait);
		}
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x0003FF9C File Offset: 0x0003E19C
	private void SetupCharacterCreationScreen()
	{
		if (this.CurrentlyEquippedPerks == null)
		{
			this.CurrentlyEquippedPerks = new List<CharacterPerk>();
		}
		else
		{
			this.CurrentlyEquippedPerks.Clear();
		}
		if (this.SelectedCharacter - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length >= this.CreatedCharacters.Count)
		{
			this.CurrentCreatedCharacter = new PlayerCharacterSaveData();
		}
		else
		{
			this.CurrentCreatedCharacter = new PlayerCharacterSaveData(this.SaveData.SaveData.CreatedCharacters[this.SelectedCharacter - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length]);
		}
		for (int i = 0; i < this.CurrentCreatedCharacter.CharacterPerks.Count; i++)
		{
			for (int j = 0; j < this.AllCharacterPerks.Count; j++)
			{
				if (this.CurrentCreatedCharacter.CharacterPerks[i].Contains(this.AllCharacterPerks[j].UniqueID))
				{
					this.CurrentlyEquippedPerks.Add(this.AllCharacterPerks[j]);
					break;
				}
			}
		}
		this.SelectedPerk = -1;
		this.EmptyPerkInfoObject.SetActive(true);
		this.PerkInfoObject.SetActive(false);
		this.NameInputSounds.MuteSpecialSounds = true;
		this.CharacterNameField.text = this.CurrentCreatedCharacter.CharacterName;
		this.NameInputSounds.MuteSpecialSounds = false;
		this.BioInputSounds.MuteSpecialSounds = true;
		this.CharacterBioField.text = this.CurrentCreatedCharacter.CharacterBio;
		this.BioInputSounds.MuteSpecialSounds = false;
		this.SelectPortrait(this.CurrentCreatedCharacter.PortraitID);
		this.RefreshPerkLists(false);
		this.RefreshSunsAndMoonsCounts();
		base.StartCoroutine(this.ResetPerksLists());
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x00040152 File Offset: 0x0003E352
	private IEnumerator ResetPerksLists()
	{
		yield return null;
		this.EquippedPerksScrollRect.normalizedPosition = Vector2.one;
		this.AvailablePerksScrollRect.normalizedPosition = Vector2.one;
		yield break;
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00040161 File Offset: 0x0003E361
	public void SelectPerkTab(int _TabIndex)
	{
		this.SelectedPerkTab = Mathf.Clamp(_TabIndex, 0, this.AllPerkTabs.Count - 1);
		this.RefreshPerkLists(true);
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00040184 File Offset: 0x0003E384
	private void RefreshPerkLists(bool _ChangedTab)
	{
		if (this.PerkTabs.Count > 0)
		{
			for (int i = 0; i < this.PerkTabs.Count; i++)
			{
				this.PerkTabs[i].Selected = (i == this.SelectedPerkTab);
			}
		}
		CharacterPerk characterPerk = null;
		if (this.SelectedPerk >= 0 && this.SelectedPerk < this.AllCharacterPerks.Count)
		{
			characterPerk = this.AllCharacterPerks[this.SelectedPerk];
		}
		if (this.AllPerkTabs == null)
		{
			this.AllCharacterPerks.Sort(new Comparison<CharacterPerk>(this.ComparePerks));
		}
		else if (this.AllPerkTabs.Count == 0)
		{
			this.AllCharacterPerks.Sort(new Comparison<CharacterPerk>(this.ComparePerks));
		}
		else if (this.AllPerkTabs[this.SelectedPerkTab].IncludesAllPerks)
		{
			this.AllCharacterPerks.Sort(new Comparison<CharacterPerk>(this.ComparePerks));
		}
		else
		{
			this.AllCharacterPerks.Sort(new Comparison<CharacterPerk>(this.ComparePerksTab));
		}
		if (characterPerk != null)
		{
			this.SelectedPerk = this.AllCharacterPerks.IndexOf(characterPerk);
		}
		int num = 0;
		float verticalNormalizedPosition = this.AvailablePerksScrollRect.verticalNormalizedPosition;
		float verticalNormalizedPosition2 = this.EquippedPerksScrollRect.verticalNormalizedPosition;
		bool flag = false;
		int num2 = 0;
		while (num2 < this.AllPerkButtons.Count || num2 < this.AllCharacterPerks.Count)
		{
			if (num2 >= this.AllPerkButtons.Count)
			{
				this.AllPerkButtons.Add(UnityEngine.Object.Instantiate<MenuPerkButton>(this.PerkButtonPrefab, this.PerkIsEquipped(this.AllCharacterPerks[num2]) ? this.EquippedPerksParent : this.AvailablePerksParent));
				MenuPerkButton menuPerkButton = this.AllPerkButtons[num2];
				menuPerkButton.OnClicked = (Action<int>)Delegate.Combine(menuPerkButton.OnClicked, new Action<int>(this.SelectPerk));
			}
			if (num2 >= this.AllCharacterPerks.Count)
			{
				this.AllPerkButtons[num2].gameObject.SetActive(false);
			}
			else if (!this.AllCharacterPerks[num2].IsAlive())
			{
				this.AllPerkButtons[num2].gameObject.SetActive(false);
			}
			else if (this.AllCharacterPerks[num2].HiddenUntilUnlocked && !this.UnlockedPerks.Contains(this.AllCharacterPerks[num2]))
			{
				this.AllPerkButtons[num2].gameObject.SetActive(false);
			}
			else
			{
				if (this.PerkIsEquipped(this.AllCharacterPerks[num2]))
				{
					if (this.AllPerkButtons[num2].transform.parent == this.AvailablePerksParent)
					{
						this.AllPerkButtons[num2].transform.SetParent(this.EquippedPerksParent);
					}
					else
					{
						this.AllPerkButtons[num2].transform.SetAsLastSibling();
					}
					if (!this.AllPerkButtons[num2].gameObject.activeSelf)
					{
						this.AllPerkButtons[num2].gameObject.SetActive(true);
					}
					num += this.AllCharacterPerks[num2].DifficultyRating;
				}
				else
				{
					if (this.AllPerkTabs.Count > 0)
					{
						if (!this.AllPerkTabs[this.SelectedPerkTab].ContainsPerk(this.AllCharacterPerks[num2]))
						{
							this.AllPerkButtons[num2].gameObject.SetActive(false);
							goto IL_47F;
						}
						this.AllPerkButtons[num2].gameObject.SetActive(true);
					}
					if (!this.UnlockedPerks.Contains(this.AllCharacterPerks[num2]) && !flag && this.LockedPerkSeparator)
					{
						this.LockedPerkSeparator.SetAsLastSibling();
						this.LockedPerkSeparator.gameObject.SetActive(true);
						flag = true;
					}
					if (this.AllPerkButtons[num2].transform.parent == this.EquippedPerksParent)
					{
						this.AllPerkButtons[num2].transform.SetParent(this.AvailablePerksParent);
					}
					else
					{
						this.AllPerkButtons[num2].transform.SetAsLastSibling();
					}
				}
				this.AllPerkButtons[num2].Setup(num2, this.AllCharacterPerks[num2], this.UnlockedPerks.Contains(this.AllCharacterPerks[num2]));
				this.AllPerkButtons[num2].Selected = (this.SelectedPerk == num2);
			}
			IL_47F:
			num2++;
		}
		if (!flag && this.LockedPerkSeparator)
		{
			this.LockedPerkSeparator.gameObject.SetActive(false);
		}
		if (this.DifficultyRating)
		{
			this.DifficultyRating.text = num.ToString() + "\n" + this.CharacterInfos.GetRating(num);
		}
		Canvas.ForceUpdateCanvases();
		if (!_ChangedTab)
		{
			this.AvailablePerksScrollRect.verticalNormalizedPosition = verticalNormalizedPosition;
		}
		this.EquippedPerksScrollRect.verticalNormalizedPosition = verticalNormalizedPosition2;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x000406B1 File Offset: 0x0003E8B1
	private bool PerkIsEquipped(CharacterPerk _Perk)
	{
		return this.CurrentCreatedCharacter != null && this.CurrentlyEquippedPerks.Contains(_Perk);
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x000406CC File Offset: 0x0003E8CC
	private void SelectPerk(int _Index)
	{
		this.SelectedPerk = _Index;
		this.PerkIcon.overrideSprite = this.AllCharacterPerks[_Index].PerkIcon;
		this.PerkName.text = this.AllCharacterPerks[_Index].PerkName + "\n" + LocalizedString.Difficulty(this.AllCharacterPerks[_Index].DifficultyRating);
		this.PerkDesc.text = this.AllCharacterPerks[_Index].PerkDescription;
		this.PerkUnlockConditions.text = LocalizedString.UnlockConditions(this.AllCharacterPerks[_Index].PerkUnlockConditions, this.UnlockedPerks.Contains(this.AllCharacterPerks[_Index]));
		this.PerkLockedIcon.SetActive(!this.UnlockedPerks.Contains(this.AllCharacterPerks[_Index]));
		this.PerkUnlockedIcon.SetActive(this.UnlockedPerks.Contains(this.AllCharacterPerks[_Index]));
		this.EquipPerkButton.interactable = (!this.PerkIsEquipped(this.AllCharacterPerks[_Index]) && (this.UnlockedPerks.Contains(this.AllCharacterPerks[_Index]) || Application.isEditor));
		this.UnEquipPerkButton.interactable = this.PerkIsEquipped(this.AllCharacterPerks[_Index]);
		this.SunsCostsObject.SetActive(this.AllCharacterPerks[_Index].SunsCost > 0);
		this.MoonsCostsObject.SetActive(this.AllCharacterPerks[_Index].MoonsCost > 0);
		this.PerkSunsCost.text = this.AllCharacterPerks[_Index].SunsCost.ToString();
		this.PerkMoonsCost.text = this.AllCharacterPerks[_Index].MoonsCost.ToString();
		this.BuyPerkButton.interactable = (this.CanAffordPerk(_Index) && !this.UnlockedPerks.Contains(this.AllCharacterPerks[_Index]));
		this.PerkUnlockSection.SetActive(this.AllCharacterPerks[_Index].SunsCost <= 0 && this.AllCharacterPerks[_Index].MoonsCost <= 0);
		this.PerkCostsSection.SetActive(this.AllCharacterPerks[_Index].SunsCost > 0 || this.AllCharacterPerks[_Index].MoonsCost > 0);
		this.PerkBoughtObject.SetActive(this.UnlockedPerks.Contains(this.AllCharacterPerks[_Index]));
		this.BuyPerkObject.SetActive(!this.UnlockedPerks.Contains(this.AllCharacterPerks[_Index]));
		this.PerkInfoObject.SetActive(true);
		this.EmptyPerkInfoObject.SetActive(false);
		for (int i = 0; i < this.AllPerkButtons.Count; i++)
		{
			this.AllPerkButtons[i].Selected = (this.SelectedPerk == i);
		}
		this.ScrollToPerk(_Index);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x000409F8 File Offset: 0x0003EBF8
	public void EquipPerk()
	{
		this.EquipPerk(this.SelectedPerk);
		this.SelectPerk(this.SelectedPerk);
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00040A14 File Offset: 0x0003EC14
	private void EquipPerk(int _Index)
	{
		if (!this.PerkIsEquipped(this.AllCharacterPerks[_Index]))
		{
			this.CurrentCreatedCharacter.CharacterPerks.Add(UniqueIDScriptable.SaveID(this.AllCharacterPerks[_Index]));
			this.CurrentlyEquippedPerks.Add(this.AllCharacterPerks[_Index]);
		}
		for (int i = 0; i < this.AllPerkGroups.Count; i++)
		{
			if (this.AllPerkGroups[i].HasPerk(this.AllCharacterPerks[_Index]))
			{
				for (int j = 0; j < this.AllCharacterPerks.Count; j++)
				{
					if (j != _Index && this.AllPerkGroups[i].HasPerk(this.AllCharacterPerks[j]))
					{
						this.UnequipPerk(j, false);
					}
				}
			}
		}
		this.RefreshPerkLists(false);
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x00040AEC File Offset: 0x0003ECEC
	public void UnequipPerk()
	{
		this.UnequipPerk(this.SelectedPerk, true);
		this.ScrollToPerk(this.SelectedPerk);
		if (this.CurrentCreatedCharacter.CharacterPerks.Count > 0)
		{
			for (int i = this.SelectedPerk; i < this.AllCharacterPerks.Count; i++)
			{
				if (this.PerkIsEquipped(this.AllCharacterPerks[i]))
				{
					this.SelectPerk(i);
					return;
				}
			}
			for (int j = this.SelectedPerk - 1; j >= 0; j--)
			{
				if (this.PerkIsEquipped(this.AllCharacterPerks[j]))
				{
					this.SelectPerk(j);
					return;
				}
			}
			return;
		}
		this.SelectPerk(this.SelectedPerk);
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x00040B98 File Offset: 0x0003ED98
	private void UnequipPerk(int _Index, bool _Refresh)
	{
		for (int i = 0; i < this.CurrentCreatedCharacter.CharacterPerks.Count; i++)
		{
			if (this.CurrentCreatedCharacter.CharacterPerks[i].Contains(this.AllCharacterPerks[_Index].UniqueID))
			{
				this.CurrentCreatedCharacter.CharacterPerks.RemoveAt(i);
				break;
			}
		}
		if (this.CurrentlyEquippedPerks.Contains(this.AllCharacterPerks[_Index]))
		{
			this.CurrentlyEquippedPerks.Remove(this.AllCharacterPerks[_Index]);
		}
		if (_Refresh)
		{
			this.RefreshPerkLists(false);
		}
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00040C37 File Offset: 0x0003EE37
	public void BuyPerk()
	{
		this.BuyPerk(this.SelectedPerk);
		this.SelectPerk(this.SelectedPerk);
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x00040C54 File Offset: 0x0003EE54
	private void BuyPerk(int _Index)
	{
		if (!this.CanAffordPerk(_Index) || this.UnlockedPerks.Contains(this.AllCharacterPerks[_Index]))
		{
			return;
		}
		this.AllCharacterPerks[_Index].ForceComplete(false);
		this.UnlockedPerks.Add(this.AllCharacterPerks[_Index]);
		this.SaveData.SaveData.Moons -= this.AllCharacterPerks[_Index].MoonsCost;
		this.SaveData.SaveData.Suns -= this.AllCharacterPerks[_Index].SunsCost;
		this.SaveData.SaveMainDataToFile();
		this.RefreshPerkLists(false);
		this.RefreshSunsAndMoonsCounts();
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00040D18 File Offset: 0x0003EF18
	private bool CanAffordPerk(int _Index)
	{
		return this.AllCharacterPerks[_Index].SunsCost <= this.SaveData.SaveData.Suns && this.AllCharacterPerks[_Index].MoonsCost <= this.SaveData.SaveData.Moons;
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00040D70 File Offset: 0x0003EF70
	private void ScrollToPerk(int _Index)
	{
		ScrollRect scrollRect = this.PerkIsEquipped(this.AllCharacterPerks[_Index]) ? this.EquippedPerksScrollRect : this.AvailablePerksScrollRect;
		RectTransform viewport = scrollRect.viewport;
		Rect rect = new Rect(viewport.transform.TransformPoint(viewport.rect.position), viewport.transform.TransformVector(viewport.rect.size));
		if (rect.Contains(this.AllPerkButtons[_Index].transform.position))
		{
			scrollRect.DOKill(false);
			return;
		}
		float num = viewport.rect.height / 2f;
		float endValue = Mathf.InverseLerp(scrollRect.content.rect.height - num, num, Mathf.Abs(scrollRect.content.InverseTransformPoint(this.AllPerkButtons[_Index].transform.position).y));
		scrollRect.DOKill(false);
		scrollRect.DOVerticalNormalizedPos(endValue, 0.3f, false).SetEase(Ease.InOutSine);
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x00040E9C File Offset: 0x0003F09C
	private void RefreshSunsAndMoonsCounts()
	{
		if (this.SunsCount != null)
		{
			for (int i = 0; i < this.SunsCount.Length; i++)
			{
				if (this.SunsCount[i])
				{
					this.SunsCount[i].text = this.SaveData.SaveData.Suns.ToString();
				}
			}
		}
		if (this.MoonsCount != null)
		{
			for (int j = 0; j < this.MoonsCount.Length; j++)
			{
				if (this.MoonsCount[j])
				{
					this.MoonsCount[j].text = this.SaveData.SaveData.Moons.ToString();
				}
			}
		}
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x00040F41 File Offset: 0x0003F141
	public void SetCustomCharacterName(string _Value)
	{
		if (this.CurrentCreatedCharacter != null)
		{
			this.CurrentCreatedCharacter.CharacterName = _Value;
		}
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x00040F57 File Offset: 0x0003F157
	public void SetCustomCharacterBio(string _Value)
	{
		if (this.CurrentCreatedCharacter != null)
		{
			this.CurrentCreatedCharacter.CharacterBio = _Value;
		}
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x00040F70 File Offset: 0x0003F170
	public void SaveCustomCharacter()
	{
		if (string.IsNullOrEmpty(this.CurrentCreatedCharacter.CharacterName))
		{
			this.EnterNamePopup.SetActive(true);
			return;
		}
		if (this.SelectedCharacter - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length < this.SaveData.SaveData.CreatedCharacters.Count)
		{
			this.SaveData.SaveData.CreatedCharacters[this.SelectedCharacter - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length] = this.CurrentCreatedCharacter;
		}
		else
		{
			this.SaveData.SaveData.CreatedCharacters.Add(this.CurrentCreatedCharacter);
		}
		this.SaveData.SaveMainDataToFile();
		this.MenuNavigation.SetGroupActive(3);
		if (this.CustomCharacterAchievement)
		{
			this.CustomCharacterAchievement.ForceComplete(false);
		}
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00041054 File Offset: 0x0003F254
	public void DeleteCustomCharacter()
	{
		if (this.DeleteCharacterConfirm && !this.DeleteCharacterConfirm.activeInHierarchy)
		{
			this.DeleteCharacterConfirm.SetActive(true);
			if (this.DeleteCharacterName)
			{
				this.DeleteCharacterName.text = this.CreatedCharacters[this.SelectedCharacter - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length].CharacterName;
			}
			return;
		}
		this.SaveData.SaveData.CreatedCharacters.RemoveAt(this.SelectedCharacter - this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length);
		this.SaveData.SaveMainDataToFile();
		this.SelectedCharacter = Mathf.Clamp(this.SelectedCharacter, 0, this.AvailableGamemodes[this.SelectedGamemode].PlayableCharacters.Length + this.CreatedCharacters.Count - 1);
		this.MenuNavigation.SetGroupActive(3);
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0004114C File Offset: 0x0003F34C
	public void NextPortrait()
	{
		this.SelectPortrait(this.CharacterInfos.NextPortrait(this.SelectedPortrait));
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00041165 File Offset: 0x0003F365
	public void PreviousPortrait()
	{
		this.SelectPortrait(this.CharacterInfos.PrevPortrait(this.SelectedPortrait));
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0004117E File Offset: 0x0003F37E
	private void SelectPortrait(string _PortraitID)
	{
		this.SelectPortrait(Mathf.Max(0, this.CharacterInfos.GetPortraitIndex(_PortraitID)));
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00041198 File Offset: 0x0003F398
	private void SelectPortrait(int _Index)
	{
		this.SelectedPortrait = _Index;
		this.CurrentCreatedCharacter.PortraitID = this.CharacterInfos.AllPortraits[this.SelectedPortrait].PortraitID;
		this.CreatedCharacterPortrait.overrideSprite = this.CharacterInfos.AllPortraits[this.SelectedPortrait].PortraitSprite;
		this.DeletePortraitButton.SetActive(this.CharacterInfos.IsCustomPortrait(this.SelectedPortrait));
		this.ImportPortraitButton.interactable = (this.CharacterInfos.CustomPortraitCount < 10);
		if (this.ImportPortraitButtonTooltip)
		{
			this.ImportPortraitButtonTooltip.SetTooltip(LocalizedString.ImportPortrait, LocalizedString.CustomPortraitCount(this.CharacterInfos.CustomPortraitCount, 10), null, 0);
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00041264 File Offset: 0x0003F464
	public void ImportPortrait()
	{
		if (this.ImportingPortrait)
		{
			return;
		}
		this.ImportingPortrait = true;
		base.StartCoroutine(ImageRetriever.GetImage(new Action<Texture2D>(this.OnPortraitImported)));
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00041290 File Offset: 0x0003F490
	private void OnPortraitImported(Texture2D _Portrait)
	{
		if (_Portrait == null)
		{
			this.ImportingPortrait = false;
			return;
		}
		if (!string.IsNullOrEmpty(this.ImportedPortraitName))
		{
			this.ImportingPortrait = false;
			return;
		}
		if (this.CharacterInfos.AlreadyHasCustomPortrait(_Portrait.name))
		{
			this.ImportingPortrait = false;
			this.SelectPortrait(_Portrait.name);
			return;
		}
		this.ImportedPortraitName = _Portrait.name;
		this.CharacterInfos.AddCustomPortrait(_Portrait, this, new Action<bool>(this.OnPortraitImportFinished));
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0004130F File Offset: 0x0003F50F
	private void OnPortraitImportFinished(bool _Success)
	{
		if (_Success)
		{
			this.SelectPortrait(this.ImportedPortraitName);
		}
		this.ImportedPortraitName = "";
		this.ImportingPortrait = false;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x00041332 File Offset: 0x0003F532
	public void DeletePortrait()
	{
		if (this.DeletingPortrait)
		{
			return;
		}
		this.DeletingPortrait = true;
		this.CharacterInfos.DeleteCustomPortrait(this.SelectedPortrait, this, new Action<bool>(this.OnPortraitDeleted));
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00041362 File Offset: 0x0003F562
	private void OnPortraitDeleted(bool _Success)
	{
		this.DeletingPortrait = false;
		this.SelectPortrait(0);
	}

	// Token: 0x040007EA RID: 2026
	public MenuingHelper MenuNavigation;

	// Token: 0x040007EB RID: 2027
	public MenuCardPreview CardPreviewPrefab;

	// Token: 0x040007EC RID: 2028
	public GameObject SaveErrorPopup;

	// Token: 0x040007ED RID: 2029
	public GameObject DemoSaveWarningPopup;

	// Token: 0x040007EE RID: 2030
	public List<PlayerCharacter> DemoCharacters;

	// Token: 0x040007EF RID: 2031
	public GameObject[] DemoObjects;

	// Token: 0x040007F0 RID: 2032
	[SpecialHeader("Main screen", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public MenuSlotButton[] GameSlots;

	// Token: 0x040007F1 RID: 2033
	public GameObject QuitPopup;

	// Token: 0x040007F2 RID: 2034
	public GameObject NewsButton;

	// Token: 0x040007F3 RID: 2035
	public GameObject SimpleDiscordButton;

	// Token: 0x040007F4 RID: 2036
	[SpecialHeader("Select gamemode", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public Gamemode[] AvailableGamemodes;

	// Token: 0x040007F5 RID: 2037
	public TextMeshProUGUI GamemodeName;

	// Token: 0x040007F6 RID: 2038
	public TextMeshProUGUI GamemodeDesc;

	// Token: 0x040007F7 RID: 2039
	public Image GamemodeIllustration;

	// Token: 0x040007F8 RID: 2040
	public IndexButton GamemodeSelectButtonPrefab;

	// Token: 0x040007F9 RID: 2041
	public IndexButton CharacterPreviewPrefab;

	// Token: 0x040007FA RID: 2042
	public RectTransform GamemodeListParent;

	// Token: 0x040007FB RID: 2043
	public MenuCardPreview GamemodeEnvPreview;

	// Token: 0x040007FC RID: 2044
	public MenuCardPreview GamemodeWeatherPreview;

	// Token: 0x040007FD RID: 2045
	public RectTransform GamemodeCardsParent;

	// Token: 0x040007FE RID: 2046
	public RectTransform GamemodeCharactersParent;

	// Token: 0x040007FF RID: 2047
	[SpecialHeader("Select character", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public TextMeshProUGUI CharacterScreenTitle;

	// Token: 0x04000800 RID: 2048
	public ScrollRect CharacterListScrollRect;

	// Token: 0x04000801 RID: 2049
	public RectTransform CharacterListParent;

	// Token: 0x04000802 RID: 2050
	public IndexButton OfficialCharactersTab;

	// Token: 0x04000803 RID: 2051
	public IndexButton CustomCharactersTab;

	// Token: 0x04000804 RID: 2052
	public MenuCharacterButton CharacterSelectButtonPrefab;

	// Token: 0x04000805 RID: 2053
	public IndexButton SelectCharacterCreationButton;

	// Token: 0x04000806 RID: 2054
	public TextMeshProUGUI CharacterName;

	// Token: 0x04000807 RID: 2055
	public TextMeshProUGUI CharacterDifficulty;

	// Token: 0x04000808 RID: 2056
	public Image CharacterPortrait;

	// Token: 0x04000809 RID: 2057
	public TextMeshProUGUI CharacterDesc;

	// Token: 0x0400080A RID: 2058
	public RectTransform CharacterCardsParent;

	// Token: 0x0400080B RID: 2059
	public MenuPerkPreview PerkPreviewPrefab;

	// Token: 0x0400080C RID: 2060
	public Toggle EasyPackageToggle;

	// Token: 0x0400080D RID: 2061
	public GameObject EasyPackagePopup;

	// Token: 0x0400080E RID: 2062
	public GameObject[] EditAndDeleteCharacterButtons;

	// Token: 0x0400080F RID: 2063
	public Button EditCharacterButton;

	// Token: 0x04000810 RID: 2064
	public Button DeleteCharacterButton;

	// Token: 0x04000811 RID: 2065
	public GameObject CreateNewCharacterScreen;

	// Token: 0x04000812 RID: 2066
	public Button CreateNewCharacterButton;

	// Token: 0x04000813 RID: 2067
	public GameObject StartGameScreen;

	// Token: 0x04000814 RID: 2068
	public Button StartGameButton;

	// Token: 0x04000815 RID: 2069
	public GameObject DeleteCharacterConfirm;

	// Token: 0x04000816 RID: 2070
	public TextMeshProUGUI DeleteCharacterName;

	// Token: 0x04000817 RID: 2071
	public GameObject StartGameGroup;

	// Token: 0x04000818 RID: 2072
	public GameObject BuyCharacterGroup;

	// Token: 0x04000819 RID: 2073
	public TextMeshProUGUI CharacterSunsCost;

	// Token: 0x0400081A RID: 2074
	public TextMeshProUGUI CharacterMoonsCost;

	// Token: 0x0400081B RID: 2075
	public Button BuyCharacterButton;

	// Token: 0x0400081C RID: 2076
	public Toggle SafeModeToggle;

	// Token: 0x0400081D RID: 2077
	public GameObject CharacterUnavailableInDemoText;

	// Token: 0x0400081E RID: 2078
	[SpecialHeader("Character creation", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public PlayerCharacter CharacterBaseModel;

	// Token: 0x0400081F RID: 2079
	public TMP_InputField CharacterNameField;

	// Token: 0x04000820 RID: 2080
	public TMP_InputField CharacterBioField;

	// Token: 0x04000821 RID: 2081
	public TextMeshProUGUI[] SunsCount;

	// Token: 0x04000822 RID: 2082
	public TextMeshProUGUI[] MoonsCount;

	// Token: 0x04000823 RID: 2083
	public Image PerkIcon;

	// Token: 0x04000824 RID: 2084
	public GameObject PerkInfoObject;

	// Token: 0x04000825 RID: 2085
	public GameObject EmptyPerkInfoObject;

	// Token: 0x04000826 RID: 2086
	public TextMeshProUGUI PerkName;

	// Token: 0x04000827 RID: 2087
	public TextMeshProUGUI PerkDesc;

	// Token: 0x04000828 RID: 2088
	public GameObject PerkUnlockSection;

	// Token: 0x04000829 RID: 2089
	public GameObject PerkCostsSection;

	// Token: 0x0400082A RID: 2090
	public TextMeshProUGUI PerkUnlockConditions;

	// Token: 0x0400082B RID: 2091
	public GameObject SunsCostsObject;

	// Token: 0x0400082C RID: 2092
	public GameObject MoonsCostsObject;

	// Token: 0x0400082D RID: 2093
	public TextMeshProUGUI PerkSunsCost;

	// Token: 0x0400082E RID: 2094
	public TextMeshProUGUI PerkMoonsCost;

	// Token: 0x0400082F RID: 2095
	public Button BuyPerkButton;

	// Token: 0x04000830 RID: 2096
	public GameObject BuyPerkObject;

	// Token: 0x04000831 RID: 2097
	public GameObject PerkBoughtObject;

	// Token: 0x04000832 RID: 2098
	public GameObject PerkLockedIcon;

	// Token: 0x04000833 RID: 2099
	public GameObject PerkUnlockedIcon;

	// Token: 0x04000834 RID: 2100
	public Button EquipPerkButton;

	// Token: 0x04000835 RID: 2101
	public Button UnEquipPerkButton;

	// Token: 0x04000836 RID: 2102
	public TextMeshProUGUI DifficultyRating;

	// Token: 0x04000837 RID: 2103
	public Image CreatedCharacterPortrait;

	// Token: 0x04000838 RID: 2104
	public ScrollRect AvailablePerksScrollRect;

	// Token: 0x04000839 RID: 2105
	public ScrollRect EquippedPerksScrollRect;

	// Token: 0x0400083A RID: 2106
	public RectTransform AvailablePerksParent;

	// Token: 0x0400083B RID: 2107
	public RectTransform EquippedPerksParent;

	// Token: 0x0400083C RID: 2108
	public MenuPerkButton PerkButtonPrefab;

	// Token: 0x0400083D RID: 2109
	public RectTransform LockedPerkSeparator;

	// Token: 0x0400083E RID: 2110
	public IndexButton PerkTabPrefab;

	// Token: 0x0400083F RID: 2111
	public RectTransform PerkTabsParent;

	// Token: 0x04000840 RID: 2112
	public GlobalCharacterInfo CharacterInfos;

	// Token: 0x04000841 RID: 2113
	public GameObject EnterNamePopup;

	// Token: 0x04000842 RID: 2114
	public GameObject DeletePortraitButton;

	// Token: 0x04000843 RID: 2115
	public Button ImportPortraitButton;

	// Token: 0x04000844 RID: 2116
	public GameObject LoadingPortraitsPopup;

	// Token: 0x04000845 RID: 2117
	public GameObject ImportingPortraitPopup;

	// Token: 0x04000846 RID: 2118
	public GameObject DeletingPortraitPopup;

	// Token: 0x04000847 RID: 2119
	public Objective CustomCharacterAchievement;

	// Token: 0x04000848 RID: 2120
	private TooltipProvider ImportPortraitButtonTooltip;

	// Token: 0x04000849 RID: 2121
	private ButtonSounds NameInputSounds;

	// Token: 0x0400084A RID: 2122
	private ButtonSounds BioInputSounds;

	// Token: 0x0400084B RID: 2123
	private int SelectedSlotIndex;

	// Token: 0x0400084C RID: 2124
	private int SelectedGamemode;

	// Token: 0x0400084D RID: 2125
	private int SelectedCharacter;

	// Token: 0x0400084E RID: 2126
	private int SelectedPortrait;

	// Token: 0x0400084F RID: 2127
	private List<IndexButton> GamemodeListButtons = new List<IndexButton>();

	// Token: 0x04000850 RID: 2128
	private List<MenuCardPreview> GamemodeStartingCards = new List<MenuCardPreview>();

	// Token: 0x04000851 RID: 2129
	private List<IndexButton> GamemodeCharacters = new List<IndexButton>();

	// Token: 0x04000852 RID: 2130
	private List<MenuCharacterButton> CharacterListButtons = new List<MenuCharacterButton>();

	// Token: 0x04000853 RID: 2131
	private List<MenuCardPreview> CharacterStartingCards = new List<MenuCardPreview>();

	// Token: 0x04000854 RID: 2132
	private List<MenuPerkPreview> CharacterPerksPreview = new List<MenuPerkPreview>();

	// Token: 0x04000855 RID: 2133
	private GameLoad SaveData;

	// Token: 0x04000856 RID: 2134
	private bool EasyPackage;

	// Token: 0x04000857 RID: 2135
	private bool SafeMode;

	// Token: 0x04000858 RID: 2136
	private List<GameModifierPackage> SelectedPackages = new List<GameModifierPackage>();

	// Token: 0x04000859 RID: 2137
	private PlayerCharacterSaveData CurrentCreatedCharacter;

	// Token: 0x0400085A RID: 2138
	private List<PlayerCharacter> UnlockedCharacters = new List<PlayerCharacter>();

	// Token: 0x0400085B RID: 2139
	private List<PlayerCharacter> CreatedCharacters = new List<PlayerCharacter>();

	// Token: 0x0400085C RID: 2140
	private List<MenuPerkButton> AllPerkButtons = new List<MenuPerkButton>();

	// Token: 0x0400085D RID: 2141
	private List<CharacterPerk> AllCharacterPerks = new List<CharacterPerk>();

	// Token: 0x0400085E RID: 2142
	private List<CharacterPerk> UnlockedPerks = new List<CharacterPerk>();

	// Token: 0x0400085F RID: 2143
	private List<PerkGroup> AllPerkGroups = new List<PerkGroup>();

	// Token: 0x04000860 RID: 2144
	private List<PerkTabGroup> AllPerkTabs = new List<PerkTabGroup>();

	// Token: 0x04000861 RID: 2145
	private List<IndexButton> PerkTabs = new List<IndexButton>();

	// Token: 0x04000862 RID: 2146
	private List<CharacterPerk> CurrentlyEquippedPerks = new List<CharacterPerk>();

	// Token: 0x04000863 RID: 2147
	private int SelectedCharacterTab;

	// Token: 0x04000864 RID: 2148
	private int SelectedPerkTab;

	// Token: 0x04000865 RID: 2149
	private int SelectedPerk;

	// Token: 0x04000866 RID: 2150
	private string ImportedPortraitName;

	// Token: 0x04000867 RID: 2151
	private bool LoadingPortraits;

	// Token: 0x04000868 RID: 2152
	private bool ImportingPortrait;

	// Token: 0x04000869 RID: 2153
	private bool DeletingPortrait;
}
