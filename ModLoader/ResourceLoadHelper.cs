using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BepInEx;
using JetBrains.Annotations;
using ModLoader.FFI;
using ModLoader.LoaderUtil;
using UnityEngine;

namespace ModLoader
{
	// Token: 0x02000004 RID: 4
	public static class ResourceLoadHelper
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002188 File Offset: 0x00000388
		[return: TupleElementNames(new string[]
		{
			"sprites",
			"modName",
			"dat",
			"entry"
		})]
		public static Task<ValueTuple<List<ValueTuple<byte[], ImageEntry>>, string>> LoadPictures(string ModName, [NotNull] string[] pictures)
		{
			ResourceLoadHelper.<>c__DisplayClass2_0 CS$<>8__locals1 = new ResourceLoadHelper.<>c__DisplayClass2_0();
			CS$<>8__locals1.ModName = ModName;
			CS$<>8__locals1.wait = (from picture in pictures
			where picture.EndsWith(".jpg") || picture.EndsWith(".jpeg") || picture.EndsWith(".png")
			let imageEntry = new ImageEntry(picture)
			select new ValueTuple<Task<byte[]>, ImageEntry>(ResourceLoadHelper.LoadFile(imageEntry), imageEntry)).ToList<ValueTuple<Task<byte[]>, ImageEntry>>();
			return Task.Run<ValueTuple<List<ValueTuple<byte[], ImageEntry>>, string>>(delegate()
			{
				ResourceLoadHelper.<>c__DisplayClass2_0.<<LoadPictures>b__3>d <<LoadPictures>b__3>d;
				<<LoadPictures>b__3>d.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<List<ValueTuple<byte[], ImageEntry>>, string>>.Create();
				<<LoadPictures>b__3>d.<>4__this = CS$<>8__locals1;
				<<LoadPictures>b__3>d.<>1__state = -1;
				<<LoadPictures>b__3>d.<>t__builder.Start<ResourceLoadHelper.<>c__DisplayClass2_0.<<LoadPictures>b__3>d>(ref <<LoadPictures>b__3>d);
				return <<LoadPictures>b__3>d.<>t__builder.Task;
			});
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000222C File Offset: 0x0000042C
		[return: TupleElementNames(new string[]
		{
			"uniqueObjs",
			"modName",
			"dat",
			"pat",
			"type"
		})]
		public static Task<ValueTuple<List<ValueTuple<byte[], string, Type>>, string>> LoadUniqueObjs(string ModName, [NotNull] string dir, Assembly GameSourceAssembly, ModInfo Info)
		{
			ResourceLoadHelper.<>c__DisplayClass3_0 CS$<>8__locals1 = new ResourceLoadHelper.<>c__DisplayClass3_0();
			CS$<>8__locals1.ModName = ModName;
			string text = Path.Combine(dir, "JsonnetLib");
			if (Directory.Exists(text))
			{
				JsonnetRuntime.JsonnetRuntimeAddPat(text);
			}
			CS$<>8__locals1.wait = new List<ValueTuple<Task<byte[]>, string, Type>>();
			using (IEnumerator<Type> enumerator = (from type in GameSourceAssembly.GetTypes()
			where type.IsSubclassOf(typeof(UniqueIDScriptable))
			select type).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					if (Directory.Exists(ModLoader.CombinePaths(new string[]
					{
						dir,
						type.Name
					})))
					{
						if (Utility.IsNullOrWhiteSpace(Info.ModEditorVersion))
						{
							Debug.LogWarningFormat("{0} Only Support Editor Mod", new object[]
							{
								CS$<>8__locals1.ModName
							});
						}
						else
						{
							CS$<>8__locals1.wait.AddRange(from file in Directory.EnumerateFiles(Path.Combine(dir, type.Name), "*.json", SearchOption.AllDirectories)
							select new ValueTuple<Task<byte[]>, string, Type>(ResourceLoadHelper.LoadFile(file), file, type));
							CS$<>8__locals1.wait.AddRange(from file in Directory.EnumerateFiles(Path.Combine(dir, type.Name), "*.jsonnet", SearchOption.AllDirectories)
							select new ValueTuple<Task<byte[]>, string, Type>(ResourceLoadHelper.LoadFile(file), file, type));
						}
					}
				}
			}
			return Task.Run<ValueTuple<List<ValueTuple<byte[], string, Type>>, string>>(delegate()
			{
				ResourceLoadHelper.<>c__DisplayClass3_0.<<LoadUniqueObjs>b__1>d <<LoadUniqueObjs>b__1>d;
				<<LoadUniqueObjs>b__1>d.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<List<ValueTuple<byte[], string, Type>>, string>>.Create();
				<<LoadUniqueObjs>b__1>d.<>4__this = CS$<>8__locals1;
				<<LoadUniqueObjs>b__1>d.<>1__state = -1;
				<<LoadUniqueObjs>b__1>d.<>t__builder.Start<ResourceLoadHelper.<>c__DisplayClass3_0.<<LoadUniqueObjs>b__1>d>(ref <<LoadUniqueObjs>b__1>d);
				return <<LoadUniqueObjs>b__1>d.<>t__builder.Task;
			});
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023AC File Offset: 0x000005AC
		private static Task<byte[]> LoadFile(string pat)
		{
			ResourceLoadHelper.<>c__DisplayClass4_0 CS$<>8__locals1 = new ResourceLoadHelper.<>c__DisplayClass4_0();
			CS$<>8__locals1.pat = pat;
			return Task.Run<byte[]>(new Func<Task<byte[]>>(CS$<>8__locals1.<LoadFile>g__loadInner|0));
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023CA File Offset: 0x000005CA
		private static Task<byte[]> LoadFile(ImageEntry entry)
		{
			ResourceLoadHelper.<>c__DisplayClass5_0 CS$<>8__locals1 = new ResourceLoadHelper.<>c__DisplayClass5_0();
			CS$<>8__locals1.entry = entry;
			return Task.Run<byte[]>(new Func<Task<byte[]>>(CS$<>8__locals1.<LoadFile>g__loadInner|0));
		}

		// Token: 0x04000003 RID: 3
		public static readonly string ResourcePat = "Resource";

		// Token: 0x04000004 RID: 4
		public static readonly string PicturePat = "Picture";
	}
}
