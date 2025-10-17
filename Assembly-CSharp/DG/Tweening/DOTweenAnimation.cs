using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
	// Token: 0x020001FA RID: 506
	[AddComponentMenu("DOTween/DOTween Animation")]
	public class DOTweenAnimation : ABSAnimationComponent
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000D96 RID: 3478 RVA: 0x0006B958 File Offset: 0x00069B58
		// (remove) Token: 0x06000D97 RID: 3479 RVA: 0x0006B98C File Offset: 0x00069B8C
		public static event Action<DOTweenAnimation> OnReset;

		// Token: 0x06000D98 RID: 3480 RVA: 0x0006B9BF File Offset: 0x00069BBF
		private static void Dispatch_OnReset(DOTweenAnimation anim)
		{
			if (DOTweenAnimation.OnReset != null)
			{
				DOTweenAnimation.OnReset(anim);
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0006B9D3 File Offset: 0x00069BD3
		private void Awake()
		{
			if (!this.isActive || !this.isValid)
			{
				return;
			}
			if (this.animationType != DOTweenAnimation.AnimationType.Move || !this.useTargetAsV3)
			{
				this.CreateTween();
				this._tweenCreated = true;
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0006BA04 File Offset: 0x00069C04
		private void Start()
		{
			if (this._tweenCreated || !this.isActive || !this.isValid)
			{
				return;
			}
			this.CreateTween();
			this._tweenCreated = true;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0006BA2C File Offset: 0x00069C2C
		private void Reset()
		{
			DOTweenAnimation.Dispatch_OnReset(this);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0006BA34 File Offset: 0x00069C34
		private void OnDestroy()
		{
			if (this.tween != null && this.tween.IsActive())
			{
				this.tween.Kill(true);
			}
			this.tween = null;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0006BA60 File Offset: 0x00069C60
		public void CreateTween()
		{
			GameObject tweenGO = this.GetTweenGO();
			if (this.target == null || tweenGO == null)
			{
				if (this.targetIsSelf && this.target == null)
				{
					Debug.LogWarning(string.Format("{0} :: This DOTweenAnimation's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", base.gameObject.name), base.gameObject);
					return;
				}
				Debug.LogWarning(string.Format("{0} :: This DOTweenAnimation's target/GameObject is unset: the tween will not be created.", base.gameObject.name), base.gameObject);
				return;
			}
			else
			{
				if (this.forcedTargetType != DOTweenAnimation.TargetType.Unset)
				{
					this.targetType = this.forcedTargetType;
				}
				if (this.targetType == DOTweenAnimation.TargetType.Unset)
				{
					this.targetType = DOTweenAnimation.TypeToDOTargetType(this.target.GetType());
				}
				switch (this.animationType)
				{
				case DOTweenAnimation.AnimationType.Move:
					if (this.useTargetAsV3)
					{
						this.isRelative = false;
						if (this.endValueTransform == null)
						{
							Debug.LogWarning(string.Format("{0} :: This tween's TO target is NULL, a Vector3 of (0,0,0) will be used instead", base.gameObject.name), base.gameObject);
							this.endValueV3 = Vector3.zero;
						}
						else if (this.targetType == DOTweenAnimation.TargetType.RectTransform)
						{
							RectTransform rectTransform = this.endValueTransform as RectTransform;
							if (rectTransform == null)
							{
								Debug.LogWarning(string.Format("{0} :: This tween's TO target should be a RectTransform, a Vector3 of (0,0,0) will be used instead", base.gameObject.name), base.gameObject);
								this.endValueV3 = Vector3.zero;
							}
							else
							{
								RectTransform rectTransform2 = this.target as RectTransform;
								if (rectTransform2 == null)
								{
									Debug.LogWarning(string.Format("{0} :: This tween's target and TO target are not of the same type. Please reassign the values", base.gameObject.name), base.gameObject);
								}
								else
								{
									this.endValueV3 = DOTweenModuleUI.Utils.SwitchToRectTransform(rectTransform, rectTransform2);
								}
							}
						}
						else
						{
							this.endValueV3 = this.endValueTransform.position;
						}
					}
					switch (this.targetType)
					{
					case DOTweenAnimation.TargetType.RectTransform:
						this.tween = ((RectTransform)this.target).DOAnchorPos3D(this.endValueV3, this.duration, this.optionalBool0);
						break;
					case DOTweenAnimation.TargetType.Rigidbody:
						this.tween = ((Transform)this.target).DOMove(this.endValueV3, this.duration, this.optionalBool0);
						break;
					case DOTweenAnimation.TargetType.Rigidbody2D:
						this.tween = ((Rigidbody2D)this.target).DOMove(this.endValueV3, this.duration, this.optionalBool0);
						break;
					case DOTweenAnimation.TargetType.Transform:
						this.tween = ((Transform)this.target).DOMove(this.endValueV3, this.duration, this.optionalBool0);
						break;
					}
					break;
				case DOTweenAnimation.AnimationType.LocalMove:
					this.tween = tweenGO.transform.DOLocalMove(this.endValueV3, this.duration, this.optionalBool0);
					break;
				case DOTweenAnimation.AnimationType.Rotate:
					switch (this.targetType)
					{
					case DOTweenAnimation.TargetType.Rigidbody:
						this.tween = ((Transform)this.target).DORotate(this.endValueV3, this.duration, this.optionalRotationMode);
						break;
					case DOTweenAnimation.TargetType.Rigidbody2D:
						this.tween = ((Rigidbody2D)this.target).DORotate(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Transform:
						this.tween = ((Transform)this.target).DORotate(this.endValueV3, this.duration, this.optionalRotationMode);
						break;
					}
					break;
				case DOTweenAnimation.AnimationType.LocalRotate:
					this.tween = tweenGO.transform.DOLocalRotate(this.endValueV3, this.duration, this.optionalRotationMode);
					break;
				case DOTweenAnimation.AnimationType.Scale:
				{
					DOTweenAnimation.TargetType targetType = this.targetType;
					this.tween = tweenGO.transform.DOScale(this.optionalBool0 ? new Vector3(this.endValueFloat, this.endValueFloat, this.endValueFloat) : this.endValueV3, this.duration);
					break;
				}
				case DOTweenAnimation.AnimationType.Color:
					this.isRelative = false;
					switch (this.targetType)
					{
					case DOTweenAnimation.TargetType.Image:
						this.tween = ((Graphic)this.target).DOColor(this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.Light:
						this.tween = ((Light)this.target).DOColor(this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.Renderer:
						this.tween = ((Renderer)this.target).material.DOColor(this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.SpriteRenderer:
						this.tween = ((SpriteRenderer)this.target).DOColor(this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.Text:
						this.tween = ((Text)this.target).DOColor(this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.TextMeshPro:
						this.tween = ((TextMeshPro)this.target).DOColor(this.endValueColor, this.duration);
						break;
					case DOTweenAnimation.TargetType.TextMeshProUGUI:
						this.tween = ((TextMeshProUGUI)this.target).DOColor(this.endValueColor, this.duration);
						break;
					}
					break;
				case DOTweenAnimation.AnimationType.Fade:
					this.isRelative = false;
					switch (this.targetType)
					{
					case DOTweenAnimation.TargetType.CanvasGroup:
						this.tween = ((CanvasGroup)this.target).DOFade(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Image:
						this.tween = ((Graphic)this.target).DOFade(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Light:
						this.tween = ((Light)this.target).DOIntensity(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Renderer:
						this.tween = ((Renderer)this.target).material.DOFade(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.SpriteRenderer:
						this.tween = ((SpriteRenderer)this.target).DOFade(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.Text:
						this.tween = ((Text)this.target).DOFade(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.TextMeshPro:
						this.tween = ((TextMeshPro)this.target).DOFade(this.endValueFloat, this.duration);
						break;
					case DOTweenAnimation.TargetType.TextMeshProUGUI:
						this.tween = ((TextMeshProUGUI)this.target).DOFade(this.endValueFloat, this.duration);
						break;
					}
					break;
				case DOTweenAnimation.AnimationType.Text:
				{
					DOTweenAnimation.TargetType targetType2 = this.targetType;
					if (targetType2 == DOTweenAnimation.TargetType.Text)
					{
						this.tween = ((Text)this.target).DOText(this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
					}
					targetType2 = this.targetType;
					if (targetType2 != DOTweenAnimation.TargetType.TextMeshPro)
					{
						if (targetType2 == DOTweenAnimation.TargetType.TextMeshProUGUI)
						{
							this.tween = ((TextMeshProUGUI)this.target).DOText(this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
						}
					}
					else
					{
						this.tween = ((TextMeshPro)this.target).DOText(this.endValueString, this.duration, this.optionalBool0, this.optionalScrambleMode, this.optionalString);
					}
					break;
				}
				case DOTweenAnimation.AnimationType.PunchPosition:
				{
					DOTweenAnimation.TargetType targetType2 = this.targetType;
					if (targetType2 != DOTweenAnimation.TargetType.RectTransform)
					{
						if (targetType2 == DOTweenAnimation.TargetType.Transform)
						{
							this.tween = ((Transform)this.target).DOPunchPosition(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
						}
					}
					else
					{
						this.tween = ((RectTransform)this.target).DOPunchAnchorPos(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0, this.optionalBool0);
					}
					break;
				}
				case DOTweenAnimation.AnimationType.PunchRotation:
					this.tween = tweenGO.transform.DOPunchRotation(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
					break;
				case DOTweenAnimation.AnimationType.PunchScale:
					this.tween = tweenGO.transform.DOPunchScale(this.endValueV3, this.duration, this.optionalInt0, this.optionalFloat0);
					break;
				case DOTweenAnimation.AnimationType.ShakePosition:
				{
					DOTweenAnimation.TargetType targetType2 = this.targetType;
					if (targetType2 != DOTweenAnimation.TargetType.RectTransform)
					{
						if (targetType2 == DOTweenAnimation.TargetType.Transform)
						{
							this.tween = ((Transform)this.target).DOShakePosition(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0, true);
						}
					}
					else
					{
						this.tween = ((RectTransform)this.target).DOShakeAnchorPos(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, this.optionalBool0, true);
					}
					break;
				}
				case DOTweenAnimation.AnimationType.ShakeRotation:
					this.tween = tweenGO.transform.DOShakeRotation(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, true);
					break;
				case DOTweenAnimation.AnimationType.ShakeScale:
					this.tween = tweenGO.transform.DOShakeScale(this.duration, this.endValueV3, this.optionalInt0, this.optionalFloat0, true);
					break;
				case DOTweenAnimation.AnimationType.CameraAspect:
					this.tween = ((Camera)this.target).DOAspect(this.endValueFloat, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraBackgroundColor:
					this.tween = ((Camera)this.target).DOColor(this.endValueColor, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraFieldOfView:
					this.tween = ((Camera)this.target).DOFieldOfView(this.endValueFloat, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraOrthoSize:
					this.tween = ((Camera)this.target).DOOrthoSize(this.endValueFloat, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraPixelRect:
					this.tween = ((Camera)this.target).DOPixelRect(this.endValueRect, this.duration);
					break;
				case DOTweenAnimation.AnimationType.CameraRect:
					this.tween = ((Camera)this.target).DORect(this.endValueRect, this.duration);
					break;
				case DOTweenAnimation.AnimationType.UIWidthHeight:
					this.tween = ((RectTransform)this.target).DOSizeDelta(this.optionalBool0 ? new Vector2(this.endValueFloat, this.endValueFloat) : this.endValueV2, this.duration, false);
					break;
				}
				if (this.tween == null)
				{
					return;
				}
				if (this.isFrom)
				{
					((Tweener)this.tween).From(this.isRelative);
				}
				else
				{
					this.tween.SetRelative(this.isRelative);
				}
				GameObject gameObject = (this.targetIsSelf || !this.tweenTargetIsTargetGO) ? base.gameObject : this.targetGO;
				this.tween.SetTarget(gameObject).SetDelay(this.delay).SetLoops(this.loops, this.loopType).SetAutoKill(this.autoKill).OnKill(delegate
				{
					this.tween = null;
				});
				if (this.isSpeedBased)
				{
					this.tween.SetSpeedBased<Tween>();
				}
				if (this.easeType == Ease.INTERNAL_Custom)
				{
					this.tween.SetEase(this.easeCurve);
				}
				else
				{
					this.tween.SetEase(this.easeType);
				}
				if (!string.IsNullOrEmpty(this.id))
				{
					this.tween.SetId(this.id);
				}
				this.tween.SetUpdate(this.isIndependentUpdate);
				if (this.hasOnStart)
				{
					if (this.onStart != null)
					{
						this.tween.OnStart(new TweenCallback(this.onStart.Invoke));
					}
				}
				else
				{
					this.onStart = null;
				}
				if (this.hasOnPlay)
				{
					if (this.onPlay != null)
					{
						this.tween.OnPlay(new TweenCallback(this.onPlay.Invoke));
					}
				}
				else
				{
					this.onPlay = null;
				}
				if (this.hasOnUpdate)
				{
					if (this.onUpdate != null)
					{
						this.tween.OnUpdate(new TweenCallback(this.onUpdate.Invoke));
					}
				}
				else
				{
					this.onUpdate = null;
				}
				if (this.hasOnStepComplete)
				{
					if (this.onStepComplete != null)
					{
						this.tween.OnStepComplete(new TweenCallback(this.onStepComplete.Invoke));
					}
				}
				else
				{
					this.onStepComplete = null;
				}
				if (this.hasOnComplete)
				{
					if (this.onComplete != null)
					{
						this.tween.OnComplete(new TweenCallback(this.onComplete.Invoke));
					}
				}
				else
				{
					this.onComplete = null;
				}
				if (this.hasOnRewind)
				{
					if (this.onRewind != null)
					{
						this.tween.OnRewind(new TweenCallback(this.onRewind.Invoke));
					}
				}
				else
				{
					this.onRewind = null;
				}
				if (this.autoPlay)
				{
					this.tween.Play<Tween>();
				}
				else
				{
					this.tween.Pause<Tween>();
				}
				if (this.hasOnTweenCreated && this.onTweenCreated != null)
				{
					this.onTweenCreated.Invoke();
				}
				return;
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0006C7F7 File Offset: 0x0006A9F7
		public override void DOPlay()
		{
			DOTween.Play(base.gameObject);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0006C805 File Offset: 0x0006AA05
		public override void DOPlayBackwards()
		{
			DOTween.PlayBackwards(base.gameObject);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0006C813 File Offset: 0x0006AA13
		public override void DOPlayForward()
		{
			DOTween.PlayForward(base.gameObject);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0006C821 File Offset: 0x0006AA21
		public override void DOPause()
		{
			DOTween.Pause(base.gameObject);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0006C82F File Offset: 0x0006AA2F
		public override void DOTogglePause()
		{
			DOTween.TogglePause(base.gameObject);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0006C840 File Offset: 0x0006AA40
		public override void DORewind()
		{
			this._playCount = -1;
			DOTweenAnimation[] components = base.gameObject.GetComponents<DOTweenAnimation>();
			for (int i = components.Length - 1; i > -1; i--)
			{
				Tween tween = components[i].tween;
				if (tween != null && tween.IsInitialized())
				{
					components[i].tween.Rewind(true);
				}
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0006C892 File Offset: 0x0006AA92
		public override void DORestart()
		{
			this.DORestart(false);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0006C89C File Offset: 0x0006AA9C
		public override void DORestart(bool fromHere)
		{
			this._playCount = -1;
			if (this.tween == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(this.tween);
				}
				return;
			}
			if (fromHere && this.isRelative)
			{
				this.ReEvaluateRelativeTween();
			}
			DOTween.Restart(base.gameObject, true, -1f);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0006C8EF File Offset: 0x0006AAEF
		public override void DOComplete()
		{
			DOTween.Complete(base.gameObject, false);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0006C8FE File Offset: 0x0006AAFE
		public override void DOKill()
		{
			DOTween.Kill(base.gameObject, false);
			this.tween = null;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0006C914 File Offset: 0x0006AB14
		public void DOPlayById(string id)
		{
			DOTween.Play(base.gameObject, id);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0006C923 File Offset: 0x0006AB23
		public void DOPlayAllById(string id)
		{
			DOTween.Play(id);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0006C92C File Offset: 0x0006AB2C
		public void DOPauseAllById(string id)
		{
			DOTween.Pause(id);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0006C935 File Offset: 0x0006AB35
		public void DOPlayBackwardsById(string id)
		{
			DOTween.PlayBackwards(base.gameObject, id);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0006C944 File Offset: 0x0006AB44
		public void DOPlayBackwardsAllById(string id)
		{
			DOTween.PlayBackwards(id);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0006C94D File Offset: 0x0006AB4D
		public void DOPlayForwardById(string id)
		{
			DOTween.PlayForward(base.gameObject, id);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0006C95C File Offset: 0x0006AB5C
		public void DOPlayForwardAllById(string id)
		{
			DOTween.PlayForward(id);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0006C968 File Offset: 0x0006AB68
		public void DOPlayNext()
		{
			DOTweenAnimation[] components = base.GetComponents<DOTweenAnimation>();
			while (this._playCount < components.Length - 1)
			{
				this._playCount++;
				DOTweenAnimation dotweenAnimation = components[this._playCount];
				if (dotweenAnimation != null && dotweenAnimation.tween != null && !dotweenAnimation.tween.IsPlaying() && !dotweenAnimation.tween.IsComplete())
				{
					dotweenAnimation.tween.Play<Tween>();
					return;
				}
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0006C9DA File Offset: 0x0006ABDA
		public void DORewindAndPlayNext()
		{
			this._playCount = -1;
			DOTween.Rewind(base.gameObject, true);
			this.DOPlayNext();
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0006C9F6 File Offset: 0x0006ABF6
		public void DORewindAllById(string id)
		{
			this._playCount = -1;
			DOTween.Rewind(id, true);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0006CA07 File Offset: 0x0006AC07
		public void DORestartById(string id)
		{
			this._playCount = -1;
			DOTween.Restart(base.gameObject, id, true, -1f);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0006CA23 File Offset: 0x0006AC23
		public void DORestartAllById(string id)
		{
			this._playCount = -1;
			DOTween.Restart(id, true, -1f);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0006CA3C File Offset: 0x0006AC3C
		public List<Tween> GetTweens()
		{
			List<Tween> list = new List<Tween>();
			foreach (DOTweenAnimation dotweenAnimation in base.GetComponents<DOTweenAnimation>())
			{
				list.Add(dotweenAnimation.tween);
			}
			return list;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0006CA78 File Offset: 0x0006AC78
		public static DOTweenAnimation.TargetType TypeToDOTargetType(Type t)
		{
			string text = t.ToString();
			int num = text.LastIndexOf(".");
			if (num != -1)
			{
				text = text.Substring(num + 1);
			}
			if (text.IndexOf("Renderer") != -1 && text != "SpriteRenderer")
			{
				text = "Renderer";
			}
			if (text == "RawImage")
			{
				text = "Image";
			}
			return (DOTweenAnimation.TargetType)Enum.Parse(typeof(DOTweenAnimation.TargetType), text);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0006CAEF File Offset: 0x0006ACEF
		public Tween CreateEditorPreview()
		{
			if (Application.isPlaying)
			{
				return null;
			}
			this.CreateTween();
			return this.tween;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0006CB06 File Offset: 0x0006AD06
		private GameObject GetTweenGO()
		{
			if (!this.targetIsSelf)
			{
				return this.targetGO;
			}
			return base.gameObject;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0006CB20 File Offset: 0x0006AD20
		private void ReEvaluateRelativeTween()
		{
			GameObject tweenGO = this.GetTweenGO();
			if (tweenGO == null)
			{
				Debug.LogWarning(string.Format("{0} :: This DOTweenAnimation's target/GameObject is unset: the tween will not be created.", base.gameObject.name), base.gameObject);
				return;
			}
			if (this.animationType == DOTweenAnimation.AnimationType.Move)
			{
				((Tweener)this.tween).ChangeEndValue(tweenGO.transform.position + this.endValueV3, true);
				return;
			}
			if (this.animationType == DOTweenAnimation.AnimationType.LocalMove)
			{
				((Tweener)this.tween).ChangeEndValue(tweenGO.transform.localPosition + this.endValueV3, true);
			}
		}

		// Token: 0x040011E0 RID: 4576
		public bool targetIsSelf = true;

		// Token: 0x040011E1 RID: 4577
		public GameObject targetGO;

		// Token: 0x040011E2 RID: 4578
		public bool tweenTargetIsTargetGO = true;

		// Token: 0x040011E3 RID: 4579
		public float delay;

		// Token: 0x040011E4 RID: 4580
		public float duration = 1f;

		// Token: 0x040011E5 RID: 4581
		public Ease easeType = Ease.OutQuad;

		// Token: 0x040011E6 RID: 4582
		public AnimationCurve easeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040011E7 RID: 4583
		public LoopType loopType;

		// Token: 0x040011E8 RID: 4584
		public int loops = 1;

		// Token: 0x040011E9 RID: 4585
		public string id = "";

		// Token: 0x040011EA RID: 4586
		public bool isRelative;

		// Token: 0x040011EB RID: 4587
		public bool isFrom;

		// Token: 0x040011EC RID: 4588
		public bool isIndependentUpdate;

		// Token: 0x040011ED RID: 4589
		public bool autoKill = true;

		// Token: 0x040011EE RID: 4590
		public bool isActive = true;

		// Token: 0x040011EF RID: 4591
		public bool isValid;

		// Token: 0x040011F0 RID: 4592
		public Component target;

		// Token: 0x040011F1 RID: 4593
		public DOTweenAnimation.AnimationType animationType;

		// Token: 0x040011F2 RID: 4594
		public DOTweenAnimation.TargetType targetType;

		// Token: 0x040011F3 RID: 4595
		public DOTweenAnimation.TargetType forcedTargetType;

		// Token: 0x040011F4 RID: 4596
		public bool autoPlay = true;

		// Token: 0x040011F5 RID: 4597
		public bool useTargetAsV3;

		// Token: 0x040011F6 RID: 4598
		public float endValueFloat;

		// Token: 0x040011F7 RID: 4599
		public Vector3 endValueV3;

		// Token: 0x040011F8 RID: 4600
		public Vector2 endValueV2;

		// Token: 0x040011F9 RID: 4601
		public Color endValueColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x040011FA RID: 4602
		public string endValueString = "";

		// Token: 0x040011FB RID: 4603
		public Rect endValueRect = new Rect(0f, 0f, 0f, 0f);

		// Token: 0x040011FC RID: 4604
		public Transform endValueTransform;

		// Token: 0x040011FD RID: 4605
		public bool optionalBool0;

		// Token: 0x040011FE RID: 4606
		public float optionalFloat0;

		// Token: 0x040011FF RID: 4607
		public int optionalInt0;

		// Token: 0x04001200 RID: 4608
		public RotateMode optionalRotationMode;

		// Token: 0x04001201 RID: 4609
		public ScrambleMode optionalScrambleMode;

		// Token: 0x04001202 RID: 4610
		public string optionalString;

		// Token: 0x04001203 RID: 4611
		private bool _tweenCreated;

		// Token: 0x04001204 RID: 4612
		private int _playCount = -1;

		// Token: 0x020002EB RID: 747
		public enum AnimationType
		{
			// Token: 0x040015EB RID: 5611
			None,
			// Token: 0x040015EC RID: 5612
			Move,
			// Token: 0x040015ED RID: 5613
			LocalMove,
			// Token: 0x040015EE RID: 5614
			Rotate,
			// Token: 0x040015EF RID: 5615
			LocalRotate,
			// Token: 0x040015F0 RID: 5616
			Scale,
			// Token: 0x040015F1 RID: 5617
			Color,
			// Token: 0x040015F2 RID: 5618
			Fade,
			// Token: 0x040015F3 RID: 5619
			Text,
			// Token: 0x040015F4 RID: 5620
			PunchPosition,
			// Token: 0x040015F5 RID: 5621
			PunchRotation,
			// Token: 0x040015F6 RID: 5622
			PunchScale,
			// Token: 0x040015F7 RID: 5623
			ShakePosition,
			// Token: 0x040015F8 RID: 5624
			ShakeRotation,
			// Token: 0x040015F9 RID: 5625
			ShakeScale,
			// Token: 0x040015FA RID: 5626
			CameraAspect,
			// Token: 0x040015FB RID: 5627
			CameraBackgroundColor,
			// Token: 0x040015FC RID: 5628
			CameraFieldOfView,
			// Token: 0x040015FD RID: 5629
			CameraOrthoSize,
			// Token: 0x040015FE RID: 5630
			CameraPixelRect,
			// Token: 0x040015FF RID: 5631
			CameraRect,
			// Token: 0x04001600 RID: 5632
			UIWidthHeight
		}

		// Token: 0x020002EC RID: 748
		public enum TargetType
		{
			// Token: 0x04001602 RID: 5634
			Unset,
			// Token: 0x04001603 RID: 5635
			Camera,
			// Token: 0x04001604 RID: 5636
			CanvasGroup,
			// Token: 0x04001605 RID: 5637
			Image,
			// Token: 0x04001606 RID: 5638
			Light,
			// Token: 0x04001607 RID: 5639
			RectTransform,
			// Token: 0x04001608 RID: 5640
			Renderer,
			// Token: 0x04001609 RID: 5641
			SpriteRenderer,
			// Token: 0x0400160A RID: 5642
			Rigidbody,
			// Token: 0x0400160B RID: 5643
			Rigidbody2D,
			// Token: 0x0400160C RID: 5644
			Text,
			// Token: 0x0400160D RID: 5645
			Transform,
			// Token: 0x0400160E RID: 5646
			tk2dBaseSprite,
			// Token: 0x0400160F RID: 5647
			tk2dTextMesh,
			// Token: 0x04001610 RID: 5648
			TextMeshPro,
			// Token: 0x04001611 RID: 5649
			TextMeshProUGUI
		}
	}
}
