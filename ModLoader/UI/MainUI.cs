using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModLoader.UI
{
	// Token: 0x02000022 RID: 34
	public static class MainUI
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00008450 File Offset: 0x00006650
		public static void CreatePanel()
		{
			GameObject gameObject = new GameObject("[ModLoaderUIBaseCanvas]", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2
			{
				x = 1920f,
				y = 1080f
			};
			component.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			component.referencePixelsPerUnit = 100f;
			Transform transform = gameObject.transform;
			GameObject gameObject2 = new GameObject("MainUIBackGround", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject2.transform.SetParent(transform);
			Image image = ModLoader.MainUIBackPanel = gameObject2.GetComponent<Image>();
			ModLoader.MainUIBackPanelRT = gameObject2.GetComponent<RectTransform>();
			image.color = new Color(0.4f, 0.9f, 1f, 0.4f);
			image.raycastTarget = true;
			image.maskable = true;
			image.type = Image.Type.Sliced;
			image.fillCenter = true;
		}
	}
}
