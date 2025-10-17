using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class Testing : MonoBehaviour
{
	// Token: 0x06000283 RID: 643 RVA: 0x00018D15 File Offset: 0x00016F15
	[ContextMenu("Test save")]
	public void TestJson()
	{
		Debug.Log(JsonUtility.ToJson(this));
		PlayerPrefs.SetString("TEST", JsonUtility.ToJson(this));
	}

	// Token: 0x06000284 RID: 644 RVA: 0x00018D34 File Offset: 0x00016F34
	[ContextMenu("Test Load")]
	public void TestLoad()
	{
		Testing testing = JsonUtility.FromJson<Testing>(PlayerPrefs.GetString("TEST"));
		Debug.Log(testing, testing);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00018D58 File Offset: 0x00016F58
	[ContextMenu("Test Replace")]
	public void TestReplace()
	{
		string text = "C:/Users/Windows/Documents/Project with Nico/SurvivalCards/TestReplace/Backup.json";
		string s = JsonUtility.ToJson(this.Data, true);
		string text2 = "C:/Users/Windows/Documents/Project with Nico/SurvivalCards/TestReplace/Main.json";
		string text3 = "D:/ProjectsWithNico/TestReplace/tempfile.json";
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		if (File.Exists(text2))
		{
			using (FileStream fileStream = File.Create(text3, 4096, FileOptions.WriteThrough))
			{
				fileStream.Write(bytes, 0, bytes.Length);
			}
			File.Replace(text3, text2, text);
			return;
		}
		using (FileStream fileStream2 = File.Create(text2, 4096, FileOptions.WriteThrough))
		{
			fileStream2.Write(bytes, 0, bytes.Length);
		}
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00018E28 File Offset: 0x00017028
	private void Awake()
	{
		this.SourceTest = base.GetComponent<RandomSoundPlay>();
	}

	// Token: 0x06000287 RID: 647 RVA: 0x00018E36 File Offset: 0x00017036
	private void Update()
	{
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00018E38 File Offset: 0x00017038
	[ContextMenu("Test References Bulk Cached")]
	private void TestReferencesBulkCached()
	{
		for (int i = 0; i < this.Cards.Length; i++)
		{
			this.CardRef = this.Cards[i];
			base.StartCoroutine(this.TestReferencesRoutine(this.CardRef, false));
		}
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00018E7C File Offset: 0x0001707C
	[ContextMenu("Test References Bulk Not Cached")]
	private void TestReferencesBulkNotCached()
	{
		for (int i = 0; i < this.Cards.Length; i++)
		{
			CardData @ref = this.Cards[i];
			base.StartCoroutine(this.TestReferencesRoutine(@ref, false));
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00018EB4 File Offset: 0x000170B4
	[ContextMenu("Test References One By One Cached")]
	private void TestReferencesOneByOneCached()
	{
		for (int i = 0; i < this.Cards.Length; i++)
		{
			this.DoSingleReferenceCached(i);
		}
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00018EDC File Offset: 0x000170DC
	[ContextMenu("Test References One By One Not Cached")]
	private void TestReferencesNotCached()
	{
		for (int i = 0; i < this.Cards.Length; i++)
		{
			this.DoSingleReferenceNotCached(i);
		}
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00018F03 File Offset: 0x00017103
	private void DoSingleReferenceCached(int _Index)
	{
		this.CardRef = this.Cards[_Index];
		base.StartCoroutine(this.TestReferencesRoutine(this.CardRef, false));
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00018F28 File Offset: 0x00017128
	private void DoSingleReferenceNotCached(int _Index)
	{
		CardData @ref = this.Cards[_Index];
		base.StartCoroutine(this.TestReferencesRoutine(@ref, false));
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00018F4D File Offset: 0x0001714D
	private IEnumerator TestReferencesRoutine(CardData _Ref, bool _Stop)
	{
		yield return null;
		if (_Stop)
		{
			yield return new WaitForSeconds(0.5f);
			Debug.Break();
		}
		yield break;
	}

	// Token: 0x040002CE RID: 718
	public CardsDropCollection[] TestDrops;

	// Token: 0x040002CF RID: 719
	public CardsDropCollection TestSingleDrop;

	// Token: 0x040002D0 RID: 720
	public BlueprintElement TestBPElement;

	// Token: 0x040002D1 RID: 721
	public Testing.TestClass Data;

	// Token: 0x040002D2 RID: 722
	public Testing.TestInheritance Inherited;

	// Token: 0x040002D3 RID: 723
	public float HelloYes = 3f;

	// Token: 0x040002D4 RID: 724
	public ExtraDurabilityChange TestDrawer;

	// Token: 0x040002D5 RID: 725
	[StatModifierOptions(false, false)]
	public StatModifier TestModDrawer;

	// Token: 0x040002D6 RID: 726
	public Testing.TestClass[] ArrayMultiEditing;

	// Token: 0x040002D7 RID: 727
	public AudioClip SoundTest;

	// Token: 0x040002D8 RID: 728
	private RandomSoundPlay SourceTest;

	// Token: 0x040002D9 RID: 729
	private const string TestKey = "TEST";

	// Token: 0x040002DA RID: 730
	public OptionalFloatValue TestInt;

	// Token: 0x040002DB RID: 731
	public CardData[] Cards;

	// Token: 0x040002DC RID: 732
	private CardData CardRef;

	// Token: 0x02000245 RID: 581
	[Serializable]
	public class TestClass
	{
		// Token: 0x040013E7 RID: 5095
		public CardData CardTest;

		// Token: 0x040013E8 RID: 5096
		public float Hello = 5f;

		// Token: 0x040013E9 RID: 5097
		public float[] Extra;
	}

	// Token: 0x02000246 RID: 582
	[Serializable]
	public class TestSerializationClass
	{
		// Token: 0x040013EA RID: 5098
		public CardData CardTest;

		// Token: 0x040013EB RID: 5099
		public float Hello;
	}

	// Token: 0x02000247 RID: 583
	[Serializable]
	public class TestInheritance : Testing.TestClass
	{
	}
}
