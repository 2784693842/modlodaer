using System;
using System.Collections.Generic;

// Token: 0x02000108 RID: 264
[Serializable]
public class TagOnBoardSubObjective : SubObjective
{
	// Token: 0x060008A3 RID: 2211 RVA: 0x00053A80 File Offset: 0x00051C80
	public void CheckForCompletion()
	{
		if (base.Complete && (!this.ConstantlyChecking || this.ForceCompleted))
		{
			return;
		}
		if (this.OnlyInEnvironment)
		{
			if (!MBSingleton<GameManager>.Instance)
			{
				return;
			}
			if (MBSingleton<GameManager>.Instance.CurrentEnvironment != this.OnlyInEnvironment || MBSingleton<GameManager>.Instance.LeavingEnvironment)
			{
				return;
			}
		}
		if (TagOnBoardSubObjective.CardsOnBoard == null)
		{
			TagOnBoardSubObjective.CardsOnBoard = new List<InGameCardBase>();
		}
		else
		{
			TagOnBoardSubObjective.CardsOnBoard.Clear();
		}
		if (MBSingleton<GameManager>.Instance.TagIsOnBoard(this.Tag, true, true, false, false, TagOnBoardSubObjective.CardsOnBoard))
		{
			base.Complete = (this.Quantity <= TagOnBoardSubObjective.CardsOnBoard.Count);
		}
		else
		{
			base.Complete = (this.Quantity <= 0);
		}
		this.PresentOnBoard = TagOnBoardSubObjective.CardsOnBoard.Count;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00053B5B File Offset: 0x00051D5B
	protected override void LoadSaveCounter(int _Counter)
	{
		base.LoadSaveCounter(_Counter);
		this.PresentOnBoard = _Counter;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00053B6B File Offset: 0x00051D6B
	protected override int GetSaveCounter()
	{
		return this.PresentOnBoard;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00053B73 File Offset: 0x00051D73
	public override float GetCompletion()
	{
		if (this.Quantity <= 1 || base.Complete)
		{
			return base.GetCompletion();
		}
		return (float)this.CompletionWeight * ((float)this.PresentOnBoard / (float)this.Quantity);
	}

	// Token: 0x04000CF5 RID: 3317
	public CardTag Tag;

	// Token: 0x04000CF6 RID: 3318
	public int Quantity;

	// Token: 0x04000CF7 RID: 3319
	public bool ConstantlyChecking;

	// Token: 0x04000CF8 RID: 3320
	public CardData OnlyInEnvironment;

	// Token: 0x04000CF9 RID: 3321
	private static List<InGameCardBase> CardsOnBoard = new List<InGameCardBase>();

	// Token: 0x04000CFA RID: 3322
	private int PresentOnBoard;
}
