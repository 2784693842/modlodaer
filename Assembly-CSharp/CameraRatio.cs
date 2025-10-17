using System;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class CameraRatio : MonoBehaviour
{
	// Token: 0x060009F2 RID: 2546 RVA: 0x00059EDC File Offset: 0x000580DC
	private void Awake()
	{
		this.m_Cam = base.GetComponent<Camera>();
		this.m_CurrentRatio = 1.7777778f;
		this.m_OriginalHeight = this.m_Cam.orthographicSize;
		this.m_OriginalWidth = this.m_OriginalHeight * this.m_CurrentRatio;
		this.RecalculateSize();
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00059F2A File Offset: 0x0005812A
	private void LateUpdate()
	{
		if (!Mathf.Approximately(this.m_CurrentRatio, (float)Screen.width / (float)Screen.height))
		{
			this.RecalculateSize();
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00059F4C File Offset: 0x0005814C
	private void RecalculateSize()
	{
		this.m_CurrentRatio = (float)Screen.width / (float)Screen.height;
		this.m_Cam.orthographicSize = Mathf.Max(this.m_OriginalWidth * (1f / this.m_CurrentRatio), this.m_OriginalHeight);
	}

	// Token: 0x04000F69 RID: 3945
	private Camera m_Cam;

	// Token: 0x04000F6A RID: 3946
	private float m_WToH;

	// Token: 0x04000F6B RID: 3947
	private float m_OriginalHeight;

	// Token: 0x04000F6C RID: 3948
	private float m_OriginalWidth;

	// Token: 0x04000F6D RID: 3949
	private float m_CurrentRatio;
}
