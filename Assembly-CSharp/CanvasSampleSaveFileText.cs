using System;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001EA RID: 490
[RequireComponent(typeof(Button))]
public class CanvasSampleSaveFileText : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06000CE2 RID: 3298 RVA: 0x00018E36 File Offset: 0x00017036
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x00068D13 File Offset: 0x00066F13
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x00068D34 File Offset: 0x00066F34
	public void OnClick()
	{
		string text = StandaloneFileBrowser.SaveFilePanel("Title", "", "sample", "txt");
		if (!string.IsNullOrEmpty(text))
		{
			File.WriteAllText(text, this._data);
		}
	}

	// Token: 0x040011BB RID: 4539
	public Text output;

	// Token: 0x040011BC RID: 4540
	private string _data = "Example text created by StandaloneFileBrowser";
}
