using System;
using TMPro;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class SaveButton : MonoBehaviour
{
	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000270 RID: 624 RVA: 0x0001832D File Offset: 0x0001652D
	// (set) Token: 0x06000271 RID: 625 RVA: 0x00018335 File Offset: 0x00016535
	public int GameIndex { get; private set; }

	// Token: 0x06000272 RID: 626 RVA: 0x00018340 File Offset: 0x00016540
	public void Setup(GameSaveData _Data, int _Index, SaveMenu _Parent, bool _Save)
	{
		base.gameObject.SetActive(true);
		this.DeleteButton.SetActive(_Data != null);
		this.DeleteConfirm.SetActive(false);
		if (_Data == null)
		{
			this.GameTitle.text = LocalizedString.SaveNewGame;
			this.GameDetails.text = "";
		}
		else
		{
			this.GameTitle.text = string.Concat(new string[]
			{
				_Save ? LocalizedString.Save : LocalizedString.Load,
				" ",
				LocalizedString.Slot,
				" ",
				(_Index + 1).ToString()
			});
			if (!string.IsNullOrEmpty(_Data.DaytimeToHour))
			{
				this.GameDetails.text = LocalizedString.DayCounter(_Data.CurrentDay) + " (" + _Data.DaytimeToHour + ")";
			}
			else
			{
				this.GameDetails.text = LocalizedString.DayCounter(_Data.CurrentDay);
			}
		}
		this.GameIndex = _Index;
		this.ParentMenu = _Parent;
		this.Save = _Save;
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00018460 File Offset: 0x00016660
	public void Click()
	{
		if (this.Save)
		{
			GameLoad.Instance.SaveGame(this.GameIndex, false);
		}
		else
		{
			GameLoad.Instance.LoadGame(this.GameIndex);
		}
		this.ParentMenu.Refresh();
		this.ParentMenu.gameObject.SetActive(false);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x000184B5 File Offset: 0x000166B5
	public void Delete()
	{
		GameLoad.Instance.DeleteGameData(this.GameIndex);
		this.ParentMenu.Refresh();
	}

	// Token: 0x0400029A RID: 666
	[SerializeField]
	private TextMeshProUGUI GameTitle;

	// Token: 0x0400029B RID: 667
	[SerializeField]
	private TextMeshProUGUI GameDetails;

	// Token: 0x0400029C RID: 668
	[SerializeField]
	private GameObject DeleteButton;

	// Token: 0x0400029D RID: 669
	[SerializeField]
	private GameObject DeleteConfirm;

	// Token: 0x0400029F RID: 671
	private bool Save;

	// Token: 0x040002A0 RID: 672
	private SaveMenu ParentMenu;
}
