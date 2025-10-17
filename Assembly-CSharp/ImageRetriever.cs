using System;
using System.Collections;
using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200018C RID: 396
public static class ImageRetriever
{
	// Token: 0x06000A8A RID: 2698 RVA: 0x0005DF25 File Offset: 0x0005C125
	public static IEnumerator GetImage(Action<Texture2D> _Callback)
	{
		if (ImageRetriever.Busy)
		{
			yield break;
		}
		ImageRetriever.Busy = true;
		Texture2D texture = null;
		FileInfo info = null;
		ExtensionFilter[] extensions = new ExtensionFilter[]
		{
			new ExtensionFilter("Image Files", new string[]
			{
				"png",
				"jpg",
				"jpeg"
			})
		};
		string[] array = StandaloneFileBrowser.OpenFilePanel("Select a portrait", "", extensions, false);
		if (array.Length != 0 && !string.IsNullOrEmpty(array[0]))
		{
			info = new FileInfo(array[0]);
			using (UnityWebRequest request = UnityWebRequestTexture.GetTexture("file://" + info.FullName))
			{
				yield return request.SendWebRequest();
				if (request.isNetworkError || request.isHttpError)
				{
					Debug.Log(request.error);
				}
				else
				{
					texture = DownloadHandlerTexture.GetContent(request);
					texture.name = info.Name;
				}
			}
			UnityWebRequest request = null;
		}
		if (_Callback != null)
		{
			_Callback(texture);
		}
		ImageRetriever.Busy = false;
		yield break;
		yield break;
	}

	// Token: 0x0400103E RID: 4158
	public static bool Busy;
}
