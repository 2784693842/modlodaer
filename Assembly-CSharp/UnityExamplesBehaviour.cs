using System;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class UnityExamplesBehaviour : MonoBehaviour
{
	// Token: 0x060009AB RID: 2475 RVA: 0x00058F42 File Offset: 0x00057142
	private void Awake()
	{
		Debug.Log(base.gameObject + " awake!");
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00058F59 File Offset: 0x00057159
	private void Start()
	{
		Debug.Log(base.gameObject + " start!");
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00058F70 File Offset: 0x00057170
	private void OnEnable()
	{
		Debug.Log(base.gameObject + " got enabled");
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00058F87 File Offset: 0x00057187
	private void OnDisable()
	{
		Debug.Log(base.gameObject + " got disabled");
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00058FD0 File Offset: 0x000571D0
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !this.ControlledObject)
		{
			this.ControlledObject = UnityEngine.Object.Instantiate<ExampleControlledObjectBehaviour>(this.ControlledObjectPrefab, base.transform.position + Vector3.right * 2f, Quaternion.identity, base.transform);
		}
		if (Input.GetKeyDown(KeyCode.Delete) && this.ControlledObject)
		{
			UnityEngine.Object.Destroy(this.ControlledObject.gameObject);
		}
		if (Input.GetKeyDown(KeyCode.A) && this.ControlledObject)
		{
			this.ControlledObject.gameObject.SetActive(!this.ControlledObject.gameObject.activeInHierarchy);
		}
		if (Input.GetKeyDown(KeyCode.B) && this.ControlledObject)
		{
			this.ControlledObject.DoSomething();
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00018E36 File Offset: 0x00017036
	private void LateUpdate()
	{
	}

	// Token: 0x04000F2D RID: 3885
	public ExampleControlledObjectBehaviour ControlledObjectPrefab;

	// Token: 0x04000F2E RID: 3886
	private ExampleControlledObjectBehaviour ControlledObject;
}
