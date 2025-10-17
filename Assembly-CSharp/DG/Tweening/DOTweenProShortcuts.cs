using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x020001FC RID: 508
	public static class DOTweenProShortcuts
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x0006CCC7 File Offset: 0x0006AEC7
		static DOTweenProShortcuts()
		{
			new SpiralPlugin();
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0006CCD0 File Offset: 0x0006AED0
		public static Tweener DOSpiral(this Transform target, float duration, Vector3? axis = null, SpiralMode mode = SpiralMode.Expand, float speed = 1f, float frequency = 10f, float depth = 0f, bool snapping = false)
		{
			if (Mathf.Approximately(speed, 0f))
			{
				speed = 1f;
			}
			if (axis != null)
			{
				Vector3? vector = axis;
				Vector3 zero = Vector3.zero;
				if (vector == null || (vector != null && !(vector.GetValueOrDefault() == zero)))
				{
					goto IL_66;
				}
			}
			axis = new Vector3?(Vector3.forward);
			IL_66:
			TweenerCore<Vector3, Vector3, SpiralOptions> tweenerCore = DOTween.To<Vector3, Vector3, SpiralOptions>(SpiralPlugin.Get(), () => target.localPosition, delegate(Vector3 x)
			{
				target.localPosition = x;
			}, axis.Value, duration).SetTarget(target);
			tweenerCore.plugOptions.mode = mode;
			tweenerCore.plugOptions.speed = speed;
			tweenerCore.plugOptions.frequency = frequency;
			tweenerCore.plugOptions.depth = depth;
			tweenerCore.plugOptions.snapping = snapping;
			return tweenerCore;
		}
	}
}
