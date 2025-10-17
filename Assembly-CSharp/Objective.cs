using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

// Token: 0x0200014B RID: 331
[CreateAssetMenu(menuName = "Survival/Objective")]
public class Objective : CompletableObject
{
	// Token: 0x06000961 RID: 2401 RVA: 0x00057C81 File Offset: 0x00055E81
	public override void Init(ObjectiveSaveData _Data)
	{
		this.NotificationSettings.Init();
		base.Init(_Data);
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x00057C95 File Offset: 0x00055E95
	[ContextMenu("Reset Journal Data")]
	public void ClearPages()
	{
		this.RelatedPages.Clear();
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00057CA2 File Offset: 0x00055EA2
	public void AddRelatedPage(ContentPage _Page)
	{
		if (!_Page)
		{
			return;
		}
		if (!this.RelatedPages.Contains(_Page))
		{
			this.RelatedPages.Add(_Page);
		}
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00057CC8 File Offset: 0x00055EC8
	public override bool IsAlive()
	{
		if (!base.IsAlive())
		{
			return false;
		}
		if (!this.JournalObjective)
		{
			return true;
		}
		if (!MBSingleton<GameManager>.Instance)
		{
			return false;
		}
		if (this.RelatedPages != null && this.RelatedPages.Count > 0)
		{
			for (int i = 0; i < this.RelatedPages.Count; i++)
			{
				if (this.RelatedPages[i] && MBSingleton<GameManager>.Instance.GetJournal.ContainsPage(this.RelatedPages[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00057D58 File Offset: 0x00055F58
	protected override void OnComplete(bool _Init)
	{
		bool isEditor = Application.isEditor;
		if (!_Init && this.OnCompleteActions != null && this.OnCompleteActions.Count > 0)
		{
			MBSingleton<GameManager>.Instance.ObjectiveActionsQueue.AddRange(this.OnCompleteActions);
		}
		if (!_Init && this.NotificationSettings.Frequency != ObjectiveNotificationFrequencies.Never && !this.IsHidden())
		{
			this.PlayNotification(base.CompletionPercent - this.NotificationSettings.LastUpdatedValue);
		}
		else if (!_Init && this.NotificationSettings.Frequency != ObjectiveNotificationFrequencies.Never && this.IsHidden())
		{
			base.NotifiedWhenHidden = true;
		}
		if (this.ObjectiveLog != null && MBSingleton<GameManager>.Instance && !string.IsNullOrEmpty(this.ObjectiveLog.LogText))
		{
			MBSingleton<GameManager>.Instance.AddEndgameLog(this.ObjectiveLog);
		}
		if (!string.IsNullOrEmpty(this.SteamAchievementID))
		{
			if (GameLoad.Instance && GameLoad.Instance.SaveData != null)
			{
				if (GameLoad.Instance.SaveData.GlobalObjectives == null)
				{
					GameLoad.Instance.SaveData.GlobalObjectives = new List<string>();
				}
				if (!GameLoad.Instance.SaveData.GlobalObjectives.Contains(this.UniqueID))
				{
					GameLoad.Instance.SaveData.GlobalObjectives.Add(this.UniqueID);
				}
			}
			Debug.Log("Achievement: " + this.SteamAchievementID);
		}
		if (SteamManager.Initialized && !string.IsNullOrEmpty(this.SteamAchievementID))
		{
			bool flag = false;
			SteamUserStats.GetAchievement(this.SteamAchievementID, out flag);
			if (!flag)
			{
				SteamUserStats.SetAchievement(this.SteamAchievementID);
				SteamUserStats.StoreStats();
			}
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00057EF8 File Offset: 0x000560F8
	protected override void OnNotComplete(bool _Init)
	{
		if (this.NotificationSettings.Frequency == ObjectiveNotificationFrequencies.OnPercentThreshold)
		{
			float lastUpdatedValue = this.NotificationSettings.LastUpdatedValue;
			if (this.NotificationSettings.UpdateProgression(base.CompletionPercent))
			{
				if (!_Init && !this.IsHidden())
				{
					this.PlayNotification(base.CompletionPercent - lastUpdatedValue);
					return;
				}
				if (!_Init && this.IsHidden())
				{
					base.NotifiedWhenHidden = true;
					return;
				}
			}
			else if (base.NotifiedWhenHidden && !this.IsHidden() && !_Init)
			{
				base.NotifiedWhenHidden = false;
				this.PlayNotification(base.CompletionPercent);
			}
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00057F85 File Offset: 0x00056185
	protected override void PlayNotification(float _Progress)
	{
		MBSingleton<GraphicsManager>.Instance.PlayObjectiveComplete(this, base.CompletionPercent - this.NotificationSettings.LastUpdatedValue);
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00057FA4 File Offset: 0x000561A4
	public override bool IsHidden()
	{
		if (this.RelatedPages.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < this.RelatedPages.Count; i++)
		{
			if (this.RelatedPages[i] && MBSingleton<GameManager>.Instance.GetJournal.ContainsPage(this.RelatedPages[i]) && this.RelatedPages[i].Sections != null && this.RelatedPages[i].Sections.Length != 0)
			{
				for (int j = 0; j < this.RelatedPages[i].Sections.Length; j++)
				{
					if (this.RelatedPages[i].Sections[j].Entries != null && this.RelatedPages[i].Sections[j].Entries.Length != 0)
					{
						for (int k = 0; k < this.RelatedPages[i].Sections[j].Entries.Length; k++)
						{
							if (!(this.RelatedPages[i].Sections[j].Entries[k].RelatedObjective != this) && !this.RelatedPages[i].Sections[j].Entries[k].CanShow)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04000EE2 RID: 3810
	public LocalizedString ObjectiveDescription;

	// Token: 0x04000EE3 RID: 3811
	public EndgameLog ObjectiveLog;

	// Token: 0x04000EE4 RID: 3812
	[SpecialHeader("Actions", HeaderSizes.Normal, HeaderStyles.Framed, 0f)]
	[SerializeField]
	private List<CardAction> OnCompleteActions;

	// Token: 0x04000EE5 RID: 3813
	public ObjectiveNotificationSettings NotificationSettings;

	// Token: 0x04000EE6 RID: 3814
	[NonSerialized]
	public List<ContentPage> RelatedPages = new List<ContentPage>();

	// Token: 0x04000EE7 RID: 3815
	public bool JournalObjective;

	// Token: 0x04000EE8 RID: 3816
	public string SteamAchievementID;
}
