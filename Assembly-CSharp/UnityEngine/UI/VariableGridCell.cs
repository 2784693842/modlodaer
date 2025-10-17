using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x020001F2 RID: 498
	[AddComponentMenu("Layout/Variable Grid Layout Group Cell", 140)]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class VariableGridCell : UIBehaviour
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0006A0CA File Offset: 0x000682CA
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x0006A0D2 File Offset: 0x000682D2
		public virtual bool overrideForceExpandWidth
		{
			get
			{
				return this.m_OverrideForceExpandWidth;
			}
			set
			{
				if (value != this.m_OverrideForceExpandWidth)
				{
					this.m_OverrideForceExpandWidth = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x0006A0EA File Offset: 0x000682EA
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x0006A0F2 File Offset: 0x000682F2
		public virtual bool forceExpandWidth
		{
			get
			{
				return this.m_ForceExpandWidth;
			}
			set
			{
				if (value != this.m_ForceExpandWidth)
				{
					this.m_ForceExpandWidth = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0006A10A File Offset: 0x0006830A
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x0006A112 File Offset: 0x00068312
		public virtual bool overrideForceExpandHeight
		{
			get
			{
				return this.m_OverrideForceExpandHeight;
			}
			set
			{
				if (value != this.m_OverrideForceExpandHeight)
				{
					this.m_OverrideForceExpandHeight = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x0006A12A File Offset: 0x0006832A
		// (set) Token: 0x06000D3E RID: 3390 RVA: 0x0006A132 File Offset: 0x00068332
		public virtual bool forceExpandHeight
		{
			get
			{
				return this.m_ForceExpandHeight;
			}
			set
			{
				if (value != this.m_ForceExpandHeight)
				{
					this.m_ForceExpandHeight = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x0006A14A File Offset: 0x0006834A
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x0006A152 File Offset: 0x00068352
		public virtual bool overrideCellAlignment
		{
			get
			{
				return this.m_OverrideCellAlignment;
			}
			set
			{
				if (value != this.m_OverrideCellAlignment)
				{
					this.m_OverrideCellAlignment = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x0006A16A File Offset: 0x0006836A
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x0006A172 File Offset: 0x00068372
		public virtual TextAnchor cellAlignment
		{
			get
			{
				return this.m_CellAlignment;
			}
			set
			{
				if (value != this.m_CellAlignment)
				{
					this.m_CellAlignment = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0006A18A File Offset: 0x0006838A
		protected VariableGridCell()
		{
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0006A192 File Offset: 0x00068392
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0006A1A0 File Offset: 0x000683A0
		protected override void OnTransformParentChanged()
		{
			this.SetDirty();
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0006A1A8 File Offset: 0x000683A8
		protected override void OnDisable()
		{
			this.SetDirty();
			base.OnDisable();
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0006A1A0 File Offset: 0x000683A0
		protected override void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0006A1A0 File Offset: 0x000683A0
		protected override void OnBeforeTransformParentChanged()
		{
			this.SetDirty();
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0006A1B6 File Offset: 0x000683B6
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
		}

		// Token: 0x040011D8 RID: 4568
		[SerializeField]
		private bool m_OverrideForceExpandWidth;

		// Token: 0x040011D9 RID: 4569
		[SerializeField]
		private bool m_ForceExpandWidth;

		// Token: 0x040011DA RID: 4570
		[SerializeField]
		private bool m_OverrideForceExpandHeight;

		// Token: 0x040011DB RID: 4571
		[SerializeField]
		private bool m_ForceExpandHeight;

		// Token: 0x040011DC RID: 4572
		[SerializeField]
		private bool m_OverrideCellAlignment;

		// Token: 0x040011DD RID: 4573
		[SerializeField]
		private TextAnchor m_CellAlignment;
	}
}
