using System;
using System.Collections.Generic;
using ChatTreeLoader.Attr;
using UnityEngine;

namespace ChatTreeLoader.ScriptObjects
{
	// Token: 0x02000007 RID: 7
	public class ExtraStatTable : ScriptableObject
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000021B0 File Offset: 0x000003B0
		private void OnEnable()
		{
			ExtraStatTable.AllTables.Add(this);
		}

		// Token: 0x04000015 RID: 21
		public static readonly List<ExtraStatTable> AllTables = new List<ExtraStatTable>();

		// Token: 0x04000016 RID: 22
		[Note("场景绑定状态")]
		[NoteEn("env binding gameStat")]
		public List<GameStat> EnvBindStats = new List<GameStat>();

		// Token: 0x04000017 RID: 23
		[Note("卡牌绑定状态")]
		[NoteEn("card binding gameStat")]
		public List<GameStat> CardBindStats = new List<GameStat>();

		// Token: 0x04000018 RID: 24
		[Note("需要绑定的卡牌")]
		[NoteEn("Cards that need to be bound")]
		public List<CardData> CardBindCards = new List<CardData>();
	}
}
