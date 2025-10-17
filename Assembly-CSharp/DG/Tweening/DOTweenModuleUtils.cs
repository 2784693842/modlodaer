using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Scripting;

namespace DG.Tweening
{
	// Token: 0x020001F9 RID: 505
	public static class DOTweenModuleUtils
	{
		// Token: 0x06000D94 RID: 3476 RVA: 0x0006B915 File Offset: 0x00069B15
		[Preserve]
		public static void Init()
		{
			if (DOTweenModuleUtils._initialized)
			{
				return;
			}
			DOTweenModuleUtils._initialized = true;
			DOTweenExternalCommand.SetOrientationOnPath += DOTweenModuleUtils.Physics.SetOrientationOnPath;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0006B936 File Offset: 0x00069B36
		[Preserve]
		private static void Preserver()
		{
			AppDomain.CurrentDomain.GetAssemblies();
			typeof(MonoBehaviour).GetMethod("Stub");
		}

		// Token: 0x040011DE RID: 4574
		private static bool _initialized;

		// Token: 0x020002EA RID: 746
		public static class Physics
		{
			// Token: 0x06001185 RID: 4485 RVA: 0x00083350 File Offset: 0x00081550
			public static void SetOrientationOnPath(PathOptions options, Tween t, Quaternion newRot, Transform trans)
			{
				trans.rotation = newRot;
			}

			// Token: 0x06001186 RID: 4486 RVA: 0x00083359 File Offset: 0x00081559
			public static bool HasRigidbody2D(Component target)
			{
				return target.GetComponent<Rigidbody2D>() != null;
			}

			// Token: 0x06001187 RID: 4487 RVA: 0x00052B1A File Offset: 0x00050D1A
			[Preserve]
			public static bool HasRigidbody(Component target)
			{
				return false;
			}

			// Token: 0x06001188 RID: 4488 RVA: 0x00083367 File Offset: 0x00081567
			[Preserve]
			public static TweenerCore<Vector3, Path, PathOptions> CreateDOTweenPathTween(MonoBehaviour target, bool tweenRigidbody, bool isLocal, Path path, float duration, PathMode pathMode)
			{
				if (!isLocal)
				{
					return target.transform.DOPath(path, duration, pathMode);
				}
				return target.transform.DOLocalPath(path, duration, pathMode);
			}
		}
	}
}
