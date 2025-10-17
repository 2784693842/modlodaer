using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class UniqueIDScriptable : ScriptableObject
{
	// Token: 0x06000C42 RID: 3138 RVA: 0x00065314 File Offset: 0x00063514
	public static UniqueIDScriptable GetFromID(string _ID)
	{
		UniqueIDScriptable.LoadedId = UniqueIDScriptable.LoadID(_ID);
		if (string.IsNullOrEmpty(UniqueIDScriptable.LoadedId))
		{
			return null;
		}
		if (!UniqueIDScriptable.AllUniqueObjects.ContainsKey(UniqueIDScriptable.LoadedId))
		{
			return null;
		}
		return UniqueIDScriptable.AllUniqueObjects[UniqueIDScriptable.LoadedId];
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00065354 File Offset: 0x00063554
	public static T GetFromID<T>(string _ID) where T : UniqueIDScriptable
	{
		UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(_ID);
		if (!fromID)
		{
			return default(T);
		}
		if (fromID is T)
		{
			return fromID as T;
		}
		return default(T);
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x00065397 File Offset: 0x00063597
	public static void ClearDict()
	{
		UniqueIDScriptable.AllUniqueObjects.Clear();
		UniqueIDScriptable.LoadedIDs.Clear();
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x000653B0 File Offset: 0x000635B0
	public static string SaveID(UniqueIDScriptable _Object)
	{
		if (!_Object)
		{
			return "";
		}
		if (UniqueIDScriptable.SB == null)
		{
			UniqueIDScriptable.SB = new StringBuilder();
		}
		else
		{
			UniqueIDScriptable.SB.Clear();
		}
		UniqueIDScriptable.SB.Append(_Object.UniqueID);
		UniqueIDScriptable.SB.Append("(");
		UniqueIDScriptable.SB.Append(_Object.name);
		UniqueIDScriptable.SB.Append(")");
		return UniqueIDScriptable.SB.ToString();
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00065438 File Offset: 0x00063638
	public static string LoadID(string _ID)
	{
		if (string.IsNullOrEmpty(_ID))
		{
			return _ID;
		}
		if (!_ID.EndsWith(")"))
		{
			return _ID;
		}
		if (UniqueIDScriptable.LoadedIDs.ContainsKey(_ID))
		{
			return UniqueIDScriptable.LoadedIDs[_ID];
		}
		if (UniqueIDScriptable.SB == null)
		{
			UniqueIDScriptable.SB = new StringBuilder();
		}
		else
		{
			UniqueIDScriptable.SB.Clear();
		}
		UniqueIDScriptable.SB.Append(_ID);
		while (!UniqueIDScriptable.SB.ToString().EndsWith("("))
		{
			UniqueIDScriptable.SB.Remove(UniqueIDScriptable.SB.Length - 1, 1);
		}
		UniqueIDScriptable.SB.Remove(UniqueIDScriptable.SB.Length - 1, 1);
		UniqueIDScriptable.LoadedIDs[_ID] = UniqueIDScriptable.SB.ToString();
		return UniqueIDScriptable.LoadedIDs[_ID];
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0006550C File Offset: 0x0006370C
	public static string RemoveNamesFromComplexID(string _ID)
	{
		if (_ID.Contains(")_"))
		{
			string text = _ID.Remove(0, _ID.Length - 2);
			bool flag = text.StartsWith("=");
			if (flag)
			{
				_ID = _ID.Remove(_ID.Length - 2, 2);
			}
			if (UniqueIDScriptable.SB == null)
			{
				UniqueIDScriptable.SB = new StringBuilder();
			}
			else
			{
				UniqueIDScriptable.SB.Clear();
			}
			string value = "";
			string value2 = "";
			for (int i = 0; i < _ID.Length; i++)
			{
				UniqueIDScriptable.SB.Append(_ID[i]);
				if (_ID[i] == ')')
				{
					if (i < _ID.Length - 1)
					{
						if (_ID[i + 1] == '_')
						{
							value = UniqueIDScriptable.LoadID(UniqueIDScriptable.SB.ToString());
							i++;
							UniqueIDScriptable.SB.Clear();
						}
					}
					else
					{
						value2 = UniqueIDScriptable.LoadID(UniqueIDScriptable.SB.ToString());
						UniqueIDScriptable.SB.Clear();
					}
				}
			}
			UniqueIDScriptable.SB.Append(value);
			UniqueIDScriptable.SB.Append("_");
			UniqueIDScriptable.SB.Append(value2);
			if (flag)
			{
				UniqueIDScriptable.SB.Append(text);
			}
			return UniqueIDScriptable.SB.ToString();
		}
		if ((_ID.Contains("_") && _ID.Contains("(")) || !_ID.Contains("_"))
		{
			return UniqueIDScriptable.LoadID(_ID);
		}
		return _ID;
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00065680 File Offset: 0x00063880
	public static string AddNamesToComplexID(string _ID)
	{
		if (_ID.Contains(")_"))
		{
			return _ID;
		}
		if (!_ID.Contains("_") || (_ID.Contains("_") && _ID.Contains(")")))
		{
			UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(_ID);
			if (fromID)
			{
				return UniqueIDScriptable.SaveID(fromID);
			}
			return _ID;
		}
		else
		{
			string text = _ID.Remove(0, _ID.Length - 2);
			bool flag = text.StartsWith("=");
			if (flag)
			{
				_ID = _ID.Remove(_ID.Length - 2, 2);
			}
			UniqueIDScriptable uniqueIDScriptable = null;
			UniqueIDScriptable uniqueIDScriptable2 = null;
			if (UniqueIDScriptable.SB == null)
			{
				UniqueIDScriptable.SB = new StringBuilder();
			}
			else
			{
				UniqueIDScriptable.SB.Clear();
			}
			for (int i = 0; i < _ID.Length; i++)
			{
				UniqueIDScriptable.SB.Append(_ID[i]);
				if (i < _ID.Length - 1)
				{
					if (_ID[i + 1] == '_')
					{
						uniqueIDScriptable = UniqueIDScriptable.GetFromID(UniqueIDScriptable.SB.ToString());
						i++;
						UniqueIDScriptable.SB.Clear();
					}
				}
				else
				{
					uniqueIDScriptable2 = UniqueIDScriptable.GetFromID(UniqueIDScriptable.SB.ToString());
					UniqueIDScriptable.SB.Clear();
				}
			}
			if (!uniqueIDScriptable || !uniqueIDScriptable2)
			{
				return _ID;
			}
			if (UniqueIDScriptable.ComplexNamesSB == null)
			{
				UniqueIDScriptable.ComplexNamesSB = new StringBuilder();
			}
			else
			{
				UniqueIDScriptable.ComplexNamesSB.Clear();
			}
			UniqueIDScriptable.ComplexNamesSB.Append(UniqueIDScriptable.SaveID(uniqueIDScriptable));
			UniqueIDScriptable.ComplexNamesSB.Append("_");
			UniqueIDScriptable.ComplexNamesSB.Append(UniqueIDScriptable.SaveID(uniqueIDScriptable2));
			if (flag)
			{
				UniqueIDScriptable.ComplexNamesSB.Append(text);
			}
			return UniqueIDScriptable.ComplexNamesSB.ToString();
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0006582C File Offset: 0x00063A2C
	public static bool ListContains(List<string> _List, UniqueIDScriptable _Scriptable)
	{
		if (_List == null || _Scriptable == null)
		{
			return false;
		}
		for (int i = 0; i < _List.Count; i++)
		{
			if (UniqueIDScriptable.LoadID(_List[i]) == _Scriptable.UniqueID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00018E36 File Offset: 0x00017036
	protected virtual void OnEnable()
	{
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00065874 File Offset: 0x00063A74
	public virtual void Init()
	{
		this.RegisterID();
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0006587C File Offset: 0x00063A7C
	private void RegisterID()
	{
		if (!UniqueIDScriptable.AllUniqueObjects.ContainsKey(this.UniqueID))
		{
			UniqueIDScriptable.AllUniqueObjects.Add(this.UniqueID, this);
			return;
		}
		if (UniqueIDScriptable.AllUniqueObjects[this.UniqueID] != this)
		{
			UniqueIDScriptable.Duplicates.Add(this);
		}
	}

	// Token: 0x04001132 RID: 4402
	[GUID]
	public string UniqueID;

	// Token: 0x04001133 RID: 4403
	private static string LoadedId;

	// Token: 0x04001134 RID: 4404
	private static Dictionary<string, UniqueIDScriptable> AllUniqueObjects = new Dictionary<string, UniqueIDScriptable>();

	// Token: 0x04001135 RID: 4405
	public static List<UniqueIDScriptable> Duplicates = new List<UniqueIDScriptable>();

	// Token: 0x04001136 RID: 4406
	private static StringBuilder SB;

	// Token: 0x04001137 RID: 4407
	private static StringBuilder ComplexNamesSB;

	// Token: 0x04001138 RID: 4408
	private static Dictionary<string, string> LoadedIDs = new Dictionary<string, string>();
}
