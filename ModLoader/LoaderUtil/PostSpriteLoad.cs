using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020JeremyAnsel.Media.Dds;
using UnityEngine;

namespace ModLoader.LoaderUtil
{
	// Token: 0x02000026 RID: 38
	public static class PostSpriteLoad
	{
		// Token: 0x06000098 RID: 152 RVA: 0x000095C4 File Offset: 0x000077C4
		public static void PostSetEnQueue(this object o, Action<object, object> setter, string id)
		{
			Queue<PostSpriteLoad.PostSetter> queue;
			if (PostSpriteLoad.PostSetQueue.TryGetValue(id, out queue))
			{
				queue.Enqueue(new PostSpriteLoad.PostSetter(setter, o, id));
				return;
			}
			PostSpriteLoad.PostSetQueue[id] = new Queue<PostSpriteLoad.PostSetter>(new PostSpriteLoad.PostSetter[]
			{
				new PostSpriteLoad.PostSetter(setter, o, id)
			});
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00009610 File Offset: 0x00007810
		public static void ToCompress(this Texture2D texture2D)
		{
			PostSpriteLoad.ShouldCompress.Enqueue(texture2D);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000961D File Offset: 0x0000781D
		public static IEnumerator CompressOnLate()
		{
			while (!PostSpriteLoad.BeginCompress)
			{
				yield return null;
			}
			while (PostSpriteLoad.SpriteLoadQueue.Count > 0 || !PostSpriteLoad.NoMoreSpriteLoadQueue)
			{
				Task<ValueTuple<List<ValueTuple<byte[], ImageEntry>>, string>> task = PostSpriteLoad.SpriteLoadQueue.Dequeue();
				while (!task.IsCompleted)
				{
					yield return null;
				}
				ValueTuple<List<ValueTuple<byte[], ImageEntry>>, string> result = task.Result;
				List<ValueTuple<byte[], ImageEntry>> item = result.Item1;
				string modName = result.Item2;
				DateTime loadStartTime = DateTime.Now;
				foreach (ValueTuple<byte[], ImageEntry> valueTuple in item)
				{
					byte[] dat = valueTuple.Item1;
					ImageEntry imageEntry = valueTuple.Item2;
					DateTime now = DateTime.Now;
					if ((now - loadStartTime).TotalMilliseconds > 25.0)
					{
						loadStartTime = now;
						yield return null;
					}
					Rect rect;
					Texture2D texture2D2;
					if (imageEntry.DdsPath != null)
					{
						DdsFile ddsFile = DdsFile.FromStream(new MemoryStream(dat));
						rect = new Rect(0f, 0f, (float)ddsFile.Width, (float)ddsFile.Height);
						Vector2Int vector2Int = new Vector2Int(((ddsFile.Width & 3) == 0) ? ddsFile.Width : ((ddsFile.Width >> 2) + 1 << 2), ((ddsFile.Height & 3) == 0) ? ddsFile.Height : ((ddsFile.Height >> 2) + 1 << 2));
						DdsFourCC fourCC = ddsFile.PixelFormat.FourCC;
						Texture2D texture2D;
						if (fourCC != DdsFourCC.DXT1)
						{
							if (fourCC != DdsFourCC.DXT5)
							{
								texture2D = new Texture2D(ddsFile.Width, ddsFile.Height, TextureFormat.RGBA32, 0, false);
							}
							else
							{
								texture2D = new Texture2D(vector2Int.x, vector2Int.y, TextureFormat.DXT5, 0, false);
							}
						}
						else
						{
							texture2D = new Texture2D(vector2Int.x, vector2Int.y, TextureFormat.DXT1, 0, false);
						}
						texture2D2 = texture2D;
						texture2D2.GetRawTextureData<byte>().CopyFrom(ddsFile.Data);
						texture2D2.Apply();
					}
					else
					{
						texture2D2 = new Texture2D(0, 0, TextureFormat.RGBA32, 0, false);
						texture2D2.LoadImage(dat);
						rect = new Rect(0f, 0f, (float)texture2D2.width, (float)texture2D2.height);
					}
					if (imageEntry.DdsPath == null && !ModLoader.TexCompatibilityMode.Value)
					{
						texture2D2.Compress(false);
					}
					Sprite sprite = Sprite.Create(texture2D2, rect, Vector2.zero);
					sprite.name = imageEntry.Name;
					if (!ModLoader.SpriteDict.ContainsKey(imageEntry.Name))
					{
						ModLoader.SpriteDict.Add(imageEntry.Name, sprite);
						Queue<PostSpriteLoad.PostSetter> queue;
						if (!PostSpriteLoad.PostSetQueue.TryGetValue(imageEntry.Name, out queue))
						{
							continue;
						}
						while (queue.Count > 0)
						{
							queue.Dequeue().Set();
						}
						PostSpriteLoad.PostSetQueue.Remove(imageEntry.Name);
					}
					else
					{
						Debug.LogWarningFormat("{0} SpriteDict Same Key was Add {1}", new object[]
						{
							modName,
							imageEntry.Name
						});
					}
					dat = null;
					imageEntry = null;
				}
				List<ValueTuple<byte[], ImageEntry>>.Enumerator enumerator = default(List<ValueTuple<byte[], ImageEntry>>.Enumerator);
				task = null;
				modName = null;
			}
			while (!PostSpriteLoad.CanEnd)
			{
				yield return null;
			}
			foreach (KeyValuePair<string, Queue<PostSpriteLoad.PostSetter>> keyValuePair in PostSpriteLoad.PostSetQueue)
			{
				string text;
				Queue<PostSpriteLoad.PostSetter> queue2;
				keyValuePair.Deconstruct(out text, out queue2);
				Queue<PostSpriteLoad.PostSetter> queue3 = queue2;
				while (queue3.Count > 0)
				{
					queue3.Dequeue().Set();
				}
			}
			foreach (CardGraphics cardGraphics in from graphics in Resources.FindObjectsOfTypeAll<CardGraphics>()
			where graphics && graphics.CardLogic
			select graphics)
			{
				try
				{
					cardGraphics.Setup(cardGraphics.CardLogic);
				}
				catch (Exception ex)
				{
					ModLoader.Instance.CommonLogger.LogError(ex);
				}
			}
			ModLoader.ShowLoadSuccess += 1.2f;
			while (PostSpriteLoad.ShouldCompress.Count > 0)
			{
				DateTime now2 = DateTime.Now;
				while ((DateTime.Now - now2).TotalMilliseconds < 25.0 && PostSpriteLoad.ShouldCompress.Count > 0)
				{
					for (int i = 0; i < 4; i++)
					{
						PostSpriteLoad.ShouldCompress.Dequeue().Compress(false);
					}
				}
				yield return null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400009E RID: 158
		public static readonly Dictionary<string, Queue<PostSpriteLoad.PostSetter>> PostSetQueue = new Dictionary<string, Queue<PostSpriteLoad.PostSetter>>();

		// Token: 0x0400009F RID: 159
		[TupleElementNames(new string[]
		{
			"sprites",
			"modName",
			"dat",
			"entry"
		})]
		public static readonly Queue<Task<ValueTuple<List<ValueTuple<byte[], ImageEntry>>, string>>> SpriteLoadQueue = new Queue<Task<ValueTuple<List<ValueTuple<byte[], ImageEntry>>, string>>>();

		// Token: 0x040000A0 RID: 160
		public static bool NoMoreSpriteLoadQueue;

		// Token: 0x040000A1 RID: 161
		public static bool CanEnd;

		// Token: 0x040000A2 RID: 162
		public static bool BeginCompress;

		// Token: 0x040000A3 RID: 163
		public static CoroutineController Controller;

		// Token: 0x040000A4 RID: 164
		public static readonly Queue<Texture2D> ShouldCompress = new Queue<Texture2D>();

		// Token: 0x02000027 RID: 39
		public class PostSetter
		{
			// Token: 0x0600009C RID: 156 RVA: 0x00009645 File Offset: 0x00007845
			public PostSetter(Action<object, object> setterFunc, object toSetObj, string spriteId)
			{
				this.SetterFunc = setterFunc;
				this.ToSetObj = toSetObj;
				this.SpriteId = spriteId;
			}

			// Token: 0x0600009D RID: 157 RVA: 0x00009664 File Offset: 0x00007864
			public void Set()
			{
				Sprite arg;
				if (ModLoader.SpriteDict.TryGetValue(this.SpriteId, out arg))
				{
					this.SetterFunc(this.ToSetObj, arg);
				}
			}

			// Token: 0x040000A5 RID: 165
			private readonly Action<object, object> SetterFunc;

			// Token: 0x040000A6 RID: 166
			private readonly object ToSetObj;

			// Token: 0x040000A7 RID: 167
			private readonly string SpriteId;
		}
	}
}
