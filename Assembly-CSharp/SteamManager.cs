using System;
using System.IO;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x0200015A RID: 346
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x0600099A RID: 2458 RVA: 0x00058D71 File Offset: 0x00056F71
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x0600099B RID: 2459 RVA: 0x00058D95 File Offset: 0x00056F95
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x0600099C RID: 2460 RVA: 0x00058DA1 File Offset: 0x00056FA1
	public static bool StartedWithAppIdFile
	{
		get
		{
			return SteamManager.Instance.m_StartedWithAppIdFile;
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00058DAD File Offset: 0x00056FAD
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x00058DB5 File Offset: 0x00056FB5
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x00058DC4 File Offset: 0x00056FC4
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(new AppId_t(GameLoad.SteamID)))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		string directoryName = Path.GetDirectoryName(Application.dataPath);
		if (File.Exists(string.Format("{0}/steam_appid.txt", directoryName)))
		{
			this.m_StartedWithAppIdFile = true;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00058EC0 File Offset: 0x000570C0
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00058F0E File Offset: 0x0005710E
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00058F32 File Offset: 0x00057132
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04000F27 RID: 3879
	protected static bool s_EverInitialized;

	// Token: 0x04000F28 RID: 3880
	protected static SteamManager s_instance;

	// Token: 0x04000F29 RID: 3881
	protected bool m_bInitialized;

	// Token: 0x04000F2A RID: 3882
	protected bool m_StartedWithAppIdFile;

	// Token: 0x04000F2B RID: 3883
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
