using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class CameraModeSwitch : MBSingleton<CameraModeSwitch>
{
	// Token: 0x170001FB RID: 507
	// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00059DA1 File Offset: 0x00057FA1
	// (set) Token: 0x060009EA RID: 2538 RVA: 0x00059DA9 File Offset: 0x00057FA9
	public bool IsPerspective { get; private set; }

	// Token: 0x060009EB RID: 2539 RVA: 0x00059DB2 File Offset: 0x00057FB2
	private void Awake()
	{
		if (MBSingleton<CameraModeSwitch>.PrivateInstance)
		{
			return;
		}
		MBSingleton<CameraModeSwitch>.PrivateInstance = this;
		CameraModeSwitch.RecalculateWorldRect();
		if (this.SwitchToPerspective)
		{
			this.SwitchToPersp();
			return;
		}
		this.SwitchToOrtho();
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00059DE1 File Offset: 0x00057FE1
	public static void RecalculateWorldRect()
	{
		if (!MBSingleton<CameraModeSwitch>.Instance)
		{
			return;
		}
		if (!MBSingleton<CameraModeSwitch>.Instance.MainCanvas)
		{
			return;
		}
		MBSingleton<CameraModeSwitch>.Instance.StartCoroutine(MBSingleton<CameraModeSwitch>.Instance.WaitAndUpdateWorldRect());
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00059E17 File Offset: 0x00058017
	private IEnumerator WaitAndUpdateWorldRect()
	{
		yield return new WaitForEndOfFrame();
		CameraModeSwitch.CanvasWorldRect = new Rect(this.MainCanvas.TransformPoint(MBSingleton<CameraModeSwitch>.Instance.MainCanvas.rect.position), this.MainCanvas.TransformVector(MBSingleton<CameraModeSwitch>.Instance.MainCanvas.rect.size));
		yield break;
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00059E28 File Offset: 0x00058028
	[ContextMenu("Switch to Orthographic")]
	public void SwitchToOrtho()
	{
		for (int i = 0; i < this.Targets.Length; i++)
		{
			this.Targets[i].SwitchToOrtho();
		}
		this.IsPerspective = false;
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00059E60 File Offset: 0x00058060
	[ContextMenu("Switch to Perspective")]
	public void SwitchToPersp()
	{
		for (int i = 0; i < this.Targets.Length; i++)
		{
			this.Targets[i].SwitchToPersp();
		}
		this.IsPerspective = true;
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00059E98 File Offset: 0x00058098
	private void OnDrawGizmosSelected()
	{
		Color color = Gizmos.color;
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(CameraModeSwitch.CanvasWorldRect.center, CameraModeSwitch.CanvasWorldRect.size);
		Gizmos.color = color;
	}

	// Token: 0x04000F64 RID: 3940
	[SerializeField]
	private bool SwitchToPerspective;

	// Token: 0x04000F65 RID: 3941
	[SerializeField]
	private CameraModeSwitch.CamAndCanvases[] Targets;

	// Token: 0x04000F66 RID: 3942
	[SerializeField]
	private RectTransform MainCanvas;

	// Token: 0x04000F67 RID: 3943
	public static Rect CanvasWorldRect;

	// Token: 0x02000299 RID: 665
	[Serializable]
	private struct CamAndCanvases
	{
		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x00081A65 File Offset: 0x0007FC65
		public bool IsPerspective
		{
			get
			{
				return this.CamTarget && !this.CamTarget.orthographic;
			}
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00081A84 File Offset: 0x0007FC84
		public void SwitchToOrtho()
		{
			if (this.CamTarget.orthographic)
			{
				return;
			}
			this.CamTarget.orthographic = true;
			float num = Mathf.Abs(this.OrthographicZPos - this.CamTarget.transform.position.z);
			this.CamTarget.orthographicSize = num * Mathf.Tan(this.CamTarget.fieldOfView * 0.5f * 0.017453292f);
			this.CamTarget.transform.position = new Vector3(this.CamTarget.transform.position.x, this.CamTarget.transform.position.y, this.OrthographicZPos - this.OrthographicDistance);
			for (int i = 0; i < this.CanvasTargets.Length; i++)
			{
				if (this.CanvasTargets[i])
				{
					this.CanvasTargets[i].planeDistance = this.OrthographicDistance;
				}
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00081B78 File Offset: 0x0007FD78
		public void SwitchToPersp()
		{
			if (!this.CamTarget.orthographic)
			{
				return;
			}
			this.CamTarget.orthographic = false;
			float num = this.CamTarget.orthographicSize / Mathf.Tan(this.CamTarget.fieldOfView * 0.5f * 0.017453292f);
			this.CamTarget.transform.position = new Vector3(this.CamTarget.transform.position.x, this.CamTarget.transform.position.y, this.OrthographicZPos - num);
			for (int i = 0; i < this.CanvasTargets.Length; i++)
			{
				if (this.CanvasTargets[i])
				{
					this.CanvasTargets[i].planeDistance = num;
				}
			}
		}

		// Token: 0x04001538 RID: 5432
		[SerializeField]
		private string GroupName;

		// Token: 0x04001539 RID: 5433
		[SerializeField]
		private Camera CamTarget;

		// Token: 0x0400153A RID: 5434
		[SerializeField]
		private Canvas[] CanvasTargets;

		// Token: 0x0400153B RID: 5435
		[SerializeField]
		private float OrthographicDistance;

		// Token: 0x0400153C RID: 5436
		[SerializeField]
		private float OrthographicZPos;
	}
}
