using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x020000A5 RID: 165
public class SpecialEnding : MonoBehaviour
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0004509C File Offset: 0x0004329C
	// (set) Token: 0x060006A2 RID: 1698 RVA: 0x000450A4 File Offset: 0x000432A4
	public bool Playing { get; private set; }

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060006A3 RID: 1699 RVA: 0x000450AD File Offset: 0x000432AD
	// (set) Token: 0x060006A4 RID: 1700 RVA: 0x000450B5 File Offset: 0x000432B5
	public bool Transitioning { get; private set; }

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060006A5 RID: 1701 RVA: 0x000450BE File Offset: 0x000432BE
	public bool Done
	{
		get
		{
			return this.CurrentStep >= this.Steps.Length && !this.Transitioning;
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x000450DC File Offset: 0x000432DC
	[ContextMenu("Test")]
	public void StartSpecialEnding()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.Steps == null)
		{
			return;
		}
		if (this.Steps.Length == 0)
		{
			return;
		}
		this.Playing = true;
		this.CurrentStep = 0;
		this.ParentGroup.SetActive(true);
		if (this.CurrentTween != null && this.CurrentTween.IsPlaying())
		{
			this.CurrentTween.Complete(true);
		}
		this.CurrentTween = DOTween.Sequence();
		this.CurrentTween.Append(this.FadeToBlack.DOFade(1f, this.FadeToBlackDuration).SetEase(Ease.Linear));
		if (this.FirstTextDelay > 0f)
		{
			this.CurrentTween.AppendInterval(this.FirstTextDelay);
		}
		this.SetupStep();
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x00045198 File Offset: 0x00043398
	private void SetupStep()
	{
		this.Transitioning = true;
		this.TransitionTimer = 0f;
		this.StayTimer = 0f;
		if (this.ContinueButtonObject)
		{
			this.ContinueButtonObject.SetActive(false);
		}
		if (this.ContinueButtonText)
		{
			this.ContinueButtonText.text = this.Steps[this.CurrentStep].AnswerText;
		}
		if (!this.FXM)
		{
			this.FXM = MBSingleton<SpecialEffectsManager>.Instance;
		}
		if (this.FXM)
		{
			this.FXM.SetSpecialEndingValue(this.Steps[this.CurrentStep].VisualEffectsValue);
		}
		this.CurrentTween.AppendCallback(delegate
		{
			this.TextObject.text = this.Steps[this.CurrentStep].TextContent;
		});
		this.CurrentTween.Append(this.TextObject.DOFade(1f, this.FadeTextTransitionDuration * 0.5f).SetEase(Ease.Linear));
		this.CurrentTween.AppendCallback(new TweenCallback(this.CompleteTransition));
		this.DoImagesFade(this.Steps[this.CurrentStep].FadeInImages, true);
		this.DoImagesFade(this.Steps[this.CurrentStep].FadeOutImages, false);
		this.DoSoundsFade(this.Steps[this.CurrentStep].Sounds);
		GraphicsManager.SetActiveGroup(this.Steps[this.CurrentStep].ActivateObjects, true);
		GraphicsManager.SetActiveGroup(this.Steps[this.CurrentStep].DeactivateObjects, false);
		this.DoParticleChanges(this.Steps[this.CurrentStep].ParticleChanges);
		this.DoAnimationChanges(this.Steps[this.CurrentStep].AnimationChanges);
		if (this.Steps[this.CurrentStep].GameAmbienceVolume)
		{
			MBSingleton<SoundManager>.Instance.AmbienceVolumeMultiplierTarget = this.Steps[this.CurrentStep].GameAmbienceVolume;
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00045390 File Offset: 0x00043590
	private void DoImagesFade(CanvasGroup[] _Images, bool _FadeIn)
	{
		if (_Images == null)
		{
			return;
		}
		if (_Images.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _Images.Length; i++)
		{
			if (_Images[i])
			{
				this.CurrentTween.Insert(0f, _Images[i].DOFade(_FadeIn ? 1f : 0f, this.ImagesFadeDuration).SetEase(Ease.Linear));
			}
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x000453F4 File Offset: 0x000435F4
	private void DoSoundsFade(SpecialEnding.SoundClipParams[] _Sounds)
	{
		if (_Sounds == null)
		{
			return;
		}
		if (_Sounds.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _Sounds.Length; i++)
		{
			if (_Sounds[i].Target)
			{
				_Sounds[i].Target.TargetVolume = _Sounds[i].Volume;
			}
		}
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00045448 File Offset: 0x00043648
	private void DoParticleChanges(SpecialEnding.ParticleSystemParams[] _Changes)
	{
		if (_Changes == null)
		{
			return;
		}
		if (_Changes.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _Changes.Length; i++)
		{
			_Changes[i].Apply();
		}
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00045478 File Offset: 0x00043678
	private void DoAnimationChanges(SpecialEnding.AnimationParams[] _Changes)
	{
		if (_Changes == null)
		{
			return;
		}
		if (_Changes.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _Changes.Length; i++)
		{
			_Changes[i].Apply();
		}
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000454A8 File Offset: 0x000436A8
	private void SkipTransition()
	{
		if (this.CurrentTween != null && this.CurrentTween.IsPlaying())
		{
			this.CurrentTween.Complete(true);
		}
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x000454CB File Offset: 0x000436CB
	private void CompleteTransition()
	{
		this.CurrentTween = null;
		this.Transitioning = false;
		if (this.ContinueButtonObject)
		{
			this.ContinueButtonObject.SetActive(true);
		}
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x000454F4 File Offset: 0x000436F4
	public void Continue()
	{
		if (this.Transitioning)
		{
			if (this.CanSkipSteps && this.TransitionTimer >= this.TransitionMinDuration)
			{
				this.SkipTransition();
			}
			return;
		}
		if (this.CurrentStep >= this.Steps.Length)
		{
			return;
		}
		if (this.CurrentStep != this.Steps.Length - 1)
		{
			this.CurrentStep++;
			this.CurrentTween = DOTween.Sequence();
			this.CurrentTween.Append(this.TextObject.DOFade(0f, this.FadeTextTransitionDuration * 0.5f).SetEase(Ease.Linear));
			this.SetupStep();
			return;
		}
		this.CurrentStep++;
		this.DoSoundsFade(this.FinalSounds);
		if (!this.FadeToWhite)
		{
			MBSingleton<EndgameMenu>.Instance.QuitGame();
			return;
		}
		if (this.ContinueButtonInteractable)
		{
			this.ContinueButtonInteractable.raycastTarget = false;
		}
		this.CurrentTween = DOTween.Sequence();
		this.CurrentTween.Append(this.FadeToWhite.DOFade(1f, this.FadeToWhiteDuration).SetEase(Ease.Linear));
		this.CurrentTween.AppendInterval(this.FadeToWhiteStay);
		if (!this.ClickToFinish)
		{
			this.CurrentTween.AppendCallback(new TweenCallback(MBSingleton<EndgameMenu>.Instance.QuitGame));
			return;
		}
		this.CurrentTween.AppendCallback(new TweenCallback(this.CompleteTransition));
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x0004566C File Offset: 0x0004386C
	private void Update()
	{
		if (!this.Playing)
		{
			return;
		}
		if (this.FadeToWhite && this.ClickToFinish && this.CurrentStep >= this.Steps.Length && this.CurrentTween == null && Input.anyKeyDown)
		{
			MBSingleton<EndgameMenu>.Instance.QuitGame();
		}
		if (this.Transitioning)
		{
			this.TransitionTimer += Time.deltaTime;
			return;
		}
		if (!this.ContinueButtonObject)
		{
			this.StayTimer += Time.deltaTime;
			if (this.StayTimer >= this.FadeTextStayDuration && this.FadeTextStayDuration > 0f)
			{
				this.Continue();
			}
		}
	}

	// Token: 0x04000933 RID: 2355
	[SerializeField]
	private GameObject ParentGroup;

	// Token: 0x04000934 RID: 2356
	[SerializeField]
	private SpecialEnding.SpecialEndingText[] Steps;

	// Token: 0x04000935 RID: 2357
	[SerializeField]
	private bool CanSkipSteps;

	// Token: 0x04000936 RID: 2358
	[SerializeField]
	private float TransitionMinDuration;

	// Token: 0x04000937 RID: 2359
	[SerializeField]
	private Image FadeToBlack;

	// Token: 0x04000938 RID: 2360
	[SerializeField]
	private float FadeToBlackDuration;

	// Token: 0x04000939 RID: 2361
	[SerializeField]
	private float FirstTextDelay;

	// Token: 0x0400093A RID: 2362
	[SerializeField]
	private float FadeTextTransitionDuration;

	// Token: 0x0400093B RID: 2363
	[SerializeField]
	private float FadeTextStayDuration;

	// Token: 0x0400093C RID: 2364
	[FormerlySerializedAs("ImagesAndSoundsFadeDuration")]
	[SerializeField]
	private float ImagesFadeDuration;

	// Token: 0x0400093D RID: 2365
	[SerializeField]
	private TextMeshProUGUI TextObject;

	// Token: 0x0400093E RID: 2366
	[SerializeField]
	private GameObject ContinueButtonObject;

	// Token: 0x0400093F RID: 2367
	[SerializeField]
	private Graphic ContinueButtonInteractable;

	// Token: 0x04000940 RID: 2368
	[SerializeField]
	private TextMeshProUGUI ContinueButtonText;

	// Token: 0x04000941 RID: 2369
	[SerializeField]
	private Image FadeToWhite;

	// Token: 0x04000942 RID: 2370
	[SerializeField]
	private float FadeToWhiteDuration;

	// Token: 0x04000943 RID: 2371
	[SerializeField]
	private float FadeToWhiteStay;

	// Token: 0x04000944 RID: 2372
	[SerializeField]
	private bool ClickToFinish;

	// Token: 0x04000945 RID: 2373
	[SerializeField]
	private SpecialEnding.SoundClipParams[] FinalSounds;

	// Token: 0x04000947 RID: 2375
	private int CurrentStep;

	// Token: 0x04000948 RID: 2376
	private float TransitionTimer;

	// Token: 0x04000949 RID: 2377
	private float StayTimer;

	// Token: 0x0400094A RID: 2378
	private Sequence CurrentTween;

	// Token: 0x0400094B RID: 2379
	private SpecialEffectsManager FXM;

	// Token: 0x02000272 RID: 626
	[Serializable]
	public struct ParticleSystemParams
	{
		// Token: 0x06000FCF RID: 4047 RVA: 0x000800AC File Offset: 0x0007E2AC
		public void Apply()
		{
			if (!this.Particles)
			{
				return;
			}
			this.Particles.emission.rateOverTimeMultiplier = this.Rate;
		}

		// Token: 0x04001490 RID: 5264
		public ParticleSystem Particles;

		// Token: 0x04001491 RID: 5265
		public float Rate;
	}

	// Token: 0x02000273 RID: 627
	[Serializable]
	public struct AnimationParams
	{
		// Token: 0x06000FD0 RID: 4048 RVA: 0x000800E0 File Offset: 0x0007E2E0
		public void Apply()
		{
			if (!this.Anim)
			{
				return;
			}
			this.Anim[this.State].speed = this.Speed;
		}

		// Token: 0x04001492 RID: 5266
		public Animation Anim;

		// Token: 0x04001493 RID: 5267
		public string State;

		// Token: 0x04001494 RID: 5268
		public float Speed;
	}

	// Token: 0x02000274 RID: 628
	[Serializable]
	public struct SoundClipParams
	{
		// Token: 0x04001495 RID: 5269
		public SimpleSoundFade Target;

		// Token: 0x04001496 RID: 5270
		public float Volume;
	}

	// Token: 0x02000275 RID: 629
	[Serializable]
	public class SpecialEndingText
	{
		// Token: 0x04001497 RID: 5271
		public LocalizedString TextContent;

		// Token: 0x04001498 RID: 5272
		public LocalizedString AnswerText;

		// Token: 0x04001499 RID: 5273
		public float VisualEffectsValue;

		// Token: 0x0400149A RID: 5274
		public OptionalFloatValue GameAmbienceVolume;

		// Token: 0x0400149B RID: 5275
		public GameObject[] ActivateObjects;

		// Token: 0x0400149C RID: 5276
		public CanvasGroup[] FadeInImages;

		// Token: 0x0400149D RID: 5277
		public SpecialEnding.SoundClipParams[] Sounds;

		// Token: 0x0400149E RID: 5278
		public GameObject[] DeactivateObjects;

		// Token: 0x0400149F RID: 5279
		public CanvasGroup[] FadeOutImages;

		// Token: 0x040014A0 RID: 5280
		public SpecialEnding.ParticleSystemParams[] ParticleChanges;

		// Token: 0x040014A1 RID: 5281
		public SpecialEnding.AnimationParams[] AnimationChanges;
	}
}
