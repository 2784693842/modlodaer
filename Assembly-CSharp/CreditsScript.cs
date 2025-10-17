using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class CreditsScript : MonoBehaviour
{
	// Token: 0x060003C0 RID: 960 RVA: 0x00027348 File Offset: 0x00025548
	private void OnEnable()
	{
		for (int i = 0; i < this.CreditScreens.Length; i++)
		{
			this.CreditScreens[i].SetActive(false);
		}
		this.CreditScreens[0].SetActive(true);
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00027384 File Offset: 0x00025584
	private void Update()
	{
		this.timer += Time.deltaTime;
		if ((this.timer >= this.ScreenTime && this.ScreenTime > 0f) || this.clicked)
		{
			this.NextPage();
		}
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x000273C1 File Offset: 0x000255C1
	public void OnClick()
	{
		this.clicked = true;
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x000273CC File Offset: 0x000255CC
	public void NextPage()
	{
		this.CreditScreens[this.currentScreen].SetActive(false);
		this.currentScreen++;
		if (this.currentScreen == this.CreditScreens.Length)
		{
			this.currentScreen = 0;
		}
		this.CreditScreens[this.currentScreen].SetActive(true);
		this.timer = 0f;
		this.clicked = false;
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00027438 File Offset: 0x00025638
	public void PrevPage()
	{
		this.CreditScreens[this.currentScreen].SetActive(false);
		this.currentScreen--;
		if (this.currentScreen < 0)
		{
			this.currentScreen = this.CreditScreens.Length - 1;
		}
		this.CreditScreens[this.currentScreen].SetActive(true);
		this.timer = 0f;
		this.clicked = false;
	}

	// Token: 0x040004D1 RID: 1233
	public GameObject[] CreditScreens;

	// Token: 0x040004D2 RID: 1234
	public float ScreenTime;

	// Token: 0x040004D3 RID: 1235
	private int currentScreen;

	// Token: 0x040004D4 RID: 1236
	private float timer;

	// Token: 0x040004D5 RID: 1237
	private bool clicked;
}
