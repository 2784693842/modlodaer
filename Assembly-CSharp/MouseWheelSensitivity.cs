using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200019C RID: 412
[DefaultExecutionOrder(-1001)]
public class MouseWheelSensitivity : MonoBehaviour
{
	// Token: 0x06000B73 RID: 2931 RVA: 0x00061218 File Offset: 0x0005F418
	private void Start()
	{
		this.GameData = GameLoad.Instance;
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x00061228 File Offset: 0x0005F428
	private void Update()
	{
		if (!this.GameData)
		{
			return;
		}
		if (!this.GameData.CurrentGameOptions)
		{
			return;
		}
		this.VerticalScrollValue = Input.GetAxis("Mouse ScrollWheel");
		this.HorizontalScrollValue = (this.GameData.CurrentGameOptions.InvertHorizontalMouseScroll ? (-Input.GetAxis("Mouse ScrollWheel")) : Input.GetAxis("Mouse ScrollWheel"));
		if (this.ScrollRects != null && this.ScrollRects.Length != 0)
		{
			for (int i = 0; i < this.ScrollRects.Length; i++)
			{
				if (this.ScrollRects[i])
				{
					if (this.ScrollRects[i].vertical)
					{
						if ((this.ScrollRects[i].verticalNormalizedPosition <= 0f && this.VerticalScrollValue < 0f) || (this.ScrollRects[i].verticalNormalizedPosition >= 1f && this.VerticalScrollValue > 0f) || this.ScrollRects[i].viewport.rect.height >= this.ScrollRects[i].content.rect.height)
						{
							this.ScrollRects[i].scrollSensitivity = 10f * this.SensitivityMultiplier;
						}
						else
						{
							this.ScrollRects[i].scrollSensitivity = this.GameData.CurrentGameOptions.MouseWheelSensitivity * this.SensitivityMultiplier;
						}
					}
					else
					{
						if ((this.ScrollRects[i].horizontalNormalizedPosition <= 0f && this.HorizontalScrollValue < 0f) || (this.ScrollRects[i].horizontalNormalizedPosition >= 1f && this.HorizontalScrollValue > 0f) || this.ScrollRects[i].viewport.rect.width >= this.ScrollRects[i].content.rect.width)
						{
							this.ScrollRects[i].scrollSensitivity = 10f * this.SensitivityMultiplier;
						}
						else
						{
							this.ScrollRects[i].scrollSensitivity = this.GameData.CurrentGameOptions.MouseWheelSensitivity * this.SensitivityMultiplier;
						}
						if (this.GameData.CurrentGameOptions.InvertHorizontalMouseScroll)
						{
							this.ScrollRects[i].scrollSensitivity *= -1f;
						}
					}
				}
			}
		}
	}

	// Token: 0x0400106F RID: 4207
	public float SensitivityMultiplier = 1f;

	// Token: 0x04001070 RID: 4208
	public ScrollRect[] ScrollRects;

	// Token: 0x04001071 RID: 4209
	private GameLoad GameData;

	// Token: 0x04001072 RID: 4210
	private const float EdgesSensitivity = 10f;

	// Token: 0x04001073 RID: 4211
	private float VerticalScrollValue;

	// Token: 0x04001074 RID: 4212
	private float HorizontalScrollValue;
}
