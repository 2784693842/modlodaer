using System;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class DynamicElementRef
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00063586 File Offset: 0x00061786
	// (set) Token: 0x06000BFD RID: 3069 RVA: 0x0006358E File Offset: 0x0006178E
	public virtual int Index
	{
		get
		{
			return this.PrivateIndex;
		}
		set
		{
			this.PrivateIndex = value;
		}
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x00063597 File Offset: 0x00061797
	public void SetActive(bool _Value, bool _IgnoreParentScript = false)
	{
		if (this.IsActive == _Value)
		{
			return;
		}
		this.IsActive = _Value;
		if (this.ParentScript && !_IgnoreParentScript)
		{
			this.ParentScript.OnElementSetActive(this.IsActive);
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06000BFF RID: 3071 RVA: 0x000635CB File Offset: 0x000617CB
	// (set) Token: 0x06000C00 RID: 3072 RVA: 0x000635D3 File Offset: 0x000617D3
	public bool IsActive { get; private set; }

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06000C01 RID: 3073 RVA: 0x000635DC File Offset: 0x000617DC
	// (set) Token: 0x06000C02 RID: 3074 RVA: 0x000635E4 File Offset: 0x000617E4
	public DynamicViewLayoutElement ElementObject { get; protected set; }

	// Token: 0x06000C03 RID: 3075 RVA: 0x000635ED File Offset: 0x000617ED
	public virtual void SetElement(DynamicViewLayoutElement _Element)
	{
		this.ElementObject = _Element;
	}

	// Token: 0x040010F5 RID: 4341
	[InspectorReadOnly]
	public string Name;

	// Token: 0x040010F7 RID: 4343
	public Vector3 Position;

	// Token: 0x040010F8 RID: 4344
	public Vector3 WorldPos;

	// Token: 0x040010F9 RID: 4345
	public Transform PosTransform;

	// Token: 0x040010FA RID: 4346
	public Rect Rectangle;

	// Token: 0x040010FB RID: 4347
	public bool Visible;

	// Token: 0x040010FD RID: 4349
	public DynamicViewLayoutGroup ParentScript;

	// Token: 0x040010FE RID: 4350
	private int PrivateIndex;
}
