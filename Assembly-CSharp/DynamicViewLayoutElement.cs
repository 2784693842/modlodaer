using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class DynamicViewLayoutElement : MonoBehaviour
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00063564 File Offset: 0x00061764
	// (set) Token: 0x06000BF6 RID: 3062 RVA: 0x0006356C File Offset: 0x0006176C
	public int Index { get; private set; }

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00063575 File Offset: 0x00061775
	public virtual void OnElementAdded(int _AtIndex)
	{
		this.Index = _AtIndex;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00063575 File Offset: 0x00061775
	public virtual void SetElementIndex(int _Index)
	{
		this.Index = _Index;
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00018E36 File Offset: 0x00017036
	public virtual void OnElementRemoved()
	{
	}
}
