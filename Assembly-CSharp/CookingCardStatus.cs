using System;
using System.Text;

// Token: 0x0200001C RID: 28
[Serializable]
public class CookingCardStatus
{
	// Token: 0x060001FE RID: 510 RVA: 0x00014C58 File Offset: 0x00012E58
	public void UpdateCookingProgressVisuals(float _Value, int _RemainingTicks, bool _Paused, string _CookingText)
	{
		if (!this.Card)
		{
			return;
		}
		if (!this.Card.CurrentContainer)
		{
			this.Card.UpdateCookingProgress(_Value, _RemainingTicks, _Paused, _CookingText, this.HideCookingProgress, this.FillsIngredientLiquid);
			return;
		}
		if (this.Card.CurrentContainer.ContainedLiquid == this.Card)
		{
			this.Card.CurrentContainer.UpdateCookingProgress(_Value, _RemainingTicks, _Paused, _CookingText, this.HideCookingProgress, this.FillsIngredientLiquid);
			return;
		}
		this.Card.UpdateCookingProgress(_Value, _RemainingTicks, _Paused, _CookingText, this.HideCookingProgress, this.FillsIngredientLiquid);
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00014D00 File Offset: 0x00012F00
	public void CancelCookingProgressVisuals()
	{
		if (!this.Card)
		{
			return;
		}
		if (!this.Card.CurrentContainer)
		{
			this.Card.UpdateCookingProgress(0f, 0, false, null, true, false);
			return;
		}
		if (this.Card.CurrentContainer.ContainedLiquid == this.Card)
		{
			this.Card.CurrentContainer.UpdateCookingProgress(0f, 0, false, null, true, false);
			return;
		}
		this.Card.UpdateCookingProgress(0f, 0, false, null, true, false);
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00014D90 File Offset: 0x00012F90
	public string GetCookingText(bool _Paused, int _RemainingTicks)
	{
		if (_Paused)
		{
			if (!string.IsNullOrEmpty(this.PausedCookingText))
			{
				return string.Format(this.PausedCookingText, HoursDisplay.HoursToShortString(GameManager.TickToHours(_RemainingTicks, 0)));
			}
			StringBuilder stringBuilder = new StringBuilder(this.CookingTextBase(_RemainingTicks));
			stringBuilder.Append(" (");
			stringBuilder.Append(LocalizedString.Paused);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
		else
		{
			if (!this.SelfPaused)
			{
				return this.CookingTextBase(_RemainingTicks);
			}
			if (!string.IsNullOrEmpty(this.SelfPausedText))
			{
				return string.Format(this.SelfPausedText, HoursDisplay.HoursToShortString(GameManager.TickToHours(_RemainingTicks, 0)));
			}
			StringBuilder stringBuilder2 = new StringBuilder(this.CookingTextBase(_RemainingTicks));
			stringBuilder2.Append(" (");
			stringBuilder2.Append(LocalizedString.Paused);
			stringBuilder2.Append(")");
			return stringBuilder2.ToString();
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00014E84 File Offset: 0x00013084
	private string CookingTextBase(int _RemainingTicks)
	{
		GameManager instance = MBSingleton<GameManager>.Instance;
		int num = _RemainingTicks;
		int num2 = 0;
		if (instance && instance.CurrentMiniTicks != 0)
		{
			num--;
			num2 = instance.DaySettings.MiniTicksPerTick - instance.CurrentMiniTicks;
		}
		if (!string.IsNullOrEmpty(this.CookingText))
		{
			return string.Format(this.CookingText, HoursDisplay.HoursToShortString(GameManager.TickToHours(num, num2)));
		}
		return LocalizedString.CookingTimeText(num, num2);
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00014EF8 File Offset: 0x000130F8
	public CookingCardStatus(InGameCardBase _Card, int _Target, LocalizedString _Text, LocalizedString _PausedText, LocalizedString _SelfPausedText, bool _HideProgress, bool _FillsCooker, bool _FillsIngredient)
	{
		this.Card = _Card;
		this.CookedDuration = 0;
		this.TargetDuration = _Target;
		this.CookingText = _Text;
		this.PausedCookingText = _PausedText;
		this.SelfPausedText = _SelfPausedText;
		this.HideCookingProgress = _HideProgress;
		this.FillsCookerLiquid = _FillsCooker;
		this.FillsIngredientLiquid = _FillsIngredient;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00014F4F File Offset: 0x0001314F
	public void ResetDuration(int _NewTarget)
	{
		this.CookedDuration = 0;
		this.TargetDuration = _NewTarget;
	}

	// Token: 0x040001F5 RID: 501
	public InGameCardBase Card;

	// Token: 0x040001F6 RID: 502
	public int CardIndex;

	// Token: 0x040001F7 RID: 503
	public bool UsesLiquid;

	// Token: 0x040001F8 RID: 504
	public int CookedDuration;

	// Token: 0x040001F9 RID: 505
	public int TargetDuration;

	// Token: 0x040001FA RID: 506
	public bool HideCookingProgress;

	// Token: 0x040001FB RID: 507
	public LocalizedString CookingText;

	// Token: 0x040001FC RID: 508
	public LocalizedString PausedCookingText;

	// Token: 0x040001FD RID: 509
	public LocalizedString SelfPausedText;

	// Token: 0x040001FE RID: 510
	public bool SelfPaused;

	// Token: 0x040001FF RID: 511
	public bool FillsCookerLiquid;

	// Token: 0x04000200 RID: 512
	public bool FillsIngredientLiquid;
}
