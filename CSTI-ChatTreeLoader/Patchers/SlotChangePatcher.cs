using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChatTreeLoader.Patchers
{
	// Token: 0x02000010 RID: 16
	public static class SlotChangePatcher
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000279C File Offset: 0x0000099C
		public static void MoreLineGetElementPosition(DynamicViewLayoutGroup __instance, int _Index, ref Vector3 __result)
		{
			if (__instance != MBSingleton<GraphicsManager>.Instance.BaseSlotsLine)
			{
				return;
			}
			List<DynamicViewExtraSpace> extraSpaces = __instance.ExtraSpaces;
			if (extraSpaces == null || extraSpaces.Count <= 0)
			{
				return;
			}
			float num = 0f;
			foreach (DynamicViewExtraSpace dynamicViewExtraSpace in extraSpaces)
			{
				if (dynamicViewExtraSpace.AtIndex <= _Index)
				{
					num += dynamicViewExtraSpace.Space;
				}
			}
			__result = __instance.LayoutOriginPos + __instance.LayoutDirection * (__instance.Spacing * (float)(_Index / 2) + num) + new Vector2(0f, -80f) * (float)(_Index % 2) + new Vector2(0f, 30f);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002884 File Offset: 0x00000A84
		public static void CalculateSizeAndProperties(DynamicViewLayoutGroup __instance)
		{
			__instance.Size = __instance.Spacing * 0.5f * (float)__instance.AllElements.Count - __instance.Spacing * 0.5f * (float)__instance.InactiveElements;
			if (__instance.ExtraSpaces != null)
			{
				foreach (DynamicViewExtraSpace dynamicViewExtraSpace in __instance.ExtraSpaces)
				{
					__instance.Size += dynamicViewExtraSpace.Space;
				}
			}
			if (__instance.AddedSize && __instance.AddedSize.gameObject.activeSelf)
			{
				if (__instance.LayoutOrientation == RectTransform.Axis.Horizontal)
				{
					__instance.Size += __instance.AddedSize.rect.width;
				}
				else
				{
					__instance.Size += __instance.AddedSize.rect.height;
				}
			}
			if (__instance.LayoutOrientation == RectTransform.Axis.Horizontal)
			{
				__instance.Size += (float)(__instance.Padding.left + __instance.Padding.right);
			}
			else
			{
				__instance.Size += (float)(__instance.Padding.top + __instance.Padding.right);
			}
			__instance.Size = Mathf.Max(__instance.Size, __instance.MinSize);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000029F4 File Offset: 0x00000BF4
		public static void SetSizeWithCurrentAnchors(RectTransform __instance, ref float size)
		{
			if (!SlotChangePatcher.OnCalSize)
			{
				return;
			}
			if (__instance != MBSingleton<GraphicsManager>.Instance.BaseSlotsLine.RectTr)
			{
				return;
			}
			Debug.Log(string.Format("set __ {0}", size));
			size -= 0.5f * (MBSingleton<GraphicsManager>.Instance.BaseSlotsLine.Spacing * (float)MBSingleton<GraphicsManager>.Instance.BaseSlotsLine.AllElements.Count - MBSingleton<GraphicsManager>.Instance.BaseSlotsLine.Spacing * (float)MBSingleton<GraphicsManager>.Instance.BaseSlotsLine.InactiveElements);
		}

		// Token: 0x04000030 RID: 48
		private static bool OnCalSize;
	}
}
