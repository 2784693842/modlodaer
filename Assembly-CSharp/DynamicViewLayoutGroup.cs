using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class DynamicViewLayoutGroup : MonoBehaviour
{
	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0006360C File Offset: 0x0006180C
	// (set) Token: 0x06000C07 RID: 3079 RVA: 0x00063614 File Offset: 0x00061814
	public Rect LayoutRect { get; private set; }

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0006361D File Offset: 0x0006181D
	// (set) Token: 0x06000C09 RID: 3081 RVA: 0x00063625 File Offset: 0x00061825
	public Vector2 LayoutOriginPos { get; private set; }

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0006362E File Offset: 0x0006182E
	// (set) Token: 0x06000C0B RID: 3083 RVA: 0x00063636 File Offset: 0x00061836
	public Vector2 LayoutDirection { get; private set; }

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0006363F File Offset: 0x0006183F
	public int Count
	{
		get
		{
			if (this.AllElements == null)
			{
				return 0;
			}
			return this.AllElements.Count;
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00063656 File Offset: 0x00061856
	// (set) Token: 0x06000C0E RID: 3086 RVA: 0x0006365E File Offset: 0x0006185E
	public float Size { get; private set; }

	// Token: 0x06000C0F RID: 3087 RVA: 0x00063667 File Offset: 0x00061867
	protected void Awake()
	{
		this.SetRectTr();
		this.GetPrefabDimensions();
		this.CalculateSizeAndProperties();
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x0006367B File Offset: 0x0006187B
	protected void SetRectTr()
	{
		if (this.RectTr)
		{
			return;
		}
		if (this.ContentRect)
		{
			this.RectTr = this.ContentRect;
			return;
		}
		this.RectTr = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x000636B4 File Offset: 0x000618B4
	public void HideAllElements()
	{
		if (this.AllElements == null)
		{
			return;
		}
		for (int i = 0; i < this.AllElements.Count; i++)
		{
			this.AllElements[i].Visible = false;
			if (this.AllElements[i].ElementObject)
			{
				this.OnElementNotVisible(i);
			}
		}
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00063714 File Offset: 0x00061914
	private void GetPrefabDimensions()
	{
		if (this.ElementPool)
		{
			RectTransform component = this.ElementPool.PoolPrefab.GetComponent<RectTransform>();
			if (component)
			{
				this.PrefabDimensions = new Rect(this.RectTr.TransformVector(component.rect.position - component.rect.center), this.RectTr.TransformVector(component.rect.size));
			}
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x000637AB File Offset: 0x000619AB
	[ContextMenu("Test Add 1")]
	private void TestAddOne()
	{
		this.TestAddElements(1);
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000637B4 File Offset: 0x000619B4
	[ContextMenu("Test Add 5")]
	private void TestAddFive()
	{
		this.TestAddElements(5);
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x000637BD File Offset: 0x000619BD
	[ContextMenu("Test Add 10")]
	private void TestAddTen()
	{
		this.TestAddElements(10);
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x000637C8 File Offset: 0x000619C8
	private void TestAddElements(int _Elements)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		for (int i = 0; i < _Elements; i++)
		{
			this.AddElement(-1);
		}
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x000637F1 File Offset: 0x000619F1
	[ContextMenu("Test Remove 1")]
	private void TestRemoveOne()
	{
		this.TestRemoveElements(1);
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x000637FA File Offset: 0x000619FA
	[ContextMenu("Test Remove 5")]
	private void TestRemoveFive()
	{
		this.TestRemoveElements(5);
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00063803 File Offset: 0x00061A03
	[ContextMenu("Test Remove 10")]
	private void TestRemoveTen()
	{
		this.TestRemoveElements(10);
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00063810 File Offset: 0x00061A10
	private void TestRemoveElements(int _Elements)
	{
		int num = 0;
		while (num < _Elements && this.AllElements.Count > 0)
		{
			this.RemoveElement(this.AllElements.Count - 1);
			num++;
		}
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x0006384A File Offset: 0x00061A4A
	public void AddExtraSpace(DynamicViewExtraSpace _Space)
	{
		if (this.ExtraSpaces == null)
		{
			this.ExtraSpaces = new List<DynamicViewExtraSpace>();
		}
		this.ExtraSpaces.Add(_Space);
		if (!this.CalculateSizeInUpdate)
		{
			this.CalculateSizeAndProperties();
		}
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x0006387C File Offset: 0x00061A7C
	public void RemoveExtraSpace(DynamicViewExtraSpace _Space)
	{
		if (this.ExtraSpaces == null)
		{
			return;
		}
		if (this.ExtraSpaces.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.ExtraSpaces.Count; i++)
		{
			if (this.ExtraSpaces[i].AtIndex == _Space.AtIndex && Mathf.Approximately(_Space.Space, this.ExtraSpaces[i].Space))
			{
				this.ExtraSpaces.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x000638FC File Offset: 0x00061AFC
	public DynamicElementRef AddElement(int _AtIndex = -1)
	{
		DynamicElementRef dynamicElementRef = new DynamicElementRef();
		dynamicElementRef.ParentScript = this;
		dynamicElementRef.SetActive(true, true);
		int num = (_AtIndex < 0) ? this.AllElements.Count : Mathf.Min(_AtIndex, this.AllElements.Count);
		dynamicElementRef.Index = num;
		if (this.UseTransformsForElementPositions)
		{
			if (this.FreePosTransforms.Count != 0)
			{
				dynamicElementRef.PosTransform = this.FreePosTransforms[0];
				this.FreePosTransforms.RemoveAt(0);
			}
			else
			{
				dynamicElementRef.PosTransform = new GameObject(string.Format("{1}_Element_{0}", num.ToString(), base.name), new Type[]
				{
					typeof(RectTransform)
				}).transform;
			}
			dynamicElementRef.PosTransform.SetParent(this.ElementTransformsParent ? this.ElementTransformsParent : this.RectTr);
			dynamicElementRef.PosTransform.localScale = Vector3.one;
		}
		dynamicElementRef.Name = string.Format("{0}_{1}", base.gameObject.name, num.ToString());
		if (num == this.AllElements.Count)
		{
			this.AllElements.Add(dynamicElementRef);
		}
		else
		{
			this.AllElements.Insert(_AtIndex, dynamicElementRef);
		}
		this.RecalculateSize = true;
		return dynamicElementRef;
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00063A44 File Offset: 0x00061C44
	public void RemoveElement(int _AtIndex)
	{
		if (_AtIndex < 0 || _AtIndex >= this.AllElements.Count)
		{
			return;
		}
		if (!this.AllElements[_AtIndex].IsActive)
		{
			this.InactiveElements--;
		}
		if (this.AllElements[_AtIndex].ElementObject)
		{
			this.ElementPool.FreeItem(this.AllElements[_AtIndex].ElementObject);
			this.AllElements[_AtIndex].ElementObject.OnElementRemoved();
		}
		if (this.AllElements[_AtIndex].PosTransform && !this.FreePosTransforms.Contains(this.AllElements[_AtIndex].PosTransform))
		{
			this.FreePosTransforms.Add(this.AllElements[_AtIndex].PosTransform);
		}
		this.AllElements.RemoveAt(_AtIndex);
		this.RecalculateSize = true;
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00063B34 File Offset: 0x00061D34
	public void MoveElement(int _From, int _To)
	{
		if (_From < 0 || _From >= this.AllElements.Count || _From == _To)
		{
			return;
		}
		Mathf.Clamp(_To, 0, this.AllElements.Count - 1);
		DynamicElementRef item = this.AllElements[_From];
		this.AllElements.RemoveAt(_From);
		this.AllElements.Insert(_To, item);
		this.GetPrefabDimensions();
		this.CalculateSizeAndProperties();
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00063B9F File Offset: 0x00061D9F
	public void SetElementActive(int _Index, bool _Active)
	{
		if (_Index < 0 || _Index >= this.AllElements.Count)
		{
			return;
		}
		this.AllElements[_Index].SetActive(_Active, false);
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00063BC7 File Offset: 0x00061DC7
	public void OnElementSetActive(bool _Active)
	{
		if (!_Active)
		{
			this.InactiveElements++;
		}
		else
		{
			this.InactiveElements--;
		}
		this.RecalculateSize = true;
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00063BF4 File Offset: 0x00061DF4
	protected virtual void LateUpdate()
	{
		if (this.CalculateSizeInUpdate || this.RecalculateSize)
		{
			this.GetPrefabDimensions();
			this.CalculateSizeAndProperties();
			this.RecalculateSize = false;
		}
		if (this.MaskRect)
		{
			this.WorldMaskRect = new Rect(this.MaskRect.transform.TransformPoint(this.MaskRect.rect.position), this.MaskRect.transform.TransformVector(this.MaskRect.rect.size));
		}
		else
		{
			this.WorldMaskRect = this.LayoutRect;
		}
		for (int i = 0; i < this.AllElements.Count; i++)
		{
			this.UpdateElementVisible(i);
			if (this.ElementMovingSpeed > 0f)
			{
				this.AllElements[i].ElementObject.transform.localPosition = Vector3.Lerp(this.AllElements[i].ElementObject.transform.localPosition, this.AllElements[i].Position, this.ElementMovingSpeed * Time.deltaTime);
			}
		}
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00063D28 File Offset: 0x00061F28
	private void UpdateList()
	{
		int num = 0;
		for (int i = 0; i < this.AllElements.Count; i++)
		{
			this.AllElements[i].Index = i;
			this.AllElements[i].Name = string.Format("{0}_{1}", base.gameObject.name, i.ToString());
			this.AllElements[i].Position = this.GetElementPosition(num);
			if (this.AllElements[i].IsActive)
			{
				num++;
			}
			if (this.AllElements[i].ElementObject && this.ElementMovingSpeed <= 0f)
			{
				this.AllElements[i].ElementObject.transform.localPosition = this.AllElements[i].Position;
			}
			this.UpdateElementVisible(i);
		}
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00063E1C File Offset: 0x0006201C
	private void UpdateElementVisible(int _Index)
	{
		Rect elementWorldRect = this.GetElementWorldRect(_Index);
		if (this.UseTransformsForElementPositions)
		{
			this.AllElements[_Index].WorldPos = this.GetElementWorldRect(_Index).center;
			if (this.AllElements[_Index].PosTransform)
			{
				this.AllElements[_Index].PosTransform.position = this.AllElements[_Index].WorldPos;
			}
		}
		this.AllElements[_Index].Visible = (this.WorldMaskRect.Overlaps(elementWorldRect) && this.AllElements[_Index].IsActive);
		if (!this.AllElements[_Index].Visible)
		{
			if (this.AllElements[_Index].ElementObject)
			{
				this.OnElementNotVisible(_Index);
			}
			return;
		}
		if (!this.AllElements[_Index].ElementObject)
		{
			this.OnElementVisible(_Index);
		}
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00063F24 File Offset: 0x00062124
	protected virtual void OnElementVisible(int _Index)
	{
		this.AllElements[_Index].SetElement(this.ElementPool.GetNextItem(this.RectTr));
		Vector2 vector = Vector2.zero;
		if (this.ElementMovingSpeed > 0f)
		{
			if (_Index > 0 && this.AllElements[_Index - 1].ElementObject)
			{
				vector = this.AllElements[_Index - 1].ElementObject.transform.localPosition - this.AllElements[_Index - 1].Position;
			}
			if (vector == Vector2.zero && _Index < this.AllElements.Count - 1 && this.AllElements[_Index + 1].ElementObject)
			{
				vector = this.AllElements[_Index + 1].ElementObject.transform.localPosition - this.AllElements[_Index + 1].Position;
			}
		}
		this.AllElements[_Index].ElementObject.transform.localPosition = this.AllElements[_Index].Position + vector;
		this.AllElements[_Index].ElementObject.OnElementAdded(_Index);
		Action<int> onElementBecameVisible = this.OnElementBecameVisible;
		if (onElementBecameVisible == null)
		{
			return;
		}
		onElementBecameVisible(_Index);
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x00064094 File Offset: 0x00062294
	protected virtual void OnElementNotVisible(int _Index)
	{
		this.ElementPool.FreeItem(this.AllElements[_Index].ElementObject);
		this.AllElements[_Index].ElementObject.OnElementRemoved();
		this.AllElements[_Index].SetElement(null);
		Action<int> onElementBecameNotVisible = this.OnElementBecameNotVisible;
		if (onElementBecameNotVisible == null)
		{
			return;
		}
		onElementBecameNotVisible(_Index);
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x000640F8 File Offset: 0x000622F8
	[ContextMenu("Refresh")]
	protected void CalculateSizeAndProperties()
	{
		this.Size = this.Spacing * (float)this.AllElements.Count - this.Spacing * (float)this.InactiveElements;
		if (this.ExtraSpaces != null)
		{
			for (int i = 0; i < this.ExtraSpaces.Count; i++)
			{
				this.Size += this.ExtraSpaces[i].Space;
			}
		}
		if (this.AddedSize && this.AddedSize.gameObject.activeSelf)
		{
			if (this.LayoutOrientation == RectTransform.Axis.Horizontal)
			{
				this.Size += this.AddedSize.rect.width;
			}
			else
			{
				this.Size += this.AddedSize.rect.height;
			}
		}
		float size = this.Size;
		if (this.LayoutOrientation == RectTransform.Axis.Horizontal)
		{
			this.Size += (float)(this.Padding.left + this.Padding.right);
		}
		else
		{
			this.Size += (float)(this.Padding.top + this.Padding.right);
		}
		this.Size = Mathf.Max(this.Size, this.MinSize);
		if (!this.RectTr)
		{
			this.SetRectTr();
		}
		this.RectTr.SetSizeWithCurrentAnchors(this.LayoutOrientation, this.Size);
		this.LayoutRect = new Rect(this.RectTr.rect.x + (float)this.Padding.left, this.RectTr.rect.y + (float)this.Padding.bottom, this.RectTr.rect.width - (float)(this.Padding.left + this.Padding.right), this.RectTr.rect.height - (float)(this.Padding.top + this.Padding.bottom));
		Vector2 vector;
		if (this.LayoutOrientation == RectTransform.Axis.Horizontal)
		{
			vector = new Vector2(size - this.Spacing, 0f) * 0.5f;
		}
		else
		{
			vector = new Vector2(0f, size - this.Spacing) * 0.5f;
		}
		switch (this.Alignment)
		{
		case TextAnchor.UpperLeft:
			this.LayoutOriginPos = new Vector2(this.LayoutRect.xMin, this.LayoutRect.yMax) - this.RectTr.InverseTransformVector(new Vector2(this.PrefabDimensions.xMin, this.PrefabDimensions.yMax));
			this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.right : Vector2.down);
			goto IL_6EC;
		case TextAnchor.UpperCenter:
			this.LayoutOriginPos = new Vector2(this.LayoutRect.center.x, this.LayoutRect.yMax) - new Vector2(vector.x, 0f) - this.RectTr.InverseTransformVector(new Vector2(this.PrefabDimensions.center.x, this.PrefabDimensions.yMax));
			this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.right : Vector2.down);
			goto IL_6EC;
		case TextAnchor.UpperRight:
			this.LayoutOriginPos = this.LayoutRect.max - this.RectTr.InverseTransformVector(this.PrefabDimensions.max);
			this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.left : Vector2.down);
			goto IL_6EC;
		case TextAnchor.MiddleLeft:
			this.LayoutOriginPos = new Vector2(this.LayoutRect.xMin, this.LayoutRect.center.y) - new Vector2(0f, vector.y) - this.RectTr.InverseTransformVector(new Vector2(this.PrefabDimensions.xMin, this.PrefabDimensions.center.y));
			this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.right : Vector2.up);
			goto IL_6EC;
		case TextAnchor.MiddleRight:
			this.LayoutOriginPos = new Vector2(this.LayoutRect.xMax, this.LayoutRect.center.y) - new Vector2(0f, vector.y) - this.RectTr.InverseTransformVector(new Vector2(this.PrefabDimensions.xMax, this.PrefabDimensions.center.y));
			this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.left : Vector2.up);
			goto IL_6EC;
		case TextAnchor.LowerLeft:
			this.LayoutOriginPos = this.LayoutRect.min - this.RectTr.InverseTransformVector(this.PrefabDimensions.min);
			this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.right : Vector2.up);
			goto IL_6EC;
		case TextAnchor.LowerCenter:
			this.LayoutOriginPos = new Vector2(this.LayoutRect.center.x, this.LayoutRect.yMin) - new Vector2(vector.x, 0f) - this.RectTr.InverseTransformVector(new Vector2(this.PrefabDimensions.center.x, this.PrefabDimensions.yMin));
			this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.right : Vector2.up);
			goto IL_6EC;
		case TextAnchor.LowerRight:
			this.LayoutOriginPos = new Vector2(this.LayoutRect.xMax, this.LayoutRect.yMin) - this.RectTr.InverseTransformVector(new Vector2(this.PrefabDimensions.xMax, this.PrefabDimensions.yMin));
			this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.left : Vector2.up);
			goto IL_6EC;
		}
		this.LayoutOriginPos = this.LayoutRect.center - vector - this.RectTr.InverseTransformVector(this.PrefabDimensions.center);
		this.LayoutDirection = ((this.LayoutOrientation == RectTransform.Axis.Horizontal) ? Vector2.right : Vector2.up);
		IL_6EC:
		this.UpdateList();
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x000647F8 File Offset: 0x000629F8
	protected Vector3 GetElementPosition(int _Index)
	{
		if (this.ExtraSpaces == null)
		{
			return this.LayoutOriginPos + this.LayoutDirection * this.Spacing * (float)_Index;
		}
		if (this.ExtraSpaces.Count == 0)
		{
			return this.LayoutOriginPos + this.LayoutDirection * this.Spacing * (float)_Index;
		}
		float num = 0f;
		for (int i = 0; i < this.ExtraSpaces.Count; i++)
		{
			if (this.ExtraSpaces[i].AtIndex <= _Index)
			{
				num += this.ExtraSpaces[i].Space;
			}
		}
		return this.LayoutOriginPos + this.LayoutDirection * (this.Spacing * (float)_Index + num);
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x000648D8 File Offset: 0x00062AD8
	protected Rect GetElementWorldRect(int _Index)
	{
		if (!this.RectTr)
		{
			this.SetRectTr();
		}
		if (this.AllElements[_Index] == null)
		{
			return new Rect(this.PrefabDimensions.position, this.PrefabDimensions.size);
		}
		return new Rect(this.RectTr.TransformPoint(this.AllElements[_Index].Position) + this.PrefabDimensions.position, this.PrefabDimensions.size);
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x00064964 File Offset: 0x00062B64
	protected Vector3 GetExtraSpacePosition(DynamicViewExtraSpace _Space)
	{
		if (_Space.AtIndex == 0)
		{
			return this.GetElementPosition(0);
		}
		return this.GetElementPosition(_Space.AtIndex - 1) + this.LayoutDirection * (this.Spacing * 0.5f + _Space.Space * 0.5f);
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x000649C0 File Offset: 0x00062BC0
	public float ElementScrollPosition(int _Index)
	{
		if (_Index == -1 || _Index >= this.Count || !this.MaskRect || !this.RectTr)
		{
			return 0f;
		}
		float result;
		if (this.LayoutOrientation == RectTransform.Axis.Horizontal)
		{
			float num = this.MaskRect.rect.width / 2f;
			float b = this.RectTr.rect.width - num;
			result = Mathf.InverseLerp(num, b, this.AllElements[_Index].Position.x);
		}
		else
		{
			float num2 = this.MaskRect.rect.height / 2f;
			result = Mathf.InverseLerp(this.LayoutRect.height - num2, num2, Mathf.Abs(this.AllElements[_Index].Position.y));
		}
		return result;
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x00064AA8 File Offset: 0x00062CA8
	protected virtual void OnDrawGizmos()
	{
		if (!this.ShowGizmos)
		{
			return;
		}
		if (!this.RectTr)
		{
			this.SetRectTr();
		}
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.WorldMaskRect.center, new Vector3(this.WorldMaskRect.width, this.WorldMaskRect.height, 0.01f));
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(this.RectTr.TransformPoint(this.LayoutRect.center), this.RectTr.TransformVector(this.LayoutRect.size) + Vector3.forward * 0.01f);
		Gizmos.DrawWireSphere(this.RectTr.TransformPoint(this.LayoutOriginPos), 0.25f);
		for (int i = 0; i < this.AllElements.Count; i++)
		{
			if (this.AllElements[i].Visible)
			{
				Gizmos.color = new Color(1f, 0f, 0f, 1f);
			}
			else
			{
				Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
			}
			Rect elementWorldRect = this.GetElementWorldRect(i);
			Gizmos.DrawWireCube(elementWorldRect.center, new Vector3(elementWorldRect.size.x, elementWorldRect.size.y, 0.01f));
		}
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(this.WorldMaskRect.center + Vector3.up * (this.WorldMaskRect.height * 0.5f + this.PrefabDimensions.height * 0.5f + 0.25f) - this.PrefabDimensions.center, new Vector3(this.PrefabDimensions.width, this.PrefabDimensions.height, 0.01f));
		Gizmos.color = Color.white;
	}

	// Token: 0x04001101 RID: 4353
	[SerializeField]
	protected RectTransform MaskRect;

	// Token: 0x04001102 RID: 4354
	[SerializeField]
	protected RectTransform ContentRect;

	// Token: 0x04001103 RID: 4355
	[SerializeField]
	protected DynamicViewLayoutElementPool ElementPool;

	// Token: 0x04001104 RID: 4356
	[SerializeField]
	private RectOffset Padding;

	// Token: 0x04001105 RID: 4357
	[SerializeField]
	private TextAnchor Alignment;

	// Token: 0x04001106 RID: 4358
	[SerializeField]
	private float Spacing;

	// Token: 0x04001107 RID: 4359
	[SerializeField]
	protected RectTransform.Axis LayoutOrientation;

	// Token: 0x04001108 RID: 4360
	[SerializeField]
	private float MinSize;

	// Token: 0x04001109 RID: 4361
	[SerializeField]
	private RectTransform AddedSize;

	// Token: 0x0400110A RID: 4362
	[SerializeField]
	private float ElementMovingSpeed;

	// Token: 0x0400110B RID: 4363
	[SerializeField]
	private bool UseTransformsForElementPositions;

	// Token: 0x0400110C RID: 4364
	[SerializeField]
	private RectTransform ElementTransformsParent;

	// Token: 0x0400110D RID: 4365
	[Header("Debug")]
	[SerializeField]
	protected bool ShowGizmos;

	// Token: 0x0400110E RID: 4366
	[SerializeField]
	private bool CalculateSizeInUpdate;

	// Token: 0x0400110F RID: 4367
	public Action<int> OnElementBecameVisible;

	// Token: 0x04001110 RID: 4368
	public Action<int> OnElementBecameNotVisible;

	// Token: 0x04001111 RID: 4369
	protected List<DynamicElementRef> AllElements = new List<DynamicElementRef>();

	// Token: 0x04001112 RID: 4370
	protected RectTransform RectTr;

	// Token: 0x04001113 RID: 4371
	protected Rect WorldMaskRect;

	// Token: 0x04001114 RID: 4372
	private List<Transform> FreePosTransforms = new List<Transform>();

	// Token: 0x04001115 RID: 4373
	private List<DynamicViewExtraSpace> ExtraSpaces = new List<DynamicViewExtraSpace>();

	// Token: 0x04001116 RID: 4374
	private Rect PrefabDimensions;

	// Token: 0x0400111B RID: 4379
	private int InactiveElements;

	// Token: 0x0400111C RID: 4380
	protected bool RecalculateSize;
}
