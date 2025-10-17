using System;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class ExampleControlledObjectBehaviour : MonoBehaviour
{
	// Token: 0x060009A5 RID: 2469 RVA: 0x00058F42 File Offset: 0x00057142
	private void Awake()
	{
		Debug.Log(base.gameObject + " awake!");
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00058F59 File Offset: 0x00057159
	private void Start()
	{
		Debug.Log(base.gameObject + " start!");
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x00058F70 File Offset: 0x00057170
	private void OnEnable()
	{
		Debug.Log(base.gameObject + " got enabled");
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00058F87 File Offset: 0x00057187
	private void OnDisable()
	{
		Debug.Log(base.gameObject + " got disabled");
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00058F9E File Offset: 0x0005719E
	public void DoSomething()
	{
		Debug.Log("Doing the thing");
		base.transform.position += Vector3.up * this.MoveUpDistance;
	}

	// Token: 0x04000F2C RID: 3884
	public float MoveUpDistance;
}
