using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x020001FD RID: 509
	public static class ShortcutExtensionsTMPText
	{
		// Token: 0x06000DBE RID: 3518 RVA: 0x0006CDB8 File Offset: 0x0006AFB8
		public static TweenerCore<Color, Color, ColorOptions> DOColor(this TMP_Text target, Color endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0006CE00 File Offset: 0x0006B000
		public static TweenerCore<Color, Color, ColorOptions> DOFaceColor(this TMP_Text target, Color32 endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.faceColor, delegate(Color x)
			{
				target.faceColor = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0006CE4C File Offset: 0x0006B04C
		public static TweenerCore<Color, Color, ColorOptions> DOOutlineColor(this TMP_Text target, Color32 endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(() => target.outlineColor, delegate(Color x)
			{
				target.outlineColor = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0006CE97 File Offset: 0x0006B097
		public static TweenerCore<Color, Color, ColorOptions> DOGlowColor(this TMP_Text target, Color endValue, float duration, bool useSharedMaterial = false)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = useSharedMaterial ? target.fontSharedMaterial.DOColor(endValue, "_GlowColor", duration) : target.fontMaterial.DOColor(endValue, "_GlowColor", duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0006CECC File Offset: 0x0006B0CC
		public static TweenerCore<Color, Color, ColorOptions> DOFade(this TMP_Text target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.color, delegate(Color x)
			{
				target.color = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0006CF14 File Offset: 0x0006B114
		public static TweenerCore<Color, Color, ColorOptions> DOFaceFade(this TMP_Text target, float endValue, float duration)
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.ToAlpha(() => target.faceColor, delegate(Color x)
			{
				target.faceColor = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0006CF5C File Offset: 0x0006B15C
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this TMP_Text target, float endValue, float duration)
		{
			Transform trans = target.transform;
			Vector3 endValue2 = new Vector3(endValue, endValue, endValue);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => trans.localScale, delegate(Vector3 x)
			{
				trans.localScale = x;
			}, endValue2, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0006CFAC File Offset: 0x0006B1AC
		public static TweenerCore<float, float, FloatOptions> DOFontSize(this TMP_Text target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.fontSize, delegate(float x)
			{
				target.fontSize = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0006CFF4 File Offset: 0x0006B1F4
		public static TweenerCore<int, int, NoOptions> DOMaxVisibleCharacters(this TMP_Text target, int endValue, float duration)
		{
			TweenerCore<int, int, NoOptions> tweenerCore = DOTween.To(() => target.maxVisibleCharacters, delegate(int x)
			{
				target.maxVisibleCharacters = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0006D03C File Offset: 0x0006B23C
		public static TweenerCore<string, string, StringOptions> DOText(this TMP_Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
		{
			TweenerCore<string, string, StringOptions> tweenerCore = DOTween.To(() => target.text, delegate(string x)
			{
				target.text = x;
			}, endValue, duration);
			tweenerCore.SetOptions(richTextEnabled, scrambleMode, scrambleChars).SetTarget(target);
			return tweenerCore;
		}
	}
}
