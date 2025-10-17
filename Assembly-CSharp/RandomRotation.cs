using System;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class RandomRotation : MonoBehaviour
{
	// Token: 0x06000B90 RID: 2960 RVA: 0x00061967 File Offset: 0x0005FB67
	private void Awake()
	{
		this.GetInitialAngles();
		if (!this.RandomizeOnEnable)
		{
			this.Randomize();
		}
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x0006197D File Offset: 0x0005FB7D
	private void GetInitialAngles()
	{
		if (this.GotInitialAngles)
		{
			return;
		}
		this.InitialEulerAngles = base.transform.localEulerAngles;
		this.GotInitialAngles = true;
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x000619A0 File Offset: 0x0005FBA0
	private void OnEnable()
	{
		if (this.RandomizeOnEnable)
		{
			this.GetInitialAngles();
			this.Randomize();
		}
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x000619B8 File Offset: 0x0005FBB8
	private void Randomize()
	{
		Vector3 zero = Vector3.zero;
		if (!Mathf.Approximately(this.AngleVariations.x, 0f))
		{
			zero.x = UnityEngine.Random.Range(-this.AngleVariations.x, this.AngleVariations.x);
		}
		if (!Mathf.Approximately(this.AngleVariations.y, 0f))
		{
			zero.y = UnityEngine.Random.Range(-this.AngleVariations.y, this.AngleVariations.y);
		}
		if (!Mathf.Approximately(this.AngleVariations.z, 0f))
		{
			zero.z = UnityEngine.Random.Range(-this.AngleVariations.z, this.AngleVariations.z);
		}
		Debug.Log(zero);
		base.transform.eulerAngles = this.InitialEulerAngles + zero;
	}

	// Token: 0x0400107C RID: 4220
	[SerializeField]
	private Vector3 AngleVariations;

	// Token: 0x0400107D RID: 4221
	[SerializeField]
	private bool RandomizeOnEnable;

	// Token: 0x0400107E RID: 4222
	private Vector3 InitialEulerAngles;

	// Token: 0x0400107F RID: 4223
	private bool GotInitialAngles;
}
