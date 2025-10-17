using System;
using System.Collections.Generic;

// Token: 0x02000025 RID: 37
[Serializable]
public class BlueprintUsedIngredients
{
	// Token: 0x06000215 RID: 533 RVA: 0x0001520C File Offset: 0x0001340C
	public BlueprintUsedIngredients(BlueprintStage _Stage, CardData _Env)
	{
		this.Ingredients = new List<SimpleCardSaveData>();
		if (_Stage.RequiredElements == null)
		{
			return;
		}
		for (int i = 0; i < _Stage.RequiredElements.Length; i++)
		{
			if (!_Stage.RequiredElements[i].DontDestroy)
			{
				for (int j = 0; j < _Stage.RequiredElements[i].GetQuantity; j++)
				{
					this.Ingredients.Add(new SimpleCardSaveData());
					this.Ingredients[this.Ingredients.Count - 1].SaveCardModel(_Stage.RequiredElements[i].AnyCard, _Env, new SlotInfo(SlotsTypes.Blueprint, i));
				}
			}
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x000152CC File Offset: 0x000134CC
	public void Compare(BlueprintStage _ToStage)
	{
		if (this.Ingredients == null)
		{
			return;
		}
		for (int i = this.Ingredients.Count - 1; i >= 0; i--)
		{
			UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(this.Ingredients[i].CardID);
			bool flag = false;
			if (fromID is CardData)
			{
				CardData card = fromID as CardData;
				for (int j = 0; j < _ToStage.RequiredElements.Length; j++)
				{
					if (!_ToStage.RequiredElements[j].DontDestroy && _ToStage.RequiredElements[j].CompatibleCard(card, null))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.Ingredients.RemoveAt(i);
				}
			}
			else
			{
				this.Ingredients.RemoveAt(i);
			}
		}
	}

	// Token: 0x0400020E RID: 526
	public List<SimpleCardSaveData> Ingredients = new List<SimpleCardSaveData>();
}
