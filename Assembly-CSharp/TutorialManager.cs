using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class TutorialManager : MBSingleton<TutorialManager>
{
	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000290 RID: 656 RVA: 0x00018F6F File Offset: 0x0001716F
	// (set) Token: 0x06000291 RID: 657 RVA: 0x00018F77 File Offset: 0x00017177
	public List<InGameCardBase> HighlightedCards { get; private set; }

	// Token: 0x06000292 RID: 658 RVA: 0x00018F80 File Offset: 0x00017180
	public bool IsActionHighlighted(ScriptableObject _FromObject, CardAction _Action)
	{
		for (int i = 0; i < this.Highlights.Length; i++)
		{
			if (this.Highlights[i].Conditions.ConditionsValid(MBSingleton<GameManager>.Instance.NotInBase, null))
			{
				for (int j = 0; j < this.Highlights[i].HighlightedActions.Length; j++)
				{
					if (this.Highlights[i].HighlightedActions[j].IsActionHighlighted(_FromObject, _Action))
					{
						return true;
					}
				}
			}
		}
		for (int k = 0; k < this.TutorialSteps.Length; k++)
		{
			if (this.TutorialSteps[k].Active && this.TutorialSteps[k].ActionsHighlighted != null && this.TutorialSteps[k].ActionsHighlighted.Length != 0)
			{
				for (int l = 0; l < this.TutorialSteps[k].ActionsHighlighted.Length; l++)
				{
					if (this.TutorialSteps[k].ActionsHighlighted[l].IsActionHighlighted(_FromObject, _Action))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00019074 File Offset: 0x00017274
	public void UpdateTutorials()
	{
		if (this.HighlightedCards == null)
		{
			this.HighlightedCards = new List<InGameCardBase>();
		}
		else
		{
			for (int i = 0; i < this.HighlightedCards.Count; i++)
			{
				if (this.HighlightedCards[i].CardVisuals)
				{
					this.HighlightedCards[i].SetTutorialHighlight(TutorialHighlightState.NotHighlighted);
				}
			}
			this.HighlightedCards.Clear();
		}
		for (int j = 0; j < this.TutorialSteps.Length; j++)
		{
			this.TutorialSteps[j].UpdateState();
			for (int k = 0; k < this.TutorialSteps[j].ActiveObjects.Length; k++)
			{
				this.TutorialSteps[j].ActiveObjects[k].SetActive(this.TutorialSteps[j].Active);
			}
			if (this.TutorialSteps[j].Active)
			{
				for (int l = 0; l < this.TutorialSteps[j].ActionsHighlighted.Length; l++)
				{
					ScriptableObject[] targetObjects = this.TutorialSteps[j].ActionsHighlighted[l].TargetObjects;
					if (targetObjects != null)
					{
						for (int m = 0; m < targetObjects.Length; m++)
						{
							if (targetObjects[m] is CardData)
							{
								CardData card = targetObjects[m] as CardData;
								MBSingleton<GameManager>.Instance.CardIsOnBoard(card, true, true, false, false, this.HighlightedCards, Array.Empty<InGameCardBase>());
							}
						}
					}
				}
			}
		}
		int count = this.HighlightedCards.Count;
		for (int n = 0; n < this.HighlightedCards.Count; n++)
		{
			if (this.HighlightedCards[n].CardVisuals)
			{
				this.HighlightedCards[n].SetTutorialHighlight(TutorialHighlightState.HighlightedWithArrow);
			}
		}
		for (int num = 0; num < this.Highlights.Length; num++)
		{
			bool flag = this.Highlights[num].Conditions.ConditionsValid(MBSingleton<GameManager>.Instance.NotInBase, null) && !this.Highlights[num].Inactive;
			for (int num2 = 0; num2 < this.Highlights[num].HighlightedObjects.Length; num2++)
			{
				if (this.Highlights[num].HighlightedObjects[num2])
				{
					this.Highlights[num].HighlightedObjects[num2].SetActive(flag);
				}
			}
			if (flag)
			{
				for (int num3 = 0; num3 < this.Highlights[num].HighlightedActions.Length; num3++)
				{
					ScriptableObject[] targetObjects = this.Highlights[num].HighlightedActions[num3].TargetObjects;
					if (targetObjects != null)
					{
						for (int num4 = 0; num4 < targetObjects.Length; num4++)
						{
							if (targetObjects[num4] is CardData)
							{
								CardData card = targetObjects[num4] as CardData;
								MBSingleton<GameManager>.Instance.CardIsOnBoard(card, true, true, false, false, this.HighlightedCards, Array.Empty<InGameCardBase>());
							}
						}
					}
				}
			}
		}
		if (count < this.HighlightedCards.Count)
		{
			for (int num5 = count; num5 < this.HighlightedCards.Count; num5++)
			{
				if (this.HighlightedCards[num5].CardVisuals)
				{
					this.HighlightedCards[num5].SetTutorialHighlight(TutorialHighlightState.Highlighted);
				}
			}
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x000193B2 File Offset: 0x000175B2
	public void SetHighlightActive(string _Highlight)
	{
		this.SetHighlightActive(_Highlight, true);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x000193BC File Offset: 0x000175BC
	public void SetHighlightInactive(string _Highlight)
	{
		this.SetHighlightActive(_Highlight, false);
	}

	// Token: 0x06000296 RID: 662 RVA: 0x000193C8 File Offset: 0x000175C8
	public void SetHighlightActive(string _HighlightName, bool _Active)
	{
		if (string.IsNullOrEmpty(_HighlightName))
		{
			return;
		}
		for (int i = 0; i < this.Highlights.Length; i++)
		{
			if (this.Highlights[i].Inactive != !_Active && this.Highlights[i].HighlightName == _HighlightName)
			{
				this.Highlights[i].Inactive = true;
			}
		}
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00019428 File Offset: 0x00017628
	public void UseHighlight(string _HighlightName)
	{
		for (int i = 0; i < this.Highlights.Length; i++)
		{
			if (!this.Highlights[i].Inactive && this.Highlights[i].HighlightName == _HighlightName && this.Highlights[i].Conditions.ConditionsValid(MBSingleton<GameManager>.Instance.NotInBase, null))
			{
				this.Highlights[i].Inactive = true;
			}
		}
	}

	// Token: 0x040002E1 RID: 737
	[SpecialHeader("Tutorial", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	[SerializeField]
	private TutorialStep[] TutorialSteps;

	// Token: 0x040002E2 RID: 738
	[SpecialHeader("Highlights", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	[SerializeField]
	private PassiveHighlights[] Highlights;
}
