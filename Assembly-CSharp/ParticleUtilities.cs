using System;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public static class ParticleUtilities
{
	// Token: 0x06000B7F RID: 2943 RVA: 0x000614D0 File Offset: 0x0005F6D0
	public static void SetIcon(ParticleSystem _System, Texture _Texture, bool _Play)
	{
		if (!_System || !_Texture)
		{
			return;
		}
		_System.GetComponent<ParticleSystemRenderer>().material.mainTexture = _Texture;
		_System.textureSheetAnimation.enabled = false;
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x00061518 File Offset: 0x0005F718
	public static void SetIcon(ParticleSystem _System, Sprite _Sprite, bool _Play)
	{
		if (!_System || !_Sprite)
		{
			return;
		}
		ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = _System.textureSheetAnimation;
		textureSheetAnimation.mode = ParticleSystemAnimationMode.Sprites;
		textureSheetAnimation.enabled = true;
		while (textureSheetAnimation.spriteCount > 0)
		{
			textureSheetAnimation.RemoveSprite(0);
		}
		textureSheetAnimation.AddSprite(_Sprite);
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x00061574 File Offset: 0x0005F774
	public static void SetRateMultiplier(ParticleSystem _System, float _Rate, bool _Play)
	{
		if (!_System)
		{
			return;
		}
		ParticleSystem.EmissionModule emission = _System.emission;
		emission.rateOverTimeMultiplier = _Rate;
		emission.rateOverDistanceMultiplier = _Rate;
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x000615AC File Offset: 0x0005F7AC
	public static void SetBurstQuantity(ParticleSystem _System, Vector2Int _BurstAmt, bool _Play, int _BurstIndex = 0)
	{
		if (!_System)
		{
			return;
		}
		ParticleSystem.EmissionModule emission = _System.emission;
		if (emission.burstCount == 0)
		{
			emission.burstCount = 1;
		}
		ParticleSystem.Burst burst = emission.GetBurst(Mathf.Min(_BurstIndex, emission.burstCount - 1));
		if (burst.count.mode == ParticleSystemCurveMode.TwoConstants)
		{
			burst.minCount = (short)_BurstAmt.x;
			burst.maxCount = (short)_BurstAmt.y;
		}
		else
		{
			burst.count = (float)Mathf.Max(_BurstAmt.x, _BurstAmt.y);
		}
		emission.SetBurst(Mathf.Min(_BurstIndex, emission.burstCount - 1), burst);
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x00061664 File Offset: 0x0005F864
	public static void SetBurstCycles(ParticleSystem _System, int _BurstCycles, bool _Play, int _BurstIndex = 0)
	{
		if (!_System)
		{
			return;
		}
		ParticleSystem.EmissionModule emission = _System.emission;
		if (emission.burstCount == 0)
		{
			emission.burstCount = 1;
		}
		ParticleSystem.Burst burst = emission.GetBurst(Mathf.Min(_BurstIndex, emission.burstCount - 1));
		burst.cycleCount = _BurstCycles;
		emission.SetBurst(Mathf.Min(_BurstIndex, emission.burstCount - 1), burst);
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x000616D4 File Offset: 0x0005F8D4
	public static void SetColor(ParticleSystem _System, Color _Color, bool _Play)
	{
		if (!_System)
		{
			return;
		}
		ParticleSystem.MainModule main = _System.main;
		main.startColor.mode = ParticleSystemGradientMode.Color;
		ParticleSystem.MinMaxGradient startColor = _Color;
		main.startColor = startColor;
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0006171C File Offset: 0x0005F91C
	public static void SetColor(ParticleSystem _System, Gradient _Color, bool _Play)
	{
		if (!_System)
		{
			return;
		}
		ParticleSystem.MainModule main = _System.main;
		main.startColor.mode = ParticleSystemGradientMode.Gradient;
		ParticleSystem.MinMaxGradient startColor = _Color;
		main.startColor = startColor;
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00061764 File Offset: 0x0005F964
	public static void SetVelocityMultiplier(ParticleSystem _System, float _Velocity, bool _Play)
	{
		if (!_System)
		{
			return;
		}
		_System.main.startSpeedMultiplier = _Velocity;
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00061794 File Offset: 0x0005F994
	public static void SetLifetimeMultiplier(ParticleSystem _System, float _Lifetime, bool _Play)
	{
		if (!_System)
		{
			return;
		}
		_System.main.startLifetimeMultiplier = _Lifetime;
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x000617C4 File Offset: 0x0005F9C4
	public static void SetSimSpeed(ParticleSystem _System, float _SimSpeed, bool _Play)
	{
		if (!_System)
		{
			return;
		}
		_System.main.simulationSpeed = _SimSpeed;
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x000617F4 File Offset: 0x0005F9F4
	public static void SetForceFields(ParticleSystem _System, ParticleSystemForceField[] _Fields, bool _Play)
	{
		if (!_System)
		{
			return;
		}
		ParticleSystem.ExternalForcesModule externalForces = _System.externalForces;
		externalForces.influenceFilter = ParticleSystemGameObjectFilter.List;
		while (externalForces.influenceCount > _Fields.Length)
		{
			externalForces.RemoveInfluence(0);
		}
		for (int i = 0; i < _Fields.Length; i++)
		{
			if (i >= externalForces.influenceCount)
			{
				externalForces.AddInfluence(_Fields[i]);
			}
			else
			{
				externalForces.SetInfluence(i, _Fields[i]);
			}
		}
		if (_Play)
		{
			_System.Play(true);
		}
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x00061868 File Offset: 0x0005FA68
	public static void SetForceField(ParticleSystem _System, ParticleSystemForceField _Field, bool _Play)
	{
		if (!_System)
		{
			return;
		}
		ParticleSystem.ExternalForcesModule externalForces = _System.externalForces;
		externalForces.influenceFilter = ParticleSystemGameObjectFilter.List;
		if (externalForces.influenceCount == 0)
		{
			externalForces.AddInfluence(_Field);
		}
		else
		{
			externalForces.SetInfluence(0, _Field);
		}
		if (_Play)
		{
			_System.Play(true);
		}
	}
}
