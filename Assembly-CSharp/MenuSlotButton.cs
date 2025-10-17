using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000093 RID: 147
public class MenuSlotButton : MonoBehaviour
{
	// Token: 0x0600063B RID: 1595 RVA: 0x000419B0 File Offset: 0x0003FBB0
	public void Setup(GameSaveData _Data, int _Index, MainMenu _Menu)
	{
		bool flag = _Data == null;
		if (!flag)
		{
			flag = !_Data.HasCardsData;
		}
		this.GameIndex = _Index;
		this.ParentMenu = _Menu;
		this.DeleteConfirm.SetActive(false);
		if (this.ButtonObject)
		{
			this.ButtonObject.interactable = true;
		}
		if (this.LoadedCharacter && this.LoadedCharacter.CustomCharacter)
		{
			UnityEngine.Object.Destroy(this.LoadedCharacter);
		}
		if (this.SlotIndex)
		{
			this.SlotIndex.text = LocalizedString.Slot + " " + (_Index + 1).ToString();
			if (_Data.SafeMode)
			{
				TextMeshProUGUI slotIndex = this.SlotIndex;
				slotIndex.text += string.Format("<size=75%> ({0})</size>", LocalizedString.SafetyMode);
			}
		}
		if (!flag)
		{
			this.NewGame.SetActive(false);
			this.LoadGame.SetActive(true);
			if (this.CharacterName || this.CharacterIcon)
			{
				this.LoadedText = LocalizedString.DefaultCharacter;
				if (!string.IsNullOrEmpty(_Data.CurrentCharacter))
				{
					this.LoadedObj = UniqueIDScriptable.GetFromID(_Data.CurrentCharacter);
					if (this.LoadedObj is PlayerCharacter)
					{
						this.LoadedCharacter = (this.LoadedObj as PlayerCharacter);
					}
				}
				if (!this.LoadedCharacter)
				{
					this.LoadedCharacter = _Menu.LoadCustomCharacter(_Data.CharacterData);
				}
				if (this.LoadedCharacter)
				{
					this.LoadedText = this.LoadedCharacter.CharacterName;
				}
				if (this.CharacterName)
				{
					this.CharacterName.text = this.LoadedText;
				}
				if (this.CharacterIcon)
				{
					this.CharacterIcon.overrideSprite = (this.LoadedCharacter ? this.LoadedCharacter.CharacterPortrait : null);
				}
			}
			if (this.TimeInfo)
			{
				if (!string.IsNullOrEmpty(_Data.DaytimeToHour))
				{
					this.TimeInfo.text = LocalizedString.DayCounter(_Data.CurrentDay) + " - " + _Data.DaytimeToHour;
				}
				else
				{
					this.TimeInfo.text = LocalizedString.DayCounter(_Data.CurrentDay);
				}
			}
			if (this.EnvironmentInfo)
			{
				this.LoadedText = "";
				this.LoadedObj = UniqueIDScriptable.GetFromID(_Data.CurrentEnvironmentCard.CardID);
				if (this.LoadedObj && this.LoadedObj is CardData)
				{
					this.LoadedCard = (this.LoadedObj as CardData);
					this.LoadedText = this.LoadedCard.CardName;
				}
				this.EnvironmentInfo.text = this.LoadedText;
				return;
			}
		}
		else
		{
			this.LoadGame.SetActive(false);
			this.NewGame.SetActive(true);
		}
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00041C9E File Offset: 0x0003FE9E
	public void OnClick()
	{
		MainMenu parentMenu = this.ParentMenu;
		if (parentMenu == null)
		{
			return;
		}
		parentMenu.ClickOnSlot(this.GameIndex);
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00041CB6 File Offset: 0x0003FEB6
	public void Delete()
	{
		MainMenu parentMenu = this.ParentMenu;
		if (parentMenu == null)
		{
			return;
		}
		parentMenu.DeleteSlot(this.GameIndex);
	}

	// Token: 0x0400087E RID: 2174
	[SerializeField]
	private TextMeshProUGUI SlotIndex;

	// Token: 0x0400087F RID: 2175
	[SerializeField]
	private GameObject NewGame;

	// Token: 0x04000880 RID: 2176
	[SerializeField]
	private GameObject LoadGame;

	// Token: 0x04000881 RID: 2177
	[SerializeField]
	private TextMeshProUGUI TimeInfo;

	// Token: 0x04000882 RID: 2178
	[SerializeField]
	private TextMeshProUGUI EnvironmentInfo;

	// Token: 0x04000883 RID: 2179
	[SerializeField]
	private Image CharacterIcon;

	// Token: 0x04000884 RID: 2180
	[SerializeField]
	private TextMeshProUGUI CharacterName;

	// Token: 0x04000885 RID: 2181
	[SerializeField]
	private GameObject DeleteConfirm;

	// Token: 0x04000886 RID: 2182
	[SerializeField]
	private Button ButtonObject;

	// Token: 0x04000887 RID: 2183
	private UniqueIDScriptable LoadedObj;

	// Token: 0x04000888 RID: 2184
	private Gamemode LoadedGamemode;

	// Token: 0x04000889 RID: 2185
	private PlayerCharacter LoadedCharacter;

	// Token: 0x0400088A RID: 2186
	private CardData LoadedCard;

	// Token: 0x0400088B RID: 2187
	private string LoadedText;

	// Token: 0x0400088C RID: 2188
	private int GameIndex = -1;

	// Token: 0x0400088D RID: 2189
	private MainMenu ParentMenu;
}
