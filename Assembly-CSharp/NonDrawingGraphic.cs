using System;
using UnityEngine.UI;

// Token: 0x0200019D RID: 413
public class NonDrawingGraphic : Graphic
{
	// Token: 0x06000B76 RID: 2934 RVA: 0x00018E36 File Offset: 0x00017036
	public override void SetMaterialDirty()
	{
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x00018E36 File Offset: 0x00017036
	public override void SetVerticesDirty()
	{
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0006149D File Offset: 0x0005F69D
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
