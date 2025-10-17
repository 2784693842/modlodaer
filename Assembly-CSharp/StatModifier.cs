using System;
using UnityEngine;

// Token: 0x02000136 RID: 310
[Serializable]
public struct StatModifier
{
	// Token: 0x170001CE RID: 462
	// (get) Token: 0x0600090E RID: 2318 RVA: 0x00055FA8 File Offset: 0x000541A8
	// (set) Token: 0x0600090F RID: 2319 RVA: 0x00055FB0 File Offset: 0x000541B0
	public string ReportSource { get; private set; }

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000910 RID: 2320 RVA: 0x00055FB9 File Offset: 0x000541B9
	// (set) Token: 0x06000911 RID: 2321 RVA: 0x00055FC1 File Offset: 0x000541C1
	public string ActionSource { get; private set; }

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000912 RID: 2322 RVA: 0x00055FCA File Offset: 0x000541CA
	// (set) Token: 0x06000913 RID: 2323 RVA: 0x00055FD2 File Offset: 0x000541D2
	public int ActionTick { get; private set; }

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06000914 RID: 2324 RVA: 0x00055FDB File Offset: 0x000541DB
	public float MaxModifiedValue
	{
		get
		{
			return Mathf.Max(this.ValueModifier.x, this.ValueModifier.y);
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x00055FF8 File Offset: 0x000541F8
	public float MaxModifiedRate
	{
		get
		{
			return Mathf.Max(this.RateModifier.x, this.RateModifier.y);
		}
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00056018 File Offset: 0x00054218
	public StatModifier(StatModifier _Model, int _DaytimeTicks, bool _IgnoreRate, string _Report, string _ActionSource, int _ActionTick)
	{
		this.Stat = _Model.Stat;
		this.IsInverse = _Model.IsInverse;
		this.ApplyEachTick = _Model.ApplyEachTick;
		this.InstantModifier = _Model.InstantModifier;
		float num = Mathf.Approximately(_Model.ValueModifier.x, _Model.ValueModifier.y) ? _Model.ValueModifier.x : UnityEngine.Random.Range(_Model.ValueModifier.x, _Model.ValueModifier.y);
		float num2 = 0f;
		if (!_IgnoreRate)
		{
			num2 = (Mathf.Approximately(_Model.RateModifier.x, _Model.RateModifier.y) ? _Model.RateModifier.x : UnityEngine.Random.Range(_Model.RateModifier.x, _Model.RateModifier.y));
		}
		this.ReportSource = _Report;
		this.ActionSource = _ActionSource;
		this.ActionTick = _ActionTick;
		if (!Mathf.Approximately(num, 0f))
		{
			this.ValueModifier = ((this.ApplyEachTick || this.InstantModifier) ? (Vector2.one * num) : (Vector2.one * (num / (float)Mathf.Max(1, _DaytimeTicks))));
		}
		else
		{
			this.ValueModifier = Vector2.zero;
		}
		if (!Mathf.Approximately(num2, 0f))
		{
			this.RateModifier = ((this.ApplyEachTick || this.InstantModifier) ? (Vector2.one * num2) : (Vector2.one * (num2 / (float)Mathf.Max(1, _DaytimeTicks))));
			return;
		}
		this.RateModifier = Vector2.zero;
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x000561A4 File Offset: 0x000543A4
	public static StatModifier InstantMod(StatModifier _Model, bool _IgnoreRate, int _Ticks)
	{
		StatModifier result = _Model.Instantiate();
		if (_Model.ApplyEachTick)
		{
			result.ValueModifier *= (float)_Ticks;
			result.RateModifier *= (float)_Ticks;
		}
		if (_IgnoreRate)
		{
			result.RateModifier = Vector2.zero;
		}
		return result;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00056204 File Offset: 0x00054404
	public StatModifier Instantiate()
	{
		StatModifier result = default(StatModifier);
		result.Stat = this.Stat;
		result.IsInverse = this.IsInverse;
		result.ApplyEachTick = this.ApplyEachTick;
		float num = Mathf.Approximately(this.ValueModifier.x, this.ValueModifier.y) ? this.ValueModifier.x : UnityEngine.Random.Range(this.ValueModifier.x, this.ValueModifier.y);
		float num2 = Mathf.Approximately(this.RateModifier.x, this.RateModifier.y) ? this.RateModifier.x : UnityEngine.Random.Range(this.RateModifier.x, this.RateModifier.y);
		result.ReportSource = this.ReportSource;
		if (!Mathf.Approximately(num, 0f))
		{
			result.ValueModifier = Vector2.one * num;
		}
		else
		{
			result.ValueModifier = Vector2.zero;
		}
		if (!Mathf.Approximately(num2, 0f))
		{
			result.RateModifier = Vector2.one * num2;
		}
		else
		{
			result.RateModifier = Vector2.zero;
		}
		return result;
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x00056334 File Offset: 0x00054534
	public StatModifier Inverse
	{
		get
		{
			return new StatModifier
			{
				Stat = this.Stat,
				ValueModifier = new Vector2(-this.ValueModifier.x, -this.ValueModifier.y),
				RateModifier = new Vector2(-this.RateModifier.x, -this.RateModifier.y),
				IsInverse = true
			};
		}
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x000563A7 File Offset: 0x000545A7
	public bool IsIdentical(StatModifier _To)
	{
		return _To.Stat == this.Stat && _To.ValueModifier == this.ValueModifier && _To.RateModifier == this.RateModifier;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x000563E4 File Offset: 0x000545E4
	public static StatModifier operator *(StatModifier _Mod, float _Mult)
	{
		return new StatModifier
		{
			Stat = _Mod.Stat,
			RateModifier = _Mod.RateModifier * _Mult,
			ValueModifier = _Mod.ValueModifier * _Mult,
			IsInverse = _Mod.IsInverse
		};
	}

	// Token: 0x04000E64 RID: 3684
	public GameStat Stat;

	// Token: 0x04000E65 RID: 3685
	[MinMax]
	public Vector2 ValueModifier;

	// Token: 0x04000E66 RID: 3686
	[MinMax]
	public Vector2 RateModifier;

	// Token: 0x04000E67 RID: 3687
	public bool ApplyEachTick;

	// Token: 0x04000E68 RID: 3688
	public bool InstantModifier;

	// Token: 0x04000E69 RID: 3689
	[HideInInspector]
	public bool IsInverse;
}
