using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000104 RID: 260
public abstract class CompletableObject : UniqueIDScriptable
{
	// Token: 0x170001AA RID: 426
	// (get) Token: 0x0600086E RID: 2158 RVA: 0x00052B25 File Offset: 0x00050D25
	// (set) Token: 0x0600086F RID: 2159 RVA: 0x00052B2D File Offset: 0x00050D2D
	public float TotalCompletionValue { get; private set; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000870 RID: 2160 RVA: 0x00052B36 File Offset: 0x00050D36
	// (set) Token: 0x06000871 RID: 2161 RVA: 0x00052B3E File Offset: 0x00050D3E
	public bool Complete { get; private set; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000872 RID: 2162 RVA: 0x00052B47 File Offset: 0x00050D47
	// (set) Token: 0x06000873 RID: 2163 RVA: 0x00052B4F File Offset: 0x00050D4F
	public bool NotifiedWhenHidden { get; protected set; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000874 RID: 2164 RVA: 0x00052B58 File Offset: 0x00050D58
	public bool HasBeenCheckedNew
	{
		get
		{
			if (this.CheckedNewFlag || !this.NotifyWhenNotChecked)
			{
				return true;
			}
			CompletableCheckNotifications checkingNotificationDetails = this.CheckingNotificationDetails;
			return checkingNotificationDetails > CompletableCheckNotifications.OnlyNew;
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000875 RID: 2165 RVA: 0x00052B88 File Offset: 0x00050D88
	public bool HasBeenCheckedComplete
	{
		get
		{
			if (!this.Complete || this.CheckedCompleteFlag || !this.NotifyWhenNotChecked)
			{
				return true;
			}
			CompletableCheckNotifications checkingNotificationDetails = this.CheckingNotificationDetails;
			return checkingNotificationDetails != CompletableCheckNotifications.All && checkingNotificationDetails != CompletableCheckNotifications.OnlyComplete;
		}
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00052BC0 File Offset: 0x00050DC0
	public void CheckRelatedContent()
	{
		this.CheckedNewFlag = true;
		if (this.Complete)
		{
			this.CheckedCompleteFlag = true;
		}
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00052BD8 File Offset: 0x00050DD8
	public virtual bool IsAlive()
	{
		return Application.isEditor || !this.EditorOnly;
	}

	// Token: 0x06000878 RID: 2168
	public abstract bool IsHidden();

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x00052BEC File Offset: 0x00050DEC
	public float CompletionPercent
	{
		get
		{
			if (this.TotalCompletionValue <= 0f)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.AllObjectives.Count; i++)
			{
				num += this.AllObjectives[i].GetCompletion();
			}
			return num / this.TotalCompletionValue;
		}
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x00052C44 File Offset: 0x00050E44
	public void OnGameQuit()
	{
		this.OnDisable();
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00052C4C File Offset: 0x00050E4C
	private void OnDisable()
	{
		for (int i = 0; i < this.ActionObjectives.Count; i++)
		{
			this.ActionObjectives[i].OnDisable();
		}
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00052C80 File Offset: 0x00050E80
	public virtual void Init(ObjectiveSaveData _Data)
	{
		this.AllObjectives = new List<SubObjective>();
		if (this.ActionObjectives != null)
		{
			this.AllObjectives.AddRange(this.ActionObjectives);
		}
		if (this.CardsOnBoardObjectives != null)
		{
			this.AllObjectives.AddRange(this.CardsOnBoardObjectives);
		}
		if (this.TagsOnBoardObjectives != null)
		{
			this.AllObjectives.AddRange(this.TagsOnBoardObjectives);
		}
		if (this.StatsObjectives != null)
		{
			this.AllObjectives.AddRange(this.StatsObjectives);
		}
		if (this.TimeObjectives != null)
		{
			this.AllObjectives.AddRange(this.TimeObjectives);
		}
		if (this.NestedObjectives != null)
		{
			this.AllObjectives.AddRange(this.NestedObjectives);
		}
		this.TotalCompletionValue = 0f;
		if (_Data != null)
		{
			this.Complete = _Data.Complete;
			this.CheckedNewFlag = _Data.HasBeenCheckedNew;
			this.CheckedCompleteFlag = _Data.HasBeenCheckedComplete;
			this.NotifiedWhenHidden = _Data.NotifiedWhenHidden;
		}
		else
		{
			this.CheckedNewFlag = false;
			this.CheckedCompleteFlag = false;
			this.NotifiedWhenHidden = false;
			this.Complete = false;
		}
		for (int i = 0; i < this.AllObjectives.Count; i++)
		{
			this.TotalCompletionValue += (float)this.AllObjectives[i].CompletionWeight;
			if (string.IsNullOrEmpty(this.AllObjectives[i].ObjectiveName))
			{
				this.AllObjectives[i].Init();
			}
			else
			{
				bool flag = false;
				if (_Data != null)
				{
					for (int j = 0; j < _Data.SubObjectives.Length; j++)
					{
						if (_Data.SubObjectives[j].ObjectiveID == this.AllObjectives[i].ObjectiveName)
						{
							this.AllObjectives[i].Load(_Data.SubObjectives[j]);
							flag = true;
							break;
						}
					}
				}
				if (!flag && this.AllObjectives[i] is ActionSubObjective)
				{
					(this.AllObjectives[i] as ActionSubObjective).ResetCounter();
				}
				if (!this.AllObjectives[i].Complete || !flag)
				{
					this.AllObjectives[i].Init();
				}
			}
		}
		if (this.StartUnlocked || this.Complete)
		{
			this.ForceComplete(false);
		}
		this.CheckForCompletion(true);
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00052EC8 File Offset: 0x000510C8
	public void CheckForCompletion(bool _Init)
	{
		if (!this.IsAlive())
		{
			this.Complete = false;
			return;
		}
		if (this.NotifiedWhenHidden && !this.IsHidden() && this.Complete && !_Init)
		{
			this.NotifiedWhenHidden = false;
			this.PlayNotification(this.CompletionPercent);
		}
		if (this.Complete && !_Init)
		{
			return;
		}
		if (CompletableObject.CompletedObjectivesNames == null)
		{
			CompletableObject.CompletedObjectivesNames = new List<string>();
		}
		else
		{
			CompletableObject.CompletedObjectivesNames.Clear();
		}
		for (int i = 0; i < this.StatsObjectives.Count; i++)
		{
			this.StatsObjectives[i].CheckForCompletion();
			if (this.StatsObjectives[i].Complete && !CompletableObject.CompletedObjectivesNames.Contains(this.StatsObjectives[i].ObjectiveName))
			{
				CompletableObject.CompletedObjectivesNames.Add(this.StatsObjectives[i].ObjectiveName);
			}
		}
		for (int j = 0; j < this.TimeObjectives.Count; j++)
		{
			this.TimeObjectives[j].CheckForCompletion();
			if (this.TimeObjectives[j].Complete && !CompletableObject.CompletedObjectivesNames.Contains(this.TimeObjectives[j].ObjectiveName))
			{
				CompletableObject.CompletedObjectivesNames.Add(this.TimeObjectives[j].ObjectiveName);
			}
		}
		for (int k = 0; k < this.CardsOnBoardObjectives.Count; k++)
		{
			this.CardsOnBoardObjectives[k].CheckForCompletion();
			if (this.CardsOnBoardObjectives[k].Complete && !CompletableObject.CompletedObjectivesNames.Contains(this.CardsOnBoardObjectives[k].ObjectiveName))
			{
				CompletableObject.CompletedObjectivesNames.Add(this.CardsOnBoardObjectives[k].ObjectiveName);
			}
		}
		for (int l = 0; l < this.TagsOnBoardObjectives.Count; l++)
		{
			this.TagsOnBoardObjectives[l].CheckForCompletion();
			if (this.TagsOnBoardObjectives[l].Complete && !CompletableObject.CompletedObjectivesNames.Contains(this.TagsOnBoardObjectives[l].ObjectiveName))
			{
				CompletableObject.CompletedObjectivesNames.Add(this.TagsOnBoardObjectives[l].ObjectiveName);
			}
		}
		for (int m = 0; m < this.ActionObjectives.Count; m++)
		{
			if (this.ActionObjectives[m].Complete && !CompletableObject.CompletedObjectivesNames.Contains(this.ActionObjectives[m].ObjectiveName))
			{
				CompletableObject.CompletedObjectivesNames.Add(this.ActionObjectives[m].ObjectiveName);
			}
		}
		for (int n = 0; n < this.NestedObjectives.Count; n++)
		{
			this.NestedObjectives[n].CheckForCompletion(_Init);
			if (this.NestedObjectives[n].Complete && !CompletableObject.CompletedObjectivesNames.Contains(this.NestedObjectives[n].ObjectiveName))
			{
				CompletableObject.CompletedObjectivesNames.Add(this.NestedObjectives[n].ObjectiveName);
			}
		}
		if (this.AllObjectives != null)
		{
			for (int num = 0; num < this.AllObjectives.Count; num++)
			{
				if (!this.AllObjectives[num].Complete && CompletableObject.CompletedObjectivesNames.Contains(this.AllObjectives[num].ObjectiveName))
				{
					this.AllObjectives[num].CompleteThroughName();
				}
			}
		}
		this.Complete = false;
		if (this.HasNoConditions)
		{
			return;
		}
		if (this.ShouldCheckWinLoseCompletion && !this.CheckWinLoseCompletion)
		{
			return;
		}
		if (this.PlayedCharacter != null && this.PlayedCharacter.Count > 0)
		{
			if (!GameManager.CurrentPlayerCharacter || !MBSingleton<GameManager>.Instance)
			{
				return;
			}
			bool flag = false;
			for (int num2 = 0; num2 < this.PlayedCharacter.Count; num2++)
			{
				if (this.PlayedCharacter[num2] == GameManager.CurrentPlayerCharacter || (!this.PlayedCharacter[num2] && GameManager.CurrentPlayerCharacter.CustomCharacter))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		for (int num3 = 0; num3 < this.AllObjectives.Count; num3++)
		{
			if (!this.AllObjectives[num3].Complete)
			{
				this.OnNotComplete(_Init);
				return;
			}
		}
		bool isEditor = Application.isEditor;
		this.Complete = true;
		this.OnComplete(_Init);
		if (!_Init && this.NotifyWhenNotChecked)
		{
			this.CheckedCompleteFlag = false;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x0600087E RID: 2174 RVA: 0x0005335C File Offset: 0x0005155C
	protected bool ShouldCheckWinLoseCompletion
	{
		get
		{
			return this.CompleteOnWin || this.CompleteOnLose;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600087F RID: 2175 RVA: 0x00053370 File Offset: 0x00051570
	protected bool CheckWinLoseCompletion
	{
		get
		{
			if (this.CompleteOnWin && !EndgameMenu.IsVictory)
			{
				return false;
			}
			if (this.CompleteOnLose && !EndgameMenu.IsGameOver)
			{
				return false;
			}
			if (this.NoSafetyMode)
			{
				if (!MBSingleton<GameManager>.Instance)
				{
					return false;
				}
				if (MBSingleton<GameManager>.Instance.IsSafeMode)
				{
					return false;
				}
			}
			return this.RequiredDifficultyScore == Vector2Int.zero || (GameManager.CharacterDifficultyScore >= Mathf.Min(this.RequiredDifficultyScore.x, this.RequiredDifficultyScore.y) && GameManager.CharacterDifficultyScore <= Mathf.Max(this.RequiredDifficultyScore.x, this.RequiredDifficultyScore.y));
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000880 RID: 2176 RVA: 0x00053420 File Offset: 0x00051620
	protected bool ShouldCheckCharacterCondition
	{
		get
		{
			return this.PlayedCharacter != null && this.PlayedCharacter.Count > 0;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000881 RID: 2177 RVA: 0x0005343A File Offset: 0x0005163A
	protected bool HasNoConditions
	{
		get
		{
			return !this.ShouldCheckCharacterCondition && !this.ShouldCheckWinLoseCompletion && (this.AllObjectives == null || this.AllObjectives.Count == 0);
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00053468 File Offset: 0x00051668
	public void ForceComplete(bool _Notify)
	{
		bool isEditor = Application.isEditor;
		if (this.AllObjectives == null)
		{
			this.Complete = true;
			this.OnComplete(!_Notify);
			if (_Notify && this.NotifyWhenNotChecked)
			{
				this.CheckedCompleteFlag = false;
			}
			return;
		}
		if (this.AllObjectives.Count == 0)
		{
			this.Complete = true;
			this.OnComplete(!_Notify);
			if (_Notify && this.NotifyWhenNotChecked)
			{
				this.CheckedCompleteFlag = false;
			}
			return;
		}
		for (int i = 0; i < this.AllObjectives.Count; i++)
		{
			this.AllObjectives[i].ForceComplete();
		}
		this.CheckForCompletion(!_Notify);
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x0005350C File Offset: 0x0005170C
	public ObjectiveSaveData Save()
	{
		ObjectiveSaveData objectiveSaveData = new ObjectiveSaveData();
		objectiveSaveData.UniqueID = UniqueIDScriptable.SaveID(this);
		objectiveSaveData.SubObjectives = new SubObjectiveSaveData[this.AllObjectives.Count];
		objectiveSaveData.HasBeenCheckedNew = this.HasBeenCheckedNew;
		objectiveSaveData.HasBeenCheckedComplete = this.CheckedCompleteFlag;
		objectiveSaveData.NotifiedWhenHidden = this.NotifiedWhenHidden;
		objectiveSaveData.Complete = this.Complete;
		for (int i = 0; i < this.AllObjectives.Count; i++)
		{
			objectiveSaveData.SubObjectives[i] = this.AllObjectives[i].Save();
		}
		return objectiveSaveData;
	}

	// Token: 0x06000884 RID: 2180
	protected abstract void OnNotComplete(bool _Init);

	// Token: 0x06000885 RID: 2181
	protected abstract void OnComplete(bool _Init);

	// Token: 0x06000886 RID: 2182
	protected abstract void PlayNotification(float _Progress);

	// Token: 0x04000CCD RID: 3277
	public const bool LogCompletions = false;

	// Token: 0x04000CCE RID: 3278
	[SpecialHeader("Conditions", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	[SerializeField]
	public bool StartUnlocked;

	// Token: 0x04000CCF RID: 3279
	[SerializeField]
	protected List<ActionSubObjective> ActionObjectives;

	// Token: 0x04000CD0 RID: 3280
	[SerializeField]
	protected List<CardOnBoardSubObjective> CardsOnBoardObjectives;

	// Token: 0x04000CD1 RID: 3281
	[SerializeField]
	protected List<TagOnBoardSubObjective> TagsOnBoardObjectives;

	// Token: 0x04000CD2 RID: 3282
	[SerializeField]
	protected List<StatSubObjective> StatsObjectives;

	// Token: 0x04000CD3 RID: 3283
	[SerializeField]
	protected List<TimeObjective> TimeObjectives;

	// Token: 0x04000CD4 RID: 3284
	[SerializeField]
	protected List<ObjectiveSubObjective> NestedObjectives;

	// Token: 0x04000CD5 RID: 3285
	[Tooltip("Empty element means a custom character")]
	[SerializeField]
	protected List<PlayerCharacter> PlayedCharacter;

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	protected bool CompleteOnWin;

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	protected bool CompleteOnLose;

	// Token: 0x04000CD8 RID: 3288
	[SerializeField]
	protected bool NoSafetyMode;

	// Token: 0x04000CD9 RID: 3289
	[SerializeField]
	[MinMax(DisplayOption = MinMaxDisplay.ProperRange)]
	protected Vector2Int RequiredDifficultyScore;

	// Token: 0x04000CDA RID: 3290
	[SerializeField]
	protected bool EditorOnly;

	// Token: 0x04000CDB RID: 3291
	public bool NotifyWhenNotChecked;

	// Token: 0x04000CDC RID: 3292
	public CompletableCheckNotifications CheckingNotificationDetails;

	// Token: 0x04000CDD RID: 3293
	protected List<SubObjective> AllObjectives;

	// Token: 0x04000CDE RID: 3294
	private static List<string> CompletedObjectivesNames = new List<string>();

	// Token: 0x04000CE2 RID: 3298
	protected bool CheckedNewFlag;

	// Token: 0x04000CE3 RID: 3299
	protected bool CheckedCompleteFlag;
}
