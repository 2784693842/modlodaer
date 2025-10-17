using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003E RID: 62
public class AffectedStatInfo : MonoBehaviour
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060002A2 RID: 674 RVA: 0x000196B7 File Offset: 0x000178B7
	// (set) Token: 0x060002A3 RID: 675 RVA: 0x000196BF File Offset: 0x000178BF
	public GameStat AssociatedStat { get; private set; }

	// Token: 0x060002A4 RID: 676 RVA: 0x000196C8 File Offset: 0x000178C8
	public void Setup(GameStat _Stat, float _RateMod, float _ValueMod)
	{
		this.AssociatedStat = _Stat;
		this.StatName.text = _Stat.GameName;
		this.StatIcon.sprite = _Stat.GetIcon;
		this.RateMod = _RateMod;
		this.ValueMod = _ValueMod;
		this.InteractionButton.interactable = !_Stat.CannotBeInspected;
		this.Update();
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0001972C File Offset: 0x0001792C
	private void Update()
	{
		if (!Mathf.Approximately(this.RateMod, 0f))
		{
			if (!this.RateTrendIndicator.gameObject.activeInHierarchy)
			{
				this.RateTrendIndicator.gameObject.SetActive(true);
			}
			this.RateTrendIndicator.UpdateAnim(this.RateMod, this.AssociatedStat.VisibleTrend, this.AssociatedStat.InvertedDirection);
			if (this.ValueTrendIndicator)
			{
				this.ValueTrendIndicator.gameObject.SetActive(false);
				return;
			}
		}
		else if (!Mathf.Approximately(this.ValueMod, 0f))
		{
			if (this.ValueTrendIndicator)
			{
				this.RateTrendIndicator.gameObject.SetActive(false);
				if (!this.ValueTrendIndicator.gameObject.activeInHierarchy)
				{
					this.ValueTrendIndicator.gameObject.SetActive(true);
				}
				this.ValueTrendIndicator.UpdateAnim(this.ValueMod, new Vector2(-1f, 1f) * Mathf.Abs(this.AssociatedStat.VisibleValue.y - this.AssociatedStat.VisibleValue.x), this.AssociatedStat.InvertedDirection);
				return;
			}
			this.RateTrendIndicator.UpdateAnim(this.ValueMod, this.AssociatedStat.VisibleTrend, this.AssociatedStat.InvertedDirection);
			return;
		}
		else
		{
			this.RateTrendIndicator.gameObject.SetActive(false);
			if (this.ValueTrendIndicator)
			{
				this.ValueTrendIndicator.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x000198BE File Offset: 0x00017ABE
	public void OnClick()
	{
		MBSingleton<GraphicsManager>.Instance.InspectStat(MBSingleton<GameManager>.Instance.StatsDict[this.AssociatedStat]);
	}

	// Token: 0x040002F8 RID: 760
	[SerializeField]
	private TextMeshProUGUI StatName;

	// Token: 0x040002F9 RID: 761
	[SerializeField]
	private Image StatIcon;

	// Token: 0x040002FA RID: 762
	[SerializeField]
	private StatTrendIndicator RateTrendIndicator;

	// Token: 0x040002FB RID: 763
	[SerializeField]
	private StatTrendIndicator ValueTrendIndicator;

	// Token: 0x040002FC RID: 764
	[SerializeField]
	private Button InteractionButton;

	// Token: 0x040002FD RID: 765
	private float RateMod;

	// Token: 0x040002FE RID: 766
	private float ValueMod;
}
