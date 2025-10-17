using System;
using UnityEngine;

// Token: 0x020000B5 RID: 181
[RequireComponent(typeof(ParticleSystem))]
public class WeatherParticles : WeatherSpecialEffect
{
	// Token: 0x06000735 RID: 1845 RVA: 0x000483BA File Offset: 0x000465BA
	private void Awake()
	{
		this.MyParticles = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x000483C8 File Offset: 0x000465C8
	public override void Remove()
	{
		if (!this.MyParticles)
		{
			this.MyParticles = base.GetComponent<ParticleSystem>();
		}
		if (this.MyParticles)
		{
			this.MyParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
		base.Remove();
	}

	// Token: 0x040009E4 RID: 2532
	private ParticleSystem MyParticles;
}
