using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class SimpleObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x06000BA8 RID: 2984 RVA: 0x00061ED8 File Offset: 0x000600D8
	public T GetNextItem(Transform _ToParent)
	{
		if (!this.PoolPrefab)
		{
			return default(T);
		}
		Transform transform = _ToParent ? _ToParent : base.transform;
		if (this.ObjectPool.Count == 0)
		{
			return UnityEngine.Object.Instantiate<T>(this.PoolPrefab, transform);
		}
		T t = this.ObjectPool.Dequeue();
		if (transform != t.transform.parent)
		{
			t.transform.SetParent(transform);
		}
		else
		{
			t.transform.SetAsLastSibling();
		}
		return t;
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00061F78 File Offset: 0x00060178
	public void FreeItem(T _Item)
	{
		if (!this.InactiveObjects && !this.DontUsePoolParent)
		{
			this.InactiveObjects = new GameObject().transform;
			this.InactiveObjects.SetParent(base.transform);
			this.InactiveObjects.gameObject.SetActive(false);
			if (!GameManager.DontRenameGOs)
			{
				this.InactiveObjects.name = "Pooled items";
			}
		}
		if (!_Item)
		{
			return;
		}
		if (this.DontUsePoolParent)
		{
			_Item.gameObject.SetActive(false);
		}
		else
		{
			_Item.transform.SetParent(this.InactiveObjects);
		}
		this.ObjectPool.Enqueue(_Item);
	}

	// Token: 0x0400108C RID: 4236
	public T PoolPrefab;

	// Token: 0x0400108D RID: 4237
	public bool DontUsePoolParent;

	// Token: 0x0400108E RID: 4238
	private Queue<T> ObjectPool = new Queue<T>();

	// Token: 0x0400108F RID: 4239
	private Transform InactiveObjects;
}
