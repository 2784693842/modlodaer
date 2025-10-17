using System;
using System.Collections;
using System.Collections.Generic;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E8 RID: 488
[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileTextMultiple : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06000CD8 RID: 3288 RVA: 0x00018E36 File Offset: 0x00017036
	public void OnPointerDown(PointerEventData eventData)
	{
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00068BBB File Offset: 0x00066DBB
	private void Start()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x00068BDC File Offset: 0x00066DDC
	private void OnClick()
	{
		string[] array = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", true);
		if (array.Length != 0)
		{
			List<string> list = new List<string>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(new Uri(array[i]).AbsoluteUri);
			}
			base.StartCoroutine(this.OutputRoutine(list.ToArray()));
		}
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00068C40 File Offset: 0x00066E40
	private IEnumerator OutputRoutine(string[] urlArr)
	{
		string outputText = "";
		int num;
		for (int i = 0; i < urlArr.Length; i = num + 1)
		{
			WWW loader = new WWW(urlArr[i]);
			yield return loader;
			outputText += loader.text;
			loader = null;
			num = i;
		}
		this.output.text = outputText;
		yield break;
	}

	// Token: 0x040011B8 RID: 4536
	public Text output;
}
