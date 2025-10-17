using System;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x020001F8 RID: 504
	public static class DOTweenCYInstruction
	{
		// Token: 0x020002E4 RID: 740
		public class WaitForCompletion : CustomYieldInstruction
		{
			// Token: 0x170003E2 RID: 994
			// (get) Token: 0x06001179 RID: 4473 RVA: 0x000831FA File Offset: 0x000813FA
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && !this.t.IsComplete();
				}
			}

			// Token: 0x0600117A RID: 4474 RVA: 0x00083219 File Offset: 0x00081419
			public WaitForCompletion(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x040015E2 RID: 5602
			private readonly Tween t;
		}

		// Token: 0x020002E5 RID: 741
		public class WaitForRewind : CustomYieldInstruction
		{
			// Token: 0x170003E3 RID: 995
			// (get) Token: 0x0600117B RID: 4475 RVA: 0x00083228 File Offset: 0x00081428
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && (!this.t.playedOnce || this.t.position * (float)(this.t.CompletedLoops() + 1) > 0f);
				}
			}

			// Token: 0x0600117C RID: 4476 RVA: 0x00083274 File Offset: 0x00081474
			public WaitForRewind(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x040015E3 RID: 5603
			private readonly Tween t;
		}

		// Token: 0x020002E6 RID: 742
		public class WaitForKill : CustomYieldInstruction
		{
			// Token: 0x170003E4 RID: 996
			// (get) Token: 0x0600117D RID: 4477 RVA: 0x00083283 File Offset: 0x00081483
			public override bool keepWaiting
			{
				get
				{
					return this.t.active;
				}
			}

			// Token: 0x0600117E RID: 4478 RVA: 0x00083290 File Offset: 0x00081490
			public WaitForKill(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x040015E4 RID: 5604
			private readonly Tween t;
		}

		// Token: 0x020002E7 RID: 743
		public class WaitForElapsedLoops : CustomYieldInstruction
		{
			// Token: 0x170003E5 RID: 997
			// (get) Token: 0x0600117F RID: 4479 RVA: 0x0008329F File Offset: 0x0008149F
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && this.t.CompletedLoops() < this.elapsedLoops;
				}
			}

			// Token: 0x06001180 RID: 4480 RVA: 0x000832C3 File Offset: 0x000814C3
			public WaitForElapsedLoops(Tween tween, int elapsedLoops)
			{
				this.t = tween;
				this.elapsedLoops = elapsedLoops;
			}

			// Token: 0x040015E5 RID: 5605
			private readonly Tween t;

			// Token: 0x040015E6 RID: 5606
			private readonly int elapsedLoops;
		}

		// Token: 0x020002E8 RID: 744
		public class WaitForPosition : CustomYieldInstruction
		{
			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x06001181 RID: 4481 RVA: 0x000832D9 File Offset: 0x000814D9
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && this.t.position * (float)(this.t.CompletedLoops() + 1) < this.position;
				}
			}

			// Token: 0x06001182 RID: 4482 RVA: 0x0008330C File Offset: 0x0008150C
			public WaitForPosition(Tween tween, float position)
			{
				this.t = tween;
				this.position = position;
			}

			// Token: 0x040015E7 RID: 5607
			private readonly Tween t;

			// Token: 0x040015E8 RID: 5608
			private readonly float position;
		}

		// Token: 0x020002E9 RID: 745
		public class WaitForStart : CustomYieldInstruction
		{
			// Token: 0x170003E7 RID: 999
			// (get) Token: 0x06001183 RID: 4483 RVA: 0x00083322 File Offset: 0x00081522
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && !this.t.playedOnce;
				}
			}

			// Token: 0x06001184 RID: 4484 RVA: 0x00083341 File Offset: 0x00081541
			public WaitForStart(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x040015E9 RID: 5609
			private readonly Tween t;
		}
	}
}
