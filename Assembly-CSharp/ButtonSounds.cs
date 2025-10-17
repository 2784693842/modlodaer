using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000164 RID: 356
public class ButtonSounds : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler
{
	// Token: 0x060009DC RID: 2524 RVA: 0x00059B1D File Offset: 0x00057D1D
	private void Awake()
	{
		if (!this.ButtonScript)
		{
			this.ButtonScript = base.GetComponent<Selectable>();
		}
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x00059B38 File Offset: 0x00057D38
	public void OnPointerEnter(PointerEventData _Pointer)
	{
		if (this.MuteHoverSounds)
		{
			return;
		}
		if (!this.ButtonScript)
		{
			return;
		}
		if (!this.Interactable)
		{
			return;
		}
		ButtonSoundsManager.PerformButtonSound(this.OnHoverSounds);
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00059B65 File Offset: 0x00057D65
	public void OnPointerClick(PointerEventData _Pointer)
	{
		if (this.MuteClickSounds)
		{
			return;
		}
		if (!this.ButtonScript)
		{
			return;
		}
		if (!this.Interactable)
		{
			return;
		}
		ButtonSoundsManager.PerformButtonSound(this.OnClickSounds);
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x00059B92 File Offset: 0x00057D92
	private void LateUpdate()
	{
		if (this.ButtonScript)
		{
			this.Interactable = this.ButtonScript.interactable;
			return;
		}
		this.Interactable = false;
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00059BBA File Offset: 0x00057DBA
	public void PlaySpecialSound()
	{
		if (this.MuteSpecialSounds)
		{
			return;
		}
		ButtonSoundsManager.PerformButtonSound(this.SpecialSounds);
	}

	// Token: 0x04000F57 RID: 3927
	[SerializeField]
	private Selectable ButtonScript;

	// Token: 0x04000F58 RID: 3928
	public bool MuteHoverSounds;

	// Token: 0x04000F59 RID: 3929
	public bool MuteClickSounds;

	// Token: 0x04000F5A RID: 3930
	public bool MuteSpecialSounds;

	// Token: 0x04000F5B RID: 3931
	[SerializeField]
	private AudioClip[] OnHoverSounds;

	// Token: 0x04000F5C RID: 3932
	[SerializeField]
	private AudioClip[] OnClickSounds;

	// Token: 0x04000F5D RID: 3933
	[SerializeField]
	private AudioClip[] SpecialSounds;

	// Token: 0x04000F5E RID: 3934
	private bool Interactable;
}
