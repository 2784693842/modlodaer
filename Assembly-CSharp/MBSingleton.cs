using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class MBSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00060D16 File Offset: 0x0005EF16
	public static T Instance
	{
		get
		{
			if (MBSingleton<T>.PrivateInstance)
			{
				return MBSingleton<T>.PrivateInstance;
			}
			MBSingleton<T>.PrivateInstance = UnityEngine.Object.FindObjectOfType<T>();
			return MBSingleton<T>.PrivateInstance;
		}
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00060D40 File Offset: 0x0005EF40
	protected void SetAsInstance(bool _Force)
	{
		if (MBSingleton<T>.PrivateInstance == null)
		{
			MBSingleton<T>.PrivateInstance = (this as T);
			return;
		}
		if (_Force)
		{
			UnityEngine.Object.Destroy(MBSingleton<T>.PrivateInstance.gameObject);
			MBSingleton<T>.PrivateInstance = (this as T);
		}
	}

	// Token: 0x0400105E RID: 4190
	protected static T PrivateInstance;
}
