using System;
using System.Collections.Generic;

// Token: 0x02000126 RID: 294
[Serializable]
public class PlayerWounds
{
	// Token: 0x060008D6 RID: 2262 RVA: 0x00054F6C File Offset: 0x0005316C
	public void GetWoundsForSeverity(WoundSeverity _WoundSeverity, ref List<PlayerWound> _List)
	{
		switch (_WoundSeverity)
		{
		default:
			if (this.UnharmedResults != null)
			{
				_List.AddRange(this.UnharmedResults);
				return;
			}
			break;
		case WoundSeverity.Minor:
			if (this.MinorWounds != null)
			{
				_List.AddRange(this.MinorWounds);
				return;
			}
			break;
		case WoundSeverity.Medium:
			if (this.MediumWounds != null)
			{
				_List.AddRange(this.MediumWounds);
				return;
			}
			break;
		case WoundSeverity.Serious:
			if (this.SeriousWounds != null)
			{
				_List.AddRange(this.SeriousWounds);
			}
			break;
		}
	}

	// Token: 0x04000DDD RID: 3549
	public PlayerWound[] UnharmedResults;

	// Token: 0x04000DDE RID: 3550
	public PlayerWound[] MinorWounds;

	// Token: 0x04000DDF RID: 3551
	public PlayerWound[] MediumWounds;

	// Token: 0x04000DE0 RID: 3552
	public PlayerWound[] SeriousWounds;
}
