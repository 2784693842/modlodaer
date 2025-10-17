using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class FollowWithinFrameTextFeedback : MonoBehaviour
{
	// Token: 0x06000524 RID: 1316 RVA: 0x00034884 File Offset: 0x00032A84
	public void Play(string _Message, Vector3 _Pos, Transform _FollowedObject, RectTransform _Frame)
	{
		this.Followed = _FollowedObject;
		this.Frame = _Frame;
		this.TextRect.position = _Pos;
		this.FollowedOffset = _Pos - _FollowedObject.position;
		base.StartCoroutine(this.TextObject.PlayFeedback(_Message));
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x000348D4 File Offset: 0x00032AD4
	private void LateUpdate()
	{
		if (this.TextObject == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!this.Followed)
		{
			return;
		}
		this.TextRect.position = this.Followed.position + this.FollowedOffset;
		if (!this.Frame)
		{
			return;
		}
		this.WorldTextRect = new Rect(this.TextRect.TransformPoint(this.TextRect.rect.position), this.TextRect.TransformVector(this.TextRect.rect.size));
		this.WorldFrameWithMargin = new Rect(this.Frame.TransformPoint(this.Frame.rect.position), this.Frame.TransformVector(this.Frame.rect.size));
		this.WorldFrameWithMargin.width = this.WorldFrameWithMargin.width - this.FrameMargin * 2f;
		this.WorldFrameWithMargin.height = this.WorldFrameWithMargin.height - this.FrameMargin * 2f;
		this.WorldFrameWithMargin.x = this.WorldFrameWithMargin.x + this.FrameMargin;
		this.WorldFrameWithMargin.y = this.WorldFrameWithMargin.y + this.FrameMargin;
		if (!this.WorldFrameWithMargin.Contains(this.WorldTextRect.min))
		{
			Vector2 vector = new Vector2(Mathf.Clamp(this.WorldTextRect.min.x, this.WorldFrameWithMargin.min.x, this.WorldFrameWithMargin.max.x), Mathf.Clamp(this.WorldTextRect.min.y, this.WorldFrameWithMargin.min.y, this.WorldFrameWithMargin.max.y)) - this.WorldTextRect.min;
			this.WorldTextRect.position = this.WorldTextRect.position + vector;
			this.TextRect.position += vector;
		}
		if (!this.WorldFrameWithMargin.Contains(this.WorldTextRect.max))
		{
			Vector2 vector = new Vector2(Mathf.Clamp(this.WorldTextRect.max.x, this.WorldFrameWithMargin.min.x, this.WorldFrameWithMargin.max.x), Mathf.Clamp(this.WorldTextRect.max.y, this.WorldFrameWithMargin.min.y, this.WorldFrameWithMargin.max.y)) - this.WorldTextRect.max;
			this.WorldTextRect.position = this.WorldTextRect.position + vector;
			this.TextRect.position += vector;
		}
	}

	// Token: 0x0400069D RID: 1693
	[SerializeField]
	private UIFeedbackText TextObject;

	// Token: 0x0400069E RID: 1694
	[SerializeField]
	private RectTransform TextRect;

	// Token: 0x0400069F RID: 1695
	[SerializeField]
	private float FrameMargin;

	// Token: 0x040006A0 RID: 1696
	private Transform Followed;

	// Token: 0x040006A1 RID: 1697
	private RectTransform Frame;

	// Token: 0x040006A2 RID: 1698
	private Vector3 FollowedOffset;

	// Token: 0x040006A3 RID: 1699
	private Rect WorldTextRect;

	// Token: 0x040006A4 RID: 1700
	private Rect WorldFrameWithMargin;
}
