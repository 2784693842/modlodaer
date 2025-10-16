using System;
using UnityEngine;

namespace ChatTreeLoader
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class ModEncounterNode : ScriptableObject
	{
		// Token: 0x04000006 RID: 6
		public GeneralCondition Condition;

		// Token: 0x04000007 RID: 7
		public GeneralCondition ShowCondition;

		// Token: 0x04000008 RID: 8
		public bool EndNode;

		// Token: 0x04000009 RID: 9
		public bool BackNode;

		// Token: 0x0400000A RID: 10
		public LocalizedString PlayerText;

		// Token: 0x0400000B RID: 11
		public float PlayerTextDuration;

		// Token: 0x0400000C RID: 12
		public AudioClip PlayerAudio;

		// Token: 0x0400000D RID: 13
		public LocalizedString Title;

		// Token: 0x0400000E RID: 14
		public LocalizedString EnemyText;

		// Token: 0x0400000F RID: 15
		public float EnemyTextDuration;

		// Token: 0x04000010 RID: 16
		public AudioClip EnemyAudio;

		// Token: 0x04000011 RID: 17
		public ModEncounterNode[] ChildrenEncounterNodes;

		// Token: 0x04000012 RID: 18
		public CardAction NodeEffect;

		// Token: 0x04000013 RID: 19
		public bool HasNodeEffect;

		// Token: 0x04000014 RID: 20
		public bool DontShowEnd;
	}
}
