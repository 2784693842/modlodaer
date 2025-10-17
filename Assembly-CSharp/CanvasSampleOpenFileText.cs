using System;
using System.Collections;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E7 RID: 487
[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileText : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06000CD3 RID: 3283 RVA: 0x00018E36 File Offset: 0x00017036
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x00068B43 File Offset: 0x00066D43
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x00068B64 File Offset: 0x00066D64
	private void OnClick()
	{
		string[] array = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", false);
		if (array.Length != 0)
		{
			base.StartCoroutine(this.OutputRoutine(new Uri(array[0]).AbsoluteUri));
		}
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x00068BA5 File Offset: 0x00066DA5
	private IEnumerator OutputRoutine(string url)
	{
		WWW loader = new WWW(url);
		yield return loader;
		this.output.text = loader.text;
		yield break;
	}

	// Token: 0x040011B7 RID: 4535
	public Text output;
}
