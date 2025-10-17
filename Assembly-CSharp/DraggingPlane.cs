using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class DraggingPlane : MonoBehaviour
{
	// Token: 0x060003E0 RID: 992 RVA: 0x000282E1 File Offset: 0x000264E1
	private void Start()
	{
		this.Cam = MBSingleton<CameraModeSwitch>.Instance;
		this.LocalPos = base.transform.localPosition;
		this.LocalPos.z = this.RestPos;
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00028310 File Offset: 0x00026510
	private void OnEnable()
	{
		GameManager.OnBeginDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnBeginDragItem, new Action<InGameDraggableCard>(this.OnBeginDrag));
		GameManager.OnEndDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnEndDragItem, new Action<InGameDraggableCard>(this.OnEndDrag));
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00028360 File Offset: 0x00026560
	private void OnDisable()
	{
		GameManager.OnBeginDragItem = (Action<InGameDraggableCard>)Delegate.Remove(GameManager.OnBeginDragItem, new Action<InGameDraggableCard>(this.OnBeginDrag));
		GameManager.OnEndDragItem = (Action<InGameDraggableCard>)Delegate.Remove(GameManager.OnEndDragItem, new Action<InGameDraggableCard>(this.OnEndDrag));
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x000283B0 File Offset: 0x000265B0
	private void Update()
	{
		if (!this.Cam.IsPerspective)
		{
			base.transform.localPosition = this.LocalPos;
			return;
		}
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.TargetPos, this.TransitionSpeed * Time.deltaTime);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00028409 File Offset: 0x00026609
	private void OnBeginDrag(InGameCardBase _Card)
	{
		this.TargetPos = new Vector3(this.LocalPos.x, this.LocalPos.y, this.DraggingPos);
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x00028432 File Offset: 0x00026632
	private void OnEndDrag(InGameCardBase _Card)
	{
		this.TargetPos = this.LocalPos;
	}

	// Token: 0x040004FB RID: 1275
	private CameraModeSwitch Cam;

	// Token: 0x040004FC RID: 1276
	[SerializeField]
	private float RestPos;

	// Token: 0x040004FD RID: 1277
	[SerializeField]
	private float DraggingPos;

	// Token: 0x040004FE RID: 1278
	[SerializeField]
	private float TransitionSpeed;

	// Token: 0x040004FF RID: 1279
	private Vector3 LocalPos;

	// Token: 0x04000500 RID: 1280
	private Vector3 TargetPos;
}
