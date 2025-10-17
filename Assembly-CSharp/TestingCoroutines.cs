using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class TestingCoroutines : MonoBehaviour
{
	// Token: 0x06000BD2 RID: 3026 RVA: 0x00063252 File Offset: 0x00061452
	private void Update()
	{
		this.CurrentFrame++;
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00063262 File Offset: 0x00061462
	[ContextMenu("Yield return")]
	public void DoWithYieldReturn()
	{
		base.StartCoroutine(this.YieldRoutine());
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00063271 File Offset: 0x00061471
	private IEnumerator YieldRoutine()
	{
		this.DoLog("-------Yield return started on frame " + this.CurrentFrame.ToString() + "------", true);
		int num;
		for (int i = 0; i < this.Iterations; i = num + 1)
		{
			yield return base.StartCoroutine(this.TestRoutine(i));
			num = i;
		}
		this.DoLog("-------Yield return finished on frame " + this.CurrentFrame.ToString() + "------", true);
		yield break;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x00063280 File Offset: 0x00061480
	[ContextMenu("Coroutine controller")]
	public void DoWithCoroutineController()
	{
		base.StartCoroutine(this.ControllerRoutine());
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x0006328F File Offset: 0x0006148F
	private IEnumerator ControllerRoutine()
	{
		this.DoLog("-------Coroutine controller started on frame " + this.CurrentFrame.ToString() + "------", true);
		if (!this.UseWaitForListForControllers)
		{
			CoroutineController controller = null;
			int num;
			for (int i = 0; i < this.Iterations; i = num + 1)
			{
				this.StartCoroutineEx(this.TestRoutine(i), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
				num = i;
			}
			controller = null;
		}
		else
		{
			List<CoroutineController> waitFor = new List<CoroutineController>();
			for (int j = 0; j < this.Iterations; j++)
			{
				CoroutineController item;
				this.StartCoroutineEx(this.TestRoutine(j), out item);
				waitFor.Add(item);
			}
			while (CoroutineController.WaitForControllerList(waitFor))
			{
				yield return null;
			}
			waitFor = null;
		}
		this.DoLog("-------Coroutine controller finished on frame " + this.CurrentFrame.ToString() + "------", true);
		yield break;
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x0006329E File Offset: 0x0006149E
	[ContextMenu("Returning Coroutine")]
	public void WithReturningCoroutine()
	{
		base.StartCoroutine(this.ReturnCouroutineEnumerator());
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x000632AD File Offset: 0x000614AD
	private IEnumerator ReturnCouroutineEnumerator()
	{
		this.DoLog("-------Return Coroutine started on frame " + this.CurrentFrame.ToString() + "------", true);
		yield return this.ReturnCoroutine();
		this.DoLog("-------Return Coroutine finished on frame " + this.CurrentFrame.ToString() + "------", true);
		yield break;
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x000632BC File Offset: 0x000614BC
	private Coroutine ReturnCoroutine()
	{
		return base.StartCoroutine(this.ControllerRoutine());
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x000632CA File Offset: 0x000614CA
	[ContextMenu("Returning Controller")]
	public void WithReturningController()
	{
		base.StartCoroutine(this.ReturnControllerEnumerator());
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x000632D9 File Offset: 0x000614D9
	private IEnumerator ReturnControllerEnumerator()
	{
		this.DoLog("-------Return Controller started on frame " + this.CurrentFrame.ToString() + "------", true);
		CoroutineController controller = this.ReturnController();
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		this.DoLog("-------Return Controller finished on frame " + this.CurrentFrame.ToString() + "------", true);
		yield break;
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x000632E8 File Offset: 0x000614E8
	private CoroutineController ReturnController()
	{
		CoroutineController result;
		this.StartCoroutineEx(this.ControllerRoutine(), out result);
		return result;
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x00063305 File Offset: 0x00061505
	private IEnumerator TestRoutine(int _Index)
	{
		if (this.DoYield)
		{
			yield return null;
		}
		this.DoLog("Finished routine " + _Index.ToString() + " on frame: " + this.CurrentFrame.ToString(), false);
		yield break;
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0006331B File Offset: 0x0006151B
	private void DoLog(string _Message, bool _Warning = false)
	{
		if (!this.OnlyLogOnGUI)
		{
			if (_Warning)
			{
				Debug.LogWarning(_Message);
			}
			else
			{
				Debug.Log(_Message);
			}
		}
		this.LogMessages.Add(_Message);
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x00063344 File Offset: 0x00061544
	private void OnGUI()
	{
		GUILayout.Label("LOGS", Array.Empty<GUILayoutOption>());
		if (this.LogMessages.Count == 0)
		{
			return;
		}
		for (int i = this.LogMessages.Count - 1; i >= 0; i--)
		{
			GUILayout.Label(this.LogMessages[i], Array.Empty<GUILayoutOption>());
		}
	}

	// Token: 0x040010E4 RID: 4324
	[SerializeField]
	private int Iterations = 100;

	// Token: 0x040010E5 RID: 4325
	[SerializeField]
	private bool DoYield;

	// Token: 0x040010E6 RID: 4326
	[SerializeField]
	private bool OnlyLogOnGUI;

	// Token: 0x040010E7 RID: 4327
	[SerializeField]
	private bool UseWaitForListForControllers;

	// Token: 0x040010E8 RID: 4328
	private int CurrentFrame;

	// Token: 0x040010E9 RID: 4329
	private List<string> LogMessages = new List<string>();
}
