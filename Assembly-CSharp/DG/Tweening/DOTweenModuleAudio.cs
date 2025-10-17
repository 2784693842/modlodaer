using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Audio;

namespace DG.Tweening
{
	// Token: 0x020001F3 RID: 499
	public static class DOTweenModuleAudio
	{
		// Token: 0x06000D4A RID: 3402 RVA: 0x0006A1D4 File Offset: 0x000683D4
		public static TweenerCore<float, float, FloatOptions> DOFade(this AudioSource target, float endValue, float duration)
		{
			if (endValue < 0f)
			{
				endValue = 0f;
			}
			else if (endValue > 1f)
			{
				endValue = 1f;
			}
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.volume, delegate(float x)
			{
				target.volume = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0006A23C File Offset: 0x0006843C
		public static TweenerCore<float, float, FloatOptions> DOPitch(this AudioSource target, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.pitch, delegate(float x)
			{
				target.pitch = x;
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0006A284 File Offset: 0x00068484
		public static TweenerCore<float, float, FloatOptions> DOSetFloat(this AudioMixer target, string floatName, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(delegate()
			{
				float result;
				target.GetFloat(floatName, out result);
				return result;
			}, delegate(float x)
			{
				target.SetFloat(floatName, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0006A2D1 File Offset: 0x000684D1
		public static int DOComplete(this AudioMixer target, bool withCallbacks = false)
		{
			return DOTween.Complete(target, withCallbacks);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0006A2DA File Offset: 0x000684DA
		public static int DOKill(this AudioMixer target, bool complete = false)
		{
			return DOTween.Kill(target, complete);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0006A2E3 File Offset: 0x000684E3
		public static int DOFlip(this AudioMixer target)
		{
			return DOTween.Flip(target);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0006A2EB File Offset: 0x000684EB
		public static int DOGoto(this AudioMixer target, float to, bool andPlay = false)
		{
			return DOTween.Goto(target, to, andPlay);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0006A2F5 File Offset: 0x000684F5
		public static int DOPause(this AudioMixer target)
		{
			return DOTween.Pause(target);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0006A2FD File Offset: 0x000684FD
		public static int DOPlay(this AudioMixer target)
		{
			return DOTween.Play(target);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0006A305 File Offset: 0x00068505
		public static int DOPlayBackwards(this AudioMixer target)
		{
			return DOTween.PlayBackwards(target);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0006A30D File Offset: 0x0006850D
		public static int DOPlayForward(this AudioMixer target)
		{
			return DOTween.PlayForward(target);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0006A315 File Offset: 0x00068515
		public static int DORestart(this AudioMixer target)
		{
			return DOTween.Restart(target, true, -1f);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0006A323 File Offset: 0x00068523
		public static int DORewind(this AudioMixer target)
		{
			return DOTween.Rewind(target, true);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0006A32C File Offset: 0x0006852C
		public static int DOSmoothRewind(this AudioMixer target)
		{
			return DOTween.SmoothRewind(target);
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0006A334 File Offset: 0x00068534
		public static int DOTogglePause(this AudioMixer target)
		{
			return DOTween.TogglePause(target);
		}
	}
}
