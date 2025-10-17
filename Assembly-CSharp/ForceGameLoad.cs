using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200017E RID: 382
public class ForceGameLoad : MonoBehaviour
{
	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0005B570 File Offset: 0x00059770
	public static bool ShouldLoadGameScene
	{
		get
		{
			return ForceGameLoad.LoadedFromGameScene;
		}
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0005B577 File Offset: 0x00059777
	private void Start()
	{
		if (GameLoad.Instance)
		{
			return;
		}
		if (this.GameScene)
		{
			ForceGameLoad.LoadedFromGameScene = true;
			GameManager.CurrentGamemode = this.StartMode;
			GameManager.CurrentPlayerCharacter = this.StartCharacter;
		}
		SceneManager.LoadScene(0);
	}

	// Token: 0x04000FB3 RID: 4019
	public bool GameScene;

	// Token: 0x04000FB4 RID: 4020
	public Gamemode StartMode;

	// Token: 0x04000FB5 RID: 4021
	public PlayerCharacter StartCharacter;

	// Token: 0x04000FB6 RID: 4022
	private static bool LoadedFromGameScene;
}
