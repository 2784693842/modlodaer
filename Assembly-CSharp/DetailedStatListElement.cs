using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200005C RID: 92
public class DetailedStatListElement : MonoBehaviour
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060003CC RID: 972 RVA: 0x000278F1 File Offset: 0x00025AF1
	// (set) Token: 0x060003CD RID: 973 RVA: 0x000278F9 File Offset: 0x00025AF9
	public InGameStat AssociatedStat { get; private set; }

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060003CE RID: 974 RVA: 0x00027902 File Offset: 0x00025B02
	// (set) Token: 0x060003CF RID: 975 RVA: 0x0002790A File Offset: 0x00025B0A
	public GameStat StatModel { get; private set; }

	// Token: 0x060003D0 RID: 976 RVA: 0x00027914 File Offset: 0x00025B14
	public void Setup(InGameStat _Stat)
	{
		this.AssociatedStat = _Stat;
		this.StatModel = this.AssociatedStat.StatModel;
		if (!this.StatModel)
		{
			this.AssociatedStat = null;
			return;
		}
		for (int i = 0; i < this.ColorElements.Length; i++)
		{
			this.ColorElements[i].color = this.StatModel.GetBarColor;
		}
		this.IconImage.sprite = this.StatModel.GetIcon;
		this.StatName.text = this.StatModel.GameName;
		this.TrendIndicator.Setup(_Stat);
		this.Update();
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x000279BC File Offset: 0x00025BBC
	private void Update()
	{
		if (!this.AssociatedStat)
		{
			return;
		}
		this.BarSlider.value = this.AssociatedStat.NormalizedVisibleValue;
		this.TrendIndicator.UpdateAnim();
		this.PinToggle.isOn = this.AssociatedStat.IsPinned;
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00027A0E File Offset: 0x00025C0E
	public void OnClick()
	{
		if (this.AssociatedStat)
		{
			MBSingleton<GraphicsManager>.Instance.InspectStat(this.AssociatedStat);
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00027A2D File Offset: 0x00025C2D
	public void OnPinClicked(bool _Value)
	{
		MBSingleton<GraphicsManager>.Instance.PinStat(this.AssociatedStat, _Value);
	}

	// Token: 0x040004E2 RID: 1250
	[SerializeField]
	private Slider BarSlider;

	// Token: 0x040004E3 RID: 1251
	[SerializeField]
	private Image[] ColorElements;

	// Token: 0x040004E4 RID: 1252
	[SerializeField]
	private TextMeshProUGUI StatName;

	// Token: 0x040004E5 RID: 1253
	[SerializeField]
	private Image IconImage;

	// Token: 0x040004E6 RID: 1254
	[SerializeField]
	private Toggle PinToggle;

	// Token: 0x040004E7 RID: 1255
	[SerializeField]
	private StatTrendIndicator TrendIndicator;
}
