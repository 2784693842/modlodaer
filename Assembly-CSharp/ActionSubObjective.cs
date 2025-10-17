using System;
using UnityEngine;

// Token: 0x02000106 RID: 262
[Serializable]
public class ActionSubObjective : SubObjective
{
	// Token: 0x06000894 RID: 2196 RVA: 0x0005367C File Offset: 0x0005187C
	public override void Init()
	{
		base.Init();
		this.LastActionReport = default(ActionReport);
		GameManager.OnActionPerformed = (Action<ActionReport>)Delegate.Remove(GameManager.OnActionPerformed, new Action<ActionReport>(this.OnActionPerformed));
		GameManager.OnActionStarted = (Action<ActionReport>)Delegate.Remove(GameManager.OnActionStarted, new Action<ActionReport>(this.OnActionPerformed));
		if (!this.OnActionStarted)
		{
			GameManager.OnActionPerformed = (Action<ActionReport>)Delegate.Combine(GameManager.OnActionPerformed, new Action<ActionReport>(this.OnActionPerformed));
			return;
		}
		GameManager.OnActionStarted = (Action<ActionReport>)Delegate.Combine(GameManager.OnActionStarted, new Action<ActionReport>(this.OnActionPerformed));
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00053724 File Offset: 0x00051924
	public void OnDisable()
	{
		GameManager.OnActionPerformed = (Action<ActionReport>)Delegate.Remove(GameManager.OnActionPerformed, new Action<ActionReport>(this.OnActionPerformed));
		GameManager.OnActionStarted = (Action<ActionReport>)Delegate.Remove(GameManager.OnActionStarted, new Action<ActionReport>(this.OnActionPerformed));
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00053771 File Offset: 0x00051971
	public void ResetCounter()
	{
		this.ActionCounter = 0;
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0005377C File Offset: 0x0005197C
	public override void ForceComplete()
	{
		base.ForceComplete();
		GameManager.OnActionPerformed = (Action<ActionReport>)Delegate.Remove(GameManager.OnActionPerformed, new Action<ActionReport>(this.OnActionPerformed));
		GameManager.OnActionStarted = (Action<ActionReport>)Delegate.Remove(GameManager.OnActionStarted, new Action<ActionReport>(this.OnActionPerformed));
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x000537CF File Offset: 0x000519CF
	protected override int GetSaveCounter()
	{
		return this.ActionCounter;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x000537D7 File Offset: 0x000519D7
	protected override void LoadSaveCounter(int _Counter)
	{
		this.ActionCounter = _Counter;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x000537E0 File Offset: 0x000519E0
	public override float GetCompletion()
	{
		if (this.PerformMultipleTimes <= 0 || base.Complete)
		{
			return base.GetCompletion();
		}
		return (float)this.ActionCounter / (float)this.PerformMultipleTimes * (float)this.CompletionWeight;
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00053814 File Offset: 0x00051A14
	private void OnActionPerformed(ActionReport _Report)
	{
		if (base.Complete)
		{
			this.OnDisable();
			return;
		}
		if (_Report.Action == null)
		{
			if (this.DebugMode)
			{
				Debug.Log(this.ObjectiveName + " null action report received");
			}
			return;
		}
		if (this.DebugMode)
		{
			Debug.Log(string.Concat(new object[]
			{
				this.ObjectiveName,
				" action report received: ",
				_Report.Action.ActionName,
				" on ",
				_Report.FromCard
			}));
		}
		if (_Report.FromCard != this.OnCard || _Report.Cancelled)
		{
			return;
		}
		if (_Report.ReportIsIdentical(this.LastActionReport))
		{
			if (this.DebugMode)
			{
				Debug.LogWarning(this.ObjectiveName + " we already got this report, we must have subscribed to the events multiple times");
				this.LastActionReport.CompareLog(_Report);
			}
			this.OnDisable();
			return;
		}
		this.LastActionReport = _Report;
		if (_Report.Action.ActionName.DefaultText == this.ActionName)
		{
			this.ActionCounter++;
			if (this.ActionCounter >= this.PerformMultipleTimes)
			{
				base.Complete = true;
				this.OnDisable();
			}
		}
	}

	// Token: 0x04000CE8 RID: 3304
	public CardData OnCard;

	// Token: 0x04000CE9 RID: 3305
	public string ActionName;

	// Token: 0x04000CEA RID: 3306
	public bool OnActionStarted;

	// Token: 0x04000CEB RID: 3307
	public int PerformMultipleTimes;

	// Token: 0x04000CEC RID: 3308
	public bool DebugMode;

	// Token: 0x04000CED RID: 3309
	private int ActionCounter;

	// Token: 0x04000CEE RID: 3310
	private ActionReport LastActionReport;
}
