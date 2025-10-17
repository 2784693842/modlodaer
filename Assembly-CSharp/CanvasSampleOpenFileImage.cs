using System;
using System.Collections;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E6 RID: 486
[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileImage : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06000CCE RID: 3278 RVA: 0x00018E36 File Offset: 0x00017036
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x00068ACB File Offset: 0x00066CCB
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x00068AEC File Offset: 0x00066CEC
	private void OnClick()
	{
		string[] array = StandaloneFileBrowser.OpenFilePanel("Title", "", "png", false);
		if (array.Length != 0)
		{
			base.StartCoroutine(this.OutputRoutine(new Uri(array[0]).AbsoluteUri));
		}
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x00068B2D File Offset: 0x00066D2D
	private IEnumerator OutputRoutine(string url)
	{
		WWW loader = new WWW(url);
		yield return loader;
		this.output.texture = loader.texture;
		yield break;
	}

	// Token: 0x040011B6 RID: 4534
	public RawImage output;
}
