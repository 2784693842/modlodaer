using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
[Serializable]
public struct CardOrTagRef
{
	// Token: 0x060009F8 RID: 2552 RVA: 0x00059F92 File Offset: 0x00058192
	public static bool IsValidTarget(ScriptableObject _Target)
	{
		return _Target && (_Target is CardData || _Target is CardTag);
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00059FB4 File Offset: 0x000581B4
	public bool CheckCard(InGameCardBase _Card)
	{
		if (!this.IsValid || !_Card)
		{
			return false;
		}
		if (!_Card.CardModel)
		{
			return false;
		}
		if (this.TargetIsTag)
		{
			return _Card.CardModel.HasTag(this.Tag);
		}
		return _Card.CardModel == this.Card;
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0005A00D File Offset: 0x0005820D
	public bool CheckCard(CardData _Card)
	{
		if (!this.IsValid || !_Card)
		{
			return false;
		}
		if (this.TargetIsTag)
		{
			return _Card.HasTag(this.Tag);
		}
		return _Card == this.Card;
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x0005A042 File Offset: 0x00058242
	public bool IsValid
	{
		get
		{
			return this.Target && (this.Target is CardData || this.Target is CardTag);
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x0005A070 File Offset: 0x00058270
	public bool TargetIsTag
	{
		get
		{
			return this.Target && this.Target is CardTag;
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x060009FD RID: 2557 RVA: 0x0005A08F File Offset: 0x0005828F
	public bool TargetIsCard
	{
		get
		{
			return this.Target && this.Target is CardData;
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x0005A0AE File Offset: 0x000582AE
	public CardData Card
	{
		get
		{
			if (this.TargetIsCard)
			{
				return this.Target as CardData;
			}
			return null;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x0005A0C5 File Offset: 0x000582C5
	public CardTag Tag
	{
		get
		{
			if (this.TargetIsTag)
			{
				return this.Target as CardTag;
			}
			return null;
		}
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0005A0DC File Offset: 0x000582DC
	public string TargetName
	{
		get
		{
			if (!this.Target)
			{
				return "";
			}
			if (!this.TargetIsTag && !this.TargetIsCard)
			{
				return this.Target.name;
			}
			if (this.TargetIsTag)
			{
				return this.Tag.InGameName;
			}
			return this.Card.CardName;
		}
	}

	// Token: 0x04000F71 RID: 3953
	public ScriptableObject Target;
}
