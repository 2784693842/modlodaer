using System;
using System.Collections.Generic;

// Token: 0x02000107 RID: 263
[Serializable]
public class CardOnBoardSubObjective : SubObjective
{
	// Token: 0x0600089D RID: 2205 RVA: 0x00053954 File Offset: 0x00051B54
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
			if (MBSingleton<GameManager>.Instance.CurrentEnvironment != this.OnlyInEnvironment)
			{
				return;
			}
		}
		if (CardOnBoardSubObjective.CardsOnBoard == null)
		{
			CardOnBoardSubObjective.CardsOnBoard = new List<InGameCardBase>();
		}
		else
		{
			CardOnBoardSubObjective.CardsOnBoard.Clear();
		}
		if (MBSingleton<GameManager>.Instance.CardIsOnBoard(this.Card, true, true, false, false, CardOnBoardSubObjective.CardsOnBoard, Array.Empty<InGameCardBase>()))
		{
			base.Complete = (this.Quantity <= CardOnBoardSubObjective.CardsOnBoard.Count);
		}
		else
		{
			base.Complete = (this.Quantity <= 0);
		}
		this.PresentOnBoard = CardOnBoardSubObjective.CardsOnBoard.Count;
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00053A28 File Offset: 0x00051C28
	protected override void LoadSaveCounter(int _Counter)
	{
		base.LoadSaveCounter(_Counter);
		this.PresentOnBoard = _Counter;
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00053A38 File Offset: 0x00051C38
	protected override int GetSaveCounter()
	{
		return this.PresentOnBoard;
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00053A40 File Offset: 0x00051C40
	public override float GetCompletion()
	{
		if (this.Quantity <= 1 || base.Complete)
		{
			return base.GetCompletion();
		}
		return (float)this.CompletionWeight * ((float)this.PresentOnBoard / (float)this.Quantity);
	}

	// Token: 0x04000CEF RID: 3311
	public CardData Card;

	// Token: 0x04000CF0 RID: 3312
	public int Quantity;

	// Token: 0x04000CF1 RID: 3313
	public bool ConstantlyChecking;

	// Token: 0x04000CF2 RID: 3314
	public CardData OnlyInEnvironment;

	// Token: 0x04000CF3 RID: 3315
	private static List<InGameCardBase> CardsOnBoard = new List<InGameCardBase>();

	// Token: 0x04000CF4 RID: 3316
	private int PresentOnBoard;
}
