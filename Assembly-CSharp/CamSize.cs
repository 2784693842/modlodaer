using System;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class CamSize : MonoBehaviour
{
	// Token: 0x060009E5 RID: 2533 RVA: 0x00059C44 File Offset: 0x00057E44
	private void Start()
	{
		this.CurrentScreen = new Vector2Int(Screen.width, Screen.height);
		this.DoLetterBoxing();
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00059C61 File Offset: 0x00057E61
	private void FixedUpdate()
	{
		this.CurrentScreen = new Vector2Int(Screen.width, Screen.height);
		if (this.PrevScreen != this.CurrentScreen)
		{
			this.DoLetterBoxing();
		}
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00059C94 File Offset: 0x00057E94
	private void DoLetterBoxing()
	{
		float num = 1.7777778f;
		float num2 = (float)this.CurrentScreen.x / (float)this.CurrentScreen.y / num;
		Camera component = base.GetComponent<Camera>();
		if (num2 < 1f)
		{
			Rect rect = component.rect;
			rect.width = 1f;
			rect.height = num2;
			rect.x = 0f;
			rect.y = (1f - num2) / 2f;
			if (component.activeTexture)
			{
				component.activeTexture.DiscardContents();
			}
			component.rect = rect;
		}
		else
		{
			float num3 = 1f / num2;
			Rect rect2 = component.rect;
			rect2.width = num3;
			rect2.height = 1f;
			rect2.x = (1f - num3) / 2f;
			rect2.y = 0f;
			if (component.activeTexture)
			{
				component.activeTexture.DiscardContents();
			}
			component.rect = rect2;
		}
		this.PrevScreen = this.CurrentScreen;
	}

	// Token: 0x04000F62 RID: 3938
	private Vector2Int CurrentScreen;

	// Token: 0x04000F63 RID: 3939
	private Vector2Int PrevScreen;
}
