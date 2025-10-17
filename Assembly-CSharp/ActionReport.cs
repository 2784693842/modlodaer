using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public struct ActionReport
{
	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06000C8E RID: 3214 RVA: 0x000672A7 File Offset: 0x000654A7
	// (set) Token: 0x06000C8F RID: 3215 RVA: 0x000672AF File Offset: 0x000654AF
	public int StartedActionTick { get; private set; }

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06000C90 RID: 3216 RVA: 0x000672B8 File Offset: 0x000654B8
	// (set) Token: 0x06000C91 RID: 3217 RVA: 0x000672C0 File Offset: 0x000654C0
	public int StartedActionMiniTick { get; private set; }

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06000C92 RID: 3218 RVA: 0x000672C9 File Offset: 0x000654C9
	// (set) Token: 0x06000C93 RID: 3219 RVA: 0x000672D1 File Offset: 0x000654D1
	public int StartedActionCounter { get; private set; }

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06000C94 RID: 3220 RVA: 0x000672DA File Offset: 0x000654DA
	// (set) Token: 0x06000C95 RID: 3221 RVA: 0x000672E2 File Offset: 0x000654E2
	public int StartedActionFrameStamp { get; private set; }

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06000C96 RID: 3222 RVA: 0x000672EB File Offset: 0x000654EB
	// (set) Token: 0x06000C97 RID: 3223 RVA: 0x000672F3 File Offset: 0x000654F3
	public int EndedActionTick { get; private set; }

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000672FC File Offset: 0x000654FC
	// (set) Token: 0x06000C99 RID: 3225 RVA: 0x00067304 File Offset: 0x00065504
	public int EndedActionMiniTick { get; private set; }

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0006730D File Offset: 0x0006550D
	// (set) Token: 0x06000C9B RID: 3227 RVA: 0x00067315 File Offset: 0x00065515
	public int EndedActionCounter { get; private set; }

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06000C9C RID: 3228 RVA: 0x0006731E File Offset: 0x0006551E
	// (set) Token: 0x06000C9D RID: 3229 RVA: 0x00067326 File Offset: 0x00065526
	public int EndedActionFrameStamp { get; private set; }

	// Token: 0x06000C9E RID: 3230 RVA: 0x0006732F File Offset: 0x0006552F
	public void StartAction(CardAction _Action, CardData _FromCard, int _Tick, int _MiniTick, int _Counter, int _FrameStamp)
	{
		this.Action = _Action;
		this.FromCard = _FromCard;
		this.SetReportTicks(_Tick, _MiniTick, _Counter, _FrameStamp);
		this.StartedActionTick = _Tick;
		this.StartedActionMiniTick = _MiniTick;
		this.StartedActionCounter = _Counter;
		this.StartedActionFrameStamp = _FrameStamp;
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0006736B File Offset: 0x0006556B
	private void SetReportTicks(int _Tick, int _MiniTick, int _Counter, int _FrameStamp)
	{
		this.ReportTick = _Tick;
		this.ReportMiniTick = _MiniTick;
		this.ReportCounter = _Counter;
		this.ReportFrameStamp = _FrameStamp;
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0006738A File Offset: 0x0006558A
	public void EndAction(bool _Cancelled, int _Tick, int _MiniTick, int _Counter, int _FrameStamp)
	{
		this.Cancelled = _Cancelled;
		this.SetReportTicks(_Tick, _MiniTick, _Counter, _FrameStamp);
		this.EndedActionTick = _Tick;
		this.EndedActionMiniTick = _MiniTick;
		this.EndedActionCounter = _Counter;
		this.EndedActionFrameStamp = _FrameStamp;
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x000673C0 File Offset: 0x000655C0
	public bool IsEndingActionReport(ActionReport _Report)
	{
		return this.FromCard == _Report.FromCard && this.Action == _Report.Action && this.Cancelled == _Report.Cancelled && this.StartedActionTick == _Report.StartedActionTick && this.StartedActionMiniTick == _Report.StartedActionMiniTick && this.StartedActionCounter == _Report.StartedActionCounter && this.StartedActionFrameStamp == _Report.StartedActionFrameStamp;
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0006743C File Offset: 0x0006563C
	public bool ReportIsIdentical(ActionReport _To)
	{
		return this.FromCard == _To.FromCard && this.Action == _To.Action && this.Cancelled == _To.Cancelled && this.ReportTick == _To.ReportTick && this.ReportMiniTick == _To.ReportMiniTick && this.ReportCounter == _To.ReportCounter;
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x000674A4 File Offset: 0x000656A4
	public void CompareLog(ActionReport _To)
	{
		string str = (this.Action == null) ? "null" : this.Action.ActionName.DefaultText;
		string str2 = (_To.Action == null) ? "null" : _To.Action.ActionName.DefaultText;
		Debug.Log(string.Concat(new object[]
		{
			"From Card: ",
			this.FromCard,
			" - ",
			_To.FromCard
		}));
		Debug.Log("Action: " + str + " - " + str2);
		Debug.Log("Cancelled: " + this.Cancelled.ToString() + " - " + _To.Cancelled.ToString());
		Debug.Log(string.Concat(new object[]
		{
			"Action Tick: ",
			this.ReportTick,
			" - ",
			_To.ReportTick
		}));
		Debug.Log(string.Concat(new object[]
		{
			"Action Mini Tick: ",
			this.ReportMiniTick,
			" - ",
			_To.ReportMiniTick
		}));
		Debug.Log(string.Concat(new object[]
		{
			"Action Counter: ",
			this.ReportCounter,
			" - ",
			_To.ReportCounter
		}));
	}

	// Token: 0x04001177 RID: 4471
	public CardData FromCard;

	// Token: 0x04001178 RID: 4472
	public CardAction Action;

	// Token: 0x04001179 RID: 4473
	public bool Cancelled;

	// Token: 0x0400117A RID: 4474
	public int ReportTick;

	// Token: 0x0400117B RID: 4475
	public int ReportMiniTick;

	// Token: 0x0400117C RID: 4476
	public int ReportCounter;

	// Token: 0x0400117D RID: 4477
	public int ReportFrameStamp;
}
