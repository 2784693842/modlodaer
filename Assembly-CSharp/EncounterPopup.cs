using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000068 RID: 104
public class EncounterPopup : MBSingleton<EncounterPopup>
{
	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x000299A9 File Offset: 0x00027BA9
	// (set) Token: 0x06000447 RID: 1095 RVA: 0x000299B1 File Offset: 0x00027BB1
	public InGameEncounter CurrentEncounter { get; private set; }

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06000448 RID: 1096 RVA: 0x000299BA File Offset: 0x00027BBA
	// (set) Token: 0x06000449 RID: 1097 RVA: 0x000299C2 File Offset: 0x00027BC2
	public bool OngoingEncounter { get; private set; }

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600044A RID: 1098 RVA: 0x000299CC File Offset: 0x00027BCC
	private bool ActionPlaying
	{
		get
		{
			if (this.ActionsPlaying == null)
			{
				return false;
			}
			if (this.ActionsPlaying.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.ActionsPlaying.Count; i++)
			{
				if (this.ActionsPlaying[i])
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600044B RID: 1099 RVA: 0x00029A19 File Offset: 0x00027C19
	public bool LogIsUpdating
	{
		get
		{
			return this.LogQueueTimer > 0f || this.LogQueue.Count > 0;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x0600044C RID: 1100 RVA: 0x00029A38 File Offset: 0x00027C38
	private LocalizedString SeparatorInTextForm
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "IGNOREKEY",
				DefaultText = "SEPARATOR"
			};
		}
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00029A66 File Offset: 0x00027C66
	private void Awake()
	{
		this.GM = MBSingleton<GameManager>.Instance;
		this.GraphicsManager = MBSingleton<GraphicsManager>.Instance;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x00029A7E File Offset: 0x00027C7E
	public void AddToLog(EncounterLogMessage _Message)
	{
		if (string.IsNullOrEmpty(_Message))
		{
			return;
		}
		this.LogQueue.Add(_Message);
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x00029A9A File Offset: 0x00027C9A
	public void AddLogSeparator(float _Duration = 0f)
	{
		this.LogQueue.Add(new EncounterLogMessage(null, _Duration));
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00029AB0 File Offset: 0x00027CB0
	private void WriteTextToLog(LocalizedString _Text, bool _Loading, OptionalTextSettings _Settings, OptionalColorValue _Color)
	{
		if (string.IsNullOrEmpty(_Text))
		{
			return;
		}
		string text = this.WithUpperCase(_Text);
		if (!_Loading)
		{
			if (_Settings)
			{
				if (_Settings.Bold)
				{
					text = string.Format("<b>{0}</b>", text);
				}
				if (_Settings.Italics)
				{
					text = string.Format("<i>{0}</i>", text);
				}
				if (_Settings.Underlined)
				{
					text = string.Format("<u>{0}</u>", text);
				}
			}
			if (_Color)
			{
				text = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(_Color.ColorValue), text);
			}
		}
		this.LogTextHistory.Add(_Text);
		if (this.CurrentLogText == null)
		{
			this.CreateLogTextObject();
		}
		this.CurrentLogTextVisibleLength = (float)this.CurrentLogTextTargetLength;
		this.CurrentLogText.maxVisibleCharacters = this.CurrentLogTextTargetLength;
		if (this.CurrentEncounterLog == null)
		{
			this.CurrentEncounterLog = new StringBuilder();
		}
		if (this.CurrentEncounterLog.Length > 0)
		{
			this.CurrentEncounterLog.Append(string.Format("\n{0}", text));
		}
		else
		{
			this.CurrentEncounterLog.Append(text);
		}
		this.CurrentLogText.text = this.CurrentEncounterLog.ToString();
		this.GM.StartCoroutine(this.UpdateLogTextMaxCharacterLength());
		if (this.LogScroll)
		{
			this.LogScroll.DOKill(false);
			this.LogScroll.DOVerticalNormalizedPos(0f, 0.3f, false).SetEase(Ease.OutSine);
		}
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00029C24 File Offset: 0x00027E24
	private IEnumerator UpdateLogTextMaxCharacterLength()
	{
		yield return null;
		this.PrevLogTextTargetLength = this.CurrentLogTextTargetLength;
		this.CurrentLogTextTargetLength = this.CurrentLogText.textInfo.characterCount;
		yield break;
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00029C34 File Offset: 0x00027E34
	private string WithUpperCase(string _Text)
	{
		if (_Text.Length == 0)
		{
			return _Text;
		}
		if (_Text.Length == 1)
		{
			return char.ToUpper(_Text[0]).ToString();
		}
		return char.ToUpper(_Text[0]).ToString() + _Text.Substring(1);
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00029C8C File Offset: 0x00027E8C
	private void CreateLogSeparator()
	{
		this.LogTextHistory.Add(this.SeparatorInTextForm);
		this.LogSeparatorCount++;
		if (this.EncounterLogSeparators.Count < this.LogSeparatorCount)
		{
			this.EncounterLogSeparators.Add(UnityEngine.Object.Instantiate<GameObject>(this.EncounterLogSeparatorPrefab, this.EncounterLogParent));
		}
		else
		{
			this.EncounterLogSeparators[this.LogSeparatorCount - 1].SetActive(true);
		}
		this.CreateLogTextObject();
		if (this.LogScroll)
		{
			this.LogScroll.DOKill(false);
			this.LogScroll.DOVerticalNormalizedPos(0f, 0.3f, false).SetEase(Ease.OutSine);
		}
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00029D40 File Offset: 0x00027F40
	private void CreateLogTextObject()
	{
		if (this.CurrentLogText)
		{
			this.CurrentLogText.maxVisibleCharacters = 99999;
		}
		this.LogTextCount++;
		if (this.EncounterLogTexts.Count < this.LogTextCount)
		{
			this.EncounterLogTexts.Add(UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.EncounterLogTextPrefab, this.EncounterLogParent));
		}
		else
		{
			this.EncounterLogTexts[this.LogTextCount - 1].text = "";
			this.EncounterLogTexts[this.LogTextCount - 1].gameObject.SetActive(true);
		}
		if (this.CurrentEncounterLog == null)
		{
			this.CurrentEncounterLog = new StringBuilder();
		}
		else
		{
			this.CurrentEncounterLog.Clear();
		}
		this.CurrentLogText = this.EncounterLogTexts[this.LogTextCount - 1];
		this.PrevLogTextTargetLength = 0;
		this.CurrentLogTextTargetLength = 0;
		this.CurrentLogTextVisibleLength = (float)this.CurrentLogTextTargetLength;
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00029E3C File Offset: 0x0002803C
	private void ClearLog()
	{
		this.LogTextHistory.Clear();
		if (this.CurrentEncounterLog == null)
		{
			this.CurrentEncounterLog = new StringBuilder();
		}
		else
		{
			this.CurrentEncounterLog.Clear();
		}
		this.LogTextCount = 1;
		this.LogSeparatorCount = 0;
		if (this.EncounterLogTexts.Count > 1)
		{
			for (int i = 1; i < this.EncounterLogTexts.Count; i++)
			{
				this.EncounterLogTexts[i].gameObject.SetActive(false);
			}
		}
		else if (this.EncounterLogTexts.Count == 0)
		{
			this.LogTextCount = 0;
			this.CreateLogTextObject();
		}
		this.CurrentLogText = this.EncounterLogTexts[0];
		this.CurrentLogText.text = "";
		for (int j = 0; j < this.EncounterLogSeparators.Count; j++)
		{
			this.EncounterLogSeparators[j].gameObject.SetActive(false);
		}
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00029F2C File Offset: 0x0002812C
	private void Update()
	{
		if (this.LogQueue.Count > 0)
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.SkipLog = true;
			}
			if (this.LogQueueTimer <= 0f || this.SkipLog)
			{
				this.PrevLogTextTargetLength = this.CurrentLogTextTargetLength;
				if (string.IsNullOrEmpty(this.LogQueue[0].GetLogText()))
				{
					this.CreateLogSeparator();
				}
				else
				{
					this.WriteTextToLog(this.LogQueue[0].GetLogText(), false, this.LogQueue[0].TextSettings, this.LogQueue[0].TextColor);
				}
				if (this.LogQueue[0].ScreenShake && MBSingleton<AmbienceImageEffect>.Instance)
				{
					MBSingleton<AmbienceImageEffect>.Instance.ShakeScreen();
				}
				this.LogQueueTimer = this.LogQueue[0].GetDuration;
				this.CurrentLogDuration = this.LogQueue[0].GetDuration;
				this.LogQueue.RemoveAt(0);
			}
		}
		else
		{
			if ((Input.GetMouseButtonDown(0) || this.SkipLog) && this.LogQueueTimer > 0f)
			{
				this.LogQueueTimer = 0f;
				this.PrevLogTextTargetLength = this.CurrentLogTextTargetLength;
				if (this.CurrentLogText)
				{
					this.CurrentLogTextVisibleLength = (float)this.CurrentLogTextTargetLength;
				}
			}
			this.SkipLog = false;
		}
		this.LogQueueTimer -= Time.deltaTime;
		if (this.CurrentLogText)
		{
			if (this.CurrentLogDuration > 0f)
			{
				this.CurrentLogTextVisibleLength = Mathf.Lerp((float)this.PrevLogTextTargetLength, (float)this.CurrentLogTextTargetLength, (this.CurrentLogDuration - this.LogQueueTimer) / this.CurrentLogDuration);
			}
			else
			{
				this.CurrentLogTextVisibleLength = (float)this.CurrentLogTextTargetLength;
			}
			this.CurrentLogText.maxVisibleCharacters = Mathf.FloorToInt(this.CurrentLogTextVisibleLength);
		}
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0002A120 File Offset: 0x00028320
	public void StartEncounter(Encounter _Encounter, bool _Load)
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (!this.GraphicsManager)
		{
			this.GraphicsManager = MBSingleton<GraphicsManager>.Instance;
		}
		if (this.UsedAmmo == null)
		{
			this.UsedAmmo = new List<InGameCardBase>();
		}
		else
		{
			this.UsedAmmo.Clear();
		}
		Action onNewEncounter = EncounterPopup.OnNewEncounter;
		if (onNewEncounter != null)
		{
			onNewEncounter();
		}
		if (this.HelpButton)
		{
			this.HelpButton.SetActive(false);
		}
		this.ClearLog();
		if (!this.CurrentEncounter)
		{
			this.CurrentEncounter = new GameObject("CurrentEncounter").AddComponent<InGameEncounter>();
		}
		this.OngoingEncounter = true;
		this.EncounterTitle.text = _Encounter.EncounterTitle;
		this.EncounterIllustration.overrideSprite = _Encounter.EncounterImage;
		this.CurrentEncounter.Init(_Encounter, this);
		if (!_Load)
		{
			this.AddToLog(_Encounter.EncounterStartingLog);
			this.CalculateStealthChecks();
			this.CurrentEncounter.CurrentRound = 0;
		}
		else
		{
			this.CurrentEncounter.Load(this.GM.CurrentSaveData.CurrentEncounter);
			this.LoadLogs(this.GM.CurrentSaveData.CurrentEncounter);
		}
		base.gameObject.SetActive(true);
		if (!this.ApplyEncounterResult())
		{
			this.RoundStart(_Load);
		}
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0002A278 File Offset: 0x00028478
	public EncounterSaveData SaveCurrentEncounter()
	{
		if (!this.CurrentEncounter || !this.OngoingEncounter)
		{
			return null;
		}
		EncounterSaveData encounterSaveData = this.CurrentEncounter.Save();
		this.SaveLogs(encounterSaveData);
		return encounterSaveData;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0002A2B0 File Offset: 0x000284B0
	private void SaveLogs(EncounterSaveData _To)
	{
		_To.PreviousLogs = new List<LocalizedString>();
		_To.PreviousLogs.AddRange(this.LogTextHistory);
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0002A2D0 File Offset: 0x000284D0
	private void LoadLogs(EncounterSaveData _From)
	{
		if (_From.PreviousLogs == null)
		{
			return;
		}
		for (int i = 0; i < _From.PreviousLogs.Count; i++)
		{
			if (_From.PreviousLogs[i].DefaultText != "SEPARATOR")
			{
				if (i < _From.PreviousLogs.Count - 1 || this.CurrentEncounter.CurrentEnemyAction != null)
				{
					this.WriteTextToLog(_From.PreviousLogs[i], true, null, null);
				}
			}
			else
			{
				this.CreateLogSeparator();
			}
		}
		for (int j = 0; j < this.LogTextCount; j++)
		{
			this.EncounterLogTexts[j].maxVisibleCharacters = 99999;
		}
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0002A37C File Offset: 0x0002857C
	private void CalculateCover()
	{
		float num = UnityEngine.Random.Range(this.CurrentEncounter.EncounterModel.PlayerCover.x, this.CurrentEncounter.EncounterModel.PlayerCover.y);
		if (this.PlayerCoverCalculation != null)
		{
			for (int i = 0; i < this.PlayerCoverCalculation.Length; i++)
			{
				num += this.PlayerCoverCalculation[i].GenerateValue(true);
			}
		}
		float num2 = UnityEngine.Random.Range(this.CurrentEncounter.EncounterModel.EnemyCover.x, this.CurrentEncounter.EncounterModel.EnemyCover.y);
		if (this.GM && this.GM.CoverCards != null)
		{
			for (int j = 0; j < this.GM.CoverCards.Count; j++)
			{
				if (this.GM.CoverCards[j].CardModel.AppliesCoverWhenEquipped)
				{
					if (this.GraphicsManager.CharacterWindow.HasCardEquipped(this.GM.CoverCards[j]))
					{
						num += this.GM.CoverCards[j].CardModel.PlayerAddedCover;
					}
				}
				else
				{
					num += this.GM.CoverCards[j].CardModel.PlayerAddedCover;
					num2 += this.GM.CoverCards[j].CardModel.EnemyAddedCover;
				}
			}
		}
		this.CurrentEncounter.CurrentPlayerCover = num;
		this.CurrentEncounter.CurrentEnemyCover = num2;
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0002A510 File Offset: 0x00028710
	private void CalculateStealthChecks()
	{
		this.CurrentRoundStealthChecks = default(EncounterStealthReport);
		if (this.PlayerStealthCalculation != null)
		{
			for (int i = 0; i < this.PlayerStealthCalculation.Length; i++)
			{
				this.CurrentRoundStealthChecks.PlayerStatsStealth = this.CurrentRoundStealthChecks.PlayerStatsStealth + this.PlayerStealthCalculation[i].GenerateValue(true);
			}
		}
		this.CurrentRoundStealthChecks.EnemyBaseStealth = this.CurrentEncounter.CurrentEnemyStealth;
		if (this.GM && this.GM.CoverCards != null)
		{
			for (int j = 0; j < this.GM.CoverCards.Count; j++)
			{
				if (this.GM.CoverCards[j].CardModel.AppliesCoverWhenEquipped)
				{
					if (this.GraphicsManager.CharacterWindow.HasCardEquipped(this.GM.CoverCards[j]))
					{
						this.CurrentRoundStealthChecks.PlayerEquipmentStealth = this.CurrentRoundStealthChecks.PlayerEquipmentStealth + this.GM.CoverCards[j].CardModel.PlayerAddedStealth;
					}
				}
				else
				{
					this.CurrentRoundStealthChecks.PlayerCoverStealth = this.CurrentRoundStealthChecks.PlayerCoverStealth + this.GM.CoverCards[j].CardModel.PlayerAddedStealth;
					this.CurrentRoundStealthChecks.EnemyCoverStealth = this.CurrentRoundStealthChecks.EnemyCoverStealth + this.GM.CoverCards[j].CardModel.EnemyAddedStealth;
				}
			}
		}
		if (this.PlayerAlertnessCalculation != null)
		{
			for (int k = 0; k < this.PlayerAlertnessCalculation.Length; k++)
			{
				this.CurrentRoundStealthChecks.PlayerStatsAlertness = this.CurrentRoundStealthChecks.PlayerStatsAlertness + this.PlayerAlertnessCalculation[k].GenerateValue(true);
			}
		}
		this.CurrentRoundStealthChecks.EnemyBaseAlertness = this.CurrentEncounter.CurrentEnemyAlertness;
		bool flag = this.CurrentRoundStealthChecks.EnemyStealth > this.CurrentRoundStealthChecks.PlayerAlertness;
		bool flag2 = this.CurrentRoundStealthChecks.PlayerStealth > this.CurrentRoundStealthChecks.EnemyAlertness;
		Debug.LogWarning("enemyStealth: " + this.CurrentRoundStealthChecks.EnemyStealth);
		Debug.LogWarning("playerAlertness: " + this.CurrentRoundStealthChecks.PlayerAlertness);
		Debug.LogWarning("enemyStealthSuccess: " + flag.ToString());
		Debug.LogWarning("playerStealth: " + this.CurrentRoundStealthChecks.PlayerStealth);
		Debug.LogWarning("CurrentEncounter.CurrentEnemyAlertness: " + this.CurrentEncounter.CurrentEnemyAlertness);
		Debug.LogWarning("playerStealthSuccess: " + flag2.ToString());
		this.CurrentEncounter.PlayerHidden = (flag2 && !flag);
		this.CurrentEncounter.EnemyHidden = (flag && !flag2);
		Action<EncounterStealthReport> onStealthChecked = EncounterPopup.OnStealthChecked;
		if (onStealthChecked != null)
		{
			onStealthChecked(this.CurrentRoundStealthChecks);
		}
		if (this.CurrentEncounter.PlayerHidden)
		{
			this.AddToLog(EncounterLogMessage.ApplyFormatArguments(this.CurrentEncounter.EncounterModel.UsesPlural ? this.PlayerHiddenPluralLog : this.PlayerHiddenLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			return;
		}
		if (this.CurrentEncounter.EnemyHidden)
		{
			this.AddToLog(EncounterLogMessage.ApplyFormatArguments(this.CurrentEncounter.EncounterModel.UsesPlural ? this.EnemyHiddenPluralLog : this.EnemyHiddenLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
		}
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0002A88C File Offset: 0x00028A8C
	private void RoundStart(bool _Loaded)
	{
		this.ActionsPlaying.Clear();
		Action<int> onNextRound = EncounterPopup.OnNextRound;
		if (onNextRound != null)
		{
			onNextRound(this.CurrentEncounter.CurrentRound);
		}
		if (this.ContinueButtonObject.activeSelf)
		{
			this.ContinueButtonObject.SetActive(false);
		}
		this.SelectForcedPlayerAction();
		if (this.ForcedPlayerAction != null)
		{
			this.AddToLog(this.ForcedActionLog);
		}
		if (!_Loaded || this.CurrentEncounter.CurrentEnemyAction == null)
		{
			this.CurrentEncounter.CurrentEnemyAction = this.CurrentEncounter.SelectAction();
			if (!this.CurrentEncounter.PlayerHidden)
			{
				this.AddToLog(this.CurrentEncounter.CurrentEnemyAction.ActionLog);
			}
		}
		this.CalculateCover();
		this.DisplayPlayerActions();
		if (this.ForcedPlayerAction != null)
		{
			this.DoPlayerAction(this.EncounterPlayerActions.Count - 1);
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0002A964 File Offset: 0x00028B64
	private void SelectForcedPlayerAction()
	{
		this.ForcedPlayerAction = null;
		this.ForcedActionLog = default(EncounterLogMessage);
		if (this.CurrentEncounter.EnemyHidden)
		{
			this.ForcedPlayerAction = this.CurrentEncounter.EnemyHiddenFreeAction(this.CurrentRoundStealthChecks.PlayerAlertness);
			return;
		}
		if (this.StatEnforcedPlayerActions != null)
		{
			for (int i = 0; i < this.StatEnforcedPlayerActions.Length; i++)
			{
				if (this.StatEnforcedPlayerActions[i].ConditionsAreMet)
				{
					this.ForcedPlayerAction = new EncounterPlayerAction(this.StatEnforcedPlayerActions[i].Action, this.CurrentEncounter.EnemyName);
					this.ForcedActionLog = this.StatEnforcedPlayerActions[i].LogMessage;
					return;
				}
			}
		}
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0002AA1C File Offset: 0x00028C1C
	private void DisplayPlayerActions()
	{
		this.EncounterPlayerActions.Clear();
		if (this.CurrentEncounter.Distant && !this.CurrentEncounter.PlayerHidden)
		{
			this.EncounterPlayerActions.Add(new EncounterPlayerAction(this.PlayerGetCloseAction, this.CurrentEncounter.EnemyName));
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (this.GM && this.GM.WeaponCards != null)
		{
			for (int i = 0; i < this.GM.WeaponCards.Count; i++)
			{
				bool flag = false;
				if (this.GM.InGameCardIsInHand(this.GM.WeaponCards[i], false, Array.Empty<InGameCardBase>()))
				{
					for (int j = 0; j < this.EncounterPlayerActions.Count; j++)
					{
						if (this.EncounterPlayerActions[j].AssociatedCard && this.EncounterPlayerActions[j].AssociatedCard.CardModel && this.EncounterPlayerActions[j].AssociatedCard.CardModel == this.GM.WeaponCards[i].CardModel)
						{
							this.EncounterPlayerActions[j].TryReplaceCard(this.GM.WeaponCards[i]);
							flag = true;
						}
					}
					if (!flag)
					{
						if (this.GM.WeaponCards[i].CardModel.NeedsAnyAmmo && this.GM.AmmoCards != null)
						{
							bool flag2 = false;
							for (int k = 0; k < this.GM.WeaponCards[i].CardModel.AmmoNeeded.Length; k++)
							{
								for (int l = 0; l < this.GM.AmmoCards.Count; l++)
								{
									if (this.GM.WeaponCards[i].CardModel.AmmoNeeded[k].CheckCard(this.GM.AmmoCards[l]) && this.GM.InGameCardIsInHand(this.GM.AmmoCards[l], false, Array.Empty<InGameCardBase>()) && !this.UsedAmmo.Contains(this.GM.AmmoCards[l]))
									{
										EncounterPlayerAction encounterPlayerAction = this.<DisplayPlayerActions>g__findAction|134_0(this.GM.WeaponCards[i], this.GM.AmmoCards[l]);
										if (encounterPlayerAction == null)
										{
											this.EncounterPlayerActions.Add(new EncounterPlayerAction(this.GM.WeaponCards[i], this.PlayerWeaponActionSuccessLog, this.PlayerWeaponActionFailureLog, this.CurrentEncounter.EnemyName, this.GM.AmmoCards[l]));
										}
										else
										{
											encounterPlayerAction.TryReplaceAmmo(this.GM.AmmoCards[l]);
											encounterPlayerAction.CurrentAmmoCount++;
										}
										flag2 = true;
									}
								}
							}
							if (!flag2)
							{
								this.EncounterPlayerActions.Add(new EncounterPlayerAction(this.GM.WeaponCards[i], this.PlayerWeaponActionSuccessLog, this.PlayerWeaponActionFailureLog, this.CurrentEncounter.EnemyName, null));
							}
						}
						else
						{
							this.EncounterPlayerActions.Add(new EncounterPlayerAction(this.GM.WeaponCards[i], this.PlayerWeaponActionSuccessLog, this.PlayerWeaponActionFailureLog, this.CurrentEncounter.EnemyName, null));
						}
					}
				}
			}
		}
		this.AddNonWeaponActions();
		while (this.EncounterPlayerActions.Count > this.ActionButtons.Count)
		{
			this.ActionButtons.Add(UnityEngine.Object.Instantiate<EncounterOptionButton>(this.ActionButtonPrefab, this.ActionButtonsParent));
			EncounterOptionButton encounterOptionButton = this.ActionButtons[this.ActionButtons.Count - 1];
			encounterOptionButton.OnClicked = (Action<int>)Delegate.Combine(encounterOptionButton.OnClicked, new Action<int>(this.DoPlayerAction));
		}
		for (int m = 0; m < this.ActionButtons.Count; m++)
		{
			if (m >= this.EncounterPlayerActions.Count)
			{
				this.ActionButtons[m].gameObject.SetActive(false);
			}
			else
			{
				if (this.EncounterPlayerActions[m].CurrentAmmoCount > 0)
				{
					EncounterPlayerAction encounterPlayerAction2 = this.EncounterPlayerActions[m];
					encounterPlayerAction2.ActionName += string.Format(" (x{0})", this.EncounterPlayerActions[m].CurrentAmmoCount.ToString());
				}
				if (!this.EncounterPlayerActions[m].DontShowSuccessChance)
				{
					float num = this.CalculateActionClashChance(this.EncounterPlayerActions[m]);
					string arg = string.Format("{0}%", (num * 100f).ToString("0"));
					this.ActionButtons[m].Setup(m, string.Format("{0}\n({1})", this.EncounterPlayerActions[m].ActionName, arg), null, false);
				}
				else
				{
					this.ActionButtons[m].Setup(m, this.EncounterPlayerActions[m].ActionName, null, false);
				}
				if (this.ForcedPlayerAction != null)
				{
					this.ActionButtons[m].Interactable = false;
				}
				else if (this.CurrentEncounter.PlayerHidden || m == 0 || this.EncounterPlayerActions[m].RequiredDistance == EncounterDistanceCondition.AnyDistance)
				{
					this.ActionButtons[m].Interactable = !this.EncounterPlayerActions[m].NoAmmo;
				}
				else
				{
					this.ActionButtons[m].Interactable = (!this.EncounterPlayerActions[m].NoAmmo && ((this.EncounterPlayerActions[m].RequiredDistance == EncounterDistanceCondition.NeedsDistance && this.CurrentEncounter.Distant) || (this.EncounterPlayerActions[m].RequiredDistance == EncounterDistanceCondition.NeedsCloseRange && !this.CurrentEncounter.Distant)));
				}
				this.ActionButtons[m].gameObject.SetActive(true);
			}
		}
		if (this.ForcedPlayerAction != null)
		{
			this.EncounterPlayerActions.Add(this.ForcedPlayerAction);
		}
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0002B0BC File Offset: 0x000292BC
	private float CalculateActionClashChance(EncounterPlayerAction _Action)
	{
		this.CurrentRoundMeleeClashResult = default(MeleeClashResultsReport);
		this.CurrentRoundRangedClashResult = default(RangedClashResultReport);
		bool flag = this.CurrentEncounter.Distant;
		EncounterDistanceChange encounterDistanceChange = this.ChangeDistanceBeforeResolving(_Action);
		if (encounterDistanceChange == EncounterDistanceChange.AddDistance)
		{
			flag = true;
		}
		else if (encounterDistanceChange == EncounterDistanceChange.CloseDistance)
		{
			flag = false;
		}
		if (flag)
		{
			if (this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee && _Action.ActionRange == ActionRange.Melee)
			{
				this.RollForClashAtNoDistance(_Action, true);
				return Mathf.Clamp01(this.CurrentRoundMeleeClashResult.PlayerHitsRangeUpTo / Mathf.Max(0.0001f, this.CurrentRoundMeleeClashResult.TieRangeUpTo));
			}
			this.RollForClashAtDistance(_Action, true);
			return Mathf.Clamp01(this.CurrentRoundRangedClashResult.PlayerSuccessChance);
		}
		else
		{
			if (this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Ranged && _Action.ActionRange == ActionRange.Ranged)
			{
				this.RollForClashAtDistance(_Action, true);
				return Mathf.Clamp01(this.CurrentRoundRangedClashResult.PlayerSuccessChance);
			}
			this.RollForClashAtNoDistance(_Action, true);
			return Mathf.Clamp01(this.CurrentRoundMeleeClashResult.PlayerHitsRangeUpTo / Mathf.Max(0.0001f, this.CurrentRoundMeleeClashResult.TieRangeUpTo));
		}
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0002B1CC File Offset: 0x000293CC
	private void AddNonWeaponActions()
	{
		if (this.CurrentEncounter.EncounterModel.PlayerActions != null && this.CurrentEncounter.EncounterModel.PlayerActions.Length != 0)
		{
			for (int i = 0; i < this.CurrentEncounter.EncounterModel.PlayerActions.Length; i++)
			{
				this.EncounterPlayerActions.Add(new EncounterPlayerAction(this.CurrentEncounter.EncounterModel.PlayerActions[i], this.CurrentEncounter.EnemyName));
			}
		}
		EncounterPlayerAction encounterPlayerAction = new EncounterPlayerAction(this.PlayerEscapeAction, this.CurrentEncounter.EnemyName);
		if (!string.IsNullOrEmpty(this.CurrentEncounter.EncounterModel.OverrideEscapeActionName))
		{
			encounterPlayerAction.ActionName = this.CurrentEncounter.EncounterModel.OverrideEscapeActionName;
		}
		if (!string.IsNullOrEmpty(this.CurrentEncounter.EncounterModel.OverrideEscapeActionLog))
		{
			encounterPlayerAction.ActionSuccessLog = this.CurrentEncounter.EncounterModel.OverrideEscapeActionLog;
			encounterPlayerAction.ActionFailureLog = this.CurrentEncounter.EncounterModel.OverrideEscapeActionLog;
		}
		this.EncounterPlayerActions.Add(encounterPlayerAction);
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0002B2EC File Offset: 0x000294EC
	public void DoPlayerAction(int _Action)
	{
		for (int i = 0; i < this.ActionButtons.Count; i++)
		{
			this.ActionButtons[i].Interactable = false;
		}
		this.CurrentEncounter.CurrentPlayerAction = this.EncounterPlayerActions[_Action];
		if (this.CurrentEncounter.PlayerHidden && !this.EncounterPlayerActions[_Action].DoesNotAttack)
		{
			this.AddToLog(EncounterLogMessage.ApplyFormatArguments(this.PlayerStealthAttackLog, new object[]
			{
				this.CurrentEncounter.EnemyName,
				this.EncounterPlayerActions[_Action].AssociatedCard ? this.EncounterPlayerActions[_Action].AssociatedCard.CardName(true) : null
			}));
		}
		this.ResolveRound();
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0002B3B8 File Offset: 0x000295B8
	private bool PlayerWeaponIneffectiveRange(EncounterPlayerAction _ForAction, bool _Distant)
	{
		return _ForAction != null && ((!_Distant && _ForAction.ActionRange == ActionRange.Ranged) || (_Distant && _ForAction.ActionRange == ActionRange.Melee));
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0002B3DC File Offset: 0x000295DC
	private bool EnemyActionIneffectiveRange(bool _Distant)
	{
		return !(this.CurrentEncounter == null) && this.CurrentEncounter.CurrentEnemyAction != null && ((!_Distant && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Ranged) || (_Distant && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee));
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0002B438 File Offset: 0x00029638
	private EncounterDistanceChange ChangeDistanceBeforeResolving(EncounterPlayerAction _PlayerAction)
	{
		if (this.CurrentEncounter.PlayerHidden && _PlayerAction != null)
		{
			if (this.CurrentEncounter.Distant && _PlayerAction.ActionRange == ActionRange.Melee)
			{
				return EncounterDistanceChange.CloseDistance;
			}
			if (!this.CurrentEncounter.Distant && _PlayerAction.ActionRange == ActionRange.Ranged)
			{
				return EncounterDistanceChange.AddDistance;
			}
		}
		if (this.CurrentEncounter.EnemyHidden)
		{
			if (this.CurrentEncounter.Distant && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee)
			{
				return EncounterDistanceChange.CloseDistance;
			}
			if (!this.CurrentEncounter.Distant && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Ranged)
			{
				return EncounterDistanceChange.CloseDistance;
			}
		}
		if (this.CurrentEncounter.AnyDistanceChange(EncounterDistanceChange.CloseDistance))
		{
			return EncounterDistanceChange.CloseDistance;
		}
		if (this.CurrentEncounter.AnyDistanceChange(EncounterDistanceChange.AddDistance))
		{
			return EncounterDistanceChange.AddDistance;
		}
		if (_PlayerAction != null && _PlayerAction.ActionRange == ActionRange.Ranged && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Ranged)
		{
			return EncounterDistanceChange.AddDistance;
		}
		return EncounterDistanceChange.DontChangeDistance;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0002B516 File Offset: 0x00029716
	private void ChangeDistanceAfterResolving()
	{
		if (this.CurrentEncounter.CurrentPlayerAction.ActionRange == ActionRange.Melee || this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee)
		{
			this.CurrentEncounter.SetDistant(false);
		}
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0002B548 File Offset: 0x00029748
	public void ResolveRound()
	{
		EncounterDistanceChange encounterDistanceChange = this.ChangeDistanceBeforeResolving(this.CurrentEncounter.CurrentPlayerAction);
		if (encounterDistanceChange == EncounterDistanceChange.AddDistance)
		{
			this.CurrentEncounter.SetDistant(true);
		}
		else if (encounterDistanceChange == EncounterDistanceChange.CloseDistance)
		{
			this.CurrentEncounter.SetDistant(false);
		}
		this.RollForClash();
		this.CurrentEncounter.CurrentRoundEnemyWound = null;
		this.CurrentEncounter.CurrentRoundEnemyWoundLocation = BodyLocations.All;
		this.CurrentEncounter.CurrentRoundEnemyWoundSeverity = WoundSeverity.NoWound;
		EncounterLogMessage encounterLogMessage = EncounterLogMessage.Duplicate(this.CurrentEncounter.CurrentEnemyAction.SuccessLog);
		if (!encounterLogMessage.TextSettings)
		{
			encounterLogMessage.TextSettings = this.EnemySuccessTextSettings;
		}
		if (!encounterLogMessage.TextColor)
		{
			encounterLogMessage.TextColor = this.EnemySuccessTextColor;
		}
		switch (this.CurrentEncounter.CurrentRoundClashResult)
		{
		case ClashResults.PlayerHits:
			this.AddToLog(this.CurrentEncounter.CurrentEnemyAction.FailureLog);
			this.AddToLog(LocalizedString.PlayerAttackSuccessLog(false, this.CurrentEncounter.CurrentPlayerAction.ActionSuccessLog));
			this.ApplyEnemyActionChanges(EnemyActionEffectCondition.OnlyOnMiss);
			this.GenerateEnemyWound();
			this.ApplyEnemyWound();
			break;
		case ClashResults.EnemyHits:
			this.AddToLog(encounterLogMessage);
			this.AddToLog(LocalizedString.PlayerAttackFailureLog(true, this.CurrentEncounter.CurrentPlayerAction.ActionFailureLog));
			if (this.PlayerWeaponIneffectiveRange(this.CurrentEncounter.CurrentPlayerAction, this.CurrentEncounter.Distant) && !string.IsNullOrEmpty(this.CurrentEncounter.CurrentPlayerAction.WeaponIneffectiveLog))
			{
				this.AddToLog(this.CurrentEncounter.CurrentPlayerAction.WeaponIneffectiveLog);
			}
			this.ApplyEnemyActionChanges(EnemyActionEffectCondition.OnlyOnHit);
			this.GenerateAndApplyPlayerWound();
			break;
		case ClashResults.BothHit:
			this.AddToLog(encounterLogMessage);
			this.AddToLog(LocalizedString.PlayerAttackSuccessLog(true, this.CurrentEncounter.CurrentPlayerAction.ActionSuccessLog));
			this.GenerateEnemyWound();
			this.ApplyEnemyActionChanges(EnemyActionEffectCondition.OnlyOnHit);
			this.ApplyEnemyWound();
			this.GenerateAndApplyPlayerWound();
			break;
		case ClashResults.NoHit:
			this.AddToLog(this.CurrentEncounter.CurrentEnemyAction.FailureLog);
			this.AddToLog(LocalizedString.PlayerAttackFailureLog(false, this.CurrentEncounter.CurrentPlayerAction.ActionFailureLog));
			this.ApplyEnemyActionChanges(EnemyActionEffectCondition.OnlyOnMiss);
			break;
		}
		if (this.CurrentEncounter.CurrentPlayerAction != null)
		{
			if (this.CurrentEncounter.CurrentPlayerAction.AssociatedCard && this.CurrentEncounter.CurrentPlayerAction.AmmoCard)
			{
				CardAction cardAction = new CardAction();
				cardAction.ActionName = new LocalizedString
				{
					LocalizationKey = "IGNOREKEY",
					DefaultText = "Encounter_Ammo_Spending"
				};
				cardAction.ReceivingCardChanges.ModType = CardModifications.DurabilityChanges;
				cardAction.ReceivingCardChanges.UsageChange = Vector2.one * -this.CurrentEncounter.CurrentPlayerAction.AssociatedCard.CardModel.AmmoUsageCost;
				if (this.CurrentEncounter.CurrentRoundEnemyWound == null)
				{
					this.MissedAmmo.Add(this.CurrentEncounter.CurrentPlayerAction.AmmoCard);
				}
				else
				{
					this.HitAmmo.Add(this.CurrentEncounter.CurrentPlayerAction.AmmoCard);
					if (this.CurrentEncounter.CurrentRoundEnemyWound.WeaponDurabilityDamage)
					{
						CardAction cardAction2 = cardAction;
						cardAction2.ReceivingCardChanges.UsageChange = cardAction2.ReceivingCardChanges.UsageChange + Vector2.one * -this.CurrentEncounter.CurrentRoundEnemyWound.WeaponDurabilityDamage;
					}
				}
				this.UsedAmmo.Add(this.CurrentEncounter.CurrentPlayerAction.AmmoCard);
				this.GM.StartCoroutine(this.WaitForAction(cardAction, this.CurrentEncounter.CurrentPlayerAction.AmmoCard));
			}
			if (this.CurrentEncounter.CurrentPlayerAction.StatModifiers != null)
			{
				CardAction cardAction3 = new CardAction();
				cardAction3.ActionName = new LocalizedString
				{
					LocalizationKey = "IGNOREKEY",
					DefaultText = "Encounter_Player_Action"
				};
				cardAction3.StatModifications = this.CurrentEncounter.CurrentPlayerAction.StatModifiers;
				this.GM.StartCoroutine(this.WaitForAction(cardAction3, null));
			}
		}
		this.ChangeDistanceAfterResolving();
		this.ResolveEncounterResultFromEnemyState();
		if (!this.ApplyEncounterResult())
		{
			this.GM.StartCoroutine(this.WaitBeforeNextRound());
		}
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0002B984 File Offset: 0x00029B84
	private bool ApplyEncounterResult()
	{
		if (this.CurrentEncounter.EncounterResult == EncounterResult.Ongoing)
		{
			return false;
		}
		this.AddLogSeparator(0f);
		EncounterLogMessage message = default(EncounterLogMessage);
		switch (this.CurrentEncounter.EncounterResult)
		{
		case EncounterResult.EnemyDefeated:
			message = this.CurrentEncounter.EncounterModel.EnemyDefeatedEffects.GetLog(EncounterLogMessage.ApplyFormatArguments(this.DefaultEnemyDefeatedEncounterLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			break;
		case EncounterResult.EnemyEscaped:
			message = this.CurrentEncounter.EncounterModel.EnemyEscapedEffects.GetLog(EncounterLogMessage.ApplyFormatArguments(this.DefaultEnemyEscapedEncounterLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			break;
		case EncounterResult.PlayerEscaped:
			message = this.CurrentEncounter.EncounterModel.PlayerEscapedEffects.GetLog(EncounterLogMessage.ApplyFormatArguments(this.DefaultPlayerEscapedLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			break;
		case EncounterResult.Special1:
			message = this.CurrentEncounter.EncounterModel.Special1Effects.GetLog(EncounterLogMessage.ApplyFormatArguments(this.DefaultSpecialResultLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			break;
		case EncounterResult.Special2:
			message = this.CurrentEncounter.EncounterModel.Special2Effects.GetLog(EncounterLogMessage.ApplyFormatArguments(this.DefaultSpecialResultLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			break;
		case EncounterResult.Special3:
			message = this.CurrentEncounter.EncounterModel.Special3Effects.GetLog(EncounterLogMessage.ApplyFormatArguments(this.DefaultSpecialResultLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			break;
		case EncounterResult.Special4:
			message = this.CurrentEncounter.EncounterModel.Special4Effects.GetLog(EncounterLogMessage.ApplyFormatArguments(this.DefaultSpecialResultLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			break;
		case EncounterResult.PlayerDemoralized:
			message = this.CurrentEncounter.EncounterModel.PlayerDemoralizedEffects.GetLog(EncounterLogMessage.ApplyFormatArguments(this.DefaultPlayerDemoralizedLog, new object[]
			{
				this.CurrentEncounter.EnemyName
			}));
			break;
		}
		this.AddToLog(message);
		this.ShowContinueButton();
		return true;
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0002BBC0 File Offset: 0x00029DC0
	private void ShowContinueButton()
	{
		if (this.ActionButtons.Count > 0)
		{
			for (int i = 0; i < this.ActionButtons.Count; i++)
			{
				this.ActionButtons[i].gameObject.SetActive(false);
			}
		}
		this.ContinueButtonObject.SetActive(true);
		this.ContinueButton.interactable = true;
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0002BC20 File Offset: 0x00029E20
	public void PressContinue()
	{
		DismantleCardAction dismantleCardAction = new DismantleCardAction();
		EncounterResultEffect encounterResultEffect = null;
		switch (this.CurrentEncounter.EncounterResult)
		{
		case EncounterResult.Ongoing:
			return;
		case EncounterResult.EnemyDefeated:
			encounterResultEffect = this.CurrentEncounter.EncounterModel.EnemyDefeatedEffects;
			break;
		case EncounterResult.EnemyEscaped:
			encounterResultEffect = this.CurrentEncounter.EncounterModel.EnemyEscapedEffects;
			break;
		case EncounterResult.PlayerEscaped:
			encounterResultEffect = this.CurrentEncounter.EncounterModel.PlayerEscapedEffects;
			break;
		case EncounterResult.Special1:
			encounterResultEffect = this.CurrentEncounter.EncounterModel.Special1Effects;
			break;
		case EncounterResult.Special2:
			encounterResultEffect = this.CurrentEncounter.EncounterModel.Special2Effects;
			break;
		case EncounterResult.Special3:
			encounterResultEffect = this.CurrentEncounter.EncounterModel.Special3Effects;
			break;
		case EncounterResult.Special4:
			encounterResultEffect = this.CurrentEncounter.EncounterModel.Special4Effects;
			break;
		case EncounterResult.PlayerDemoralized:
			encounterResultEffect = this.CurrentEncounter.EncounterModel.PlayerDemoralizedEffects;
			break;
		}
		if (encounterResultEffect == null)
		{
			return;
		}
		List<CardData> list = new List<CardData>();
		if (encounterResultEffect.DroppedCards != null && encounterResultEffect.DroppedCards.Count > 0)
		{
			for (int i = 0; i < encounterResultEffect.DroppedCards.Count; i++)
			{
				if (encounterResultEffect.DroppedCards[i] && encounterResultEffect.DroppedCards[i].CanSpawnOnBoard())
				{
					list.Add(encounterResultEffect.DroppedCards[i]);
				}
			}
		}
		dismantleCardAction.ActionName = new LocalizedString
		{
			LocalizationKey = "IGNOREKEY",
			DefaultText = "Encounter_Finished"
		};
		dismantleCardAction.ProducedCards = new CardsDropCollection[1];
		dismantleCardAction.ProducedCards[0] = new CardsDropCollection();
		dismantleCardAction.ProducedCards[0].SetDroppedCards(list);
		List<StatModifier> list2 = new List<StatModifier>();
		list2.AddRange(encounterResultEffect.StatChanges);
		if (encounterResultEffect.TransferValuesToStats != null)
		{
			for (int j = 0; j < encounterResultEffect.TransferValuesToStats.Length; j++)
			{
				list2.Add(encounterResultEffect.TransferValuesToStats[j].ToModifier(this.CurrentEncounter));
			}
		}
		dismantleCardAction.StatModifications = list2.ToArray();
		dismantleCardAction.SetDayTimeCost(1);
		this.GM.StartCoroutine(this.WaitForAction(dismantleCardAction, null));
		List<InGameCardBase> list3 = null;
		switch (encounterResultEffect.AmmoRecovery)
		{
		case AmmoRecoveryOptions.RecoverMissed:
			list3 = this.HitAmmo;
			break;
		case AmmoRecoveryOptions.RecoverHit:
			list3 = this.MissedAmmo;
			break;
		case AmmoRecoveryOptions.RecoverNone:
			list3 = this.UsedAmmo;
			break;
		}
		if (list3 != null)
		{
			for (int k = 0; k < list3.Count; k++)
			{
				if (list3[k] && !list3[k].Destroyed)
				{
					CardAction cardAction = new CardAction();
					cardAction.ActionName = new LocalizedString
					{
						LocalizationKey = "IGNOREKEY",
						DefaultText = "Encounter_Destroy_Ammo"
					};
					cardAction.ReceivingCardChanges.ModType = CardModifications.Destroy;
					this.GM.StartCoroutine(this.WaitForAction(cardAction, list3[k]));
				}
			}
		}
		this.GM.StartCoroutine(this.WaitBeforeClosingWindow());
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0002BF34 File Offset: 0x0002A134
	private void ResolveEncounterResultFromEnemyState()
	{
		if (this.CurrentEncounter.AnyResultFromWounds(EncounterResult.EnemyDefeated))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.EnemyDefeated;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromWounds(EncounterResult.Special1))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.Special1;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromWounds(EncounterResult.Special2))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.Special2;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromWounds(EncounterResult.Special3))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.Special3;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromWounds(EncounterResult.Special4))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.Special4;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromWounds(EncounterResult.EnemyEscaped))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.EnemyEscaped;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromWounds(EncounterResult.PlayerEscaped))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.PlayerEscaped;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromWounds(EncounterResult.PlayerDemoralized))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.PlayerDemoralized;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromEnemyState(EncounterResult.EnemyDefeated))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.EnemyDefeated;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromEnemyState(EncounterResult.Special1))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.Special1;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromEnemyState(EncounterResult.Special2))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.Special2;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromEnemyState(EncounterResult.Special3))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.Special3;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromEnemyState(EncounterResult.Special4))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.Special4;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromEnemyState(EncounterResult.EnemyEscaped))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.EnemyEscaped;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromEnemyState(EncounterResult.PlayerEscaped))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.PlayerEscaped;
			return;
		}
		if (this.CurrentEncounter.AnyResultFromEnemyState(EncounterResult.PlayerDemoralized))
		{
			this.CurrentEncounter.EncounterResult = EncounterResult.PlayerDemoralized;
			return;
		}
		this.CurrentEncounter.EncounterResult = EncounterResult.Ongoing;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0002C100 File Offset: 0x0002A300
	private void RollForClash()
	{
		this.CurrentRoundMeleeClashResult = default(MeleeClashResultsReport);
		this.CurrentRoundRangedClashResult = default(RangedClashResultReport);
		if (this.CurrentEncounter.Distant)
		{
			this.RollForClashAtDistance(this.CurrentEncounter.CurrentPlayerAction, false);
			return;
		}
		this.RollForClashAtNoDistance(this.CurrentEncounter.CurrentPlayerAction, false);
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0002C158 File Offset: 0x0002A358
	private void RollForClashAtDistance(EncounterPlayerAction _ForAction, bool _Simulated)
	{
		if (_ForAction.ActionRange == ActionRange.Melee && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee)
		{
			this.RollForClashAtNoDistance(_ForAction, _Simulated);
			return;
		}
		if (_ForAction.ActionRange == ActionRange.Melee && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Ranged)
		{
			float attack = this.CalculatePlayerMeleeCombat(_ForAction, true, !_Simulated);
			float num = this.CalculateEnemyRangedCombat(true, _ForAction.IsEscapeAction, !_Simulated);
			this.CurrentRoundRangedClashResult.CommonClashReport.EnemyUsesRangedAttack = true;
			this.CalculatePlayerRangedDefense(ref this.CurrentRoundRangedClashResult, UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.ClashRangedInaccuracy.x, this.CurrentEncounter.CurrentEnemyAction.ClashRangedInaccuracy.y));
			this.CurrentRoundRangedClashResult.EnemyDefenseClashValue = num;
			bool playerHit = this.RollForHit(attack, num, true, _ForAction.CannotFailClash);
			bool enemyHit = this.RollForHit(num, this.CurrentRoundRangedClashResult.EnemyDefense, false, this.CurrentEncounter.CurrentEnemyAction.CannotFailClash);
			if (!_Simulated)
			{
				this.CalculateRoundClashResult(playerHit, enemyHit);
				Action<RangedClashResultReport> onRangedClashRoll = EncounterPopup.OnRangedClashRoll;
				if (onRangedClashRoll == null)
				{
					return;
				}
				onRangedClashRoll(this.CurrentRoundRangedClashResult);
				return;
			}
		}
		else if (_ForAction.ActionRange == ActionRange.Ranged && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee)
		{
			float num2 = this.CalculatePlayerRangedCombat(_ForAction, true, !_Simulated);
			float attack2 = this.CalculateEnemyMeleeCombat(_ForAction.IsEscapeAction, true, !_Simulated);
			this.CurrentRoundRangedClashResult.CommonClashReport.PlayerUsesRangedAttack = true;
			this.CurrentRoundRangedClashResult.PlayerDefenseClashValue = num2;
			this.CalculateEnemyRangedDefense(ref this.CurrentRoundRangedClashResult, _ForAction.GetClashInaccuracy(!_Simulated));
			bool playerHit2 = this.RollForHit(num2, this.CurrentRoundRangedClashResult.EnemyDefense, true, _ForAction.CannotFailClash);
			bool enemyHit2 = this.RollForHit(attack2, num2, false, this.CurrentEncounter.CurrentEnemyAction.CannotFailClash);
			if (!_Simulated)
			{
				this.CalculateRoundClashResult(playerHit2, enemyHit2);
				Action<RangedClashResultReport> onRangedClashRoll2 = EncounterPopup.OnRangedClashRoll;
				if (onRangedClashRoll2 == null)
				{
					return;
				}
				onRangedClashRoll2(this.CurrentRoundRangedClashResult);
				return;
			}
		}
		else if (_ForAction.ActionRange == ActionRange.Ranged && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Ranged)
		{
			float attack3 = this.CalculatePlayerRangedCombat(_ForAction, true, !_Simulated);
			float attack4 = this.CalculateEnemyRangedCombat(true, _ForAction.IsEscapeAction, !_Simulated);
			this.CurrentRoundRangedClashResult.CommonClashReport.PlayerUsesRangedAttack = true;
			this.CurrentRoundRangedClashResult.CommonClashReport.EnemyUsesRangedAttack = true;
			this.CalculatePlayerRangedDefense(ref this.CurrentRoundRangedClashResult, UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.ClashRangedInaccuracy.x, this.CurrentEncounter.CurrentEnemyAction.ClashRangedInaccuracy.y));
			this.CalculateEnemyRangedDefense(ref this.CurrentRoundRangedClashResult, _ForAction.GetClashInaccuracy(!_Simulated));
			bool playerHit3 = this.RollForHit(attack3, this.CurrentRoundRangedClashResult.EnemyDefense, true, _ForAction.CannotFailClash);
			bool enemyHit3 = this.RollForHit(attack4, this.CurrentRoundRangedClashResult.PlayerDefense, false, this.CurrentEncounter.CurrentEnemyAction.CannotFailClash);
			if (!_Simulated)
			{
				this.CalculateRoundClashResult(playerHit3, enemyHit3);
				Action<RangedClashResultReport> onRangedClashRoll3 = EncounterPopup.OnRangedClashRoll;
				if (onRangedClashRoll3 == null)
				{
					return;
				}
				onRangedClashRoll3(this.CurrentRoundRangedClashResult);
			}
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0002C468 File Offset: 0x0002A668
	private void RollForClashAtNoDistance(EncounterPlayerAction _ForAction, bool _Simulated)
	{
		if (_ForAction == null)
		{
			if (this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee)
			{
				this.RollForMeleeClash(0f, this.CalculateEnemyMeleeCombat(false, _ForAction.IsEscapeAction, !_Simulated), _Simulated, false, this.CurrentEncounter.CurrentEnemyAction.CannotFailClash);
				return;
			}
			this.RollForMeleeClash(0f, this.CalculateEnemyRangedCombat(false, _ForAction.IsEscapeAction, !_Simulated), _Simulated, false, this.CurrentEncounter.CurrentEnemyAction.CannotFailClash);
			return;
		}
		else
		{
			if (this.CurrentEncounter.CurrentEnemyAction != null)
			{
				if (_ForAction.ActionRange == ActionRange.Melee && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee)
				{
					float playerCombat = this.CalculatePlayerMeleeCombat(_ForAction, false, !_Simulated);
					float enemyCombat = this.CalculateEnemyMeleeCombat(false, _ForAction.IsEscapeAction, !_Simulated);
					this.RollForMeleeClash(playerCombat, enemyCombat, _Simulated, _ForAction.CannotFailClash, this.CurrentEncounter.CurrentEnemyAction.CannotFailClash);
				}
				if (_ForAction.ActionRange == ActionRange.Melee && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Ranged)
				{
					float playerCombat2 = this.CalculatePlayerMeleeCombat(_ForAction, false, !_Simulated);
					float enemyCombat2 = this.CalculateEnemyRangedCombat(false, _ForAction.IsEscapeAction, !_Simulated);
					this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyUsesRangedAttack = true;
					this.RollForMeleeClash(playerCombat2, enemyCombat2, _Simulated, _ForAction.CannotFailClash, this.CurrentEncounter.CurrentEnemyAction.CannotFailClash);
				}
				if (_ForAction.ActionRange == ActionRange.Ranged && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee)
				{
					float playerCombat3 = this.CalculatePlayerRangedCombat(_ForAction, false, !_Simulated);
					float enemyCombat3 = this.CalculateEnemyMeleeCombat(false, _ForAction.IsEscapeAction, !_Simulated);
					this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerUsesRangedAttack = true;
					this.RollForMeleeClash(playerCombat3, enemyCombat3, _Simulated, _ForAction.CannotFailClash, this.CurrentEncounter.CurrentEnemyAction.CannotFailClash);
				}
				if (_ForAction.ActionRange == ActionRange.Ranged && this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Ranged)
				{
					this.RollForClashAtDistance(_ForAction, _Simulated);
				}
				return;
			}
			if (_ForAction.ActionRange == ActionRange.Melee)
			{
				this.RollForMeleeClash(this.CalculatePlayerMeleeCombat(_ForAction, false, !_Simulated), 0f, _Simulated, _ForAction.CannotFailClash, false);
				return;
			}
			this.RollForMeleeClash(this.CalculatePlayerRangedCombat(_ForAction, false, !_Simulated), 0f, _Simulated, _ForAction.CannotFailClash, false);
			return;
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0002C691 File Offset: 0x0002A891
	private void CalculateRoundClashResult(bool _PlayerHit, bool _EnemyHit)
	{
		if (_PlayerHit && _EnemyHit)
		{
			this.CurrentEncounter.CurrentRoundClashResult = ClashResults.BothHit;
			return;
		}
		if (_PlayerHit)
		{
			this.CurrentEncounter.CurrentRoundClashResult = ClashResults.PlayerHits;
			return;
		}
		if (_EnemyHit)
		{
			this.CurrentEncounter.CurrentRoundClashResult = ClashResults.EnemyHits;
			return;
		}
		this.CurrentEncounter.CurrentRoundClashResult = ClashResults.NoHit;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0002C6D4 File Offset: 0x0002A8D4
	private void RollForMeleeClash(float _PlayerCombat, float _EnemyCombat, bool _Simulated, bool _PlayerCannotFail, bool _EnemyCannotFail)
	{
		float num = this.CalculateTieChance(_PlayerCombat, _EnemyCombat);
		float num2 = _PlayerCombat + _EnemyCombat;
		float num3 = UnityEngine.Random.Range(0f, num2);
		this.CurrentRoundMeleeClashResult.TiePercentChance = num;
		this.CurrentRoundMeleeClashResult.PlayerHitsRangeUpTo = _PlayerCombat - _PlayerCombat * num;
		this.CurrentRoundMeleeClashResult.EnemyHitsRangeUpTo = num2 - num2 * num;
		this.CurrentRoundMeleeClashResult.TieRangeUpTo = num2;
		this.CurrentRoundMeleeClashResult.RollValue = num3;
		if (_Simulated)
		{
			return;
		}
		if (num3 < _PlayerCombat - _PlayerCombat * num)
		{
			if (!_EnemyCannotFail)
			{
				this.CurrentEncounter.CurrentRoundClashResult = ClashResults.PlayerHits;
			}
			else
			{
				this.CurrentEncounter.CurrentRoundClashResult = ClashResults.BothHit;
			}
		}
		else if (num3 < num2 - num2 * num)
		{
			if (!_PlayerCannotFail)
			{
				this.CurrentEncounter.CurrentRoundClashResult = ClashResults.EnemyHits;
			}
			else
			{
				this.CurrentEncounter.CurrentRoundClashResult = ClashResults.BothHit;
			}
		}
		else
		{
			this.CurrentEncounter.CurrentRoundClashResult = ClashResults.BothHit;
		}
		Action<MeleeClashResultsReport> onMeleeClashRoll = EncounterPopup.OnMeleeClashRoll;
		if (onMeleeClashRoll == null)
		{
			return;
		}
		onMeleeClashRoll(this.CurrentRoundMeleeClashResult);
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0002C7B4 File Offset: 0x0002A9B4
	private bool RollForHit(float _Attack, float _Defense, bool _ForPlayer, bool _CannotFail)
	{
		float num = _CannotFail ? -1f : UnityEngine.Random.Range(0f, _Attack + _Defense);
		if (_ForPlayer)
		{
			this.CurrentRoundRangedClashResult.PlayerRandomRoll = num;
		}
		else
		{
			this.CurrentRoundRangedClashResult.EnemyRandomRoll = num;
		}
		return num < _Attack || _CannotFail;
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0002C804 File Offset: 0x0002AA04
	private float CalculateTieChance(float _PlayerMeleeCombat, float _EnemyMeleeCombat)
	{
		float num = Mathf.Max(_PlayerMeleeCombat, _EnemyMeleeCombat);
		float num2 = Mathf.Min(_PlayerMeleeCombat, _EnemyMeleeCombat);
		if (num2 <= 0f && num <= 0f)
		{
			return 1f;
		}
		if (num2 <= 0f)
		{
			return 0f;
		}
		float input = num / num2;
		float num3 = 0f;
		if (this.ClashDifferenceTieChance != null)
		{
			for (int i = 0; i < this.ClashDifferenceTieChance.Length; i++)
			{
				num3 = this.ClashDifferenceTieChance[i].GetInterpolatedValue(input);
				if (num3 > 0f)
				{
					return num3;
				}
			}
		}
		return num3;
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0002C890 File Offset: 0x0002AA90
	public WoundSeverity GenerateWoundSeverity(float _Attack, float _Defense)
	{
		if (_Attack <= 0f)
		{
			return WoundSeverity.NoWound;
		}
		if (_Defense <= 0f)
		{
			return WoundSeverity.Serious;
		}
		float num = _Attack / _Defense;
		float num2 = float.MaxValue;
		if (this.WoundSeverityMappings == null || this.WoundSeverityMappings.Length == 0)
		{
			return WoundSeverity.Serious;
		}
		for (int i = 0; i < this.WoundSeverityMappings.Length; i++)
		{
			if (num2 > this.WoundSeverityMappings[i].AttackDefenseRatio.x)
			{
				num2 = this.WoundSeverityMappings[i].AttackDefenseRatio.x;
			}
			if (num >= this.WoundSeverityMappings[i].AttackDefenseRatio.x && num <= this.WoundSeverityMappings[i].AttackDefenseRatio.y)
			{
				return this.WoundSeverityMappings[i].WoundSeverity;
			}
		}
		if (num < num2)
		{
			return WoundSeverity.NoWound;
		}
		return WoundSeverity.Serious;
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0002C968 File Offset: 0x0002AB68
	private float CalculateEnemyMeleeCombat(bool _Distant, bool _EscapeModifier, bool _WithRandomness)
	{
		this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyActionName = this.CurrentEncounter.CurrentEnemyAction.ActionLog.MainLogDefaultText;
		this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyCannotFail = this.CurrentEncounter.CurrentEnemyAction.CannotFailClash;
		this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyActionClashValue = this.CurrentEncounter.CurrentEnemyAction.BaseClashValue;
		this.CurrentRoundMeleeClashResult.CommonClashReport.EnemySizeClashValue = this.CurrentEncounter.CurrentEnemySize;
		this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyActionReachClashValue = this.CurrentEncounter.CurrentEnemyAction.Reach;
		this.CurrentEncounter.CurrentEnemyAction.AddClashValueFromEnemyValues(this.CurrentEncounter, ref this.CurrentRoundMeleeClashResult.CommonClashReport, _WithRandomness);
		this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyClashAddedValueFromWounds = this.CurrentEncounter.CurrentEnemyAction.AddedClashValueFromWounds(this.CurrentEncounter, _WithRandomness);
		this.CurrentEncounter.CurrentEnemyAction.AddClashFromStats(ref this.CurrentRoundMeleeClashResult.CommonClashReport, _WithRandomness);
		if (this.CurrentEncounter.EnemyHidden)
		{
			this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyClashStealthBonus = UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.ClashStealthBonus.x, this.CurrentEncounter.CurrentEnemyAction.ClashStealthBonus.y);
		}
		if (this.EnemyActionIneffectiveRange(_Distant))
		{
			this.CurrentRoundRangedClashResult.CommonClashReport.EnemyClashIneffectiveRangeMalus = UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.IneffectiveRangeMalus.x, this.CurrentEncounter.CurrentEnemyAction.IneffectiveRangeMalus.y);
		}
		if (_EscapeModifier)
		{
			this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyClashEscapeModifier = UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.ClashVsEscapeModifier.x, this.CurrentEncounter.CurrentEnemyAction.ClashVsEscapeModifier.y);
		}
		this.CurrentRoundRangedClashResult.CommonClashReport = this.CurrentRoundMeleeClashResult.CommonClashReport;
		return this.CurrentRoundMeleeClashResult.CommonClashReport.EnemyClashValue;
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0002CB78 File Offset: 0x0002AD78
	private float CalculateEnemyRangedCombat(bool _Distant, bool _EscapeModifier, bool _WithRandomness)
	{
		this.CurrentRoundRangedClashResult.CommonClashReport.EnemyActionName = this.CurrentEncounter.CurrentEnemyAction.ActionLog.MainLogDefaultText;
		this.CurrentRoundRangedClashResult.CommonClashReport.EnemyCannotFail = this.CurrentEncounter.CurrentEnemyAction.CannotFailClash;
		this.CurrentRoundRangedClashResult.CommonClashReport.EnemyActionClashValue = this.CurrentEncounter.CurrentEnemyAction.BaseClashValue;
		this.CurrentEncounter.CurrentEnemyAction.AddClashValueFromEnemyValues(this.CurrentEncounter, ref this.CurrentRoundRangedClashResult.CommonClashReport, _WithRandomness);
		this.CurrentRoundRangedClashResult.CommonClashReport.EnemyClashAddedValueFromWounds = this.CurrentEncounter.CurrentEnemyAction.AddedClashValueFromWounds(this.CurrentEncounter, _WithRandomness);
		this.CurrentEncounter.CurrentEnemyAction.AddClashFromStats(ref this.CurrentRoundRangedClashResult.CommonClashReport, _WithRandomness);
		if (this.CurrentEncounter.EnemyHidden)
		{
			this.CurrentRoundRangedClashResult.CommonClashReport.EnemyClashStealthBonus = UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.ClashStealthBonus.x, this.CurrentEncounter.CurrentEnemyAction.ClashStealthBonus.y);
		}
		if (this.EnemyActionIneffectiveRange(_Distant))
		{
			this.CurrentRoundRangedClashResult.CommonClashReport.EnemyClashIneffectiveRangeMalus = UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.IneffectiveRangeMalus.x, this.CurrentEncounter.CurrentEnemyAction.IneffectiveRangeMalus.y);
		}
		if (_EscapeModifier)
		{
			this.CurrentRoundRangedClashResult.CommonClashReport.EnemyClashEscapeModifier = UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.ClashVsEscapeModifier.x, this.CurrentEncounter.CurrentEnemyAction.ClashVsEscapeModifier.y);
		}
		this.CurrentRoundMeleeClashResult.CommonClashReport = this.CurrentRoundRangedClashResult.CommonClashReport;
		return this.CurrentRoundRangedClashResult.EnemyClashValue;
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0002CD46 File Offset: 0x0002AF46
	private void CalculateEnemyRangedDefense(ref RangedClashResultReport _Report, float _Inaccuracy)
	{
		_Report.EnemyDefenseSize = -this.CurrentEncounter.CurrentEnemySize;
		_Report.EnemyDefenseCover = this.CurrentEncounter.CurrentEnemyCover;
		_Report.EnemyDefenseInaccuracy = _Inaccuracy;
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0002CD74 File Offset: 0x0002AF74
	private float CalculatePlayerMeleeCombat(EncounterPlayerAction _ForAction, bool _Distant, bool _WithRandomness)
	{
		this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerActionName = _ForAction.ActionName;
		this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerCannotFail = _ForAction.CannotFailClash;
		this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerActionClashValue = _ForAction.GetClash(_WithRandomness);
		this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerSizeClashValue = this.PlayerSize;
		this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerActionReachClashValue = _ForAction.Reach;
		this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerClashStatsAddedValues = _ForAction.GetClashStatsAddedValues(_WithRandomness);
		if (this.CurrentEncounter.PlayerHidden)
		{
			this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerClashStealthBonus = _ForAction.GetClashStealthBonus(_WithRandomness);
		}
		if (this.PlayerWeaponIneffectiveRange(_ForAction, _Distant))
		{
			this.CurrentRoundRangedClashResult.CommonClashReport.PlayerClashIneffectiveRangeMalus = _ForAction.GetClashIneffectiveRangeMalus(_WithRandomness);
		}
		if (this.CurrentEncounter.CurrentEnemyAction != null && this.CurrentEncounter.CurrentEnemyAction.IsEscapeAction)
		{
			this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerClashEscapeModifier = _ForAction.GetClashEscapeModifier(_WithRandomness);
		}
		this.CurrentRoundRangedClashResult.CommonClashReport = this.CurrentRoundMeleeClashResult.CommonClashReport;
		return this.CurrentRoundMeleeClashResult.CommonClashReport.PlayerClashValue;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0002CEA8 File Offset: 0x0002B0A8
	private float CalculatePlayerRangedCombat(EncounterPlayerAction _ForAction, bool _Distant, bool _WithRandomness)
	{
		this.CurrentRoundRangedClashResult.CommonClashReport.PlayerActionName = _ForAction.ActionName;
		this.CurrentRoundRangedClashResult.CommonClashReport.PlayerCannotFail = _ForAction.CannotFailClash;
		this.CurrentRoundRangedClashResult.CommonClashReport.PlayerActionClashValue = _ForAction.GetClash(_WithRandomness);
		this.CurrentRoundRangedClashResult.CommonClashReport.PlayerClashStatsAddedValues = _ForAction.GetClashStatsAddedValues(_WithRandomness);
		if (this.CurrentEncounter.PlayerHidden)
		{
			this.CurrentRoundRangedClashResult.CommonClashReport.PlayerClashStealthBonus = _ForAction.GetClashStealthBonus(_WithRandomness);
		}
		if (this.PlayerWeaponIneffectiveRange(_ForAction, _Distant))
		{
			this.CurrentRoundRangedClashResult.CommonClashReport.PlayerClashIneffectiveRangeMalus = _ForAction.GetClashIneffectiveRangeMalus(_WithRandomness);
		}
		if (this.CurrentEncounter.CurrentEnemyAction != null && this.CurrentEncounter.CurrentEnemyAction.IsEscapeAction)
		{
			this.CurrentRoundRangedClashResult.CommonClashReport.PlayerClashEscapeModifier = _ForAction.GetClashEscapeModifier(_WithRandomness);
		}
		this.CurrentRoundMeleeClashResult.CommonClashReport = this.CurrentRoundRangedClashResult.CommonClashReport;
		return this.CurrentRoundRangedClashResult.PlayerClashValue;
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0002CFAB File Offset: 0x0002B1AB
	private void CalculatePlayerRangedDefense(ref RangedClashResultReport _Report, float _Inaccuracy)
	{
		_Report.PlayerDefenseSize = -this.PlayerSize;
		_Report.PlayerDefenseCover = this.CurrentEncounter.CurrentPlayerCover;
		_Report.PlayerDefenseInaccuracy = _Inaccuracy;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0002CFD4 File Offset: 0x0002B1D4
	private void ApplyEnemyActionChanges(EnemyActionEffectCondition _HitCondition)
	{
		this.CurrentEncounter.ModifyEnemyValues(this.CurrentEncounter.CurrentEnemyAction.EnemyValueChanges, _HitCondition);
		if (this.CurrentEncounter.CurrentEnemyAction.TransferValuesToStats != null)
		{
			CardAction cardAction = new CardAction();
			List<StatModifier> list = new List<StatModifier>();
			cardAction.ActionName = new LocalizedString
			{
				LocalizationKey = "IGNOREKEY",
				DefaultText = "Encounter_Enemy_Action"
			};
			for (int i = 0; i < this.CurrentEncounter.CurrentEnemyAction.TransferValuesToStats.Length; i++)
			{
				list.Add(this.CurrentEncounter.CurrentEnemyAction.TransferValuesToStats[i].ToModifier(this.CurrentEncounter));
			}
			cardAction.StatModifications = list.ToArray();
			this.GM.StartCoroutine(this.WaitForAction(cardAction, null));
		}
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0002D0AC File Offset: 0x0002B2AC
	private void GenerateAndApplyPlayerWound()
	{
		this.PlayerBodyLocationHit = default(PlayerBodyLocationSelectionReport);
		this.CurrentRoundEnemyDamageReport = default(EncounterEnemyDamageReport);
		if (this.CurrentEncounter.CurrentEnemyAction.DoesNotAttack)
		{
			this.CurrentEncounter.CurrentRoundPlayerWound = null;
			return;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		if (this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee)
		{
			this.PlayerBodyLocationHit.Ranged = false;
			this.PlayerBodyLocationHit.BaseWeights.Head = this.Head.MeleeHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.Torso = this.Torso.MeleeHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.LArm = this.LArm.MeleeHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.RArm = this.RArm.MeleeHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.LLeg = this.LLeg.MeleeHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.RLeg = this.RLeg.MeleeHitChanceWeight;
		}
		else
		{
			this.PlayerBodyLocationHit.BaseWeights.Head = this.Head.RangedHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.Torso = this.Torso.RangedHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.LArm = this.LArm.RangedHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.RArm = this.RArm.RangedHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.LLeg = this.LLeg.RangedHitChanceWeight;
			this.PlayerBodyLocationHit.BaseWeights.RLeg = this.RLeg.RangedHitChanceWeight;
			if (this.CurrentEncounter.Distant && this.GM && this.GM.CoverCards != null)
			{
				for (int i = 0; i < this.GM.CoverCards.Count; i++)
				{
					if (!this.GM.CoverCards[i].CardModel.AppliesCoverWhenEquipped || (this.GM.CoverCards[i].CardModel.AppliesCoverWhenEquipped && this.GraphicsManager.CharacterWindow.HasCardEquipped(this.GM.CoverCards[i])))
					{
						this.PlayerBodyLocationHit.CoverWeights.Head = this.GM.CoverCards[i].CardModel.PlayerCoverHitProbabilityModifiers.HeadHitProbabilityModifier;
						this.PlayerBodyLocationHit.CoverWeights.Torso = this.GM.CoverCards[i].CardModel.PlayerCoverHitProbabilityModifiers.TorsoHitProbabilityModifier;
						this.PlayerBodyLocationHit.CoverWeights.LArm = this.GM.CoverCards[i].CardModel.PlayerCoverHitProbabilityModifiers.LArmHitProbabilityModifier;
						this.PlayerBodyLocationHit.CoverWeights.RArm = this.GM.CoverCards[i].CardModel.PlayerCoverHitProbabilityModifiers.RArmHitProbabilityModifier;
						this.PlayerBodyLocationHit.CoverWeights.LLeg = this.GM.CoverCards[i].CardModel.PlayerCoverHitProbabilityModifiers.LLegHitProbabilityModifier;
						this.PlayerBodyLocationHit.CoverWeights.RLeg = this.GM.CoverCards[i].CardModel.PlayerCoverHitProbabilityModifiers.RLegHitProbabilityModifier;
					}
				}
			}
		}
		if (this.GM && this.GM.ArmorCards != null)
		{
			for (int j = 0; j < this.GM.ArmorCards.Count; j++)
			{
				if (this.GraphicsManager.CharacterWindow.HasCardEquipped(this.GM.ArmorCards[j]))
				{
					this.PlayerBodyLocationHit.ArmorWeights.Head = this.PlayerBodyLocationHit.ArmorWeights.Head + this.GM.ArmorCards[j].CardModel.ArmorValues.HeadHitProbabilityModifier;
					this.PlayerBodyLocationHit.ArmorWeights.Torso = this.PlayerBodyLocationHit.ArmorWeights.Torso + this.GM.ArmorCards[j].CardModel.ArmorValues.TorsoHitProbabilityModifier;
					this.PlayerBodyLocationHit.ArmorWeights.LArm = this.PlayerBodyLocationHit.ArmorWeights.LArm + this.GM.ArmorCards[j].CardModel.ArmorValues.LArmHitProbabilityModifier;
					this.PlayerBodyLocationHit.ArmorWeights.RArm = this.PlayerBodyLocationHit.ArmorWeights.RArm + this.GM.ArmorCards[j].CardModel.ArmorValues.RArmHitProbabilityModifier;
					this.PlayerBodyLocationHit.ArmorWeights.LLeg = this.PlayerBodyLocationHit.ArmorWeights.LLeg + this.GM.ArmorCards[j].CardModel.ArmorValues.LLegHitProbabilityModifier;
					this.PlayerBodyLocationHit.ArmorWeights.RLeg = this.PlayerBodyLocationHit.ArmorWeights.RLeg + this.GM.ArmorCards[j].CardModel.ArmorValues.RLegHitProbabilityModifier;
					num += this.GM.ArmorCards[j].CardModel.ArmorValues.CalculateArmorForLocation(this.CurrentEncounter.CurrentEnemyAction.DamageTypes, BodyLocations.Head);
					num2 += this.GM.ArmorCards[j].CardModel.ArmorValues.CalculateArmorForLocation(this.CurrentEncounter.CurrentEnemyAction.DamageTypes, BodyLocations.Torso);
					num3 += this.GM.ArmorCards[j].CardModel.ArmorValues.CalculateArmorForLocation(this.CurrentEncounter.CurrentEnemyAction.DamageTypes, BodyLocations.LArm);
					num4 += this.GM.ArmorCards[j].CardModel.ArmorValues.CalculateArmorForLocation(this.CurrentEncounter.CurrentEnemyAction.DamageTypes, BodyLocations.RArm);
					num5 += this.GM.ArmorCards[j].CardModel.ArmorValues.CalculateArmorForLocation(this.CurrentEncounter.CurrentEnemyAction.DamageTypes, BodyLocations.LLeg);
					num6 += this.GM.ArmorCards[j].CardModel.ArmorValues.CalculateArmorForLocation(this.CurrentEncounter.CurrentEnemyAction.DamageTypes, BodyLocations.RLeg);
				}
			}
		}
		this.PlayerBodyLocationHit.EnemyActionWeights.Head = this.PlayerBodyLocationHit.EnemyActionWeights.Head + this.CurrentEncounter.CurrentEnemyAction.AddedPlayerLocationHitProbabilities.HeadHitProbabilityModifier;
		this.PlayerBodyLocationHit.EnemyActionWeights.Torso = this.PlayerBodyLocationHit.EnemyActionWeights.Torso + this.CurrentEncounter.CurrentEnemyAction.AddedPlayerLocationHitProbabilities.TorsoHitProbabilityModifier;
		this.PlayerBodyLocationHit.EnemyActionWeights.LArm = this.PlayerBodyLocationHit.EnemyActionWeights.LArm + this.CurrentEncounter.CurrentEnemyAction.AddedPlayerLocationHitProbabilities.LArmHitProbabilityModifier;
		this.PlayerBodyLocationHit.EnemyActionWeights.RArm = this.PlayerBodyLocationHit.EnemyActionWeights.RArm + this.CurrentEncounter.CurrentEnemyAction.AddedPlayerLocationHitProbabilities.RArmHitProbabilityModifier;
		this.PlayerBodyLocationHit.EnemyActionWeights.LLeg = this.PlayerBodyLocationHit.EnemyActionWeights.LLeg + this.CurrentEncounter.CurrentEnemyAction.AddedPlayerLocationHitProbabilities.LLegHitProbabilityModifier;
		this.PlayerBodyLocationHit.EnemyActionWeights.RLeg = this.PlayerBodyLocationHit.EnemyActionWeights.RLeg + this.CurrentEncounter.CurrentEnemyAction.AddedPlayerLocationHitProbabilities.RLegHitProbabilityModifier;
		this.PlayerBodyLocationHit.FillRanges();
		this.PlayerBodyLocationHit.RandomRoll = UnityEngine.Random.Range(0f, this.PlayerBodyLocationHit.TotalWeight);
		BodyLocations bodyLocations;
		if (this.PlayerBodyLocationHit.RandomRoll < this.PlayerBodyLocationHit.Ranges.Head)
		{
			bodyLocations = BodyLocations.Head;
		}
		else if (this.PlayerBodyLocationHit.RandomRoll < this.PlayerBodyLocationHit.Ranges.Torso)
		{
			bodyLocations = BodyLocations.Torso;
		}
		else if (this.PlayerBodyLocationHit.RandomRoll < this.PlayerBodyLocationHit.Ranges.LArm)
		{
			bodyLocations = BodyLocations.LArm;
		}
		else if (this.PlayerBodyLocationHit.RandomRoll < this.PlayerBodyLocationHit.Ranges.RArm)
		{
			bodyLocations = BodyLocations.RArm;
		}
		else if (this.PlayerBodyLocationHit.RandomRoll < this.PlayerBodyLocationHit.Ranges.LLeg)
		{
			bodyLocations = BodyLocations.LLeg;
		}
		else if (this.PlayerBodyLocationHit.RandomRoll < this.PlayerBodyLocationHit.Ranges.RLeg)
		{
			bodyLocations = BodyLocations.RLeg;
		}
		else
		{
			bodyLocations = BodyLocations.Torso;
		}
		Action<PlayerBodyLocationSelectionReport> onPlayerHitLocationPicked = EncounterPopup.OnPlayerHitLocationPicked;
		if (onPlayerHitLocationPicked != null)
		{
			onPlayerHitLocationPicked(this.PlayerBodyLocationHit);
		}
		this.CurrentRoundEnemyDamageReport.SizeDamage = ((this.CurrentEncounter.CurrentEnemyAction.ActionRange == ActionRange.Melee) ? this.CurrentEncounter.CurrentEnemySize : 0f);
		this.CurrentRoundEnemyDamageReport.ActionDamage = UnityEngine.Random.Range(this.CurrentEncounter.CurrentEnemyAction.Damage.x, this.CurrentEncounter.CurrentEnemyAction.Damage.y);
		this.CurrentRoundEnemyDamageReport.ValuesDamage = this.CurrentEncounter.CurrentEnemyAction.AddedDamageFromEnemyValues(this.CurrentEncounter, true);
		this.CurrentRoundEnemyDamageReport.WoundsDamage = this.CurrentEncounter.CurrentEnemyAction.AddedDamageValueFromWounds(this.CurrentEncounter, true);
		this.CurrentRoundEnemyDamageReport.StatsAddedDamage = this.CurrentEncounter.CurrentEnemyAction.AddedDamageFromStats(true);
		if (this.CurrentEncounter.CurrentPlayerAction.IsEscapeAction)
		{
			this.CurrentRoundEnemyDamageReport.VsEscapeDamage = this.CurrentEncounter.CurrentEnemyAction.AddedDamageVsEscape(true);
		}
		this.CurrentRoundEnemyDamageReport.DmgTypes = new DamageType[this.CurrentEncounter.CurrentEnemyAction.DamageTypes.Length];
		this.CurrentEncounter.CurrentEnemyAction.DamageTypes.CopyTo(this.CurrentRoundEnemyDamageReport.DmgTypes, 0);
		this.CurrentRoundEnemyDamageReport.SizeDefense = this.PlayerSize;
		switch (bodyLocations)
		{
		case BodyLocations.Head:
			this.CurrentRoundEnemyDamageReport.HitBodyPart = BodyLocations.Head;
			this.CurrentRoundEnemyDamageReport.BodyPartArmor = this.Head.GetArmor(this.CurrentEncounter.CurrentEnemyAction.DamageTypes);
			this.CurrentRoundEnemyDamageReport.ArmorDefense = num;
			break;
		case BodyLocations.Torso:
			this.CurrentRoundEnemyDamageReport.HitBodyPart = BodyLocations.Torso;
			this.CurrentRoundEnemyDamageReport.BodyPartArmor = this.Torso.GetArmor(this.CurrentEncounter.CurrentEnemyAction.DamageTypes);
			this.CurrentRoundEnemyDamageReport.ArmorDefense = num2;
			break;
		case BodyLocations.LArm:
			this.CurrentRoundEnemyDamageReport.HitBodyPart = BodyLocations.LArm;
			this.CurrentRoundEnemyDamageReport.BodyPartArmor = this.LArm.GetArmor(this.CurrentEncounter.CurrentEnemyAction.DamageTypes);
			this.CurrentRoundEnemyDamageReport.ArmorDefense = num3;
			break;
		case BodyLocations.RArm:
			this.CurrentRoundEnemyDamageReport.HitBodyPart = BodyLocations.RArm;
			this.CurrentRoundEnemyDamageReport.BodyPartArmor = this.RArm.GetArmor(this.CurrentEncounter.CurrentEnemyAction.DamageTypes);
			this.CurrentRoundEnemyDamageReport.ArmorDefense = num4;
			break;
		case BodyLocations.LLeg:
			this.CurrentRoundEnemyDamageReport.HitBodyPart = BodyLocations.LLeg;
			this.CurrentRoundEnemyDamageReport.BodyPartArmor = this.LLeg.GetArmor(this.CurrentEncounter.CurrentEnemyAction.DamageTypes);
			this.CurrentRoundEnemyDamageReport.ArmorDefense = num5;
			break;
		case BodyLocations.RLeg:
			this.CurrentRoundEnemyDamageReport.HitBodyPart = BodyLocations.RLeg;
			this.CurrentRoundEnemyDamageReport.BodyPartArmor = this.RLeg.GetArmor(this.CurrentEncounter.CurrentEnemyAction.DamageTypes);
			this.CurrentRoundEnemyDamageReport.ArmorDefense = num6;
			break;
		}
		this.CurrentRoundEnemyDamageReport.StatsDefense = 0f;
		if (this.PlayerExtraDefenseCalculation != null)
		{
			for (int k = 0; k < this.PlayerExtraDefenseCalculation.Length; k++)
			{
				this.CurrentRoundEnemyDamageReport.StatsDefense = this.CurrentRoundEnemyDamageReport.StatsDefense + this.PlayerExtraDefenseCalculation[k].GenerateValue(true);
			}
		}
		WoundSeverity woundSeverity = this.GenerateWoundSeverity(this.CurrentRoundEnemyDamageReport.EnemyDamage, this.CurrentRoundEnemyDamageReport.PlayerDefense);
		this.CurrentRoundEnemyDamageReport.AttackSeverity = woundSeverity;
		List<PlayerWound> list = new List<PlayerWound>();
		this.CurrentEncounter.CurrentEnemyAction.PlayerWounds.GetWoundsForSeverity(woundSeverity, ref list);
		if (list.Count == 0)
		{
			this.CurrentEncounter.EncounterModel.DefaultPlayerWounds.GetWoundsForSeverity(woundSeverity, ref list);
		}
		if (list.Count == 0)
		{
			this.DefaultPlayerWounds.GetWoundsForSeverity(woundSeverity, ref list);
		}
		for (int l = list.Count - 1; l >= 0; l--)
		{
			if (!list[l].LocationFilter.CanHit(bodyLocations))
			{
				list.RemoveAt(l);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("There are no Wounds found for a player, please check general Wound lists");
			return;
		}
		Action<EncounterEnemyDamageReport> onEnemyInflictedDamage = EncounterPopup.OnEnemyInflictedDamage;
		if (onEnemyInflictedDamage != null)
		{
			onEnemyInflictedDamage(this.CurrentRoundEnemyDamageReport);
		}
		this.CurrentEncounter.CurrentRoundPlayerWound = this.SelectPlayerWound(list, bodyLocations);
		if (woundSeverity != WoundSeverity.NoWound)
		{
			EncounterLogMessage encounterLogMessage = EncounterLogMessage.Duplicate(this.CurrentEncounter.CurrentRoundPlayerWound.WoundLog);
			if (!encounterLogMessage.TextSettings)
			{
				encounterLogMessage.TextSettings = this.PlayerWoundTextSettings;
			}
			if (!encounterLogMessage.TextColor)
			{
				encounterLogMessage.TextColor = this.PlayerWoundTextColor;
			}
			this.AddToLog(encounterLogMessage);
		}
		else
		{
			this.AddToLog(this.CurrentEncounter.CurrentRoundPlayerWound.WoundLog);
		}
		List<CardData> list2 = new List<CardData>();
		List<StatModifier> list3 = new List<StatModifier>();
		if (this.CurrentEncounter.CurrentRoundPlayerWound.DroppedCards != null && this.CurrentEncounter.CurrentRoundPlayerWound.DroppedCards.Length != 0)
		{
			for (int m = 0; m < this.CurrentEncounter.CurrentRoundPlayerWound.DroppedCards.Length; m++)
			{
				if (this.CurrentEncounter.CurrentRoundPlayerWound.DroppedCards[m] && this.CurrentEncounter.CurrentRoundPlayerWound.DroppedCards[m].CanSpawnOnBoard())
				{
					list2.Add(this.CurrentEncounter.CurrentRoundPlayerWound.DroppedCards[m]);
				}
			}
		}
		if (this.CurrentEncounter.CurrentRoundPlayerWound.StatChanges != null && this.CurrentEncounter.CurrentRoundPlayerWound.StatChanges.Length != 0)
		{
			list3.AddRange(this.CurrentEncounter.CurrentRoundPlayerWound.StatChanges);
		}
		CardAction cardAction = new CardAction();
		cardAction.ActionName = new LocalizedString
		{
			LocalizationKey = "IGNOREKEY",
			DefaultText = "Encounter_Player_Wound"
		};
		cardAction.ProducedCards = new CardsDropCollection[1];
		cardAction.ProducedCards[0] = new CardsDropCollection();
		cardAction.ProducedCards[0].SetDroppedCards(list2);
		cardAction.StatModifications = list3.ToArray();
		List<ExtraDurabilityChange> list4 = new List<ExtraDurabilityChange>();
		List<CardData> list5 = new List<CardData>();
		for (int n = 0; n < this.GM.ArmorCards.Count; n++)
		{
			if (!this.GM.ArmorCards[n].Destroyed && this.GraphicsManager.CharacterWindow.HasCardEquipped(this.GM.ArmorCards[n]) && this.GM.ArmorCards[n].CardModel.ArmorValues.ProtectsLocation(bodyLocations) && !list5.Contains(this.GM.ArmorCards[n].CardModel))
			{
				list4.Add(new ExtraDurabilityChange());
				list4[list4.Count - 1].AffectedCards = new List<CardData>();
				list4[list4.Count - 1].AffectedCards.Add(this.GM.ArmorCards[n].CardModel);
				list4[list4.Count - 1].AppliesTo = RemoteDurabilityChanges.EquippedCards;
				list4[list4.Count - 1].CardChanges = RemoteCardStateChanges.ChangeDurabilities;
				list4[list4.Count - 1].DropOnDestroyList = true;
				if (this.CurrentEncounter.CurrentEnemyAction.ArmorDurabilityDamage)
				{
					list4[list4.Count - 1].UsageChange = Vector2.one * -this.CurrentEncounter.CurrentEnemyAction.ArmorDurabilityDamage;
				}
				else
				{
					list4[list4.Count - 1].UsageChange = Vector2.one * -this.ArmorDefaultDurabilityDamage;
				}
				list5.Add(this.GM.ArmorCards[n].CardModel);
			}
		}
		if (list4.Count > 0)
		{
			cardAction.ExtraDurabilityModifications = list4.ToArray();
		}
		this.GM.StartCoroutine(this.WaitForAction(cardAction, null));
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0002E175 File Offset: 0x0002C375
	private IEnumerator WaitForAction(CardAction _Action, InGameCardBase _OnCard)
	{
		int index = this.ActionsPlaying.Count;
		this.ActionsPlaying.Add(true);
		yield return GameManager.PerformAction(_Action, _OnCard, false);
		this.ActionsPlaying[index] = false;
		yield break;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0002E192 File Offset: 0x0002C392
	private IEnumerator WaitBeforeNextRound()
	{
		while (this.ActionPlaying || this.LogIsUpdating)
		{
			yield return null;
		}
		if (this.StatBasedEncounterResults != null)
		{
			for (int i = 0; i < this.StatBasedEncounterResults.Length; i++)
			{
				if (this.StatBasedEncounterResults[i].Stat && this.StatBasedEncounterResults[i].IsInRange(this.GM.StatsDict[this.StatBasedEncounterResults[i].Stat].SimpleCurrentValue))
				{
					this.CurrentEncounter.EncounterResult = this.StatBasedEncounterResults[i].Result;
					if (this.ApplyEncounterResult())
					{
						yield break;
					}
				}
			}
		}
		this.CurrentEncounter.CurrentRound++;
		this.AddLogSeparator(0f);
		this.CurrentEncounter.PlayerHidden = false;
		this.CurrentEncounter.EnemyHidden = false;
		this.RoundStart(false);
		yield break;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0002E1A1 File Offset: 0x0002C3A1
	private IEnumerator WaitBeforeClosingWindow()
	{
		base.gameObject.SetActive(false);
		this.ContinueButton.interactable = false;
		while (this.ActionPlaying)
		{
			yield return null;
		}
		this.LogQueue.Clear();
		this.LogQueueTimer = 0f;
		this.OngoingEncounter = false;
		yield break;
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0002E1B0 File Offset: 0x0002C3B0
	public PlayerWound SelectPlayerWound(List<PlayerWound> _PlayerWounds, BodyLocations _HitLocation)
	{
		PlayerWoundSelectionReport playerWoundSelectionReport = this.GetPlayerWoundSelectionReport(_PlayerWounds);
		if (playerWoundSelectionReport.PlayerWounds == null)
		{
			return null;
		}
		if (playerWoundSelectionReport.PlayerWounds.Length == 0)
		{
			return null;
		}
		if (playerWoundSelectionReport.TotalWeight == 0f)
		{
			playerWoundSelectionReport.SelectedWound = UnityEngine.Random.Range(0, playerWoundSelectionReport.PlayerWounds.Length);
			playerWoundSelectionReport.RandomValue = (float)playerWoundSelectionReport.SelectedWound;
			Action<PlayerWoundSelectionReport> onPlayerWoundSelected = EncounterPopup.OnPlayerWoundSelected;
			if (onPlayerWoundSelected != null)
			{
				onPlayerWoundSelected(playerWoundSelectionReport);
			}
			return _PlayerWounds[playerWoundSelectionReport.SelectedWound];
		}
		float num = UnityEngine.Random.Range(0f, playerWoundSelectionReport.TotalWeight - 0.001f);
		playerWoundSelectionReport.RandomValue = num;
		for (int i = 0; i < _PlayerWounds.Count; i++)
		{
			if (num < playerWoundSelectionReport.PlayerWounds[i].RangeUpTo)
			{
				playerWoundSelectionReport.SelectedWound = i;
				Action<PlayerWoundSelectionReport> onPlayerWoundSelected2 = EncounterPopup.OnPlayerWoundSelected;
				if (onPlayerWoundSelected2 != null)
				{
					onPlayerWoundSelected2(playerWoundSelectionReport);
				}
				return _PlayerWounds[i];
			}
		}
		playerWoundSelectionReport.SelectedWound = -1;
		Action<PlayerWoundSelectionReport> onPlayerWoundSelected3 = EncounterPopup.OnPlayerWoundSelected;
		if (onPlayerWoundSelected3 != null)
		{
			onPlayerWoundSelected3(playerWoundSelectionReport);
		}
		return null;
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0002E2A8 File Offset: 0x0002C4A8
	private PlayerWoundSelectionReport GetPlayerWoundSelectionReport(List<PlayerWound> _PlayerWounds)
	{
		PlayerWoundSelectionReport result = default(PlayerWoundSelectionReport);
		result.FillPlayerWoundSelectionInfo(_PlayerWounds);
		return result;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0002E2C8 File Offset: 0x0002C4C8
	private void GenerateEnemyWound()
	{
		bool flag = this.CurrentEncounter.CurrentPlayerAction.ActionRange == ActionRange.Melee;
		this.EnemyBodyLocationHit = default(EnemyBodyLocationSelectionReport);
		this.EnemyBodyLocationHit.Ranged = !flag;
		this.EnemyBodyLocationHit.BaseWeights.Head = (flag ? this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.Head.MeleeHitChanceWeight : this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.Head.RangedHitChanceWeight);
		this.EnemyBodyLocationHit.BaseWeights.Torso = (flag ? this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.Torso.MeleeHitChanceWeight : this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.Torso.RangedHitChanceWeight);
		this.EnemyBodyLocationHit.BaseWeights.LArm = (flag ? this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.LArm.MeleeHitChanceWeight : this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.LArm.RangedHitChanceWeight);
		this.EnemyBodyLocationHit.BaseWeights.RArm = (flag ? this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.RArm.MeleeHitChanceWeight : this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.RArm.RangedHitChanceWeight);
		this.EnemyBodyLocationHit.BaseWeights.LLeg = (flag ? this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.LLeg.MeleeHitChanceWeight : this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.LLeg.RangedHitChanceWeight);
		this.EnemyBodyLocationHit.BaseWeights.RLeg = (flag ? this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.RLeg.MeleeHitChanceWeight : this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.RLeg.RangedHitChanceWeight);
		this.EnemyBodyLocationHit.ArmorWeights.Head = this.CurrentEncounter.EncounterModel.EnemyArmor.HeadHitProbabilityModifier;
		this.EnemyBodyLocationHit.ArmorWeights.Torso = this.CurrentEncounter.EncounterModel.EnemyArmor.TorsoHitProbabilityModifier;
		this.EnemyBodyLocationHit.ArmorWeights.LArm = this.CurrentEncounter.EncounterModel.EnemyArmor.LArmHitProbabilityModifier;
		this.EnemyBodyLocationHit.ArmorWeights.RArm = this.CurrentEncounter.EncounterModel.EnemyArmor.RArmHitProbabilityModifier;
		this.EnemyBodyLocationHit.ArmorWeights.LLeg = this.CurrentEncounter.EncounterModel.EnemyArmor.LLegHitProbabilityModifier;
		this.EnemyBodyLocationHit.ArmorWeights.RLeg = this.CurrentEncounter.EncounterModel.EnemyArmor.RLegHitProbabilityModifier;
		this.EnemyBodyLocationHit.TrackingWeights.Head = this.CurrentEncounter.CurrentEnemyBodyProbabilities.CurrentHeadProbModifier;
		this.EnemyBodyLocationHit.TrackingWeights.Torso = this.CurrentEncounter.CurrentEnemyBodyProbabilities.CurrentTorsoProbModifier;
		this.EnemyBodyLocationHit.TrackingWeights.LArm = this.CurrentEncounter.CurrentEnemyBodyProbabilities.CurrentLArmProbModifier;
		this.EnemyBodyLocationHit.TrackingWeights.RArm = this.CurrentEncounter.CurrentEnemyBodyProbabilities.CurrentRArmProbModifier;
		this.EnemyBodyLocationHit.TrackingWeights.LLeg = this.CurrentEncounter.CurrentEnemyBodyProbabilities.CurrentLLegProbModifier;
		this.EnemyBodyLocationHit.TrackingWeights.RLeg = this.CurrentEncounter.CurrentEnemyBodyProbabilities.CurrentRLegProbModifier;
		this.EnemyBodyLocationHit.FillRanges();
		this.EnemyBodyLocationHit.RandomRoll = UnityEngine.Random.Range(0f, this.EnemyBodyLocationHit.TotalWeight);
		this.CurrentRoundPlayerDamageReport = default(EncounterPlayerDamageReport);
		this.CurrentRoundPlayerDamageReport.SizeDefense = this.CurrentEncounter.CurrentEnemySize;
		if (this.EnemyBodyLocationHit.RandomRoll < this.EnemyBodyLocationHit.Ranges.Head)
		{
			this.CurrentEncounter.CurrentRoundEnemyWoundLocation = BodyLocations.Head;
			this.CurrentRoundPlayerDamageReport.HitBodyPart = BodyLocations.Head;
			this.CurrentRoundPlayerDamageReport.BodyPartArmor = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.Head.GetArmor(this.CurrentEncounter.CurrentPlayerAction.DamageTypes);
		}
		else if (this.EnemyBodyLocationHit.RandomRoll < this.EnemyBodyLocationHit.Ranges.Torso)
		{
			this.CurrentEncounter.CurrentRoundEnemyWoundLocation = BodyLocations.Torso;
			this.CurrentRoundPlayerDamageReport.HitBodyPart = BodyLocations.Torso;
			this.CurrentRoundPlayerDamageReport.BodyPartArmor = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.Torso.GetArmor(this.CurrentEncounter.CurrentPlayerAction.DamageTypes);
		}
		else if (this.EnemyBodyLocationHit.RandomRoll < this.EnemyBodyLocationHit.Ranges.LArm)
		{
			this.CurrentEncounter.CurrentRoundEnemyWoundLocation = BodyLocations.LArm;
			this.CurrentRoundPlayerDamageReport.HitBodyPart = BodyLocations.LArm;
			this.CurrentRoundPlayerDamageReport.BodyPartArmor = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.LArm.GetArmor(this.CurrentEncounter.CurrentPlayerAction.DamageTypes);
		}
		else if (this.EnemyBodyLocationHit.RandomRoll < this.EnemyBodyLocationHit.Ranges.RArm)
		{
			this.CurrentEncounter.CurrentRoundEnemyWoundLocation = BodyLocations.RArm;
			this.CurrentRoundPlayerDamageReport.HitBodyPart = BodyLocations.RArm;
			this.CurrentRoundPlayerDamageReport.BodyPartArmor = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.RArm.GetArmor(this.CurrentEncounter.CurrentPlayerAction.DamageTypes);
		}
		else if (this.EnemyBodyLocationHit.RandomRoll < this.EnemyBodyLocationHit.Ranges.LLeg)
		{
			this.CurrentEncounter.CurrentRoundEnemyWoundLocation = BodyLocations.LLeg;
			this.CurrentRoundPlayerDamageReport.HitBodyPart = BodyLocations.LLeg;
			this.CurrentRoundPlayerDamageReport.BodyPartArmor = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.LLeg.GetArmor(this.CurrentEncounter.CurrentPlayerAction.DamageTypes);
		}
		else if (this.EnemyBodyLocationHit.RandomRoll < this.EnemyBodyLocationHit.Ranges.RLeg)
		{
			this.CurrentEncounter.CurrentRoundEnemyWoundLocation = BodyLocations.RLeg;
			this.CurrentRoundPlayerDamageReport.HitBodyPart = BodyLocations.RLeg;
			this.CurrentRoundPlayerDamageReport.BodyPartArmor = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.RLeg.GetArmor(this.CurrentEncounter.CurrentPlayerAction.DamageTypes);
		}
		else
		{
			this.CurrentEncounter.CurrentRoundEnemyWoundLocation = BodyLocations.Torso;
			this.CurrentRoundPlayerDamageReport.HitBodyPart = BodyLocations.Torso;
			this.CurrentRoundPlayerDamageReport.BodyPartArmor = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.Torso.GetArmor(this.CurrentEncounter.CurrentPlayerAction.DamageTypes);
		}
		this.CurrentRoundPlayerDamageReport.DmgTypes = new DamageType[this.CurrentEncounter.CurrentPlayerAction.DamageTypes.Length];
		this.CurrentEncounter.CurrentPlayerAction.DamageTypes.CopyTo(this.CurrentRoundPlayerDamageReport.DmgTypes, 0);
		this.CurrentRoundPlayerDamageReport.ArmorDefense = this.CurrentEncounter.EncounterModel.EnemyArmor.CalculateArmorForLocation(this.CurrentEncounter.CurrentPlayerAction.DamageTypes, this.CurrentEncounter.CurrentRoundEnemyWoundLocation);
		this.CurrentRoundPlayerDamageReport.TrackingDefense = this.CurrentEncounter.CurrentEnemyBodyProbabilities.GetDefenseModifierForLocation(this.CurrentEncounter.CurrentRoundEnemyWoundLocation);
		this.CurrentRoundPlayerDamageReport.SizeDamage = ((this.CurrentEncounter.CurrentPlayerAction.ActionRange == ActionRange.Melee) ? this.PlayerSize : 0f);
		this.CurrentRoundPlayerDamageReport.ActionDamage = this.CurrentEncounter.CurrentPlayerAction.GetDamage(true);
		this.CurrentRoundPlayerDamageReport.StatsAddedDamage = this.CurrentEncounter.CurrentPlayerAction.GetDamageStatSum(true);
		if (this.CurrentEncounter.CurrentEnemyAction.IsEscapeAction)
		{
			this.CurrentRoundPlayerDamageReport.VsEscapeDamage = this.CurrentEncounter.CurrentPlayerAction.GetDmgVsEscapeModifier(true);
		}
		if (this.CurrentEncounter.CurrentPlayerAction.DoesNotAttack)
		{
			this.CurrentEncounter.CurrentRoundEnemyWoundSeverity = WoundSeverity.NoWound;
			this.CurrentEncounter.CurrentRoundEnemyWound = null;
			this.CurrentRoundEnemyWoundSelectionReport = new EnemyWoundSelectionReport(null, -1);
			return;
		}
		this.CurrentEncounter.CurrentRoundEnemyWoundSeverity = this.GenerateWoundSeverity(this.CurrentRoundPlayerDamageReport.PlayerDamage, this.CurrentRoundPlayerDamageReport.EnemyDefense);
		this.CurrentRoundPlayerDamageReport.AttackSeverity = this.CurrentEncounter.CurrentRoundEnemyWoundSeverity;
		EnemyWound[] woundsForSeverityDamageType = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.GetBodyLocation(this.CurrentEncounter.CurrentRoundEnemyWoundLocation).GetWoundsForSeverityDamageType(this.CurrentEncounter.CurrentRoundEnemyWoundSeverity, this.CurrentEncounter.CurrentPlayerAction.DamageTypes);
		if (woundsForSeverityDamageType == null || woundsForSeverityDamageType.Length == 0)
		{
			this.CurrentEncounter.CurrentRoundEnemyWound = this.CurrentEncounter.EncounterModel.EnemyBodyTemplate.DefaultWound;
			this.CurrentRoundEnemyWoundSelectionReport = new EnemyWoundSelectionReport(null, -1);
			return;
		}
		int num = UnityEngine.Random.Range(0, woundsForSeverityDamageType.Length);
		this.CurrentEncounter.CurrentRoundEnemyWound = woundsForSeverityDamageType[num];
		this.CurrentRoundEnemyWoundSelectionReport = new EnemyWoundSelectionReport(woundsForSeverityDamageType, num);
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0002EBD0 File Offset: 0x0002CDD0
	public float GetDefaultWoundScore(WoundSeverity _ForSeverity)
	{
		switch (_ForSeverity)
		{
		default:
			return this.NoWoundDefaultScore;
		case WoundSeverity.Minor:
			return this.MinorWoundDefaultScore;
		case WoundSeverity.Medium:
			return this.MediumWoundDefaultScore;
		case WoundSeverity.Serious:
			return this.SeriousWoundDefaultScore;
		}
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0002EC04 File Offset: 0x0002CE04
	private void ApplyEnemyWound()
	{
		if (this.CurrentEncounter.CurrentRoundEnemyWound == null)
		{
			return;
		}
		Action<EnemyBodyLocationSelectionReport> onEnemyHitLocationPicked = EncounterPopup.OnEnemyHitLocationPicked;
		if (onEnemyHitLocationPicked != null)
		{
			onEnemyHitLocationPicked(this.EnemyBodyLocationHit);
		}
		float num = this.CurrentEncounter.CurrentRoundEnemyWound.AccumulatedWoundValue ? this.CurrentEncounter.CurrentRoundEnemyWound.AccumulatedWoundValue.GetRandomValue : this.GetDefaultWoundScore(this.CurrentEncounter.CurrentRoundEnemyWoundSeverity);
		this.CurrentEncounter.AllWoundsAccumulated += num;
		switch (this.CurrentEncounter.CurrentRoundEnemyWoundLocation)
		{
		case BodyLocations.Head:
			this.CurrentEncounter.HeadWoundsAccumulated += num;
			break;
		case BodyLocations.Torso:
			this.CurrentEncounter.TorsoWoundsAccumulated += num;
			break;
		case BodyLocations.LArm:
			this.CurrentEncounter.LeftArmWoundsAccumulated += num;
			break;
		case BodyLocations.RArm:
			this.CurrentEncounter.RightArmWoundsAccumulated += num;
			break;
		case BodyLocations.LLeg:
			this.CurrentEncounter.LeftLegWoundsAccumulated += num;
			break;
		case BodyLocations.RLeg:
			this.CurrentEncounter.RightLegWoundsAccumulated += num;
			break;
		}
		Action<EncounterPlayerDamageReport> onPlayerInflictedDamage = EncounterPopup.OnPlayerInflictedDamage;
		if (onPlayerInflictedDamage != null)
		{
			onPlayerInflictedDamage(this.CurrentRoundPlayerDamageReport);
		}
		Action<EnemyWoundSelectionReport> onEnemyWoundSelected = EncounterPopup.OnEnemyWoundSelected;
		if (onEnemyWoundSelected != null)
		{
			onEnemyWoundSelected(this.CurrentRoundEnemyWoundSelectionReport);
		}
		if (this.CurrentEncounter.CurrentRoundEnemyWoundSeverity != WoundSeverity.NoWound)
		{
			EncounterLogMessage encounterLogMessage = EncounterLogMessage.Duplicate(this.CurrentEncounter.CurrentRoundEnemyWound.CombatLog);
			if (!encounterLogMessage.TextSettings)
			{
				encounterLogMessage.TextSettings = this.EnemyWoundTextSettings;
			}
			if (!encounterLogMessage.TextColor)
			{
				encounterLogMessage.TextColor = this.EnemyWoundTextColor;
			}
			this.AddToLog(encounterLogMessage);
		}
		else
		{
			this.AddToLog(this.CurrentEncounter.CurrentRoundEnemyWound.CombatLog);
		}
		if (this.CurrentEncounter.CurrentRoundEnemyWound.StatChanges != null || this.CurrentEncounter.CurrentRoundEnemyWound.DroppedCards != null)
		{
			CardAction cardAction = new CardAction();
			cardAction.ActionName = new LocalizedString
			{
				LocalizationKey = "IGNOREKEY",
				DefaultText = this.CurrentEncounter.CurrentRoundEnemyWound.CombatLog.MainLogDefaultText
			};
			cardAction.StatModifications = this.CurrentEncounter.CurrentRoundEnemyWound.StatChanges;
			if (this.CurrentEncounter.CurrentRoundEnemyWound.DroppedCards != null)
			{
				cardAction.ProducedCards = new CardsDropCollection[1];
				cardAction.ProducedCards[0] = new CardsDropCollection();
				List<CardData> list = new List<CardData>();
				if (this.CurrentEncounter.CurrentRoundEnemyWound.DroppedCards != null && this.CurrentEncounter.CurrentRoundEnemyWound.DroppedCards.Length != 0)
				{
					for (int i = 0; i < this.CurrentEncounter.CurrentRoundEnemyWound.DroppedCards.Length; i++)
					{
						if (this.CurrentEncounter.CurrentRoundEnemyWound.DroppedCards[i] && this.CurrentEncounter.CurrentRoundEnemyWound.DroppedCards[i].CanSpawnOnBoard())
						{
							list.Add(this.CurrentEncounter.CurrentRoundEnemyWound.DroppedCards[i]);
						}
					}
				}
				cardAction.ProducedCards[0].SetDroppedCards(list);
			}
			this.GM.StartCoroutine(this.WaitForAction(cardAction, null));
		}
		this.CurrentEncounter.CurrentEnemyBodyProbabilities.ApplyChanges(this.CurrentEncounter.CurrentRoundEnemyWound.BodyLocationModifiers);
		switch (this.CurrentEncounter.CurrentRoundClashResult)
		{
		case ClashResults.PlayerHits:
		case ClashResults.NoHit:
			this.CurrentEncounter.ModifyEnemyValues(this.CurrentEncounter.CurrentRoundEnemyWound.EnemyValuesModifiers, EnemyActionEffectCondition.OnlyOnMiss);
			break;
		case ClashResults.EnemyHits:
		case ClashResults.BothHit:
			this.CurrentEncounter.ModifyEnemyValues(this.CurrentEncounter.CurrentRoundEnemyWound.EnemyValuesModifiers, EnemyActionEffectCondition.OnlyOnHit);
			break;
		}
		if (this.CurrentEncounter.CurrentPlayerAction.AssociatedCard)
		{
			CardAction cardAction2 = new CardAction();
			cardAction2.ActionName = new LocalizedString
			{
				LocalizationKey = "IGNOREKEY",
				DefaultText = "Encounter_Weapon_Durability"
			};
			cardAction2.ReceivingCardChanges.ModType = CardModifications.DurabilityChanges;
			if (this.CurrentEncounter.CurrentPlayerAction.AmmoCard)
			{
				CardAction cardAction3 = new CardAction();
				cardAction3.ActionName = new LocalizedString
				{
					LocalizationKey = "IGNOREKEY",
					DefaultText = "Encounter_Ammo_Durability"
				};
				cardAction3.ReceivingCardChanges.ModType = CardModifications.DurabilityChanges;
				cardAction2.ReceivingCardChanges.UsageChange = -Vector2.one;
				if (this.CurrentEncounter.CurrentRoundEnemyWound.WeaponDurabilityDamage)
				{
					cardAction3.ReceivingCardChanges.UsageChange = Vector2.one * -this.CurrentEncounter.CurrentRoundEnemyWound.WeaponDurabilityDamage;
				}
				else
				{
					cardAction3.ReceivingCardChanges.UsageChange = -Vector2.one;
				}
				this.GM.StartCoroutine(this.WaitForAction(cardAction3, this.CurrentEncounter.CurrentPlayerAction.AmmoCard));
			}
			else if (this.CurrentEncounter.CurrentRoundEnemyWound.WeaponDurabilityDamage)
			{
				cardAction2.ReceivingCardChanges.UsageChange = Vector2.one * -this.CurrentEncounter.CurrentRoundEnemyWound.WeaponDurabilityDamage;
			}
			else
			{
				cardAction2.ReceivingCardChanges.UsageChange = -Vector2.one;
			}
			this.GM.StartCoroutine(this.WaitForAction(cardAction2, this.CurrentEncounter.CurrentPlayerAction.AssociatedCard));
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0002F234 File Offset: 0x0002D434
	[CompilerGenerated]
	private EncounterPlayerAction <DisplayPlayerActions>g__findAction|134_0(InGameCardBase _Weapon, InGameCardBase _Ammo)
	{
		for (int i = 0; i < this.EncounterPlayerActions.Count; i++)
		{
			if ((this.EncounterPlayerActions[i].AmmoCard || !_Ammo) && this.EncounterPlayerActions[i].AssociatedCard.CardModel == _Weapon.CardModel && this.EncounterPlayerActions[i].AmmoCard.CardModel == _Ammo.CardModel)
			{
				return this.EncounterPlayerActions[i];
			}
		}
		return null;
	}

	// Token: 0x0400053C RID: 1340
	[SpecialHeader("Player Body", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public float PlayerSize;

	// Token: 0x0400053D RID: 1341
	public BodyLocation Head;

	// Token: 0x0400053E RID: 1342
	public BodyLocation Torso;

	// Token: 0x0400053F RID: 1343
	public BodyLocation LArm;

	// Token: 0x04000540 RID: 1344
	public BodyLocation RArm;

	// Token: 0x04000541 RID: 1345
	public BodyLocation LLeg;

	// Token: 0x04000542 RID: 1346
	public BodyLocation RLeg;

	// Token: 0x04000543 RID: 1347
	[SpecialHeader("Combat Rules", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public InterpolatedValue[] ClashDifferenceTieChance;

	// Token: 0x04000544 RID: 1348
	public WoundSeverityMappings[] WoundSeverityMappings;

	// Token: 0x04000545 RID: 1349
	[Space]
	public PlayerEncounterVariable[] PlayerStealthCalculation;

	// Token: 0x04000546 RID: 1350
	public PlayerEncounterVariable[] PlayerAlertnessCalculation;

	// Token: 0x04000547 RID: 1351
	public PlayerEncounterVariable[] PlayerCoverCalculation;

	// Token: 0x04000548 RID: 1352
	public PlayerEncounterVariable[] PlayerExtraDefenseCalculation;

	// Token: 0x04000549 RID: 1353
	[Space]
	public StatEncounterResult[] StatBasedEncounterResults;

	// Token: 0x0400054A RID: 1354
	public StatEnforcedPlayerAction[] StatEnforcedPlayerActions;

	// Token: 0x0400054B RID: 1355
	[Space]
	public float NoWoundDefaultScore;

	// Token: 0x0400054C RID: 1356
	public float MinorWoundDefaultScore = 1f;

	// Token: 0x0400054D RID: 1357
	public float MediumWoundDefaultScore = 3f;

	// Token: 0x0400054E RID: 1358
	public float SeriousWoundDefaultScore = 9f;

	// Token: 0x0400054F RID: 1359
	[Space]
	public float ArmorDefaultDurabilityDamage = 250f;

	// Token: 0x04000550 RID: 1360
	[Space]
	public PlayerWounds DefaultPlayerWounds;

	// Token: 0x04000551 RID: 1361
	[Space]
	public GenericEncounterPlayerAction PlayerEscapeAction;

	// Token: 0x04000552 RID: 1362
	public GenericEncounterPlayerAction PlayerGetCloseAction;

	// Token: 0x04000553 RID: 1363
	[SpecialHeader("Default Log Texts", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public EncounterLogMessage EnemyHiddenLog;

	// Token: 0x04000554 RID: 1364
	public EncounterLogMessage EnemyHiddenPluralLog;

	// Token: 0x04000555 RID: 1365
	public EncounterLogMessage PlayerHiddenLog;

	// Token: 0x04000556 RID: 1366
	public EncounterLogMessage PlayerHiddenPluralLog;

	// Token: 0x04000557 RID: 1367
	public EncounterLogMessage EnemyGetCloseActionResultLog;

	// Token: 0x04000558 RID: 1368
	public EncounterLogMessage PlayerStealthAttackLog;

	// Token: 0x04000559 RID: 1369
	[Space]
	public OptionalTextSettings EnemySuccessTextSettings;

	// Token: 0x0400055A RID: 1370
	public OptionalColorValue EnemySuccessTextColor;

	// Token: 0x0400055B RID: 1371
	public OptionalTextSettings EnemyWoundTextSettings;

	// Token: 0x0400055C RID: 1372
	public OptionalColorValue EnemyWoundTextColor;

	// Token: 0x0400055D RID: 1373
	public OptionalTextSettings PlayerWoundTextSettings;

	// Token: 0x0400055E RID: 1374
	public OptionalColorValue PlayerWoundTextColor;

	// Token: 0x0400055F RID: 1375
	[Space]
	public EncounterLogMessage PlayerWeaponActionSuccessLog;

	// Token: 0x04000560 RID: 1376
	public EncounterLogMessage PlayerWeaponActionFailureLog;

	// Token: 0x04000561 RID: 1377
	[Space]
	[FormerlySerializedAs("DefaultPlayerWinsEncounterLog")]
	public EncounterLogMessage DefaultEnemyDefeatedEncounterLog;

	// Token: 0x04000562 RID: 1378
	[FormerlySerializedAs("DefaultEnemyWinsEncounterLog")]
	public EncounterLogMessage DefaultEnemyEscapedEncounterLog;

	// Token: 0x04000563 RID: 1379
	[FormerlySerializedAs("DefaultEncounterIsATieLog")]
	public EncounterLogMessage DefaultPlayerEscapedLog;

	// Token: 0x04000564 RID: 1380
	public EncounterLogMessage DefaultPlayerDemoralizedLog;

	// Token: 0x04000565 RID: 1381
	public EncounterLogMessage DefaultSpecialResultLog;

	// Token: 0x04000566 RID: 1382
	[SpecialHeader("Visuals Setup", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	[SerializeField]
	private TextMeshProUGUI EncounterTitle;

	// Token: 0x04000567 RID: 1383
	[SerializeField]
	private Image EncounterIllustration;

	// Token: 0x04000568 RID: 1384
	[SerializeField]
	private EncounterOptionButton ActionButtonPrefab;

	// Token: 0x04000569 RID: 1385
	[SerializeField]
	private RectTransform ActionButtonsParent;

	// Token: 0x0400056A RID: 1386
	[SerializeField]
	private RectTransform EncounterLogParent;

	// Token: 0x0400056B RID: 1387
	[SerializeField]
	private TextMeshProUGUI EncounterLogTextPrefab;

	// Token: 0x0400056C RID: 1388
	[SerializeField]
	private GameObject EncounterLogSeparatorPrefab;

	// Token: 0x0400056D RID: 1389
	[SerializeField]
	private GameObject ContinueButtonObject;

	// Token: 0x0400056E RID: 1390
	[SerializeField]
	private Button ContinueButton;

	// Token: 0x0400056F RID: 1391
	[SerializeField]
	private GameObject HelpButton;

	// Token: 0x04000570 RID: 1392
	[SerializeField]
	private ScrollRect LogScroll;

	// Token: 0x04000573 RID: 1395
	[NonSerialized]
	public List<EncounterPlayerAction> EncounterPlayerActions = new List<EncounterPlayerAction>();

	// Token: 0x04000574 RID: 1396
	private List<EncounterOptionButton> ActionButtons = new List<EncounterOptionButton>();

	// Token: 0x04000575 RID: 1397
	private EncounterPlayerAction ForcedPlayerAction;

	// Token: 0x04000576 RID: 1398
	private EncounterLogMessage ForcedActionLog;

	// Token: 0x04000577 RID: 1399
	private GameManager GM;

	// Token: 0x04000578 RID: 1400
	private GraphicsManager GraphicsManager;

	// Token: 0x04000579 RID: 1401
	public const float DefaultLogTextDuration = 0.5f;

	// Token: 0x0400057A RID: 1402
	private StringBuilder CurrentEncounterLog;

	// Token: 0x0400057B RID: 1403
	private List<EncounterLogMessage> LogQueue = new List<EncounterLogMessage>();

	// Token: 0x0400057C RID: 1404
	private float LogQueueTimer;

	// Token: 0x0400057D RID: 1405
	private int LogTextCount;

	// Token: 0x0400057E RID: 1406
	private int LogSeparatorCount;

	// Token: 0x0400057F RID: 1407
	private List<LocalizedString> LogTextHistory = new List<LocalizedString>();

	// Token: 0x04000580 RID: 1408
	private List<TextMeshProUGUI> EncounterLogTexts = new List<TextMeshProUGUI>();

	// Token: 0x04000581 RID: 1409
	private List<GameObject> EncounterLogSeparators = new List<GameObject>();

	// Token: 0x04000582 RID: 1410
	private TextMeshProUGUI CurrentLogText;

	// Token: 0x04000583 RID: 1411
	private int PrevLogTextTargetLength;

	// Token: 0x04000584 RID: 1412
	private int CurrentLogTextTargetLength;

	// Token: 0x04000585 RID: 1413
	private float CurrentLogTextVisibleLength;

	// Token: 0x04000586 RID: 1414
	private float CurrentLogDuration;

	// Token: 0x04000587 RID: 1415
	private bool SkipLog;

	// Token: 0x04000588 RID: 1416
	private MeleeClashResultsReport CurrentRoundMeleeClashResult;

	// Token: 0x04000589 RID: 1417
	private RangedClashResultReport CurrentRoundRangedClashResult;

	// Token: 0x0400058A RID: 1418
	private PlayerBodyLocationSelectionReport PlayerBodyLocationHit;

	// Token: 0x0400058B RID: 1419
	private EnemyBodyLocationSelectionReport EnemyBodyLocationHit;

	// Token: 0x0400058C RID: 1420
	private EncounterPlayerDamageReport CurrentRoundPlayerDamageReport;

	// Token: 0x0400058D RID: 1421
	private EncounterEnemyDamageReport CurrentRoundEnemyDamageReport;

	// Token: 0x0400058E RID: 1422
	private EnemyWoundSelectionReport CurrentRoundEnemyWoundSelectionReport;

	// Token: 0x0400058F RID: 1423
	private EncounterStealthReport CurrentRoundStealthChecks;

	// Token: 0x04000590 RID: 1424
	private List<bool> ActionsPlaying = new List<bool>();

	// Token: 0x04000591 RID: 1425
	private List<InGameCardBase> UsedAmmo = new List<InGameCardBase>();

	// Token: 0x04000592 RID: 1426
	private List<InGameCardBase> MissedAmmo = new List<InGameCardBase>();

	// Token: 0x04000593 RID: 1427
	private List<InGameCardBase> HitAmmo = new List<InGameCardBase>();

	// Token: 0x04000594 RID: 1428
	public static Action OnNewEncounter;

	// Token: 0x04000595 RID: 1429
	public static Action<int> OnNextRound;

	// Token: 0x04000596 RID: 1430
	public static Action<MeleeClashResultsReport> OnMeleeClashRoll;

	// Token: 0x04000597 RID: 1431
	public static Action<RangedClashResultReport> OnRangedClashRoll;

	// Token: 0x04000598 RID: 1432
	public static Action<PlayerBodyLocationSelectionReport> OnPlayerHitLocationPicked;

	// Token: 0x04000599 RID: 1433
	public static Action<EnemyBodyLocationSelectionReport> OnEnemyHitLocationPicked;

	// Token: 0x0400059A RID: 1434
	public static Action<EncounterPlayerDamageReport> OnPlayerInflictedDamage;

	// Token: 0x0400059B RID: 1435
	public static Action<EncounterEnemyDamageReport> OnEnemyInflictedDamage;

	// Token: 0x0400059C RID: 1436
	public static Action<PlayerWoundSelectionReport> OnPlayerWoundSelected;

	// Token: 0x0400059D RID: 1437
	public static Action<EnemyWoundSelectionReport> OnEnemyWoundSelected;

	// Token: 0x0400059E RID: 1438
	public static Action<EncounterStealthReport> OnStealthChecked;

	// Token: 0x0400059F RID: 1439
	private const string SeparatorID = "SEPARATOR";

	// Token: 0x040005A0 RID: 1440
	private const int DefaultMaxVisibleCharacters = 99999;

	// Token: 0x040005A1 RID: 1441
	public const string StalenessPlayerActionID = "Encounter_Player_Action";

	// Token: 0x040005A2 RID: 1442
	public const string StalenessEnemyActionID = "Encounter_Enemy_Action";

	// Token: 0x040005A3 RID: 1443
	public const string StalenessWoundID = "Encounter_Player_Wound";
}
