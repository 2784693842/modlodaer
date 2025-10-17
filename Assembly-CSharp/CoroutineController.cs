using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000173 RID: 371
public class CoroutineController
{
	// Token: 0x06000A11 RID: 2577 RVA: 0x0005A91E File Offset: 0x00058B1E
	public CoroutineController(IEnumerator routine)
	{
		this._routine = routine;
		this.state = CoroutineState.Ready;
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0005A934 File Offset: 0x00058B34
	public IEnumerator Start()
	{
		if (this.state != CoroutineState.Ready)
		{
			throw new InvalidOperationException("Unable to start coroutine in state: " + this.state);
		}
		this.state = CoroutineState.Running;
		while (this._routine.MoveNext())
		{
			yield return this._routine.Current;
			while (this.state == CoroutineState.Paused)
			{
				yield return null;
			}
			if (this.state == CoroutineState.Finished)
			{
				yield break;
			}
		}
		this.state = CoroutineState.Finished;
		yield break;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0005A943 File Offset: 0x00058B43
	public void Stop()
	{
		if (this.state != CoroutineState.Running && this.state != CoroutineState.Paused)
		{
			throw new InvalidOperationException("Unable to stop coroutine in state: " + this.state);
		}
		this.state = CoroutineState.Finished;
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0005A979 File Offset: 0x00058B79
	public void Pause()
	{
		if (this.state != CoroutineState.Running)
		{
			throw new InvalidOperationException("Unable to pause coroutine in state: " + this.state);
		}
		this.state = CoroutineState.Paused;
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x0005A9A6 File Offset: 0x00058BA6
	public void Resume()
	{
		if (this.state != CoroutineState.Paused)
		{
			throw new InvalidOperationException("Unable to resume coroutine in state: " + this.state);
		}
		this.state = CoroutineState.Running;
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0005A9D4 File Offset: 0x00058BD4
	public static bool WaitForControllerList(List<CoroutineController> _Controllers)
	{
		if (_Controllers == null)
		{
			return false;
		}
		if (_Controllers.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < _Controllers.Count; i++)
		{
			if (_Controllers[i].state != CoroutineState.Finished)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000F98 RID: 3992
	private IEnumerator _routine;

	// Token: 0x04000F99 RID: 3993
	public CoroutineState state;
}
