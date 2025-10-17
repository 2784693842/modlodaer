using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000141 RID: 321
[CreateAssetMenu(menuName = "Survival/Global Character Info")]
public class GlobalCharacterInfo : ScriptableObject
{
	// Token: 0x170001DD RID: 477
	// (get) Token: 0x0600093B RID: 2363 RVA: 0x00057046 File Offset: 0x00055246
	public int CustomPortraitCount
	{
		get
		{
			if (this.AllPortraits == null)
			{
				return 0;
			}
			if (this.AllPortraits.Count == 0)
			{
				return 0;
			}
			return this.AllPortraits.Count - this.PortraitSprites.Length;
		}
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00057075 File Offset: 0x00055275
	public bool IsCustomPortrait(int _PortraitIndex)
	{
		return this.AllPortraits != null && this.AllPortraits.Count != 0 && _PortraitIndex >= this.PortraitSprites.Length;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x000570A0 File Offset: 0x000552A0
	public bool AlreadyHasCustomPortrait(string _PortraitName)
	{
		if (this.AllPortraits == null)
		{
			return false;
		}
		if (this.AllPortraits.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < this.AllPortraits.Count; i++)
		{
			if (this.AllPortraits[i].PortraitID == _PortraitName)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x000570F8 File Offset: 0x000552F8
	public void AddCustomPortrait(Texture2D _NewPortrait, MonoBehaviour _RoutineBehaviour, Action<bool> _Callback)
	{
		if (!_NewPortrait)
		{
			return;
		}
		if (this.GetPortraitIndex(_NewPortrait.name) != -1)
		{
			return;
		}
		byte[] bytes = null;
		string text = _NewPortrait.name.ToLower();
		if (text.EndsWith(".png"))
		{
			bytes = _NewPortrait.EncodeToPNG();
		}
		else if (text.EndsWith(".jpg") || text.EndsWith(".jpeg"))
		{
			bytes = _NewPortrait.EncodeToJPG();
		}
		if (!Directory.Exists(this.ImagesPath))
		{
			Directory.CreateDirectory(this.ImagesPath);
		}
		File.WriteAllBytes(this.ImagesPath + "/" + _NewPortrait.name, bytes);
		UnityEngine.Object.DestroyImmediate(_NewPortrait);
		this.LoadCustomPortraits(_RoutineBehaviour, _Callback);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x000571A8 File Offset: 0x000553A8
	public void DeleteCustomPortrait(int _Index, MonoBehaviour _RoutineBehaviour, Action<bool> _Callback)
	{
		if (_Index < 0 || _Index >= this.AllPortraits.Count)
		{
			return;
		}
		if (!Directory.Exists(this.ImagesPath))
		{
			return;
		}
		string path = this.ImagesPath + "/" + this.AllPortraits[_Index].PortraitID;
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		this.LoadCustomPortraits(_RoutineBehaviour, _Callback);
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00057210 File Offset: 0x00055410
	public void LoadCustomPortraits(MonoBehaviour _RoutineBehaviour, Action<bool> _Callback)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.ImagesPath = Application.persistentDataPath + "/Portraits";
		this.AllPortraits.Clear();
		this.AllPortraits.AddRange(this.PortraitSprites);
		_RoutineBehaviour.StartCoroutine(this.LoadPortraitsRoutine(_Callback));
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00057264 File Offset: 0x00055464
	private IEnumerator LoadPortraitsRoutine(Action<bool> _Callback)
	{
		if (this.LoadedTextures.Count > 0)
		{
			for (int j = 0; j < this.LoadedTextures.Count; j++)
			{
				UnityEngine.Object.DestroyImmediate(this.LoadedTextures[j]);
			}
		}
		this.LoadedTextures.Clear();
		if (!Directory.Exists(this.ImagesPath))
		{
			if (_Callback != null)
			{
				_Callback(false);
			}
			yield break;
		}
		DirectoryInfo directoryInfo = new DirectoryInfo(this.ImagesPath);
		FileInfo[] allFiles = directoryInfo.GetFiles();
		int num;
		for (int i = 0; i < allFiles.Length; i = num + 1)
		{
			if (!(allFiles[i].Extension.ToLower() != ".png") || !(allFiles[i].Extension.ToLower() != ".jpg") || !(allFiles[i].Extension.ToLower() != ".jpeg"))
			{
				using (UnityWebRequest request = UnityWebRequestTexture.GetTexture("file://" + allFiles[i].FullName))
				{
					yield return request.SendWebRequest();
					if (request.isNetworkError || request.isHttpError)
					{
						Debug.Log(request.error);
					}
					else
					{
						Texture2D content = DownloadHandlerTexture.GetContent(request);
						content.name = allFiles[i].Name;
						this.LoadedTextures.Add(content);
					}
				}
				UnityWebRequest request = null;
			}
			num = i;
		}
		if (this.LoadedTextures.Count == 0)
		{
			if (_Callback != null)
			{
				_Callback(false);
			}
			yield break;
		}
		for (int k = 0; k < this.LoadedTextures.Count; k++)
		{
			Sprite portraitSprite = Sprite.Create(this.LoadedTextures[k], new Rect(0f, 0f, (float)this.LoadedTextures[k].width, (float)this.LoadedTextures[k].height), Vector2.one * 0.5f);
			this.AllPortraits.Add(new CharacterPortraitRef
			{
				PortraitID = this.LoadedTextures[k].name,
				PortraitSprite = portraitSprite
			});
		}
		if (_Callback != null)
		{
			_Callback(true);
		}
		yield break;
		yield break;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0005727A File Offset: 0x0005547A
	public int NextPortrait(int _Index)
	{
		if (_Index + 1 >= this.AllPortraits.Count)
		{
			return 0;
		}
		return _Index + 1;
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00057291 File Offset: 0x00055491
	public int PrevPortrait(int _Index)
	{
		if (_Index - 1 < 0)
		{
			return this.AllPortraits.Count - 1;
		}
		return _Index - 1;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x000572AC File Offset: 0x000554AC
	public Sprite GetCharacterPortrait(string _ID)
	{
		int portraitIndex = this.GetPortraitIndex(_ID);
		if (portraitIndex == -1)
		{
			return null;
		}
		return this.AllPortraits[portraitIndex].PortraitSprite;
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x000572D8 File Offset: 0x000554D8
	public int GetPortraitIndex(string _ID)
	{
		if (string.IsNullOrEmpty(_ID))
		{
			return -1;
		}
		for (int i = 0; i < this.AllPortraits.Count; i++)
		{
			if (this.AllPortraits[i].PortraitID == _ID)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x00057324 File Offset: 0x00055524
	public int GetScore(CharacterPerk[] _AllPerks)
	{
		if (_AllPerks == null)
		{
			return 0;
		}
		if (_AllPerks.Length == 0)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < _AllPerks.Length; i++)
		{
			if (_AllPerks[i])
			{
				num += _AllPerks[i].DifficultyRating;
			}
		}
		return num;
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00057364 File Offset: 0x00055564
	public string GetRating(int _Score)
	{
		if (this.DifficultyRatings == null)
		{
			return "";
		}
		if (this.DifficultyRatings.Length == 0)
		{
			return "";
		}
		int num = 0;
		for (int i = 0; i < this.DifficultyRatings.Length; i++)
		{
			num = Mathf.Min(Mathf.Min(this.DifficultyRatings[i].Range.x, this.DifficultyRatings[i].Range.y), num);
			if ((this.DifficultyRatings[i].Range.x <= _Score && this.DifficultyRatings[i].Range.y > _Score) || (this.DifficultyRatings[i].Range.x == this.DifficultyRatings[i].Range.y && this.DifficultyRatings[i].Range.x == _Score))
			{
				return this.DifficultyRatings[i].Label;
			}
		}
		if (_Score < num)
		{
			return this.DifficultyRatings[0].Label;
		}
		return this.DifficultyRatings[this.DifficultyRatings.Length - 1].Label;
	}

	// Token: 0x04000EB7 RID: 3767
	[SerializeField]
	private CharacterPortraitRef[] PortraitSprites;

	// Token: 0x04000EB8 RID: 3768
	public DifficultyRatingLabel[] DifficultyRatings;

	// Token: 0x04000EB9 RID: 3769
	[NonSerialized]
	public List<CharacterPortraitRef> AllPortraits = new List<CharacterPortraitRef>();

	// Token: 0x04000EBA RID: 3770
	private string ImagesPath;

	// Token: 0x04000EBB RID: 3771
	private List<Texture2D> LoadedTextures = new List<Texture2D>();

	// Token: 0x04000EBC RID: 3772
	public const int MaxCustomPortraits = 10;

	// Token: 0x04000EBD RID: 3773
	public const int MaxPortraitSize = 1024;
}
