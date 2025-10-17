using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class OpenLogLocation : MonoBehaviour
{
	// Token: 0x06000B7C RID: 2940 RVA: 0x0000548A File Offset: 0x0000368A
	public void DoOpen()
	{
		OpenLogLocation.OpenLog();
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x000614BA File Offset: 0x0005F6BA
	public static void OpenLog()
	{
		Application.OpenURL("file://" + Application.persistentDataPath);
	}
}
