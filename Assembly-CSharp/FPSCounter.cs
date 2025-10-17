using System;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class FPSCounter : MonoBehaviour
{
	// Token: 0x06000A2B RID: 2603 RVA: 0x0005B0C5 File Offset: 0x000592C5
	private void Awake()
	{
		if (!Debug.isDebugBuild)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.Samples = new float[this.SampleSize];
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0005B0F8 File Offset: 0x000592F8
	private void Update()
	{
		this.Samples[this.CurrentSample] = Time.deltaTime;
		this.CurrentSample++;
		if (this.CurrentSample >= this.Samples.Length)
		{
			this.CurrentSample = 0;
		}
		this.Result = 0f;
		for (int i = 0; i < this.Samples.Length; i++)
		{
			this.Result += this.Samples[i];
		}
		this.Result /= (float)this.SampleSize;
		this.Result = 1f / this.Result;
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0005B198 File Offset: 0x00059398
	public void OnGUI()
	{
		if (!CheatsManager.ShowFPS)
		{
			return;
		}
		GUI.matrix = Matrix4x4.Scale(Vector3.one * 2f);
		GUILayout.BeginArea(new Rect((float)Screen.width * 0.5f - (float)Screen.width * 0.25f, 0f, (float)Screen.width * 0.5f, (float)Screen.height * 0.3f));
		GUILayout.Label(Mathf.FloorToInt(this.Result).ToString() + " FPS", Array.Empty<GUILayoutOption>());
		GUILayout.EndArea();
		GUI.matrix = Matrix4x4.identity;
	}

	// Token: 0x04000FA3 RID: 4003
	[SerializeField]
	private int SampleSize = 200;

	// Token: 0x04000FA4 RID: 4004
	private float[] Samples;

	// Token: 0x04000FA5 RID: 4005
	private int CurrentSample;

	// Token: 0x04000FA6 RID: 4006
	private float Result;
}
