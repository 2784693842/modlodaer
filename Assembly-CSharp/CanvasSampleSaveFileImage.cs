using System;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E9 RID: 489
[RequireComponent(typeof(Button))]
public class CanvasSampleSaveFileImage : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06000CDD RID: 3293 RVA: 0x00068C58 File Offset: 0x00066E58
	private void Awake()
	{
		int num = 100;
		int num2 = 100;
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				texture2D.SetPixel(i, j, Color.red);
			}
		}
		texture2D.Apply();
		this._textureBytes = texture2D.EncodeToPNG();
		UnityEngine.Object.Destroy(texture2D);
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x00018E36 File Offset: 0x00017036
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x00068CB7 File Offset: 0x00066EB7
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x00068CD8 File Offset: 0x00066ED8
	public void OnClick()
	{
		string text = StandaloneFileBrowser.SaveFilePanel("Title", "", "sample", "png");
		if (!string.IsNullOrEmpty(text))
		{
			File.WriteAllBytes(text, this._textureBytes);
		}
	}

	// Token: 0x040011B9 RID: 4537
	public Text output;

	// Token: 0x040011BA RID: 4538
	private byte[] _textureBytes;
}
