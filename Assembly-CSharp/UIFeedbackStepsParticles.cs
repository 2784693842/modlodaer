using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class UIFeedbackStepsParticles : UIFeedbackStepsBase
{
	// Token: 0x06000718 RID: 1816 RVA: 0x00047EFE File Offset: 0x000460FE
	protected override IEnumerator CustomStepsProcessing(Sprite _Icon, int _Steps, bool _Negative)
	{
		yield return null;
		if (!this.Particles || _Steps <= 0)
		{
			yield break;
		}
		if (this.CurrentForceFields != null)
		{
			ParticleUtilities.SetForceFields(this.Particles, this.CurrentForceFields, false);
		}
		ParticleUtilities.SetIcon(this.Particles, _Icon, false);
		switch (this.AffectParam)
		{
		case UIFeedbackStepsParticles.AffectedParticlesParam.Rate:
			ParticleUtilities.SetRateMultiplier(this.Particles, (float)_Steps * this.StepMultiplierValue, true);
			break;
		case UIFeedbackStepsParticles.AffectedParticlesParam.BurstCount:
			ParticleUtilities.SetBurstQuantity(this.Particles, Vector2Int.one * Mathf.RoundToInt((float)_Steps * this.StepMultiplierValue), true, 0);
			break;
		case UIFeedbackStepsParticles.AffectedParticlesParam.BurstCycles:
			ParticleUtilities.SetBurstCycles(this.Particles, Mathf.RoundToInt((float)_Steps * this.StepMultiplierValue), true, 0);
			break;
		}
		yield break;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x00047F1B File Offset: 0x0004611B
	public void SetForceFields(ParticleSystemForceField[] _ForceFields)
	{
		this.CurrentForceFields = _ForceFields;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00047F24 File Offset: 0x00046124
	protected override void OnStop()
	{
		if (this.Particles)
		{
			this.Particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}

	// Token: 0x040009D9 RID: 2521
	[SerializeField]
	private ParticleSystem Particles;

	// Token: 0x040009DA RID: 2522
	[SerializeField]
	private float StepMultiplierValue;

	// Token: 0x040009DB RID: 2523
	[SerializeField]
	private UIFeedbackStepsParticles.AffectedParticlesParam AffectParam;

	// Token: 0x040009DC RID: 2524
	private ParticleSystemForceField[] CurrentForceFields;

	// Token: 0x0200027D RID: 637
	public enum AffectedParticlesParam
	{
		// Token: 0x040014C2 RID: 5314
		Rate,
		// Token: 0x040014C3 RID: 5315
		BurstCount,
		// Token: 0x040014C4 RID: 5316
		BurstCycles
	}
}
