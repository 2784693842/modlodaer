using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x020001A6 RID: 422
[CreateAssetMenu(menuName = "Survival/Save Edits")]
public class SaveFileEditing : ScriptableObject
{
	// Token: 0x06000B9B RID: 2971 RVA: 0x00061C10 File Offset: 0x0005FE10
	[ContextMenu("Fill Perks")]
	public void FillPerksList()
	{
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (!instance)
		{
			return;
		}
		this.AllPerksDefaultOrder.Clear();
		for (int i = 0; i < instance.AllPerks.Count; i++)
		{
			if (!this.AllPerksDefaultOrder.Contains(UniqueIDScriptable.SaveID(instance.AllPerks[i])))
			{
				this.AllPerksDefaultOrder.Add(UniqueIDScriptable.SaveID(instance.AllPerks[i]));
			}
		}
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x00061C88 File Offset: 0x0005FE88
	public string DoSaveEdit(string _SaveFile)
	{
		SaveReplaceElement[] saveStringReplacements = this.SaveStringReplacements;
		return this.DoSaveEdit(_SaveFile, saveStringReplacements);
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00061CA4 File Offset: 0x0005FEA4
	public string DoSaveEdit(string _SaveFile, SaveReplaceElement[] _Replacements)
	{
		if (_Replacements == null)
		{
			return _SaveFile;
		}
		if (_Replacements.Length == 0)
		{
			return _SaveFile;
		}
		string text = _SaveFile;
		for (int i = 0; i < _Replacements.Length; i++)
		{
			text = Regex.Replace(text, _Replacements[i].Pattern(), _Replacements[i].Replacement());
		}
		return text;
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00061CE4 File Offset: 0x0005FEE4
	public bool DoPerkUnlockFix(ref int _FixVersion, ref List<string> _UnlockedPerks)
	{
		if (_FixVersion == 2)
		{
			return false;
		}
		if (_UnlockedPerks == null)
		{
			return false;
		}
		if (_UnlockedPerks.Count == 0)
		{
			return false;
		}
		Debug.LogWarning("Performing Unlocks Fix (Version " + 2 + ")");
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		int num = -1;
		list.Add("");
		for (int i = 0; i < _UnlockedPerks.Count; i++)
		{
			if (i != 0)
			{
				string text = _UnlockedPerks[i - 1];
			}
			if (i != _UnlockedPerks.Count - 1)
			{
				string text2 = _UnlockedPerks[i + 1];
			}
			if (!this.IsOutOfOrder(_UnlockedPerks, i, list))
			{
				if (num == -1)
				{
					num = i;
				}
				list2.Add(_UnlockedPerks[i]);
			}
			else
			{
				for (int j = 0; j < list2.Count; j++)
				{
					list.Add(list2[j]);
				}
				list2.Clear();
				num = -1;
				list.Add(_UnlockedPerks[i]);
			}
		}
		if (num != -1)
		{
			Debug.Log("The bug started unlocking things from " + _UnlockedPerks[num]);
			for (int k = _UnlockedPerks.Count - 1; k >= num; k--)
			{
				_UnlockedPerks.RemoveAt(k);
			}
		}
		_FixVersion = 2;
		Debug.LogWarning("Fix Successful");
		return true;
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x00061E20 File Offset: 0x00060020
	private bool IsOutOfOrder(List<string> _SavedList, int _Index, List<string> _AlreadyUnlockedList)
	{
		bool flag = false;
		int num = _Index;
		for (int i = 0; i < this.AllPerksDefaultOrder.Count; i++)
		{
			if (this.AllPerksDefaultOrder[i] == _SavedList[_Index])
			{
				flag = true;
			}
			if (flag)
			{
				if (num >= _SavedList.Count)
				{
					return true;
				}
				if (this.AllPerksDefaultOrder[i] == _SavedList[num])
				{
					num++;
				}
				else if (!_AlreadyUnlockedList.Contains(this.AllPerksDefaultOrder[i]))
				{
					return true;
				}
			}
		}
		return !flag;
	}

	// Token: 0x04001086 RID: 4230
	[SerializeField]
	private StringSaveReplace[] SaveStringReplacements;

	// Token: 0x04001087 RID: 4231
	[SerializeField]
	private List<string> AllPerksDefaultOrder = new List<string>();

	// Token: 0x04001088 RID: 4232
	public const int UnlocksFixVersion = 2;
}
